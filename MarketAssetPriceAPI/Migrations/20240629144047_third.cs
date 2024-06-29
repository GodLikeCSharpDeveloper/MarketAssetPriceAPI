using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketAssetPriceAPI.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstrumentProviders_Instruments_InstrumentEntityId",
                table: "InstrumentProviders");

            migrationBuilder.DropForeignKey(
                name: "FK_InstrumentProviders_Providers_ProviderEntityId",
                table: "InstrumentProviders");

            migrationBuilder.DropIndex(
                name: "IX_InstrumentProviders_InstrumentEntityId",
                table: "InstrumentProviders");

            migrationBuilder.DropIndex(
                name: "IX_InstrumentProviders_ProviderEntityId",
                table: "InstrumentProviders");

            migrationBuilder.DropColumn(
                name: "InstrumentEntityId",
                table: "InstrumentProviders");

            migrationBuilder.DropColumn(
                name: "ProviderEntityId",
                table: "InstrumentProviders");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentProviders_InstrumentId",
                table: "InstrumentProviders",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentProviders_ProviderId_InstrumentId",
                table: "InstrumentProviders",
                columns: new[] { "ProviderId", "InstrumentId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InstrumentProviders_Instruments_InstrumentId",
                table: "InstrumentProviders",
                column: "InstrumentId",
                principalTable: "Instruments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstrumentProviders_Providers_ProviderId",
                table: "InstrumentProviders",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstrumentProviders_Instruments_InstrumentId",
                table: "InstrumentProviders");

            migrationBuilder.DropForeignKey(
                name: "FK_InstrumentProviders_Providers_ProviderId",
                table: "InstrumentProviders");

            migrationBuilder.DropIndex(
                name: "IX_InstrumentProviders_InstrumentId",
                table: "InstrumentProviders");

            migrationBuilder.DropIndex(
                name: "IX_InstrumentProviders_ProviderId_InstrumentId",
                table: "InstrumentProviders");

            migrationBuilder.AddColumn<int>(
                name: "InstrumentEntityId",
                table: "InstrumentProviders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProviderEntityId",
                table: "InstrumentProviders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentProviders_InstrumentEntityId",
                table: "InstrumentProviders",
                column: "InstrumentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentProviders_ProviderEntityId",
                table: "InstrumentProviders",
                column: "ProviderEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstrumentProviders_Instruments_InstrumentEntityId",
                table: "InstrumentProviders",
                column: "InstrumentEntityId",
                principalTable: "Instruments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstrumentProviders_Providers_ProviderEntityId",
                table: "InstrumentProviders",
                column: "ProviderEntityId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
