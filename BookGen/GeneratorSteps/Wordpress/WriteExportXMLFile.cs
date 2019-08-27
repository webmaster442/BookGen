//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Domain;
using BookGen.Domain.wordpress;

namespace BookGen.GeneratorSteps.Wordpress
{
    internal class WriteExportXmlFile : IGeneratorStep
    {
        private readonly Session _session;

        public WriteExportXmlFile(Session session)
        {
            _session = session;
        }


        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Writing wordpressExport.xml file...");
            FsPath outFile = settings.OutputDirectory.Combine("wordpressExport.xml");
            outFile.SerializeXml<Channel>(_session.CurrentChannel);
        }
    }
}
