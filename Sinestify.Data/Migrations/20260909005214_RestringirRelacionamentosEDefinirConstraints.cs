using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sinestify.Migrations
{
    /// <inheritdoc />
    public partial class RestringirRelacionamentosEDefinirConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Musicas_Emocoes_EmocaoId",
                table: "Musicas");

            migrationBuilder.DropForeignKey(
                name: "FK_Musicas_Generos_GeneroId",
                table: "Musicas");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Musicas",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Cantor",
                table: "Musicas",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Generos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Sentimento",
                table: "Emocoes",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Generos_Nome",
                table: "Generos",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Emocoes_Sentimento",
                table: "Emocoes",
                column: "Sentimento",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Musicas_Emocoes_EmocaoId",
                table: "Musicas",
                column: "EmocaoId",
                principalTable: "Emocoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Musicas_Generos_GeneroId",
                table: "Musicas",
                column: "GeneroId",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Musicas_Emocoes_EmocaoId",
                table: "Musicas");

            migrationBuilder.DropForeignKey(
                name: "FK_Musicas_Generos_GeneroId",
                table: "Musicas");

            migrationBuilder.DropIndex(
                name: "IX_Generos_Nome",
                table: "Generos");

            migrationBuilder.DropIndex(
                name: "IX_Emocoes_Sentimento",
                table: "Emocoes");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Musicas",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Cantor",
                table: "Musicas",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Generos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Sentimento",
                table: "Emocoes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Musicas_Emocoes_EmocaoId",
                table: "Musicas",
                column: "EmocaoId",
                principalTable: "Emocoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Musicas_Generos_GeneroId",
                table: "Musicas",
                column: "GeneroId",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
