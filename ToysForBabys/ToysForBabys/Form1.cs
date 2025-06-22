using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ToysForBabys
{
    public partial class Form1 : Form
    {
        private string connectionString;

        public Form1()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["ToysForBabys"].ConnectionString;

            this.Load += Form1_Load;
            comboBoxTables.SelectedIndexChanged += comboBoxTables_SelectedIndexChanged;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxTables.Items.AddRange(new string[]
            {
                "Categories", "Item", "Clients", "Orders", "OrderItems",
                "OrderDelivery", "Suppliers", "Supplies", "Employees", "Warehouses"
            });

            comboBoxTables.SelectedIndex = 0;
        }

        private void comboBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTable = comboBoxTables.SelectedItem.ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = $"SELECT * FROM [{selectedTable}]";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при завантаженні даних: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string selectedTable = comboBoxTables.SelectedItem.ToString();

            if (selectedTable == "Clients")
            {
                AddClientForm form = new AddClientForm();
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
            else if (selectedTable == "Categories")
            {
                AddCategoryForm form = new AddCategoryForm();
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
            else if (selectedTable == "Item")
            {
                AddItemForm form = new AddItemForm();
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
            else if (selectedTable == "Orders")
            {
                AddOrderForm form = new AddOrderForm();
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
            else if (selectedTable == "OrderItems")
            {
                AddOrderItemForm form = new AddOrderItemForm();
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null); 
                form.ShowDialog();
            }
            else if (selectedTable == "OrderDelivery")
            {
                AddOrderDeliveryForm form = new AddOrderDeliveryForm();
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
            else if (selectedTable == "Suppliers")
            {
                AddSupplierForm form = new AddSupplierForm();
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
            else if (selectedTable == "Supplies")
            {
                AddSupplyForm form = new AddSupplyForm();
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
            else if (selectedTable == "Employees")
            {
                AddEmployeeForm form = new AddEmployeeForm();
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
            else if (selectedTable == "Warehouses")
            {
                AddWarehouseForm form = new AddWarehouseForm();
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
        }



        private void btnEdit_Click(object sender, EventArgs e)
        {
            string selectedTable = comboBoxTables.SelectedItem.ToString();

            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Будь ласка, виберіть рядок для редагування.");
                return;
            }

            DataGridViewRow row = dataGridView.SelectedRows[0];

            if (selectedTable == "Clients")
            {
                int id = Convert.ToInt32(row.Cells["ID"].Value);
                string firstName = row.Cells["FirstName"].Value.ToString();
                string lastName = row.Cells["LastName"].Value.ToString();
                string contacts = row.Cells["Contacts"].Value.ToString();
                string email = row.Cells["Email"].Value.ToString();

                EditClientForm form = new EditClientForm(id, firstName, lastName, contacts, email);
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
            else if (selectedTable == "Categories")
            {
                int categoryId = Convert.ToInt32(row.Cells["CategoryID"].Value);
                string name = row.Cells["Name"].Value.ToString();

                EditCategoryForm form = new EditCategoryForm(categoryId, name);
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
            else if (selectedTable == "Item")
            {
                if (dataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Будь ласка, виберіть рядок для редагування.");
                    return;
                }


                int id = Convert.ToInt32(row.Cells["ItemID"].Value);
                string name = row.Cells["Name"].Value?.ToString();
                int categoryId = Convert.ToInt32(row.Cells["CategoryID"].Value);
                string description = row.Cells["Description"].Value?.ToString();
                decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
                int stock = Convert.ToInt32(row.Cells["Stock"].Value);

                EditItemForm form = new EditItemForm(id, name, categoryId, description, price, stock);
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null); 
                form.ShowDialog();
            }
            else if (selectedTable == "Orders")
            {
                if (dataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Оберіть замовлення для редагування.");
                    return;
                }

                int orderId = Convert.ToInt32(row.Cells["OrderID"].Value);
                int clientId = row.Cells["ClientID"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["ClientID"].Value) : 0;
                decimal totalAmount = Convert.ToDecimal(row.Cells["TotalAmount"].Value);
                DateTime orderDate = Convert.ToDateTime(row.Cells["OrderDate"].Value);

                EditOrderForm form = new EditOrderForm(orderId, clientId, totalAmount, orderDate);
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
            else if (selectedTable == "OrderItems")
            {
                if (dataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Оберіть запис для редагування.");
                    return;
                }

                int orderItemId = Convert.ToInt32(row.Cells["OrderItemID"].Value);
                int orderId = Convert.ToInt32(row.Cells["OrderID"].Value);
                int itemId = Convert.ToInt32(row.Cells["ItemID"].Value);
                int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                decimal price = Convert.ToDecimal(row.Cells["Price"].Value);

                EditOrderItemForm form = new EditOrderItemForm(orderItemId, orderId, itemId, quantity, price);
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null); 
                form.ShowDialog();
            }
            else if (selectedTable == "OrderDelivery")
            {
                if (dataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Оберіть запис для редагування.");
                    return;
                }

                int orderDeliveryId = Convert.ToInt32(row.Cells["OrderDeliveryID"].Value);
                int orderId = Convert.ToInt32(row.Cells["OrderID"].Value);
                DateTime deliveryDate = Convert.ToDateTime(row.Cells["DeliveryDate"].Value);

                EditOrderDeliveryForm form = new EditOrderDeliveryForm(orderDeliveryId, orderId, deliveryDate);
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
            else if (selectedTable == "Suppliers")
            {
                if (dataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Будь ласка, виберіть постачальника для редагування.");
                    return;
                }

                int id = Convert.ToInt32(row.Cells["SupplierID"].Value);
                string name = row.Cells["Name"].Value.ToString();
                string contactName = row.Cells["ContactName"].Value.ToString();
                string phone = row.Cells["Phone"].Value.ToString();
                string email = row.Cells["Email"].Value.ToString();
                string address = row.Cells["Address"].Value.ToString();

                EditSupplierForm form = new EditSupplierForm(id, name, contactName, phone, email, address);
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
            else if (selectedTable == "Supplies")
            {
                if (dataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Будь ласка, виберіть поставку для редагування.");
                    return;
                }

                int supplyId = Convert.ToInt32(row.Cells["SupplyID"].Value);
                int supplierId = Convert.ToInt32(row.Cells["SupplierID"].Value);
                int itemId = Convert.ToInt32(row.Cells["ItemID"].Value);
                decimal supplyPrice = Convert.ToDecimal(row.Cells["SupplyPrice"].Value);
                DateTime lastDeliveryDate = Convert.ToDateTime(row.Cells["LastDeliveryDate"].Value);

                EditSupplyForm form = new EditSupplyForm(supplyId, supplierId, itemId, supplyPrice, lastDeliveryDate);
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
            else if (selectedTable == "Employees")
            {
                if (dataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Будь ласка, виберіть працівника для редагування.");
                    return;
                }

                int id = Convert.ToInt32(row.Cells["EmployeeID"].Value);
                string firstName = row.Cells["FirstName"].Value.ToString();
                string lastName = row.Cells["LastName"].Value.ToString();
                string position = row.Cells["Position"].Value.ToString();
                DateTime hireDate = Convert.ToDateTime(row.Cells["HireDate"].Value);
                string phone = row.Cells["Phone"].Value.ToString();
                string email = row.Cells["Email"].Value.ToString();

                EditEmployeeForm form = new EditEmployeeForm(id, firstName, lastName, position, hireDate, phone, email);
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
            else if (selectedTable == "Warehouses")
            {
                if (dataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Будь ласка, виберіть склад для редагування.");
                    return;
                }

                int id = Convert.ToInt32(row.Cells["WarehouseID"].Value);
                string name = row.Cells["Name"].Value.ToString();
                string location = row.Cells["Location"].Value.ToString();
                string phone = row.Cells["Phone"].Value.ToString();

                EditWarehouseForm form = new EditWarehouseForm(id, name, location, phone);
                form.FormClosed += (s, args) => comboBoxTables_SelectedIndexChanged(null, null);
                form.ShowDialog();
            }
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (comboBoxTables.SelectedItem == null)
                return;

            string tableName = comboBoxTables.SelectedItem.ToString();

            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Оберіть запис для видалення.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
            string keyColumnName = dataGridView.Columns[0].Name;
            object keyValue = selectedRow.Cells[0].Value;

            if (keyValue == null)
            {
                MessageBox.Show("Не вдалося визначити значення ключа запису.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Ви впевнені, що хочете видалити запис з таблиці '{tableName}'?",
                "Підтвердження видалення",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = $"DELETE FROM [{tableName}] WHERE [{keyColumnName}] = @keyValue";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@keyValue", keyValue);

                    try
                    {
                        conn.Open();
                        int affectedRows = cmd.ExecuteNonQuery();

                        if (affectedRows > 0)
                        {
                            MessageBox.Show("Запис успішно видалено.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            comboBoxTables_SelectedIndexChanged(null, null); 
                        }
                        else
                        {
                            MessageBox.Show("Запис не знайдено або вже видалено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при видаленні: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
