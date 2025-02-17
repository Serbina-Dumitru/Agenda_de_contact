using Aplicatia.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace Agenda_de_contact.Model
{ 
    internal class Context
    {
        private readonly string connectionString;
        public Context()
        {
            this.connectionString = "Data Source = Agenda_de_contacte.db";
        }
        public void AddAgenda(Agenda agenda)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Agenda (Denumirea,Fk_id_utilizator) VALUES (@denumirea,@utilizator);";
            //command.Parameters.AddWithValue("@id", agenda.Id);
            command.Parameters.AddWithValue("@denumirea", agenda.Denumirea);
            command.Parameters.AddWithValue("@utilizator", agenda.Fk_id_utilizator);
            command.ExecuteNonQuery();

        }
        public List<Agenda> GetAllAgendas()
        {
            List<Agenda> list = new List<Agenda>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = @"SELECT * FROM Agenda;";
                SqliteCommand command = new SqliteCommand(sql, connection);
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string denumire = Convert.ToString(reader["Denumirea"]);
                    int Fk_id_utilizator = Convert.ToInt32(reader["Fk_id_utilizator"]);// here i would need somehow to prind (if the id exists) the information from the utilizator 
                    list.Add(new Agenda(id, denumire, Fk_id_utilizator));
                    //Console.WriteLine($"Agenda cu id {id} are denumirea de {denumire} si este al utilizatorului cu idul de {Fk_id_utilizator}");
                }
            }
            return list;
        }
        public void AddUtilizator(Utilizator utilizator)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Utilizator (Nume,Prenume) VALUES (@nume,@prenume);";
            //command.Parameters.AddWithValue("@id", utilizator.Id);
            command.Parameters.AddWithValue("@nume", utilizator.Nume);
            command.Parameters.AddWithValue("@prenume", utilizator.Prenume);
            command.ExecuteNonQuery();
        }
        public List<Utilizator> GetAllUtilizators()
        {
            List<Utilizator> list = new List<Utilizator>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = @"SELECT * FROM Utilizator;";
                SqliteCommand command = new SqliteCommand(sql, connection);
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string nume = Convert.ToString(reader["Nume"]);
                    string prenume = Convert.ToString(reader["Prenume"]);
                    list.Add(new Utilizator(id, nume, prenume));
                    //Console.WriteLine($"Utilizatorul cu id {id} are numele de {nume} si prenumele de {prenume}");
                }
            }
            return list;
        }
        public void AddContact(Contact contact)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Contact (Nume,Prenume,Numarul_de_contact,Mail,Fk_id_Agenda,Preferat) VALUES (@nume,@prenume,@numar,@mail,@fkagenda,@preferat);";
            //command.Parameters.AddWithValue("@id", contact.Id);
            command.Parameters.AddWithValue("@nume", contact.Nume);
            command.Parameters.AddWithValue("@prenume", contact.Prenume);
            command.Parameters.AddWithValue("@numar", contact.Numarul_de_contact);
            command.Parameters.AddWithValue("@mail", contact.Mail);
            command.Parameters.AddWithValue("@fkagenda", contact.Fk_id_Agenda);
            command.Parameters.AddWithValue("@preferat", contact.Preferat);
            command.ExecuteNonQuery();

        }
        public List<Contact> GetAllContacts()   
        {
            List<Contact> list = new List<Contact>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = @"SELECT * FROM Contact;";
                SqliteCommand command = new SqliteCommand(sql, connection);
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string nume = Convert.ToString(reader["Nume"]);
                    string prenume = Convert.ToString(reader["Prenume"]);
                    string nrcontact = Convert.ToString(reader["Numarul_de_contact"]);
                    string mail = Convert.ToString(reader["Mail"]);
                    int fkagenda = Convert.ToInt32(reader["Fk_id_Agenda"]);
                    int preferat = Convert.ToInt32(reader["Preferat"]);
                    list.Add(new Contact(id, nume, prenume, nrcontact, mail, fkagenda,preferat));
                    //Console.WriteLine($"Contactul cu idul {id} are numele de {nume} prenumele de {prenume} numarul de contact al lui este {nrcontact} mailul lui este {mail} si este inregistrat in agenda cu idul de { fkagenda}");
                }
            }
            return list;
        }
        public void DeleteContactWithCertainId(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM Contact WHERE id=@id;";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
        public void AlterCertainColumnFromContacts(Contact c)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"UPDATE Contact SET Nume = @nume, Prenume = @prenume, Numarul_de_contact = @num, Mail = @mail, Fk_id_Agenda = @fkid WHERE id=@id;";
            command.Parameters.AddWithValue("@nume", c.Nume);
            command.Parameters.AddWithValue("@prenume", c.Prenume);
            command.Parameters.AddWithValue("@num", c.Numarul_de_contact);
            command.Parameters.AddWithValue("@mail", c.Mail);
            command.Parameters.AddWithValue("@fkid", c.Fk_id_Agenda);
            command.Parameters.AddWithValue("@id", c.Id);
            command.ExecuteNonQuery();
        }
        public List<Contact> SelectContactsByNumber(string number)
        {
            List<Contact> contactele = new List<Contact>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = @"SELECT * FROM Contact WHERE Numarul_de_contact = @num;";

                SqliteCommand command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("@num", number);
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string nume = Convert.ToString(reader["Nume"]);
                    string prenume = Convert.ToString(reader["Prenume"]);
                    string nrcontact = Convert.ToString(reader["Numarul_de_contact"]);
                    string mail = Convert.ToString(reader["Mail"]);
                    int fkagenda = Convert.ToInt32(reader["Fk_id_Agenda"]);
                    int preferat = Convert.ToInt32(reader["Preferat"]);
                    contactele.Add(new Contact(id, nume, prenume, nrcontact, mail, fkagenda,preferat));
                }
            }
            return contactele;
        }
        public List<Contact> SelectContactsByMail(string amail)
        {
            List<Contact> contactele = new List<Contact>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = @"SELECT * FROM Contact WHERE Mail = @mail;";

                SqliteCommand command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("@mail",amail);
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string nume = Convert.ToString(reader["Nume"]);
                    string prenume = Convert.ToString(reader["Prenume"]);
                    string nrcontact = Convert.ToString(reader["Numarul_de_contact"]);
                    string mail = Convert.ToString(reader["Mail"]);
                    int fkagenda = Convert.ToInt32(reader["Fk_id_Agenda"]);
                    int preferat = Convert.ToInt32(reader["Preferat"]);
                    contactele.Add(new Contact(id, nume, prenume, nrcontact, mail, fkagenda,preferat));
                }
            }
            return contactele;
        }
        public List<Contact> SelectContactsByName(string name)
        {
            List<Contact> contactele = new List<Contact>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = @"SELECT * FROM Contact WHERE Nume = @name;";

                SqliteCommand command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("@name",name);
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string nume = Convert.ToString(reader["Nume"]);
                    string prenume = Convert.ToString(reader["Prenume"]);
                    string nrcontact = Convert.ToString(reader["Numarul_de_contact"]);
                    string mail = Convert.ToString(reader["Mail"]);
                    int fkagenda = Convert.ToInt32(reader["Fk_id_Agenda"]);
                    int preferat = Convert.ToInt32(reader["Preferat"]);
                    contactele.Add(new Contact(id, nume, prenume, nrcontact, mail, fkagenda,preferat));
                }
            }
            return contactele;
        }
        public List<Contact> SelectContactsByPrenume(string aprenume)
        {
            List<Contact> contactele = new List<Contact>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = @"SELECT * FROM Contact WHERE Prenume = @prenume;";

                SqliteCommand command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("@prenume",aprenume);
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string nume = Convert.ToString(reader["Nume"]);
                    string prenume = Convert.ToString(reader["Prenume"]);
                    string nrcontact = Convert.ToString(reader["Numarul_de_contact"]);
                    string mail = Convert.ToString(reader["Mail"]);
                    int fkagenda = Convert.ToInt32(reader["Fk_id_Agenda"]);
                    int preferat = Convert.ToInt32(reader["Preferat"]);
                    contactele.Add(new Contact(id, nume, prenume, nrcontact, mail, fkagenda,preferat));
                }
            }
            return contactele;
        }
        public List<Contact> SelectContactsAndOrderByNumber()
        {
            List<Contact> contactele = new List<Contact>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = @"SELECT * FROM Contact ORDER BY Numarul_de_contact ASC;";

                SqliteCommand command = new SqliteCommand(sql, connection);
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string nume = Convert.ToString(reader["Nume"]);
                    string prenume = Convert.ToString(reader["Prenume"]);
                    string nrcontact = Convert.ToString(reader["Numarul_de_contact"]);
                    string mail = Convert.ToString(reader["Mail"]);
                    int fkagenda = Convert.ToInt32(reader["Fk_id_Agenda"]);
                    int preferat = Convert.ToInt32(reader["Preferat"]);
                    contactele.Add(new Contact(id, nume, prenume, nrcontact, mail, fkagenda,preferat));
                }
            }
            return contactele;
        }
        public List<Contact> SelectContactsAndOrderByMail()
        {
            List<Contact> contactele = new List<Contact>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = @"SELECT * FROM Contact ORDER BY Numarul_de_contact ASC;";

                SqliteCommand command = new SqliteCommand(sql, connection);
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string nume = Convert.ToString(reader["Nume"]);
                    string prenume = Convert.ToString(reader["Prenume"]);
                    string nrcontact = Convert.ToString(reader["Numarul_de_contact"]);
                    string mail = Convert.ToString(reader["Mail"]);
                    int fkagenda = Convert.ToInt32(reader["Fk_id_Agenda"]);
                    int preferat = Convert.ToInt32(reader["Preferat"]);
                    contactele.Add(new Contact(id, nume, prenume, nrcontact, mail, fkagenda,preferat));
                }
            }
            return contactele;
        }
        public List<Contact> SelectContactsAndOrderByName()
        {
            List<Contact> contactele = new List<Contact>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = @"SELECT * FROM Contact ORDER BY Nume ASC;";

                SqliteCommand command = new SqliteCommand(sql, connection);
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string nume = Convert.ToString(reader["Nume"]);
                    string prenume = Convert.ToString(reader["Prenume"]);
                    string nrcontact = Convert.ToString(reader["Numarul_de_contact"]);
                    string mail = Convert.ToString(reader["Mail"]);
                    int fkagenda = Convert.ToInt32(reader["Fk_id_Agenda"]);
                    int preferat = Convert.ToInt32(reader["Preferat"]);
                    contactele.Add(new Contact(id, nume, prenume, nrcontact, mail, fkagenda,preferat));
                }
            }
            return contactele;
        }
        public List<Contact> SelectContactsAndOrderByPrenume()
        {
            List<Contact> contactele = new List<Contact>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = @"SELECT * FROM Contact ORDER BY Prenume ASC;";

                SqliteCommand command = new SqliteCommand(sql, connection);
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string nume = Convert.ToString(reader["Nume"]);
                    string prenume = Convert.ToString(reader["Prenume"]);
                    string nrcontact = Convert.ToString(reader["Numarul_de_contact"]);
                    string mail = Convert.ToString(reader["Mail"]);
                    int fkagenda = Convert.ToInt32(reader["Fk_id_Agenda"]);
                    int preferat = Convert.ToInt32(reader["Preferat"]);
                    contactele.Add(new Contact(id, nume, prenume, nrcontact, mail, fkagenda,preferat));
                }
            }
            return contactele;
        }
        public void MarkContactAsStarred(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"UPDATE Contact SET Preferat=1 WHERE id=@id;";
            command.Parameters.AddWithValue("@id",id);
            command.ExecuteNonQuery();
        }
        public void UnmarkContactFromStarred(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"UPDATE Contact SET Preferat=0 WHERE id=@id;";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
        public void WriteToJsonFile(List<Contact> contacte)
        {
            var _options = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

            var jsonString = JsonSerializer.Serialize(contacte, _options);
            string filename = "ContacteBackup" + DateTime.Now.ToString("__yyyy-MM-dd__HH-mm") + ".json";
            Console.WriteLine(filename);
            File.WriteAllText(filename, jsonString);
        }
        public List<Contact> ReadFromJsonFile(string filename)
        {
            var  _options = new JsonSerializerOptions() {PropertyNameCaseInsensitive = true};
            var json = File.ReadAllText(filename); //"ContacteBackup__2023-06-06__09-02.json"
            List<Contact> contactele;
            try
            {
                contactele = JsonSerializer.Deserialize<List<Contact>>(json, _options);
            }
            catch (Exception)
            {
                Console.WriteLine("Fileul ales este invalid.");
                return new List<Contact>();
                throw;
            }
            return contactele;
        }
    }
}
