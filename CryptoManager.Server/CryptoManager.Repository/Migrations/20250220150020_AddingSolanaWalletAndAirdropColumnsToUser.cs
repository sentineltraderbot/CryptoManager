using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoManager.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddingSolanaWalletAndAirdropColumnsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasReceivedAirdrop",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SolanaWalletAddress",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasReceivedAirdrop",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SolanaWalletAddress",
                table: "AspNetUsers");
        }
    }
}
