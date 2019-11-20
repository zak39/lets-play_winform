using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// nouveau ajout de using
using System.IO;   // pour importer un fichier
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace lets_play_winform
{
    public class Database
    {
        // constructeur a vide
        public Database() { }

        // attributs
        public string addrIPDB, usernameDB, passwordDB, name;

        // Constructeur
        public Database(string IP="127.0.0.1", string username="", string password="", string nameDatabase="")
        {
            this.addrIPDB = IP;
            this.usernameDB = username;
            this.passwordDB = password;
            this.name = nameDatabase;
        }

        // getters and setters
        public string AddrIPDB
        {
            get => this.addrIPDB;
            set => this.addrIPDB = value;
        }

        public string UsernameDB
        {
            get => this.usernameDB;
            set => this.usernameDB = value;
        }

        public string PasswordDB
        {
            get => this.passwordDB;
            set => this.passwordDB = value;
        }
        
        public string Name
        {
            get => this.name;
            set => this.name = value;
        }

        // Methodes

        public string AfficherDatabase()
        {
            string result = "";
            string connectionString = "SERVER=" + this.addrIPDB + ";DATABASE=" + this.name + ";UID=" + this.usernameDB + ";PASSWORD=" + this.passwordDB + "";
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT * FROM scores ORDER BY note DESC";

            DbDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result = result + reader.GetString(0) + "\t\t\t" + reader.GetInt32(1) + "\r\n";
            }

            connection.Close();
            reader.Close();

            return result;

        }
        public void SaveDatabase(string prenom, int note)
        {
            //List<String> list_prenoms = new List<String>();
            //List<int> list_notes = new List<int>();

            string prenomInDB = "";

            string connectionString = "SERVER="+ this.addrIPDB +";DATABASE="+ this.name +";UID="+ this.usernameDB +";PASSWORD="+ this.passwordDB +"";
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT prenom from scores WHERE prenom = \""+prenom+"\"";
            
            // On recupere le prenom dans la database
            DbDataReader reader = cmd.ExecuteReader(); // Execute the request
            
            while(reader.Read())
            {
                prenomInDB = reader.GetString(0);
            }

            reader.Close();

            if (prenomInDB == "")
            {
                // Creer l'utilisateur
                cmd.CommandText = "INSERT INTO scores(prenom,note) VALUES (@prenom, @note)";

                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@note", note);

                cmd.ExecuteNonQuery(); // Execute the request

            }
            else
            {
                int noteCurrently = 0;
                // Mettre a jour le score du user
                cmd.CommandText = "SELECT note from scores WHERE prenom = \""+ prenom +"\"";
                
                // On recupere la note de la database
                reader = cmd.ExecuteReader(); // Execute the request
                
                while(reader.Read())
                {
                    noteCurrently = reader.GetInt32(0);
                }

                reader.Close();

                // On additionne l'ancienne avec la nouvelle note
                int newNote = noteCurrently + note;

                // On la met a jour
                cmd.CommandText = "UPDATE scores SET prenom=@prenom,note=@note WHERE prenom=@prenom";
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@note", newNote);
                cmd.ExecuteNonQuery(); // Execute the request
            }

            connection.Close();

        }

        public void ChargeDatabase(string pathRelative = "C:\\Users\\hela\\Documents\\code\\csharp\\lets-play_winform-with-git\\lets-play_winform\\lets-play_winform\\file.txt")
        {

            string connectionString = "SERVER=" + this.addrIPDB + ";DATABASE=" + this.name + ";UID=" + this.usernameDB + ";PASSWORD=" + this.passwordDB + "";
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();

            using (var reader = new StreamReader(@pathRelative)) // https://stackoverflow.com/questions/5282999/reading-csv-file-and-storing-values-into-an-array
            {
                List<string> listPernom = new List<string>();
                List<string> listSolution = new List<string>();
                List<string> listMot = new List<string>();
                List<int> listScore = new List<int>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    listPernom.Add(values[0]);
                    listSolution.Add(values[1]);
                    listMot.Add(values[2]);
                    listScore.Add(int.Parse(values[3]));
                }

                int lenPrenom = listPernom.Count;
                int lenSolution = listSolution.Count;
                int lenMot = listMot.Count;
                int lenScore = listScore.Count;

                if (lenSolution == lenMot)
                {
                    for (int i = 0; i <= lenMot - 1; i++)
                    {
                        SaveDatabase(listPernom[i], listScore[i]);
                    }
                }
                connection.Close();
            }
        }

        public void DeleteAllDatabase()
        {
            string connectionString = "SERVER=" + this.addrIPDB + ";DATABASE=" + this.name + ";UID=" + this.usernameDB + ";PASSWORD=" + this.passwordDB + "";
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM scores";
            cmd.ExecuteNonQuery(); // Execute the request
            connection.Close();
        }
    }
}
