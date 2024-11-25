using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TypesNS
{
    public class Types
    {
        #region Declaration 

        private string[] tTypes = new string[40];
        private string[] tAnnees = new string[40];

        #endregion

        #region Enumeration

        public enum CodeTypes
        {
            Types,
            Annees
        }

        #endregion

        #region Constructeur

        public Types()
        {
            InitAnnes();
            InitTypes();
        }

        #endregion

        #region Initialisation

        private void InitAnnes()
        {

            using (StreamReader sr = new StreamReader("C:\\Users\\ejalbert26\\Desktop\\Ventes iPhones\\Ventes iPhones\\Data\\Annees.data"))
            {
                try
                {
                    string line = sr.ReadLine();
                    int i = 0;

                    while (line != null)
                    {
                        tAnnees[i] = line;
                        line = sr.ReadLine();
                        i++;
                    }

                    Array.Resize(ref tAnnees, i);

                }
                catch (FormatException)
                {
                    throw new FormatException("Impossible de convertir le prix en valeur réelle.");
                }
                catch (FileNotFoundException)
                {
                    throw new FileNotFoundException("Le fichier des prix n’est pas disponible.");
                }
                catch (Exception)
                {
                    throw new Exception("Erreur indéterminée dans la lecture des prix.");
                }

            }
        }

        private void InitTypes()
        {

            using (StreamReader sr = new StreamReader("C:\\Users\\ejalbert26\\Desktop\\Ventes iPhones\\Ventes iPhones\\Data\\Types.data"))
            {
                try
                {
                    string line = sr.ReadLine();
                    int i = 0;

                    while (line != null)
                    {
                        tTypes[i] = line;
                        line = sr.ReadLine();
                        i++;
                    }

                    Array.Resize(ref tTypes, i);

                    Console.WriteLine(tTypes[0]);
                    Console.WriteLine(tTypes[1]);

                }
                catch (FormatException)
                {
                    throw new FormatException("Impossible de convertir le prix en valeur réelle.");
                }
                catch (FileNotFoundException)
                {
                    throw new FileNotFoundException("Le fichier des prix n’est pas disponible.");
                }
                catch (Exception)
                {
                    throw new Exception("Erreur indéterminée dans la lecture des prix.");
                }

            }

        }



        //public void InitTypes()
        //{
        //    tTypes = new string[2] { "iPhone", "iPad" };
        //}

        //public void InitAnnes()
        //{
        //    tAnnees = new string[3] { "2022", "2023", "2024" };
        //}

        #endregion

        #region getTypes

        public string[] GetTypes(CodeTypes type)
        {
            switch (type)
            {
                case CodeTypes.Types:
                    return tTypes;
                case CodeTypes.Annees:
                    return tAnnees;
                default:
                    throw new ArgumentOutOfRangeException("Erreur");
            }
        }

        #endregion
    }
}
