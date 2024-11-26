using System;
using System.IO;
using System.Globalization;
using System.Net.Mime;
using System.Reflection;
using System.Security.AccessControl;

namespace TransactionNS
{
    #region XML

    /// <summary>
    /// Class Transaction
    /// </summary>

    #endregion

    public class Transaction
    {
        #region Declaration des champs prives

        public DateTime datePaiement;


        #endregion

        #region Declarations des enumerations

        public enum CodeErreurs
        {
            nomObligatoire,
            prenomObligatoire,
            addressObligatoire,
            codePostalObligatoire,
            codePostalInvalide,
            dateLivraisonInvalide,
            erreurIndeterminee,
            prixInvalide,
            anneeObligatoire,
            marqueInvalide,
            marqueObligatoire,
            dateInvalide,
            telephoneObligatoire,
            telephoneInvalide,
            modelInvalide,
            modelObligatoire,
            typeObligatoire,
            typeInvalide
        }

        private string[] tMessagesErrurs;

        private void InitMessagesErreurs()
        {
            tMessagesErrurs = new string[Enum.GetValues(typeof(CodeErreurs)).Length];// initialisation
            tMessagesErrurs[(int)CodeErreurs.nomObligatoire] = "Le nom est obligatoire.";
            tMessagesErrurs[(int)CodeErreurs.prenomObligatoire] = "Le prenom est obligatoire.";
            tMessagesErrurs[(int)CodeErreurs.addressObligatoire] = "L'adresse est obligatoire.";
            tMessagesErrurs[(int)CodeErreurs.anneeObligatoire] = "L'annee est obligatoire";
            tMessagesErrurs[(int)CodeErreurs.codePostalObligatoire] = "Le code postal est obligatoire.";
            tMessagesErrurs[(int)CodeErreurs.codePostalInvalide] = "Le code postal est invalide.";
            tMessagesErrurs[(int)CodeErreurs.marqueInvalide] = "La marque est invalide";
            tMessagesErrurs[(int)CodeErreurs.marqueObligatoire] = "La marque est obligatoire";
            tMessagesErrurs[(int)CodeErreurs.modelInvalide] = "Le modèle est invalide.";
            tMessagesErrurs[(int)CodeErreurs.modelObligatoire] = "Le modèle est obligatoire.";
            tMessagesErrurs[(int)CodeErreurs.dateLivraisonInvalide] = "La date de livraison est invalide.";
            tMessagesErrurs[(int)CodeErreurs.prixInvalide] = "Prix invalide";
            tMessagesErrurs[(int)CodeErreurs.erreurIndeterminee] = "Erreur indeterminee";
            tMessagesErrurs[(int)CodeErreurs.dateInvalide] = "La date doit se situer dans les 15 jours precedant ou suivant de la date courante";
            tMessagesErrurs[(int)CodeErreurs.telephoneInvalide] = "Numero de telephone invalide";
            tMessagesErrurs[(int)CodeErreurs.telephoneObligatoire] = "Le numero de telephone est obligatoire";
            tMessagesErrurs[(int)CodeErreurs.typeInvalide] = "Type invalide";
            tMessagesErrurs[(int)CodeErreurs.typeObligatoire] = "Le type est obligatoire";

        }

        #endregion

        #region Declaration des tableaux

        private string[] tMarques = new string[20];
        private string[] tModel = new string[20];
        private decimal[,] tPrix = new decimal[20, 20];

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
            try
            {
                using (StreamReader sr = new StreamReader("C:\\Users\\ejalbert26\\Desktop\\Ventes iPhones\\Ventes iPhones\\Data\\Marques.data"))
                {
                    string ligne = sr.ReadLine();
                    int nombre = int.Parse(ligne);

                    Array.Resize(ref tMarques, nombre);   

                    for (int i = 0; i < nombre; i++)
                    {
                        tMarques[i] = sr.ReadLine();
                    }
                }
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

        private void InitModel()
        {
            try
            {
                using (StreamReader sr = new StreamReader("C:\\Users\\ejalbert26\\Desktop\\Ventes iPhones\\Ventes iPhones\\Data\\Modeles.data"))
                {
                    string ligne = sr.ReadLine();
                    int nombre = int.Parse(ligne);

                    Array.Resize(ref tModel, nombre);

                    for (int i = 0; i < nombre; i++)
                    {
                        tModel[i] = sr.ReadLine();
                    }
                }
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

        private void InitPrix()
        {
            try
            {
                using (StreamReader sr = new StreamReader("C:\\Users\\ejalbert26\\Desktop\\Ventes iPhones\\Ventes iPhones\\Data\\Prix.data"))
                {
                    int rangee = tMarques.Length - 1;
                    int colonne = tModel.Length - 1;

                    for (int ran = 0; ran <= tMarques.Length - 1; ran++)
                        for (int col = 0; col <= tModel.Length - 1; col++)
                            tPrix[ran, col] = decimal.Parse(sr.ReadLine(), CultureInfo.CreateSpecificCulture("en-CA"));

                    ResizeArray(tPrix, rangee, colonne);   

                }
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

        #endregion

        #region ResizeArray

        T[,] ResizeArray<T>(T[,] original, int rows, int cols)
        {
            var newArray = new T[rows, cols];
            int minRows = Math.Min(rows, original.GetLength(0));
            int minCols = Math.Min(cols, original.GetLength(1));
            for (int i = 0; i < minRows; i++)
                for (int j = 0; j < minCols; j++)
                    newArray[i, j] = original[i, j];
            return newArray;
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
            set 
            {
                if (value != null)
                {
                    value = value.Trim();

                    if (value != string.Empty)
                        nom = value;
                    else
                        throw new ArgumentException(tMessagesErrurs[(int)CodeErreurs.nomObligatoire]);
                }
                else
                    throw new ArgumentNullException(tMessagesErrurs[(int)CodeErreurs.nomObligatoire]);
            }
        }

        public string Prenom
        {
            get { return prenom; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();

                    if (value != string.Empty)
                        prenom = value;
                    else
                        throw new ArgumentException(tMessagesErrurs[(int)CodeErreurs.prenomObligatoire]);
                }
                else
                    throw new ArgumentNullException(tMessagesErrurs[(int)CodeErreurs.prenomObligatoire]);
            }
        }

        public string Adresse
        {
            get { return adresse; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();

                    if (value != string.Empty)
                        adresse = value;
                    else
                        throw new ArgumentException(tMessagesErrurs[(int)CodeErreurs.addressObligatoire]);
                }
                else
                    throw new ArgumentNullException(tMessagesErrurs[(int)CodeErreurs.addressObligatoire]);
            }
        }

        public string Annee
        {
            get { return annee;  }
            set
            {
                if (value != null)
                {
                    value = value.Trim();

                    if (value != string.Empty)
                        annee = value;
                    else
                        throw new ArgumentException(tMessagesErrurs[(int)CodeErreurs.anneeObligatoire]);
                }
                else
                    throw new ArgumentNullException(tMessagesErrurs[(int)CodeErreurs.anneeObligatoire]);
            }
        }

        public string CodePostal
        {
            get { return codePostal; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();

                    if (value != string.Empty)
                        codePostal = value;
                    else
                        throw new ArgumentException(tMessagesErrurs[(int)CodeErreurs.codePostalInvalide]);
                }
                else
                    throw new ArgumentNullException(tMessagesErrurs[(int)CodeErreurs.codePostalObligatoire]);
            }
        }

        public string Telephone
        {
            get { return telephone; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();

                    if (value != string.Empty)
                        telephone = value;
                    else
                        throw new ArgumentException(tMessagesErrurs[(int)CodeErreurs.telephoneInvalide]);
                }
                else
                    throw new ArgumentNullException(tMessagesErrurs[(int)CodeErreurs.telephoneObligatoire]);
            }
        }

        public string Type
        {
            get { return type; }
            set 
            {
                if (value != null)
                {
                    value = value.Trim();

                    if (value != string.Empty)
                        type = value;
                    else
                        throw new ArgumentException(tMessagesErrurs[(int)CodeErreurs.typeInvalide]);
                }
                else
                    throw new ArgumentNullException(tMessagesErrurs[(int)CodeErreurs.typeObligatoire]);
            }
        }

        public string Marque
        {
            get { return marque; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();

                    if (Array.IndexOf(tMarques, value) != -1)
                        marque = value;
                    else
                        throw new ArgumentNullException(tMessagesErrurs[(int)CodeErreurs.marqueInvalide]);
                }
                else
                    throw new ArgumentNullException(tMessagesErrurs[(int)CodeErreurs.marqueObligatoire]);
            }
        }
        

        public string Modele
        {
            get { return modele; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();

                    if (Array.IndexOf(tModel, value) != -1)
                        modele = value;
                    else
                        throw new ArgumentNullException(tMessagesErrurs[(int)CodeErreurs.marqueInvalide]);
                }
                else
                    throw new ArgumentNullException(tMessagesErrurs[(int)CodeErreurs.marqueObligatoire]);
            }
        }

        public DateTime DateLivraison
        {
            get { return dateLivraison; }
            set
            {
                if (DateTime.Now.AddDays(-15) <= value && value <= DateTime.Now.AddDays(15))
                {
                    dateLivraison = value;
                    datePaiement = dateLivraison.AddDays(30);
                }
                else
                    throw new ArgumentOutOfRangeException(tMessagesErrurs[(int)CodeErreurs.dateInvalide]);
            }
        }

        public decimal Prix
        {
            get { return prix; }
            set
            {
                if (value > 0)
                {
                    if (marque != String.Empty && modele != String.Empty)
                    {
                        int positionMarque = Array.IndexOf(tMarques, marque);
                        int positionModel = Array.IndexOf(tModel, modele);

                        if (tPrix[positionMarque, positionModel] == value)
                            prix = value;
                        else
                            throw new ArgumentException(tMessagesErrurs[(int)CodeErreurs.prixInvalide]);
                    }
                    else 
                        throw new ArgumentNullException(tMessagesErrurs[(int)CodeErreurs.prixInvalide]);
                }
                else
                    throw new ArgumentOutOfRangeException(tMessagesErrurs[(int)CodeErreurs.prixInvalide]);
            }
        }

        #endregion

        #region Constructeur

        public Transaction()
        {
            InitMarques();
            InitModel();
            InitPrix();
            InitMessagesErreurs();
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

            Enregistrer();
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
                                string type, string modele, DateTime dateLivraison, decimal prix, string marque)
        {
            Console.WriteLine(prix);
            this.Nom = nom;
            this.Prenom = prenom;
            this.Adresse = adresse;
            this.CodePostal = codePostal;
            this.Telephone = telephone;
            this.Type = type;
            this.Modele = modele;
            this.DateLivraison = dateLivraison;
            this.Marque = marque;
            this.Prix = prix;
            InitMarques();
            InitModel();
            InitPrix();
            Enregistrer();
        }

        #endregion
    }
}
