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
    public partial class EditCategoryForm : Form
    {
        private readonly int categoryId;
        private readonly string connectionString;

        public EditCategoryForm(int id, string currentName)
        {
            InitializeComponent();
            categoryId = id;
            txtCategoryName.Text = currentName;

            connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            string newName = txtCategoryName.Text.Trim();

            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Назва не може бути порожньою.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Categories SET Name = @Name WHERE CategoryID = @ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", newName);
                    cmd.Parameters.AddWithValue("@ID", categoryId);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Категорію оновлено успішно.");
                        this.Close();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627 || ex.Number == 2601)
                            MessageBox.Show("Категорія з такою назвою вже існує.");
                        else
                            MessageBox.Show("Помилка при оновленні: " + ex.Message);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditCategoryForm_Load(object sender, EventArgs e)
        {

        }
    }
}

