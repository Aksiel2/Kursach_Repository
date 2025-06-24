using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SmartphoneStore
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            SetupDataGridView();
            RefreshData();
        }

        private void SetupDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Настройка столбцов
            dataGridView1.Columns.Add("Id", "ID");
            dataGridView1.Columns.Add("Brand", "Бренд");
            dataGridView1.Columns.Add("Model", "Модель");
            dataGridView1.Columns.Add("Price", "Цена");
            dataGridView1.Columns.Add("Storage", "Память (ГБ)");
            dataGridView1.Columns.Add("RAM", "ОЗУ (ГБ)");
            dataGridView1.Columns.Add("Processor", "Процессор");
            dataGridView1.Columns.Add("ScreenSize", "Экран (дюймы)");
            dataGridView1.Columns.Add("BatteryCapacity", "Батарея (mAh)");
            dataGridView1.Columns.Add("Quantity", "Количество");
        }

        private void RefreshData()
        {
            dataGridView1.Rows.Clear();
            DataTable data = DatabaseHelper.GetAllSmartphones();

            foreach (DataRow row in data.Rows)
            {
                dataGridView1.Rows.Add(
                    row["Id"],
                    row["Brand"],
                    row["Model"],
                    row["Price"],
                    row["Storage"],
                    row["RAM"],
                    row["Processor"],
                    row["ScreenSize"],
                    row["BatteryCapacity"],
                    row["Quantity"]);
            }

            statusLabel.Text = $"Все записи: {dataGridView1.Rows.Count}";
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            var form = new AddEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                DatabaseHelper.AddSmartphone(form.Smartphone);
                RefreshData();
            }
        }
       
        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            try
            {
                string searchText = searchTextBox.Text.Trim();
                DataTable searchResults = DatabaseHelper.SearchSmartphones(searchText);

                // Очищаем текущие строки
                dataGridView1.Rows.Clear();

                // Добавляем новые данные
                foreach (DataRow row in searchResults.Rows)
                {
                    dataGridView1.Rows.Add(
                        row["Id"],
                        row["Brand"],
                        row["Model"],
                        row["Price"],
                        row["Storage"],
                        row["RAM"],
                        row["Processor"],
                        row["ScreenSize"],
                        row["BatteryCapacity"],
                        row["Quantity"]);
                }

                // Обновляем статус
                if (string.IsNullOrEmpty(searchText))
                {
                    statusLabel.Text = $"Все записи: {dataGridView1.Rows.Count}";
                }
                else if (dataGridView1.Rows.Count == 0)
                {
                    statusLabel.Text = $"Записей не найдено: '{searchText}'";
                    MessageBox.Show($"Смартфоны с параметром '{searchText}' не найдены.",
                                  "Результаты поиска",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
                else
                {
                    statusLabel.Text = $"Найдено записей: {dataGridView1.Rows.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для редактирования", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            var smartphone = new Smartphone
            {
                Id = Convert.ToInt32(selectedRow.Cells["Id"].Value),
                Brand = selectedRow.Cells["Brand"].Value.ToString(),
                Model = selectedRow.Cells["Model"].Value.ToString(),
                Price = Convert.ToDecimal(selectedRow.Cells["Price"].Value),
                Storage = Convert.ToInt32(selectedRow.Cells["Storage"].Value),
                RAM = Convert.ToInt32(selectedRow.Cells["RAM"].Value),
                Processor = selectedRow.Cells["Processor"].Value.ToString(),
                ScreenSize = Convert.ToSingle(selectedRow.Cells["ScreenSize"].Value),
                BatteryCapacity = Convert.ToInt32(selectedRow.Cells["BatteryCapacity"].Value),
                Quantity = Convert.ToInt32(selectedRow.Cells["Quantity"].Value)
            };

            var form = new AddEditForm(smartphone);
            if (form.ShowDialog() == DialogResult.OK)
            {
                DatabaseHelper.UpdateSmartphone(form.Smartphone);
                RefreshData();
            }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить выбранную запись?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                DatabaseHelper.DeleteSmartphone(id);
                RefreshData();
            }
        }

        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick(); // Выполняем поиск при нажатии Enter
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(searchTextBox.Text))
            {
                btnSearch.PerformClick();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Yellow;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
        }
    }
}