using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using dimplom.Pages;


namespace dimplom.Pages
{

    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BRegistration_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegPage());
        }

        private void Blogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
          var loggedUser = App.Db.Users.FirstOrDefault(x => x.Login == TBLogin.Text && x.Password == PBPassword.Password); 
          if (loggedUser != null)
            {
                MessageBox.Show("Неправильный логин или пароль");
               return;
            }
            App.loggedUser = loggedUser;
          NavigationService.Navigate(new MainPage());
        }
    }
}
