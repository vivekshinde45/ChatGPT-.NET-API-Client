using System.Text;
using Newtonsoft.Json;

if (args.Length > 0)
{
    var postAPIurl = "https://api.openai.com/v1/completions";
    var openaiAPIKey = "YOUR_API_KEY";
    HttpClient client = new HttpClient();

    client.DefaultRequestHeaders.Add("authorization", $"Bearer {openaiAPIKey}");
    var content = new StringContent("{\"model\": \"text-davinci-001\", \"prompt\": \"" + args[0] + "\", \"temperature\": 1, \"max_tokens\": 100}",
        Encoding.UTF8, "application/json");

    HttpResponseMessage response = await client.PostAsync(postAPIurl, content);
    var responseString = await response.Content.ReadAsStringAsync();

    try
    {
        var dynamicData = JsonConvert.DeserializeObject<dynamic>(responseString);

        var guess = GuessCommand(dynamicData!.choices[0].text);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"--> My guess at the command prompt is: {guess}");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> cannot able to deseriealize {ex.Message}");
    }

    Console.WriteLine(responseString);
}
else
{
    Console.WriteLine("---> You need to provide some input");
}


static string GuessCommand(string raw)
{
    Console.WriteLine("--> GPT-3 API Returned Text:");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(raw);

    var lastIndex = raw.LastIndexOf("\n");

    var guess = raw.Substring(lastIndex + 1);
    Console.ResetColor();

    // copy guess to clipboard
    TextCopy.ClipboardService.SetText(guess);

    return guess;
}
