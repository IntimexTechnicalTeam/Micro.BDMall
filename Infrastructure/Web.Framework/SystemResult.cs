namespace Web.Framework
{
    [Serializable]
    [DataContract]
    public class SystemResult
    {
        [DataMember]
        public bool Succeeded { get; set; } = false;

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public object ReturnValue { get; set; }

    }

    /// <summary>
    /// 强烈建议使用泛型对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [DataContract]
    public class SystemResult<T> : SystemResult
    {

        [DataMember(Name = "ReturnValue")]
        public new virtual T ReturnValue { get; set; } = default(T);
 
    }

}
