namespace Web.Framework
{
    public class SystemResult
    {
        //[JsonProperty(PropertyName = "Succeeded")]
        public bool Succeeded { get; set; } = false;

        //[JsonProperty(PropertyName = "Message")]
        public string Message { get; set; }

       // [JsonProperty(PropertyName = "ReturnValue")]
        public object ReturnValue { get; set; }

    }

    [Serializable]
    [DataContract]
    public class SystemResult<T> : SystemResult
    {

        [DataMember(Name = "ReturnValue")]
        public virtual T ReturnValue { get; set; }
 
    }

}
