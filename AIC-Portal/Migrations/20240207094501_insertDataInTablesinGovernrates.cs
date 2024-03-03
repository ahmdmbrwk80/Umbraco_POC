using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AICPortal.Migrations
{
    /// <inheritdoc />
    public partial class insertDataInTablesinGovernrates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.InsertData(
			table: "Governorates",
			columns: new[] { "province_id", "GovernorateNameAr", "GovernorateNameEn", "CountriesId" },
			values: new object[] { "1", "القاهرة", "Cairo", "1" });
			migrationBuilder.InsertData(
		table: "Governorates",
		columns: new[] { "province_id", "GovernorateNameAr", "GovernorateNameEn", "CountriesId" },
		values: new object[] { "1", "أسيوط", "Assuit", "1" });
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DeleteData(
table: "Governorates",
keyColumn: "province_id",
keyValue: "1"); // Assuming "province_id" is the primary key column for "Governorates"

			migrationBuilder.DeleteData(
				table: "Governorates",
				keyColumn: "province_id",
				keyValue: "2"); // Assuming "province_id" is the primary key column for "Governorates"

		}
	}
}
