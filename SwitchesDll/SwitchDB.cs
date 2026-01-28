using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwitchesDll
{
    public class SwitchDB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MacAdress { get; set; } = "";
        public string Name { get; set; } = "";
        public string Ip { get; set; } = "";

        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        public List<Equipment> Equipments { get; set; } = new();
    }
}
