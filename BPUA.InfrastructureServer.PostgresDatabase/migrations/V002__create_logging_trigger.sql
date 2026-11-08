CREATE OR REPLACE FUNCTION infrastructure_server.log_hosted_application_layer_change()
RETURNS trigger
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO public.hosted_application_layer_log
    (
        logged_entity_id,
        application_layer_name,
        domain_name,
        url,
        use_case_name,
        logged_entity_business_guid,
        logged_entity_business_string_representation,
        logged_entity_created_by_user_guid,
        logged_entity_created_by_user_name,
        logged_entity_date_of_creation,
        logged_entity_date_of_modification,
        logged_entity_guid,
        logged_entity_is_archived,
        logged_entity_is_deleted,
        logged_entity_modified_by_user_guid,
        logged_entity_modified_by_user_name,
        logged_entity_string_representation,
        business_guid,
        business_string_representation,
        created_by_user_guid,
        created_by_user_name,
        date_of_creation,
        date_of_modification,
        guid,
        is_archived,
        is_deleted,
        modified_by_user_guid,
        modified_by_user_name
    )
    VALUES
    (
        NEW.id,
        NEW.application_layer_name,
        NEW.domain_name,
        NEW.url,
        NEW.use_case_name,
        NEW.business_guid,
        NEW.business_string_representation,
        NEW.created_by_user_guid,
        NEW.created_by_user_name,
        NEW.date_of_creation,
        NEW.date_of_modification,
        NEW.guid,
        NEW.is_archived,
        NEW.is_deleted,
        NEW.modified_by_user_guid,
        NEW.modified_by_user_name,
        NEW.string_representation,
        NEW.business_guid,
        NEW.business_string_representation,
        '00000000-0000-0000-0000-000000000001',
        'System',
        CURRENT_TIMESTAMP,
        CURRENT_TIMESTAMP,
        gen_random_uuid(),
        false,
        false,
        '00000000-0000-0000-0000-000000000001',
        'System'
    );

    RETURN NEW;
END;
$$;

CREATE TRIGGER hosted_application_layer_logging
AFTER INSERT OR UPDATE ON public.hosted_application_layer
FOR EACH ROW
EXECUTE FUNCTION infrastructure_server.log_hosted_application_layer_change();
