//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki G�bor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net.Mime;

using BookGen.Cli;
using BookGen.Resources;
using BookGen.WebGui;
using BookGen.WebGui.Services;

using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;

using static System.Net.Mime.MediaTypeNames;

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
builder.Services.AddSingleton<IFileService, FileService>();
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
app.MapGet("/tex-svg-full.js", async context => await ServeKnown(context, KnownFile.MathJax, MediaTypeNames.Text.JavaScript));

app.MapGet("/Raw", async ([FromServices]IFileService files, string id, HttpContext context) =>
{
    await using var content = files.GetContent(id);
    context.Response.ContentType = files.GetMimeTypeOf(id);
    context.Response.StatusCode = StatusCodes.Status200OK;
    await content.CopyToAsync(context.Response.Body);
});


app.UseRouting();

app.MapRazorPages();

void PrintUrlInfos()
{
    Console.WriteLine("Webserver started.");
    var server = app.Services.GetRequiredService<IServer>();
    var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

    if (serverAddressesFeature != null)
    {
        foreach (var address in serverAddressesFeature.Addresses)
        {
            Console.WriteLine($"Application is listening on: {address}");
        }
    }

    Console.WriteLine("Press Ctrl+C to stop the application.");
}

app.Start();

PrintUrlInfos();

await app.WaitForShutdownAsync();
