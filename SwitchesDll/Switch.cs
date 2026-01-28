using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SwitchesDll
{
    public class Switch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MacAdress { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }

        public int PortNumber { get; set; }
        public int SwitchDBId { get; set; }
    }
}
