using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// nouveau ajout de using
using System.IO;   // pour importer un fichier
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace lets_play_winform
{
    //public enum Mode { pendu, bescherelle }; // On a cree une variable qui est globale dans toutes les classes et fonctions si

    public partial class Form1 : Form
    {
        public Orthogenie monjeu;   // J'ai eu l'idée mais c'est Arthur qui m'a indique de declarer mon constructeur d'objet de cette facon
        public Revision mode_revision;   // J'ai eu l'idée mais c'est Arthur qui m'a indique de declarer mon constructeur d'objet de cette facon
        //public MySqlConnection connection;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            monjeu = new Orthogenie();
            if (Form2.state_checkBox1)
            {
                mode_revision = new Revision("unkwown");
            }
            monjeu.solution = Form2.motATrouve;
            monjeu.motMystere(monjeu.solution);
            monjeu.mode = Form2.mode_de_jeu;
            monjeu.nbDeCoup = monjeu.mot.Length;

            textBox1.Text = monjeu.mot;
            textBox1.Enabled = false;
            textBox3.Text = monjeu.mot;

            // string connectionString = "SERVER=127.0.0.1;DATABASE=orthogenie;UID=root;PASSWORD=";
            // MySqlConnection connection = new MySqlConnection(connectionString);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //motATrouve = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            monjeu.joue(textBox2.Text[0]);
            textBox2.Text = "";
            textBox3.Text = monjeu.mot;
            monjeu.nbDeCoup -= 1;

            if (monjeu.nbDeCoup == 0)
            {
                textBox1.Text = "Perdu ! Vous n'avez pas gagne mais vous avez " + monjeu.points + " points !";
                button2.Enabled = false;
                textBox1.Enabled = false;
            }

            if(monjeu.victoire)
            {
                textBox1.Text = "Victoire ! Vous avez gagne avec "+ monjeu.points +" points !";
                button2.Enabled = false;
                textBox1.Enabled = false;
                if (Form2.state_checkBox1)
                {
                    label3.Visible = true;
                    label4.Visible = true;

                    textBox4.Visible = true;
                    textBox5.Visible = true;
                    textBox5.Text = Convert.ToString(monjeu.points);

                    button4.Visible = true;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            /*label1.Text = Form2.motATrouve;*/

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 window = new Form2();
            window.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //mode_revision.prenom = textBox4.Text;
            //mode_revision.note = monjeu.points;
            string prenom = textBox4.Text;
            int score = monjeu.points;
            //Form2.classer.classement.Add(mode_revision);
            Form2.classer.Save(prenom, score);
            this.Hide();
            Form2 window = new Form2();
            window.Show();

        }
    }

    interface Int_Jeu
    {
        void joue(char caractere);
    }

    public class Orthogenie : Int_Jeu
    /// This class allows to game the "pendu" or "bescherelle".
    {
        // constructeur
        public string solution, mot;
        public bool victoire;
        public Orthogenie() { }   // Constructeur vide
        //public Orthogenie( int points ) { }   // Constructeur vide
        public Orthogenie(string solution, string mot, bool victoire) { this.solution = ""; this.mot = ""; this.victoire = false; }
        // public Orthogenie (string solution) { this.solution = System.String.Empty; }

        public int points, nbDeCoup;
        public Orthogenie(int points, int nbDeCoup) { this.points = 0; this.nbDeCoup = 0; }

        public Orthogenie(int points) { this.points = 0; } // Si je n'ai pas cette ligne, j'aurais un probleme avec cette ligne ":base(points)" dans la classe enfant "Revision"

        public Mode mode;
        public Orthogenie(Mode mode) { this.mode = Mode.pendu; }


        // getters and setters
        public string Solution
        {
            get => this.solution;
            set => this.solution = value;
        }

        public int Points
        {
            get => this.points;
            set => this.points = value;
        }

        public int NbDeCoup
        {
            get => this.nbDeCoup;
            set => this.nbDeCoup = value;
        }

        public string Mot
        {
            get => this.mot;
            set => this.mot = value;
        }

        public bool Victoire
        {
            get => this.victoire;
            set => this.victoire = value;
        }

        // Methodes
        public void motMystere(string monMot)
        /// Initialize the word mystery with stars and it will print to end user.
        {
            int compteur;

            for (compteur = 0; compteur <= this.solution.Length - 1; compteur++)
            {
                this.mot = this.mot + "*";
            }

        }

        public void joue(char caractere)
        /// This method allows to execute the "pendu" or "bescherelle" game. 
        {
            // Jeu du pendu
            int compteur;
            StringBuilder motMystere = new StringBuilder(this.mot);

            for (compteur = 0; compteur <= this.solution.Length - 1; compteur++)
            {
                if (this.mode == Mode.pendu)
                {
                    if (this.solution[compteur] == caractere)
                    {
                        motMystere[compteur] = caractere;
                        this.mot = motMystere.ToString();
                        this.points = this.points + 1;
                    }
                }
                if (this.mode == Mode.bescherelle)
                {

                    // Console.WriteLine("-------");
                    // Console.WriteLine(this.solution[compteur]);
                    // Console.WriteLine(this.mot[compteur]);
                    // Console.WriteLine(motMystere[compteur]);
                    // Console.WriteLine(caractere);

                    // 'g' == 'g' && 'g' =! '*'
                    // 'g' == 'i' && '*' =! '*'
                    // 1 && 1
                    // if ( this.solution[compteur] == caractere && this.mot[compteur] != '*' )

                    if (this.mot[compteur] == '*')
                    {
                        if (this.solution[compteur] == caractere)
                        {
                            motMystere[compteur] = caractere;
                            this.mot = motMystere.ToString();
                            this.points = this.points + 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

            }

            if(this.mot.Contains('*'))
            {
                this.victoire = false;
            }
            else
            {
                this.victoire = true;
            }

        }

    }

    public class Revision : Orthogenie
    {

        // constructeur
        public string solution, mot;
        public Revision() { }
        public Revision(string solution, string mot, bool victoire)
        :base(solution, mot, victoire)
        {
        }

        public int points, note;
        public Revision(int points, int note)
        :base(points)
        {
        }

        public string prenom;
        public Revision(string prenom) { this.prenom = "unknown"; }

        public string Prenom
        {
            get => this.prenom;
            set => this.prenom = value;
        }


        public int Note
        {
            get => this.note;
            set => this.note = value;
        }
    }

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
            while(reader.Read())
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
            for (int i = 0; i <= this.classement.Count()-1; i++)
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
        public void Charge(string pathRelative = "C:\\Users\\hela\\Documents\\code\\csharp\\lets-play_winform\\lets-play_winform\\lets-play_winform\\file.txt") // For Linux
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
