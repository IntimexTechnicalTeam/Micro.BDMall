namespace BDMall.Domain
{
    public class QueryParam
    {
        public StringBuilder strSql { get; set; } = new StringBuilder();
        
        public object[] ParamList { get; set; } = new object[0];
    }
}
