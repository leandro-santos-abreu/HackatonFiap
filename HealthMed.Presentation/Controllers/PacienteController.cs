using AutoMapper;
using HealthMed.Application.Contracts;
using HealthMed.Application.Services;
using HealthMed.Data.DTO;
using HealthMed.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class PacienteController(IPacienteServices pacienteServices, IMapper _mapper) : ControllerBase
{
    [HttpGet()]
    //[Authorize(Roles = "paciente")]
    public async Task<IActionResult> Get()
    {
        var result = await pacienteServices.Get();
        return Ok(result);
    }

    [HttpGet("id")]
    //[Authorize(Roles = "paciente")]
    public async Task<IActionResult> GetbyId(int id)
    {
        var result = await pacienteServices.GetById(id);
        return Ok(result);
    }

    //[Authorize(Roles = "paciente")]
    [HttpPost]
    public async Task<IActionResult> CreatePaciente([FromBody] CreatePacienteDTO pacientedto)
    {
        if (pacientedto == null)
        {
            return BadRequest("O corpo da requisição não pode estar vazio.");
        }

        PacienteEntity paciente = _mapper.Map<PacienteEntity>(pacientedto);

        var result = await pacienteServices.Create(paciente);

        ReadPacienteDTO pacienteRetorno = _mapper.Map<ReadPacienteDTO>(paciente);

        return result ? Ok(pacienteRetorno) : BadRequest(new { Message = "Todos os campos devem ser preenchidos." });
    }

    [HttpPut()]
    [Authorize(Roles = "paciente")]
    public async Task<IActionResult> UpdatePaciente([FromBody] UpdatePacienteDTO updatedpacientedto)
    {
        if (updatedpacientedto == null)
        {
            return BadRequest("O corpo da requisição não pode estar vazio.");
        }

        var pacienteExistente = await pacienteServices.GetById(updatedpacientedto.IdPaciente);
        if (pacienteExistente == null)
        {
            return NotFound(new { Message = $"Paciente com ID {updatedpacientedto.IdPaciente} não encontrado." });
        }

        _mapper.Map(updatedpacientedto, pacienteExistente);

        pacienteServices.Update(pacienteExistente);

        //if (result)
        //{
        //    logger.LogInformation("{Time} - Medico {Id} - {Titulo} - Alterado",
        //    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), id, updatedpaciente.Titulo);
        //}


        //return result ? Ok(new { Message = "Cartão atualizado com sucesso." }) : BadRequest(new { Message = "Todos os campos (titulo, conteudo, lista) devem ser preenchidos." });
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "paciente")]
    public async Task<IActionResult> DeletePaciente(int id)
    {
        var existingMedico = pacienteServices.GetById(id);
        if (existingMedico == null)
        {
            return NotFound(new { Message = $"Nenhum paciente encontrado com o ID: {id}" });
        }
        var result = await pacienteServices.Delete(id);
        if (!result)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Erro ao excluir o paciente." });
        }

        //logger.LogInformation("{Time} - Medico {Id} - {Titulo} - Removido",
        //DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), id, existingMedico.Titulo);

        var remainingMedicos = await pacienteServices.Get();
        return Ok(remainingMedicos);
    }

   
}
