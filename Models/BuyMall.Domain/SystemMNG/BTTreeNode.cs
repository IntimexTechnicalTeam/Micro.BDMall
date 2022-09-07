using Newtonsoft.Json;

namespace BDMall.Domain
{
    /// <summary>
    /// Bootstrap Tree 节点对象
    /// </summary>
    public class BTTreeNode
    {
        [JsonProperty(PropertyName = "state")]
        public state State { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }


        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "nodes")]
        public List<BTTreeNode> Nodes { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }

        [JsonProperty(PropertyName = "checkable")]
        public bool Checkable { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public List<string> Tags { get; set; }
    }
    public class state
    {
        [JsonProperty(PropertyName = "checked")]
        public bool Checked { get; set; }
        [JsonProperty(PropertyName = "disabled")]
        public bool Disabled { get; set; }
        [JsonProperty(PropertyName = "expanded")]
        public bool Expanded { get; set; }
        [JsonProperty(PropertyName = "selected")]
        public bool Selected { get; set; }



    }
}
