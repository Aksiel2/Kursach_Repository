using System;
using System.Windows.Forms;

namespace SmartphoneStore
{
    public partial class AddEditForm : Form
    {
        public Smartphone Smartphone { get; private set; }

        public AddEditForm()
        {
            InitializeComponent();
            Text = "Добавить новый смартфон";
            Smartphone = new Smartphone();
            SetupForm();
        }

        public AddEditForm(Smartphone smartphone) : this()
        {
            Text = "Редактировать смартфон";
            Smartphone = smartphone;
            FillFields();
        }

        private void SetupForm()
        {
          
        }

        private void FillFields()
        {
            txtBrand.Text = Smartphone.Brand;
            txtModel.Text = Smartphone.Model;
            txtPrice.Text = Smartphone.Price.ToString("0.00");
            txtStorage.Text = Smartphone.Storage.ToString();
            txtRAM.Text = Smartphone.RAM.ToString();
            txtProcessor.Text = Smartphone.Processor;
            txtScreenSize.Text = Smartphone.ScreenSize.ToString("0.00");
            txtBattery.Text = Smartphone.BatteryCapacity.ToString();
            txtQuantity.Text = Smartphone.Quantity.ToString();
        }
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtBrand.Text)) return ShowError("Введите бренд");
            if (string.IsNullOrWhiteSpace(txtModel.Text)) return ShowError("Введите модель");
            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0) return ShowError("Некорректная цена");
            if (!int.TryParse(txtStorage.Text, out int storage) || storage <= 0) return ShowError("Некорректный объем памяти");
            if (!int.TryParse(txtRAM.Text, out int ram) || ram <= 0) return ShowError("Некорректный объем ОЗУ");
            if (string.IsNullOrWhiteSpace(txtProcessor.Text)) return ShowError("Введите процессор");
            if (!float.TryParse(txtScreenSize.Text, out float size) || size <= 0) return ShowError("Некорректный размер экрана");
            if (!int.TryParse(txtBattery.Text, out int battery) || battery <= 0) return ShowError("Некорректная емкость батареи");
            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0) return ShowError("Некорректное количество");

            return true;
        }

        private bool ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            Smartphone.Brand = txtBrand.Text;
            Smartphone.Model = txtModel.Text;
            Smartphone.Price = decimal.Parse(txtPrice.Text);
            Smartphone.Storage = int.Parse(txtStorage.Text);
            Smartphone.RAM = int.Parse(txtRAM.Text);
            Smartphone.Processor = txtProcessor.Text;
            Smartphone.ScreenSize = float.Parse(txtScreenSize.Text);
            Smartphone.BatteryCapacity = int.Parse(txtBattery.Text);
            Smartphone.Quantity = int.Parse(txtQuantity.Text);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}