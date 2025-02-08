using AutoMapper;
using HealthMed.Application.Contracts;
using HealthMed.Data.DTO;
using HealthMed.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class MedicoController(IMedicoServices medicoServices, IMapper _mapper) : ControllerBase
{
    [HttpGet()]
    //[Authorize(Roles = "paciente,medico")]
    public async Task<IActionResult> Get()
    {
        var result = await medicoServices.Get();
        return Ok(result);
    }

    [HttpGet("id")]
    //[Authorize(Roles = "paciente,medico")]
    public async Task<IActionResult> GetbyId(int id)
    {
        var result = await medicoServices.GetById(id);
        return Ok(result);
    }

    [HttpGet("Nome")]
    //[Authorize(Roles = "paciente,medico")]
    public async Task<IActionResult> GetbyNome(string Nome)
    {
        var result = await medicoServices.GetByNome(Nome);
        return Ok(result);
    }

    [HttpGet("CRM")]
    //[Authorize(Roles = "paciente,medico")]
    public async Task<IActionResult> GetbyCRM(string CRM)
    {
        var result = await medicoServices.GetByCRM(CRM);
        return Ok(result);
    }

    [HttpGet("Especialidade")]
    //[Authorize(Roles = "paciente,medico")]
    public async Task<IActionResult> GetbyEspecialidade(string Especialidade)
    {
        var result = await medicoServices.GetByEspecialidade(Especialidade);
        return Ok(result);
    }

    [HttpPost]
    //[Authorize(Roles = "medico")]
    public async Task<IActionResult> CreateMedico([FromBody] CreateMedicoDTO medicodto)
    {
        if (medicodto == null)
        {
            return BadRequest("O corpo da requisição não pode estar vazio.");
        }

        MedicoEntity medico = _mapper.Map<MedicoEntity>(medicodto);

        var result = await medicoServices.Create(medico);

        ReadMedicoResumoDTO medicoRetorno = _mapper.Map<ReadMedicoResumoDTO>(medico);

        return result ? Ok(medicoRetorno) : BadRequest(new { Message = "Todos os campos devem ser preenchidos." });

    }

    [HttpPut()]
    [Authorize(Roles = "medico")]
    public async Task<IActionResult> UpdateMedico([FromBody] UpdateMedicoDTO updatedmedicodto)
    {
        if (updatedmedicodto == null)
        {
            return BadRequest("O corpo da requisição não pode estar vazio.");
        }

        var medicoExistente = await medicoServices.GetById(updatedmedicodto.IdMedico);
        if (medicoExistente == null)
        {
            return NotFound(new { Message = $"Médico com ID {updatedmedicodto.IdMedico} não encontrado." });
        }


        _mapper.Map(updatedmedicodto, medicoExistente);

        medicoServices.Update(medicoExistente);

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
            return NotFound(new { Message = $"Nenhum médico encontrado com o ID: {id}" });
        }
        var result = await medicoServices.Delete(id);
        if (!result)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Erro ao excluir o médico." });
        }

        //logger.LogInformation("{Time} - Medico {Id} - {Titulo} - Removido",
        //DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), id, existingMedico.Titulo);

        var remainingMedicos = await medicoServices.Get();
        return Ok(remainingMedicos);
    }
}
