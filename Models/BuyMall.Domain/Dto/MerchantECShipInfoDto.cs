namespace BDMall.Domain
{
    public class MerchantECShipInfoDto
    {
        public Guid Id { get; set; }

        public string SPName { get; set; }

        public string SPPassword { get; set; }

        public string SPIntegraterName { get; set; }


        public string ECShipName { get; set; }

        public string ECShipPassword { get; set; }

        public string ECShipIntegraterName { get; set; }

        public string ECShipEmail { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDate { get; set; }

      
        public DateTime? UpdateDate { get; set; }

        public Guid CreateBy { get; set; }

        public Guid? UpdateBy { get; set; }
    }
}
