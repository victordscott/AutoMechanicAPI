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




