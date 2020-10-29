using System;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AppVeyor;
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

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
[AppVeyor(AppVeyorImage.VisualStudio2019, AutoGenerate = false)]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
internal class Build : NukeBuild
{
    private static AbsolutePath _sourceDirectory = RootDirectory / "src";
    private static AbsolutePath _artifactsDirectory = RootDirectory / "artifacts";
    private static AbsolutePath _coverageDirectory = RootDirectory / "coverage";

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    private readonly Configuration _configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution]
    private readonly Solution _solution;

    [GitVersion(Framework = "netcoreapp3.1", NoFetch = true)]
    private readonly GitVersion _gitVersion;

    private Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            _sourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(_artifactsDirectory);
        });

    private Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(_solution));
        });

    private Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(_solution)
                .SetConfiguration(_configuration)
                .SetAssemblyVersion(_gitVersion.AssemblySemVer)
                .SetFileVersion(_gitVersion.AssemblySemFileVer)
                .SetInformationalVersion(_gitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    private Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            foreach (var project in _solution.GetProjects("*.Tests"))
            {
                var settings = new DotNetTestSettings()
                    .SetProjectFile(project)
                    .SetConfiguration(_configuration)
                    .EnableNoRestore()
                    .EnableNoBuild();

                var outDir = project.GetMSBuildProject(_configuration).GetPropertyValue("OutputPath");
                var assembly = project.Directory / outDir / $"{project.Name}.dll";

                EnsureCleanDirectory(_coverageDirectory);

                CoverletTasks.Coverlet(s => s
                    .SetTargetSettings(settings)
                    .SetAssembly(assembly)
                    .SetOutput(_coverageDirectory / "coverage")
                    .SetFormat(CoverletOutputFormat.opencover));

                ReportGeneratorTasks.ReportGenerator(s => s
                    .SetFramework("netcoreapp3.0")
                    .SetTargetDirectory(_coverageDirectory)
                    .SetReports(_coverageDirectory / "coverage.opencover.xml"));
            }
        });

    private Target Pack => _ => _
        .DependsOn(Test)
        .Produces(_artifactsDirectory / "*.nupkg")
        .Executes(() =>
        {
            EnsureCleanDirectory(_artifactsDirectory);

            DotNetPack(s => s
                .SetProject(_solution)
                .SetConfiguration(_configuration)
                .SetOutputDirectory(_artifactsDirectory)
                .SetVersion(_gitVersion.NuGetVersionV2)
                .EnableIncludeSource()
                .EnableIncludeSymbols()
                .EnableNoRestore()
                .EnableNoBuild());
        });

    public static int Main() => Execute<Build>(x => x.Pack);
}
