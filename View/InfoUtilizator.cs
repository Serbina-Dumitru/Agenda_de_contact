using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agenda_de_contact.Model;
using Aplicatia.Model;

namespace Agenda_de_contact.View
{
    internal class InfoUtilizator
    {
        public Utilizator ReadUtilizator() 
        {
            Utilizator utilizator = new Utilizator();
            Console.WriteLine("Introdu numele utilizatorului: ");
            utilizator.Nume = Console.ReadLine();
            Console.WriteLine("Introdu prenumele utilizatorului: ");
            utilizator.Prenume = Console.ReadLine();
            return utilizator;
        }
    }
}
