using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArmoredCoreSixEmblemBrowser.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddImageExtensionColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_extension",
                schema: "ac6",
                table: "emblem",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_extension",
                schema: "ac6",
                table: "emblem");
        }
    }
}
