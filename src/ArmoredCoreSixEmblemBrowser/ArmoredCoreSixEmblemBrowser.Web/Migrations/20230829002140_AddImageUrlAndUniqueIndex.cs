using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArmoredCoreSixEmblemBrowser.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlAndUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_url",
                schema: "ac6",
                table: "emblem",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_unique_share_id_platform",
                schema: "ac6",
                table: "emblem",
                column: "share_id",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "platform" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_unique_share_id_platform",
                schema: "ac6",
                table: "emblem");

            migrationBuilder.DropColumn(
                name: "image_url",
                schema: "ac6",
                table: "emblem");
        }
    }
}
