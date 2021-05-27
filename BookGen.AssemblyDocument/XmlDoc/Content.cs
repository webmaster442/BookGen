using System.Xml.Serialization;
using System;

namespace BookGen.AssemblyDocument.XmlDoc
{
    [XmlInclude(typeof(Typeparam))]
    [XmlInclude(typeof(Param))]
    [XmlInclude(typeof(Exception))]
    [XmlInclude(typeof(Example))]
    [XmlInclude(typeof(Value))]
    [XmlInclude(typeof(Returns))]
    [XmlInclude(typeof(Remarks))]
    [XmlInclude(typeof(Para))]
    [XmlInclude(typeof(Summary))]
    [Serializable]
    public abstract class Content
    {
        [XmlElement("c", typeof(C))]
        [XmlElement("code", typeof(Code))]
        [XmlElement("list", typeof(List))]
        [XmlElement("paramref", typeof(Paramref))]
        [XmlElement("see", typeof(See))]
        [XmlElement("typeparamref", typeof(Typeparamref))]
        public object[] Items { get; set; }

        [XmlText]
        public string[] Text { get; set; }

        [XmlIgnore]
        public string JoinedText
        {
            get => string.Join('\n', Text);
        }


        public Content()
        {
            Items = Array.Empty<object>();
            Text = Array.Empty<string>();
        }
    }
}