using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppRelation.Migrations
{
    /// <inheritdoc />
    public partial class updatev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Availability",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
