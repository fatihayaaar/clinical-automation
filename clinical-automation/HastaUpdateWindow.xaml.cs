using System;
using System.Collections;
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
    /// BookUpdateWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class HastaUpdateWindow : Window
    {
        public DbPoliklinikEntities db;
        public string _selectedId;
        public HastaListWindow hastaWindow;
        private bool vali;
        private string alanlar;

        private HastaUpdateWindow()
        {
            InitializeComponent();
            Init();
            LoadValues();
            LoadValuesCinsiyetComboBox();
            LoadValuesRandevuTipiComboBox();
            LoadValuesDoktorComboBox();
            InitComponents();
        }

        private void InitComponents()
        {
            hastaNameTextBox.Text = "";
            hastaTCnoTextBox.Text = "";
            cinsiyetComboBox.SelectedIndex = -1;
            hastaDatePixer.SelectedDate = null;
            hastaRandevuDatePixer.SelectedDate = null;
            telephoneNoTextBox.Text = "";
            randevuTipiComboBox.SelectedIndex = -1;
            doctorComboBox.SelectedIndex = -1;

            updateButton.IsEnabled = false;
            hastaNameTextBox.IsEnabled = false;
            hastaTCnoTextBox.IsEnabled = false;
            cinsiyetComboBox.IsEnabled = false;
            hastaDatePixer.IsEnabled = false;
            hastaRandevuDatePixer.IsEnabled = false;
            telephoneNoTextBox.IsEnabled = false;
            randevuTipiComboBox.IsEnabled = false;
            doctorComboBox.IsEnabled = false;

            vali = true;
            alanlar = "";
        }

        public static HastaUpdateWindow GetInstance()
        {
            return new HastaUpdateWindow();
        }

        private void Init()
        {
            db = new DbPoliklinikEntities();
        }

        private void LoadValuesCinsiyetComboBox()
        {
            cinsiyetComboBox.Items.Add(AppData.Erkek);
            cinsiyetComboBox.Items.Add(AppData.KadinUtf8);
        }

        private void LoadValuesRandevuTipiComboBox()
        {
            randevuTipiComboBox.Items.Add(AppData.NormalRandevu);
            randevuTipiComboBox.Items.Add(AppData.TekrarRandevu);
            randevuTipiComboBox.Items.Add(AppData.GecmisRandevu);
        }

        private void LoadValuesDoktorComboBox()
        {
            Hashtable hashtable = new Hashtable();
            List<TBLDOKTOR> data = db.TBLDOKTOR.ToList();
            foreach (TBLDOKTOR doktor in data)
            {
                hashtable.Add(doktor.Id, doktor.AdiSoyadi);
            }

            foreach (DictionaryEntry doktor in hashtable)
            {
                doctorComboBox.Items.Add(doktor);
            }
            doctorComboBox.DisplayMemberPath = "Value";
            doctorComboBox.SelectedValuePath = "Key";
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

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            Validate();
            if (vali)
            {
                updateData();
            }
            else
            {
                MessageBox.Show("Lütfen belirtilen alanları boş bırakmayınız : " + alanlar);
            }
            vali = true;
            alanlar = "";
        }

        public void updateData()
        {
            int id = Convert.ToInt32(_selectedId);
            var record = db.TBLHASTA.Find(id);
            record.AdiSoyadi = hastaNameTextBox.Text;
            record.KimlikNo = hastaTCnoTextBox.Text;
            record.TelefonNo = telephoneNoTextBox.Text;
            record.Cinsiyeti = cinsiyetComboBox.SelectedItem.ToString();
            record.DogumTarihi = hastaDatePixer.SelectedDate;
            record.RandevuTarihi = hastaRandevuDatePixer.SelectedDate;
            record.KayitTipi = randevuTipiComboBox.SelectedItem.ToString();
            record.DoktorId = Convert.ToInt32(((DictionaryEntry)doctorComboBox.SelectedItem).Key);

            string messageBoxText = record.KimlikNo + " kimlik numaralı " + record.AdiSoyadi.ToUpper() +
                   " isimli hastanın kaydını güncellemek istediğinizden emin misiniz?";
            string caption = "Emin misiniz?";

            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    db.SaveChanges();
                    MessageBox.Show("Kayıt Güncellendi", "Bilgi");
                    break;
                case MessageBoxResult.No:
                case MessageBoxResult.Cancel:
                    break;
            }
            LoadValues();
            InitComponents();

            if (hastaWindow != null)
            {
                hastaWindow.LoadValues();
            }
        }

        private void hastaDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGridCellInfo cell = hastaDataGrid.SelectedCells[0];
                _selectedId = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text;
            }
            catch (Exception)
            {

            }
            bindData();
        }

        public void bindData()
        {
            try
            {
                updateButton.IsEnabled = true;
                hastaNameTextBox.IsEnabled = true;
                hastaTCnoTextBox.IsEnabled = true;
                cinsiyetComboBox.IsEnabled = true;
                hastaDatePixer.IsEnabled = true;
                hastaRandevuDatePixer.IsEnabled = true;
                telephoneNoTextBox.IsEnabled = true;
                randevuTipiComboBox.IsEnabled = true;
                doctorComboBox.IsEnabled = true;

                int id = Convert.ToInt32(_selectedId);
                var record = db.TBLHASTA.Find(id);
                hastaNameTextBox.Text = record.AdiSoyadi;
                hastaTCnoTextBox.Text = record.KimlikNo;
                telephoneNoTextBox.Text = record.TelefonNo;
                hastaDatePixer.SelectedDate = record.DogumTarihi;
                hastaRandevuDatePixer.SelectedDate = record.RandevuTarihi;

                if (record.Cinsiyeti == AppData.Erkek)
                    cinsiyetComboBox.SelectedIndex = 0;
                else if (record.Cinsiyeti == AppData.Kadin || record.Cinsiyeti == AppData.KadinUtf8)
                    cinsiyetComboBox.SelectedIndex = 1;
                else
                    cinsiyetComboBox.SelectedIndex = -1;

                if (record.KayitTipi == AppData.NormalRandevu || record.KayitTipi == AppData.NormaRandevuNonUtf8)
                    randevuTipiComboBox.SelectedIndex = 0;
                else if (record.KayitTipi == AppData.TekrarRandevu)
                    randevuTipiComboBox.SelectedIndex = 1;
                else if (record.KayitTipi == AppData.GecmisRandevu || record.KayitTipi == AppData.GecmisRandevu2 || record.KayitTipi == AppData.GecmisRandevu3)
                    randevuTipiComboBox.SelectedIndex = 2;
                else randevuTipiComboBox.SelectedIndex = -1;

                doctorComboBox.SelectedIndex = -1;
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

        private void temizleButton_Click(object sender, RoutedEventArgs e)
        {
            InitComponents();
            LoadValues();
        }

        private void Validate()
        {
            if (hastaNameTextBox.Text.ToString().Trim() == "")
            {
                alanlar += "\nHasta adı";
                vali = false;
            }
            if (hastaTCnoTextBox.Text.ToString().Trim() == "")
            {
                alanlar += "\nHasta kimlik numarası";
                vali = false;
            }
            if (telephoneNoTextBox.Text.ToString().Trim() == "")
            {
                alanlar += "\nHasta telefon numarası";
                vali = false;
            }
            if (cinsiyetComboBox.SelectedItem == null || cinsiyetComboBox.SelectedIndex == -1)
            {
                alanlar += "\nHasta cinsiyeti";
                vali = false;
            }
            if (randevuTipiComboBox.SelectedItem == null || randevuTipiComboBox.SelectedIndex == -1)
            {
                alanlar += "\nHasta randevu tipi";
                vali = false;
            }
            if (doctorComboBox.SelectedItem == null || doctorComboBox.SelectedIndex == -1)
            {
                alanlar += "\nHastanın randevu aldığı doktor";
                vali = false;
            }
            if (hastaDatePixer.SelectedDate == null || hastaDatePixer.SelectedDate.ToString() == "")
            {
                alanlar += "\nHastanın dogum tarihi";
                vali = false;
            }
            if (hastaRandevuDatePixer.SelectedDate == null || hastaRandevuDatePixer.SelectedDate.ToString() == "")
            {
                alanlar += "\nHastanın randevu tipi";
                vali = false;
            }
        }
    }
}
