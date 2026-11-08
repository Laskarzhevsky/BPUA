CREATE OR REPLACE FUNCTION hosted_application_layer.find_hosted_application_layers_by_identifiers(
    hosted_application_layers jsonb)
RETURNS SETOF public.hosted_application_layer
LANGUAGE sql
STABLE
AS $$
    SELECT destination.*
    FROM public.hosted_application_layer destination
    WHERE destination.is_archived = false
      AND destination.is_deleted = false
      AND EXISTS
      (
          SELECT 1
          FROM jsonb_array_elements(COALESCE(hosted_application_layers, '[]'::jsonb)) source
          WHERE COALESCE(source->>'domain_name', source->>'DomainName') = destination.domain_name
            AND COALESCE(source->>'use_case_name', source->>'UseCaseName') = destination.use_case_name
            AND COALESCE(source->>'application_layer_name', source->>'ApplicationLayerName') = destination.application_layer_name
      );
$$;

CREATE OR REPLACE FUNCTION hosted_application_layer.synchronize_hosted_application_layers(
    hosted_application_layers jsonb,
    user_guid uuid DEFAULT NULL,
    user_name varchar(50) DEFAULT NULL)
RETURNS void
LANGUAGE plpgsql
AS $$
DECLARE
    now_utc timestamptz := CURRENT_TIMESTAMP;
    effective_user_guid uuid := COALESCE(user_guid, '00000000-0000-0000-0000-000000000001');
    effective_user_name varchar(50) := COALESCE(user_name, 'System');
BEGIN
    CREATE TEMPORARY TABLE requested_hosted_application_layer
    (
        application_layer_name varchar(32) NOT NULL,
        domain_name varchar(128) NOT NULL,
        url varchar(1024) NOT NULL,
        use_case_name varchar(128) NOT NULL,
        business_guid uuid NULL,
        business_string_representation text NULL,
        PRIMARY KEY (domain_name, use_case_name, application_layer_name)
    ) ON COMMIT DROP;

    INSERT INTO requested_hosted_application_layer
    SELECT
        COALESCE(source->>'application_layer_name', source->>'ApplicationLayerName'),
        COALESCE(source->>'domain_name', source->>'DomainName'),
        MAX(COALESCE(source->>'url', source->>'Url')),
        COALESCE(source->>'use_case_name', source->>'UseCaseName'),
        MAX(NULLIF(COALESCE(source->>'business_guid', source->>'BusinessGuid'), '')::uuid),
        MAX(COALESCE(source->>'business_string_representation', source->>'BusinessStringRepresentation'))
    FROM jsonb_array_elements(COALESCE(hosted_application_layers, '[]'::jsonb)) source
    WHERE COALESCE(source->>'application_layer_name', source->>'ApplicationLayerName') IS NOT NULL
      AND COALESCE(source->>'domain_name', source->>'DomainName') IS NOT NULL
      AND COALESCE(source->>'url', source->>'Url') IS NOT NULL
      AND COALESCE(source->>'use_case_name', source->>'UseCaseName') IS NOT NULL
    GROUP BY
        COALESCE(source->>'application_layer_name', source->>'ApplicationLayerName'),
        COALESCE(source->>'domain_name', source->>'DomainName'),
        COALESCE(source->>'use_case_name', source->>'UseCaseName');

    UPDATE public.hosted_application_layer destination
    SET date_of_modification = now_utc,
        is_deleted = true,
        modified_by_user_guid = effective_user_guid,
        modified_by_user_name = effective_user_name
    WHERE destination.is_deleted = false
      AND EXISTS
      (
          SELECT 1 FROM requested_hosted_application_layer requested_url
          WHERE requested_url.url = destination.url
      )
      AND NOT EXISTS
      (
          SELECT 1 FROM requested_hosted_application_layer requested
          WHERE requested.domain_name = destination.domain_name
            AND requested.use_case_name = destination.use_case_name
            AND requested.application_layer_name = destination.application_layer_name
      );

    INSERT INTO public.hosted_application_layer
    (
        application_layer_name, domain_name, url, use_case_name,
        business_guid, business_string_representation,
        created_by_user_guid, created_by_user_name,
        date_of_creation, date_of_modification,
        is_archived, is_deleted,
        modified_by_user_guid, modified_by_user_name
    )
    SELECT
        source.application_layer_name, source.domain_name, source.url, source.use_case_name,
        source.business_guid, source.business_string_representation,
        effective_user_guid, effective_user_name,
        now_utc, now_utc, false, false,
        effective_user_guid, effective_user_name
    FROM requested_hosted_application_layer source
    ON CONFLICT (domain_name, use_case_name, application_layer_name)
    DO UPDATE SET
        url = EXCLUDED.url,
        business_guid = EXCLUDED.business_guid,
        business_string_representation = EXCLUDED.business_string_representation,
        date_of_modification = EXCLUDED.date_of_modification,
        is_archived = false,
        is_deleted = false,
        modified_by_user_guid = EXCLUDED.modified_by_user_guid,
        modified_by_user_name = EXCLUDED.modified_by_user_name;
END;
$$;
