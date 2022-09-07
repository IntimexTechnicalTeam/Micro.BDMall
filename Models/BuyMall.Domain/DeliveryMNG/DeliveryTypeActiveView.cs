namespace BDMall.Domain
{
    public class DeliveryTypeActiveView
    {
        public DeliveryTypeActiveView()
        {
            this.D = false;
            this.P = false;
            this.Z = false;
        }
        /// <summary>
        /// 邮递是否可用
        /// </summary>
        public bool D { get; set; }
        /// <summary>
        /// 柜台是否可用
        /// </summary>
        public bool P { get; set; }
        /// <summary>
        /// 智邮站是否可用
        /// </summary>
        public bool Z { get; set; }
    }
}
