namespace Nameless.BlazorServerDocker.Web.Components.Pages;

public partial class Home {
    private HomeModel Model { get; set; } = new(new EnvironmentInfo());
    
    private static string GetInBestUnit(long size) {
        const double mebibyte = 1024 * 1024;
        const double gibibyte = mebibyte * 1024;

        if (size < mebibyte) {
            return $"{size} bytes";
        }

        if (size < gibibyte) {
            var mebibytes = size / mebibyte;
            return $"{mebibytes:N2} MiB";
        }

        var gibibytes = size / gibibyte;
        return $"{gibibytes:N2} GiB";
    }

    private record HomeModel {
        public HomeModel(EnvironmentInfo env) {
            EnvironmentName = env.EnvironmentName;
            DotNetRuntimeVersion = env.DotNetRuntimeVersion;
            OperatingSystem = env.OperatingSystem;
            ProcessorArchitecture = env.ProcessorArchitecture;
            ProcessorCount = env.ProcessorCount;
            IsContainerized = env.IsContainerized;
            CurrentUser = env.CurrentUser;
            MemoryLimitInBytes = env.MemoryLimitInBytes;
            MemoryUsageInBytes = env.MemoryUsageInBytes;
            TotalAvailableMemoryInBytes = env.TotalAvailableMemoryInBytes;
            Hostname = env.HostName;
            IpList = env.IpList.ToArray();
        }

        public string EnvironmentName { get; init; } = string.Empty;
        public string DotNetRuntimeVersion { get; init; } = string.Empty;
        public string OperatingSystem { get; init; } = string.Empty;
        public string ProcessorArchitecture { get; init; } = string.Empty;
        public int ProcessorCount { get; init; }
        public bool IsContainerized { get; init; }
        public string CurrentUser { get; init; } = string.Empty;
        public long MemoryLimitInBytes { get; init; }
        public long MemoryUsageInBytes { get; init; }
        public long TotalAvailableMemoryInBytes { get; init; }
        public string Hostname { get; init; } = string.Empty;
        public string[] IpList { get; init; } = [];
    }
}