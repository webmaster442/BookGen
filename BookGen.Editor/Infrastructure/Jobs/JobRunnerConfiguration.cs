//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Threading;

namespace BookGen.Editor.Infrastructure.Jobs
{
    public class JobRunnerConfiguration<Tinput, Toutput>
    {
        public delegate Toutput JobFunctionDelegate(Tinput inputdata, IProgress<float> progress, CancellationToken ct);
        public Tinput JobInput { get; set; }
        public bool ReportTaskBarProgress { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public JobFunctionDelegate JobFunction { get; set; } 
    }
}
