using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agenda_de_contact.Model;

namespace Agenda_de_contact.View
{
    internal interface IMenuOption
    {
        string Description { get; }
        void Execute(Context context);
    }
}
