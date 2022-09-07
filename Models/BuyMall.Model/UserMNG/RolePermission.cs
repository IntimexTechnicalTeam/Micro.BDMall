namespace BDMall.Model
{
    [Table("RolePermissions")]
    public class RolePermission : BaseEntity<Guid>
    {

       
        public Guid PermissionId { get; set; }

        public Guid RoleId { get; set; }


        //[ForeignKey("RoleId")]
        //public virtual Role Role { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}
