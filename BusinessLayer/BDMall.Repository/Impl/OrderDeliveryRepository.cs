using BDMall.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Repository
{
    public class OrderDeliveryRepository : PublicBaseRepository, IOrderDeliveryRepository
    {
        public OrderDeliveryRepository(IServiceProvider service) : base(service)
        {
        }

        public void UpdateOrderDeliveryStatus(OrderDelivery delivery)
        {

            string sql = @"
                             update OrderDeliveries set Status =@status
                            ,updateBy=@updateBy,UpdateDate=getDate()
                            where id=@id;
                            ";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@id", delivery.Id));
            paramList.Add(new SqlParameter("@status", delivery.Status));
            paramList.Add(new SqlParameter("@updateBy", Guid.Parse(CurrentUser.UserId)));

            baseRepository.ExecuteSqlCommand(sql, paramList.ToArray());
        }

        public void UpdateOrderDeliveryArrangedStatus(OrderDelivery delivery)
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
    }
}
