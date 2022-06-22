using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EM.Entity.Migrations
{
    public partial class DoctorTable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
             name: "Doctors",
             columns: table => new
             {
                 DoctorId = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                 UserId = table.Column<int>(type: "int", nullable: false),
                 Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                 PhoneNumber = table.Column<int>(type: "decimal(18, 0)", nullable: false),
                 Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                 CreatedBy = table.Column<int>(type: "int", nullable: false),
                 CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                 ModifiedBy = table.Column<int>(type: "int", nullable: true),
                 ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
             },
             constraints: table =>
             {
                 table.PrimaryKey("PK_Doctor", x => x.DoctorId);
                 table.ForeignKey(
                       name: "FK_Doctor_User_UserId",
                       column: x => x.UserId,
                       principalTable: "User",
                       principalColumn: "UserId",
                       onDelete: ReferentialAction.Cascade);
             });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "Doctors");
        }
    }
}
