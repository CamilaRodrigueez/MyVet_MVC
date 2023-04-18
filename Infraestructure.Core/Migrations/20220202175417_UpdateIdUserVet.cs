using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructure.Core.Migrations
{
    public partial class UpdateIdUserVet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dates_User_IdUserVet",
                schema: "Vet",
                table: "Dates");

            migrationBuilder.AlterColumn<int>(
                name: "IdUserVet",
                schema: "Vet",
                table: "Dates",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Dates_User_IdUserVet",
                schema: "Vet",
                table: "Dates",
                column: "IdUserVet",
                principalSchema: "Security",
                principalTable: "User",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dates_User_IdUserVet",
                schema: "Vet",
                table: "Dates");

            migrationBuilder.AlterColumn<int>(
                name: "IdUserVet",
                schema: "Vet",
                table: "Dates",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Dates_User_IdUserVet",
                schema: "Vet",
                table: "Dates",
                column: "IdUserVet",
                principalSchema: "Security",
                principalTable: "User",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
