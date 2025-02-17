using System.ComponentModel.DataAnnotations;

// Todo first and foremost convert myconsole functionality to work with string instead of char array
namespace Agenda_de_contact.Model
{
    public class Utilizator
    {
        [Key]
        public int? Id { get; set; }
        public string? Nume { get; set; }
        public string? Prenume { get; set; }
        public Utilizator(int a, string b, string c)
        {
            Id = a;
            Nume = b;
            Prenume = c;
        }
        public Utilizator() { }
    }
}