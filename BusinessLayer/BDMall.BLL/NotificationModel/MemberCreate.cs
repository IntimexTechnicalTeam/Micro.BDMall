namespace BDMall.BLL
{
    public class MemberCreate<T> : INotification
    {
        public string Id;

        public T Param { get; set; } = default(T);
    }
}
