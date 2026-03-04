using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendaPro.Infrastucture.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateServiceValidations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IntervaloMin",
                table: "Services",
                newName: "TempoIntervaloMin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TempoIntervaloMin",
                table: "Services",
                newName: "IntervaloMin");
        }
    }
}
