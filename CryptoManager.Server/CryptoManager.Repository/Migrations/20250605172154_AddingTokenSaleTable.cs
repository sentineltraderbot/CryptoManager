using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoManager.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddingTokenSaleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TokenSale",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    OrderType = table.Column<int>(type: "INTEGER", nullable: false),
                    UserEmail = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    UserWalletAddress = table.Column<string>(type: "TEXT", nullable: true),
                    BlockchainTx = table.Column<string>(type: "TEXT", nullable: true),
                    SOLQuantity = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    SENTBOTQuantity = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    ReferralQuantity = table.Column<decimal>(type: "decimal(18, 8)", nullable: false),
                    ReferralWalletAddress = table.Column<string>(type: "TEXT", nullable: true),
                    ReferralUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsExcluded = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    RegistryDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenSale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokenSale_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TokenSale_AspNetUsers_ReferralUserId",
                        column: x => x.ReferralUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TokenSale_ApplicationUserId",
                table: "TokenSale",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TokenSale_ReferralUserId",
                table: "TokenSale",
                column: "ReferralUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenSale");
        }
    }
}
