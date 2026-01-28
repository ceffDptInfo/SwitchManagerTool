using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwitchesDll
{
    public class Equipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PortId { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string MacAddress { get; set; } = "";
        public string IpV4 { get; set; } = "";

    }
}
