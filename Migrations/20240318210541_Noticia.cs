using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ms_usuario.Migrations
{
    /// <inheritdoc />
    public partial class Noticia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Noticia",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Resumo = table.Column<string>(type: "text", nullable: false),
                    Conteudo = table.Column<string>(type: "text", nullable: false),
                    UsuarioCadastroId = table.Column<long>(type: "bigint", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Noticia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NoticiaAreaInteresse",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NoticiaId = table.Column<long>(type: "bigint", nullable: false),
                    AreaInteresseId = table.Column<long>(type: "bigint", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoticiaAreaInteresse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoticiaAreaInteresse_AreaInteresse_AreaInteresseId",
                        column: x => x.AreaInteresseId,
                        principalTable: "AreaInteresse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoticiaAreaInteresse_Noticia_NoticiaId",
                        column: x => x.NoticiaId,
                        principalTable: "Noticia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoticiaAreaInteresse_AreaInteresseId",
                table: "NoticiaAreaInteresse",
                column: "AreaInteresseId");

            migrationBuilder.CreateIndex(
                name: "IX_NoticiaAreaInteresse_NoticiaId",
                table: "NoticiaAreaInteresse",
                column: "NoticiaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoticiaAreaInteresse");

            migrationBuilder.DropTable(
                name: "Noticia");
        }
    }
}
