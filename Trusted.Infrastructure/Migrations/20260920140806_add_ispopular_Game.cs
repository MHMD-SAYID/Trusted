using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trusted.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_ispopular_Game : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // No-op: IsPopular was already created in Add_Games.
            // Leaving this migration so existing databases that recorded it stay consistent.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
