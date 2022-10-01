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
using System.Windows.Threading;

namespace PoliklinikOtomasyon
{
    /// <summary>
    /// UyeWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class DoctorWindow : Window
    {
        public DbPoliklinikEntities db;
        public string _selectedId;
        private bool vali;
        private string alanlar;

        private DoctorWindow()
        {
            InitializeComponent();

            Init();
            LoadValues();
            LoadValuesCinsiyetComboBox();

            vali = true;
        }

        private void LoadValuesCinsiyetComboBox()
        {
            cinsiyetComboBox.Items.Add(AppData.Erkek);
            cinsiyetComboBox.Items.Add(AppData.KadinUtf8);
        }

        private void LoadValues()
        {
            try
            {
                var query = from item in db.TBLDOKTOR
                            select new
                            {
                                item.Id,
                                item.AdiSoyadi,
                                item.KimlikNo,
                                item.TelefonNo,
                                item.Cinsiyeti,
                                item.MailAdresi,
                                item.DogumTarihi
                            };

                doctorDataGrid.ItemsSource = query.ToList();
            }
            catch (Exception)
            {
            }
        }

        private void Init()
        {
            db = new DbPoliklinikEntities();
            deleteButton.IsEnabled = false;
            editButton.IsEnabled = false;
        }

        private void InitComponents()
        {
            nameTextBox.Text = "";
            tcnoTextBox.Text = "";
            telNumberTextBox.Text = "";
            mailTextBox.Text = "";
            cinsiyetComboBox.SelectedIndex = -1;
            userDatePixer.SelectedDate = null;
            deleteButton.IsEnabled = false;
            editButton.IsEnabled = false;
            insertButton.IsEnabled = true;

            alanlar = "";
        }

        public static DoctorWindow GetInstance()
        {
            return new DoctorWindow();
        }

        private void insertButton_Click(object sender, RoutedEventArgs e)
        {
            Validate();
            if (vali)
            {
                TBLDOKTOR tblDoktor = new TBLDOKTOR();
                tblDoktor.AdiSoyadi = nameTextBox.Text;
                tblDoktor.KimlikNo = tcnoTextBox.Text;
                tblDoktor.TelefonNo = telNumberTextBox.Text;
                tblDoktor.Cinsiyeti = cinsiyetComboBox.SelectedItem.ToString();
                tblDoktor.MailAdresi = mailTextBox.Text;
                tblDoktor.DogumTarihi = userDatePixer.SelectedDate;

                string messageBoxText = "Yeni Bir Kayıt Oluşturmak istediğinizden emin misiniz?";
                string caption = "Emin misiniz?";

                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;

                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        db.TBLDOKTOR.Add(tblDoktor);
                        db.SaveChanges();
                        MessageBox.Show("Yeni Kayıt Oluşturuldu", "Bilgi");
                        break;
                    case MessageBoxResult.No:
                    case MessageBoxResult.Cancel:
                        break;
                }
                LoadValues();
                InitComponents();
            }
            else
            {
                MessageBox.Show("Lütfen belirtilen alanları boş bırakmayınız : " + alanlar);
            }
            vali = true;
            alanlar = "";
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(_selectedId);
                var record = db.TBLDOKTOR.Find(id);

                string messageBoxText = record.KimlikNo + " kimlik numaralı " + record.AdiSoyadi.ToUpper() +
                    " isimli doktorun kaydını silmek istediğinizden emin misiniz?";
                string caption = "Emin misiniz?";

                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;

                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        db.TBLDOKTOR.Remove(record);
                        db.SaveChanges();
                        MessageBox.Show("Kayıt Silindi", "Bilgi");
                        break;
                    case MessageBoxResult.No:
                    case MessageBoxResult.Cancel:
                        break;
                }
                LoadValues();
            }
            catch (Exception)
            {
                MessageBox.Show("Silme başarısız. Sileceğiniz doktorun bekleyen mevcut randevusu bulunmaktadır. Silmek için önce randevularını iptal ediniz yada randevunun hekimini değiştiriniz.", "Başarısız");
            }
            InitComponents();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            Validate();
            if (vali)
            {
                int id = Convert.ToInt32(_selectedId);
                var record = db.TBLDOKTOR.Find(id);

                record.AdiSoyadi = nameTextBox.Text;
                record.KimlikNo = tcnoTextBox.Text;
                record.TelefonNo = telNumberTextBox.Text;
                record.Cinsiyeti = cinsiyetComboBox.SelectedItem.ToString();
                record.MailAdresi = mailTextBox.Text;
                record.DogumTarihi = userDatePixer.SelectedDate;

                string messageBoxText = record.KimlikNo + " kimlik numaralı " + record.AdiSoyadi.ToUpper() +
                        " isimli doktorun kaydını güncellemek istediğinizden emin misiniz?";
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
            }
            else
            {
                MessageBox.Show("Lütfen belirtilen alanları boş bırakmayınız : " + alanlar);
            }
            vali = true;
            alanlar = "";
        }

        private void doctorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            insertButton.IsEnabled = false;

            DataGridCellInfo cell = doctorDataGrid.SelectedCells[0];
            _selectedId = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text;
            deleteButton.IsEnabled = true;
            editButton.IsEnabled = true;

            int id = Convert.ToInt32(_selectedId);
            var record = db.TBLDOKTOR.Find(id);
            nameTextBox.Text = record.AdiSoyadi;
            tcnoTextBox.Text = record.KimlikNo;
            telNumberTextBox.Text = record.TelefonNo;
            mailTextBox.Text = record.MailAdresi;

            if (record.Cinsiyeti == AppData.Erkek)
                cinsiyetComboBox.SelectedIndex = 0;
            else if (record.Cinsiyeti == AppData.Kadin || record.Cinsiyeti == AppData.KadinUtf8)
                cinsiyetComboBox.SelectedIndex = 1;
            else
                cinsiyetComboBox.SelectedIndex = -1;

            userDatePixer.SelectedDate = record.DogumTarihi;
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            InitComponents();
        }

        private void Validate()
        {
            if (nameTextBox.Text.ToString().Trim() == "")
            {
                alanlar += "\nDoktor Adı ve Soyadı";
                vali = false;
            }
            if (tcnoTextBox.Text.ToString().Trim() == "")
            {
                alanlar += "\nKimlik Numarası";
                vali = false;
            }
            if (telNumberTextBox.Text.ToString().Trim() == "")
            {
                alanlar += "\nTelefon Numarası";
                vali = false;
            }
            if (mailTextBox.Text.ToString().Trim() == "")
            {
                alanlar += "\nE-Mail Adresi";
                vali = false;
            }
            if (cinsiyetComboBox.SelectedItem == null || cinsiyetComboBox.SelectedIndex == -1)
            {
                alanlar += "\nCinsiyeti";
                vali = false;
            }
            if (userDatePixer.SelectedDate == null || userDatePixer.SelectedDate.ToString() == "")
            {
                alanlar += "\nDogum Tarihi";
                vali = false;
            }
        }
    }
}
