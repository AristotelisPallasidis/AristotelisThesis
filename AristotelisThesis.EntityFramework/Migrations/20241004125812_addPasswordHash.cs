using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AristotelisThesis.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class addPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Students",
                newName: "PasswordHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Students",
                newName: "Password");
        }
    }
}
