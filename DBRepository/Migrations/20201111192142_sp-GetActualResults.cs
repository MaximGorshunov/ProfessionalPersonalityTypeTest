using Microsoft.EntityFrameworkCore.Migrations;

namespace DBRepository.Migrations
{
    public partial class spGetActualResults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[GetActualResults]
                        AS
                        BEGIN
                            WITH cte AS
                            (
                               SELECT *,
                                     ROW_NUMBER() OVER (PARTITION BY UserId ORDER BY Date DESC) AS rn
                               FROM [dbo].[UserResults]
                            )
                            SELECT *
                            FROM cte
                            WHERE rn = 1
                        END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
