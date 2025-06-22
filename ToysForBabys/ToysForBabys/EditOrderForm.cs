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
    public partial class EditOrderForm : Form
    {
        private string connectionString;
        private int orderId;

        public EditOrderForm(int orderId, int clientId, decimal totalAmount, DateTime orderDate)
        {
            InitializeComponent();
            this.orderId = orderId;

            connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;

            LoadClients(clientId);

            txtTotalAmount.Text = totalAmount.ToString("F2");
            dateOrderDate.Value = orderDate;
        }

        private void LoadClients(int selectedClientId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ID, CONCAT(FirstName, ' ', LastName) AS FullName FROM Clients";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboClient.DataSource = dt;
                comboClient.DisplayMember = "FullName";
                comboClient.ValueMember = "ID";

                comboClient.SelectedValue = selectedClientId;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (comboClient.SelectedIndex == -1)
            {
                MessageBox.Show("Оберіть клієнта.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtTotalAmount.Text, out decimal totalAmount))
            {
                MessageBox.Show("Введіть коректну суму.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int clientId = (int)comboClient.SelectedValue;
            DateTime orderDate = dateOrderDate.Value;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Orders 
                                 SET ClientID = @clientId, TotalAmount = @totalAmount, OrderDate = @orderDate 
                                 WHERE OrderID = @orderId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@clientId", clientId);
                    cmd.Parameters.AddWithValue("@totalAmount", totalAmount);
                    cmd.Parameters.AddWithValue("@orderDate", orderDate);
                    cmd.Parameters.AddWithValue("@orderId", orderId);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Замовлення оновлено.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при оновленні: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditOrderForm_Load(object sender, EventArgs e)
        {

        }
    }
}

