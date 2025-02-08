﻿using System.ComponentModel.DataAnnotations;

namespace HealthMed.Data.DTO;

public class ReadMedicoResumoDTO
{
    public string Nome { get; set; }
    public string CRM { get; set; }    
    public string Especialidade { get; set; }
}