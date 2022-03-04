//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpLoad.Domain;
using WpLoad.Infrastructure;

namespace WpLoad.Commands
{
    internal class Down : LoadCommandBase
    {
        public override string CommandName => nameof(Down);

        public override async Task<ExitCode> Execute(ILog log, IReadOnlyList<string> arguments)
        {

        }
    }
}
