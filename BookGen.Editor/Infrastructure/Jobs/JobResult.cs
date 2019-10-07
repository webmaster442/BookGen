//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Editor.Infrastructure.Jobs
{
    public class JobResult<Toutput>
    {
        public Toutput Result { get; set; }
        public bool IsValid { get; set; }

        private JobResult() { }

        public static JobResult<Toutput> Create(Toutput output, bool valid)
        {
            return new JobResult<Toutput>
            {
                Result = output,
                IsValid = valid
            };
        }

        public static JobResult<Toutput> CanceledJob()
        {
            return new JobResult<Toutput>
            {
                Result = default,
                IsValid = false
            };
        }
    }
}
