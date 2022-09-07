namespace BDMall.Repository
{
    public class OrderDeliveryRepository : PublicBaseRepository, IOrderDeliveryRepository
    {
        public OrderDeliveryRepository(IServiceProvider service) : base(service)
        {
        }
     
        /// <summary>
        /// 更新OrderDelivery状态
        /// </summary>
        /// <param name="delivery"></param>
        public void UpdateOrderDeliveryStatus(OrderDelivery delivery)
        {

            string sql = @"
                             update OrderDeliveries set Status =@status,LocationId=@loc,TrackingNo=@trackingNo
                            ,updateBy=@updateBy,UpdateDate=getDate()
                            where id=@id;
                            ";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@id", delivery.Id));
            paramList.Add(new SqlParameter("@status", delivery.Status));
            paramList.Add(new SqlParameter("@loc", delivery.LocationId));
            paramList.Add(new SqlParameter("@trackingNo", delivery.TrackingNo));
            paramList.Add(new SqlParameter("@updateBy", Guid.Parse(CurrentUser.UserId)));

            baseRepository.ExecuteSqlCommand(sql, paramList.ToArray());
        }

        public void UpdateOrderDeliveryDetail(OrderDeliveryDetailDto deliveryDetail)
        {
            string sql = @" update OrderDeliveryDetails set LocationId=@loc,TrackingNo=@trackingNo,updateBy=@updateBy,UpdateDate=getDate() where id=@id; ";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@id", deliveryDetail.Id));
            
            paramList.Add(new SqlParameter("@loc", deliveryDetail.LocationId));
            paramList.Add(new SqlParameter("@trackingNo", deliveryDetail.TrackingNo));
            paramList.Add(new SqlParameter("@updateBy", Guid.Parse(CurrentUser.UserId)));

            baseRepository.ExecuteSqlCommand(sql, paramList.ToArray());
        }

        public void UpdateOrderDeliveryTrackingNo(OrderDelivery delivery)
        {

            string sql = @"
                             update OrderDeliveries set TrackingNo=@trackingNo
                            ,updateBy=@updateBy,UpdateDate=getDate()
                            where id=@id;
                            ";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@id", delivery.Id));
            paramList.Add(new SqlParameter("@trackingNo", delivery.TrackingNo));
            paramList.Add(new SqlParameter("@updateBy", Guid.Parse(CurrentUser.UserId)));

            baseRepository.ExecuteSqlCommand(sql, paramList.ToArray());
        }

        public void UpdateDeliverySendBackCount(OrderDelivery delivery)
        {
            string sql = @"
                             update OrderDeliveries set 
                            SendBackCount=@SendBackCount,
                            updateBy=@updateBy,UpdateDate=getDate()
                            where id=@id;
                            ";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@id", delivery.Id));
            paramList.Add(new SqlParameter("@SendBackCount", delivery.SendBackCount));
            paramList.Add(new SqlParameter("@updateBy", Guid.Parse(CurrentUser.UserId)));

            baseRepository.ExecuteSqlCommand(sql, paramList.ToArray());
        }

        public List<OrderDelivery> Search(Guid orderId, Guid merchantId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@oid", orderId));

            string sql = @" OPEN SYMMETRIC KEY AES256key_Do1Mall DECRYPTION BY CERTIFICATE [CERTDO1MAll]; 
                     select  CONVERT(nvarchar(1000),DecryptByKey(CONVERT(varbinary(1000), Recipients))) as  Recipients, 
					    CONVERT(nvarchar(1000),DecryptByKey(CONVERT(varbinary(1000), ContactPhone))) as ContactPhone,	
                        CONVERT(nvarchar(1000),DecryptByKey(CONVERT(varbinary(1000), Address))) as Address, 
						CONVERT(nvarchar(1000),DecryptByKey(CONVERT(varbinary(1000), Address1))) as Address1,
						CONVERT(nvarchar(1000),DecryptByKey(CONVERT(varbinary(1000), Address2))) as Address2,
						CONVERT(nvarchar(1000),DecryptByKey(CONVERT(varbinary(1000), Address3))) as Address3,
				         id,orderId,ClientId,IsActive,IsDeleted,CreateDate,CreateBy,UpdateDate,UpdateBy,City,PostalCode,Email,Seq,MerchantId,
                Amount,Freight,FreightDiscount,ExpressCompanyId,TotalPrice,DeliveryType,ServiceType,Country,Province,City,
                PLType,PLCode,ItemQty,DeliveryNO,DiscountFreight,BackupPhone,[Status],COCode,LocationId,TrackingNo,CountryCode,MailType,ECShipNo,ECShipDocNo,DiscountAmount,DiscountPrice,MCNCode,SendBackCount,ActualAmount,FreeShippingFreight,Remark,CoolDownDay
					    from OrderDeliveries   where orderId=  @oid ";
            if (merchantId != Guid.Empty)
            {
                sql += " and merchantId=@mid";
                sqlParameters.Add(new SqlParameter("@mid", merchantId));
            }
            sql += " order by DeliveryNO ;CLOSE SYMMETRIC KEY AES256key_Do1Mall;";

            var result = baseRepository.SqlQuery<OrderDelivery>(sql, sqlParameters.ToArray()).ToList();
            return result;

        }

        public void UpdateDeliveryInfo(OrderDeliveryInfo delivery)
        {
            string sql = @" OPEN SYMMETRIC KEY AES256key_Do1Mall DECRYPTION BY CERTIFICATE [CERTDO1MAll]; 
                             update OrderDeliveries set 
                             City=@City,
                              Address=CONVERT(nvarchar(200),  EncryptByKey(Key_GUID('AES256key_Do1Mall'),@Address)),
							  Address1=CONVERT(nvarchar(200),  EncryptByKey(Key_GUID('AES256key_Do1Mall'),@Address1)),
							  Address2=CONVERT(nvarchar(200),  EncryptByKey(Key_GUID('AES256key_Do1Mall'),@Address2)),
							  Address3=CONVERT(nvarchar(200),  EncryptByKey(Key_GUID('AES256key_Do1Mall'),@Address3)),
                              Recipients=CONVERT(nvarchar(200),  EncryptByKey(Key_GUID('AES256key_Do1Mall'),@Name)),
                              ContactPhone=CONVERT(nvarchar(200),  EncryptByKey(Key_GUID('AES256key_Do1Mall'),@Phone)),
                              Email=@Email,
                            updateBy=@updateBy,UpdateDate=getDate(),PostalCode=@PostalCode,Remark=@Remark
                            where id=@id;
                            CLOSE SYMMETRIC KEY  AES256key_Do1Mall;
                            ";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@id", delivery.Id));
            paramList.Add(new SqlParameter("@City", delivery.City ?? ""));
            paramList.Add(new SqlParameter("@Name", delivery.ConactName ?? ""));
            paramList.Add(new SqlParameter("@Phone", delivery.ConactPhone ?? ""));
            paramList.Add(new SqlParameter("@Address", delivery.Address ?? ""));
            paramList.Add(new SqlParameter("@Address1", delivery.Address1 ?? ""));
            paramList.Add(new SqlParameter("@Address2", delivery.Address2 ?? ""));
            paramList.Add(new SqlParameter("@Address3", delivery.Address3 ?? ""));
            paramList.Add(new SqlParameter("@updateBy", CurrentUser.UserId));
            paramList.Add(new SqlParameter("@PostalCode", delivery.PostCode ?? ""));
            paramList.Add(new SqlParameter("@Email", delivery.ConactMail ?? ""));
            paramList.Add(new SqlParameter("@Remark", delivery.Remark ?? ""));

            baseRepository.ExecuteSqlCommand(sql, paramList.ToArray());
        }
    }
}
