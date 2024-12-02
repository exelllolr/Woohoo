using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using dimplom.Pages;
using dimplom.Properties;
using dimplom.Model;
using GalaSoft.MvvmLight;
using System.Data.SqlClient;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace dimplom.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>

    public partial class MainPage : Page
    {
        private Stack<Page> overlayHistory = new Stack<Page>();
        private string connectionString = "Data Source=WooHooEntities1;Initial Catalog=WooHooEntities1;Integrated Security=True";
        private int currentProfileId = 1;
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };

        public MainPage()
        {
            InitializeComponent();
            Task task = LoadChatsAsync();
            LoadProfile (currentProfileId);
            
        }

        private void BProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigateOverlay(new OverlayPage());
        }

        public void NavigateOverlay(Page overlayPage)
        {
            if (OverlayFrame.Visibility == Visibility.Visible)
            {
                if (OverlayFrameTop.Visibility == Visibility.Visible)
                {
                    overlayHistory.Push(OverlayFrameTop.Content as Page);
                }
                else
                {
                    overlayHistory.Push(OverlayFrame.Content as Page);
                    OverlayFrameTop.Visibility = Visibility.Visible;
                }

                OverlayFrameTop.Navigate(overlayPage);
            }
            else
            {
                OverlayFrame.Navigate(overlayPage);
                OverlayFrame.Visibility = Visibility.Visible;
            }
        }

        public void GoBackOverlay()
        {
            if (overlayHistory.Count > 0)
            {
                var previousOverlay = overlayHistory.Pop();

                if (OverlayFrameTop.Visibility == Visibility.Visible)
                {
                    if (overlayHistory.Count == 0)
                    {
                        OverlayFrameTop.Visibility = Visibility.Collapsed;
                    }

                    OverlayFrameTop.Navigate(previousOverlay);
                }
                else
                {
                    OverlayFrame.Navigate(previousOverlay);
                    OverlayFrame.Visibility = Visibility.Visible;
                }
            }
            else
            {
                OverlayFrame.Visibility = Visibility.Collapsed;
                OverlayFrameTop.Visibility = Visibility.Collapsed;
            }
        }

        public void CloseOverlay()
        {
            overlayHistory.Clear();
            OverlayFrame.Visibility = Visibility.Collapsed;
            OverlayFrameTop.Visibility = Visibility.Collapsed;
        }

        private void BNext_Click(object sender, RoutedEventArgs e)
        {
            currentProfileId++;
            LoadProfile(currentProfileId);
        }
        private void LoadProfile(int profileId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Photo, Name, Age FROM Profiles WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", profileId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string photoPath = reader["Photo"].ToString();
                        string name = reader["Name"].ToString();
                        int age = Convert.ToInt32(reader["Age"]);

                        // Загрузка и отображение фотографии
                        if (!string.IsNullOrEmpty(photoPath))
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(photoPath, UriKind.RelativeOrAbsolute);
                            bitmap.EndInit();
                            IViewer.Source = bitmap;
                        }

                       TBName.Text = name;
                        TBAge.Text = age.ToString() + " лет";
                    }
                    else
                    {
                        MessageBox.Show("Анкета не найдена.");
                    }
                }
            }
        }
        public interface IChatService
        {
            IEnumerable<Chats> GetAll();
            Chats GetById(int id);
            void Add(Chats chat);
            void Update(Chats chat);
            void Delete(int id);
        }

        public class ChatService : IChatService
        {
            private readonly List<Chats> _chats = new List<Chats>();

            public IEnumerable<Chats> GetAll()
            {
                return _chats;
            }

            public Chats GetById(int id)
            {
                return _chats.FirstOrDefault(chat => chat.Id == id);
            }

            public void Add(Chats chat)
            {
                chat.Id = _chats.Count > 0 ? _chats.Max(c => c.Id) + 1 : 1;
                _chats.Add(chat);
            }

            public void Update(Chats chat)
            {
                var existingChat = GetById(chat.Id);
                if (existingChat != null)
                {
                    existingChat.Name = chat.Name;
                }
            }

            public void Delete(int id)
            {
                var chat = GetById(id);
                if (chat != null)
                {
                    _chats.Remove(chat);
                }
            }
        }
        private async Task LoadChatsAsync()
        {
            try
            {
                var chats = await _httpClient.GetFromJsonAsync<List<Chat>>("api/chats");
                ChatListBox.ItemsSource = chats;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading chats: {ex.Message}");
            }
        }

        private async void BAddChat_Click(object sender, RoutedEventArgs e)
        {
            var chat = new Chat { Name = ChatNameTextBox.Text };
            var response = await _httpClient.PostAsJsonAsync("api/chats", chat);
            if (response.IsSuccessStatusCode)
            {
                ChatNameTextBox.Clear();
                await LoadChatsAsync();
            }
            else
            {
                MessageBox.Show("Error adding chat");
            }
        }

        private async void BUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (ChatListBox.SelectedItem is Chat selectedChat)
            {
                selectedChat.Name = ChatNameTextBox.Text;
                var response = await _httpClient.PutAsJsonAsync($"api/chats/{selectedChat.Id}", selectedChat);
                if (response.IsSuccessStatusCode)
                {
                    await LoadChatsAsync();
                }
                else
                {
                    MessageBox.Show("Error updating chat");
                }
            }
        }

        private async void BDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ChatListBox.SelectedItem is Chat selectedChat)
            {
                var response = await _httpClient.DeleteAsync($"api/chats/{selectedChat.Id}");
                if (response.IsSuccessStatusCode)
                {
                    await LoadChatsAsync();
                }
                else
                {
                    MessageBox.Show("Error deleting chat");
                }
            }
        }
    }

    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

   

    
