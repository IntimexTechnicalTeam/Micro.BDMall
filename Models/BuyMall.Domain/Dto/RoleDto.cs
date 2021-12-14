using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class RoleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

    
        public string Remark { get; set; }

        public List<MutiLanguage> FullNames { get; set; }


        public List<MutiLanguage> Remarks { get; set; }

        public Guid FullNameTransId { get; set; }

        public Guid RemarkTransId { get; set; }

        public bool IsSystem { get; set; }

        public List<PermissionDto> PermissionList { get; set; }
    }
}
