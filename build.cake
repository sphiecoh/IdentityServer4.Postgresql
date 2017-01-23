var target          = Argument("target", "Default");
var configuration   = Argument<string>("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var isLocalBuild        = !AppVeyor.IsRunningOnAppVeyor;
var sourcePath          = Directory("./src");
var testsPath           = Directory("test");
var samplesPath			= Directory("sample");
var buildArtifacts      = Directory("./artifacts/packages");

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
	var projects = GetFiles("./**/project.json");

	foreach(var project in projects)
	{
        var settings = new DotNetCoreBuildSettings 
        {
            Configuration = configuration
            // Runtime = IsRunningOnWindows() ? null : "unix-x64"
        };

	    DotNetCoreBuild(project.GetDirectory().FullPath, settings); 
    }
});

Task("RunTests")
    .IsDependentOn("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var projects = GetFiles("./test/**/project.json");

    foreach(var project in projects)
	{
        var settings = new DotNetCoreTestSettings
        {
            Configuration = configuration
        };
        if((!IsRunningOnWindows() || !isLocalBuild )  && project.GetDirectory().FullPath.Contains("IntegrationTests"))
            continue;
         DotNetCoreTest(project.GetDirectory().FullPath, settings);
    }
});

Task("Pack")
    .IsDependentOn("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var settings = new DotNetCorePackSettings
    {
        Configuration = configuration,
        OutputDirectory = buildArtifacts,
    };
     if(!isLocalBuild)
    {
        settings.VersionSuffix = "build" + AppVeyor.Environment.Build.Number.ToString().PadLeft(5,'0');
    }
   var projects = GetFiles("./src/**/*.xproj");
    foreach(var project in projects)
  {
       DotNetCorePack(project.GetDirectory().FullPath, settings);
   
  }
    
});

Task("Clean")
    .Does(() =>
{
    CleanDirectories(new DirectoryPath[] { buildArtifacts });
});

Task("Restore")
    .Does(() =>
{
    var settings = new DotNetCoreRestoreSettings
    {
        Sources = new [] { "https://api.nuget.org/v3/index.json" }
    };

    DotNetCoreRestore(sourcePath, settings);
    DotNetCoreRestore(testsPath, settings);
	DotNetCoreRestore(samplesPath, settings);
});

Task("Default")
  .IsDependentOn("Build")
  .IsDependentOn("RunTests")
  .IsDependentOn("Pack");

RunTarget(target);