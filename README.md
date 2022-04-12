# Bulk Redirects

Add multiple redirects from a single text config file. This is based on the `_redirects` format from [Netlify](https://docs.netlify.com/routing/redirects/#syntax-for-the-redirects-file).

Built for `net6.0`, `net5.0`, `netcoreapp3.1`

### Install

Install package from Nuget: https://www.nuget.org/packages/BulkRedirects

```
dotnet add package BulkRedirects
```

```csharp
var builder = WebApplication.CreateBuilder(args);

// add in pipeline where redirect endpoints should be added
app.MapBulkRedirects();

```

### Usage

Place `_redirects` file in the root of project and ensure it's in included in the build output.

Example `_redirects` file:

```
# comments and blank lines are allowed

/page1 /gohereinstead
/page2 /gosomewhereelse 302
/remotepage https://github.com

```
