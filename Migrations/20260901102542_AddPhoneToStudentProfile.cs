using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternLink.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoneToStudentProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "StudentProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "StudentProfiles");
        }
    }
}
