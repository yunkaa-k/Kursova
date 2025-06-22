using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace ToysForBabys
{
    public partial class AddClientForm : Form
    {
        private string connectionString;

        public AddClientForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;
            btnSave.Click += BtnSave_Click;
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string contacts = txtContacts.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (firstName == "" || lastName == "" || contacts == "" || email == "")
            {
                MessageBox.Show("Будь ласка, заповніть усі поля.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Clients (FirstName, LastName, Contacts, Email) VALUES (@FirstName, @LastName, @Contacts, @Email)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Contacts", contacts);
                cmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Клієнт успішно доданий.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при додаванні клієнта: " + ex.Message);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

