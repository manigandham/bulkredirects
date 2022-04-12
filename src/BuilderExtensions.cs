﻿using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BulkRedirects;

public static class BuilderExtensions
{
    public static IEndpointRouteBuilder MapBulkRedirects(this IEndpointRouteBuilder endpoints, string filePath = "_redirects")
    {
        var env = endpoints.ServiceProvider.GetService<IHostEnvironment>();
        if (env is null)
            throw new Exception("Cannot resolve IHostEnvironment to get the path to the redirects file.");

        var redirects = ParseFile(Path.Combine(env.ContentRootPath, filePath));

        foreach (var redirect in redirects.Values)
        {
            endpoints.MapGet(redirect.From, () => Results.Redirect(redirect.To, permanent: redirect.StatusCode == HttpStatusCode.MovedPermanently));
        }

        return endpoints;
    }

    private static Dictionary<string, Redirect> ParseFile(string filepath)
    {
        var redirects = new Dictionary<string, Redirect>();
        var lines = File.ReadAllLines(filepath);

        foreach (var line in lines)
        {
            // skip empty lines and comments
            if (String.IsNullOrWhiteSpace(line) || line[0] == '#')
                continue;

            var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                continue;

            var from = parts[0];
            var to = parts[1];

            var statusCode = HttpStatusCode.MovedPermanently;
            if (parts.Length == 3 && parts[2] == "302")
                statusCode = HttpStatusCode.Found;

            if (!redirects.TryAdd(from, new Redirect(from, to, statusCode)))
                throw new Exception($"Redirect from '{from}' has multiple entries.");
        }

        return redirects;
    }
}

internal record Redirect(string From, string To, HttpStatusCode StatusCode);
