-- Create lookup tables for the ChlorineDioxideHub application
-- These tables contain reference data that other tables will reference

-- Timezone lookup table
DROP TABLE IF EXISTS time_zone;
CREATE TABLE time_zone (
    time_zone_id SMALLINT PRIMARY KEY,
    time_zone_name VARCHAR(100) NOT NULL UNIQUE,
    time_zone_abbreviation VARCHAR(10),
    utc_offset INTERVAL,
    is_dst BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- Create indexes for better performance
CREATE INDEX idx_time_zone_name ON time_zone(time_zone_name);
