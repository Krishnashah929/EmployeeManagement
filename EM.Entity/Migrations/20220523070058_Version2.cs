using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EM.Entity.Migrations
{
    /// <summary>
    /// second  migration
    /// </summary>
    public partial class Version2 : Migration
    {
        /// <summary>
        /// override up method
        /// </summary>
        /// <param name="migrationBuilder"></param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetPasswordCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });
            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Degree = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    speciality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Experience = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.DoctorId);
                });

            //migrationBuilder.CreateTable(
            //   name: "AppointmentDetail",
            //   columns: table => new
            //   {
            //       AppointmentId = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
            //       PatientId = table.Column<int>(type: "int", nullable: false),
            //       DoctorId = table.Column<int>(type: "int", nullable: false),
            //       Diagnosis = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //       Remarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //       StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
            //       EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
            //       IsDelete = table.Column<bool>(type: "bit", nullable: false),
            //       CreatedBy = table.Column<int>(type: "int", nullable: false),
            //       CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //       ModifiedBy = table.Column<int>(type: "int", nullable: true),
            //       ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),

            //   },
            //   constraints: table =>
            //   {
            //       table.PrimaryKey("PK_AppointmentDetail", x => x.AppointmentId);
            //       table.ForeignKey(
            //           name: "FK_AppointmentDetail_Doctor_DoctorId",
            //           column: x => x.DoctorId,
            //           principalTable: "Doctor",
            //           principalColumn: "DoctorId",
            //           onDelete: ReferentialAction.Cascade);
            //   });
        
            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Compsitekey = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.UserRoleId);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");
        }

        /// <summary>
        /// override down method
        /// </summary>
        /// <param name="migrationBuilder"></param>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
               name: "Doctor");
            
            //migrationBuilder.DropTable(
            //   name: "AppointmentDetail");
        }
    }
}
