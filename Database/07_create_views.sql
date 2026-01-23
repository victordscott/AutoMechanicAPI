-- Create useful views for the ChlorineDioxideHub application
-- These views provide convenient access to commonly queried data

-- View for user information with type and timezone details
DROP VIEW user_detail;

CREATE OR REPLACE VIEW user_detail AS
SELECT 
    a."Id" as "UserId",
    a."UserName",
    a."Email",
	a."EmailConfirmed",
	a."FirstName",
    a."LastName",
    a."PhoneNumber",
    a."PhoneNumberConfirmed",
	a."State",
	a."Country",
	a."TimeZoneId",
	a."DateCreated",
	a."IsEnabled",
	a."IsActive",
	b."RoleId",
	c."Name" as "RoleName",
    d.time_zone_name,
    d.time_zone_abbreviation
FROM "AspNetUsers" a
JOIN "AspNetUserRoles" b ON a."Id" = b."UserId"
JOIN "AspNetRoles" c ON b."RoleId" = c."Id"
JOIN time_zone d ON a."TimeZoneId" = d.time_zone_id;

DROP VIEW consultant_info;

CREATE OR REPLACE VIEW consultant_info AS
SELECT 
    a."Id" as "UserId",
    a."UserName",
    a."Email",
	a."EmailConfirmed",
	a."FirstName",
    a."LastName",
    a."PhoneNumber",
    a."PhoneNumberConfirmed",
	a."State",
	a."Country",
	a."TimeZoneId",
	a."DateCreated",
	a."IsEnabled",
	a."IsActive",
	b."RoleId",
	c."Name" as "RoleName",
    d.time_zone_name,
    d.time_zone_abbreviation,
	e.description as "consultant_description",
	e.primary_image_upload_id,
	e.primary_video_upload_id,
	f.file_type_id as "primary_image_file_type_id",
	g.file_type_id as "primary_video_file_type_id",
	f.file_name as "primary_image_file_name",
	g.file_name as "primary_video_file_name",
	f.file_note as "primary_image_file_note",
	g.file_note as "primary_video_file_note"
FROM "AspNetUsers" a
JOIN "AspNetUserRoles" b ON a."Id" = b."UserId"
JOIN "AspNetRoles" c ON b."RoleId" = c."Id"
JOIN time_zone d ON a."TimeZoneId" = d.time_zone_id
JOIN consultant_detail e on a."Id" = e.user_id
LEFT JOIN file_upload f on e.primary_image_upload_id = f.file_upload_id
LEFT JOIN file_upload g on e.primary_video_upload_id = g.file_upload_id
where c."Name" = 'Consultant';
