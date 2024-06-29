using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketAssetPriceAPI.Migrations
{
    /// <inheritdoc />
    public partial class third23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exchanges_Providers_ProviderEntityId",
                table: "Exchanges");

            migrationBuilder.DropIndex(
                name: "IX_Exchanges_ProviderEntityId",
                table: "Exchanges");

            migrationBuilder.DropColumn(
                name: "ProviderEntityId",
                table: "Exchanges");

            migrationBuilder.CreateIndex(
                name: "IX_Exchanges_ProviderId",
                table: "Exchanges",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exchanges_Providers_ProviderId",
                table: "Exchanges",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exchanges_Providers_ProviderId",
                table: "Exchanges");

            migrationBuilder.DropIndex(
                name: "IX_Exchanges_ProviderId",
                table: "Exchanges");

            migrationBuilder.AddColumn<int>(
                name: "ProviderEntityId",
                table: "Exchanges",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exchanges_ProviderEntityId",
                table: "Exchanges",
                column: "ProviderEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exchanges_Providers_ProviderEntityId",
                table: "Exchanges",
                column: "ProviderEntityId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
