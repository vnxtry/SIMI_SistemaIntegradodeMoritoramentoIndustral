using Microsoft.EntityFrameworkCore;
using Shared; // Para enxergar a classe SensorData

namespace ApiProcessamento.Data
{
    // 1. Cria a classe que vai representar a nossa nova tabela no banco
    public class Configuracao
    {
        public int Id { get; set; }
        public double TemperaturaMaxima { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Tabela original
        public DbSet<SensorData> Sensores { get; set; }

        // 2. Adiciona a nova tabela de configurações ao Entity Framework
        public DbSet<Configuracao> Configuracoes { get; set; }
    }
}