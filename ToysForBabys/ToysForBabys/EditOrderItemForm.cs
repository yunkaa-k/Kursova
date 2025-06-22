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
    public partial class EditOrderItemForm : Form
    {
        private string connectionString;
        private int orderItemId;

        public EditOrderItemForm(int orderItemId, int orderId, int itemId, int quantity, decimal price)
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;
            this.orderItemId = orderItemId;

            LoadOrders();
            LoadItems();

            comboOrder.SelectedValue = orderId;
            comboItem.SelectedValue = itemId;
            numQuantity.Value = quantity;
            txtPrice.Text = price.ToString("F2");
        }

        private void LoadOrders()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT OrderID FROM Orders", conn);
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
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT ItemID, Name FROM Item", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                comboItem.DataSource = dt;
                comboItem.DisplayMember = "Name";
                comboItem.ValueMember = "ItemID";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Неправильний формат ціни.");
                return;
            }

            int orderId = (int)comboOrder.SelectedValue;
            int itemId = (int)comboItem.SelectedValue;
            int quantity = (int)numQuantity.Value;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE OrderItems SET OrderID = @orderId, ItemID = @itemId, 
                                Quantity = @quantity, Price = @price WHERE OrderItemID = @orderItemId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@orderItemId", orderItemId);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Запис успішно оновлено.");
                        this.Close();
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

        private void EditOrderItemForm_Load(object sender, EventArgs e)
        {

        }
    }
}
