using System.Net;

namespace BulkRedirects;

internal class BulkRedirects
{
    public static Dictionary<string, Redirect> ParseFile(string filepath)
    {
        var redirects = new Dictionary<string, Redirect>();
        var lines = File.ReadAllLines(filepath);

        foreach (var line in lines)
        {
            // skip empty lines and comments
            if (String.IsNullOrWhiteSpace(line) || line[0] == '#')
                continue;

            var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToArray();
            if (parts.Length < 2)
                continue;

            var from = parts[0];
            var to = parts[1];

            var statusCode = HttpStatusCode.MovedPermanently;
            if (parts.Length == 3 && parts[2] == "302")
                statusCode = HttpStatusCode.Found;

            if (!redirects.TryAdd(from, new Redirect { From = from, To = to, StatusCode = statusCode }))
                throw new Exception($"Redirect from '{from}' has multiple entries.");
        }

        return redirects;
    }
}

internal class Redirect
{
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public HttpStatusCode StatusCode { get; set; }
}
