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
    public partial class EditEmployeeForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;
        private int employeeId;

        public EditEmployeeForm(int id, string firstName, string lastName, string position, DateTime hireDate, string phone, string email)
        {
            InitializeComponent();

            employeeId = id;
            txtFirstName.Text = firstName;
            txtLastName.Text = lastName;
            txtPosition.Text = position;
            dateTimePickerHireDate.Value = hireDate;
            txtPhone.Text = phone;
            txtEmail.Text = email;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string position = txtPosition.Text.Trim();
            DateTime hireDate = dateTimePickerHireDate.Value.Date;
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(position) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"UPDATE Employees SET FirstName = @FirstName, LastName = @LastName, Position = @Position,
                             HireDate = @HireDate, Phone = @Phone, Email = @Email WHERE EmployeeID = @EmployeeID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Position", position);
                cmd.Parameters.AddWithValue("@HireDate", hireDate);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeId);

                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Дані працівника оновлено успішно.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Працівника не знайдено.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                        MessageBox.Show("Працівник з таким телефоном вже існує.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Помилка при оновленні працівника: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditEmployeeForm_Load(object sender, EventArgs e)
        {

        }
    }
}
