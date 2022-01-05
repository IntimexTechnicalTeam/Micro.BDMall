using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class RnpSubmitDataInfoView
    {
        public Guid Id { get; set; }
        public Guid SubmitId { get; set; }
        public Guid QuestionId { get; set; }
        public string Display { get; set; }
        public List<RnpSubmitDataGroupView> SubmitDataGroup { get; set; }
    }
}
