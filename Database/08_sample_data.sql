-- Insert sample data for testing and development
-- This script creates sample users and data to test the application

-- Insert sample users

INSERT INTO "AspNetUsers"(
	"UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "FirstName", "LastName", "PhoneNumber", "PhoneNumberConfirmed", "State", "Country", "TimeZoneAbbrev", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount") VALUES 

-- Administrators
('victordscott', '', 'victordscott@gmail.com', '', true, 'Victor', 'Scott', '678-472-9866', true, 'GA', 'US', 'EST', false, true, 0),
('victord_scott@yahoo.com', '', 'victord_scott@yahoo.com', '', true, 'Victor', 'Scott', '678-472-9866', true, 'GA', 'US', 'EST', false, true, 0);

INSERT INTO "AspNetUserRoles"(
	"UserId", "RoleId") VALUES 

-- Administrator roles
	((SELECT "Id" FROM "AspNetUsers" WHERE "UserName" = 'victordscott'), '9ffa891a-351e-47f6-bfa0-fbf887bd169d');

INSERT INTO "AspNetUserRoles"(
	"UserId", "RoleId") VALUES 

-- Customer roles
	((SELECT "Id" FROM "AspNetUsers" WHERE "UserName" = 'victord_scott@yahoo.com'), 'f496117f-88ab-41eb-bc81-b3c0c2ed3dcd');

-- set NormalizedUserName and NormalizedEmail
update "AspNetUsers" set
"NormalizedUserName" = UPPER("UserName"),
"NormalizedEmail" = UPPER("Email");

-- select * from "AspNetUsers";