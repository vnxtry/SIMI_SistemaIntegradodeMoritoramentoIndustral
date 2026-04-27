using ApiProcessamento.Data;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ApiProcessamento.Controllers
{
    /// <summary>
    /// Controller responsável pelo processamento e persistência dos dados dos sensores industriais.
    /// </summary>
    [ApiController]
    [Route("api/v1/sensores")]
    public class SensorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SensorController(AppDbContext context)
        {
            _context = context;
        }

        // =======================================================
        // NOVOS ENDPOINTS PARA O DESAFIO (CONFIGURAÇÃO)
        // =======================================================

        /// <summary>
        /// Recupera a temperatura máxima permitida salva no banco de dados SQLite.
        /// </summary>
        /// <returns>O objeto de configuração contendo o limite de temperatura.</returns>
        /// <response code="200">Retorna a configuração atual com sucesso.</response>
        [HttpGet("configuracao")]
        [ProducesResponseType(typeof(Configuracao), 200)]
        public async Task<IActionResult> ObterConfiguracao()
        {
            var config = await _context.Configuracoes.FirstOrDefaultAsync();
            if (config == null)
            {
                // Retorna um valor padrão seguro caso o WPF ainda não tenha salvo nada
                return Ok(new Configuracao { TemperaturaMaxima = 30.0 });
            }
            return Ok(config);
        }

        /// <summary>
        /// Atualiza ou insere o limite de temperatura máxima no banco de dados SQLite.
        /// </summary>
        /// <param name="novaTemperaturaMaxima">O novo valor de limite térmico.</param>
        /// <returns>O objeto de configuração atualizado.</returns>
        /// <response code="200">Retorna a configuração atualizada com sucesso.</response>
        [HttpPost("configuracao")]
        [ProducesResponseType(typeof(Configuracao), 200)]
        public async Task<IActionResult> SalvarConfiguracao([FromBody] double novaTemperaturaMaxima)
        {
            var config = await _context.Configuracoes.FirstOrDefaultAsync();

            if (config == null)
            {
                config = new Configuracao { TemperaturaMaxima = novaTemperaturaMaxima };
                _context.Configuracoes.Add(config);
            }
            else
            {
                config.TemperaturaMaxima = novaTemperaturaMaxima;
                _context.Configuracoes.Update(config);
            }

            await _context.SaveChangesAsync();
            return Ok(config);
        }

        // =======================================================
        // ENDPOINTS ORIGINAIS
        // =======================================================

        /// <summary>
        /// Recebe uma nova leitura de sensor, valida os limites térmicos e persiste no banco SQLite.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/v1/sensores
        ///     {
        ///        "temperatura": 25.5,
        ///        "pressao": 95.0,
        ///        "timestamp": "2023-10-27T10:00:00"
        ///     }
        /// </remarks>
        /// <param name="sensor">Objeto contendo os dados de telemetria do sensor.</param>
        /// <returns>O objeto recém-criado com seu ID gerado pelo banco de dados.</returns>
        /// <response code="201">Retorna o item criado e confirma a persistência.</response>
        /// <response code="400">Se a temperatura ultrapassar o limite definido no banco de dados.</response>
        [HttpPost]
        [ProducesResponseType(typeof(SensorData), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Receber(SensorData sensor)
        {
            // 1. Busca a regra de negócio direto do banco
            var config = await _context.Configuracoes.FirstOrDefaultAsync();
            double limiteTemperatura = config != null ? config.TemperaturaMaxima : 30.0;

            // 2. Valida contra o limite dinâmico
            if (sensor.Temperatura > limiteTemperatura)
            {
                return BadRequest($"Alerta Crítico: Temperatura ({sensor.Temperatura}ºC) acima do limite permitido de {limiteTemperatura}ºC.");
            }

            // O Banco gera o ID automaticamente
            sensor.Id = 0;

            _context.Sensores.Add(sensor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Listar), new { id = sensor.Id }, sensor);
        }

        /// <summary>
        /// Recupera todo o histórico de leituras armazenado no banco de dados SQLite.
        /// </summary>
        /// <returns>Uma lista de objetos SensorData.</returns>
        /// <response code="200">Retorna a lista de sensores com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SensorData>), 200)]
        public async Task<IActionResult> Listar()
        {
            var lista = await _context.Sensores.ToListAsync();
            return Ok(lista);
        }
    }
}