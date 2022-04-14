# Bulk Redirects

Serve multiple redirects from a single text config file. Based on the `_redirects` format from [Netlify](https://docs.netlify.com/routing/redirects/#syntax-for-the-redirects-file).

Built for `net6.0`, `net5.0`, `netcoreapp3.1`.

-   Local path to local path.
-   Local path to remote URL.
-   HTTP 301 Moved Permanently (default) or 302 Found.

## Install

Package from Nuget: https://www.nuget.org/packages/BulkRedirects

```
dotnet add package BulkRedirects
```

Add to application routing:

```csharp
var builder = WebApplication.CreateBuilder(args);

// place where needed in the pipeline
app.UseBulkRedirects();

app.Run();
```

## Config

-   Looks for `_redirects` file in the [application content root](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-6.0#contentroot) by default, but custom path can be specified.
-   One redirect per line with format: `source` path, `destination` path, and optional `status` code, all separated by spaces or tabs.
-   Blank lines and lines starting with `#` are ignored.
-   _Note: Ensure that it's included in the build output._

Example file:

```
# comments and blank lines are allowed

/page1  /gohereinstead
/page2  /goheretemporarily   302
/remotepage   https://github.com

```

## Changelog

**0.4.0**

-   Renamed to `UseBulkRedirects`.
-   Custom file path and access checks.

**0.3.0**

-   Added [Source Link](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/sourcelink) for debugging.
-   Added logging statements on startup.
-   Fixed trimming in redirect entires.

**0.1.0**

-   Initial release.
