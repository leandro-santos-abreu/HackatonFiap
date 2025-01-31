using HealthMed.Application.Contracts;
using HealthMed.Application.Services;
using HealthMed.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AgendaController(IAgendaServices agendaServices) : ControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        var result = await agendaServices.Get();
        return Ok(result);
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetbyId(int id)
    {
        var result = await agendaServices.GetById(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAgenda([FromBody] AgendaEntity agenda)
    {
        if (agenda == null)
        {
            return BadRequest("O corpo da requisição não pode estar vazio.");
        }

        var result = await agendaServices.Create(agenda);

        return result ? CreatedAtAction(nameof(Get), new { id = agenda.IdAgenda }, agenda) : BadRequest(new { Message = "Todos os campos (titulo, conteudo, lista) devem ser preenchidos." });

    }

    [HttpPut()]
    public IActionResult UpdateAgenda([FromBody] AgendaEntity updatedagenda)
    {
        if (updatedagenda == null)
        {
            return BadRequest("O corpo da requisição não pode estar vazio.");
        }

        var result = agendaServices.Update(updatedagenda);

        //if (result)
        //{
        //    logger.LogInformation("{Time} - Medico {Id} - {Titulo} - Alterado",
        //    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), id, updatedagenda.Titulo);
        //}


        //return result ? Ok(new { Message = "Cartão atualizado com sucesso." }) : BadRequest(new { Message = "Todos os campos (titulo, conteudo, lista) devem ser preenchidos." });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAgenda(int id)
    {
        var existingMedico = agendaServices.GetById(id);
        if (existingMedico == null)
        {
            return NotFound(new { Message = $"Nenhum cartão encontrado com o ID: {id}" });
        }
        var result = await agendaServices.Delete(id);
        if (!result)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Erro ao excluir o cartão." });
        }

        //logger.LogInformation("{Time} - Medico {Id} - {Titulo} - Removido",
        //DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), id, existingMedico.Titulo);

        var remainingMedicos = await agendaServices.Get();
        return Ok(remainingMedicos);
    }
}
