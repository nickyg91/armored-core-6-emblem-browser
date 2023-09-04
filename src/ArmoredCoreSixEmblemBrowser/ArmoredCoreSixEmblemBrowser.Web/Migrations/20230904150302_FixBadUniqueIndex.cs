using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArmoredCoreSixEmblemBrowser.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixBadUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_unique_share_id_platform",
                schema: "ac6",
                table: "emblem");

            migrationBuilder.CreateIndex(
                name: "ix_unique_share_id_platform",
                schema: "ac6",
                table: "emblem",
                columns: new[] { "share_id", "platform" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_unique_share_id_platform",
                schema: "ac6",
                table: "emblem");

            migrationBuilder.CreateIndex(
                name: "ix_unique_share_id_platform",
                schema: "ac6",
                table: "emblem",
                column: "share_id",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "platform" });
        }
    }
}
