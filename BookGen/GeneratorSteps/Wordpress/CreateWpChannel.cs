//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Domain;
using BookGen.Domain.wordpress;
using BookGen.Utilities;
using System;

namespace BookGen.GeneratorSteps.Wordpress
{
    internal class CreateWpChannel : IGeneratorStep
    {
        private readonly Session _session;

        public CreateWpChannel(Session session)
        {
            _session = session;
        }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            _session.CurrentChannel = new Channel
            {
                Title = settings.Configuration.Metadata.Title,
                Link = settings.CurrentBuildConfig.TemplateOptions[TemplateOptions.WordpressTargetHost],
                PubDate = DateTime.UtcNow.ToW3CTimeFormat(),
                Language = string.Empty,
                Wxr_version = "1.2",
                Base_site_url = settings.CurrentBuildConfig.TemplateOptions[TemplateOptions.WordpressTargetHost],
                Base_blog_url = settings.CurrentBuildConfig.TemplateOptions[TemplateOptions.WordpressTargetHost],
                Generator = "BookGen",
                Description = string.Empty,
                Author = new Author
                {
                    Author_display_name = settings.CurrentBuildConfig.TemplateOptions[TemplateOptions.WordpressAuthorDisplayName],
                    Author_email = settings.CurrentBuildConfig.TemplateOptions[TemplateOptions.WordpressAuthorEmail],
                    Author_first_name = settings.CurrentBuildConfig.TemplateOptions[TemplateOptions.WordpressAuthorFirstName],
                    Author_last_name = settings.CurrentBuildConfig.TemplateOptions[TemplateOptions.WordpressAuthorLastName],
                    Author_id = settings.CurrentBuildConfig.TemplateOptions[TemplateOptions.WordpressAuthorId],
                    Author_login = settings.CurrentBuildConfig.TemplateOptions[TemplateOptions.WordpressAuthorLogin]
                }
            };
        }
    }
}
