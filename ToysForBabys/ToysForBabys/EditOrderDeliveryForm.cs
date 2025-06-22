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
    public partial class EditOrderDeliveryForm : Form
    {
        private string connectionString;
        private int orderDeliveryId;

        public EditOrderDeliveryForm(int orderDeliveryId, int orderId, DateTime deliveryDate)
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;
            this.orderDeliveryId = orderDeliveryId;

            LoadOrders();
            comboOrder.SelectedValue = orderId;
            dateDelivery.Value = deliveryDate;
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
            int newOrderId = (int)comboOrder.SelectedValue;
            DateTime newDate = dateDelivery.Value.Date;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE OrderDelivery SET OrderID = @orderId, DeliveryDate = @deliveryDate WHERE OrderDeliveryID = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@orderId", newOrderId);
                    cmd.Parameters.AddWithValue("@deliveryDate", newDate);
                    cmd.Parameters.AddWithValue("@id", orderDeliveryId);

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

        private void EditOrderDeliveryForm_Load(object sender, EventArgs e)
        {

        }
    }
}
