using BDMall.Domain;
using BDMall.Model;
using BDMall.Repository;
using Intimex.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public class ScheduleJobRepository : PublicBaseRepository, IScheduleJobRepository
    {

        public ScheduleJobRepository(IServiceProvider service) : base(service)
        {

        }

     
    }
}
