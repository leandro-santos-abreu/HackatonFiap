using HealthMed.Data.DTO;
using HealthMed.Domain.Dto;
using HealthMed.Domain.Entity;
using HealthMed.Presentation.Model;
using HealthMed.Tests.IntegrationTests.BaseClasses;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace HealthMed.Tests.IntegrationTests
{
    [TestFixture]
    public class AgendaIntegrationTest
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _factory = new CustomWebApplicationFactory();
            await _factory.InitializeAsync();
            _client = _factory.CreateClient();

        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await _factory.DisposeAsync();
            _client?.Dispose();
        }

        [Test]
        public async Task ShouldCreateAgendamentoAndAgendarSuccessfully()
        {
            #region Criar Médico
            var medico = new CreateMedicoDTO()
            {
                Nome = "Caio Maciente",
                CRM = "SP123456",
                Especialidade = "Oftalmologista",
                Email = "leandroa445@gmail.com",
                Senha = "123456",
                CPF = "68546380031"
            };
            
            var body = JsonSerializer.Serialize(medico);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("Medico", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            #endregion

            #region Criar Paciente
            var paciente = new CreatePacienteDTO()
            {
                Nome = "Cesar Pinto",
                CPF = "25797657350",
                Email = "cpinto@gmail.com",
                Senha = "123456"
            };

            body = JsonSerializer.Serialize(paciente);
            content = new StringContent(body, Encoding.UTF8, "application/json");

            response = await _client.PostAsync("Paciente", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            #endregion

            #region Criar Agenda

            var login = new LoginRequest()
            {
                CRM = "SP123456",
                Senha = "123456",
                CPF = "",
                Login = ""
            };

            body = JsonSerializer.Serialize(login);
            content = new StringContent(body, Encoding.UTF8, "application/json");

            response = await _client.PostAsync("auth/login", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));


            var agenda = new CreateAgendaDTO()
            {
                IdMedico = 1,
                HorarioDisponivel = new DateTime(2025, 02, 15, 10, 15, 0),
                ValorConsulta = 150
            };

            body = JsonSerializer.Serialize(agenda);
            content = new StringContent(body, Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Content.ReadAsStringAsync().Result.Replace("\"", "").Substring(7)[..^1]);

            response = await _client.PostAsync("Agenda", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            #endregion

            #region Agendar Consulta

            login = new LoginRequest()
            {
                CPF = "25797657350",
                Senha = "123456",
                Login = "",
                CRM = ""
            };

            body = JsonSerializer.Serialize(login);
            content = new StringContent(body, Encoding.UTF8, "application/json");

            response = await _client.PostAsync("auth/login", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var solicitacao = new AgendamentoRequestDTO()
            {
                IdAgenda = 1,
                IdPaciente = 1
            };

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (await response.Content.ReadAsStringAsync()).Replace("\"", "").Substring(7)[..^1]);
            body = JsonSerializer.Serialize(solicitacao);
            content = new StringContent(body, Encoding.UTF8, "application/json");
            response = await _client.PostAsync("Agenda/AgendarHorario", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));


            #endregion

            await Task.Delay(5000);

            var getResponse = await _client.GetAsync($"Agenda");
            IEnumerable<AgendaEntity> agendas = (await getResponse.Content.ReadFromJsonAsync<IEnumerable<AgendaEntity>>())!;

            Assert.That(agendas?.FirstOrDefault()?.IdPaciente > 0, Is.True);

        }
    }
}
