using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Numerics;

namespace IISAdministration.Models.JsonModels {

    /// <summary>
    /// Represents a JSON object that encapsulates the response from calling an endpoint at: "/api/webserver/monitoring/".
    /// </summary>
    public class MonitoringJson {

        /// <summary>
        /// The website ID for the given collection of data.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Set of data for network related aspects of the website.
        /// </summary>
        [JsonProperty("network")]
        public MonitoringNetworkJson Network { get; set; }

        /// <summary>
        /// Set of data for requests made to the website.
        /// </summary>
        [JsonProperty("requests")]
        public MonitoringRequestsJson Requests { get; set; }

        /// <summary>
        /// Set of data for the system's memory.
        /// </summary>
        [JsonProperty("memory")]
        public MonitoringMemoryJson Memory { get; set; }

        /// <summary>
        /// Set of data for the system's CPU.
        /// </summary>
        [JsonProperty("cpu")]
        public MonitoringCpuJson Cpu { get; set; }

        /// <summary>
        /// Set of data for the system's disk.
        /// </summary>
        [JsonProperty("disk")]
        public MonitoringDiskJson Disk { get; set; }

        /// <summary>
        /// Set of data for the system's cache
        /// </summary>
        [JsonProperty("cache")]
        public MonitoringCacheJson Cache { get; set; }

        public void CopyTo(ref Metrics metrics) {
            metrics.BytesSentPerSec = Network.BytesSentPerSec;
            metrics.BytesRecvPerSec = Network.BytesRecvPerSec;
            metrics.CurrentConnections = Network.CurrentConnections;
            metrics.RequestsPerSecond = Requests.PerSecond;
            metrics.SystemMemoryInUse = Memory.SystemInUse;
            metrics.Threads = Cpu.Threads;
            metrics.Processes = Cpu.Processes;
            metrics.CpuPercentUsage = Cpu.PercentUsage;
            metrics.SystemCpuPercentUsage = Cpu.SystemPercentUsage;
            metrics.IoWriteOpsPerSec = Disk.IoWriteOpsPerSec;
            metrics.IoReadOpsPerSec = Disk.IoReadOpsPerSec;
        }

        public static MonitoringJson ParseJson(JObject json) {
            MonitoringJson monitor = new MonitoringJson();
            MonitoringNetworkJson net = new MonitoringNetworkJson();
            MonitoringRequestsJson req = new MonitoringRequestsJson();
            MonitoringMemoryJson mem = new MonitoringMemoryJson();
            MonitoringCpuJson cpu = new MonitoringCpuJson();
            MonitoringDiskJson dsk = new MonitoringDiskJson();
            MonitoringCacheJson cch = new MonitoringCacheJson();

            net.BytesSentPerSec = json["network"]["bytes_sent_sec"].ToObject<int>();
            net.BytesRecvPerSec = json["network"]["bytes_recv_sec"].ToObject<int>();
            net.ConnectAttemptsPerSec = json["network"]["connection_attempts_sec"].ToObject<int>();
            net.TotalBytesSent = json["network"]["total_bytes_sent"].ToObject<BigInteger>();
            net.TotalBytesRecv = json["network"]["total_bytes_recv"].ToObject<BigInteger>();
            net.TotalConnectAttempts = json["network"]["total_connection_attempts"].ToObject<BigInteger>();
            net.CurrentConnections = json["network"]["current_connections"].ToObject<int>();

            req.Active = json["requests"]["active"].ToObject<int>();
            req.PerSecond = json["requests"]["per_sec"].ToObject<int>();
            req.Total = json["requests"]["total"].ToObject<BigInteger>();

            mem.Handles = json["memory"]["handles"].ToObject<int>();
            mem.PrivateBytes = json["memory"]["private_bytes"].ToObject<BigInteger>();
            mem.PrivateWorkingSet = json["memory"]["private_working_set"].ToObject<BigInteger>();
            mem.SystemInUse = json["memory"]["system_in_use"].ToObject<long>();
            mem.Installed = json["memory"]["installed"].ToObject<long>();

            cpu.Threads = json["cpu"]["threads"].ToObject<int>();
            cpu.Processes = json["cpu"]["processes"].ToObject<int>();
            cpu.PercentUsage = json["cpu"]["percent_usage"].ToObject<int>();
            cpu.SystemPercentUsage = json["cpu"]["system_percent_usage"].ToObject<int>();

            dsk.IoWriteOpsPerSec = json["disk"]["io_write_operations_sec"].ToObject<int>();
            dsk.IoReadOpsPerSec = json["disk"]["io_read_operations_sec"].ToObject<int>();
            dsk.PageFaultsPerSec = json["disk"]["page_faults_sec"].ToObject<int>();

            cch.FileCacheCount = json["cache"]["file_cache_count"].ToObject<int>();
            cch.FileCacheMemoryUsage = json["cache"]["file_cache_memory_usage"].ToObject<int>();
            cch.FileCacheHits = json["cache"]["file_cache_hits"].ToObject<int>();
            cch.FileCacheMisses = json["cache"]["file_cache_misses"].ToObject<int>();
            cch.TotalFilesCached = json["cache"]["total_files_cached"].ToObject<int>();
            cch.OutputCacheCount = json["cache"]["output_cache_count"].ToObject<int>();
            cch.OutputCacheMemoryUsage = json["cache"]["output_cache_memory_usage"].ToObject<int>();
            cch.OutputCacheHits = json["cache"]["output_cache_hits"].ToObject<int>();
            cch.OutputCacheMisses = json["cache"]["output_cache_misses"].ToObject<int>();
            cch.UriCacheCount = json["cache"]["uri_cache_count"].ToObject<int>();
            cch.UriCacheHits = json["cache"]["uri_cache_hits"].ToObject<int>();
            cch.UriCacheMisses = json["cache"]["uri_cache_misses"].ToObject<int>();
            cch.TotalUrisCached = json["cache"]["total_uris_cached"].ToObject<int>();

            monitor.Id = json["id"].ToObject<string>();
            monitor.Network = net;
            monitor.Requests = req;
            monitor.Memory = mem;
            monitor.Cpu = cpu;
            monitor.Disk = dsk;
            monitor.Cache = cch;

            return monitor;
        }
    }
    
    public class MonitoringNetworkJson {
        [JsonProperty("bytes_sent_sec")]
        public int BytesSentPerSec { get; set; }

        [JsonProperty("bytes_recv_sec")]
        public int BytesRecvPerSec { get; set; }

        [JsonProperty("connection_attempts_sec")]
        public int ConnectAttemptsPerSec { get; set; }

        [JsonProperty("total_bytes_sent")]
        public BigInteger TotalBytesSent { get; set; }

        [JsonProperty("total_bytes_recv")]
        public BigInteger TotalBytesRecv { get; set; }

        [JsonProperty("total_connection_attempts")]
        public BigInteger TotalConnectAttempts { get; set; }

        [JsonProperty("current_connections")]
        public int CurrentConnections { get; set; }
    }

    public class MonitoringRequestsJson {
        [JsonProperty("active")]
        public int Active { get; set; }

        [JsonProperty("per_sec")]
        public int PerSecond { get; set; }

        [JsonProperty("total")]
        public BigInteger Total { get; set; }
    }

    public class MonitoringMemoryJson {
        [JsonProperty("handles")]
        public int Handles { get; set; }

        [JsonProperty("private_bytes")]
        public BigInteger PrivateBytes { get; set; }

        [JsonProperty("private_working_set")]
        public BigInteger PrivateWorkingSet { get; set; }

        /// <summary>
        /// The total amount of memory used by the system, given in bytes.
        /// </summary>
        [JsonProperty("system_in_use")]
        public long SystemInUse { get; set; }

        /// <summary>
        /// The amount of memory installed on the system, given in bytes.
        /// </summary>
        [JsonProperty("installed")]
        public long Installed { get; set; }
    }

    public class MonitoringCpuJson {
        [JsonProperty("threads")]
        public int Threads { get; set; }

        [JsonProperty("processes")]
        public int Processes { get; set; }

        [JsonProperty("percent_usage")]
        public int PercentUsage { get; set; }

        [JsonProperty("system_percent_usage")]
        public int SystemPercentUsage { get; set; }
    }

    public class MonitoringDiskJson {
        [JsonProperty("io_write_operations_sec")]
        public int IoWriteOpsPerSec { get; set; }

        [JsonProperty("io_read_operations_sec")]
        public int IoReadOpsPerSec { get; set; }

        [JsonProperty("page_faults_sec")]
        public int PageFaultsPerSec { get; set; }
    }

    public class MonitoringCacheJson {
        [JsonProperty("file_cache_count")]
        public int FileCacheCount { get; set; }

        [JsonProperty("file_cache_memory_usage")]
        public int FileCacheMemoryUsage { get; set; }

        [JsonProperty("file_cache_hits")]
        public int FileCacheHits { get; set; }

        [JsonProperty("file_cache_misses")]
        public int FileCacheMisses { get; set; }

        [JsonProperty("total_files_cached")]
        public int TotalFilesCached { get; set; }

        [JsonProperty("output_cache_count")]
        public int OutputCacheCount { get; set; }

        [JsonProperty("output_cache_memory_usage")]
        public int OutputCacheMemoryUsage { get; set; }

        [JsonProperty("output_cache_hits")]
        public int OutputCacheHits { get; set; }

        [JsonProperty("output_cache_misses")]
        public int OutputCacheMisses { get; set; }

        [JsonProperty("uri_cache_count")]
        public int UriCacheCount { get; set; }

        [JsonProperty("uri_cache_hits")]
        public int UriCacheHits { get; set; }

        [JsonProperty("uri_cache_misses")]
        public int UriCacheMisses { get; set; }

        [JsonProperty("total_uris_cached")]
        public int TotalUrisCached { get; set; }
    }
}
