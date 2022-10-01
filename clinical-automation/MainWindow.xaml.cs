using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PoliklinikOtomasyon
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window _hastaUpdateWindow;
        private HastaDeleteWindow _hastaDeleteWindow;
        private HastaInsertWindow _hastaInsertWindow;
        private HastaListWindow _hastaListWindow;
        private Window _doctorWindow;
        private Window _aboutWindow;
        public DbPoliklinikEntities db;

        public MainWindow()
        {
            InitializeComponent();
            db = new DbPoliklinikEntities();
            LoadValues();
        }

        public void LoadValues()
        {
            try
            {
                var count = db.TBLHASTA.Where(q => q.KayitTipi != AppData.GecmisRandevu).Count();
                label.Text = "Bekleyen Randevular: " + count;
            }
            catch (Exception)
            {
                label.Text = "Bekleyen Randevular: 0";
            }
        }

        private void hastaUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            _hastaUpdateWindow = HastaUpdateWindow.GetInstance();
            _hastaUpdateWindow.Show();
        }

        private void hastaInsertButton_Click(object sender, RoutedEventArgs e)
        {
            _hastaInsertWindow = HastaInsertWindow.GetInstance();
            _hastaInsertWindow.mainWindow = this;
            _hastaInsertWindow.Show();
        }

        private void hastaDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            _hastaDeleteWindow = HastaDeleteWindow.GetInstance();
            _hastaDeleteWindow.MainWindow = this;
            _hastaDeleteWindow.Show();
        }

        private void hastaListButton_Click(object sender, RoutedEventArgs e)
        {
            _hastaListWindow = HastaListWindow.GetInstance();
            _hastaListWindow.mainWindow = this;
            _hastaListWindow.Show();
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            _aboutWindow = AboutWindow.GetInstance();
            _aboutWindow.Show();
        }

        private void doctorButton_Click(object sender, RoutedEventArgs e)
        {
            _doctorWindow = DoctorWindow.GetInstance();
            _doctorWindow.Show();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
