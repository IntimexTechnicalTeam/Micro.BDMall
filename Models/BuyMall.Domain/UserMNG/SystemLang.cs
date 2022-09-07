namespace BDMall.Domain
{
    public class SystemLang
    {
        public SystemLang()
        {

        }
        public SystemLang(string text, string code)
        {
            Text = text;
            Code = code;
        }
        public string Text { get; set; }
        public string Code { get; set; }

        public int Id { get; set; }
      
        public static SystemLang DefaultLang
        {
            get
            {
                SystemLang E = new SystemLang("English", "E");
                return E;
            }
        }
    }


}
