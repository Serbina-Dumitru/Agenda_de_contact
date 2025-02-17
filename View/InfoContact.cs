using Aplicatia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Agenda_de_contact.View
{
    internal class InfoContact
    {
        public Contact ReadContact()
        {
            Contact contact = new Contact();
            try
            {
                Console.Write("Introde Numele contactului: ");
                contact.Nume = Console.ReadLine();
                Console.Write("Introde Prenumele contactului: ");
                contact.Prenume = Console.ReadLine();
                Console.Write("Introduceti numarul de contact ");
                do
                {
                    contact.Numarul_de_contact = Console.ReadLine();
                    if (contact.Numarul_de_contact.Any(x => char.IsLetter(x) || (char.IsSymbol(x) & (x!='+'))))
                    {
                        Console.WriteLine("Terog introdu un numar valid.");
                        continue;
                    }
                    break;
                } while (true);
                Console.Write("Introdu emailul contactului: ");
                contact.Mail = Console.ReadLine();
                Console.Write("Introduceti idul agendei in care va fi scris contactul: ");
                do
                {
                    if (!int.TryParse(Console.ReadLine(), out int Fk_id_agenda))
                    {
                        Console.WriteLine("Introduceti va rog frumos un numar valid.");
                        continue;
                    }
                    contact.Fk_id_Agenda = Fk_id_agenda;
                    break;
                } while (true) ;
                Console.WriteLine("Introdu 1 pentru a marca contactul ca preferat, sau 0 pentru al lasa ca nepreferat: ");
                do
                {
                    if (!int.TryParse(Console.ReadLine(), out int preferat))
                    {
                        Console.WriteLine("Introduceti va rog frumos un numar valid.");
                        continue;
                    }
                    if (preferat < 0 || preferat > 1)
                    {
                        Console.WriteLine("Te rog introduce doar 1 sau 0.");
                        continue;
                    }
                    contact.Preferat = preferat;
                    break;
                } while (true) ;
                

            }
            catch (Exception ex) { Console.WriteLine("A aparut o eroare in citirea datelor."); }
            return contact;
        }
    }
}
