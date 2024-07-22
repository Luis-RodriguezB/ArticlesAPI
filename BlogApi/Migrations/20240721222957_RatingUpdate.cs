using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticlesAPI.Migrations
{
    /// <inheritdoc />
    public partial class RatingUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DisLike",
                table: "Ratings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisLike",
                table: "Ratings");
        }
    }
}
