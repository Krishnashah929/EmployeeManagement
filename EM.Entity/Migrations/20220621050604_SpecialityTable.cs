using Microsoft.EntityFrameworkCore.Migrations;

namespace EM.Entity.Migrations
{
    public partial class SpecialityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
             name: "Specialities",
             columns: table => new
             {
                 DoctorId = table.Column<int>(type: "int", nullable: false),
                 SpecialityId = table.Column<int>(type: "int", nullable: false),
                 
             },
             constraints: table =>
             {
                  
                 table.ForeignKey(
                       name: "FK_PK_Specialities_Doctors_DoctorId",
                       column: x => x.DoctorId,
                       principalTable: "Doctors",
                       principalColumn: "DoctorId",
                       onDelete: ReferentialAction.Cascade);
             });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "Specialities");
        }
    }
}
