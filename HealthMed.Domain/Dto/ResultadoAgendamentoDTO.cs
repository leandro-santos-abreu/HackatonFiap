﻿namespace HealthMed.Data.DTO
{
    public class ResultadoAgendamentoDTO
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }

        public ResultadoAgendamentoDTO(bool sucesso, string mensagem)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
        }
    }
}
