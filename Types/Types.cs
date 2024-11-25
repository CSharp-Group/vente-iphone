using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
            InitTypes();
            InitAnnes();
        }

        #endregion

        #region Initialisation

        public void InitTypes()
        {

            using (StreamReader sr = new StreamReader("C:\\Users\\ejalbert26\\source\\repos\\Annee2\\PROG1236 - C#\\Ventes iPhones\\Ventes iPhones\\Data\\Annees.data"))
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
                } 
                catch (Exception e) 
                {

                }
             
                
            }

        }

        public void InitAnnes()
        {

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
