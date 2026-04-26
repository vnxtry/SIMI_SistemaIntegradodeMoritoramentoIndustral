using Shared;
using System.Net.Http.Json;

var http = new HttpClient();
var random = new Random();

Console.WriteLine("--- SIMULADOR DE SENSORES INICIADO ---");

while (true)
{
    var sensor = new SensorData
    {
        // Removi o Id fixo para permitir que o SQLite gere automaticamente
        Temperatura = random.Next(20, 100),
        Pressao = random.Next(80, 120), // Simulando pressão industrial em PSI
        Timestamp = DateTime.Now
    };

    try
    {
        var response = await http.PostAsJsonAsync(
            "https://localhost:44379/api/v1/sensores", sensor);

        if (!response.IsSuccessStatusCode)
        {
            var erro = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Erro: {response.StatusCode} - {erro}");
        }
        else
        {
            Console.WriteLine($"[{sensor.Timestamp:HH:mm:ss}] Enviado -> Temp: {sensor.Temperatura}°C | Pressão: {sensor.Pressao} PSI");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro de conexão: {ex.Message}");
    }

    await Task.Delay(2000);
}