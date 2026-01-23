DROP TABLE IF EXISTS service_length;
CREATE TABLE service_length (
	service_length_id smallint PRIMARY KEY,
    service_length_name TEXT NOT NULL,
    service_length_desc TEXT NOT NULL,
    length_minutes smallint NOT NULL
);

DROP TABLE IF EXISTS vehicle;
CREATE TABLE vehicle (
	vehicle_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    customer_id UUID NOT NULL,
    vehicle_year int NOT NULL,
    vehicle_make TEXT NOT NULL,
    vehicle_model TEXT NOT NULL,
    vehicle_submodel TEXT NULL,
    external_make_id int NULL,
    external_model_id int NULL,
    external_submodel_id int NULL,
    customer_note TEXT NULL,
    consultant_note TEXT NULL,
    date_created timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    date_updated timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    CONSTRAINT fk_customer_vehicle_user FOREIGN KEY (customer_id) REFERENCES "AspNetUsers"("Id")
);

DROP TABLE IF EXISTS vehicle_file;
CREATE TABLE vehicle_file (
	vehicle_file_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    vehicle_id UUID NOT NULL,
    file_type_id NOT NULL,
    file_name TEXT NOT NULL,
    customer_note TEXT NULL,
    consultant_note TEXT NULL,
    date_created timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    date_updated timestamptz NOT NULL DEFAULT (now() at time zone 'utc'),
    CONSTRAINT fk_vehicle_file_vehicle FOREIGN KEY (vehicle_id) REFERENCES vehicle(vehicle_id),
    CONSTRAINT fk_vehicle_file_type FOREIGN KEY (file_type_id) REFERENCES file_type(file_type_id)
);
