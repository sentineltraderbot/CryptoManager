using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoManager.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddingReferralToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferredById",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferredById",
                table: "AspNetUsers");
        }
    }
}
