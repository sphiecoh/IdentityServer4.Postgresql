#tool "nuget:?package=xunit.runner.console"

var target          = Argument("target", "Default");
var configuration   = Argument<string>("configuration", "Release");
var framework		= Argument<string>("tfm",string.Empty);

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
	var settings = new DotNetCoreBuildSettings
     {
         Framework = framework,
         Configuration = configuration
     };
	DotNetCoreBuild("IdentityServer4.Postgresql.sln", settings); 
   
});

Task("RunTests")
    .Does(() =>
{
	var settings = new DotNetCoreTestSettings{
		NoBuild = true,
		Configuration = "Release"
	}; 
	
	DotNetCoreTest("./test/IdentityServer4.Postgresql.UnitTests/IdentityServer4.Postgresql.UnitTests.csproj");
	 if(isLocalBuild)
	 DotNetCoreTest("./test/IdentityServer4.Postgresql.IntegrationTests/IdentityServer4.Postgresql.IntegrationTests.csproj");
	
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
  //.IsDependentOn("RunTests")
  .IsDependentOn("Pack");

RunTarget(target);
