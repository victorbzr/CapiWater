using System;

namespace CapiWater.Models
{
    public class UserSettings
    {
        // Intervalo entre os lembretes (Padrão: 40 minutos)
        public int IntervalMinutes { get; set; } = 40;

        // Meta diária em Litros (Padrão: 2.0L)
        public double DailyGoalLiters { get; set; } = 2.0;

        // Volume de cada "copo" que você confirma (Padrão: 0.25L ou 250ml)
        public double GlassSizeLiters { get; set; } = 0.25;

        // Se o pet deve ficar sempre no topo de outras janelas
        public bool KeepOnTop { get; set; } = true;

        // Caminho para o tema/imagem do pet (para expansões futuras)
        public string PetTheme { get; set; } = "CapyDefault";

        // Horário que o dia "reseta" (útil se você trabalha de madrugada)
        public TimeSpan ResetTime { get; set; } = new TimeSpan(0, 0, 0);
    }
}