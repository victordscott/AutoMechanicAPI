-- Create the car_mechanic database
-- This script creates the main database for the online consultation SaaS application

-- Create database (uncomment if creating from scratch)
-- CREATE DATABASE car_mechanic;
CREATE DATABASE car_mechanic;

-- Connect to the database
\c car_mechanic;

-- Enable UUID extension for generating UUIDs
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Set timezone to UTC for consistency
SET timezone = 'UTC';

CREATE TABLE IF NOT EXISTS logs
(
    message text COLLATE pg_catalog."default",
    message_template text COLLATE pg_catalog."default",
    level integer,
    "timestamp" timestamp without time zone,
    exception text COLLATE pg_catalog."default",
    log_event jsonb
);

CREATE SCHEMA IF NOT EXISTS hangfire;

