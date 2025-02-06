using HealthMed.Application.Contracts;
using HealthMed.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using HealthMed.Data.DTO;
using AutoMapper;
using HealthMed.Application.Services;

namespace HealthMed.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
//public class AgendaController(IAgendaServices agendaServices, IBus _bus, IMapper _mapper) : ControllerBase
public class AgendaController(IAgendaServices agendaServices, IMapper _mapper) : ControllerBase
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

    /// <summary>
    /// Permite que um paciente marque um horário disponível.
    /// </summary>
    [HttpPost("AgendarHorario")]
    [Authorize(Roles = "paciente")]
    public async Task<IActionResult> AgendarHorario([FromBody] AgendamentoRequestDTO request)
    {
        var resultado = await agendaServices.AgendarHorarioAsync(request.IdPaciente, request.IdAgenda);

        if (!resultado.Sucesso)
            return BadRequest(resultado.Mensagem);

        return Ok(new { resultado.Mensagem });
    }

    [HttpPost]
    [Authorize(Roles = "medico")]
    public async Task<IActionResult> CreateAgenda([FromBody] CreateAgendaDTO agendadto)
    {
        try
        {
            if (agendadto == null)
            {
                return BadRequest("O corpo da requisição não pode estar vazio.");
            }

            AgendaEntity agenda = _mapper.Map<AgendaEntity>(agendadto);

            var result = await agendaServices.Create(agenda);

            return result ? CreatedAtAction(nameof(Get), new { id = agenda.IdAgenda }, agenda) : BadRequest(new { Message = "Ocorreu um erro ao criar a agenda. "  }); 


            //var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:CreateAgendamento"));

            //await endpoint.Send(agenda);

            //return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut()]
    [Authorize(Roles = "medico")]
    public async Task<IActionResult> UpdateAgenda([FromBody] UpdateAgendaDTO updatedagendadto)
    {
        try
        {
            if (updatedagendadto == null)
            {
                return BadRequest("O corpo da requisição não pode estar vazio.");
            }

            var agendaExistente = await agendaServices.GetById(updatedagendadto.IdMedico);
            if (agendaExistente == null)
            {
                return NotFound(new { Message = $"Médico com ID {updatedagendadto.IdMedico} não encontrado." });
            }


            _mapper.Map(updatedagendadto, agendaExistente);

            agendaServices.Update(agendaExistente);
            //var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:UpdateAgendamento"));

            //await endpoint.Send(updatedagenda);

            return NoContent();
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

            //var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:DeleteAgendamento"));

            //await endpoint.Send(existingAgendamento);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
