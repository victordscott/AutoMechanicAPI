-- Create main tables for the ChlorineDioxideHub application
-- These tables contain the core business data

DROP TABLE IF EXISTS file_type CASCADE;
CREATE TABLE file_type (
    file_type_id SMALLINT PRIMARY KEY,
    file_type_name VARCHAR(100) NOT NULL
);

DROP TABLE IF EXISTS service_length CASCADE;
CREATE TABLE service_length (
	service_length_id smallint PRIMARY KEY,
    service_length_name TEXT NOT NULL,
    service_length_desc TEXT NOT NULL,
    length_minutes smallint NOT NULL,
    service_length_cost numeric(6,2) NOT NULL
);

DROP TABLE IF EXISTS rating CASCADE;
CREATE TABLE rating (
	rating_id smallint PRIMARY KEY,
    rating_name TEXT NOT NULL
);

DROP TABLE IF EXISTS file_upload CASCADE;

DROP TABLE IF EXISTS file_upload;
CREATE TABLE file_upload (
	file_upload_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    uploaded_by_id UUID NOT NULL,
    file_type_id SMALLINT NOT NULL,
    file_name TEXT NOT NULL,
	url_domain TEXT NOT NULL,
	url_path TEXT NOT NULL,
	original_file_name TEXT NOT NULL,
    file_note TEXT NULL,
	file_size_bytes INT NOT NULL,
	video_length_sec INT NULL,
    is_public bool NOT NULL DEFAULT false,
	date_created timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    date_updated timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
	is_deleted bool NOT NULL DEFAULT false,
	deleted_date timestamptz NULL,
    CONSTRAINT fk_file_upload_user FOREIGN KEY (uploaded_by_id) REFERENCES "AspNetUsers"("Id"),
    CONSTRAINT fk_file_upload_type FOREIGN KEY (file_type_id) REFERENCES file_type(file_type_id)
);

DROP TABLE IF EXISTS user_login CASCADE;
CREATE TABLE user_login (
	user_login_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
	user_id UUID NOT NULL,
	refresh_token TEXT NOT NULL,
	refresh_token_expiry_time timestamptz NOT NULL,
	login_date timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
	CONSTRAINT fk_user_login_user FOREIGN KEY (user_id) REFERENCES "AspNetUsers"("Id")
);

DROP TABLE IF EXISTS consultant_availability_schedule CASCADE;
CREATE TABLE consultant_availability_schedule (
	consultant_availability_schedule_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    user_id UUID NOT NULL,
	title TEXT NOT NULL,
	description TEXT NULL,
	start_time timestamptz NOT NULL,
	start_time_zone TEXT NULL,
	end_time timestamptz NOT NULL,
	end_time_zone TEXT NULL,
	is_all_day boolean NOT NULL default false,
	recurrence_id UUID NULL,
	recurrence_rule TEXT NULL,
	recurrence_exceptions TEXT NULL,
	date_inserted timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
	date_updated timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    FOREIGN KEY ("user_id") REFERENCES "AspNetUsers"("Id")
);


DROP TABLE IF EXISTS consultant_availability_date CASCADE;
CREATE TABLE consultant_availability_date (
	consultant_availability_date_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
	consultant_availability_schedule_id UUID NOT NULL,
	user_id UUID NOT NULL,
	start_date timestamptz NOT NULL,
	end_date timestamptz NOT NULL,
	date_inserted timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
	CONSTRAINT fk_consult_avail_date_user FOREIGN KEY (user_id) REFERENCES "AspNetUsers"("Id"),
	CONSTRAINT fk_consult_avail_date_sched FOREIGN KEY (consultant_availability_schedule_id) REFERENCES consultant_availability_schedule(consultant_availability_schedule_id)
);

DROP TABLE IF EXISTS consultant_detail CASCADE;
CREATE TABLE consultant_detail (
	user_id UUID PRIMARY KEY,
    description TEXT NOT NULL,
    primary_image_upload_id UUID NULL,
    primary_video_upload_id UUID NULL,
	CONSTRAINT fk_consultant_detail_user FOREIGN KEY (user_id) REFERENCES "AspNetUsers"("Id"),
	CONSTRAINT fk_consultant_detail_image_file FOREIGN KEY (primary_image_upload_id) REFERENCES file_upload(file_upload_id),
	CONSTRAINT fk_consultant_detail_video_file FOREIGN KEY (primary_video_upload_id) REFERENCES file_upload(file_upload_id)
);

DROP TABLE IF EXISTS appointment_status CASCADE;
CREATE TABLE appointment_status (
	appointment_status_id smallint PRIMARY KEY,
    status_name TEXT NOT NULL,
    status_description TEXT NULL
);

DROP TABLE IF EXISTS vehicle CASCADE;
CREATE TABLE vehicle (
	vehicle_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    customer_id UUID NOT NULL,
	vehicle_vin TEXT NULL,
    vehicle_year int NOT NULL,
    vehicle_make TEXT NOT NULL,
    vehicle_model TEXT NOT NULL,
    vehicle_submodel TEXT NULL,
    external_make_id int NULL,
    external_model_id int NULL,
    external_submodel_id int NULL,
	vin_lookup_result TEXT NULL,
    current_mileage int NULL,
    customer_note TEXT NULL,
    consultant_note TEXT NULL,
    date_created timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    date_updated timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
	is_deleted bool NOT NULL DEFAULT false,
	deleted_date timestamptz NULL,
    CONSTRAINT fk_customer_vehicle_user FOREIGN KEY (customer_id) REFERENCES "AspNetUsers"("Id")
);

DROP TABLE IF EXISTS vehicle_mileage CASCADE;
CREATE TABLE vehicle_mileage (
	vehicle_mileage_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    vehicle_id UUID NOT NULL,
    mileage_id int NOT NULL,
    date_created timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    CONSTRAINT fk_vehicle_mileage_vehicle FOREIGN KEY (vehicle_id) REFERENCES vehicle(vehicle_id)
);

DROP TABLE IF EXISTS vehicle_file CASCADE;
CREATE TABLE vehicle_file (
	vehicle_file_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    vehicle_id UUID NOT NULL,
    file_upload_id UUID NOT NULL,
    customer_note TEXT NULL,
    consultant_note TEXT NULL,
    date_created timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    date_updated timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
	is_deleted bool NOT NULL DEFAULT false,
	deleted_date timestamptz NULL,	
    CONSTRAINT fk_vehicle_file_upload_vehicle FOREIGN KEY (vehicle_id) REFERENCES vehicle(vehicle_id),
    CONSTRAINT fk_vehicle_file_upload_file FOREIGN KEY (file_upload_id) REFERENCES file_upload(file_upload_id)
);

DROP TABLE IF EXISTS appointment CASCADE;
CREATE TABLE appointment (
	appointment_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    consultant_id UUID NOT NULL,
    customer_id UUID NOT NULL,
	service_length_id smallint NOT NULL,
    appointment_status_id smallint NOT NULL,
	customer_rating_id smallint NULL,
    consultant_confirmed bool NOT NULL default false,
    customer_confirmed bool NOT NULL default false,
    consultant_note TEXT NULL,
    customer_note TEXT NULL,
    length_minutes smallint NOT NULL,
    start_date timestamptz NOT NULL,
    end_date timestamptz NOT NULL,
    date_status_changed timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    date_created timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    date_updated timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    CONSTRAINT fk_appt_consultant_user FOREIGN KEY (consultant_id) REFERENCES "AspNetUsers"("Id"),
    CONSTRAINT fk_appt_customer_user FOREIGN KEY (customer_id) REFERENCES "AspNetUsers"("Id"),
    CONSTRAINT fk_appt_service_length FOREIGN KEY (service_length_id) REFERENCES service_length(service_length_id),
    CONSTRAINT fk_appt_appt_status FOREIGN KEY (appointment_status_id) REFERENCES appointment_status(appointment_status_id),
    CONSTRAINT fk_appt_cust_rating FOREIGN KEY (customer_rating_id) REFERENCES rating(rating_id)
);

DROP TABLE IF EXISTS appointment_note CASCADE;
CREATE TABLE appointment_note (
	appointment_note_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    appointment_id UUID NOT NULL,
    user_id UUID NOT NULL,
    note TEXT NOT NULL,
    date_created timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    date_updated timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    is_deleted bool NOT NULL DEFAULT false,
    deleted_date timestamptz NULL,
    CONSTRAINT fk_appt_note_appt FOREIGN KEY (appointment_id) REFERENCES appointment(appointment_id),
    CONSTRAINT fk_appt_note_user FOREIGN KEY (user_id) REFERENCES "AspNetUsers"("Id")
);

DROP TABLE IF EXISTS appointment_file CASCADE;
CREATE TABLE appointment_file (
	appointment_file_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    appointment_id UUID NOT NULL,
    file_upload_id UUID NOT NULL,
    customer_note TEXT NULL,
    consultant_note TEXT NULL,
    date_created timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    date_updated timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
	is_deleted bool NOT NULL DEFAULT false,
	deleted_date timestamptz NULL,	
    CONSTRAINT fk_appt_file_upload_appt FOREIGN KEY (appointment_id) REFERENCES appointment(appointment_id),
    CONSTRAINT fk_appt_file_upload_file FOREIGN KEY (file_upload_id) REFERENCES file_upload(file_upload_id)
);

DROP TABLE IF EXISTS appointment_log CASCADE;
CREATE TABLE appointment_log (
	appointment_log_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    appointment_id UUID NOT NULL,
    appointment_status_id smallint NOT NULL,
    note TEXT NULL,
    log_date timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    is_deleted bool NOT NULL DEFAULT false,
    deleted_date timestamptz NULL,	
    CONSTRAINT fk_appt_log_appt FOREIGN KEY (appointment_id) REFERENCES appointment(appointment_id),
    CONSTRAINT fk_appt_log_appt_status FOREIGN KEY (appointment_status_id) REFERENCES appointment_status(appointment_status_id)
);

-- for directly making a file visible to a user?
DROP TABLE IF EXISTS user_file CASCADE;
CREATE TABLE user_file (
	user_file_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    user_id UUID NOT NULL,
    file_upload_id UUID NOT NULL,
    customer_note TEXT NULL,
    consultant_note TEXT NULL,
    date_created timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    date_updated timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
	is_deleted bool NOT NULL DEFAULT false,
	deleted_date timestamptz NULL,	
    CONSTRAINT fk_user_file_upload_user FOREIGN KEY (user_id) REFERENCES "AspNetUsers"("Id"),
    CONSTRAINT fk_user_file_upload_file FOREIGN KEY (file_upload_id) REFERENCES file_upload(file_upload_id)
);

DROP TABLE IF EXISTS user_login_otp_code CASCADE;
CREATE TABLE user_login_otp_code (
	user_login_otp_code_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
	user_id UUID NOT NULL,
	otp_code TEXT NOT NULL,
	otp_code_create_date timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
	otp_code_expire_date timestamptz NOT NULL,
	otp_code_used bool NOT NULL DEFAULT false,
	CONSTRAINT fk_user_login_otp_code_user FOREIGN KEY (user_id) REFERENCES "AspNetUsers"("Id")
);


