using HealthMed.Application.Contracts;
using HealthMed.Data;
using HealthMed.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HealthMed.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class MedicoController(IMedicoServices medicoServices) : ControllerBase
{
    [HttpGet()]
    [Authorize(Roles = "paciente,medico")]
    public async Task<IActionResult> Get()
    {
        var result = await medicoServices.Get();
        return Ok(result);
    }

    [HttpGet("id")]
    [Authorize(Roles = "paciente,medico")]
    public async Task<IActionResult> GetbyId(int id)
    {
        var result = await medicoServices.GetById(id);
        return Ok(result);
    }

    [HttpGet("Nome")]
    [Authorize(Roles = "paciente,medico")]
    public async Task<IActionResult> GetbyNome(string Nome)
    {
        var result = await medicoServices.GetByNome(Nome);
        return Ok(result);
    }

    [HttpGet("CRM")]
    [Authorize(Roles = "paciente,medico")]
    public async Task<IActionResult> GetbyCRM(string CRM)
    {
        var result = await medicoServices.GetByCRM(CRM);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "medico")]
    public async Task<IActionResult> CreateMedico([FromBody] MedicoEntity medico)
    {
        if (medico == null)
        {
            return BadRequest("O corpo da requisição não pode estar vazio.");
        }

        var result = await medicoServices.Create(medico);

        return result ? CreatedAtAction(nameof(Get), new { id = medico.IdMedico }, medico) : BadRequest(new { Message = "Todos os campos (titulo, conteudo, lista) devem ser preenchidos." });

    }

    [HttpPut()]
    [Authorize(Roles = "medico")]
    public IActionResult UpdateMedico([FromBody] MedicoEntity updatedmedico)
    {
        if (updatedmedico == null)
        {
            return BadRequest("O corpo da requisição não pode estar vazio.");
        }

        var result = medicoServices.Update(updatedmedico);

        //if (result)
        //{
        //    logger.LogInformation("{Time} - Medico {Id} - {Titulo} - Alterado",
        //    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), id, updatedmedico.Titulo);
        //}


        //return result ? Ok(new { Message = "Cartão atualizado com sucesso." }) : BadRequest(new { Message = "Todos os campos (titulo, conteudo, lista) devem ser preenchidos." });
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "medico")]
    public async Task<IActionResult> DeleteMedico(int id)
    {
        var existingMedico = medicoServices.GetById(id);
        if (existingMedico == null)
        {
            return NotFound(new { Message = $"Nenhum cartão encontrado com o ID: {id}" });
        }
        var result = await medicoServices.Delete(id);
        if (!result)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Erro ao excluir o cartão." });
        }

        //logger.LogInformation("{Time} - Medico {Id} - {Titulo} - Removido",
        //DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), id, existingMedico.Titulo);

        var remainingMedicos = await medicoServices.Get();
        return Ok(remainingMedicos);
    }
}
