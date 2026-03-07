using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoccerTournaments.Tournaments.Migrations
{
    /// <inheritdoc />
    public partial class AddTournamentTeams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tournament_teams",
                columns: table => new
                {
                    tournament_id = table.Column<Guid>(type: "uuid", nullable: false),
                    team_id = table.Column<Guid>(type: "uuid", nullable: false),
                    added_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tournament_teams", x => new { x.tournament_id, x.team_id });
                    table.ForeignKey(
                        name: "FK_tournament_teams_tournaments_tournament_id",
                        column: x => x.tournament_id,
                        principalTable: "tournaments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tournament_teams_team_id",
                table: "tournament_teams",
                column: "team_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tournament_teams");
        }
    }
}
