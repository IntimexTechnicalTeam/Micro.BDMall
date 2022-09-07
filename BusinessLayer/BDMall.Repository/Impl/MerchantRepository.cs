namespace BDMall.Repository
{
    public class MerchantRepository : PublicBaseRepository, IMerchantRepository
    {
        public MerchantRepository(IServiceProvider service) : base(service)
        {
        }

        public PageData<MerchantView> SearchMerchByCond(MerchantPageInfo condition)
        {
            PageData<MerchantView> data = new PageData<MerchantView>();
            StringBuilder sb = new StringBuilder();

            var baseQuery = GenBaseQuery(condition);               
            data.TotalRecord = GetMerchantCount(baseQuery);

            sb.AppendLine("select Id ,ClientId  ,MerchNo  ,Name  ,NameTransId  ,Contact  ,ContactTransId  ,ContactPhoneNum  ,FaxNum ");
            sb.AppendLine(",ContactAddress  ,ContactAddrTransId  ,ContactEmail  ,OrderEmail  ,Remarks  ,RemarksTransId  ,IsActive  ");
            sb.AppendLine(" ,MerchantType  ,IsExternal  ,GCP  ,CommissionRate  ,Lang  ,UpdateDate   from(");
            if (!condition.SortName.IsEmpty())
            {               
                sb.AppendLine($"select ROW_NUMBER() OVER(order by {condition.SortName} {condition.SortOrder}) as rowNum");
            }
            else
            {
                sb.AppendLine("select ROW_NUMBER() OVER(order by MerchNo) as rowNum");
            }
            sb.AppendLine(" ,*from(");

            sb.AppendLine($"{ baseQuery.strSql }");

            sb.AppendLine(") a");
            sb.AppendLine(")b where rowNum between @StartIndex and @EndIndex");

            List<SqlParameter> paramList = new List<SqlParameter>();

            var fromIndex = ((condition.Page - 1) * condition.PageSize) + 1;
            var toIndex = condition.Page * condition.PageSize;

            foreach (var item in baseQuery.ParamList)
            {
                SqlParameter p = (SqlParameter)item;
                paramList.Add(new SqlParameter { ParameterName = p.ParameterName, Value = p.Value });
            }

            paramList.Add(new SqlParameter("@StartIndex", fromIndex));
            paramList.Add(new SqlParameter("@EndIndex", toIndex));
            var result = baseRepository.SqlQuery<MerchantView>(sb.ToString(), paramList.ToArray());
            
            data.Data = result;
            return data;
        }




















        /// <summary>
        /// 读取总数
        /// </summary>
        /// <param name="baseQuery"></param>
        /// <returns></returns>
        private int GetMerchantCount(QueryParam baseQuery)
        {
            StringBuilder sb = new StringBuilder();
            List<SqlParameter> paramList = new List<SqlParameter>();         
            
            sb.AppendLine($"select count(1) from ({ baseQuery.strSql } ) as b");
            var result = baseRepository.IntFromSql(sb.ToString(), baseQuery.ParamList.ToArray());

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private QueryParam GenBaseQuery(MerchantPageInfo condition)
        {
            StringBuilder sb = new StringBuilder();
            var cond = condition.Condition;
            List<SqlParameter> paramList = new List<SqlParameter>();

            sb.AppendLine("select distinct m.Id, m.ClientId, m.MerchNo, m.ContactPhoneNum, m.FaxNum, m.ContactEmail, m.OrderEmail, m.IsActive, m.IsDeleted, m.CreateDate");
            sb.AppendLine(", m.UpdateDate, m.CreateBy, m.UpdateBy, m.NameTransId, m.ContactTransId, m.ContactAddrTransId, m.RemarksTransId, m.IsExternal, m.MerchantType");
            sb.AppendLine(", m.GCP, m.[Language] as Lang, m.[CommissionRate], isnull(t.Value, '') as Name, isnull(dt.Value, '') as ContactAddress, isnull(ct.Value, '') as Contact, isnull(rt.Value, '') as Remarks");

            sb.AppendLine("from merchants m");
            sb.AppendLine("left join Translations t on m.NameTransId = t.TransId and t.Lang = @lang ");
            sb.AppendLine("left join Translations dt on m.ContactAddrTransId = dt.TransId and dt.Lang = @lang");
            sb.AppendLine("left join Translations ct on m.ContactTransId = ct.TransId and ct.Lang = @lang");
            sb.AppendLine("left join Translations rt on m.RemarksTransId = rt.TransId and rt.Lang = @lang");

            if (!cond.Name.IsEmpty())
            {
                sb.AppendLine(" inner join(");
                sb.AppendLine(" select tm.NameTransId from Merchants tm");
                sb.AppendLine(" left join Translations tt on tm.NameTransId = tt.TransId ");
                sb.AppendFormat(" where tt.[Value] like '%{0}%'", cond.Name.Trim());
                sb.AppendLine(" ) e on e.NameTransId = m.NameTransId");
            }

            sb.AppendLine("inner join(");
            sb.AppendLine("select distinct gm.Id, gp.CreateDate from Merchants gm");
            sb.AppendLine("left join MerchantPromotions gp on gp.MerchantId = gm.Id and gp.IsActive = 1 and gp.IsDeleted = 0");
            sb.AppendLine("where gm.IsDeleted = 0");


            if (cond.ApproveStatus != -1)
            {
                if (cond.ApproveStatus == 3)
                {
                    sb.AppendLine("and(ApproveStatus = 3 or ApproveStatus is null)");
                }
                else
                {
                    sb.AppendFormat("and ApproveStatus = @Status");
                    paramList.Add(new SqlParameter("@Status", cond.ApproveStatus));
                }
            }
            else
            {
                sb.AppendLine(" or ApproveStatus is null");
            }

            sb.AppendLine(") g on g.Id = m.Id");

            sb.AppendLine("left join(");
            sb.AppendLine("select MerchantId,LastEditDate=Max(CreateDate) from MerchantPromotions");
            sb.AppendLine("group by MerchantId");
            sb.AppendLine(")md on md.MerchantId=m.Id");
            sb.AppendLine("left join Users u on u.MerchantId=m.Id");

            sb.AppendLine("where  m.IsDeleted = 0");

            sb.AppendLine("and (md.LastEditDate is null or (md.LastEditDate is not null and md.LastEditDate=g.CreateDate))");

            if (cond.IsActiveCond != null)
            {
                var a = cond.IsActiveCond.Value ? 1 : 0;
                sb.AppendLine("and m.IsActive = @IsActive");
                paramList.Add(new SqlParameter("@IsActive", a));
            }
            if (!string.IsNullOrEmpty(cond.MerchNo))
            {
                sb.AppendLine("and m.MerchNo like @MerchNo");
                paramList.Add(new SqlParameter("@MerchNo", "%" + cond.MerchNo.Trim() + "%"));
            }

            if (cond.IsAccountCreated != null)
            {
                if (cond.IsAccountCreated.Value)
                {
                    sb.AppendLine("and u.Id is not null");
                }
                else
                {
                    sb.AppendLine("and u.Id is null");
                }
            }

            if (!string.IsNullOrEmpty(cond.ContactEmail))
            {
                sb.AppendLine("and m.ContactEmail like @ContactEmail");
                paramList.Add(new SqlParameter("@ContactEmail", "%" + cond.ContactEmail.Trim() + "%"));
            }

            if (!string.IsNullOrEmpty(cond.OrderEmail))
            {
                sb.AppendLine("and m.OrderEmail like @OrderEmail");
                paramList.Add(new SqlParameter("@OrderEmail", "%" + cond.OrderEmail.Trim() + "%"));
            }

            if (!string.IsNullOrEmpty(cond.AccountCreateDateB))
            {
                DateTime dateB;
                if (DateTime.TryParse(cond.AccountCreateDateB, out dateB))
                {
                    sb.AppendLine("and u.CreateDate>='" + dateB.Date.ToString("yyyy-MM-dd 00:00") + "'");
                }
            }

            if (!string.IsNullOrEmpty(cond.AccountCreateDateE))
            {
                DateTime dateE;
                if (DateTime.TryParse(cond.AccountCreateDateE, out dateE))
                {
                    sb.AppendLine("and u.CreateDate<'" + dateE.AddDays(1).Date.ToString("yyyy-MM-dd 00:00") + "'");
                }
            }

            paramList.Add(new SqlParameter("@lang", CurrentUser.Lang.ToInt()));
            var result = new QueryParam { strSql = sb, ParamList = paramList.ToArray() };
            return result;     
        }
    }
}
