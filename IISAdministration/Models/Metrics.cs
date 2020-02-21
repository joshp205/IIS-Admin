using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IISAdministration.Models {

    public class Metrics {
        
        [Key]
        [JsonProperty("id")]
        public int MetricsId { get; set; }

        [JsonProperty("server_id")]
        [DisplayName("Server ID")]
        public int ServerId { get; set; }

        [JsonProperty("timestamp")]
        [DisplayName("Timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("date")]
        [DisplayName("Date")]
        public int Date { get; set; }

        [JsonProperty("time")]
        [DisplayName("Time")]
        public int Time { get; set; }

        [JsonProperty("day_of_week")]
        [DisplayName("Day of Week")]
        public int DayOfWeek { get; set; }

        [JsonProperty("bytes_sent_sec")]
        [DisplayName("Bytes Sent Per Second")]
        public int BytesSentPerSec { get; set; }

        [JsonProperty("bytes_recv_sec")]
        [DisplayName("Bytes Received Per Second")]
        public int BytesRecvPerSec { get; set; }

        [JsonProperty("current_connections")]
        [DisplayName("Current Connections")]
        public int CurrentConnections { get; set; }

        [JsonProperty("requests_per_sec")]
        [DisplayName("Requests Per Second")]
        public int RequestsPerSecond { get; set; }

        [JsonProperty("system_memory_in_use")]
        [DisplayName("System Memory In Use")]
        public long SystemMemoryInUse { get; set; }

        [JsonProperty("threads")]
        [DisplayName("Threads")]
        public int Threads { get; set; }

        [JsonProperty("processes")]
        [DisplayName("Processes")]
        public int Processes { get; set; }

        [JsonProperty("cpu_percent_usage")]
        [DisplayName("CPU Percent Usage")]
        public int CpuPercentUsage { get; set; }

        [JsonProperty("system_cpu_percent_usage")]
        [DisplayName("System CPU Percent Usage")]
        public int SystemCpuPercentUsage { get; set; }

        [JsonProperty("io_write_operations_sec")]
        [DisplayName("IO Write Operations Per Second")]
        public int IoWriteOpsPerSec { get; set; }

        [JsonProperty("io_read_operations_sec")]
        [DisplayName("IO Read Operations Per Second")]
        public int IoReadOpsPerSec { get; set; }
        
        public void Add(ref Metrics that) {
            this.BytesSentPerSec += that.BytesSentPerSec;
            this.BytesRecvPerSec += that.BytesRecvPerSec;
            this.CurrentConnections += that.CurrentConnections;
            this.RequestsPerSecond += that.RequestsPerSecond;
            this.SystemMemoryInUse += that.SystemMemoryInUse;
            this.Threads += that.Threads;
            this.Processes += that.Processes;
            this.CpuPercentUsage = (this.CpuPercentUsage + that.CpuPercentUsage) / 2;
            this.SystemCpuPercentUsage = (this.SystemCpuPercentUsage + that.SystemCpuPercentUsage) / 2;
            this.IoWriteOpsPerSec += that.IoWriteOpsPerSec;
            this.IoReadOpsPerSec += that.IoReadOpsPerSec;
        }
    }
}
