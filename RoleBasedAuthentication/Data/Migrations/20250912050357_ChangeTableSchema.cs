using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoleBasedAuthentication.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassTable",
                table: "ClassTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassNameMapping",
                table: "ClassNameMapping");

            migrationBuilder.RenameTable(
                name: "ClassTable",
                newName: "teacherClassMap");

            migrationBuilder.RenameTable(
                name: "ClassNameMapping",
                newName: "classNameMap");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "teacherClassMap",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_teacherClassMap",
                table: "teacherClassMap",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_classNameMap",
                table: "classNameMap",
                column: "classId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_teacherClassMap",
                table: "teacherClassMap");

            migrationBuilder.DropPrimaryKey(
                name: "PK_classNameMap",
                table: "classNameMap");

            migrationBuilder.DropColumn(
                name: "description",
                table: "teacherClassMap");

            migrationBuilder.RenameTable(
                name: "teacherClassMap",
                newName: "ClassTable");

            migrationBuilder.RenameTable(
                name: "classNameMap",
                newName: "ClassNameMapping");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassTable",
                table: "ClassTable",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassNameMapping",
                table: "ClassNameMapping",
                column: "classId");
        }
    }
}
