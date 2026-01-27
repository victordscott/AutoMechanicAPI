-- Users Table
DROP TABLE IF EXISTS "AspNetUsers" CASCADE;
DROP TABLE IF EXISTS "AspNetRoles" CASCADE;
DROP TABLE IF EXISTS "AspNetUserRoles" CASCADE;
DROP TABLE IF EXISTS "AspNetUserClaims" CASCADE;
DROP TABLE IF EXISTS "AspNetRoleClaims" CASCADE;
DROP TABLE IF EXISTS "AspNetUserLogins" CASCADE;
DROP TABLE IF EXISTS "AspNetUserTokens" CASCADE;

CREATE TABLE "AspNetUsers" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "UserName" VARCHAR(256) NOT NULL,
    "NormalizedUserName" VARCHAR(256) NOT NULL,
    "Email" VARCHAR(256) NOT NULL,
    "NormalizedEmail" VARCHAR(256) NOT NULL,
    "EmailConfirmed" BOOLEAN NOT NULL DEFAULT FALSE,
	"FirstName" TEXT NOT NULL,
	"LastName" TEXT NOT NULL,
    "PasswordHash" TEXT,
    "SecurityStamp" TEXT,
    "ConcurrencyStamp" TEXT,
    "PhoneNumber" TEXT,
	"PhoneNumberConfirmed" BOOLEAN NOT NULL DEFAULT FALSE,
	"State" TEXT NOT NULL,
	"Country" TEXT NOT NULL,
	"TimeZoneAbbrev" TEXT NOT NULL,
    "TwoFactorEnabled" BOOLEAN NOT NULL DEFAULT FALSE,
    "LockoutEnd" TIMESTAMP,
    "LockoutEnabled" BOOLEAN NOT NULL DEFAULT FALSE,
    "AccessFailedCount" INT NOT NULL DEFAULT 0,
	"DateCreated" timestamptz NOT NULL default (now() at time zone 'utc'),
	"IsEnabled" bool NOT NULL default true,
	"IsActive" bool NOT NULL default true,
	FOREIGN KEY ("TimeZoneAbbrev") REFERENCES supported_time_zone(time_zone_abbrev),
	UNIQUE ("Email")
);

--ALTER TABLE "AspNetUsers"
--	ADD CONSTRAINT unique_email UNIQUE ("Email");

-- Roles Table
CREATE TABLE "AspNetRoles" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Name" VARCHAR(256) NOT NULL,
    "NormalizedName" VARCHAR(256) NOT NULL,
    "ConcurrencyStamp" TEXT
);

-- UserRoles Table
CREATE TABLE "AspNetUserRoles" (
    "UserId" UUID NOT NULL,
    "RoleId" UUID NOT NULL,
    PRIMARY KEY ("UserId", "RoleId"),
    FOREIGN KEY ("UserId") REFERENCES "AspNetUsers"("Id") ON DELETE CASCADE,
    FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles"("Id") ON DELETE CASCADE
);

-- UserClaims Table
CREATE TABLE "AspNetUserClaims" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" UUID NOT NULL,
    "ClaimType" TEXT,
    "ClaimValue" TEXT,
    FOREIGN KEY ("UserId") REFERENCES "AspNetUsers"("Id") ON DELETE CASCADE
);

-- RoleClaims Table
CREATE TABLE "AspNetRoleClaims" (
    "Id" SERIAL PRIMARY KEY,
    "RoleId" UUID NOT NULL,
    "ClaimType" TEXT,
    "ClaimValue" TEXT,
    FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles"("Id") ON DELETE CASCADE
);

-- UserLogins Table
CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" TEXT NOT NULL,
    "ProviderKey" TEXT NOT NULL,
    "ProviderDisplayName" TEXT,
    "UserId" UUID NOT NULL,
    PRIMARY KEY ("LoginProvider", "ProviderKey"),
    FOREIGN KEY ("UserId") REFERENCES "AspNetUsers"("Id") ON DELETE CASCADE
);

-- UserTokens Table
CREATE TABLE "AspNetUserTokens" (
    "UserId" UUID NOT NULL,
    "LoginProvider" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Value" TEXT,
    PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    FOREIGN KEY ("UserId") REFERENCES "AspNetUsers"("Id") ON DELETE CASCADE
);
