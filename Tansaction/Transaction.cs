using System;
using System.IO;
using System.Globalization;
using System.Net.Mime;
using System.Reflection;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Text;
using static System.Net.WebRequestMethods;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;

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
        private const string CODE_POSTAL_CANADIEN_PATTERN_String = @"[A-Z][0-9][A-Z] ?[0-9][A-Z][0-9]";
        private const string TELEPHONE_CANADIEN_PATTERN_String = "^(\\([2-9]\\d{2}\\)|[2-9]\\d{2})[- .]?\\d{3}[- .]?\\d{4}$";
        private const string CULTURE_CA = "en-CA";

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
            typeInvalide,
            convertionImpossible,
            fichierInvalide
        }

        private string[] tMessagesErreurs;

        private void InitMessagesErreurs()
        {
            tMessagesErreurs = new string[Enum.GetValues(typeof(CodeErreurs)).Length];// initialisation
            tMessagesErreurs[(int)CodeErreurs.nomObligatoire] = "Le nom est obligatoire.";
            tMessagesErreurs[(int)CodeErreurs.prenomObligatoire] = "Le prenom est obligatoire.";
            tMessagesErreurs[(int)CodeErreurs.addressObligatoire] = "L'adresse est obligatoire.";
            tMessagesErreurs[(int)CodeErreurs.anneeObligatoire] = "L'annee est obligatoire";
            tMessagesErreurs[(int)CodeErreurs.codePostalObligatoire] = "Le code postal est obligatoire.";
            tMessagesErreurs[(int)CodeErreurs.codePostalInvalide] = "Le code postal est invalide.";
            tMessagesErreurs[(int)CodeErreurs.marqueInvalide] = "La marque est invalide";
            tMessagesErreurs[(int)CodeErreurs.marqueObligatoire] = "La marque est obligatoire";
            tMessagesErreurs[(int)CodeErreurs.modelInvalide] = "Le modèle est invalide.";
            tMessagesErreurs[(int)CodeErreurs.modelObligatoire] = "Le modèle est obligatoire.";
            tMessagesErreurs[(int)CodeErreurs.dateLivraisonInvalide] = "La date de livraison est invalide.";
            tMessagesErreurs[(int)CodeErreurs.prixInvalide] = "Prix invalide";
            tMessagesErreurs[(int)CodeErreurs.erreurIndeterminee] = "Erreur indeterminee";
            tMessagesErreurs[(int)CodeErreurs.dateInvalide] = "La date doit se situer dans les 15 jours precedant ou suivant de la date courante";
            tMessagesErreurs[(int)CodeErreurs.telephoneInvalide] = "Numero de telephone invalide";
            tMessagesErreurs[(int)CodeErreurs.telephoneObligatoire] = "Le numero de telephone est obligatoire";
            tMessagesErreurs[(int)CodeErreurs.typeInvalide] = "Type invalide";
            tMessagesErreurs[(int)CodeErreurs.typeObligatoire] = "Le type est obligatoire";
            tMessagesErreurs[(int)CodeErreurs.convertionImpossible] = "Impossible de convertir le prix en valeur réelle.";
            tMessagesErreurs[(int)CodeErreurs.fichierInvalide] = "Le fichier des prix n’est pas disponible.";


        }

        #endregion

        #region Declaration des tableaux

        private string[] tMarques = new string[20];
        private string[] tModel = new string[20];
        private decimal[,] tPrix = new decimal[20, 20];

        private readonly long id;
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
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\.."));
                string filePath = Path.Combine(projectRoot, "Data", "Marques.data");

                using (StreamReader sr = new StreamReader(filePath))
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
                throw new FormatException(tMessagesErreurs[(int)CodeErreurs.modelInvalide]);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException(tMessagesErreurs[(int)CodeErreurs.modelInvalide]);
            }
            catch (Exception)
            {
                throw new Exception(tMessagesErreurs[(int)CodeErreurs.modelInvalide]);
            }
        }

        private void InitModel()
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\.."));
                string filePath = Path.Combine(projectRoot, "Data", "Modeles.data");

                using (StreamReader sr = new StreamReader(filePath))
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
                throw new FormatException(tMessagesErreurs[(int)CodeErreurs.modelInvalide]);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException(tMessagesErreurs[(int)CodeErreurs.modelInvalide]);
            }
            catch (Exception)
            {
                throw new Exception(tMessagesErreurs[(int)CodeErreurs.modelInvalide]);
            }
        }

        private void InitPrix()
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\.."));
                string filePath = Path.Combine(projectRoot, "Data", "Prix.data");

                using (StreamReader sr = new StreamReader(filePath))
                {
                    int rangee = tMarques.Length - 1;
                    int colonne = tModel.Length - 1;

                    for (int ran = 0; ran <= tMarques.Length - 1; ran++)
                        for (int col = 0; col <= tModel.Length - 1; col++)
                            tPrix[ran, col] = decimal.Parse(sr.ReadLine(), CultureInfo.CreateSpecificCulture(CULTURE_CA));   

                }
            }
            catch (FormatException)
            {
                throw new FormatException(tMessagesErreurs[(int)CodeErreurs.convertionImpossible]);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException(tMessagesErreurs[(int)CodeErreurs.fichierInvalide]);
            }
            catch (Exception)
            {
                throw new Exception("Erreur indéterminée dans la lecture des prix.");
            }
        }

        #endregion
        
        

        #region Propriétés

        public long Id
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
                        throw new ArgumentException(tMessagesErreurs[(int)CodeErreurs.nomObligatoire]);
                }
                else
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodeErreurs.nomObligatoire]);
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
                        throw new ArgumentException(tMessagesErreurs[(int)CodeErreurs.prenomObligatoire]);
                }
                else
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodeErreurs.prenomObligatoire]);
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
                        throw new ArgumentException(tMessagesErreurs[(int)CodeErreurs.addressObligatoire]);
                }
                else
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodeErreurs.addressObligatoire]);
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
                        throw new ArgumentException(tMessagesErreurs[(int)CodeErreurs.anneeObligatoire]);
                }
                else
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodeErreurs.anneeObligatoire]);
            }
        }

        public string CodePostal
        {
            get { return codePostal; }
            set
            {
                if (value != null)
                {
                    if (Regex.IsMatch(value, CODE_POSTAL_CANADIEN_PATTERN_String))
                    {
                        codePostal = value.Trim();
                    }
                    else
                    {
                        throw new FormatException(tMessagesErreurs[(int)CodeErreurs.codePostalInvalide]);
                    }
                }
                else
                {
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodeErreurs.codePostalObligatoire]);
                }
            }
        }

        public string Telephone
        {
            get { return telephone; }
            set
            {
                if (value != null)
                {
                    if (value != String.Empty)
                    {
                        if (Regex.IsMatch(value, TELEPHONE_CANADIEN_PATTERN_String))
                            telephone = value.Trim();
                        else
                            throw new FormatException(tMessagesErreurs[(int)CodeErreurs.telephoneInvalide]);
                    }
                    else
                        throw new ArgumentException(tMessagesErreurs[(int)CodeErreurs.telephoneObligatoire]);
                }
                else
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodeErreurs.telephoneObligatoire]);
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
                        throw new ArgumentException(tMessagesErreurs[(int)CodeErreurs.typeInvalide]);
                }
                else
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodeErreurs.typeObligatoire]);
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
                        throw new ArgumentNullException(tMessagesErreurs[(int)CodeErreurs.marqueInvalide]);
                }
                else
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodeErreurs.marqueObligatoire]);
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
                        throw new ArgumentNullException(tMessagesErreurs[(int)CodeErreurs.modelInvalide]);
                }
                else
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodeErreurs.modelObligatoire]);
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
                    throw new ArgumentOutOfRangeException(tMessagesErreurs[(int)CodeErreurs.dateInvalide]);
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
                            throw new ArgumentException(tMessagesErreurs[(int)CodeErreurs.prixInvalide]);
                    }
                    else 
                        throw new ArgumentNullException(tMessagesErreurs[(int)CodeErreurs.prixInvalide]);
                }
                else
                    throw new ArgumentOutOfRangeException(tMessagesErreurs[(int)CodeErreurs.prixInvalide]);
            }
        }

        #endregion

        #region Constructeur

        public Transaction()
        {
            InitMessagesErreurs();
            InitMarques();
            InitModel();
            InitPrix();

            id = DateTime.UtcNow.Ticks;
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
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\.."));
            string dataFolder = Path.Combine(projectRoot, "Data");
            string filePath = Path.Combine(dataFolder, "Transactions.data");

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            if (!TransactionComplete())
            {
                throw new Exception("Transaction incomplète");
            }


            CultureInfo culture = new CultureInfo("en-CA", false);

            string data = "";
            data += id + ";";
            data += nom + ";";
            data += prenom + ";";
            data += adresse + ";";
            data += codePostal + ";";
            data += telephone + ";";
            data += type + ";";
            data += annee + ";";
            data += marque + ";";
            data += modele + ";";
            data += dateLivraison.ToShortDateString() + ";";
            data += prix.ToString("C", culture) + ";";

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(data);
            }

            //Console.WriteLine($"Transaction #{numTransaction}");
            //Console.WriteLine($"Transaction de {Nom} {Prenom}:");
            //Console.WriteLine($"Adresse: {Adresse}, {CodePostal}");
            //Console.WriteLine($"Téléphone: {Telephone}");
            //Console.WriteLine($"Type: {Type}, Modèle: {Modele}");
            //Console.WriteLine($"Date de livraison: {DateLivraison.ToShortDateString()}");
            //Console.WriteLine($"Prix: {Prix:C}");
        }

        /// <summary>
        /// Enregistrer la transaction avec des paramètres
        /// </summary>
        public void Enregistrer(string nom, string prenom, string adresse, string codePostal, string telephone,
                                string type,string annee, string marque, string modele, DateTime dateLivraison, decimal prix)
        {
            this.Nom = nom;
            this.Prenom = prenom;
            this.Adresse = adresse;
            this.CodePostal = codePostal;
            this.Telephone = telephone;
            this.Type = type;
            this.Annee = annee;
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

        #region Transaction Complete
        public bool TransactionComplete()
        {
            if (string.IsNullOrWhiteSpace(this.Nom)) return false;
            if (string.IsNullOrWhiteSpace(this.Prenom)) return false;
            if (string.IsNullOrWhiteSpace(this.Adresse)) return false;
            if (string.IsNullOrWhiteSpace(this.CodePostal)) return false;
            if (string.IsNullOrWhiteSpace(this.Telephone)) return false;
            if (string.IsNullOrWhiteSpace(this.Type)) return false;
            if (string.IsNullOrWhiteSpace(this.Modele)) return false;
            if (string.IsNullOrWhiteSpace(this.Marque)) return false;
            if (string.IsNullOrWhiteSpace(this.Annee)) return false;
            if (this.DateLivraison == null) return false;
            if (this.Prix == 0) return false;

            return true;
        }
        #endregion
    }
}
