using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Todo first and foremost convert myconsole functionality to work with string instead of char array
namespace Aplicatia.Model
{
    public class Contact
    {
        [Key]
        public int? Id { get; set; }
        public string? Nume { get; set; }
        public string? Prenume { get; set; }
        public string? Numarul_de_contact { get; set; }
        public string? Mail { get; set; }
        [ForeignKey("Agenda")]
        public int Fk_id_Agenda { get; set; }
        public int Preferat { get; set; }
        public Contact(int a, string b, string c, string d, string e, int f, int g)
        {
            Id = a; Nume = b;
            Prenume = c;
            Numarul_de_contact = d;
            Mail = e;
            Fk_id_Agenda = f;
            Preferat = g;
        }
        public Contact() { }
    }
}