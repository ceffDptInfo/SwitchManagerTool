using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SwitchesDll
{
    public class Port
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PortNumber { get; set; }

        public int SwitchId { get; set; }
        public Switch? Switch { get; set; }

        public List<Switch> ConnectedSwitches { get; set; } = new List<Switch>();

        public List<Windows> Windows { get; set; } = new List<Windows>();
    }
}
