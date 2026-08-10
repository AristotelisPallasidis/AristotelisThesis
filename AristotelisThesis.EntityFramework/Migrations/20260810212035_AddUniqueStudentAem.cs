using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AristotelisThesis.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueStudentAem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Students_AEM",
                table: "Students",
                column: "AEM",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_AEM",
                table: "Students");
        }
    }
}
