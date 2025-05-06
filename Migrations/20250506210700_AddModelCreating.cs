using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DinamicaXP.Migrations
{
    /// <inheritdoc />
    public partial class AddModelCreating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contas_clientes_cliente_id",
                table: "contas");

            migrationBuilder.DropForeignKey(
                name: "FK_pagamentos_clientes_cliente_id",
                table: "pagamentos");

            migrationBuilder.AlterColumn<int>(
                name: "cliente_id",
                table: "pagamentos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "cliente_id",
                table: "contas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "clientes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "conta_id",
                table: "clientes",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_contas_clientes_cliente_id",
                table: "contas",
                column: "cliente_id",
                principalTable: "clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_pagamentos_clientes_cliente_id",
                table: "pagamentos",
                column: "cliente_id",
                principalTable: "clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contas_clientes_cliente_id",
                table: "contas");

            migrationBuilder.DropForeignKey(
                name: "FK_pagamentos_clientes_cliente_id",
                table: "pagamentos");

            migrationBuilder.DropColumn(
                name: "conta_id",
                table: "clientes");

            migrationBuilder.AlterColumn<int>(
                name: "cliente_id",
                table: "pagamentos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "cliente_id",
                table: "contas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "clientes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_contas_clientes_cliente_id",
                table: "contas",
                column: "cliente_id",
                principalTable: "clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_pagamentos_clientes_cliente_id",
                table: "pagamentos",
                column: "cliente_id",
                principalTable: "clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
