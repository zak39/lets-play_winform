using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lets_play_winform
{
    class Database
    {
        // constructeur a vide
        public Database() { }

        // attributs
        public string addrIPDB, usernameDB, passwordDB;

        // Constructeur
        public Database(string IP="127.0.0.1", string username="", string password="")
        {
            this.addrIPDB = IP;
            this.usernameDB = username;
            this.passwordDB = password;
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

        // Methodes
    }
}
