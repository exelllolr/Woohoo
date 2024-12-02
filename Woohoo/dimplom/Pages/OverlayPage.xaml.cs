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

namespace dimplom.Pages
{
    /// <summary>
    /// Логика взаимодействия для OverlayPage.xaml
    /// </summary>
    public partial class OverlayPage : Page
    {
        public OverlayPage()
        {
            InitializeComponent();
        }

        private void BEditProfile_Click(object sender, RoutedEventArgs e)
        {
            ((MainPage)((Frame)Application.Current.MainWindow.FindName("MainFrame")).Content).NavigateOverlay(new EditProfilePage());
        }

        private void BSetting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClosedProfileEdit_Click(object sender, RoutedEventArgs e)
        {
           ((MainPage)((Frame)Application.Current.MainWindow.FindName("MainFrame")).Content).CloseOverlay();
        }
    }
}
