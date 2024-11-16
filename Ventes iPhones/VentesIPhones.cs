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
            //oTrans = new Transaction(
            //nomMaskedTextBox,
            //prenomMaskedTextBox,
            //adresseMaskedTextBox,
            //codePostalMaskedTextBox,
            //telephoneMaskedTextBox,
            //typesComboBox,
            //anneesComboBox,
            //marquesComboBox,
            //modelComboBox,
            //prixLabel);
                
            oTrans.Nom = nomMaskedTextBox.Text;
            oTrans.Prenom = prenomMaskedTextBox.Text;
            oTrans.Adresse = adresseMaskedTextBox.Text;
            oTrans.CodePostal = codePostalMaskedTextBox.Text;
            oTrans.Telephone = telephoneMaskedTextBox.Text;
            oTrans.Type = typesComboBox.SelectedItem.ToString();
            oTrans.Modele = modelComboBox.SelectedItem.ToString();
            oTrans.DateLivraison = DateTime.Now;
            oTrans.Prix = Decimal.Parse(prixLabel.Text, System.Globalization.NumberStyles.Currency);

            try
            {
                oTrans.Enregistrer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
    }
}
