//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Domain;
using BookGen.Domain.wordpress;
using BookGen.Framework;
using BookGen.Utilities;
using System;

namespace BookGen.GeneratorSteps.Wordpress
{
    internal class CreateWpPages : ITemplatedStep
    {
        public Template Template { get; set; }
        public IContent Content { get; set; }

        public void RunStep(RuntimeSettings settings, ILog log)
        {

        }
    }
}
