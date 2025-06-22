using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace ToysForBabys
{
    public partial class EditSupplyForm : Form
    {
        private string connectionString;
        private int supplyId;

        public EditSupplyForm(int supplyId, int supplierId, int itemId, decimal supplyPrice, DateTime lastDeliveryDate)
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;
            this.supplyId = supplyId;

            LoadSuppliers();
            LoadItems();

            comboSuppliers.SelectedValue = supplierId;
            comboItems.SelectedValue = itemId;
            txtPrice.Text = supplyPrice.ToString();
            dateDelivery.Value = lastDeliveryDate;
        }

        private void LoadSuppliers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT SupplierID, Name FROM Suppliers";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboSuppliers.DataSource = dt;
                comboSuppliers.DisplayMember = "Name";
                comboSuppliers.ValueMember = "SupplierID";
            }
        }

        private void LoadItems()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ItemID, Name FROM Item";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboItems.DataSource = dt;
                comboItems.DisplayMember = "Name";
                comboItems.ValueMember = "ItemID";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (comboSuppliers.SelectedValue == null || comboItems.SelectedValue == null || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля.");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Ціна повинна бути числом.");
                return;
            }

            int supplierId = Convert.ToInt32(comboSuppliers.SelectedValue);
            int itemId = Convert.ToInt32(comboItems.SelectedValue);
            DateTime deliveryDate = dateDelivery.Value;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Supplies 
                                 SET SupplierID = @supplierId, ItemID = @itemId, SupplyPrice = @price, LastDeliveryDate = @deliveryDate
                                 WHERE SupplyID = @supplyId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@supplierId", supplierId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@deliveryDate", deliveryDate);
                    cmd.Parameters.AddWithValue("@supplyId", supplyId);

                    try
                    {
                        conn.Open();
                        int affectedRows = cmd.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            MessageBox.Show("Поставка успішно оновлена.");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Запис не знайдено.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при оновленні: " + ex.Message);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditSupplyForm_Load(object sender, EventArgs e)
        {

        }
    }
}

