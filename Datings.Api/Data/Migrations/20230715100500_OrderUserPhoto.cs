using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExcalibQuestions.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderUserPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "UserPhoto",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "UserPhoto");
        }
    }
}
