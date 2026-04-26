using System;
using System.ComponentModel.DataAnnotations;

namespace Shared
{
    public class SensorData
    {
        [Key] // Define explicitamente como chave primária para o Banco de Dados
        public int Id { get; set; }

        public double Temperatura { get; set; }

        // Novo sinal industrial escolhido: Pressão (medida em PSI ou Bar)
        public double Pressao { get; set; }

        public DateTime Timestamp { get; set; }
    }
}