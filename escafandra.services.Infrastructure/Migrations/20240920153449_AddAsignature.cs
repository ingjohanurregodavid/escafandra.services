using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace escafandra.services.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAsignature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "PDFDocuments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Signature",
                table: "PDFDocuments");
        }
    }
}
