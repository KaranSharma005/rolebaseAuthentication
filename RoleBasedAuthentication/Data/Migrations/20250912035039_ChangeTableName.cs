using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoleBasedAuthentication.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassMappingTable");

            migrationBuilder.CreateTable(
                name: "ClassNameMapping",
                columns: table => new
                {
                    classId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    className = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassNameMapping", x => x.classId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassNameMapping");

            migrationBuilder.CreateTable(
                name: "ClassMappingTable",
                columns: table => new
                {
                    classId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    className = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassMappingTable", x => x.classId);
                });
        }
    }
}
