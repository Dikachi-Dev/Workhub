-- PostGIS Integration Migration
-- This script adds the Location column with PostGIS support to the Profiles table

-- Step 1: Enable PostGIS extension
CREATE EXTENSION IF NOT EXISTS postgis;

-- Step 2: Add Location column as geography type
-- geography(Point, 4326) uses WGS84 coordinate system and calculates geodesic distances
ALTER TABLE "Profiles" 
ADD COLUMN IF NOT EXISTS "Location" geography(Point, 4326);

-- Step 3: Create spatial index for fast proximity queries
-- GIST index is optimized for spatial data
CREATE INDEX IF NOT EXISTS idx_profile_location 
ON "Profiles" USING GIST ("Location");

-- Step 4: Migrate existing LongLat data to Location column
-- LongLat format is "latitude,longitude"
-- PostGIS Point format is (longitude, latitude) - note the reversed order
UPDATE "Profiles"
SET "Location" = ST_SetSRID(
    ST_MakePoint(
        CAST(SPLIT_PART("LongLat", ',', 2) AS DOUBLE PRECISION),  -- longitude
        CAST(SPLIT_PART("LongLat", ',', 1) AS DOUBLE PRECISION)   -- latitude
    ), 
    4326  -- WGS84 SRID
)
WHERE "LongLat" IS NOT NULL 
  AND "LongLat" != '' 
  AND "Location" IS NULL;

-- Step 5: Verify the migration
SELECT 
    COUNT(*) as total_profiles,
    COUNT("Location") as profiles_with_location,
    COUNT(*) - COUNT("Location") as profiles_without_location
FROM "Profiles";

-- Example query to test proximity search (optional)
-- This finds profiles within 50km of a given point
-- COMMENT OUT OR MODIFY THE COORDINATES BELOW:
/*
SELECT 
    "Id",
    "FirstName",
    "LastName",
    "Country",
    ST_Distance(
        "Location",
        ST_SetSRID(ST_MakePoint(3.3792, 6.5244), 4326)  -- Example: Lagos, Nigeria
    ) / 1000 as distance_km
FROM "Profiles"
WHERE "Location" IS NOT NULL
  AND ST_DWithin(
      "Location",
      ST_SetSRID(ST_MakePoint(3.3792, 6.5244), 4326),
      50000  -- 50km radius in meters
  )
ORDER BY "Location" <-> ST_SetSRID(ST_MakePoint(3.3792, 6.5244), 4326)
LIMIT 10;
*/
