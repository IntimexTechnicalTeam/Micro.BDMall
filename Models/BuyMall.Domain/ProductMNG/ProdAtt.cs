namespace BDMall.Domain
{
    public class ProdAtt
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<ProdAttValue> Values { get; set; } = new List<ProdAttValue>();

        //設置前端界面屬性的佈局，0--用下拉框顯示、1--用平鋪顯示
        public AttrLayout Layout { get; set; }

        public int Seq { get; set; }

    }
}
