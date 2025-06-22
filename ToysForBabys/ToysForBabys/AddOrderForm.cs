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
    public partial class AddOrderForm : Form
    {
        private string connectionString;

        public AddOrderForm()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;

            LoadClients();

            dateOrderDate.Value = DateTime.Now;
        }

        private void LoadClients()
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

                comboClient.SelectedIndex = -1; 
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
                MessageBox.Show("Введіть коректну суму замовлення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int clientId = (int)comboClient.SelectedValue;
            DateTime orderDate = dateOrderDate.Value;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Orders (ClientID, TotalAmount, OrderDate) 
                                 VALUES (@clientId, @totalAmount, @orderDate)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@clientId", clientId);
                    cmd.Parameters.AddWithValue("@totalAmount", totalAmount);
                    cmd.Parameters.AddWithValue("@orderDate", orderDate);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Замовлення успішно додано.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при додаванні замовлення: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddOrderForm_Load(object sender, EventArgs e)
        {

        }
    }
}

