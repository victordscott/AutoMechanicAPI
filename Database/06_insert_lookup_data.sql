-- Insert initial data into lookup tables
-- This script populates the lookup tables with the required reference data

-- Insert Timezones (PostgreSQL recognized timezones)
INSERT INTO time_zone (time_zone_id, time_zone_name, time_zone_abbreviation, utc_offset, is_dst) VALUES
(1, 'UTC', 'UTC', '00:00:00', FALSE),
(2, 'America/New_York', 'EST/EDT', '-05:00:00', TRUE),
(3, 'America/Chicago', 'CST/CDT', '-06:00:00', TRUE),
(4, 'America/Denver', 'MST/MDT', '-07:00:00', TRUE),
(5, 'America/Los_Angeles', 'PST/PDT', '-08:00:00', TRUE),
(6, 'America/Phoenix', 'MST', '-07:00:00', FALSE),
(7, 'America/Anchorage', 'AKST/AKDT', '-09:00:00', TRUE),
(8, 'Pacific/Honolulu', 'HST', '-10:00:00', FALSE),
(9, 'Europe/London', 'GMT/BST', '00:00:00', TRUE),
(10, 'Europe/Paris', 'CET/CEST', '+01:00:00', TRUE),
(11, 'Europe/Berlin', 'CET/CEST', '+01:00:00', TRUE),
(12, 'Europe/Rome', 'CET/CEST', '+01:00:00', TRUE),
(13, 'Europe/Madrid', 'CET/CEST', '+01:00:00', TRUE),
(14, 'Europe/Amsterdam', 'CET/CEST', '+01:00:00', TRUE),
(15, 'Europe/Stockholm', 'CET/CEST', '+01:00:00', TRUE),
(16, 'Europe/Oslo', 'CET/CEST', '+01:00:00', TRUE),
(17, 'Europe/Copenhagen', 'CET/CEST', '+01:00:00', TRUE),
(18, 'Europe/Helsinki', 'EET/EEST', '+02:00:00', TRUE),
(19, 'Europe/Athens', 'EET/EEST', '+02:00:00', TRUE),
(20, 'Europe/Moscow', 'MSK', '+03:00:00', FALSE),
(21, 'Asia/Tokyo', 'JST', '+09:00:00', FALSE),
(22, 'Asia/Shanghai', 'CST', '+08:00:00', FALSE),
(23, 'Asia/Hong_Kong', 'HKT', '+08:00:00', FALSE),
(24, 'Asia/Singapore', 'SGT', '+08:00:00', FALSE),
(25, 'Asia/Seoul', 'KST', '+09:00:00', FALSE),
(26, 'Asia/Kolkata', 'IST', '+05:30:00', FALSE),
(27, 'Asia/Dubai', 'GST', '+04:00:00', FALSE),
(28, 'Asia/Tehran', 'IRST', '+03:30:00', FALSE),
(29, 'Asia/Karachi', 'PKT', '+05:00:00', FALSE),
(30, 'Asia/Dhaka', 'BST', '+06:00:00', FALSE),
(31, 'Asia/Bangkok', 'ICT', '+07:00:00', FALSE),
(32, 'Asia/Jakarta', 'WIB', '+07:00:00', FALSE),
(33, 'Asia/Manila', 'PHT', '+08:00:00', FALSE),
(34, 'Australia/Sydney', 'AEST/AEDT', '+10:00:00', TRUE),
(35, 'Australia/Melbourne', 'AEST/AEDT', '+10:00:00', TRUE),
(36, 'Australia/Brisbane', 'AEST', '+10:00:00', FALSE),
(37, 'Australia/Perth', 'AWST', '+08:00:00', FALSE),
(38, 'Australia/Adelaide', 'ACST/ACDT', '+09:30:00', TRUE),
(39, 'Pacific/Auckland', 'NZST/NZDT', '+12:00:00', TRUE),
(40, 'Pacific/Fiji', 'FJT', '+12:00:00', FALSE),
(41, 'America/Toronto', 'EST/EDT', '-05:00:00', TRUE),
(42, 'America/Vancouver', 'PST/PDT', '-08:00:00', TRUE),
(43, 'America/Montreal', 'EST/EDT', '-05:00:00', TRUE),
(44, 'America/Calgary', 'MST/MDT', '-07:00:00', TRUE),
(45, 'America/Winnipeg', 'CST/CDT', '-06:00:00', TRUE),
(46, 'America/Halifax', 'AST/ADT', '-04:00:00', TRUE),
(47, 'America/St_Johns', 'NST/NDT', '-03:30:00', TRUE),
(48, 'America/Mexico_City', 'CST/CDT', '-06:00:00', TRUE),
(49, 'America/Sao_Paulo', 'BRT', '-03:00:00', FALSE),
(50, 'America/Buenos_Aires', 'ART', '-03:00:00', FALSE),
(51, 'America/Lima', 'PET', '-05:00:00', FALSE),
(52, 'America/Bogota', 'COT', '-05:00:00', FALSE),
(53, 'America/Caracas', 'VET', '-04:00:00', FALSE),
(54, 'America/Santiago', 'CLT', '-04:00:00', FALSE),
(55, 'Africa/Cairo', 'EET', '+02:00:00', FALSE),
(56, 'Africa/Johannesburg', 'SAST', '+02:00:00', FALSE),
(57, 'Africa/Lagos', 'WAT', '+01:00:00', FALSE),
(58, 'Africa/Nairobi', 'EAT', '+03:00:00', FALSE),
(59, 'Africa/Casablanca', 'WET', '+00:00:00', FALSE),
(60, 'Africa/Tunis', 'CET', '+01:00:00', FALSE);

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


