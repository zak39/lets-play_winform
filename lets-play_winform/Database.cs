using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// nouveau ajout de using
using System.IO;   // pour importer un fichier
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;  // Necessaire pour obtenir les DataSet
using System.Data.SqlClient;
using MySql.Data;

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

        public void UpdateOneElementDatabase(string prenom, int score)
        {
            string connectionString = "SERVER="+ this.addrIPDB +";DATABASE="+ this.name +";UID="+ this.usernameDB +";PASSWORD="+ this.passwordDB +"";
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = "UPDATE scores SET prenom=@prenom,note=@note WHERE prenom=@prenom";
            cmd.Parameters.AddWithValue("@prenom", prenom);
            cmd.Parameters.AddWithValue("@note", score);

            cmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public class DatabaseDataSet
    {
        public DatabaseDataSet()
        {
        }

        // attributs
        string addrIPDBDS, usernameDBDS, passwordDBDS, name;

        // constructeur
        public DatabaseDataSet(string IP="127.0.0.1", string username="", string password="", string nameDatabaseDataSet="")
        {
            this.addrIPDBDS = IP;
            this.usernameDBDS = username;
            this.passwordDBDS = password;
            this.name = nameDatabaseDataSet;
        }

        // getters & setters
        public string AddrIPDBDS
        {
            get => this.addrIPDBDS;
            set => this.addrIPDBDS = value;
        }

        public string UsernameDBDS
        {
            get => this.usernameDBDS;
            set => this.usernameDBDS = value;
        }

        public string PasswordDBBDS
        {
            get => this.passwordDBDS;
            set => this.passwordDBDS = value;
        }

        public string Name
        {
            get => this.name;
            set => this.name = value;
        }

        // Methodes
        public string AfficherDatabaseDataSet()
        {

            string result = "";
            string connectionString = "SERVER=127.0.0.1;DATABASE=orthogenie;UID=root;PASSWORD=";
            MySqlConnection connection = new MySqlConnection(connectionString);

            string requestSelect = "SELECT * FROM scores ORDER BY note DESC";                       // Preparation de la requete
            MySqlCommand cmd = new MySqlCommand(requestSelect,connection);                          // Creation d'un objet MySqlCommand qui ...

            MySqlDataAdapter daScores = new MySqlDataAdapter();                                     // Creation de l'objet Data Adapter (DA) est l'interface entre la base de donnees et le data set. Le DA
            
            daScores.SelectCommand = cmd;                                                           // Permet d'executer le code SQL pour le Select seulement
          
            DataSet dsScores = new DataSet();                                                       // Creation de l'objet data set qui definit dans une zone memoire dans laquelle les donnees
                                                                                                    // peuvent etre lues ou modifier. Il est possible de stocker plusieurs tables (?)

            daScores.Fill(dsScores, "Scores");                                                      // La methode Fill permet de ...

            DataTable myDataTable = dsScores.Tables["scores"];                                      // Creation de l'objet DataTable qui permet de selectionner une table de la database

            // Ajoute le contenu de la table dans la variable result
            foreach (DataRow myDataRow in myDataTable.Rows)
            {
                result += myDataRow[("prenom")] + "\t\t\t" + myDataRow[("note")] + "\r\n";
            }

            return result;
        }

        public void SaveDatabaseDataSet(string prenom, int note)
        {
            string prenomInDB = "";

            //string connectionString = "SERVER=" + this.addrIPDB + ";DATABASE=" + this.name + ";UID=" + this.usernameDB + ";PASSWORD=" + this.passwordDB + "";
            //MySqlConnection connection = new MySqlConnection(connectionString);

            //connection.Open()C
            string connectionString = "SERVER=127.0.0.1;DATABASE=orthogenie;UID=root;PASSWORD=";
            MySqlConnection connection = new MySqlConnection(connectionString);

            string requestSelect = "SELECT prenom from scores WHERE prenom =@prenom";                       // Preparation de la requete

            MySqlCommand cmd = new MySqlCommand(requestSelect, connection);                          // Creation d'un objet MySqlCommand qui ...
            
            cmd.Parameters.Add("@prenom", prenom);
            
            MySqlDataAdapter daScores = new MySqlDataAdapter();                                     // Creation de l'objet Data Adapter (DA) est l'interface entre la base de donnees et le data set. Le DA

            daScores.SelectCommand = cmd;

            DataSet dsScores = new DataSet();

            daScores.Fill(dsScores, "Scores");

            DataTable myDataTable = dsScores.Tables["scores"];
            
            foreach (DataRow myDataRow in myDataTable.Rows) // doit remplacer la boucle while (reader.Readd())
            {
                prenomInDB += myDataRow[("prenom")];
            }

            
            string number = "", word = "";
            if(prenomInDB == "")
            {
                // Technique 1 - marche
           /*     MySqlDataAdapter daInsert = new MySqlDataAdapter();

                DataSet dsInsert = new DataSet();                                                       // Creation de l'objet data set qui definit dans une zone memoire dans laquelle les donnees


                daInsert.InsertCommand = new MySqlCommand("INSERT INTO scores(prenom,note) VALUES (@prenom, @note)",connection);
                daInsert.InsertCommand.Parameters.Add("@prenom", prenom);
                daInsert.InsertCommand.Parameters.Add("@note", note);
                
                connection.Open();
                daInsert.InsertCommand.ExecuteNonQuery(); // la connection doit etre ouverte et ferme mais on ne veut pas de ca
                connection.Close();
                number = note.ToString();
                word = prenom + " " + number;*/

                // technique 1Bis

                MySqlDataAdapter daInsert = new MySqlDataAdapter();

                DataSet dsInsert = new DataSet();                                                       // Creation de l'objet data set qui definit dans une zone memoire dans laquelle les donnees

                DataTable dtInsert = dsInsert.Tables.Add("scores");         // Init Data Table

                dtInsert.Columns.Add("prenom",typeof(string));
                dtInsert.Columns.Add("note",typeof(int));
                dtInsert.Rows.Add(prenom, note);

                daInsert.InsertCommand = new MySqlCommand("INSERT INTO scores(prenom,note) VALUES (@prenom, @note)", connection);
                daInsert.InsertCommand.Parameters.Add("@prenom", prenom);
                daInsert.InsertCommand.Parameters.Add("@note", note);

                daInsert.Update(dtInsert);

            }
            else
            {
                string requestSelect02 = "SELECT note from scores WHERE prenom =@prenom";                       // Preparation de la requete
                int noteCurrently = 0;
                //string number = "";
                MySqlCommand cmdSelect = new MySqlCommand(requestSelect02, connection);                          // Creation d'un objet MySqlCommand qui ...

                MySqlDataAdapter daUpdate = new MySqlDataAdapter();

                cmdSelect.Parameters.Add("@prenom", prenom);

                daUpdate.SelectCommand = cmdSelect;

                daUpdate.Fill(dsScores, "scores");
                
                DataTable myDataTable02 = dsScores.Tables["scores"];

                foreach( DataRow myDataRow02 in myDataTable02.Rows )
                {
                    number += myDataRow02[("note")];
                }
                
                noteCurrently = Convert.ToInt32(number);
                int newNote = noteCurrently + note; // mise a jour du score du jour

                // Preparation des commandes d'Update en SQL
                daUpdate.UpdateCommand = new MySqlCommand("UPDATE scores SET prenom=@prenom,note=@note WHERE prenom=@prenom",connection);
                daUpdate.UpdateCommand.Parameters.Add("@prenom", prenom);
                daUpdate.UpdateCommand.Parameters.Add("@note", newNote);

                // Je definis comment est ma table
                DataTable dtUpdate = new DataTable("scores");
                dtUpdate.Columns.Add("prenom", typeof(string));
                dtUpdate.Columns.Add("note", typeof(int));

                // Je definis mon Data Set en precisant que j'utilise la table dans dtUpdate
                DataSet dsUpdate = new DataSet();
                dsUpdate.Tables.Add(dtUpdate);

                // Je precise les lignes à utilisers et ce que je veux mettre ajour
                DataRow drUpdate = dtUpdate.NewRow();       // Init Data Row
                dtUpdate.Rows.Add(drUpdate);                // Ajout de ma mise a jour
                drUpdate.AcceptChanges();                   
                drUpdate.SetModified();

                daUpdate.Update(dtUpdate);

            }
        }

    }
}
