-- Parameters - Typical prefixes:
-- p_ → parameter (p_id, p_name)
-- in_ → input parameter (in_customer_id)
-- arg_ → argument (arg_status)

-- Local Variables: - Typical prefixes:
-- v_ → variable (v_count, v_total)
-- tmp_ → temporary variable

-- Constants → c_ prefix (if used)

CREATE OR REPLACE FUNCTION delete_user_by_email(email_address text) RETURNS int
	LANGUAGE plpgsql
	AS $$
declare v_user_id uuid;
BEGIN
	select "Id" into v_user_id
	from "AspNetUsers"
	where "Email" = email_address;

	if (v_user_id is not null) then
		delete from user_login_otp_code
		where user_id = v_user_id;

		delete from user_login
		where user_id = v_user_id;
	
		delete from "AspNetUserRoles"
		where "UserId" = v_user_id;

		delete from "AspNetUsers"
		where "Id" = v_user_id;
	
		return 1;
	else
		return 0;
	end if;
END $$;

CREATE OR REPLACE FUNCTION get_vehicle_with_files(p_vehicle_id uuid)
RETURNS JSON LANGUAGE 
sql AS
-- variable declaration requires plpgsql and return statement
-- BEGIN and END do not work with sql (only plpsql)
--plpgsql AS
$$
--BEGIN
	--return (
	SELECT 
		json_build_object(
			'vehicle_id', a.vehicle_id,
			'customer_id', a.customer_id,
			'vehicle_vin', a.vehicle_vin,
			'vehicle_year', a.vehicle_year,
			'vehicle_make', a.vehicle_make,
			'vehicle_model', a.vehicle_model,
			'vin_lookup_result', a.vin_lookup_result,
			'current_mileage', a.current_mileage,			   
			'customer_note', a.customer_note,
			'consultant_note', a.consultant_note,
			'date_created', a.date_created,
			'date_updated', a.date_updated,
			'current_files', COALESCE
			(
				(
					SELECT json_agg 
					(
						json_build_object 
						(
							'vehicle_file_id', b.vehicle_file_id,
							'file_upload_id', b.file_upload_id,
							'file_type_name', d.file_type_name,
							'customer_note', b.customer_note,
							'consultant_note', b.customer_note,
							'date_created', b.date_created,
							'date_updated', a.date_updated,
							'file_type_id', c.file_type_id,
							'file_name', c.file_name,
							'url_domain', c.url_domain,
							'url_path', c.url_path,
							'original_file_name', c.original_file_name,
							'file_note', c.file_note,
							'file_size_bytes', c.file_size_bytes,
							'is_public', c.is_public,
							'upload_date_created', c.date_created
						) order by b.date_created desc
					)
					FROM vehicle_file b inner join file_upload c on b.file_upload_id = c.file_upload_id
					inner join file_type d on c.file_type_id = d.file_type_id
					WHERE b.vehicle_id = a.vehicle_id
					and b.is_deleted = false 
					and c.is_deleted = false
				), '[]'::json
			)
		)
	FROM vehicle a
	where vehicle_id = p_vehicle_id;
	--);
--END 
$$;




