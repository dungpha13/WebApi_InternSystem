using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazingTech.InternSystem.Migrations
{
    public partial class Test25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternInfo_KiThucTap_KiThucTapId",
                schema: "dbo",
                table: "InternInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_InternInfo_TruongHoc_IdTruong",
                schema: "dbo",
                table: "InternInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_KiThucTap_TruongHoc_IdTruong",
                schema: "dbo",
                table: "KiThucTap");

            migrationBuilder.DropIndex(
                name: "IX_KiThucTap_IdTruong",
                schema: "dbo",
                table: "KiThucTap");

            migrationBuilder.DropIndex(
                name: "IX_InternInfo_IdTruong",
                schema: "dbo",
                table: "InternInfo");

            migrationBuilder.DropIndex(
                name: "IX_InternInfo_KiThucTapId",
                schema: "dbo",
                table: "InternInfo");

            migrationBuilder.DropColumn(
                name: "IdTruong",
                schema: "dbo",
                table: "KiThucTap");

            migrationBuilder.DropColumn(
                name: "IdKiThucTap",
                schema: "dbo",
                table: "InternInfo");

            migrationBuilder.DropColumn(
                name: "IdTruong",
                schema: "dbo",
                table: "InternInfo");

            migrationBuilder.AlterColumn<string>(
                name: "KiThucTapId",
                schema: "dbo",
                table: "InternInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "InternTruongKyThucTap",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdIntern = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdTruongHoc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdKiThucTap = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternTruongKyThucTap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternTruongKyThucTap_InternInfo_IdIntern",
                        column: x => x.IdIntern,
                        principalSchema: "dbo",
                        principalTable: "InternInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InternTruongKyThucTap_KiThucTap_IdKiThucTap",
                        column: x => x.IdKiThucTap,
                        principalSchema: "dbo",
                        principalTable: "KiThucTap",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InternTruongKyThucTap_TruongHoc_IdTruongHoc",
                        column: x => x.IdTruongHoc,
                        principalSchema: "dbo",
                        principalTable: "TruongHoc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InternTruongKyThucTap_IdIntern",
                schema: "dbo",
                table: "InternTruongKyThucTap",
                column: "IdIntern");

            migrationBuilder.CreateIndex(
                name: "IX_InternTruongKyThucTap_IdKiThucTap",
                schema: "dbo",
                table: "InternTruongKyThucTap",
                column: "IdKiThucTap");

            migrationBuilder.CreateIndex(
                name: "IX_InternTruongKyThucTap_IdTruongHoc",
                schema: "dbo",
                table: "InternTruongKyThucTap",
                column: "IdTruongHoc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternTruongKyThucTap",
                schema: "dbo");

            migrationBuilder.AddColumn<string>(
                name: "IdTruong",
                schema: "dbo",
                table: "KiThucTap",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KiThucTapId",
                schema: "dbo",
                table: "InternInfo",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdKiThucTap",
                schema: "dbo",
                table: "InternInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdTruong",
                schema: "dbo",
                table: "InternInfo",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KiThucTap_IdTruong",
                schema: "dbo",
                table: "KiThucTap",
                column: "IdTruong");

            migrationBuilder.CreateIndex(
                name: "IX_InternInfo_IdTruong",
                schema: "dbo",
                table: "InternInfo",
                column: "IdTruong");

            migrationBuilder.CreateIndex(
                name: "IX_InternInfo_KiThucTapId",
                schema: "dbo",
                table: "InternInfo",
                column: "KiThucTapId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternInfo_KiThucTap_KiThucTapId",
                schema: "dbo",
                table: "InternInfo",
                column: "KiThucTapId",
                principalSchema: "dbo",
                principalTable: "KiThucTap",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InternInfo_TruongHoc_IdTruong",
                schema: "dbo",
                table: "InternInfo",
                column: "IdTruong",
                principalSchema: "dbo",
                principalTable: "TruongHoc",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KiThucTap_TruongHoc_IdTruong",
                schema: "dbo",
                table: "KiThucTap",
                column: "IdTruong",
                principalSchema: "dbo",
                principalTable: "TruongHoc",
                principalColumn: "Id");
        }
    }
}
