using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AICPortal.Migrations
{
    /// <inheritdoc />
    public partial class insertDataInTablesinTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.InsertData(
				table: "Types",
				columns: new[] { "Type", "TypeAR" },
				values: new object[] { "Careers", "وظائف" });
			migrationBuilder.InsertData(
	table: "Types",
	columns: new[] { "Type", "TypeAR" },
	values: new object[] { "complains", "شكاوى" });
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DeleteData(
	   table: "Types",
	   keyColumn: "Id", // Assuming "Id" is the primary key column
	   keyValue: 1); // Replace 1 with the actual primary key value of the row you want to delete

			migrationBuilder.DeleteData(
				table: "Types",
				keyColumn: "Id", // Assuming "Id" is the primary key column
				keyValue: 2);
		}
    }
}
