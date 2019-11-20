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

    }


}
