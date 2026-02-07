using System.Windows;
using CapiWater.Models;
using CapiWater.Services;

namespace CapiWater.Views
{
    public partial class SettingsWindow : Window
    {
        private UserSettings _currentSettings;

        public SettingsWindow()
        {
            InitializeComponent();
            LoadCurrentSettings();
        }

        private void LoadCurrentSettings()
        {
            _currentSettings = SettingsService.Load();

            // Preenche a UI com os valores salvos
            SliderInterval.Value = _currentSettings.IntervalMinutes;
            SliderGoal.Value = _currentSettings.DailyGoalLiters;
            CheckTopmost.IsChecked = _currentSettings.KeepOnTop;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Atualiza o objeto com os valores da UI
            _currentSettings.IntervalMinutes = (int)SliderInterval.Value;
            _currentSettings.DailyGoalLiters = SliderGoal.Value;
            _currentSettings.KeepOnTop = CheckTopmost.IsChecked ?? true;

            // Salva no JSON
            SettingsService.Save(_currentSettings);

            // Notifica o usuário e fecha
            MessageBox.Show("Configurações salvas com sucesso! 🐾", "CapiWater", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }
    }
}