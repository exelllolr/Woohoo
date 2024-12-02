using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using dimplom.Model;
using dimplom.Controlers;
using dimplom.Pages;
using dimplom.Properties;

namespace dimplom.Pages
{
    public partial class RegPage : Page
    {
        private Users contextUser;

       public List<string> Genders { get; set; }

        public RegPage()
        {
            InitializeComponent();
            contextUser = new Users();
            DataContext = this;
            // Инициализация списка гендеров
            List<string> list = new List<string> { "М", "Ж" };
            Genders = list;
        }

        private void BBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateUser(contextUser);
                App.Db.Users.Add(contextUser);
                App.Db.SaveChanges();
                NavigationService.GoBack();
            }
            catch (ValidationException ex)
            {
                MessageBox.Show($"Validation error: {ex.Message}");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        MessageBox.Show($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
            }
        }

        private void ValidateUser(Users user)
        {
            var validationContext = new ValidationContext(user);
            Validator.ValidateObject(user, validationContext, validateAllProperties: true);
        }

        private void BBrowse_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                byte[] imageData = File.ReadAllBytes(imagePath);
                SavePhotoToDatabase(imageData);
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

                PhotoProfile.Source = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message);
            }
        }

        private void SavePhotoToDatabase(byte[] photoData)
        {
            var photo = new Photos
            {
                PhotoData = photoData,
                Users = contextUser
            };

            if (contextUser.Photos == null)
            {
                contextUser.Photos = new List<Photos>();
            }

            contextUser.Photos.Add(photo);
        }
    }
}