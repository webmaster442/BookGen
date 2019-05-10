var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var buildDir = Directory("./bin") + Directory(configuration);

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./BookGen.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
      MSBuild("./BookGen.sln", settings =>
			settings.SetConfiguration(configuration)
			.UseToolVersion(MSBuildToolVersion.VS2019)
			.SetVerbosity(Verbosity.Minimal));
});

Task("CleanRelasePDB")
	.IsDependentOn("Build")
	.Does(() =>
{
	var pdbs = GetFiles("./bin/Release/*.pdb");
	DeleteFiles(pdbs);
	var xmls = GetFiles("./bin/Release/*.xml");
	DeleteFiles(xmls);
});

Task("Default")
	.IsDependentOn("CleanRelasePDB");

RunTarget(target);