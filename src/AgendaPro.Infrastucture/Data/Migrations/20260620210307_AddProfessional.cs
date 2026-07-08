using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendaPro.Infrastucture.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProfessional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdetadAt",
                table: "Tags",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdetadAt",
                table: "Services",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdetadAt",
                table: "Clients",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdetadAt",
                table: "Categories",
                newName: "UpdatedAt");

            migrationBuilder.CreateTable(
                name: "Professionals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Specialty = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professionals", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_Email",
                table: "Professionals",
                column: "Email",
                unique: true,
                filter: "\"Email\" IS NOT NULL AND \"IsActive\" = true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Professionals");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Tags",
                newName: "UpdetadAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Services",
                newName: "UpdetadAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Clients",
                newName: "UpdetadAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Categories",
                newName: "UpdetadAt");
        }
    }
}
