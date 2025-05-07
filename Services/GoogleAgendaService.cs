using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;

public class GoogleAgendaService
{
    private static readonly string[] Scopes = { CalendarService.Scope.Calendar };
    private const string ApplicationName = "ThayNutri";

    public string CriarEvento(string pacienteNome, DateTime data, TimeSpan hora, string observacao)
    {
        UserCredential credential;

        using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        {
            string credPath = "token.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
        }

        var service = new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        var evento = new Event()
        {
            Summary = $"Paciente: {pacienteNome} às {hora:hh\\:mm}",
            Description = observacao ?? "",
            Start = new EventDateTime()
            {
                DateTime = data.Date + hora,
                TimeZone = "America/Sao_Paulo"
            },
            End = new EventDateTime()
            {
                DateTime = data.Date + hora + TimeSpan.FromMinutes(60),
                TimeZone = "America/Sao_Paulo"
            }
        };

        var createdEvent = service.Events.Insert(evento, "primary").Execute();
        return createdEvent.Id;
    }

    public void ExcluirEvento(string eventId)
    {
        try
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            service.Events.Delete("primary", eventId).Execute();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao excluir evento do Google Agenda: " + ex.Message);
            // Você pode registrar em log, se quiser
        }
    }
}
