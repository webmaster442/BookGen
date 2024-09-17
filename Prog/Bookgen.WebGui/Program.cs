using System.Net.Mime;

using BookGen.Cli;
using BookGen.Resources;
using BookGen.WebGui;
using BookGen.WebGui.Services;

var builder = WebApplication.CreateBuilder(args);

static async Task ServeKnown(HttpContext context, KnownFile knownFile, string mime)
{
    context.Response.ContentType = mime;
    context.Response.StatusCode = StatusCodes.Status200OK;
    using var stream = ResourceHandler.GetFileStream(knownFile);
    await stream.CopyToAsync(context.Response.Body);
}

var current = new CurrentSession();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ICurrentSession>(current);
builder.Services.AddScoped<IMarkdownRenderer, MarkdownRenderer>();
builder.Services.AddScoped<IDocumentProvider, DocumentProvider>();

var app = builder.Build();
ArgumentParser<Arguments> parser = new ArgumentParser<Arguments>(app.Logger);
current.StartDirectory = new BookGen.Interfaces.FsPath(parser.Parse(args).Directory);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.MapGet("/bootstrap.css", async context => await ServeKnown(context, KnownFile.BootstrapMinCss, MediaTypeNames.Text.Css));
app.MapGet("/bootstrap.js", async context => await ServeKnown(context, KnownFile.BootstrapMinJs, MediaTypeNames.Text.Css));

app.UseRouting();

app.MapRazorPages();

app.Run();
