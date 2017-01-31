//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
#tool nuget:?package=NUnit.ConsoleRunner&version=3.6.0

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// SET ERROR LEVELS
//////////////////////////////////////////////////////////////////////

var ErrorDetail = new List<string>();

//////////////////////////////////////////////////////////////////////
// SET PACKAGE VERSION
//////////////////////////////////////////////////////////////////////

var version = "1.0.0";
var modifier = "";

var isAppveyor = BuildSystem.IsRunningOnAppVeyor;
var dbgSuffix = configuration == "Debug" ? "-dbg" : "";
var packageVersion = version + modifier + dbgSuffix;

//////////////////////////////////////////////////////////////////////
// SUPPORTED FRAMEWORKS
//////////////////////////////////////////////////////////////////////

var WindowsFrameworks = new string[] {
    "net-4.5" };

var LinuxFrameworks = new string[] {
    "net-4.5" };

var AllFrameworks = IsRunningOnWindows() ? WindowsFrameworks : LinuxFrameworks;

//////////////////////////////////////////////////////////////////////
// DEFINE RUN CONSTANTS
//////////////////////////////////////////////////////////////////////

var PROJECT_DIR = Context.Environment.WorkingDirectory.FullPath + "/";
var PACKAGE_DIR = PROJECT_DIR + "package/";
var BIN_DIR = PROJECT_DIR + "bin/" + configuration + "/";
var IMAGE_DIR = PROJECT_DIR + "images/";

//var SOLUTION_FILE = IsRunningOnWindows()
//    ? "./commercetools.NET-Net45.sln"
//    : "./commercetools.NET-Net45.sln";

// Package sources for nuget restore
var PACKAGE_SOURCE = new string[]
    {
        "https://www.nuget.org/api/v2"
    };

// Test Assemblies
var SDK_TESTS = "commercetools.NET.Tests.dll";

bool isDotNetCoreInstalled = false;

var packages = new string[]{
    "commercetools.NET/packages.config",
    "commercetools.NET.Tests/packages.config"
};

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{
    Information("Building version {0} of c.", packageVersion);

    isDotNetCoreInstalled = CheckIfDotNetCoreInstalled();
});

//////////////////////////////////////////////////////////////////////
// CLEAN
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Description("Deletes all files in the BIN directory")
    .Does(() =>
    {
        CleanDirectory(BIN_DIR);
    });


//////////////////////////////////////////////////////////////////////
// INITIALIZE FOR BUILD
//////////////////////////////////////////////////////////////////////

Task("InitializeBuild")
    .Description("Initializes the build")
    .Does(() =>
    {
        foreach(var package in packages)
        {
            Information("Restoring NuGet package " + package);
            NuGetRestore(package, new NuGetRestoreSettings
            {
                PackagesDirectory = "./packages/",
                Source = PACKAGE_SOURCE
            });
        }

        if(isDotNetCoreInstalled)
        {
            Information("Restoring .NET Core packages");
            StartProcess("dotnet", new ProcessSettings
            {
                Arguments = "restore"
            });
        }

        if (isAppveyor)
        {
            var tag = AppVeyor.Environment.Repository.Tag;

            if (tag.IsTag)
            {
                packageVersion = tag.Name;
            }
            else
            {
                var buildNumber = AppVeyor.Environment.Build.Number.ToString("00000");
                var branch = AppVeyor.Environment.Repository.Branch;
                var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;

                if (branch == "master" && !isPullRequest)
                {
                    packageVersion = version + "-dev-" + buildNumber + dbgSuffix;
                }
                else
                {
                    var suffix = "-ci-" + buildNumber + dbgSuffix;

                    if (isPullRequest)
                        suffix += "-pr-" + AppVeyor.Environment.PullRequest.Number;
                    else if (AppVeyor.Environment.Repository.Branch.StartsWith("release", StringComparison.OrdinalIgnoreCase))
                        suffix += "-pre-" + buildNumber;
                    else
                        suffix += "-" + branch.Replace("_", "-");

                    // Nuget limits "special version part" to 20 chars. Add one for the hyphen.
                    if (suffix.Length > 21)
                        suffix = suffix.Substring(0, 21);

                    packageVersion = version + suffix;
                }
            }

            AppVeyor.UpdateBuildVersion(packageVersion);
        }
    });

//////////////////////////////////////////////////////////////////////
// BUILD FRAMEWORKS
//////////////////////////////////////////////////////////////////////

Task("Build45")
    .Description("Builds the .NET 4.5 version of the framework")
    .Does(() =>
    {
        BuildProject("commercetools.NET/commercetools.NET.csproj", configuration);
        BuildProject("commercetools.NET.Tests/commercetools.NET.Tests.csproj", configuration);
    });

//////////////////////////////////////////////////////////////////////
// TEST
//////////////////////////////////////////////////////////////////////

Task("CheckForError")
    .Description("Checks for errors running the test suites")
    .Does(() => CheckForError(ref ErrorDetail));

//////////////////////////////////////////////////////////////////////
// TEST FRAMEWORK
//////////////////////////////////////////////////////////////////////

Task("Test45")
    .Description("Tests the .NET 4.5 version of the framework")
    .IsDependentOn("Build45")
    .OnError(exception => { ErrorDetail.Add(exception.Message); })
    .Does(() =>
    {
        var runtime = "net-4.5";
        var dir = BIN_DIR + runtime + "/";
        RunNUnitTests(dir, SDK_TESTS, runtime, ref ErrorDetail);
		if (isAppveyor)
		{
			var wc = new System.Net.WebClient();
			var jobId = AppVeyor.Environment.JobId;
			wc.UploadFile("https://ci.appveyor.com/api/testresults/nunit3/" + jobId, "TestResult.xml");
		}
	
    });


//////////////////////////////////////////////////////////////////////
// PACKAGE
//////////////////////////////////////////////////////////////////////

var RootFiles = new FilePath[]
{
    "LICENSE",
    "README.md"
};

// Not all of these are present in every framework
// The Microsoft and System assemblies are part of the BCL
// used by the .NET 4.0 framework. 4.0 tests will not run without them.
// NUnit.System.Linq is only present for the .NET 2.0 build.
var FrameworkFiles = new FilePath[]
{
    "commercetools.NET.dll",
    "commercetools.NET.xml"
};

Task("CreateImage")
    .Description("Copies all files into the image directory")
    .Does(() =>
    {
        var currentImageDir = IMAGE_DIR + "commercetools.NET-" + packageVersion + "/";
        var imageBinDir = currentImageDir + "bin/";

        CleanDirectory(currentImageDir);

        CopyFiles(RootFiles, currentImageDir);

        CreateDirectory(imageBinDir);
        Information("Created directory " + imageBinDir);

        foreach (var runtime in AllFrameworks)
        {
            var targetDir = imageBinDir + Directory(runtime);
            var sourceDir = BIN_DIR + Directory(runtime);
            CreateDirectory(targetDir);
            foreach (FilePath file in FrameworkFiles)
            {
                var sourcePath = sourceDir + "/" + file;
                if (FileExists(sourcePath))
                    CopyFileToDirectory(sourcePath, targetDir);
            }
        }
    });

Task("PackageSDK")
    .Description("Creates NuGet packages of the framework")
    .IsDependentOn("CreateImage")
    .Does(() =>
    {
        var currentImageDir = IMAGE_DIR + "commercetools.NET-" + packageVersion + "/";

        CreateDirectory(PACKAGE_DIR);

        NuGetPack("commercetools.NET.nuspec", new NuGetPackSettings()
        {
            Version = packageVersion,
            BasePath = currentImageDir,
            OutputDirectory = PACKAGE_DIR
        });
    });

Task("PackageZip")
    .Description("Creates a ZIP file of the framework")
    .IsDependentOn("CreateImage")
    .Does(() =>
    {
        CreateDirectory(PACKAGE_DIR);

        var currentImageDir = IMAGE_DIR + "commercetools.NET-" + packageVersion + "/";
		var zipPackage = PACKAGE_DIR + "commercetools.NET-" + packageVersion + ".zip";
        var zipFiles =
            GetFiles(currentImageDir + "*.*") +
            GetFiles(currentImageDir + "bin/net-4.5/*.*");
        Zip(currentImageDir, File(zipPackage), zipFiles);
    });

//////////////////////////////////////////////////////////////////////
// UPLOAD ARTIFACTS
//////////////////////////////////////////////////////////////////////

Task("UploadArtifacts")
    .Description("Uploads artifacts to AppVeyor")
    .IsDependentOn("Package")
    .Does(() =>
    {
        UploadArtifacts(PACKAGE_DIR, "*.nupkg");
        UploadArtifacts(PACKAGE_DIR, "*.zip");
    });

//////////////////////////////////////////////////////////////////////
// SETUP AND TEARDOWN TASKS
//////////////////////////////////////////////////////////////////////

Teardown(context => CheckForError(ref ErrorDetail));

//////////////////////////////////////////////////////////////////////
// HELPER METHODS - GENERAL
//////////////////////////////////////////////////////////////////////

bool CheckIfDotNetCoreInstalled()
{
    try
    {
        Information("Checking if .NET Core SDK is installed");
        StartProcess("dotnet", new ProcessSettings
        {
            Arguments = "--version"
        });
    }
    catch(Exception)
    {
        Warning(".NET Core SDK is not installed. It can be installed from https://www.microsoft.com/net/core");
        return false;
    }
    return true;
}

void RunGitCommand(string arguments)
{
    StartProcess("git", new ProcessSettings()
    {
        Arguments = arguments
    });
}

void UploadArtifacts(string packageDir, string searchPattern)
{
    foreach(var zip in System.IO.Directory.GetFiles(packageDir, searchPattern))
        AppVeyor.UploadArtifact(zip);
}

void CheckForError(ref List<string> errorDetail)
{
    if(errorDetail.Count != 0)
    {
        var copyError = new List<string>();
        copyError = errorDetail.Select(s => s).ToList();
        errorDetail.Clear();
        throw new Exception("One or more unit tests failed, breaking the build.\n"
                              + copyError.Aggregate((x,y) => x + "\n" + y));
    }
}

//////////////////////////////////////////////////////////////////////
// HELPER METHODS - BUILD
//////////////////////////////////////////////////////////////////////

void BuildProject(string projectPath, string configuration)
{
    DotNetBuild(projectPath, settings =>
        settings.SetConfiguration(configuration)
        .SetVerbosity(Verbosity.Minimal)
        .WithTarget("Build")
        .WithProperty("NodeReuse", "false")
		.WithProperty("Platform", "AnyCPU"));
}

//////////////////////////////////////////////////////////////////////
// HELPER METHODS - TEST
//////////////////////////////////////////////////////////////////////

void RunNUnitTests(DirectoryPath workingDir, string testAssembly, string framework, ref List<string> errorDetail)
{
    try
    {
        var path = workingDir.CombineWithFilePath(new FilePath(testAssembly));
		Information("Test directory " + path);
        var settings = new NUnit3Settings();
        if(!IsRunningOnWindows())
            settings.Process = NUnit3ProcessOption.InProcess;
        NUnit3(path.ToString(), settings);
    }
    catch(CakeException ce)
    {
        errorDetail.Add(string.Format("{0}: {1}", framework, ce.Message));
    }
}

void RunTest(FilePath exePath, DirectoryPath workingDir, string framework, ref List<string> errorDetail)
{
    RunTest(exePath, workingDir, null, framework, ref errorDetail);
}

void RunTest(FilePath exePath, DirectoryPath workingDir, string arguments, string framework, ref List<string> errorDetail)
{
    int rc = StartProcess(
        MakeAbsolute(exePath),
        new ProcessSettings()
        {
            Arguments = arguments,
            WorkingDirectory = workingDir
        });

    if (rc > 0)
        errorDetail.Add(string.Format("{0}: {1} tests failed", framework, rc));
    else if (rc < 0)
        errorDetail.Add(string.Format("{0} returned rc = {1}", exePath, rc));
}

void RunDotnetCoreTests(FilePath exePath, DirectoryPath workingDir, string framework, ref List<string> errorDetail)
{
    RunDotnetCoreTests(exePath, workingDir, null, framework, ref errorDetail);
}

void RunDotnetCoreTests(FilePath exePath, DirectoryPath workingDir, string arguments, string framework, ref List<string> errorDetail)
{
    int rc = StartProcess(
        "dotnet",
        new ProcessSettings()
        {
            Arguments = exePath + " " + arguments,
            WorkingDirectory = workingDir
        });

    if (rc > 0)
        errorDetail.Add(string.Format("{0}: {1} tests failed", framework, rc));
    else if (rc < 0)
        errorDetail.Add(string.Format("{0} returned rc = {1}", exePath, rc));
}

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Rebuild")
    .Description("Rebuilds all versions of the framework")
    .IsDependentOn("Clean")
    .IsDependentOn("Build");

Task("Build")
    .Description("Builds all versions of the framework")
    .IsDependentOn("InitializeBuild")
    .IsDependentOn("Build45");

Task("Test")
    .Description("Builds and tests all versions of the framework")
    .IsDependentOn("Build")
    .IsDependentOn("Test45");

Task("Package")
    .Description("Packages all versions of the framework")
    .IsDependentOn("CheckForError")
    .IsDependentOn("PackageSDK")
    .IsDependentOn("PackageZip");

Task("Appveyor")
    .Description("Builds, tests and packages on AppVeyor")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package")
    .IsDependentOn("UploadArtifacts");

Task("Travis")
    .Description("Builds and tests on Travis")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("Default")
    .Description("Builds all versions of the framework")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);