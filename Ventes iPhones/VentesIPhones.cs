using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransactionNS;
using TypesNS;
using g = VentesIPhones.ClassGeneral;


namespace VentesIPhones
{

    public partial class VentesIPhones : Form
    {
        #region XML

        /// <summary>
        /// Class pour gerer la ventes de iPhones
        /// </summary>

        #endregion

        #region Constructeur

        public VentesIPhones() 
        {
            InitializeComponent();
        }

        #endregion

        #region Initialisation

        private void VentesPneusForm_Load(object sender, EventArgs e)
        {
            Transaction oTrans = new Transaction();
            Types oTypes = new Types();

            try
            {
                typesComboBox.Items.AddRange(oTypes.GetTypes((Types.CodeTypes)0));
                anneesComboBox.Items.AddRange(oTypes.GetTypes((Types.CodeTypes)1));

                typesComboBox.SelectedIndex = 0;
                anneesComboBox.SelectedIndex = 0;

                marquesComboBox.Items.AddRange(oTrans.GetMarques());
                modelComboBox.Items.AddRange(oTrans.getModel());

                marquesComboBox.SelectedIndex = 0;
                modelComboBox.SelectedIndex = 0;
            }
            catch (ArgumentOutOfRangeException ex) 
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Obtenir le prix

        private void MarquesModelSelectedIndexChanged(object sender, EventArgs e)
        {
            Transaction oTrans = new Transaction();

            try
            {
                if (marquesComboBox.SelectedIndex != -1 && modelComboBox.SelectedIndex != -1)
                {
                    try
                    {
                        prixLabel.Text = oTrans.GetPrix(marquesComboBox.SelectedIndex, modelComboBox.SelectedIndex).ToString("C");
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        MessageBox.Show(ex.Message, "Out of Range");
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message, "Invalid argument.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(g.tMessagesErreursStr[(int)g.CodeErreurs.ceException] + ": " + ex.Message, "Erreur");
                    }

                    try
                    {
                        prixLabel.Text = oTrans.GetPrix(marquesComboBox.SelectedItem.ToString(), modelComboBox.SelectedItem.ToString()).ToString("C");
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        MessageBox.Show(ex.Message, "Out of Range");
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message, "Invalid argument.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(g.tMessagesErreursStr[(int)g.CodeErreurs.ceException] + ": " + ex.Message, "Erreur");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region Quitter

        private void quitter(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region MaskedTextBox
        private void MaskedTextBox_Enter(object sender, EventArgs e)
        {
            MaskedTextBox maskedTextBox = sender as MaskedTextBox;
            maskedTextBox.SelectAll();
        }
        #endregion

        private void Enregistrer(object sender, EventArgs e)
        {
            ValidateChildren();
            Transaction oTrans = new Transaction();

            FormatTextFields();

            try
            {
                oTrans.Enregistrer(nomMaskedTextBox.Text, prenomMaskedTextBox.Text, adresseMaskedTextBox.Text,
                    codePostalMaskedTextBox.Text, telephoneMaskedTextBox.Text, typesComboBox.Text,
                    modelComboBox.Text, DateTime.Parse(dateLivraisonDateTimePicker.Text), Decimal.Parse(prixLabel.Text, System.Globalization.NumberStyles.Currency),
                    marquesComboBox.Text);

                paiementDuLabel.Text = oTrans.datePaiement.ToShortDateString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DateLivrasionValidating(object sender, CancelEventArgs e)
        {
            DateTime date;

            if (DateTime.TryParse(dateLivraisonDateTimePicker.Text, out date))
                dateLivraisonDateTimePicker.Text = date.ToLongDateString();
            else
                dateLivraisonDateTimePicker.Text = DateTime.Now.ToLongDateString();
        }

        /// <summary>
        /// Formate les champs de texte pour garantir une présentation standardisée.
        /// </summary>
        private void FormatTextFields()
        {
            // Check si les field sont vides
            var textFields = new[]
            {
                prenomMaskedTextBox.Text,
                nomMaskedTextBox.Text,
                adresseMaskedTextBox.Text,
                codePostalMaskedTextBox.Text,
                telephoneMaskedTextBox.Text
            };

            // si un des champs est vide, on ne fait rien
            if (!textFields.All(field => !string.IsNullOrWhiteSpace(field)))
            {
                return;
            }

            try
            {
                // Trim Whitespace
                prenomMaskedTextBox.Text = prenomMaskedTextBox.Text.Trim();
                nomMaskedTextBox.Text = nomMaskedTextBox.Text.Trim();
                adresseMaskedTextBox.Text = adresseMaskedTextBox.Text.Trim();
                codePostalMaskedTextBox.Text = codePostalMaskedTextBox.Text.Trim();
                telephoneMaskedTextBox.Text = telephoneMaskedTextBox.Text.Trim();

                // Format le nom et prenom
                prenomMaskedTextBox.Text = prenomMaskedTextBox.Text.ToLower();
                nomMaskedTextBox.Text = nomMaskedTextBox.Text.ToLower();
                prenomMaskedTextBox.Text = prenomMaskedTextBox.Text.Substring(0, 1).ToUpper() + prenomMaskedTextBox.Text.Substring(1);
                nomMaskedTextBox.Text = nomMaskedTextBox.Text.Substring(0, 1).ToUpper() + nomMaskedTextBox.Text.Substring(1);

                // Format l'adresse
                codePostalMaskedTextBox.Text = codePostalMaskedTextBox.Text.ToUpper();
                codePostalMaskedTextBox.Text = codePostalMaskedTextBox.Text.Replace(" ", "");

                telephoneMaskedTextBox.Text = telephoneMaskedTextBox.Text.Replace("(", "");
                telephoneMaskedTextBox.Text = telephoneMaskedTextBox.Text.Replace(")", "");
                telephoneMaskedTextBox.Text = telephoneMaskedTextBox.Text.Replace("-", "");
                telephoneMaskedTextBox.Text = telephoneMaskedTextBox.Text.Replace(" ", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
