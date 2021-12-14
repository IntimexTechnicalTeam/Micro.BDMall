using BDMall.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Repository
{
    public class ProductRepository : PublicBaseRepository ,IProductRepository
    {
        public IBaseRepository baseRepository;

        public ProductRepository(IServiceProvider service) : base(service)
        {
           
        }

        public Task UpdateProduct()
        {
            baseRepository = this.Services.Resolve<IBaseRepository>();

            var unitwork = this.UnitOfWork;
            var dbContext = this.DbContext;

            var testUser = CurrentUser;

            return Task.CompletedTask;
        }
    }
}
