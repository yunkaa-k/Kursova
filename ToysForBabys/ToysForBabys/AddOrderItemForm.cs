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
    public partial class AddOrderItemForm : Form
    {
        private string connectionString;

        public AddOrderItemForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;

            LoadOrders();
            LoadItems();
        }

        private void LoadOrders()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT OrderID FROM Orders ORDER BY OrderID DESC";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboOrder.DataSource = dt;
                comboOrder.DisplayMember = "OrderID";
                comboOrder.ValueMember = "OrderID";
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

                comboItem.DataSource = dt;
                comboItem.DisplayMember = "Name";
                comboItem.ValueMember = "ItemID";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (comboOrder.SelectedIndex == -1 || comboItem.SelectedIndex == -1)
            {
                MessageBox.Show("Будь ласка, оберіть замовлення та товар.");
                return;
            }

            int orderId = (int)comboOrder.SelectedValue;
            int itemId = (int)comboItem.SelectedValue;
            int quantity = (int)numQuantity.Value;

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Введіть коректну ціну.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO OrderItems (OrderID, ItemID, Quantity, Price)
                                 VALUES (@orderId, @itemId, @quantity, @price)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@price", price);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Деталь замовлення додано.");
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

        private void AddOrderItemForm_Load(object sender, EventArgs e)
        {

        }
    }
}

