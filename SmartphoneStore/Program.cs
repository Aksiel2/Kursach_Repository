using System;
using System.Windows.Forms;

namespace SmartphoneStore
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Инициализация базы данных перед запуском формы
            DatabaseHelper.InitializeDatabase();

            Application.Run(new Form1());
        }
    }
}