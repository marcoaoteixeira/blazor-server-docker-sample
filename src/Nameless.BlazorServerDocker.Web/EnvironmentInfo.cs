using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;

namespace Nameless.BlazorServerDocker.Web;

public readonly struct EnvironmentInfo {
    public EnvironmentInfo() {
        FetchMemoryStatistics(out var totalAvailableMemoryInBytes,
                              out var memoryLimitInBytes,
                              out var memoryUsageInBytes);

        TotalAvailableMemoryInBytes = totalAvailableMemoryInBytes;
        MemoryLimitInBytes = memoryLimitInBytes;
        MemoryUsageInBytes = memoryUsageInBytes;
    }

    public string EnvironmentName => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
    public string DotNetRuntimeVersion => RuntimeInformation.FrameworkDescription;
    public string OperatingSystem => RuntimeInformation.OSDescription;
    public string ProcessorArchitecture => RuntimeInformation.OSArchitecture.ToString();
    public int ProcessorCount => Environment.ProcessorCount;
    public bool IsContainerized => GetIsContainerized();
    public string CurrentUser => Environment.UserName;
    public long MemoryLimitInBytes { get; }
    public long MemoryUsageInBytes { get; }
    public long TotalAvailableMemoryInBytes { get; }
    public string HostName => Dns.GetHostName();
    public IEnumerable<string> IpList { get; } = Dns.GetHostAddresses(Dns.GetHostName())
                                                    .Select(ip => ip.ToString());

    private static bool GetIsContainerized() {
        var value = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");

        return bool.TryParse(value, out var isContainerized) && isContainerized;
    }

    private static void FetchMemoryStatistics(
        out long totalAvailableMemoryInBytes,
        out long memoryLimitInBytes,
        out long memoryUsageInBytes) {
        totalAvailableMemoryInBytes = 0L;
        memoryLimitInBytes = 0L;
        memoryUsageInBytes = 0L;

        var garbageCollectorMemoryInfo = GC.GetGCMemoryInfo();
        totalAvailableMemoryInBytes = garbageCollectorMemoryInfo.TotalAvailableMemoryBytes;

        if (System.OperatingSystem.IsWindows()) {
            FetchMemoryStatisticsOnWindows(out memoryLimitInBytes, out memoryUsageInBytes);
        }

        if (System.OperatingSystem.IsLinux()) {
            FetchMemoryStatisticsOnLinux(out memoryLimitInBytes, out memoryUsageInBytes);
        }
    }

    private static void FetchMemoryStatisticsOnWindows(out long memoryLimitInBytes, out long memoryUsageInBytes) {
        using var process = Process.GetCurrentProcess();
        
        memoryLimitInBytes = process.WorkingSet64;
        memoryUsageInBytes = process.PrivateMemorySize64;
    }

    private static void FetchMemoryStatisticsOnLinux(out long memoryLimitInBytes, out long memoryUsageInBytes) {
        var memoryLimitPaths = new[] {
            "/sys/fs/cgroup/memory.max",
            "/sys/fs/cgroup/memory.high",
            "/sys/fs/cgroup/memory.low",
            "/sys/fs/cgroup/memory/memory.limit_in_bytes",
        };

        var currentMemoryPaths = new[] {
            "/sys/fs/cgroup/memory.current",
            "/sys/fs/cgroup/memory/memory.usage_in_bytes",
        };

        memoryLimitInBytes = GetBestValue(memoryLimitPaths);
        memoryUsageInBytes = GetBestValue(currentMemoryPaths);

        static long GetBestValue(string[] paths) {
            foreach (var path in paths) {
                if (Path.Exists(path) && long.TryParse(File.ReadAllText(path), out var result)) {
                    return result;
                }
            }

            return 0;
        }
    }
}