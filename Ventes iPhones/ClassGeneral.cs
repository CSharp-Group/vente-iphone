using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentesIPhones
{
    #region XML

    /// <summary>
    /// Class General
    /// </summary>

    #endregion
    internal class ClassGeneral
    {
        #region Initialisation

        public ClassGeneral()
        {
            InitMessagesErreurs();
        }

        #endregion

        #region Code d'erreurs
        public enum CodeErreurs
        {
            ceOutOfMemory,
            ceException
        }

        #endregion

        #region Message d'erreurs

        public static string[] tMessagesErreursStr = new string[2];

        public static void InitMessagesErreurs()
        {
            tMessagesErreursStr[(int)CodeErreurs.ceOutOfMemory] = "Plus de mémoire";
            tMessagesErreursStr[(int)CodeErreurs.ceException] = "Une erreur s'est produite. Veuillez contacter le programmeur.";
        }

        #endregion
    }
}
