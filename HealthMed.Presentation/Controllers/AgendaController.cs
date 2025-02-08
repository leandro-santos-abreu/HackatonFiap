using HealthMed.Application.Contracts;
using HealthMed.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using HealthMed.Data.DTO;
using AutoMapper;
using System.Security.Claims;
using MassTransit.Middleware;

namespace HealthMed.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AgendaController(IAgendaServices agendaServices, IBus _bus, IMapper _mapper) : ControllerBase
//public class AgendaController(IAgendaServices agendaServices, IMapper _mapper) : ControllerBase
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
        //var resultado = await agendaServices.AgendarHorarioAsync(request.IdPaciente, request.IdAgenda);
        try
        {
            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:CreateAgendarHorario"));

            await endpoint.Send(request);

            //if (!resultado.Sucesso)
            //    return BadRequest(resultado.Mensagem);

            return Ok(); // new { resultado.Mensagem });

        }
        catch (Exception ex)
        {

            throw;
        }
    }

    /// <summary>
    /// Permite que um paciente marque um horário disponível.
    /// </summary>
    [HttpPost("CancelarAgendamento")]
    [Authorize(Roles = "paciente")]
    public async Task<IActionResult> CancelarAgendamento([FromBody] CancelarAgendaRequestDTO request)
    {
        //var resultado = await agendaServices.AgendarHorarioAsync(request.IdPaciente, request.IdAgenda);
        try
        {
            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:CancelarAgendamento"));

            await endpoint.Send(request);

            //if (!resultado.Sucesso)
            //    return BadRequest(resultado.Mensagem);

            return Ok(); // new { resultado.Mensagem });

        }
        catch (Exception ex)
        {

            throw;
        }
    }

    /// <summary>
    /// Permite que um paciente marque um horário disponível.
    /// </summary>
    [HttpPost("ConfirmarAgendamento")]
    [Authorize(Roles = "medico")]
    public async Task<IActionResult> ConfirmarAgendamento([FromBody] ConfirmarAgendaRequestDTO request)
    {
        try
        {
            //var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:CreateAgendarHorario"));

            //await endpoint.Send(request);
            // Buscar a agenda pelo ID correto
            var agendaExistente = await agendaServices.GetById(request.IdAgenda);
            if (agendaExistente == null)
            {
                return NotFound(new { Message = $"Agenda com ID {request.IdAgenda} não encontrada." });
            }

            // Obtém o ID do médico autenticado
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { Message = "Usuário não autenticado." });
            }

            // Verifica se o médico autenticado é o dono da agenda
            if (agendaExistente.IdMedico.ToString() != userId)
            {
                return Forbid("Não é possivel confirmar o agendamento de outro médico."); // Retorna 403 Forbidden
            }

            var resultado = await agendaServices.ConfirmaAgendamento(request.IdAgenda, request.AceitarAgendamento);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Mensagem);

            return Ok(); // new { resultado.Mensagem });

        }
        catch (Exception ex)
        {

            throw;
        }
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

            return CreatedAtAction(nameof(Get), new { id = result.IdAgenda }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new {ex.Message});
        }
    }



    [HttpPut]
    [Authorize(Roles = "medico")]
    public async Task<IActionResult> UpdateAgenda([FromBody] UpdateAgendaDTO updatedAgendaDto)
    {
        if (updatedAgendaDto == null)
        {
            return BadRequest(new { Message = "O corpo da requisição não pode estar vazio." });
        }

        // Buscar a agenda pelo ID correto
        var agendaExistente = await agendaServices.GetById(updatedAgendaDto.IdAgenda);
        if (agendaExistente == null)
        {
            return NotFound(new { Message = $"Agenda com ID {updatedAgendaDto.IdAgenda} não encontrada." });
        }

        // Obtém o ID do médico autenticado
        var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized(new { Message = "Usuário não autenticado." });
        }

        // Verifica se o médico autenticado é o dono da agenda
        if (agendaExistente.IdMedico.ToString() != userId)
        {
            return Forbid("Não é possivel modificar a agenda de outro médico."); // Retorna 403 Forbidden
        }

        // Atualiza os campos da agenda com os novos dados
        _mapper.Map(updatedAgendaDto, agendaExistente);

        // Chama o método assíncrono para salvar no banco
        await agendaServices.UpdateAsync(agendaExistente);

        //var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:UpdateAgendamento"));

        //await endpoint.Send(agendaExistente);

        return NoContent(); // Retorna 204 No Content quando atualizado com sucesso
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

            var result = await agendaServices.Delete(id);
            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Erro ao excluir o cartão." });
            }

            var remainingMedicos = await agendaServices.Get();
            
            return Ok(remainingMedicos);

            //var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:DeleteAgendamento"));

            //await endpoint.Send(existingAgendamento);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
