namespace BDMall.Domain
{
    public class FavoriteCond : PageInfo
    {
        
        //public FavoriteType FavoriteType { get; set; }
    }

    public enum FavoriteType
    { 
        /// <summary>
        /// 收藏商品
        /// </summary>
        Product=1,
        /// <summary>
        /// 收藏商家
        /// </summary>
        Merchant
    }
}
