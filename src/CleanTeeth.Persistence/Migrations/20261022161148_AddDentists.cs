using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanTeeth.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDentists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "DentalOffices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Dentists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dentists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DentalOfficeDentist",
                columns: table => new
                {
                    DentalOfficesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DentistsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentalOfficeDentist", x => new { x.DentalOfficesId, x.DentistsId });
                    table.ForeignKey(
                        name: "FK_DentalOfficeDentist_DentalOffices_DentalOfficesId",
                        column: x => x.DentalOfficesId,
                        principalTable: "DentalOffices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DentalOfficeDentist_Dentists_DentistsId",
                        column: x => x.DentistsId,
                        principalTable: "Dentists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DentalOfficeDentist_DentistsId",
                table: "DentalOfficeDentist",
                column: "DentistsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DentalOfficeDentist");

            migrationBuilder.DropTable(
                name: "Dentists");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "DentalOffices");
        }
    }
}
