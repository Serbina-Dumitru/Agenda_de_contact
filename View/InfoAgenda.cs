using Aplicatia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda_de_contact.View
{
    internal class InfoAgenda
    {
        public Agenda ReadAgenda()
        {
            Agenda agenda = new Agenda();
            try
            {
                Console.WriteLine("Introduceti idul utilizatorului: ");
                agenda.Denumirea = Console.ReadLine();
                do
                {
                    if(!int.TryParse(Console.ReadLine(),out int Fk_id_utilizator))
                    {
                        Console.WriteLine("Introduceti va rog frumos un numar valid.");
                        continue;
                    }
                    agenda.Fk_id_utilizator = Fk_id_utilizator;
                    break;
                } while (true);
            }catch (Exception ex) { Console.WriteLine("A aparut o eroare in citirea datelor."); }
            return agenda;
        }
    }
}
