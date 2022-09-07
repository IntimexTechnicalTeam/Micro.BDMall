namespace BDMall.Domain
{
    public class RolePermissionDto:BaseDto
    {
        public Guid PermissionId { get; set; } 

        public Guid RoleId { get; set; }

    }
}
