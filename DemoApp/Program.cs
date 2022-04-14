using BulkRedirects;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapGet("/", () => "hello");
app.MapGet("/world", () => "world");
app.UseBulkRedirects();

app.Run();
