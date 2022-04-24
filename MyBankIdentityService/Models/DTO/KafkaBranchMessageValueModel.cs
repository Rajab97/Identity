using System.Runtime.Serialization;

namespace MyBankIdentityService.Models.DTO
{
    [DataContract()]
    public class KafkaBranchMessageValueModel
    {
        [DataMember(Name = "Code")]
        public string Code { get; set; }

        [DataMember(Name = "NameEng")]
        public string NameEng { get; set; }

        [DataMember(Name = "ManagerHrCode")]
        public string ManagerHrCode { get; set; }

        [DataMember(Name = "Disable")]
        public bool Disable { get; set; }
    }
}
