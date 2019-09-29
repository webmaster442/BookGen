//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace BookGen.Editor.Infrastructure.Jobs
{
    public static class JobRunner
    {
        public static async Task<JobResult<Toutput>> GetJobResultAsync<Tinput, Toutput>(JobRunnerConfiguration<Tinput, Toutput> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var jobWindow = new JobWindow(configuration.JobTitle, configuration.JobDescription, configuration.ReportTaskBarProgress);
            try
            {
                Toutput result = default;
                Application.Current.Dispatcher.Invoke(() => jobWindow.Show());
                result = await Task.Run(() => configuration.Job.JobFunction(configuration.JobInput, jobWindow.Reporter, jobWindow.CancelToken)).ConfigureAwait(false);
                Application.Current.Dispatcher.Invoke(() => jobWindow.Close());
                return JobResult<Toutput>.Create(result, true);
            }
            catch (OperationCanceledException ex)
            {
                jobWindow.Close();
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine("Task Cancelced: {0}", ex);
                }
                return JobResult<Toutput>.CanceledJob();
            }
        }
    }
}
