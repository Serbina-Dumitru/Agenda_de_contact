using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Todo first and foremost convert myconsole functionality to work with string instead of char array
namespace Aplicatia.Model
{
    public class Agenda
    {
        [Key]
        public int Id { get; set; }
        public string Denumirea { get; set; }
        [ForeignKey("Utilizator")]
        public int Fk_id_utilizator { get; set; }
        public Agenda(int Id, string Denumirea, int fk)
        {
            this.Id = Id; this.Denumirea = Denumirea; Fk_id_utilizator = fk;
        }
        public Agenda() { }
    }
}