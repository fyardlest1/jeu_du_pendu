using System;
using System.Collections.Generic;
using System.IO;
using AsciiArt;

namespace jeu_du_pendu
{
    class Program
    {
        // FONCTION POUR AFFICHER LE MOT PAR DES UNDERSCORES
        static void AfficherMot(string mot, List<char> lettres)
        {
            // _ _ _ _ _ _
            for (int i = 0; i < mot.Length; i++)
            {
                char lettre = mot[i];
                if (lettres.Contains(lettre))
                {
                    Console.Write($"{lettre} ");
                }
                else
                {
                    Console.Write("_ ");
                }
                
            }
            Console.WriteLine();
        }

        // TOUTES LES LETTRES SONT DEVINEES
        static bool ToutesLettresDevivees(string mot, List<char> lettres)
        {
            foreach (var lettre in lettres)
            {
                mot = mot.Replace(lettre.ToString(), "");
            }

            if (mot.Length == 0)
            {
                return true;
            }
            return false;
        }

        // FONCTION POUR RECUPERER UNE LETTRE
        static char DemanderUneLettre(string message = "Rentrer une lettre: ")
        {
            // rentrer une lettre
            while (true)
            {
                Console.Write(message);
                string reponse = Console.ReadLine();

                if (reponse.Length == 1)
                {
                    reponse = reponse.ToUpper();
                    return reponse[0];
                }
                Console.WriteLine("ERREUR: Vous devez rentrer une lettre pour continuer.");
            }            
        }

        // FONCTION POUR DEVINER LE MOT
        static void DevinerMot(string mot)
        {
            var lettreUtilisateur = new List<char>();
            var lettresExclues = new List<char>();

            const int NBRE_VIES = 6;
            int viesRestantes = NBRE_VIES;

            while (viesRestantes > 0)
            {
                Console.WriteLine(Ascii.PENDU[NBRE_VIES - viesRestantes]);
                Console.WriteLine();

                AfficherMot(mot, lettreUtilisateur);                
                Console.WriteLine();

                var lettre = DemanderUneLettre();
                Console.Clear();

                if (mot.Contains(lettre))
                {
                    Console.WriteLine("Bravo, cette lettre est dans le mot");
                    lettreUtilisateur.Add(lettre);

                    // Quand on gagne
                    if (ToutesLettresDevivees(mot, lettreUtilisateur))
                    {
                        break;
                    }
                }
                else
                {
                    if (!lettresExclues.Contains(lettre))
                    {
                        viesRestantes--;
                        lettresExclues.Add(lettre);
                    }
                                       
                    Console.WriteLine($"Vies restante : {viesRestantes}");
                }

                if (lettresExclues.Count > 0)
                {
                    Console.WriteLine($"Le mot ne contient pas les lettres : {String.Join(", ", lettresExclues)}");
                }                
                Console.WriteLine();
            }

            Console.WriteLine(Ascii.PENDU[NBRE_VIES - viesRestantes]);

            if (viesRestantes == 0)
            {
                Console.WriteLine($"GAME OVER! Le mot était : {mot}");
            }
            else
            {
                AfficherMot(mot, lettreUtilisateur);
                Console.WriteLine();
                Console.WriteLine("GAGNE ! BON BOULOT");
            }
        }

        // CHARGER LES MOTS EN UTILISANT UN FICHIER
        static string[] ChargerLesMots(string nomDuFichier)
        {
            try
            {
                return File.ReadAllLines(nomDuFichier);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur de lecture du fichier : {nomDuFichier} ({ex.Message})");
            }

            return null;
        }
        // DEMANDE A L'UTILISATEUR DE JOUER A NOUVEAU
        static bool JouerEncore()
        {
            char reponse = DemanderUneLettre("Voulez-vous rejouer (o/n)? ");

            if ((reponse == 'o' || reponse == 'O'))
            {
                return true;
            }
            else if ((reponse == 'n' || reponse == 'N'))
            {
                return false;
            }
            else
            {
                Console.WriteLine("Erreur : Vous devez repondre avec o ou n");
                return JouerEncore();
            }
        }

        static void Main(string[] args)
        {
            var mots = ChargerLesMots("mots.txt");

            if ((mots == null || mots.Length == 0))
            {
                Console.WriteLine("Désolé, la liste de mots est vide.");
            }
            else
            {
                while (true)
                {
                    Random rand = new Random();
                    int motAleatoire = rand.Next(mots.Length);

                    string mot = mots[motAleatoire].Trim().ToUpper();
                    DevinerMot(mot);

                    if (!JouerEncore())
                    {
                        break;
                    }
                    Console.Clear();
                }
                Console.WriteLine("Merci et à bientôt!");                
            }

            Console.WriteLine();
            
        }
    }
}
