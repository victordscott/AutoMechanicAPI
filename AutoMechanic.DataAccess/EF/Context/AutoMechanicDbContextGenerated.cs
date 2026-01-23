using TimeZone = AutoMechanic.DataAccess.EF.Models.TimeZone;
using System;
using System.Collections.Generic;
using AutoMechanic.DataAccess.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoMechanic.DataAccess.EF.Context;

public partial class AutoMechanicDbContextGenerated : DbContext
{
    public AutoMechanicDbContextGenerated(DbContextOptions<AutoMechanicDbContextGenerated> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentLog> AppointmentLogs { get; set; }

    public virtual DbSet<AppointmentNote> AppointmentNotes { get; set; }

    public virtual DbSet<AppointmentStatus> AppointmentStatuses { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<ConsultantAvailabilityDate> ConsultantAvailabilityDates { get; set; }

    public virtual DbSet<ConsultantAvailabilitySchedule> ConsultantAvailabilitySchedules { get; set; }

    public virtual DbSet<ConsultantDetail> ConsultantDetails { get; set; }

    public virtual DbSet<ConsultantInfo> ConsultantInfos { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<TimeZone> TimeZones { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("appointment_pkey");

            entity.ToTable("appointment");

            entity.Property(e => e.AppointmentId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("appointment_id");
            entity.Property(e => e.AppointmentStatusId).HasColumnName("appointment_status_id");
            entity.Property(e => e.ConsultantConfirmed)
                .HasDefaultValue(false)
                .HasColumnName("consultant_confirmed");
            entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");
            entity.Property(e => e.ConsultantNote).HasColumnName("consultant_note");
            entity.Property(e => e.CustomerConfirmed)
                .HasDefaultValue(false)
                .HasColumnName("customer_confirmed");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CustomerNote).HasColumnName("customer_note");
            entity.Property(e => e.CustomerRatingId).HasColumnName("customer_rating_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnName("date_created");
            entity.Property(e => e.DateStatusChanged)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnName("date_status_changed");
            entity.Property(e => e.DateUpdated)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnName("date_updated");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.LengthMinutes).HasColumnName("length_minutes");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.AppointmentStatus).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.AppointmentStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_appt_appt_status");

            entity.HasOne(d => d.Consultant).WithMany(p => p.AppointmentConsultants)
                .HasForeignKey(d => d.ConsultantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_appt_consultant_user");

            entity.HasOne(d => d.Customer).WithMany(p => p.AppointmentCustomers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_appt_customer_user");

            entity.HasOne(d => d.CustomerRating).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.CustomerRatingId)
                .HasConstraintName("fk_appt_cust_rating");
        });

        modelBuilder.Entity<AppointmentLog>(entity =>
        {
            entity.HasKey(e => e.AppointmentLogId).HasName("appointment_log_pkey");

            entity.ToTable("appointment_log");

            entity.Property(e => e.AppointmentLogId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("appointment_log_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.AppointmentStatusId).HasColumnName("appointment_status_id");
            entity.Property(e => e.LogDate)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnName("log_date");
            entity.Property(e => e.Note).HasColumnName("note");

            entity.HasOne(d => d.Appointment).WithMany(p => p.AppointmentLogs)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_appt_log_appt");

            entity.HasOne(d => d.AppointmentStatus).WithMany(p => p.AppointmentLogs)
                .HasForeignKey(d => d.AppointmentStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_appt_log_appt_status");
        });

        modelBuilder.Entity<AppointmentNote>(entity =>
        {
            entity.HasKey(e => e.AppointmentNoteId).HasName("appointment_note_pkey");

            entity.ToTable("appointment_note");

            entity.Property(e => e.AppointmentNoteId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("appointment_note_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnName("date_created");
            entity.Property(e => e.DateUpdated)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnName("date_updated");
            entity.Property(e => e.DeletedDate).HasColumnName("deleted_date");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Appointment).WithMany(p => p.AppointmentNotes)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_appt_note_appt");

            entity.HasOne(d => d.User).WithMany(p => p.AppointmentNotes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_appt_note_user");
        });

        modelBuilder.Entity<AppointmentStatus>(entity =>
        {
            entity.HasKey(e => e.AppointmentStatusId).HasName("appointment_status_pkey");

            entity.ToTable("appointment_status");

            entity.Property(e => e.AppointmentStatusId)
                .ValueGeneratedNever()
                .HasColumnName("appointment_status_id");
            entity.Property(e => e.StatusDescription).HasColumnName("status_description");
            entity.Property(e => e.StatusName).HasColumnName("status_name");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AspNetRoles_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AspNetRoleClaims_pkey");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("AspNetRoleClaims_RoleId_fkey");
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AspNetUsers_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.AccessFailedCount).HasDefaultValue(0);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.EmailConfirmed).HasDefaultValue(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsEnabled).HasDefaultValue(true);
            entity.Property(e => e.LockoutEnabled).HasDefaultValue(false);
            entity.Property(e => e.LockoutEnd).HasColumnType("timestamp without time zone");
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.PhoneNumberConfirmed).HasDefaultValue(false);
            entity.Property(e => e.TwoFactorEnabled).HasDefaultValue(false);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasOne(d => d.TimeZone).WithMany(p => p.AspNetUsers)
                .HasForeignKey(d => d.TimeZoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AspNetUsers_TimeZoneId_fkey");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("AspNetUserRoles_RoleId_fkey"),
                    l => l.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("AspNetUserRoles_UserId_fkey"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("AspNetUserRoles_pkey");
                        j.ToTable("AspNetUserRoles");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AspNetUserClaims_pkey");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("AspNetUserClaims_UserId_fkey");
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey }).HasName("AspNetUserLogins_pkey");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("AspNetUserLogins_UserId_fkey");
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name }).HasName("AspNetUserTokens_pkey");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("AspNetUserTokens_UserId_fkey");
        });

        modelBuilder.Entity<ConsultantAvailabilityDate>(entity =>
        {
            entity.HasKey(e => e.ConsultantAvailabilityDateId).HasName("consultant_availability_date_pkey");

            entity.ToTable("consultant_availability_date");

            entity.Property(e => e.ConsultantAvailabilityDateId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("consultant_availability_date_id");
            entity.Property(e => e.ConsultantAvailabilityScheduleId).HasColumnName("consultant_availability_schedule_id");
            entity.Property(e => e.DateInserted)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnName("date_inserted");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.ConsultantAvailabilitySchedule).WithMany(p => p.ConsultantAvailabilityDates)
                .HasForeignKey(d => d.ConsultantAvailabilityScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_consult_avail_date_sched");

            entity.HasOne(d => d.User).WithMany(p => p.ConsultantAvailabilityDates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_consult_avail_date_user");
        });

        modelBuilder.Entity<ConsultantAvailabilitySchedule>(entity =>
        {
            entity.HasKey(e => e.ConsultantAvailabilityScheduleId).HasName("consultant_availability_schedule_pkey");

            entity.ToTable("consultant_availability_schedule");

            entity.Property(e => e.ConsultantAvailabilityScheduleId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("consultant_availability_schedule_id");
            entity.Property(e => e.DateInserted)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnName("date_inserted");
            entity.Property(e => e.DateUpdated)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnName("date_updated");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.EndTimeZone).HasColumnName("end_time_zone");
            entity.Property(e => e.IsAllDay)
                .HasDefaultValue(false)
                .HasColumnName("is_all_day");
            entity.Property(e => e.RecurrenceExceptions).HasColumnName("recurrence_exceptions");
            entity.Property(e => e.RecurrenceId).HasColumnName("recurrence_id");
            entity.Property(e => e.RecurrenceRule).HasColumnName("recurrence_rule");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.StartTimeZone).HasColumnName("start_time_zone");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.ConsultantAvailabilitySchedules)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("consultant_availability_schedule_user_id_fkey");
        });

        modelBuilder.Entity<ConsultantDetail>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("consultant_detail_pkey");

            entity.ToTable("consultant_detail");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Description).HasColumnName("description");

            entity.HasOne(d => d.User).WithOne(p => p.ConsultantDetail)
                .HasForeignKey<ConsultantDetail>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_consultant_detail_user");
        });

        modelBuilder.Entity<ConsultantInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("consultant_info");

            entity.Property(e => e.ConsultantDescription).HasColumnName("consultant_description");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.RoleName).HasMaxLength(256);
            entity.Property(e => e.TimeZoneAbbreviation)
                .HasMaxLength(10)
                .HasColumnName("time_zone_abbreviation");
            entity.Property(e => e.TimeZoneName)
                .HasMaxLength(100)
                .HasColumnName("time_zone_name");
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("logs");

            entity.Property(e => e.Exception).HasColumnName("exception");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.LogEvent)
                .HasColumnType("jsonb")
                .HasColumnName("log_event");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.MessageTemplate).HasColumnName("message_template");
            entity.Property(e => e.Timestamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timestamp");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("rating_pkey");

            entity.ToTable("rating");

            entity.Property(e => e.RatingId)
                .ValueGeneratedNever()
                .HasColumnName("rating_id");
            entity.Property(e => e.RatingName).HasColumnName("rating_name");
        });

        modelBuilder.Entity<TimeZone>(entity =>
        {
            entity.HasKey(e => e.TimeZoneId).HasName("time_zone_pkey");

            entity.ToTable("time_zone");

            entity.HasIndex(e => e.TimeZoneName, "idx_time_zone_name");

            entity.HasIndex(e => e.TimeZoneName, "time_zone_time_zone_name_key").IsUnique();

            entity.Property(e => e.TimeZoneId)
                .ValueGeneratedNever()
                .HasColumnName("time_zone_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.IsDst)
                .HasDefaultValue(false)
                .HasColumnName("is_dst");
            entity.Property(e => e.TimeZoneAbbreviation)
                .HasMaxLength(10)
                .HasColumnName("time_zone_abbreviation");
            entity.Property(e => e.TimeZoneName)
                .HasMaxLength(100)
                .HasColumnName("time_zone_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UtcOffset).HasColumnName("utc_offset");
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("user_detail");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.RoleName).HasMaxLength(256);
            entity.Property(e => e.TimeZoneAbbreviation)
                .HasMaxLength(10)
                .HasColumnName("time_zone_abbreviation");
            entity.Property(e => e.TimeZoneName)
                .HasMaxLength(100)
                .HasColumnName("time_zone_name");
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.UserLoginId).HasName("user_login_pkey");

            entity.ToTable("user_login");

            entity.Property(e => e.UserLoginId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("user_login_id");
            entity.Property(e => e.LoginDate)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnName("login_date");
            entity.Property(e => e.RefreshToken).HasColumnName("refresh_token");
            entity.Property(e => e.RefreshTokenExpiryTime).HasColumnName("refresh_token_expiry_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_login_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
