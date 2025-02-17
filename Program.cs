using Agenda_de_contact.Model;
using Agenda_de_contact.View;
using Aplicatia.Model;
using Microsoft.VisualBasic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;


namespace Agenda_de_contact
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Context context = new Context();
            var menuOptions = new List<IMenuOption> 
            {
                new MenuOption("Adaugarea unui nou contact în agenda utilizatorului.",context => {
                    InfoContact infoContact = new InfoContact();
                    context.AddContact(infoContact.ReadContact());
                    Console.WriteLine("Datele au fost adaugate in baza de date.");
                }),
                new MenuOption("Stergerea unui contact existent din agenda utilizatorului.",context =>{
                    var utilizatori = context.GetAllUtilizators().ToList();
                    utilizatori.ForEach(c => {
                        Console.WriteLine($"Id: {c.Id},Nume: {c.Nume},Prenume: {c.Prenume}");
                    });
                    Console.WriteLine("0. Iesire");
                    int id = 0;
                    Console.WriteLine("Introdu idul utilizatorului a carui contact doresti sa stergi: ");
                    do
                    {
                        if(!int.TryParse(Console.ReadLine(),out id))
                        {
                            Console.WriteLine("Introdu un numar valid.");
                            continue;
                        }
                        if(id == 0)
                        {
                            return;
                        }
                        else if(!utilizatori.Where(x=> x.Id == id).Any())
                        {
                            Console.WriteLine("Utilizator cu acest id nu exista.");
                            continue;
                        }
                        break;

	                } while (true);
                    var agende = context.GetAllAgendas().ToList();
                    var agendaExdsts = agende.Where(x => x.Fk_id_utilizator == id);
                    if (agendaExdsts.Any())
                    {
                        var agendeleUtilizatorului = agendaExdsts.ToList();
                        var agendaAleasaDeUtilizator = new Agenda();
                        if(agendeleUtilizatorului.Count() > 1) 
                        {
                            Console.WriteLine("Agnedele utilizatorulu sunt: ");
                            agendeleUtilizatorului.ForEach(x => Console.WriteLine($"Id: {x.Id}, Agenda: {x.Denumirea}"));
                            int tempid = 0;
                            do
                            {
                                if(!int.TryParse(Console.ReadLine(),out tempid))
                                {
                                    Console.WriteLine("Introdu un numar valid.");
                                    continue;
                                }
                                if(!agendeleUtilizatorului.Where(x=> x.Id == tempid).Any())
                                {
                                    Console.Write("Agenda cu acest id nu exista.");
                                    continue;
                                }
                                break;

                            } while (true);
                            agendaAleasaDeUtilizator = agendeleUtilizatorului[tempid-1];
                        }
                        else {agendaAleasaDeUtilizator = agendeleUtilizatorului[0]; }
                        Console.WriteLine($"Id: {agendaAleasaDeUtilizator.Id}, Denumirea: {agendaAleasaDeUtilizator.Denumirea}");
                        Console.Clear();
                        Console.WriteLine($"Agenda cu numele {agendaAleasaDeUtilizator.Denumirea} al utilizatorului {utilizatori[id].Nume} {utilizatori[id].Prenume} are urmatoarele contacte: ");
                        var contactele = context.GetAllContacts().Where(x => x.Fk_id_Agenda == agendaAleasaDeUtilizator.Id).ToList();
                        contactele.ForEach(x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));
                        Console.WriteLine("0. Iesire");
                        int idToDelete;
                        Console.Write("Introdu idul contactului pe care doresti sa il stergi: ");
                        do
                        {
                            if(!int.TryParse(Console.ReadLine(),out idToDelete))
                            {
                                Console.WriteLine("Introdu un numar valid.");
                                continue;
                            }
                            if(idToDelete == 0){return;}
                            else if(!contactele.Where(x=> x.Id == idToDelete).Any())
                            {
                                Console.Write("Utilizator cu acest id nu exista.");
                                continue;
                            }
                            break;
                        } while (true);
                        context.DeleteContactWithCertainId(idToDelete);
                        Console.WriteLine("Comanda a fost executata.");
                    }
                    else
                    {
                        Console.WriteLine("Acest utilizator nu are agenda.");
                    }
                }),
                new MenuOption("Actualizarea informatiilor referitoare la un contact existent.",context => {
                    var contacte = context.GetAllContacts();
                    contacte.ForEach(x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));
                    Console.WriteLine("0. Iesire");
                    Console.WriteLine("Intrdouceti id-ul contactului al carui date doresti sa le actualizezi: ");
                    int idToModify = 0;
                    do
                    {
                        if(!int.TryParse(Console.ReadLine(), out idToModify))
                        {
                            Console.WriteLine("Introdu un numar valid.");
                            continue;
                        }
                        if(idToModify == 0){return;}
                        else if(!contacte.Where(x=>x.Id == idToModify).Any())
                        {
                            Console.WriteLine("Contact cu acest id nu exista.");
                            continue;
                        }
                        break;
	                } while (true);
                    var element = contacte.Where(x => x.Id  == idToModify).ToList()[0];
                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"1. Nume: {element.Nume}\n2. Prenume: {element.Prenume}\n3. Numarul de contact: {element.Numarul_de_contact}\n4. Mail: {element.Mail}\n5. Id-ul agendei in care contactul se afla: {element.Fk_id_Agenda}");
                        Console.WriteLine("0. Iesire si salvare\n-1.Iesire fara salvare");
                        Console.WriteLine("Intredu numarul elementului pe care doresti sal modifici");
                        var idOfTheField = 0;
                        do
                        {
                            if(!int.TryParse(Console.ReadLine(),out idOfTheField))
                            {
                                Console.WriteLine("Introdu un numar valid.");
                                continue;
                            }
                            if(idOfTheField == -1){return;}
                            else if(idOfTheField ==  0)
                            {
                                context.AlterCertainColumnFromContacts(element);
                                return;
                            }
                            else if(idOfTheField > 5 ||  idOfTheField < -1)
                            {
                                Console.WriteLine("Introdu un numar de mai sus.");
                                continue;
                            }
                            break;
	                    } while (true);
                        switch (idOfTheField)
                        {
                            case 1:
                                Console.WriteLine("Introdu numele nou al persoanei: ");
                                element.Nume = Console.ReadLine();
                                break;
                            case 2:
                                Console.WriteLine("Introdu prenumele nou al persoanei: ");
                                element.Prenume = Console.ReadLine();
                                break;
                            case 3:
                                Console.WriteLine("Introdu numarul de contact nou al persoanei: ");
                                element.Numarul_de_contact = Console.ReadLine();
                                break;
                            case 4:
                                Console.WriteLine("Introdu mailul nou al persoanei: ");
                                element.Mail = Console.ReadLine();
                                break;
                            case 5:
                                Console.WriteLine("Introdu idul nou al agendei in care doresti el sa fie: ");
                                int newid = 0;
                                do
                                {
                                    if(!int.TryParse(Console.ReadLine(), out newid))
                                    {
                                        Console.WriteLine("Introdu un numar valid.");
                                        continue;
                                    }
                                    break;
	                            } while (true);
                                element.Fk_id_Agenda = newid;
                                break;

                        };
	                } while (true);
                }),
                new MenuOption("Afisarea tuturor contactelor din agenda utilizatorului.",context =>{
                    var utilizatori = context.GetAllUtilizators().ToList();
                    utilizatori.ForEach(c => {
                        Console.WriteLine($"Id: {c.Id},Nume: {c.Nume},Prenume: {c.Prenume}");
                    });
                    Console.WriteLine("0. Iesire");
                    int id;
                    Console.WriteLine("Introdu idul utilizatorului a carui contacte doresti sa le vizualizezi: ");
                    do
                    {
                        if(!int.TryParse(Console.ReadLine(),out id))
                        {
                            Console.WriteLine("Introdu un numar valid.");
                            continue;
                        }
                        if (id == 0){return; }
                        else if(!utilizatori.Where(x=> x.Id == id).Any())
                        {
                            Console.Write("Utilizator cu acest id nu exista.");
                            continue;
                        }
                        break;
                    } while (true);
                    var agende = context.GetAllAgendas().ToList();
                    var agendaExdsts = agende.Where(x => x.Fk_id_utilizator == id);
                    if (agendaExdsts.Count()>0)
                    {
                        var agendeleUtilizatorului = agendaExdsts.ToList();
                        var agendaAleasaDeUtilizator = new Agenda();
                        //int id = 0;
                        if(agendeleUtilizatorului.Count() > 1)
                        {
                            Console.WriteLine("Agnedele utilizatorulu sunt: ");
                            agendeleUtilizatorului.ForEach(x => Console.WriteLine($"Id: {x.Id}, Agenda: {x.Denumirea}"));
                            int tempid = 0;
                            do
                            {
                                if(!int.TryParse(Console.ReadLine(),out tempid))
                                {
                                    Console.WriteLine("Introdu un numar valid.");
                                    continue;
                                }
                                if(!agendeleUtilizatorului.Where(x=> x.Id == tempid).Any())
                                {
                                    Console.Write("Agenda cu acest id nu exista.");
                                    continue;
                                }
                                break;

                            } while (true);
                            agendaAleasaDeUtilizator = agendeleUtilizatorului[tempid-1];
                        }
                        else {agendaAleasaDeUtilizator = agendeleUtilizatorului[0]; }
                        //Console.WriteLine($"Id: {agendaAleasaDeUtilizator.Id}, Denumirea: {agendaAleasaDeUtilizator.Denumirea}");
                        Console.Clear();
                        Console.WriteLine($"Agenda cu numele {agendaAleasaDeUtilizator.Denumirea} al utilizatorului {utilizatori[id].Nume} {utilizatori[id].Prenume} are urmatoarele contacte: ");
                        var contactele = context.GetAllContacts().Where(x => x.Fk_id_Agenda == agendaAleasaDeUtilizator.Id).ToList();
                        contactele.ForEach(x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));
                    }
                    else
                    {
                        Console.WriteLine("Acest utilizator nu are agenda.");
                    }

                }),
                new MenuOption("Cautarea unui contact dupa nume sau după alte criterii(telefon, email, etc.).",context =>{
                    Console.WriteLine("Cautarea unui contact");
                    Console.WriteLine("1. Dupa telefon");
                    Console.WriteLine("2. Dupa email");
                    Console.WriteLine("3. Dupa Nume");
                    Console.WriteLine("4. Dupa Prenume");
                    Console.WriteLine("0. Iesire");
                    var choosing = 0;
                    do
                    {
                        if(!int.TryParse(Console.ReadLine(),out choosing))
                        {
                            Console.WriteLine("Introdu un numar valid.");
                            continue;
                        }
                        else if(choosing <0 || choosing > 5)
                        {
                            Console.WriteLine("Introdu unul din numerele de mai sus.");
                            continue;
                        }
                        break;
                    } while (true);
                    switch (choosing)
                    {
                        case 0: return;
                        case 1:
                            {
                                Console.WriteLine("Introdu numarul de telefon pentru a cauta cu ajutorul lui: ");
                                string numar = Console.ReadLine();
                                var elemente = context.SelectContactsByNumber(numar);
                                if (elemente.Count()>0)
                                {
                                    elemente.ForEach(x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));;
                                }
                                else { Console.WriteLine("Nu exista nici un contact cu acest numar."); }
                            }
                            break;
                        case 2:
                            {
                                Console.WriteLine("Introdu emailul pentru a cauta cu ajutorul lui: ");
                                string mail = Console.ReadLine();
                                var elemente = context.SelectContactsByMail(mail);
                                if (elemente.Count()>0)
                                {
                                    elemente.ForEach(x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));;
                                }
                                else { Console.WriteLine("Nu exista nici un contact cu acest mail."); }
                            }
                            break;
                        case 3:
                            {
                                Console.WriteLine("Introdu Numele pentru a cauta cu ajutorul lui: ");
                                string name = Console.ReadLine();
                                var elemente = context.SelectContactsByName(name);
                                if (elemente.Count()>0)
                                {
                                    elemente.ForEach(x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));;
                                }
                                else { Console.WriteLine("Nu exista nici un contact cu acest nume."); }
                            }
                            break;
                        case 4:
                            {
                                Console.WriteLine("Introdu Prenumele pentru a cauta cu ajutorul lui: ");
                                string prenume = Console.ReadLine();
                                var elemente = context.SelectContactsByPrenume(prenume);
                                if (elemente.Count()>0)
                                {
                                    elemente.ForEach(x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));;
                                }
                                else { Console.WriteLine("Nu exista nici un contact cu acest prenume."); }
                            }
                            break;
                        
                    }
                }),
                new MenuOption("Sortarea contactelor în functie de nume, prenume sau alt criteriu.",context =>{
                    Console.WriteLine("Afisarea contactelor sortate ");
                    Console.WriteLine("1. Dupa telefon");
                    Console.WriteLine("2. Dupa email");
                    Console.WriteLine("3. Dupa Nume");
                    Console.WriteLine("4. Dupa Prenume");
                    Console.WriteLine("0. Iesire");
                    var choosing = 0;
                    do
                    {
                        if(!int.TryParse(Console.ReadLine(),out choosing))
                        {
                            Console.WriteLine("Introdu un numar valid.");
                            continue;
                        }
                        else if(choosing <0 || choosing > 5)
                        {
                            Console.WriteLine("Introdu unul din numerele de mai sus.");
                            continue;
                        }
                        break;
                    } while (true);
                    switch (choosing)
                    {
                        case 0: return;
                        case 1:
                            {
                                var elemente = context.SelectContactsAndOrderByNumber();
                                if (elemente.Count()>0)
                                {
                                    elemente.ForEach(x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));;
                                }
                                else { Console.WriteLine("Nu exista nici un contact cu acest numar."); }
                            }
                            break;
                        case 2:
                            {
                                var elemente = context.SelectContactsAndOrderByMail();
                                if (elemente.Count()>0)
                                {
                                    elemente.ForEach(x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));;
                                }
                                else { Console.WriteLine("Nu exista nici un contact cu acest mail."); }
                            }
                            break;
                        case 3:
                            {
                                var elemente = context.SelectContactsAndOrderByName();
                                if (elemente.Count()>0)
                                {
                                    elemente.ForEach(x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));;
                                }
                                else { Console.WriteLine("Nu exista nici un contact cu acest nume."); }
                            }
                            break;
                        case 4:
                            {
                                var elemente = context.SelectContactsAndOrderByPrenume();
                                if (elemente.Count()>0)
                                {
                                    elemente.ForEach(x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));;
                                }
                                else { Console.WriteLine("Nu exista nici un contact cu acest prenume."); }
                            }
                            break;

                    }
                }),
                new MenuOption("Gestionarea contactelor preferate si afisarea acestora in parte.",context =>{
                    Console.WriteLine("1. Marcarea contactului ca preferat");
                    Console.WriteLine("2. Afisarea contactului preferat");
                    Console.WriteLine("3. Demarcarea contactului preferat");
                    int choosed;
                    do
                    {
                        if(!int.TryParse(Console.ReadLine(),out choosed))
                        {
                            Console.WriteLine("Introdu un numar valid.");
                            continue;
                        }
                        if(choosed < 0 || choosed > 3)
                        {
                            Console.Write("Optiunea introdusa nu exista.");
                            continue;
                        }
                        break;
                    }while (true);
                    if (choosed == 1)
                    {
                        var contacte = context.GetAllContacts();
                        contacte.ForEach(x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));
                        int idToModify;
                        Console.WriteLine("Introdu id-ul contactului pe care doresti sa il marchezi ca preferat: ");
                        do
                        {
                            if(!int.TryParse(Console.ReadLine(), out idToModify))
                            {
                                Console.WriteLine("Introdu un numar valid.");
                                continue;
                            }
                                if(idToModify == 0){return;}
                            else if(!contacte.Where(x=>x.Id == idToModify).Any())
                            {
                                Console.WriteLine("Contact cu acest id nu exista.");
                                continue;
                            }
                            break;
                        } while (true);
                        context.MarkContactAsStarred(idToModify);
                    }
                    else if (choosed == 2)
                    {
                        var contacte = context.GetAllContacts().Where(x => x.Preferat == 1).ToList();
                        if(contacte.Count() > 0)
                        {
                            contacte.ForEach(x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));
                        }
                        else
                        {
                            Console.WriteLine("Nu exista nici un contact marcat ca preferat.");
                        }
                    }
                    else if(choosed == 3)
                    {
                        var contacte = context.GetAllContacts().Where(x => x.Preferat == 1).ToList();
                        if(contacte.Count() > 0)
                        {
                            contacte.ForEach(x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));
                            Console.WriteLine("Introdu id-ul contactului pe care doresti sa il demarchezi din preferate: ");
                            int idToModify;
                            do
                            {
                                if(!int.TryParse(Console.ReadLine(), out idToModify))
                                {
                                    Console.WriteLine("Introdu un numar valid.");
                                    continue;
                                }
                                    if(idToModify == 0){return;}
                                else if(!contacte.Where(x=>x.Id == idToModify).Any())
                                {
                                    Console.WriteLine("Contact cu acest id nu exista.");
                                    continue;
                                }
                                break;
                            } while (true);
                            context.UnmarkContactFromStarred(idToModify);
                        }else{Console.WriteLine("Nu exista contacte marcate ca preferate.");}
                    }
                }),                
                new MenuOption("Exportul / agregarea contactelor în format JSON.",context =>{
                    Console.WriteLine("1. Exportarea contactelor");
                    Console.WriteLine("2. Importarea contactelor");
                    Console.WriteLine("0. Iesire");
                    Console.WriteLine("AHTUNG: La importare fileurile.json trebuie ca fileurile exportate sa se afle in folderul in care se afla programa.");
                    int choosed = 0;
                    do
                    {
                        if(!int.TryParse(Console.ReadLine(),out choosed))
                        {
                            Console.WriteLine("Introdu un numar valid.");
                            continue;
                        }
                        if(choosed < 0 || choosed > 2)
                        {
                            Console.Write("Optiunea introdusa nu exista.");
                            continue;
                        }
                        break;
                    }while (true);
                    if(choosed == 1)
                    {
                        var contactele = context.GetAllContacts();
                        List<Contact> ales = new List<Contact>();
                        do
                        {
                        Console.Clear();
                        Console.WriteLine("Exportarea contactelor");
                        contactele.ForEach(x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));
                        Console.WriteLine("Re alegeti elementul pentru al deselecta.");
                        Console.Write("Contactele selectate: ");
                        ales.ForEach(x=> Console.Write($"{x.Id} "));
                        Console.WriteLine("\n0. Exportare si iesire");
                        Console.WriteLine("-1. Iesire fara exportare");
                            int temp;
                            if(!int.TryParse(Console.ReadLine(), out temp))
                            {
                                Console.WriteLine("Introdu un numar normal");
                                continue;
                            }
                            else if(contactele.Where(x => x.Id == temp).Count()>0)
                            {
                                var check = ales.Where(x => x.Id == temp).ToList();
                                if (check.Count() > 0)
                                {
                                    ales.Remove(check[0]);
                                }
                                else
                                {
                                    ales.Add(contactele.Where(x => x.Id == temp).ToList()[0]);
                                }
                            }
                            else if(temp == 0)
                            {
                                context.WriteToJsonFile(ales);
                                Console.WriteLine("Contactele au fost exportate in json file.");
                                return;
                                // exit with save
                            }
                            else if(temp == -1)
                            {
                                return;
                            }
                        } while (true);
                    }
                    else if (choosed == 2)
                    {
                        Console.Clear();
                        var binaryFilePath = System.AppDomain.CurrentDomain.BaseDirectory;
                        DirectoryInfo d = new DirectoryInfo(binaryFilePath); 
                        FileInfo[] Files = d.GetFiles("ContacteBackup*.json");

                        
                        //foreach(FileInfo (file,Index) in Files ){Console.WriteLine($"{file.Name}");}
                        for(int i = 0; i < Files.Length; i++)
                        {
                            Console.WriteLine($"{i+1}. {Files[i].Name}");
                        }
                        Console.WriteLine("0. Iesire");

                        
                        var filechoose = 0;
                        do
                        {
                            if(!int.TryParse(Console.ReadLine(), out filechoose))
                            {
                                Console.WriteLine("Introdu un numar normal");
                                continue;
                            }
                            else if(Files.Count() < filechoose - 1 || filechoose < 0)
                            {
                                Console.WriteLine("Asa o optiune nu exista");
                                continue;
                            }
                            else if(filechoose == 0)
                            {
                                return;
                            }
                            break;
                        } while (true);

                        var contacte = context.ReadFromJsonFile(Files[filechoose-1].FullName) ;
                        if(contacte.Count == 0)
                        {
                            return;
                        }
                        List<Contact> ales = new List<Contact>();
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("Contactele citite din file: ");
                            contacte.ForEach( x => Console.WriteLine($"Id: {x.Id}, Nume: {x.Nume}, Prenume: {x.Prenume}, Numarul de contact: {x.Numarul_de_contact}, Mail: {x.Mail}"));
                            Console.Write("Contactele alese pentru importare: ");
                            ales.ForEach( x => Console.Write($"{x.Id} "));
                            Console.WriteLine("\n0. Iesire si salvare importarilor");
                            Console.WriteLine("-1. Iesire fara salvare");
                            int temp;
                            if(!int.TryParse(Console.ReadLine(), out temp))
                            {
                                Console.WriteLine("Introdu un numar normal");
                                continue;
                            }
                            else if(contacte.Where(x => x.Id == temp).Count()>0)
                            {
                                var check = ales.Where(x => x.Id == temp).ToList();
                                if (check.Count() > 0)
                                {
                                    ales.Remove(check[0]);
                                }
                                else
                                {
                                    ales.Add(contacte.Where(x => x.Id == temp).ToList()[0]);
                                }
                            }
                            else if(temp == 0)
                            {
                                var contacteForSql = context.GetAllContacts();
                                for(int j = 0; j < ales.Count(); ++j)
                                {
                                    bool exists = false;
                                    for(int i = 0;!exists && i < contacteForSql.Count(); ++i)
                                    {
                                        exists = ales[j].Id == contacteForSql[i].Id;
                                    }
                                    if(!exists){ context.AddContact(ales[j]); }
                                    else
                                    {
                                        Console.WriteLine($"Contactul cu id-ul {ales[j].Id} nu a fost adaugat, pricina fiind existenta lui in baza de date.");
                                    }
                                }
                                return;
                            }
                            else if(temp == -1)
                            {
                                return;
                            }
                        } while (true);
                    }
                })

            };

            while (true)
            {
                Console.Clear();
                Process.Start("cmd.exe", "/c cls").WaitForExit();
                Console.WriteLine("Selectati o optiune din cele propuse:");
                for (int i = 0; i < menuOptions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {menuOptions[i].Description}");
                }
                Console.WriteLine("0. Iesire");
                var option = Console.ReadLine();
                if (int.TryParse(option, out int index))
                {
                    if (index == 0) break;
                    switch (index)
                    {
                        case 1:case 2:case 3:case 4:case 5:case 6:case 7:case 8:
                            Process.Start("cmd.exe", "/c cls").WaitForExit();
                            menuOptions[index - 1].Execute(context);
                            Console.WriteLine("Apasati o tasta pentru a continua...");
                            break;
                        default:
                            Console.WriteLine("Optiunea selectata nu exista.");
                            Console.WriteLine("Apasati o tasta pentru a continua...");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Te rog frumos sa introduci un numar.");
                }
                Console.ReadKey();
            }
        }
    }
}