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
// using System.IO;   // pour importer un fichier
// using MySql.Data.MySqlClient;
// using System.Data.Common;

namespace lets_play_winform
{
    //public enum Mode { pendu, bescherelle }; // On a cree une variable qui est globale dans toutes les classes et fonctions si

    public partial class Form1 : Form
    {
        public Orthogenie monjeu;   // J'ai eu l'idée mais c'est Arthur qui m'a indique de declarer mon constructeur d'objet de cette facon
        public Revision mode_revision;   // J'ai eu l'idée mais c'est Arthur qui m'a indique de declarer mon constructeur d'objet de cette facon
        public Database orthoDb = new Database("127.0.0.1","root","","orthogenie");
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
            orthoDb.SaveDatabase(prenom, score);
            //Form2.classer.Save(prenom, score);
            this.Hide();
            Form2 window = new Form2();
            window.Show();

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
