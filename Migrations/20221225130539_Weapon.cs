using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DOTNETRPG.Migrations
{
    /// <inheritdoc />
    public partial class Weapon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Users_userid",
                table: "Characters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Characters",
                table: "Characters");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Characters",
                newName: "characters");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_userid",
                table: "characters",
                newName: "IX_characters_userid");

            migrationBuilder.AddColumn<int>(
                name: "weaponid",
                table: "characters",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_characters",
                table: "characters",
                column: "id");

            migrationBuilder.CreateTable(
                name: "weapons",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    damage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weapons", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_characters_weaponid",
                table: "characters",
                column: "weaponid");

            migrationBuilder.AddForeignKey(
                name: "FK_characters_users_userid",
                table: "characters",
                column: "userid",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_characters_weapons_weaponid",
                table: "characters",
                column: "weaponid",
                principalTable: "weapons",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_characters_users_userid",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "FK_characters_weapons_weaponid",
                table: "characters");

            migrationBuilder.DropTable(
                name: "weapons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_characters",
                table: "characters");

            migrationBuilder.DropIndex(
                name: "IX_characters_weaponid",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "weaponid",
                table: "characters");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "characters",
                newName: "Characters");

            migrationBuilder.RenameIndex(
                name: "IX_characters_userid",
                table: "Characters",
                newName: "IX_Characters_userid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Characters",
                table: "Characters",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Users_userid",
                table: "Characters",
                column: "userid",
                principalTable: "Users",
                principalColumn: "id");
        }
    }
}
