using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PoliklinikOtomasyon
{
    /// <summary>
    /// BookDeleteWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class HastaDeleteWindow : Window
    {
        DbPoliklinikEntities db;
        private string _selectedId;
        public MainWindow MainWindow;

        private HastaDeleteWindow()
        {
            InitializeComponent();

            Init();
            LoadValues();
        }

        public static HastaDeleteWindow GetInstance()
        {
            return new HastaDeleteWindow();
        }

        private void Init()
        {
            db = new DbPoliklinikEntities();
        }

        public void LoadValues()
        {
            try
            {
                var query = from d1 in db.TBLHASTA
                             join d2 in db.TBLDOKTOR
                             on d1.DoktorId equals d2.Id
                             select new
                             {
                                 d1.Id,
                                 d1.AdiSoyadi,
                                 d1.KimlikNo,
                                 d1.TelefonNo,
                                 d1.Cinsiyeti,
                                 d1.DogumTarihi,
                                 d1.RandevuTarihi,
                                 d1.KayitTipi,
                                 DoktorAdi = d2.AdiSoyadi
                             };

                hastaDataGrid.ItemsSource = query.ToList();
            } 
            catch (Exception)
            {
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(_selectedId);
                var record = db.TBLHASTA.Find(id);

                string messageBoxText = record.KimlikNo + " kimlik numaralı " + record.AdiSoyadi.ToUpper() +
                    " isimli hastanın " + record.RandevuTarihi + " tarihli kaydını silmek istediğinizden emin misiniz?";
                string caption = "Emin misiniz?";

                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;

                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        db.TBLHASTA.Remove(record);
                        db.SaveChanges();
                        MessageBox.Show("Kayıt Silindi", "Bilgi");
                        MainWindow.LoadValues();
                        break;
                    case MessageBoxResult.No:
                    case MessageBoxResult.Cancel:
                        break;
                }
                LoadValues();
            } 
            catch(Exception)
            {
            }

            deleteButton.IsEnabled = false;
        }

        private void hastaDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridCellInfo cell = hastaDataGrid.SelectedCells[0];
            _selectedId = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text;
            deleteButton.IsEnabled = true;
        }

        private void hastaTcKimlikNoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = hastaTcKimlikNoTextBox.Text.Trim().ToLower();
            try
            {
                if (text != "")
                {
                    var query = from d1 in db.TBLHASTA
                                join d2 in db.TBLDOKTOR
                                on d1.DoktorId equals d2.Id
                                select new
                                {
                                    d1.Id,
                                    d1.AdiSoyadi,
                                    d1.KimlikNo,
                                    d1.TelefonNo,
                                    d1.Cinsiyeti,
                                    d1.DogumTarihi,
                                    d1.RandevuTarihi,
                                    d1.KayitTipi,
                                    DoktorAdi = d2.AdiSoyadi
                                };

                    hastaDataGrid.ItemsSource = query.Where(q => q.KimlikNo.StartsWith(text)).ToList();
                }
                else
                {
                    LoadValues();
                }
            }
            catch (Exception)
            {
            }

        }
    }
}
