-- Create lookup tables for the ChlorineDioxideHub application
-- These tables contain reference data that other tables will reference

-- Timezone lookup table
DROP TABLE IF EXISTS time_zone CASCADE;

DROP TABLE IF EXISTS supported_time_zone CASCADE;
CREATE TABLE supported_time_zone (
    time_zone_abbrev TEXT PRIMARY KEY,
    time_zone_name TEXT NOT NULL,
	time_zone_iana TEXT NOT NULL
);

