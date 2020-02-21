using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IISAdministration.Migrations
{
    public partial class Metrics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    MetricsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ServerId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    Date = table.Column<int>(nullable: false),
                    Time = table.Column<int>(nullable: false),
                    DayOfWeek = table.Column<int>(nullable: false),
                    BytesSentPerSec = table.Column<int>(nullable: false),
                    BytesRecvPerSec = table.Column<int>(nullable: false),
                    CurrentConnections = table.Column<int>(nullable: false),
                    RequestsPerSecond = table.Column<int>(nullable: false),
                    SystemMemoryInUse = table.Column<long>(nullable: false),
                    Threads = table.Column<int>(nullable: false),
                    Processes = table.Column<int>(nullable: false),
                    CpuPercentUsage = table.Column<int>(nullable: false),
                    SystemCpuPercentUsage = table.Column<int>(nullable: false),
                    IoWriteOpsPerSec = table.Column<int>(nullable: false),
                    IoReadOpsPerSec = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.MetricsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Metrics");
        }
    }
}
