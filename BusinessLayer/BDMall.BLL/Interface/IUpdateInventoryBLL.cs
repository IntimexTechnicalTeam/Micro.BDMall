namespace BDMall.BLL
{
    public interface IUpdateInventoryBLL:IDependency
    {
        /// <summary>
        /// 更新Inverntory表
        /// </summary>
        /// <param name="insertLst"></param>
        /// <param name="transIOTyp"></param>
        /// <param name="transType"></param>
        /// <returns></returns>
        SystemResult DealProductInventory(List<InvTransactionDtlDto> insertLst, InvTransIOType? transIOTyp, InvTransType transType);
    }
}
