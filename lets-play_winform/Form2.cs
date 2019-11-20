using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// mysql
using MySql.Data.MySqlClient;


namespace lets_play_winform
{
    public partial class Form2 : Form
    {
        public static string motATrouve;
        public static Mode mode_de_jeu;
        public static Revision mode_revision;
        public static bool state_checkBox1;
        public static Classe classer = new Classe();
        public Database orthoDb = new Database("127.0.0.1", "root", "", "orthogenie");

        // Mysql
        // public static MySqlConnection connection;

        public Form2()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                motATrouve = textBox1.Text;
                mode_de_jeu = Mode.pendu;

                this.Hide();
                Form1 window = new Form1();
                window.Show();
            }
            else
            {
               // Ouvrir message d'alerte
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                motATrouve = textBox1.Text;
                mode_de_jeu = Mode.bescherelle;

                this.Hide();
                Form1 window = new Form1();
                window.Show();

            }
            else
            {
                // Ouvrir un message d'alerte
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox2.AppendText("Prenom\t\t\tScore" + "\r\n");
            textBox2.AppendText(orthoDb.AfficherDatabase());          
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Enabled = true;
            state_checkBox1 = checkBox1.Enabled;
        }

        private void Form2_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            orthoDb.ChargeDatabase();
            textBox2.Clear();
            textBox2.AppendText("Prenom\t\t\tScore" + "\r\n");
            textBox2.AppendText(orthoDb.AfficherDatabase());
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            orthoDb.DeleteAllDatabase();
            textBox2.Clear();
            textBox2.AppendText("Prenom\t\t\tScore" + "\r\n");
            textBox2.AppendText(orthoDb.AfficherDatabase());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 window = new Form3();
            window.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 window = new Form4();
            window.Show();
        }
    }
}
