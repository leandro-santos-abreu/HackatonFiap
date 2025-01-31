using HealthMed.Application.Contracts;
using HealthMed.Application.Services;
using HealthMed.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class PacienteController(IPacienteServices pacienteServices) : ControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        var result = await pacienteServices.Get();
        return Ok(result);
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetbyId(int id)
    {
        var result = await pacienteServices.GetById(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePaciente([FromBody] PacienteEntity paciente)
    {
        if (paciente == null)
        {
            return BadRequest("O corpo da requisição não pode estar vazio.");
        }

        var result = await pacienteServices.Create(paciente);

        return result ? CreatedAtAction(nameof(Get), new { id = paciente.IdPaciente }, paciente) : BadRequest(new { Message = "Todos os campos (titulo, conteudo, lista) devem ser preenchidos." });

    }

    [HttpPut()]
    public IActionResult UpdatePaciente([FromBody] PacienteEntity updatedpaciente)
    {
        if (updatedpaciente == null)
        {
            return BadRequest("O corpo da requisição não pode estar vazio.");
        }

        var result = pacienteServices.Update(updatedpaciente);

        //if (result)
        //{
        //    logger.LogInformation("{Time} - Medico {Id} - {Titulo} - Alterado",
        //    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), id, updatedpaciente.Titulo);
        //}


        //return result ? Ok(new { Message = "Cartão atualizado com sucesso." }) : BadRequest(new { Message = "Todos os campos (titulo, conteudo, lista) devem ser preenchidos." });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePaciente(int id)
    {
        var existingMedico = pacienteServices.GetById(id);
        if (existingMedico == null)
        {
            return NotFound(new { Message = $"Nenhum cartão encontrado com o ID: {id}" });
        }
        var result = await pacienteServices.Delete(id);
        if (!result)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Erro ao excluir o cartão." });
        }

        //logger.LogInformation("{Time} - Medico {Id} - {Titulo} - Removido",
        //DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), id, existingMedico.Titulo);

        var remainingMedicos = await pacienteServices.Get();
        return Ok(remainingMedicos);
    }
}
