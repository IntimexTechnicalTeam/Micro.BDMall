namespace BDMall.Domain
{
    public class RoleDto:BaseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

    
        public string Remark { get; set; }

        public List<MutiLanguage> FullNames { get; set; } = new List<MutiLanguage>();   


        public List<MutiLanguage> Remarks { get; set; } = new List<MutiLanguage>();

        public Guid FullNameTransId { get; set; }

        public Guid RemarkTransId { get; set; }

        public bool IsSystem { get; set; }

        public List<PermissionDto> PermissionList { get; set; } = new List<PermissionDto>();

        public List<RolePermissionDto> RolePermission { get; set; } = new List<RolePermissionDto>();
    }
}
