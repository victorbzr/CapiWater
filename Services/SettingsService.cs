using System;
using System.IO;
using System.Text.Json;
using CapiWater.Models;

namespace CapiWater.Services
{
    public static class SettingsService
    {
        // Define o caminho: C:\Users\Nome\AppData\Roaming\CapiWater\settings.json
        private static readonly string FolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "CapiWater"
        );

        private static readonly string FilePath = Path.Combine(FolderPath, "settings.json");

        public static void Save(UserSettings settings)
        {
            try
            {
                // Garante que a pasta CapiWater existe
                if (!Directory.Exists(FolderPath))
                    Directory.CreateDirectory(FolderPath);

                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                //TODO: Criar um logger
                Console.WriteLine($"Erro ao salvar configurações: {ex.Message}");
            }
        }

        public static UserSettings Load()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    return JsonSerializer.Deserialize<UserSettings>(json) ?? new UserSettings();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar configurações: {ex.Message}");
            }

            // Se o arquivo não existir ou der erro, retorna as configurações padrão
            return new UserSettings();
        }
    }
}