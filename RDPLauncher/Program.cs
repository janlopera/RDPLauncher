using System.Net.Http.Json;
using CliWrap;
using RDPLauncher;

string baseUrl = "http://10.0.1.5";

HttpClient client = new HttpClient();
var res = await client.GetAsync($"{baseUrl}/Connect");

if (res.IsSuccessStatusCode)
{
    var conn = await res.Content.ReadFromJsonAsync<ConnectionModel>();

    Console.WriteLine($"Conn: {conn.IpAddress}");

    var result = await Cli.Wrap("/usr/local/bin/xfreerdp")
        .WithArguments($"/v:{conn!.IpAddress}:{conn!.Port} /f /u:admin /p:admin /d:  /network:auto /rfx /multimon /rfx-mode:video /cert:ignore")
        .WithValidation(CommandResultValidation.None)
        .ExecuteAsync();

    Console.WriteLine($"Uwu {result.ExitCode}");

    await client.PostAsync($"{baseUrl}/Release", JsonContent.Create<ConnectionModel>(conn));


    //await Cli.Wrap("systemctl poweroff").ExecuteAsync();
}
