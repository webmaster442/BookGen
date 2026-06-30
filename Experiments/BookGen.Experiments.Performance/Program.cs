using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

using BookGen.Experiments.Performance;

var config = DefaultConfig.Instance;
var summary = BenchmarkRunner.Run<TemplateEngineBenchmarks>(config, args);
