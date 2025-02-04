using HealthMed.Application.Contracts;
using HealthMed.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AgendaController(IAgendaServices agendaServices, IBus _bus) : ControllerBase
{
    [HttpGet()]
    [Authorize(Roles = "paciente,medico")]
    public async Task<IActionResult> Get()
    {
        var result = await agendaServices.Get();
        return Ok(result);
    }

    [HttpGet("id")]
    [Authorize(Roles = "paciente,medico")]
    public async Task<IActionResult> GetbyId(int id)
    {
        var result = await agendaServices.GetById(id);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "medico")]
    public async Task<IActionResult> CreateAgenda([FromBody] AgendaEntity agenda)
    {
        try
        {
            if (agenda == null)
            {
                return BadRequest("O corpo da requisição não pode estar vazio.");
            }

            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:CreateAgendamento"));

            await endpoint.Send(agenda);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut()]
    [Authorize(Roles = "medico")]
    public async Task<IActionResult> UpdateAgenda([FromBody] AgendaEntity updatedagenda)
    {
        try
        {
            if (updatedagenda == null)
            {
                return BadRequest("O corpo da requisição não pode estar vazio.");
            }

            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:UpdateAgendamento"));

            await endpoint.Send(updatedagenda);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "medico")]
    public async Task<IActionResult> DeleteAgenda(int id)
    {
        try
        {
            var existingAgendamento = await agendaServices.GetById(id);
            if (existingAgendamento == null)
            {
                return NotFound(new { Message = $"Nenhum agendamento encontrado com o ID: {id}" });
            }

            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:DeleteAgendamento"));

            await endpoint.Send(existingAgendamento);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
