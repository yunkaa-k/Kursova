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
    public partial class EditClientForm : Form
    {
        private string connectionString;
        private int clientId;

        public EditClientForm(int id, string firstName, string lastName, string contacts, string email)
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;

            // Ініціалізуємо поля
            clientId = id;
            txtFirstName.Text = firstName;
            txtLastName.Text = lastName;
            txtContacts.Text = contacts;
            txtEmail.Text = email;

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
                string query = "UPDATE Clients SET FirstName = @FirstName, LastName = @LastName, Contacts = @Contacts, Email = @Email WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Contacts", contacts);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@ID", clientId);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Клієнт успішно оновлений.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при оновленні клієнта: " + ex.Message);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditClientForm_Load(object sender, EventArgs e)
        {

        }
    }
}

