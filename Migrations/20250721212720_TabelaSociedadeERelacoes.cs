using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ms_usuario.Migrations
{
    /// <inheritdoc />
    public partial class TabelaSociedadeERelacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Sociedade",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Sociedade",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<long>(
                name: "UsuarioLiderId",
                table: "Sociedade",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SociedadeId",
                table: "Noticia",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UsuarioSociedade",
                columns: table => new
                {
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    SociedadeId = table.Column<long>(type: "bigint", nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioSociedade", x => new { x.UsuarioId, x.SociedadeId });
                    table.ForeignKey(
                        name: "FK_UsuarioSociedade_Sociedade_SociedadeId",
                        column: x => x.SociedadeId,
                        principalTable: "Sociedade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioSociedade_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sociedade_UsuarioLiderId",
                table: "Sociedade",
                column: "UsuarioLiderId");

            migrationBuilder.CreateIndex(
                name: "IX_Noticia_SociedadeId",
                table: "Noticia",
                column: "SociedadeId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioSociedade_SociedadeId",
                table: "UsuarioSociedade",
                column: "SociedadeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Noticia_Sociedade_SociedadeId",
                table: "Noticia",
                column: "SociedadeId",
                principalTable: "Sociedade",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sociedade_Usuario_UsuarioLiderId",
                table: "Sociedade",
                column: "UsuarioLiderId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Noticia_Sociedade_SociedadeId",
                table: "Noticia");

            migrationBuilder.DropForeignKey(
                name: "FK_Sociedade_Usuario_UsuarioLiderId",
                table: "Sociedade");

            migrationBuilder.DropTable(
                name: "UsuarioSociedade");

            migrationBuilder.DropIndex(
                name: "IX_Sociedade_UsuarioLiderId",
                table: "Sociedade");

            migrationBuilder.DropIndex(
                name: "IX_Noticia_SociedadeId",
                table: "Noticia");

            migrationBuilder.DropColumn(
                name: "UsuarioLiderId",
                table: "Sociedade");

            migrationBuilder.DropColumn(
                name: "SociedadeId",
                table: "Noticia");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Sociedade",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Sociedade",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);
        }
    }
}
