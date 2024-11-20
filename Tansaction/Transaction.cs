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
        #region Champs Privés
        private DateTime datePaiement;
        #endregion

        #region Declaration des tableaux

        private string[] tMarques;
        private string[] tModel;
        private decimal[,] tPrix;

        private int id;
        private string nom;
        private string prenom;
        private string adresse;
        private string codePostal;
        private string telephone;
        private string type;
        private string annee;
        private DateTime dateLivraison;
        private string marque;
        private string modele;
        private decimal prix;

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

        private void InitPrix()
        {
            tPrix = new decimal[3, 3]
            {
                { 989.99M, 1109.99M, 1301.99M },
                { 1087.99M, 1233.99M, 1409.99M },
                { 1100.99M, 1301.99M, 1498.99M }
            };
        }

        #endregion

        #region Propriétés

        public int Id
        {
            get { return id; }
        }

        public string Nom
        {
            get { return nom; }
            set { nom = value.Trim(); }
        }

        public string Prenom
        {
            get { return prenom; }
            set { prenom = value.Trim(); }
        }

        public string Adresse
        {
            get { return adresse; }
            set { adresse = value.Trim(); }
        }

        public string CodePostal
        {
            get { return codePostal; }
            set { codePostal = value.Trim(); }
        }

        public string Telephone
        {
            get { return telephone; }
            set { telephone = value.Trim(); }
        }

        public string Type
        {
            get { return type; }
            set { type = value.Trim(); }
        }

        public string Modele
        {
            get { return modele; }
            set { modele = value.Trim(); }
        }

        public DateTime DateLivraison
        {
            get { return dateLivraison; }
            set { dateLivraison = value; }
        }

        public decimal Prix
        {
            get { return prix; }
            set { prix = value; }
        }

        #endregion

        #region Constructeur

        public Transaction()
        {
            InitMarques();
            InitModel();
            InitPrix();
        }

        /// <summary>
        /// Constructeur avec paramètres pour initialiser une transaction.
        /// </summary>
        /// <param name="nom">Nom du client</param>
        /// <param name="prenom">Prénom du client</param>
        /// <param name="adresse">Adresse du client</param>
        /// <param name="codePostal">Code postal du client</param>
        /// <param name="telephone">Numéro de téléphone du client</param>
        /// <param name="type">Type d'appareil</param>
        /// <param name="annee">Année de l'appareil</param>
        /// <param name="marque">Marque d'appareil</param>
        /// <param name="modele">Modèle d'appareil</param>
        /// <param name="dateLivraison">Date de livraison de la commande</param>
        /// <param name="prix">Prix de l'appareil</param>
        /// 
        public Transaction(string nom, string prenom, string adresse, string codePostal, string telephone,
                           string type, string annee, string marque, string modele, DateTime dateLivraison, decimal prix)
        {
            this.nom = nom;
            this.prenom = prenom;
            this.adresse = adresse;
            this.codePostal = codePostal;
            this.telephone = telephone;
            this.type = type;
            this.annee = annee;
            this.dateLivraison = dateLivraison;
            this.marque = marque;
            this.modele = modele;
            this.prix = prix;
            InitMarques();
            InitModel();
            InitPrix();
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
            if (marque >= 0 && marque < tMarques.Length)
            {
                if (model >= 0 && model <= tModel.Length)
                {
                    return tPrix[marque, model];
                }
                else
                    throw new ArgumentOutOfRangeException("Erreur: Model invalide");
            }
            else
                throw new ArgumentOutOfRangeException("Erreur: Marque invalide");
        }

        public decimal GetPrix(string marque, string model)
        {
            int positionMarque = Array.IndexOf(tMarques, marque);
            int positionModel = Array.IndexOf(tModel, model);

            if (positionMarque >= 0 && positionMarque < tMarques.Length)
            {
                if (positionModel >= 0 && positionModel < tModel.Length)
                {
                    return tPrix[positionMarque, positionModel];
                }
                else
                    throw new ArgumentException("Erreur: Model invalide");
            }
            else
                throw new ArgumentException("Erreur: Marque invalide");

        }

        #endregion

        #region Enregistrer

        /// <summary>
        /// Enregistrer la transaction (affichage des informations)
        /// </summary>
        public void Enregistrer()
        {
            Console.WriteLine($"Transaction de {Nom} {Prenom}:");
            Console.WriteLine($"Adresse: {Adresse}, {CodePostal}");
            Console.WriteLine($"Téléphone: {Telephone}");
            Console.WriteLine($"Type: {Type}, Modèle: {Modele}");
            Console.WriteLine($"Date de livraison: {DateLivraison.ToShortDateString()}");
            Console.WriteLine($"Prix: {Prix:C}");
        }

        /// <summary>
        /// Enregistrer la transaction avec des paramètres
        /// </summary>
        public void Enregistrer(string nom, string prenom, string adresse, string codePostal, string telephone,
                                string type, string modele, DateTime dateLivraison, decimal prix)
        {
            this.Nom = nom;
            this.Prenom = prenom;
            this.Adresse = adresse;
            this.CodePostal = codePostal;
            this.Telephone = telephone;
            this.Type = type;
            this.Modele = modele;
            this.DateLivraison = dateLivraison;
            this.Prix = prix;

            Enregistrer();
        }

        #endregion
    }
}
