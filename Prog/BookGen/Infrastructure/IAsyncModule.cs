using BookGen.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookGen.Infrastructure
{
    internal interface IAsyncModule
    {
        Task<ModuleRunResult> ExecuteAsync(string[] arguments);
    }
}
