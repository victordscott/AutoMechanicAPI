-- Insert initial data into lookup tables
-- This script populates the lookup tables with the required reference data

-- select * from pg_timezone_names;
insert into supported_time_zone (time_zone_abbrev, time_zone_name, time_zone_iana) values
('PST', 'Pacific Time', 'America/Los_Angeles'),
('MST', 'Mountain Time', 'America/Denver'),
('CST', 'Central Time', 'America/Chicago'),
('EST', 'Eastern Time', 'America/New_York'),
('AST', 'Atlantic Time', 'Canada/Atlantic'),
('NST', 'Newfoundland Time', 'Canada/Newfoundland');

INSERT INTO appointment_status (appointment_status_id, status_name) VALUES
(1, 'Requested'),
(2, 'Rescheduled'),
(3, 'Confirmed'),
(4, 'Canceled'),
(5, 'Started'),
(6, 'Successfully Completed');

INSERT INTO rating (rating_id, rating_name) VALUES
(1, 'Very Dissatisfied'),
(2, 'Dissatisfied'),
(3, 'Neutral'),
(4, 'Satisfied'),
(5, 'Very Satisfied');

INSERT INTO service_length (service_length_id, service_length_name, service_length_desc, length_minutes, service_length_cost) VALUES
(1, '15 Minutes', '', 15, 30.0),
(2, '30 Minutes', '', 30, 60.0),
(3, '45 Minutes', '', 45, 90.0),
(4, '1 Hour', '', 60, 120.0);

INSERT INTO file_type (file_type_id, file_type_name) VALUES
(1, 'Image'),
(2, 'Video'),
(3, 'PDF'),
(4, 'Text');


