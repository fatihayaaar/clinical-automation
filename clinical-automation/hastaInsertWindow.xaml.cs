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
    /// bookInsertPage.xaml etkileşim mantığı
    /// </summary>
    public partial class HastaInsertWindow : Window
    {
        public DbPoliklinikEntities db;
        public HastaListWindow hastaWindow;
        public MainWindow mainWindow;
        private bool vali;
        private string alanlar;

        private HastaInsertWindow()
        {
            InitializeComponent();

            Init();
            LoadValues();
            LoadValuesCinsiyetComboBox();
            LoadValuesRandevuTipiComboBox();
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
            vali = true;
            alanlar = "";
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
                alanlar += "\nHastanın randevu tarihi";
                vali = false;
            }
        }

        private void LoadValues()
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

        private void Init()
        {
            db = new DbPoliklinikEntities();
        }

        public static HastaInsertWindow GetInstance()
        {
            return new HastaInsertWindow();
        }
        

        private void insertButton_Click(object sender, RoutedEventArgs e)
        {
            Validate();
            if (vali)
            {
                try
                {
                    TBLHASTA tBLHASTA = new TBLHASTA();
                    tBLHASTA.AdiSoyadi = hastaNameTextBox.Text;
                    tBLHASTA.KimlikNo = hastaTCnoTextBox.Text;
                    tBLHASTA.Cinsiyeti = cinsiyetComboBox.SelectedItem.ToString();
                    tBLHASTA.DogumTarihi = hastaDatePixer.SelectedDate.Value.Date;
                    tBLHASTA.RandevuTarihi = hastaRandevuDatePixer.SelectedDate;
                    tBLHASTA.TelefonNo = telephoneNoTextBox.Text;
                    tBLHASTA.KayitTipi = randevuTipiComboBox.SelectedItem.ToString();
                    tBLHASTA.DoktorId = Convert.ToInt32(((DictionaryEntry)doctorComboBox.SelectedItem).Key);

                    db.TBLHASTA.Add(tBLHASTA);
                    db.SaveChanges();

                    MessageBox.Show("Yeni Kayıt Oluşturuldu", "Başarılı");
                    mainWindow.LoadValues();
                    InitComponents();
                    if (hastaWindow != null)
                    {
                        hastaWindow.LoadValues();
                        Close();
                    }
                }
                catch (Exception)
                {
                }
            }
            else 
            {
                MessageBox.Show("Lütfen belirtilen alanları boş bırakmayınız : " + alanlar);
            }
            vali = true;
            alanlar = "";
        }
    }
}
