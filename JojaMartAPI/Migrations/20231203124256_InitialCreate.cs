using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JojaMartAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    username = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    dob = table.Column<DateTime>(type: "date", nullable: false),
                    gender = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false),
                    address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    calling_code = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    registration_date = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    last_login_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    account_status = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false),
                    profile_picture_url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__3214EC272BDBC05D", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "UQ__users__AB6E6164BAE3FE3F",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__users__F3DBC5722275F697",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
