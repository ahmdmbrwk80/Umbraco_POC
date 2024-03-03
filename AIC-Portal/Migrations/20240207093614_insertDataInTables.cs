using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AICPortal.Migrations
{
    /// <inheritdoc />
    public partial class insertDataInTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.InsertData(
				table: "Countries",
				columns: new[] { "CountryName", "CountryNameAR" },
				values: new object[] { "Egypt", "مصر" });

			migrationBuilder.InsertData(
				table: "Countries",
				columns: new[] { "CountryName", "CountryNameAR" },
				values: new object[] { "sudan", "سودان" });


		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "Countries",
				keyColumn: "Id", // Assuming "Id" is the primary key column
				keyValue: 1); // Replace 1 with the actual primary key value of the row you want to delete

			migrationBuilder.DeleteData(
				table: "Countries",
				keyColumn: "Id", // Assuming "Id" is the primary key column
				keyValue: 2); // Replace 2 with the actual primary key value of the row you want to delete

			// Add more DeleteData calls for additional rows if needed
		}

	}
}
