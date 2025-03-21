﻿//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Wordpress;

namespace BookGen.GeneratorSteps.Wordpress;

internal sealed class WriteExportXmlFile : IGeneratorStep
{
    private readonly Session _session;

    public WriteExportXmlFile(Session session)
    {
        _session = session;
    }


    public void RunStep(IReadonlyRuntimeSettings settings, ILogger log)
    {
        log.LogInformation("Writing wordpressExport.xml file...");
        FsPath outFile = settings.OutputDirectory.Combine("wordpressExport.xml");

        var namespaces = new List<(string, string)>
        {
            ("excerpt", "http://wordpress.org/export/1.2/excerpt/"),
            ("content", "http://purl.org/rss/1.0/modules/content/"),
            ("wfw", "http://wellformedweb.org/CommentAPI/"),
            ("dc", "http://purl.org/dc/elements/1.1/"),
            ("wp", "http://wordpress.org/export/1.2/")

        };

        var output = new Rss()
        {
            Version = "2.0",
            Channel = _session.CurrentChannel
        };

        outFile.SerializeXml<Rss>(output, log, namespaces);
    }
}
