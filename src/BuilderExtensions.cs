using System.Net;
using Microsoft.AspNetCore.Builder;
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


        var redirects = BulkRedirects.ParseFile(Path.Combine(env.ContentRootPath, filePath));
        foreach (var redirect in redirects.Values)
        {
            var permanent = redirect.StatusCode == HttpStatusCode.MovedPermanently;
            endpoints.MapGet(redirect.From, context =>
            {
                context.Response.Redirect(redirect.To, permanent);
                return Task.CompletedTask;
            });
        }

        return endpoints;
    }
}
