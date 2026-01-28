using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchesDll
{
    public class Windows
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MacAdress { get; set; }
        public string Name { get; set; }

        public int PortNumber { get; set; }
        public int SwitchDBId { get; set; }
    }
}
