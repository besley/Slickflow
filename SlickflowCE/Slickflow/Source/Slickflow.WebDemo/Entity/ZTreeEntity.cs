
namespace Slickflow.WebDemo.Entity
{
    public class ZTreeEntity
    {
        public ZTreeEntity() { }

        public ZTreeEntity(string id, string pId, string name, string icon, string url, bool nocheck)
        {
            this.id = id;
            this.pId = pId;
            this.name = name;
            this.icon = icon;
            this.url = url;
            this.nocheck = nocheck;
        }
        public string id { get; set; }
        public string pId { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public string url { get; set; }

        /// <summary>
        /// 节点是否显示checkbox控件 true-不显示 false-显示
        /// </summary>
        public bool nocheck { get; set; }

    }
}