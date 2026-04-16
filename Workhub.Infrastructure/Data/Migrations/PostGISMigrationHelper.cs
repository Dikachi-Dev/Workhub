using Microsoft.EntityFrameworkCore;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Data.Migrations;

/// <summary>
/// Helper class to apply PostGIS migration manually
/// </summary>
public static class PostGISMigrationHelper
{
    public static async Task ApplyPostGISMigration(AppDataContext context)
    {
        var sql = @"
-- Enable PostGIS extension
CREATE EXTENSION IF NOT EXISTS postgis;

-- Add Location column as geography type
ALTER TABLE ""Profiles"" 
ADD COLUMN IF NOT EXISTS ""Location"" geography(Point, 4326);

-- Create spatial index for fast proximity queries
CREATE INDEX IF NOT EXISTS idx_profile_location 
ON ""Profiles"" USING GIST (""Location"");

-- Migrate existing LongLat data to Location column
UPDATE ""Profiles""
SET ""Location"" = ST_SetSRID(
    ST_MakePoint(
        CAST(SPLIT_PART(""LongLat"", ',', 2) AS DOUBLE PRECISION),
        CAST(SPLIT_PART(""LongLat"", ',', 1) AS DOUBLE PRECISION)
    ), 
    4326
)
WHERE ""LongLat"" IS NOT NULL 
  AND ""LongLat"" != '' 
  AND ""Location"" IS NULL;
";

        try
        {
            await context.Database.ExecuteSqlRawAsync(sql);
        }
        catch (Exception ex) when (ex.Message.Contains("extension \"postgis\" is not available") || ex.InnerException?.Message.Contains("extension \"postgis\" is not available") == true)
        {
            // Log a friendly warning but don't crash the app if possible, or rethrow with clear message
            throw new Exception("CRITICAL: PostGIS extension is missing on the PostgreSQL server. Please install PostGIS to continue.", ex);
        }
    }
}
