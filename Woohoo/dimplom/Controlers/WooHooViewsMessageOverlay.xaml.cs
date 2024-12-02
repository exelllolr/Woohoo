using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using dimplom.Model;

namespace dimplom.Controlers
{
    /// <summary>
    /// Логика взаимодействия для WooHooViewsMessageOverlay.xaml
    /// </summary>
    public partial class WooHooViewsMessageOverlay : UserControl
    {
        public WooHooViewsMessageOverlay()
        {
            InitializeComponent();
        }

        private void BSendReply_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Message message)
            {
                string reply = ReplyTextBox.Text;
                // Логика отправки ответа
            }
            
        }
    }
}
