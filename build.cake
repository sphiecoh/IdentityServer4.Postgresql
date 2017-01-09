var target          = Argument("target", "Default");
var configuration   = Argument<string>("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var isLocalBuild        = !AppVeyor.IsRunningOnAppVeyor;
var sourcePath          = Directory("./src");
var testsPath           = Directory("test");
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

Task("RunUnitTests")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var projects = GetFiles("./test/**/project.json");

    foreach(var project in projects.GetDirectory().FullPath.Contains("UnitTests"))
	{
        var settings = new DotNetCoreTestSettings
        {
            Configuration = configuration
        };
       DotNetCoreTest(project.GetDirectory().FullPath, settings);
    }
});

Task("RunIntegrationTests")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var projects = GetFiles("./test/**/project.json");

    foreach(var project in projects.GetDirectory().FullPath.Contains("IntegrationTests"))
	{
        var settings = new DotNetCoreTestSettings
        {
            Configuration = configuration
        };
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
});

Task("Default")
  .IsDependentOn("Build")
  .IsDependentOn("RunUnitTests")
  .IsDependentOn("Pack");

RunTarget(target);