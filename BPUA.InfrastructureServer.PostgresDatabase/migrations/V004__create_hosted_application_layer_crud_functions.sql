CREATE OR REPLACE FUNCTION hosted_application_layer.get(id_value bigint)
RETURNS SETOF public.hosted_application_layer
LANGUAGE sql
STABLE
AS $$
    SELECT *
    FROM public.hosted_application_layer entity
    WHERE entity.id = id_value
      AND is_archived = false
      AND is_deleted = false;
$$;

CREATE OR REPLACE FUNCTION hosted_application_layer.find(
    top_expression varchar(20) DEFAULT NULL,
    nullable_columns text DEFAULT NULL,
    order_by_clause text DEFAULT NULL,
    id_collection bigint[] DEFAULT NULL,
    application_layer_name_value varchar(32) DEFAULT NULL,
    domain_name_value varchar(128) DEFAULT NULL,
    guid_value uuid DEFAULT NULL,
    string_representation_value varchar(290) DEFAULT NULL,
    url_value varchar(1024) DEFAULT NULL,
    use_case_name_value varchar(128) DEFAULT NULL,
    business_guid_value uuid DEFAULT NULL,
    business_string_representation_value text DEFAULT NULL,
    created_by_user_guid_value uuid DEFAULT NULL,
    created_by_user_name_value varchar(50) DEFAULT NULL,
    date_of_creation_from timestamptz DEFAULT NULL,
    date_of_creation_to timestamptz DEFAULT NULL,
    date_of_modification_from timestamptz DEFAULT NULL,
    date_of_modification_to timestamptz DEFAULT NULL,
    is_archived_value boolean DEFAULT NULL,
    is_deleted_value boolean DEFAULT NULL,
    modified_by_user_guid_value uuid DEFAULT NULL,
    modified_by_user_name_value varchar(50) DEFAULT NULL)
RETURNS SETOF public.hosted_application_layer
LANGUAGE plpgsql
STABLE
AS $$
DECLARE
    sql_text text := 'SELECT * FROM public.hosted_application_layer WHERE true';
    limit_value integer;
    order_item text;
    order_column text;
    order_direction text;
    safe_order text := '';
    mapped_column text;
BEGIN
    IF top_expression IS NOT NULL THEN
        BEGIN
            limit_value := top_expression::integer;
        EXCEPTION WHEN invalid_text_representation THEN
            RAISE EXCEPTION 'Invalid TopExpression. Integer value is expected.';
        END;
        IF limit_value < 0 THEN
            RAISE EXCEPTION 'Invalid TopExpression. Non-negative integer value is expected.';
        END IF;
    END IF;

    IF id_collection IS NOT NULL AND cardinality(id_collection) > 0 THEN
        sql_text := sql_text || ' AND id = ANY($1)';
    END IF;
    IF application_layer_name_value IS NOT NULL THEN sql_text := sql_text || ' AND application_layer_name = $2'; END IF;
    IF domain_name_value IS NOT NULL THEN sql_text := sql_text || ' AND domain_name = $3'; END IF;
    IF guid_value IS NOT NULL THEN sql_text := sql_text || ' AND guid = $4'; END IF;
    IF string_representation_value IS NOT NULL THEN sql_text := sql_text || ' AND string_representation = $5'; END IF;
    IF url_value IS NOT NULL THEN sql_text := sql_text || ' AND url = $6'; END IF;
    IF use_case_name_value IS NOT NULL THEN sql_text := sql_text || ' AND use_case_name = $7'; END IF;
    IF business_guid_value IS NOT NULL THEN
        sql_text := sql_text || ' AND business_guid = $8';
    ELSIF position(',BusinessGuid,' in COALESCE(nullable_columns, '')) > 0 OR position(',business_guid,' in COALESCE(nullable_columns, '')) > 0 THEN
        sql_text := sql_text || ' AND business_guid IS NULL';
    END IF;
    IF business_string_representation_value IS NOT NULL THEN
        sql_text := sql_text || ' AND business_string_representation = $9';
    ELSIF position(',BusinessStringRepresentation,' in COALESCE(nullable_columns, '')) > 0 OR position(',business_string_representation,' in COALESCE(nullable_columns, '')) > 0 THEN
        sql_text := sql_text || ' AND business_string_representation IS NULL';
    END IF;
    IF created_by_user_guid_value IS NOT NULL THEN sql_text := sql_text || ' AND created_by_user_guid = $10'; END IF;
    IF created_by_user_name_value IS NOT NULL THEN sql_text := sql_text || ' AND created_by_user_name = $11'; END IF;
    IF date_of_creation_from IS NOT NULL THEN sql_text := sql_text || ' AND date_of_creation >= $12'; END IF;
    IF date_of_creation_to IS NOT NULL THEN sql_text := sql_text || ' AND date_of_creation < $13'; END IF;
    IF date_of_modification_from IS NOT NULL THEN sql_text := sql_text || ' AND date_of_modification >= $14'; END IF;
    IF date_of_modification_to IS NOT NULL THEN sql_text := sql_text || ' AND date_of_modification < $15'; END IF;
    IF is_archived_value IS NOT NULL THEN sql_text := sql_text || ' AND is_archived = $16'; ELSE sql_text := sql_text || ' AND is_archived = false'; END IF;
    IF is_deleted_value IS NOT NULL THEN sql_text := sql_text || ' AND is_deleted = $17'; ELSE sql_text := sql_text || ' AND is_deleted = false'; END IF;
    IF modified_by_user_guid_value IS NOT NULL THEN sql_text := sql_text || ' AND modified_by_user_guid = $18'; END IF;
    IF modified_by_user_name_value IS NOT NULL THEN sql_text := sql_text || ' AND modified_by_user_name = $19'; END IF;

    IF order_by_clause IS NOT NULL AND btrim(order_by_clause) <> '' THEN
        FOREACH order_item IN ARRAY string_to_array(order_by_clause, ',') LOOP
            order_item := btrim(order_item);
            order_direction := '';
            IF order_item ~* '\s+DESC$' THEN
                order_direction := ' DESC';
                order_column := btrim(regexp_replace(order_item, '\s+DESC$', '', 'i'));
            ELSIF order_item ~* '\s+ASC$' THEN
                order_direction := ' ASC';
                order_column := btrim(regexp_replace(order_item, '\s+ASC$', '', 'i'));
            ELSE
                order_column := order_item;
            END IF;
            order_column := trim(both '[]"' from order_column);
            mapped_column := CASE lower(order_column)
                WHEN 'id' THEN 'id'
                WHEN 'applicationlayername' THEN 'application_layer_name'
                WHEN 'application_layer_name' THEN 'application_layer_name'
                WHEN 'domainname' THEN 'domain_name'
                WHEN 'domain_name' THEN 'domain_name'
                WHEN 'url' THEN 'url'
                WHEN 'usecasename' THEN 'use_case_name'
                WHEN 'use_case_name' THEN 'use_case_name'
                WHEN 'businessguid' THEN 'business_guid'
                WHEN 'business_guid' THEN 'business_guid'
                WHEN 'businessstringrepresentation' THEN 'business_string_representation'
                WHEN 'business_string_representation' THEN 'business_string_representation'
                WHEN 'createdbyuserguid' THEN 'created_by_user_guid'
                WHEN 'created_by_user_guid' THEN 'created_by_user_guid'
                WHEN 'createdbyusername' THEN 'created_by_user_name'
                WHEN 'created_by_user_name' THEN 'created_by_user_name'
                WHEN 'dateofcreation' THEN 'date_of_creation'
                WHEN 'date_of_creation' THEN 'date_of_creation'
                WHEN 'dateofmodification' THEN 'date_of_modification'
                WHEN 'date_of_modification' THEN 'date_of_modification'
                WHEN 'guid' THEN 'guid'
                WHEN 'isarchived' THEN 'is_archived'
                WHEN 'is_archived' THEN 'is_archived'
                WHEN 'isdeleted' THEN 'is_deleted'
                WHEN 'is_deleted' THEN 'is_deleted'
                WHEN 'modifiedbyuserguid' THEN 'modified_by_user_guid'
                WHEN 'modified_by_user_guid' THEN 'modified_by_user_guid'
                WHEN 'modifiedbyusername' THEN 'modified_by_user_name'
                WHEN 'modified_by_user_name' THEN 'modified_by_user_name'
                WHEN 'stringrepresentation' THEN 'string_representation'
                WHEN 'string_representation' THEN 'string_representation'
                ELSE NULL
            END;
            IF mapped_column IS NULL THEN RAISE EXCEPTION 'Invalid OrderByClause.'; END IF;
            IF safe_order <> '' THEN safe_order := safe_order || ', '; END IF;
            safe_order := safe_order || quote_ident(mapped_column) || order_direction;
        END LOOP;
        sql_text := sql_text || ' ORDER BY ' || safe_order;
    END IF;
    IF limit_value IS NOT NULL THEN sql_text := sql_text || ' LIMIT ' || limit_value; END IF;

    RETURN QUERY EXECUTE sql_text USING
        id_collection, application_layer_name_value, domain_name_value, guid_value,
        string_representation_value, url_value, use_case_name_value, business_guid_value,
        business_string_representation_value, created_by_user_guid_value, created_by_user_name_value,
        date_of_creation_from, date_of_creation_to, date_of_modification_from,
        date_of_modification_to, is_archived_value, is_deleted_value,
        modified_by_user_guid_value, modified_by_user_name_value;
END;
$$;

CREATE OR REPLACE FUNCTION hosted_application_layer.save(
    hosted_application_layers jsonb,
    nullable_columns jsonb DEFAULT '[]'::jsonb,
    user_guid uuid DEFAULT NULL,
    user_name varchar(50) DEFAULT NULL)
RETURNS TABLE(id bigint, created_by_user_name varchar(50), date_of_creation timestamptz, value bigint, client_key varchar(50))
LANGUAGE plpgsql
AS $$
DECLARE
    source jsonb;
    inserted_id bigint;
    inserted_created_by_user_name varchar(50);
    inserted_date_of_creation timestamptz;
    source_id bigint;
    source_client_key varchar(50);
    effective_user_guid uuid;
    effective_user_name varchar(50);
BEGIN
    FOR source IN SELECT * FROM jsonb_array_elements(COALESCE(hosted_application_layers, '[]'::jsonb)) LOOP
        source_id := NULLIF(COALESCE(source->>'id', source->>'Id'), '')::bigint;
        source_client_key := COALESCE(source->>'__client_key', source->>'__ClientKey');

        IF COALESCE(source->>'__change_state', source->>'__ChangeState')::integer = 1 THEN
            effective_user_guid := COALESCE(user_guid, NULLIF(COALESCE(source->>'created_by_user_guid', source->>'CreatedByUserGuid'), '')::uuid, '00000000-0000-0000-0000-000000000001');
            effective_user_name := COALESCE(user_name, COALESCE(source->>'created_by_user_name', source->>'CreatedByUserName'), 'System');
            INSERT INTO public.hosted_application_layer
            (application_layer_name, domain_name, url, use_case_name, business_guid, business_string_representation,
             created_by_user_guid, created_by_user_name, modified_by_user_guid, modified_by_user_name)
            VALUES
            (COALESCE(source->>'application_layer_name', source->>'ApplicationLayerName'),
             COALESCE(source->>'domain_name', source->>'DomainName'),
             COALESCE(source->>'url', source->>'Url'),
             COALESCE(source->>'use_case_name', source->>'UseCaseName'),
             NULLIF(COALESCE(source->>'business_guid', source->>'BusinessGuid'), '')::uuid,
             COALESCE(source->>'business_string_representation', source->>'BusinessStringRepresentation'),
             effective_user_guid, effective_user_name, effective_user_guid, effective_user_name)
            RETURNING public.hosted_application_layer.id, public.hosted_application_layer.created_by_user_name,
                      public.hosted_application_layer.date_of_creation
            INTO inserted_id, inserted_created_by_user_name, inserted_date_of_creation;
            id := inserted_id; created_by_user_name := inserted_created_by_user_name;
            date_of_creation := inserted_date_of_creation; value := source_id; client_key := source_client_key;
            RETURN NEXT;
        ELSIF COALESCE(source->>'__change_state', source->>'__ChangeState')::integer = 2 THEN
            UPDATE public.hosted_application_layer destination SET
                application_layer_name = COALESCE(COALESCE(source->>'application_layer_name', source->>'ApplicationLayerName'), destination.application_layer_name),
                domain_name = COALESCE(COALESCE(source->>'domain_name', source->>'DomainName'), destination.domain_name),
                url = COALESCE(COALESCE(source->>'url', source->>'Url'), destination.url),
                use_case_name = COALESCE(COALESCE(source->>'use_case_name', source->>'UseCaseName'), destination.use_case_name),
                business_guid = NULLIF(COALESCE(source->>'business_guid', source->>'BusinessGuid'), '')::uuid,
                business_string_representation = COALESCE(source->>'business_string_representation', source->>'BusinessStringRepresentation'),
                date_of_modification = CASE WHEN user_name IS NULL THEN COALESCE(NULLIF(COALESCE(source->>'date_of_modification', source->>'DateOfModification'), '')::timestamptz, destination.date_of_modification) ELSE CURRENT_TIMESTAMP END,
                is_archived = COALESCE(NULLIF(COALESCE(source->>'is_archived', source->>'IsArchived'), '')::boolean, destination.is_archived),
                is_deleted = COALESCE(NULLIF(COALESCE(source->>'is_deleted', source->>'IsDeleted'), '')::boolean, destination.is_deleted),
                modified_by_user_guid = COALESCE(user_guid, NULLIF(COALESCE(source->>'modified_by_user_guid', source->>'ModifiedByUserGuid'), '')::uuid, destination.modified_by_user_guid),
                modified_by_user_name = COALESCE(user_name, COALESCE(source->>'modified_by_user_name', source->>'ModifiedByUserName'), destination.modified_by_user_name)
            WHERE destination.id = source_id;
        ELSIF COALESCE(source->>'__change_state', source->>'__ChangeState')::integer = 3 THEN
            UPDATE public.hosted_application_layer destination SET
                date_of_modification = CASE WHEN user_name IS NULL THEN COALESCE(NULLIF(COALESCE(source->>'date_of_modification', source->>'DateOfModification'), '')::timestamptz, CURRENT_TIMESTAMP) ELSE CURRENT_TIMESTAMP END,
                is_deleted = true,
                modified_by_user_guid = COALESCE(user_guid, NULLIF(COALESCE(source->>'modified_by_user_guid', source->>'ModifiedByUserGuid'), '')::uuid, destination.modified_by_user_guid),
                modified_by_user_name = COALESCE(user_name, COALESCE(source->>'modified_by_user_name', source->>'ModifiedByUserName'), destination.modified_by_user_name)
            WHERE destination.id = source_id;
        END IF;
    END LOOP;
END;
$$;
