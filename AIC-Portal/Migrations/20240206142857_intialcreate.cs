using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AICPortal.Migrations
{
    /// <inheritdoc />
    public partial class intialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryNameAR = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeAR = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    province_id = table.Column<byte>(type: "tinyint", nullable: false),
                    GovernorateNameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GovernorateNameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Governorates_Countries_CountriesId",
                        column: x => x.CountriesId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactForm",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    typeId = table.Column<int>(type: "int", nullable: false),
                    governoratesId = table.Column<int>(type: "int", nullable: false),
                    work = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactForm", x => x.id);
                    table.ForeignKey(
                        name: "FK_ContactForm_Governorates_governoratesId",
                        column: x => x.governoratesId,
                        principalTable: "Governorates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContactForm_Types_typeId",
                        column: x => x.typeId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactForm_governoratesId",
                table: "ContactForm",
                column: "governoratesId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactForm_typeId",
                table: "ContactForm",
                column: "typeId");

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_CountriesId",
                table: "Governorates",
                column: "CountriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactForm");

            migrationBuilder.DropTable(
                name: "Governorates");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
