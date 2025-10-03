using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterAlleviationFoundations.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLinksToModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "VolunteerTasks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "IncidentReports",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Donations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerTasks_UserId",
                table: "VolunteerTasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentReports_UserId",
                table: "IncidentReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_UserId",
                table: "Donations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_AspNetUsers_UserId",
                table: "Donations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentReports_AspNetUsers_UserId",
                table: "IncidentReports",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerTasks_AspNetUsers_UserId",
                table: "VolunteerTasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_AspNetUsers_UserId",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidentReports_AspNetUsers_UserId",
                table: "IncidentReports");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerTasks_AspNetUsers_UserId",
                table: "VolunteerTasks");

            migrationBuilder.DropIndex(
                name: "IX_VolunteerTasks_UserId",
                table: "VolunteerTasks");

            migrationBuilder.DropIndex(
                name: "IX_IncidentReports_UserId",
                table: "IncidentReports");

            migrationBuilder.DropIndex(
                name: "IX_Donations_UserId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "VolunteerTasks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "IncidentReports");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");
        }
    }
}
