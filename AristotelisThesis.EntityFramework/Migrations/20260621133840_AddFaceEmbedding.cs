using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AristotelisThesis.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddFaceEmbedding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Embedding",
                table: "FaceImages",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Embedding",
                table: "FaceImages");
        }
    }
}
