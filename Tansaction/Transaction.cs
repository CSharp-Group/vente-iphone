using System;
using System.Net.Mime;

namespace TransactionNS
{
    #region XML

    /// <summary>
    /// Class Transaction
    /// </summary>

    #endregion

    public class Transaction
    {
        #region Declaration des tableaux

        private string[] tMarques;
        private string[] tModel;
        private decimal[,] tPrix;

        #endregion

        #region Initialisation des tableaux

        private void InitMarques()
        {
            tMarques = new string[3] { "iPhone 14", "iPhone 15", "iPhone 16" };
        }

        private void InitModel()
        {
            tModel = new string[3] { "Regular", "Pro", "Pro Max" };
        }

        private void initPrix()
        {
            tPrix = new decimal[3, 3]
            {
                { 989.99M, 1109.99M, 1301.99M },
                { 1087.99M, 1233.99M, 1409.99M },
                { 1100.99M, 1301.99M, 1498.99M }
            };
        }

        #endregion

        #region Constructeur

        public Transaction()
        {
            InitMarques();
            InitModel();
            initPrix();
        }

        #endregion

        #region Marques

        public string[] GetMarques()
        {
            return tMarques;
        }

        #endregion

        #region Models

        public string[] getModel()
        {
            return tModel;
        }

        #endregion

        #region Prix

        public decimal GetPrix(int marque, int model)
        {
            if (marque >= 0 && marque <= 2)
            {
                if (model >= 0 && model <= 2)
                {
                    return tPrix[marque, model];
                }
                else
                    throw new ArgumentOutOfRangeException("Erreur");
            }
            else 
                throw new ArgumentOutOfRangeException("Erreur");
        }

        public decimal GetPrix(string marque, string model)
        {
            int positionMarque = Array.IndexOf(tMarques, marque);
            int positionModel = Array.IndexOf(tModel, model);

            if (positionMarque >= 0 && positionMarque <= 2)
            {
                if (positionModel >= 0 && positionModel <= 2)
                {
                    return tPrix[positionMarque, positionModel];
                }
                else
                    throw new ArgumentException("Erreur");
            }
            else
                throw new ArgumentException("Erreur");
        }

        #endregion
    }
}
