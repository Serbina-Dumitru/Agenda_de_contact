using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agenda_de_contact.Model;
using Agenda_de_contact.View;

namespace Agenda_de_contact.View
{
    internal class MenuOption : IMenuOption
    {
        private readonly string _description;
        private readonly Action<Context> _action;
        public MenuOption(string description, Action<Context> action)
        {
            _description = description;
            _action = action;
        }
        public string Description => _description;
        public void Execute(Context context) 
        {
            _action(context);
        }
    }
}
