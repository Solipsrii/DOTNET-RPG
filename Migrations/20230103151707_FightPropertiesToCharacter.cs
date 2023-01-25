using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DOTNETRPG.Migrations
{
    /// <inheritdoc />
    public partial class FightPropertiesToCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "skills",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "skills",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "skills",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "skills",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.AddColumn<int>(
                name: "defeats",
                table: "characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "fights",
                table: "characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "victories",
                table: "characters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "defeats",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "fights",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "victories",
                table: "characters");

            migrationBuilder.InsertData(
                table: "skills",
                columns: new[] { "id", "damage", "name" },
                values: new object[,]
                {
                    { 1, 15, "fireball" },
                    { 2, 7, "ice arrow" },
                    { 3, 20, "lightning bolt" },
                    { 4, 12, "earth lance" }
                });
        }
    }
}
