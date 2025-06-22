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
    public partial class AddSupplyForm : Form
    {
        private string connectionString;

        public AddSupplyForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;

            LoadSuppliers();
            LoadItems();
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
            DateTime deliveryDate = dateDelivery.Value.Date;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Supplies (SupplierID, ItemID, SupplyPrice, LastDeliveryDate) " +
                               "VALUES (@supplierId, @itemId, @price, @deliveryDate)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@supplierId", supplierId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@deliveryDate", deliveryDate);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Поставку додано успішно.");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при додаванні: " + ex.Message);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddSupplyForm_Load(object sender, EventArgs e)
        {

        }
    }
}
