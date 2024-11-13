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

        private void MarquesDiametresSelectedIndexChanged(object sender, EventArgs e)
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
                    catch (ArgumentOutOfRangeException)
                    {
                        MessageBox.Show("Out of Range");
                    }
                    catch (ArgumentException)
                    {
                        MessageBox.Show("Invalid argument.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    try
                    {
                        prixLabel.Text = oTrans.GetPrix(marquesComboBox.SelectedItem.ToString(), modelComboBox.SelectedItem.ToString()).ToString("C");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        MessageBox.Show("Out of Range");
                    }
                    catch (ArgumentException)
                    {
                        MessageBox.Show("Invalid argument.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
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

        private void quitterButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}
