using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    static readonly AbsolutePath SourceDirectory = RootDirectory / "src";
    static readonly AbsolutePath ArtifactsDirectory = RootDirectory / "artifacts";
    static readonly AbsolutePath CoverageDirectory = RootDirectory / "coverage";

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter] readonly bool APPVEYOR_REPO_TAG;
    [Parameter] readonly string APPVEYOR_REPO_BRANCH;
    [Parameter] readonly string MYGET_API_KEY;
    [Parameter] readonly string NUGET_API_KEY;

    [Solution] readonly Solution Solution;

    [GitVersion(Framework = "netcoreapp3.1", NoFetch = true)]
    readonly GitVersion GitVersion;

    public static int Main() => Execute<Build>(x => x.Pack);

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            foreach (var project in Solution.GetProjects("*.Tests"))
            {
                var settings = new DotNetTestSettings()
                    .SetProjectFile(project)
                    .SetConfiguration(Configuration)
                    .EnableNoRestore()
                    .EnableNoBuild();

                var outDir = project.GetMSBuildProject(Configuration).GetPropertyValue("OutputPath");
                var assembly = project.Directory / outDir / $"{project.Name}.dll";

                EnsureCleanDirectory(CoverageDirectory);

                CoverletTasks.Coverlet(s => s
                    .SetTargetSettings(settings)
                    .SetAssembly(assembly)
                    .SetOutput(CoverageDirectory / "coverage")
                    .SetFormat(CoverletOutputFormat.opencover));

                ReportGeneratorTasks.ReportGenerator(s => s
                    .SetFramework("netcoreapp3.0")
                    .SetTargetDirectory(CoverageDirectory)
                    .SetReports(CoverageDirectory / "coverage.opencover.xml"));
            }
        });

    Target Pack => _ => _
        .DependsOn(Test)
        .Produces(ArtifactsDirectory / "*.nupkg")
        .Executes(() =>
        {
            EnsureCleanDirectory(ArtifactsDirectory);

            DotNetPack(s => s
                .SetProject(Solution)
                .SetConfiguration(Configuration)
                .SetOutputDirectory(ArtifactsDirectory)
                .SetVersion(GitVersion.NuGetVersionV2)
                .EnableIncludeSource()
                .EnableIncludeSymbols()
                .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                .EnableNoRestore()
                .EnableNoBuild());
        });

    Target Push => _ => _
        .DependsOn(Pack)
        .Executes(() =>
        {
            if (APPVEYOR_REPO_TAG)
            {
                ArtifactsDirectory.GlobFiles("*.nupkg")
                    .ForEach(x =>
                    {
                        DotNetNuGetPush(s => s
                            .SetTargetPath(x)
                            .SetSource("https://api.nuget.org/v3/index.json")
                            .SetApiKey(NUGET_API_KEY));
                    });
            }
            else if (APPVEYOR_REPO_BRANCH == "main")
            {
                ArtifactsDirectory.GlobFiles("*.nupkg")
                    .ForEach(x =>
                    {
                        DotNetNuGetPush(s => s
                            .SetTargetPath(x)
                            .SetSource("https://www.myget.org/F/baunegaard/api/v2/package")
                            .SetApiKey(MYGET_API_KEY));
                    });

                ArtifactsDirectory.GlobFiles("*.snupkg")
                    .ForEach(x =>
                    {
                        DotNetNuGetPush(s => s
                            .SetTargetPath(x)
                            .SetSource("https://www.myget.org/F/baunegaard/api/v3/index.json")
                            .SetApiKey(MYGET_API_KEY));
                    });
            }
        });
}
