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
    public partial class EditItemForm : Form
    {
        private string connectionString;
        private int itemId;

        public EditItemForm(int id, string name, int categoryId, string description, decimal price, int stock)
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;
            itemId = id;

            LoadCategories();

            // Заповнюємо поля
            txtName.Text = name;
            comboCategory.SelectedValue = categoryId;
            txtDescription.Text = description;
            txtPrice.Text = price.ToString();
            txtStock.Text = stock.ToString();
        }

        private void LoadCategories()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CategoryID, Name FROM Categories";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboCategory.DataSource = dt;
                comboCategory.DisplayMember = "Name";
                comboCategory.ValueMember = "CategoryID";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                comboCategory.SelectedItem == null ||
                !decimal.TryParse(txtPrice.Text, out decimal price) ||
                !int.TryParse(txtStock.Text, out int stock))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля коректно.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txtName.Text;
            int categoryId = Convert.ToInt32(comboCategory.SelectedValue);
            string description = txtDescription.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Item 
                                 SET Name = @name, CategoryID = @categoryId, Description = @desc, Price = @price, Stock = @stock 
                                 WHERE ItemID = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    cmd.Parameters.AddWithValue("@desc", description);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@id", itemId);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Товар успішно оновлено.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Запис не знайдено або не було змінено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при оновленні товару: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditItemForm_Load(object sender, EventArgs e)
        {

        }
    }
}

