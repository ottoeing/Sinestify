using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sinestify.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Emocoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Sentimento = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emocoes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Generos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Musicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cantor = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GeneroId = table.Column<int>(type: "int", nullable: false),
                    EmocaoId = table.Column<int>(type: "int", nullable: false),
                    Velocidade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musicas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Musicas_Emocoes_EmocaoId",
                        column: x => x.EmocaoId,
                        principalTable: "Emocoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Musicas_Generos_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "Generos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Emocoes",
                columns: new[] { "Id", "Sentimento" },
                values: new object[,]
                {
                    { 1, "Alegria" },
                    { 2, "Tristeza" },
                    { 3, "Euforia" },
                    { 4, "Saudade" },
                    { 5, "Calma" },
                    { 6, "Nostalgia" },
                    { 7, "Esperança" },
                    { 8, "Melancolia" },
                    { 9, "Empolgação" },
                    { 10, "Romantismo" }
                });

            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Pop" },
                    { 2, "Rock" },
                    { 3, "Eletrônica" },
                    { 4, "R&B" },
                    { 5, "Clássica" },
                    { 6, "Hip Hop" },
                    { 7, "Jazz" },
                    { 8, "Reggae" },
                    { 9, "Funk" },
                    { 10, "Country" },
                    { 11, "Indie" },
                    { 12, "Metal" }
                });

            migrationBuilder.InsertData(
                table: "Musicas",
                columns: new[] { "Id", "Cantor", "EmocaoId", "GeneroId", "Nome", "Velocidade" },
                values: new object[,]
                {
                    { 1, "Imagine Dragons", 3, 2, "Believer", 120 },
                    { 2, "Imagine Dragons", 3, 2, "Thunder", 118 },
                    { 3, "Ed Sheeran", 1, 1, "Shape Of You", 76 },
                    { 4, "OneRepublic", 5, 11, "Counting Stars", 94 },
                    { 5, "Ed Sheeran", 1, 1, "Perfect", 48 },
                    { 6, "The Weeknd", 4, 3, "Blinding Lights", 65 },
                    { 7, "Adele", 2, 5, "Someone Like You", 40 },
                    { 8, "Linkin Park", 2, 12, "Numb", 90 },
                    { 9, "Imagine Dragons", 4, 2, "Radioactive", 100 },
                    { 10, "Coldplay", 1, 11, "Viva La Vida", 110 },
                    { 11, "Bob Marley", 1, 8, "Three Little Birds", 76 },
                    { 12, "Mark Ronson feat. Bruno Mars", 3, 9, "Uptown Funk", 115 },
                    { 13, "John Denver", 1, 10, "Take Me Home, Country Roads", 82 },
                    { 14, "Coldplay", 10, 1, "Yellow", 87 },
                    { 15, "Coldplay", 8, 11, "The Scientist", 73 },
                    { 16, "Journey", 7, 4, "Don't Stop Believin'", 119 },
                    { 17, "Bryan Adams", 6, 4, "Summer Of '69", 138 },
                    { 18, "Red Hot Chili Peppers", 9, 12, "Can't Stop", 91 },
                    { 19, "Avicii", 7, 7, "Wake Me Up", 124 },
                    { 20, "Guns N' Roses", 6, 4, "Sweet Child O' Mine", 125 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Musicas_EmocaoId",
                table: "Musicas",
                column: "EmocaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Musicas_GeneroId",
                table: "Musicas",
                column: "GeneroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Musicas");

            migrationBuilder.DropTable(
                name: "Emocoes");

            migrationBuilder.DropTable(
                name: "Generos");
        }
    }
}
