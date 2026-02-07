using System.Timers;
using CapiWater.Models;

namespace CapiWater.Services
{
    public class WaterTimerService
    {
        private System.Timers.Timer _timer;
        private UserSettings _settings;
        private int _secondsRemaining;

        // Eventos para a UI reagir
        public event Action OnWaterTimeReached;
        public event Action<int> OnTick; // Envia os segundos restantes

        public bool IsWaitingConfirmation { get; private set; }

        public WaterTimerService()
        {
            // Carrega as configurações via SettingsService
            _settings = SettingsService.Load();

            _timer = new System.Timers.Timer(1000); // Tick de 1 segundo
            _timer.Elapsed += HandleTimerElapsed;

            ResetTimer();
        }

        public void Start()
        {
            IsWaitingConfirmation = false;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void ResetTimer()
        {
            Stop();
            // Converte os minutos salvos em segundos para o contador
            _secondsRemaining = _settings.IntervalMinutes * 60;
            IsWaitingConfirmation = false;
            _timer.Start();
        }

        private void HandleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_secondsRemaining > 0)
            {
                _secondsRemaining--;
                OnTick?.Invoke(_secondsRemaining);
            }
            else
            {
                _timer.Stop();
                IsWaitingConfirmation = true;
                OnWaterTimeReached?.Invoke();
            }
        }

        // Método para quando você clicar na Capivara avisando que bebeu
        public void ConfirmDrinking()
        {
            // Aqui poderíamos lógica de meta diária no futuro
            ResetTimer();
        }

        // Caso você mude as configs, o serviço se atualiza
        public void RefreshSettings()
        {
            _settings = SettingsService.Load();
            ResetTimer();
        }
    }
}