using ApiProcessamento.Config;
using ApiProcessamento.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared;
using Microsoft.EntityFrameworkCore;

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
        private readonly IOptions<ApiConfig> _config;

        public SensorController(AppDbContext context, IOptions<ApiConfig> config)
        {
            _context = context;
            _config = config;
        }

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
        /// <response code="400">Se a temperatura ultrapassar o limite definido no ApiConfig.</response>
        [HttpPost]
        [ProducesResponseType(typeof(SensorData), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Receber(SensorData sensor)
        {
            // Validação baseada no ApiConfig (launchSettings/appsettings)
            if (sensor.Temperatura > _config.Value.MaxTemperatura)
            {
                return BadRequest($"Alerta Crítico: Temperatura ({sensor.Temperatura}ºC) acima do limite permitido.");
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