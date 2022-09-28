using System.Diagnostics;

namespace WpLoad.Infrastructure
{
    internal sealed class ProgressReporter
    {
        private readonly ILog _log;
        private readonly Stopwatch _watch;
        private long _total;

        public ProgressReporter(ILog log)
        {
            _watch = new Stopwatch();
            _log = log;
        }

        public void Start() => _watch.Start();
        public void Stop() => _watch.Stop();

        public void Report(string path)
        {
            var file = new FileInfo(path);
            _total += file.Length;

            _log.Info($"\tAvg speed: {CalculateSpeed():0.00} KiB/s");
        }

        private float CalculateSpeed()
        {
            float seconds = _watch.ElapsedMilliseconds / 1000.0f;
            float kibs = _total / 1024.0f;
            return kibs / seconds;
        }
    }
}
