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
    public partial class EditSupplierForm : Form
    {
        private readonly int supplierId;
        private readonly string connectionString;

        public EditSupplierForm(int id, string name, string contactName, string phone, string email, string address)
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;

            supplierId = id;
            txtName.Text = name;
            txtContactName.Text = contactName;
            txtPhone.Text = phone;
            txtEmail.Text = email;
            txtAddress.Text = address;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string contactName = txtContactName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string address = txtAddress.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(contactName) ||
                string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Suppliers 
                                 SET Name = @name, ContactName = @contactName, Phone = @phone, Email = @email, Address = @address
                                 WHERE SupplierID = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@contactName", contactName);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@id", supplierId);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Постачальника оновлено успішно.");
                        this.Close();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                        {
                            MessageBox.Show("Постачальник з таким номером телефону вже існує.");
                        }
                        else
                        {
                            MessageBox.Show("Помилка при оновленні: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditSupplierForm_Load(object sender, EventArgs e)
        {

        }
    }
}

