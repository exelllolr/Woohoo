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
using dimplom.Pages;
using dimplom.Properties;

namespace dimplom.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditProfilePage.xaml
    /// </summary>
    public partial class EditProfilePage : Page
    {
        public EditProfilePage()
        {
            InitializeComponent();
        }


        private void BClose_Click(object sender, RoutedEventArgs e)
        {
            ((MainPage)((Frame)Application.Current.MainWindow.FindName("MainFrame")).Content).CloseOverlay();
        }

        private void BGoBack_Click(object sender, RoutedEventArgs e)
        {
            ((MainPage)((Frame)Application.Current.MainWindow.FindName("MainFrame")).Content).GoBackOverlay();
        }

        private void BEditPhoto_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                LoadImage(imagePath);
            }
        }
        private void LoadImage(string imagePath)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(imagePath);
                bitmap.EndInit();

                EPhotoProfile.Source = bitmap; // ProfileImage - имя вашего элемента управления Image
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message);
            }
        }
    }
}
