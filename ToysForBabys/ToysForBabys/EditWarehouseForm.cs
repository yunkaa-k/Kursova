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
    public partial class EditWarehouseForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;
        private int warehouseId;

        public EditWarehouseForm(int id, string name, string location, string phone)
        {
            InitializeComponent();

            warehouseId = id;
            txtName.Text = name;
            txtLocation.Text = location;
            txtPhone.Text = phone;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string location = txtLocation.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(location) || string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE Warehouses SET Name = @Name, Location = @Location, Phone = @Phone WHERE WarehouseID = @WarehouseID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Location", location);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@WarehouseID", warehouseId);

                try
                {
                    conn.Open();
                    int affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows > 0)
                    {
                        MessageBox.Show("Дані успішно оновлено.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Запис не знайдено або не було змін.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при оновленні даних: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditWarehouseForm_Load(object sender, EventArgs e)
        {

        }
    }
}

