using Shared;
using System.Net.Http.Json;

var http = new HttpClient();
var random = new Random();

Console.WriteLine("--- SIMULADOR DE SENSORES INICIADO ---");
Console.WriteLine("Aguardando envios...\n");

while (true)
{
    var sensor = new SensorData
    {
        // Mantido o seu random original para forçar as temperaturas altas e dar o BadRequest!
        Temperatura = random.Next(20, 100),
        Pressao = random.Next(80, 120),
        Timestamp = DateTime.Now
    };

    try
    {
        var response = await http.PostAsJsonAsync("https://localhost:44379/api/v1/sensores", sensor);

        if (!response.IsSuccessStatusCode)
        {
            var erro = await response.Content.ReadAsStringAsync();

            // Pinta de vermelho para mostrar o BadRequest barrando o dado
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[BLOQUEADO] Erro: {response.StatusCode} - {erro}");
            Console.ResetColor();
        }
        else
        {
            // Pinta de verde quando a temperatura passa
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{sensor.Timestamp:HH:mm:ss}] ENVIADO -> Temp: {sensor.Temperatura}°C | Pressão: {sensor.Pressao} PSI");
            Console.ResetColor();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro de conexão: {ex.Message}");
    }

    await Task.Delay(2000);
}