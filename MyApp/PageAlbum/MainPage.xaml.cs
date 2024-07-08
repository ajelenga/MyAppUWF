using MyApp.Model;
using MyApp.Service;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MyApp
{
    public partial class MainPage : Page
    {
        public ObservableCollection<Album> Albums { get; set; }

        public MainPage()
        {
            InitializeComponent();
            Albums = new ObservableCollection<Album>();
            DataContext = this;

            LoadAlbumsAsync();
        }

        private async void LoadAlbumsAsync()
        {
            try
            {
                var service = new AlbumService();
                var result = await service.GetAllAsync();

                foreach (var item in result)
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        Albums.Add(item);
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading albums: {ex.Message}");
            }
        }
    }
}