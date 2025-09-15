using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoleBasedAuthentication.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddClassTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "ClassTable",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    classId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTable", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassMappingTable");

            migrationBuilder.DropTable(
                name: "ClassTable");
        }
    }
}
