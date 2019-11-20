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
        public void SaveDatabase(string prenom, int note)
        {
            //List<String> list_prenoms = new List<String>();
            //List<int> list_notes = new List<int>();

            string prenomInDB = "";

            string connectionString = "SERVER="+ this.addrIPDB +";DATABASE="+ this.name +";UID="+ this.usernameDB +";PASSWORD="+ this.passwordDB +"";
            MySqlConnection connection = new MySqlConnection(connectionString);

            // Au depart je faisais reference au listing des fichiers deja present et non a la database
            // Il faut coder pour que ca analyse la database
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
    }
}
