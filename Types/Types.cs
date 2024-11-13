using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypesNS
{
    public class Types
    {
        #region Declaration 

        private string[] tTypes;
        private string[] tAnnees;

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
            tTypes = new string[2] { "iPhone", "iPad" };
        }

        public void InitAnnes()
        {
            tAnnees = new string[3] { "2022", "2023", "2024" };
        }

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
