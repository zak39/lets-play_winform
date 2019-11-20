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

    //  A decomenter
    public class Classe
    {
        // Constructeur
        public Classe() { }

        public List<Orthogenie> list_Orthogenie = new List<Orthogenie>()
        {
        };

        public List<Revision> list_Revision = new List<Revision>()
        {

        };

        private List<Orthogenie> list_Orthogenie_pub;

        public Classe(List<Orthogenie> list_Orthogenie)
        {
            this.list_Orthogenie_pub = list_Orthogenie;
        }

        public List<Revision> classement;
        public Classe(List<Revision> list_revision, List<Revision> classement)
        {
            this.list_Revision = list_revision;
            this.classement = classement;
        }

        /*public Classe (List<String> classement) 
        {
            this.classement = new List<String>();
            this.list_Orthogenie_pub = new List<Orthogenie>();
        }*/



        public List<Orthogenie> List_Orthogenie_pub
        {
            get => this.list_Orthogenie_pub;
            set => this.list_Orthogenie_pub = value;
        }

        public List<Revision> Classement
        {
            get => this.classement;
            set => this.classement = value;
        }

        public List<Revision> List_Revision
        {
            get => this.list_Revision;
            set => this.list_Revision = value;
        }

        public void get_classement()
        {
            this.classement = this.classement.OrderByDescending(a => a.note).ToList();

        }

        public void SaveDatabase()
        {
            //List<String> list_prenoms = new List<String>();
            //List<int> list_notes = new List<int>();

            string connectionString = "SERVER=127.0.0.1;DATABASE=orthogenie;UID=root;PASSWORD=";
            MySqlConnection connection = new MySqlConnection(connectionString);

            for (int i = 0; i <= Form2.classer.classement.Count() - 1; i++)
            {
                //list_prenoms.Add(Form2.classer.classement[i].prenom);
                //list_notes.Add(Form2.classer.classement[i].note);
                //Form2.connection;
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO scores(prenom,note) VALUES (@prenom, @note)";

                cmd.Parameters.AddWithValue("@prenom", Form2.classer.classement[i].prenom);
                cmd.Parameters.AddWithValue("@note", Form2.classer.classement[i].note);

                cmd.ExecuteNonQuery(); // Execute the request

                connection.Close();
            }
        }

        public void DeleteDatabase()
        {
            string connectionString = "SERVER=127.0.0.1;DATABASE=orthogenie;UID=root;PASSWORD=";
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM scores";
            cmd.ExecuteNonQuery(); // Execute the request
            connection.Close();
        }

        public string AfficherDatabase()
        /// https://dev.mysql.com/doc/dev/connector-net/8.0/html/T_MySql_Data_MySqlClient_MySqlDataReader.htm
        {
            string connectionString = "SERVER=127.0.0.1;DATABASE=orthogenie;UID=root;PASSWORD=";
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();


            cmd.CommandText = "SELECT * FROM scores";
            //cmd.ExecuteNonQuery();

            DbDataReader reader = cmd.ExecuteReader();

            string result = "Prenom\t\t\tScore\r\n";
            //string result = reader.ToString();
            while (reader.Read())
            {
                result = result + reader.GetString(0) + "\t\t\t" + reader.GetInt32(1) + "\r\n";
            }


            connection.Close();
            reader.Close();

            return result;
        }

        public void UpdateDabase(string prenom, int score)
        {
            string connectionString = "SERVER=127.0.0.1;DATABASE=orthogenie;UID=root;PASSWORD=";
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = "UPDATE scores SET prenom=@prenom,note=@note WHERE prenom=@prenom";
            cmd.Parameters.AddWithValue("@prenom", prenom);
            cmd.Parameters.AddWithValue("@note", score);

            cmd.ExecuteNonQuery();

            connection.Close();

        }

        public void Save(string prenom, int score)
        {
            List<string> prenoms = new List<string>();
            for (int i = 0; i <= this.classement.Count() - 1; i++)
            {
                prenoms.Add(this.classement[i].prenom);
            }

            if (prenoms.Contains(prenom) == false)
            {
                Revision new_joueur = new Revision("unknwon");
                new_joueur.prenom = prenom;
                new_joueur.note = score;
                this.classement.Add(new_joueur);
            }
            else
            {
                for (int i = 0; i <= this.classement.Count() - 1; i++)
                {
                    if (this.classement[i].prenom == prenom)
                    {
                        Console.WriteLine("There is a duplicate");
                        this.classement[i].note = this.classement[i].note + score;
                    }
                }
            }
        }

        /*public void Charge(string pathRelative="./file.txt")*/ // For Linux
        // public void Charge(string pathRelative="E:\\docs\\code\\csharp\\01_pendu-bescherelle\\lets-play\\file.txt")
        public void Charge(string pathRelative = "C:\\Users\\hela\\Documents\\code\\csharp\\lets-play_winform-with-git\\lets-play_winform\\lets-play_winform\\file.txt") // For Linux
        {
            /* string readText = File.ReadAllText(pathRelative);
             List<string> listReadText = readText.Split(',').ToList();
             Console.WriteLine(readText);
             Console.WriteLine(listReadText);*/

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
                        Orthogenie new_orthogenie = new Orthogenie();
                        new_orthogenie.solution = listSolution[i];
                        new_orthogenie.mot = listMot[i];
                        new_orthogenie.points = listScore[i];

                        Revision new_revision = new Revision();
                        new_revision.prenom = listPernom[i];
                        new_revision.note = listScore[i];

                        //this.list_Orthogenie_pub.Add(new_object); // Error -> System.NullReferenceException : 'Object reference not set to an instance of an object.'
                        this.list_Orthogenie.Add(new_orthogenie); // Error -> System.NullReferenceException : 'Object reference not set to an instance of an object.'
                        //this.list_Revision.Add(new_revision); // Error -> System.NullReferenceException : 'Object reference not set to an instance of an object.'
                        this.classement.Add(new_revision); // Error -> System.NullReferenceException : 'Object reference not set to an instance of an object.'

                        // Mysql

                    }
                }
            }
        }

    }


}
