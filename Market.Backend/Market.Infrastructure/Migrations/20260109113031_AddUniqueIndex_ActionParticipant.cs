using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Market.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndex_ActionParticipant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActionParticipants_UserId",
                table: "ActionParticipants");

            migrationBuilder.CreateIndex(
                name: "IX_ActionParticipants_UserId_ActionId",
                table: "ActionParticipants",
                columns: new[] { "UserId", "ActionId" },
                unique: true,
                filter: "[UserId] IS NOT NULL AND [ActionId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActionParticipants_UserId_ActionId",
                table: "ActionParticipants");

            migrationBuilder.CreateIndex(
                name: "IX_ActionParticipants_UserId",
                table: "ActionParticipants",
                column: "UserId");
        }
    }
}
