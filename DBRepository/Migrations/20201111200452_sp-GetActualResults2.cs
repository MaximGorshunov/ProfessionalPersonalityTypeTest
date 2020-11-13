using Microsoft.EntityFrameworkCore.Migrations;

namespace DBRepository.Migrations
{
    public partial class spGetActualResults2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP PROCEDURE [dbo].[GetActualResults]";

            migrationBuilder.Sql(sp);

            sp = @"CREATE PROCEDURE [dbo].[GetActualResults]
                        @DateMin Date NULL,
						@DateMax Date NULL,
						@AgeMin int NULL,
						@AgeMax int NULL,
						@IsMan bit NULL,
						@ByLogin varchar NULL,
						@Actual bit
						AS
                        BEGIN
							WITH cte AS
                            (
                               SELECT ur.Id, ur.UserId, ur.[Date], ur.R, ur.I, ur.A, ur.S, ur.E, ur.C, 
									  CAST (DATEDIFF(DD, u.Birthdate, GETUTCDATE())/365.25 AS INT) AS age,
                                      ROW_NUMBER() OVER (PARTITION BY ur.UserId ORDER BY Date DESC) AS rn
                               FROM [dbo].[UserResults] ur
							   JOIN [dbo].[Users] u ON u.Id = ur.UserId
							   WHERE (@DateMin IS NULL OR ur.[Date] >= @DateMin) AND (@DateMax IS NULL OR ur.[Date] <= @DateMax)
									AND (@ByLogin IS NULL OR u.[Login] = @ByLogin )
									AND (@IsMan IS NULL OR u.IsMan = @IsMan)
                            )
                            SELECT cte.Id, cte.UserId, cte.[Date], cte.R, cte.I, cte.A, cte.S, cte.E, cte.C
                            FROM cte
                            WHERE (@Actual = 0 OR rn = 1)
								AND (@AgeMin IS NULL OR age >= @AgeMin) AND (@AgeMax IS NULL OR age <= @AgeMax)
                        END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
