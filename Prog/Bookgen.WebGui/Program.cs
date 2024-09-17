using System.Net.Mime;

using BookGen.Resources;
using BookGen.WebGui.Services;

using Microsoft.AspNetCore.Mvc.Formatters;

var builder = WebApplication.CreateBuilder(args);

static async Task ServeKnown(HttpContext context, KnownFile knownFile, string mime)
{
    context.Response.ContentType = mime;
    context.Response.StatusCode = StatusCodes.Status200OK;
    using var stream = ResourceHandler.GetFileStream(knownFile);
    await stream.CopyToAsync(context.Response.Body);
}

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ICurrentSession>(new CurrentSession(Environment.CurrentDirectory));
builder.Services.AddScoped<IMarkdownRenderer, MarkdownRenderer>();
builder.Services.AddScoped<IDocumentProvider, DocumentProvider>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.MapGet("/bootstrap.css", async context => await ServeKnown(context, KnownFile.BootstrapMinCss, MediaTypeNames.Text.Css));
app.MapGet("/bootstrap.js", async context => await ServeKnown(context, KnownFile.BootstrapMinJs, MediaTypeNames.Text.Css));

app.UseRouting();

app.MapRazorPages();

app.Run();
