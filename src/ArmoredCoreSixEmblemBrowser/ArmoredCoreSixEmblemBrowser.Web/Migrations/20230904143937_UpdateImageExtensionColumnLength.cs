using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArmoredCoreSixEmblemBrowser.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageExtensionColumnLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "image_extension",
                schema: "ac6",
                table: "emblem",
                type: "character varying(24)",
                maxLength: 24,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "image_extension",
                schema: "ac6",
                table: "emblem",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(24)",
                oldMaxLength: 24);
        }
    }
}
