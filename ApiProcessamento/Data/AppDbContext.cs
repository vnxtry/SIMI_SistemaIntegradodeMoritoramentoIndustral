using Microsoft.EntityFrameworkCore;
using Shared; // Para enxergar a classe SensorData

namespace ApiProcessamento.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Esta propriedade vira a tabela "Sensores" no SQLite
        public DbSet<SensorData> Sensores { get; set; }
    }
}