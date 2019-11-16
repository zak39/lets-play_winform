using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lets_play_winform
{

    public enum Mode { pendu, bescherelle };

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

            if (this.mot.Contains('*'))
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
        : base(solution, mot, victoire)
        {
        }

        public int points, note;
        public Revision(int points, int note)
        : base(points)
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

}
