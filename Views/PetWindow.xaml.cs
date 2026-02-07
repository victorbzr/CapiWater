using CapiWater.Models;
using CapiWater.Services;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace CapiWater.Views
{
    public partial class PetWindow : Window
    {
        private WaterTimerService _timerService;
        private Storyboard _bounceStoryboard;

        public PetWindow()
        {
            InitializeComponent();
            _bounceStoryboard = (Storyboard)this.Resources["BounceAnimation"];

            _timerService = new WaterTimerService();
            _timerService.OnWaterTimeReached += HandleThirstyState;
            _timerService.Start();
        }

        // Permite arrastar a capivara pela tela
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        // Abre o menu de contexto ao clicar com o botão direito na capivara
        private void Capi_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MyNotifyIcon.ContextMenu.IsOpen = true;
        }

        // Lógica de quando o tempo de beber água termina
        private void HandleThirstyState()
        {
            Dispatcher.Invoke(() =>
            {
                CapiImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/capi_thirsty.png"));
                SpeechBubble.Visibility = Visibility.Visible;

                // Inicia o quique!
                _bounceStoryboard.Begin();
            });
        }

        // Reinicia o timer após beber água
        protected override async void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            if (_timerService.IsWaitingConfirmation)
            {
                _bounceStoryboard.Stop();
                SpeechBubble.Visibility = Visibility.Collapsed;

                _timerService.ConfirmDrinking();
                CapiImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/capi_celebrate.png"));

                CapiImage.LayoutTransform = new ScaleTransform(1.2, 1.2); // Aumenta 20%
                await Task.Delay(3000);
                CapiImage.LayoutTransform = new ScaleTransform(1.0, 1.0); // Volta ao normal
                
                CapiImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/capi_idle.png"));
            }
        }

        // Eventos do Menu da Bandeja
        private void ShowPet_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
            this.Activate();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWin = new SettingsWindow();
            settingsWin.ShowDialog();
            RefreshSettings();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MyNotifyIcon.Dispose();
            Application.Current.Shutdown();
        }

        public void RefreshSettings()
        {
            _timerService.RefreshSettings();
            var settings = SettingsService.Load();
            this.Topmost = settings.KeepOnTop;
        }
    }
}