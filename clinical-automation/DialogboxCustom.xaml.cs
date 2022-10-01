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
    /// DialogboxCustom.xaml etkileşim mantığı
    /// </summary>
    public partial class DialogboxCustom : Window
    {
        public bool okey;
        public DialogboxCustom(string message)
        {
            InitializeComponent();

            txtMessage.Text = message;
            okey = false;
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            okey = true;
            this.Close();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            okey = true;
            this.Close();
        }
    }
}
