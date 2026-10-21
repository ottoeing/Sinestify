using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sinestify.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarMaisMusicas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Musicas",
                columns: new[] { "Id", "Cantor", "EmocaoId", "GeneroId", "Nome", "Velocidade" },
                values: new object[,]
                {
                    { 21, "Pharrell Williams", 1, 1, "Happy", 160 },
                    { 22, "Coldplay", 7, 2, "Fix You", 69 },
                    { 23, "Lewis Capaldi", 2, 1, "Someone You Loved", 110 },
                    { 24, "Dua Lipa", 9, 1, "Levitating", 103 },
                    { 25, "David Guetta feat. Sia", 7, 3, "Titanium", 126 },
                    { 26, "Coldplay", 6, 11, "Clocks", 131 },
                    { 27, "John Legend", 10, 4, "All of Me", 63 },
                    { 28, "Eminem", 9, 6, "Lose Yourself", 171 },
                    { 29, "Louis Armstrong", 5, 7, "What a Wonderful World", 107 },
                    { 30, "Bob Marley", 5, 8, "No Woman, No Cry", 78 },
                    { 31, "Chitãozinho & Xororó", 4, 10, "Evidências", 76 },
                    { 32, "Charlie Brown Jr.", 7, 2, "Dias de Luta, Dias de Glória", 98 },
                    { 33, "Tom Jobim", 10, 7, "Garota de Ipanema", 120 },
                    { 34, "Claude Debussy", 5, 5, "Clair de Lune", 69 },
                    { 35, "Linkin Park", 8, 12, "In the End", 105 },
                    { 36, "a-ha", 3, 1, "Take On Me", 169 },
                    { 37, "Gotye feat. Kimbra", 2, 11, "Somebody That I Used to Know", 129 },
                    { 38, "Bruno Mars", 10, 4, "Just the Way You Are", 109 },
                    { 39, "Survivor", 9, 2, "Eye of the Tiger", 109 },
                    { 40, "Bobby McFerrin", 1, 7, "Don't Worry Be Happy", 114 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Musicas",
                keyColumn: "Id",
                keyValue: 40);
        }
    }
}
