using System.Xml.Serialization;

namespace PP3.Api.Models
{
    [XmlRoot("Result")]
    public class ResultDto
    {
        public string Ori { get; set; } = string.Empty;
        public string New { get; set; } = string.Empty;
    }
}
