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
    public partial class AddOrderDeliveryForm : Form
    {
        private string connectionString;

        public AddOrderDeliveryForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;
            LoadOrders();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            int orderId = (int)comboOrder.SelectedValue;
            DateTime deliveryDate = dateDelivery.Value.Date;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO OrderDelivery (OrderID, DeliveryDate) VALUES (@orderId, @deliveryDate)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.Parameters.AddWithValue("@deliveryDate", deliveryDate);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Доставку успішно додано.");
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

        private void AddOrderDeliveryForm_Load(object sender, EventArgs e)
        {

        }
    }
}
