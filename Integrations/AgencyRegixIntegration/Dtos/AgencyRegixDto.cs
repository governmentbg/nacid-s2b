using System.Xml.Serialization;

namespace Integrations.AgencyRegixIntegration.Dtos
{
    [XmlRoot(ElementName = "ActualStateResponseV2", Namespace = "http://egov.bg/RegiX/AV/TR/ActualStateResponseV2")]
    [Serializable]
    public class AgencyRegixDto
    {
        [XmlElement(ElementName = "Deed")]
        public AgencyDataRegixDto Deed { get; set; }
    }

    [Serializable]
    public class AgencyDataRegixDto
    {
        [XmlElement(ElementName = "UIC")]
        public string UIC { get; set; }

        [XmlElement(ElementName = "CompanyName")]
        public string CompanyName { get; set; }

        [XmlArray(ElementName = "Records")]
        [XmlArrayItem(ElementName = "Record")]
        public List<AgencyRecordRegixDto> Records { get; set; } = new List<AgencyRecordRegixDto>();
    }

    [Serializable]
    public class AgencyRecordRegixDto
    {
        [XmlElement(ElementName = "RecordId")]
        public int RecordId { get; set; }

        [XmlElement(ElementName = "RecordData")]
        public AgencyRecordDataRegixDto RecordData { get; set; }
    }

    [Serializable]
    public class AgencyRecordDataRegixDto
    {
        [XmlElement(ElementName = "LegalForm")]
        public AgencyLegalFormRegixDto LegalForm { get; set; }

        [XmlElement(ElementName = "Seat")]
        public AgencySeatRegixDto Seat { get; set; }
    }

    [Serializable]
    public class AgencyLegalFormRegixDto
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }
    }

    [Serializable]
    public class AgencySeatRegixDto
    {
        [XmlElement(ElementName = "Address")]
        public AgencyAddressRegixDto Address { get; set; }
    }

    [Serializable]
    public class AgencyAddressRegixDto
    {
        [XmlElement(ElementName = "Area")]
        public string Area { get; set; }

        [XmlElement(ElementName = "Street")]
        public string Street { get; set; }

        [XmlElement(ElementName = "StreetNumber")]
        public string StreetNumber { get; set; }

        [XmlElement(ElementName = "Block")]
        public string Block { get; set; }

        [XmlElement(ElementName = "Entrance")]
        public string Entrance { get; set; }

        [XmlElement(ElementName = "Floor")]
        public string Floor { get; set; }

        [XmlElement(ElementName = "Apartment")]
        public string Apartment { get; set; }

        [XmlElement(ElementName = "SettlementEKATTE")]
        public string SettlementEKATTE { get; set; }
    }
}
