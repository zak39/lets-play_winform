using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lets_play_winform
{
    public partial class Form4 : Form
    {
        public Database orthoDB = new Database("127.0.0.1", "root", "", "orthogenie");
        public DatabaseDataSet orthoDbDs = new DatabaseDataSet();

        public Form4()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //orthoDB.UpdateOneElementDatabase(textBox1.Text, Convert.ToInt32(textBox2.Text)); // whithout DataSet
            orthoDbDs.UpdateOneElementDatabaseDataSet(textBox1.Text, Convert.ToInt32(textBox2.Text)); // with DataSet
            this.Hide();
            Form2 window = new Form2();
            window.Show();
        }
    }
}
