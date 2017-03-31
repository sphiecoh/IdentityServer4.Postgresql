#tool "nuget:?package=xunit.runner.console"

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
	DotNetBuild("IdentityServer4.Postgresql.sln", settings =>
		settings.SetConfiguration("Release")
        .WithTarget("Build")); 
   
});

Task("RunTests")
    .Does(() =>
{
	var settings = new DotNetCoreTestSettings{
		NoBuild = true,

	}; 
	
	DotNetCoreTest("./test/IdentityServer4.Postgresql.UnitTests/IdentityServer4.Postgresql.UnitTests.csproj",settings);
	 if(isLocalBuild)
	 DotNetCoreTest("./test/IdentityServer4.Postgresql.IntegrationTests/IdentityServer4.Postgresql.IntegrationTests.csproj",settings);
	
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
   var projects = GetFiles("./src/**/*.csproj");
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

    DotNetCoreRestore("IdentityServer4.Postgresql.sln", settings);
  
});

Task("Default")
  .IsDependentOn("Build")
  .IsDependentOn("RunTests")
  .IsDependentOn("Pack");

RunTarget(target);