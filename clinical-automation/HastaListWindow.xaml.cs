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
using System.Windows.Shapes;

namespace PoliklinikOtomasyon
{
    /// <summary>
    /// BookListWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class HastaListWindow : Window
    {
        private HastaInsertWindow _hastaInsertWindow;
        private HastaUpdateWindow _hastaUpdateWindow;
        public MainWindow mainWindow;
        private DbPoliklinikEntities db;
        private string cagrilanHastaName;
        private string cagrilanHastaId;
        public string _selectedId;
        public int yasliSayaci;

        private HastaListWindow()
        {
            InitializeComponent();

            Init();
            LoadValues();
            InitComponents();

            yasliSayaci = 0;
        }

        private void Init()
        {
            db = new DbPoliklinikEntities();
        }

        public static HastaListWindow GetInstance()
        {
            return new HastaListWindow();
        }

        public void InitComponents()
        {
            mrandevuBitirButton.Visibility = Visibility.Hidden;
            oncelikAtaButton.IsEnabled = false;
            kayitDuzenleButton.IsEnabled = false;
            randevuBitirButton.IsEnabled = false;
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

                hastaDataGrid.ItemsSource = query.Where(q => q.KayitTipi != AppData.GecmisRandevu).ToList();
            }
            catch (Exception)
            {
            }
        }

        private void bekleyenGosterButton_Click(object sender, RoutedEventArgs e)
        {
            InitComponents();
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

            hastaDataGrid.ItemsSource = query.Where(q => q.KayitTipi != AppData.GecmisRandevu).ToList();
        }

        private void gecmisGosterButton_Click(object sender, RoutedEventArgs e)
        {
            InitComponents();
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

            hastaDataGrid.ItemsSource = query.Where(q => q.KayitTipi == AppData.GecmisRandevu).ToList();
        }

        private void kayitDuzenleButton_Click(object sender, RoutedEventArgs e)
        {
            _hastaUpdateWindow = HastaUpdateWindow.GetInstance();
            _hastaUpdateWindow.hastaWindow = this;
            _hastaUpdateWindow._selectedId = _selectedId;
            _hastaUpdateWindow.bindData();
            _hastaUpdateWindow.Show();
        }

        private void randevuBitirButton_Click(object sender, RoutedEventArgs e)
        {
            bitirRandevu("NULL");
        }

        private void bitirRandevu(string ID)
        {
            try
            {
                int id;
                if (ID == "NULL")
                {
                    id = Convert.ToInt32(_selectedId);
                }
                else
                {
                    id = Convert.ToInt32(ID);
                }

                var record = db.TBLHASTA.Find(id);
                record.KayitTipi = AppData.GecmisRandevu;

                string messageBoxText = record.KimlikNo + " kimlik numaralı " + record.AdiSoyadi.ToUpper() +
                       " isimli hastanın randevusunu bitirmek istediğinizden emin misiniz?";
                string caption = "Emin misiniz?";

                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;

                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        db.SaveChanges();
                        MessageBox.Show("Randevu bitirildi", "Bilgi");
                        mainWindow.LoadValues();
                        break;
                    case MessageBoxResult.No:
                    case MessageBoxResult.Cancel:
                        break;
                }
                LoadValues();
            }
            catch (Exception)
            {
            }
        }

        private void hastaEkleButton_Click(object sender, RoutedEventArgs e)
        {
            _hastaInsertWindow = HastaInsertWindow.GetInstance();
            _hastaInsertWindow.hastaWindow = this;
            _hastaInsertWindow.mainWindow = mainWindow;
            _hastaInsertWindow.Show();
        }

        private void temizleButton_Click(object sender, RoutedEventArgs e)
        {
            InitComponents();
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

        private void hastaDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            oncelikAtaButton.IsEnabled = true;
            kayitDuzenleButton.IsEnabled = true;
            randevuBitirButton.IsEnabled = true;
            try
            {
                DataGridCellInfo cell = hastaDataGrid.SelectedCells[0];
                _selectedId = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text;
            } 
            catch (Exception)
            {
            }
        }

        private void hastaCagir()
        {
            db = new DbPoliklinikEntities();
            if (yasliSayaci % 2 == 0)
            {
                try
                {
                    var record = db.TBLHASTA.Where(q => q.KayitTipi != AppData.GecmisRandevu && q.RandevuTarihi <= DateTime.Now).ToList().OrderByDescending(q => q.RandevuTarihi).First();
                    cagrilanHastaId = record.Id.ToString();
                    cagrilanHastaName = record.AdiSoyadi;
                }
                catch (Exception)
                {
                    mrandevuBitirButton.Visibility = Visibility.Hidden;
                    labelHasta.Content = "";
                    MessageBox.Show("Bekleyen Hasta Yok", "Hasta Yok");
                }
            }
            else
            {
                int sayac = 0;
                while (true)
                {
                    try
                    {
                        var record = db.TBLHASTA.Where(q => q.KayitTipi != AppData.GecmisRandevu && q.RandevuTarihi <= DateTime.Now).ToList().OrderByDescending(q => q.RandevuTarihi).ElementAt(sayac);
                        double day = ((TimeSpan)(DateTime.Now - record.DogumTarihi)).TotalDays;
                        if (day >= 23375)
                        {
                            cagrilanHastaId = record.Id.ToString();
                            cagrilanHastaName = record.AdiSoyadi;
                            break;
                        }
                        sayac++;
                    }
                    catch (Exception)
                    {
                        try
                        {
                            var record = db.TBLHASTA.Where(q => q.KayitTipi != AppData.GecmisRandevu && q.RandevuTarihi <= DateTime.Now).ToList().OrderByDescending(q => q.RandevuTarihi).First();
                            cagrilanHastaId = record.Id.ToString();
                            cagrilanHastaName = record.AdiSoyadi;
                        }
                        catch (Exception)
                        {
                            mrandevuBitirButton.Visibility = Visibility.Hidden;
                            labelHasta.Content = "";
                            MessageBox.Show("Bekleyen Hasta Yok", "Hasta Yok");
                        }
                        break;
                    }
                }
            }
            yasliSayaci++;
       
        }

        private void mrandevuBitirButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var record = db.TBLHASTA.Find(Convert.ToInt32(cagrilanHastaId));

                string messageBoxText = cagrilanHastaName + " isimli hastanın randevusunu bitirmek için bitire basınınız, " +
                    "ileriki tarihe yeni bir randevu oluşturmak için randevu oluştura basınız.";

                DialogboxCustom dialogbox = new DialogboxCustom(messageBoxText);
                var result = (bool)dialogbox.ShowDialog();

                switch (result)
                {
                    case true:
                        record.KayitTipi = AppData.GecmisRandevu;
                        db.SaveChanges();
                        mainWindow.LoadValues();
                        labelHasta.Content = "";
                        mrandevuBitirButton.Visibility = Visibility.Hidden;
                        LoadValues();
                        MessageBox.Show("Randevu Bitirildi", "Başarılı");
                        break;
                    case false:
                        if (dialogbox.okey)
                        {
                            _hastaInsertWindow = HastaInsertWindow.GetInstance();
                            _hastaInsertWindow.hastaWindow = this;
                            _hastaInsertWindow.hastaNameTextBox.Text = record.AdiSoyadi;
                            _hastaInsertWindow.hastaTCnoTextBox.Text = record.KimlikNo;
                            _hastaInsertWindow.telephoneNoTextBox.Text = record.TelefonNo;
                            if (record.Cinsiyeti == AppData.Erkek)
                                _hastaInsertWindow.cinsiyetComboBox.SelectedIndex = 0;
                            else if (record.Cinsiyeti == AppData.Kadin || record.Cinsiyeti == AppData.KadinUtf8)
                                _hastaInsertWindow.cinsiyetComboBox.SelectedIndex = 1;
                            _hastaInsertWindow.randevuTipiComboBox.SelectedIndex = 1;
                            _hastaInsertWindow.hastaDatePixer.SelectedDate = record.DogumTarihi;
                            _hastaInsertWindow.mainWindow = mainWindow;
                            _hastaInsertWindow.randevuTipiComboBox.IsEnabled = false;
                            _hastaInsertWindow.Show();
                            try
                            {
                                var record2 = db.TBLHASTA.Find(Convert.ToInt32(cagrilanHastaId));
                                record2.KayitTipi = AppData.GecmisRandevu;
                                db.SaveChanges();
                                mainWindow.LoadValues();
                                LoadValues();
                            }
                            catch (Exception)
                            {
                            }
                            labelHasta.Content = "";
                            mrandevuBitirButton.Visibility = Visibility.Hidden;
                        }
                        break;
                }
            }
            catch (Exception)
            {
            }
            cagrilanHastaId = "";
            cagrilanHastaName = "";
        }

        private void hastaCagirButton_Click(object sender, RoutedEventArgs e)
        {
            cagrilanHastaName = "";
            cagrilanHastaId = "";
            
                hastaCagir();
                if (cagrilanHastaName != "" && cagrilanHastaName != null)
                {
                    mrandevuBitirButton.Visibility = Visibility.Visible;
                    labelHasta.Content = "İçerideki hasta :" + cagrilanHastaName;
                }
                else
                {
                    mrandevuBitirButton.Visibility = Visibility.Hidden;
                    labelHasta.Content = "";
                    MessageBox.Show("Bekleyen Hasta Yok", "Hasta Yok");
                }
            
         
        }

        private void oncelikAtaButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var record = db.TBLHASTA.Find(Convert.ToInt32(_selectedId));
                record.RandevuTarihi = DateTime.Now;
                db.SaveChanges();
                MessageBox.Show("Öncelik Atandı", "Başarılı");
                LoadValues();
            }
            catch (Exception)
            {
            }
        }
    }
}
