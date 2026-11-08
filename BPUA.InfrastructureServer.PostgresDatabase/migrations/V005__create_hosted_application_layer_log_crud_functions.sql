CREATE OR REPLACE FUNCTION hosted_application_layer_log.get(id_value bigint)
RETURNS SETOF public.hosted_application_layer_log
LANGUAGE sql
STABLE
AS $$
    SELECT * FROM public.hosted_application_layer_log entity
    WHERE entity.id = id_value AND is_archived = false AND is_deleted = false;
$$;

-- The log entity has many generated filter parameters. PostgreSQL receives them
-- as a JSON object while preserving the same equality/range/null/default-active behavior.
CREATE OR REPLACE FUNCTION hosted_application_layer_log.find(
    filters jsonb DEFAULT '{}'::jsonb,
    top_expression varchar(20) DEFAULT NULL,
    nullable_columns text DEFAULT NULL,
    order_by_clause text DEFAULT NULL,
    id_collection bigint[] DEFAULT NULL)
RETURNS SETOF public.hosted_application_layer_log
LANGUAGE plpgsql
STABLE
AS $$
DECLARE
    sql_text text := 'SELECT * FROM public.hosted_application_layer_log WHERE true';
    key_name text;
    column_name text;
    value_text text;
    parameter_values text[] := ARRAY[]::text[];
    parameter_index integer := 0;
    limit_value integer;
    order_item text;
    order_column text;
    order_direction text;
    safe_order text := '';
BEGIN
    IF top_expression IS NOT NULL THEN
        BEGIN limit_value := top_expression::integer;
        EXCEPTION WHEN invalid_text_representation THEN
            RAISE EXCEPTION 'Invalid TopExpression. Integer value is expected.';
        END;
        IF limit_value < 0 THEN RAISE EXCEPTION 'Invalid TopExpression.'; END IF;
    END IF;

    IF id_collection IS NOT NULL AND cardinality(id_collection) > 0 THEN
        sql_text := sql_text || format(' AND id = ANY(%L::bigint[])', id_collection::text);
    END IF;

    FOR key_name, value_text IN SELECT key, value #>> '{}' FROM jsonb_each(COALESCE(filters, '{}'::jsonb)) LOOP
        IF lower(key_name) IN ('dateofcreationfrom', 'date_of_creation_from', 'dateofcreationto', 'date_of_creation_to',
                               'dateofmodificationfrom', 'date_of_modification_from', 'dateofmodificationto', 'date_of_modification_to') THEN
            CONTINUE;
        END IF;
        column_name := CASE lower(key_name)
            WHEN 'id' THEN 'id'
            WHEN 'applicationlayername' THEN 'application_layer_name' WHEN 'application_layer_name' THEN 'application_layer_name'
            WHEN 'domainname' THEN 'domain_name' WHEN 'domain_name' THEN 'domain_name'
            WHEN 'url' THEN 'url' WHEN 'usecasename' THEN 'use_case_name' WHEN 'use_case_name' THEN 'use_case_name'
            WHEN 'loggedentityid' THEN 'logged_entity_id' WHEN 'logged_entity_id' THEN 'logged_entity_id'
            WHEN 'loggedentitybusinessguid' THEN 'logged_entity_business_guid' WHEN 'logged_entity_business_guid' THEN 'logged_entity_business_guid'
            WHEN 'loggedentitybusinessstringrepresentation' THEN 'logged_entity_business_string_representation' WHEN 'logged_entity_business_string_representation' THEN 'logged_entity_business_string_representation'
            WHEN 'loggedentitycreatedbyuserguid' THEN 'logged_entity_created_by_user_guid' WHEN 'logged_entity_created_by_user_guid' THEN 'logged_entity_created_by_user_guid'
            WHEN 'loggedentitycreatedbyusername' THEN 'logged_entity_created_by_user_name' WHEN 'logged_entity_created_by_user_name' THEN 'logged_entity_created_by_user_name'
            WHEN 'loggedentitydateofcreation' THEN 'logged_entity_date_of_creation' WHEN 'logged_entity_date_of_creation' THEN 'logged_entity_date_of_creation'
            WHEN 'loggedentitydateofmodification' THEN 'logged_entity_date_of_modification' WHEN 'logged_entity_date_of_modification' THEN 'logged_entity_date_of_modification'
            WHEN 'loggedentityguid' THEN 'logged_entity_guid' WHEN 'logged_entity_guid' THEN 'logged_entity_guid'
            WHEN 'loggedentityisarchived' THEN 'logged_entity_is_archived' WHEN 'logged_entity_is_archived' THEN 'logged_entity_is_archived'
            WHEN 'loggedentityisdeleted' THEN 'logged_entity_is_deleted' WHEN 'logged_entity_is_deleted' THEN 'logged_entity_is_deleted'
            WHEN 'loggedentitymodifiedbyuserguid' THEN 'logged_entity_modified_by_user_guid' WHEN 'logged_entity_modified_by_user_guid' THEN 'logged_entity_modified_by_user_guid'
            WHEN 'loggedentitymodifiedbyusername' THEN 'logged_entity_modified_by_user_name' WHEN 'logged_entity_modified_by_user_name' THEN 'logged_entity_modified_by_user_name'
            WHEN 'loggedentitystringrepresentation' THEN 'logged_entity_string_representation' WHEN 'logged_entity_string_representation' THEN 'logged_entity_string_representation'
            WHEN 'businessguid' THEN 'business_guid' WHEN 'business_guid' THEN 'business_guid'
            WHEN 'businessstringrepresentation' THEN 'business_string_representation' WHEN 'business_string_representation' THEN 'business_string_representation'
            WHEN 'createdbyuserguid' THEN 'created_by_user_guid' WHEN 'created_by_user_guid' THEN 'created_by_user_guid'
            WHEN 'createdbyusername' THEN 'created_by_user_name' WHEN 'created_by_user_name' THEN 'created_by_user_name'
            WHEN 'guid' THEN 'guid'
            WHEN 'isarchived' THEN 'is_archived' WHEN 'is_archived' THEN 'is_archived'
            WHEN 'isdeleted' THEN 'is_deleted' WHEN 'is_deleted' THEN 'is_deleted'
            WHEN 'modifiedbyuserguid' THEN 'modified_by_user_guid' WHEN 'modified_by_user_guid' THEN 'modified_by_user_guid'
            WHEN 'modifiedbyusername' THEN 'modified_by_user_name' WHEN 'modified_by_user_name' THEN 'modified_by_user_name'
            WHEN 'stringrepresentation' THEN 'string_representation' WHEN 'string_representation' THEN 'string_representation'
            ELSE NULL END;
        IF column_name IS NULL THEN RAISE EXCEPTION 'Unsupported filter column: %', key_name; END IF;
        IF value_text IS NULL THEN
            sql_text := sql_text || format(' AND %I IS NULL', column_name);
        ELSE
            sql_text := sql_text || format(' AND %I::text = %L', column_name, value_text);
        END IF;
    END LOOP;

    IF NOT (filters ? 'DateOfCreationFrom' OR filters ? 'date_of_creation_from') THEN NULL; ELSE
        sql_text := sql_text || format(' AND date_of_creation >= %L::timestamptz', COALESCE(filters->>'DateOfCreationFrom', filters->>'date_of_creation_from'));
    END IF;
    IF filters ? 'DateOfCreationTo' OR filters ? 'date_of_creation_to' THEN
        sql_text := sql_text || format(' AND date_of_creation < %L::timestamptz', COALESCE(filters->>'DateOfCreationTo', filters->>'date_of_creation_to'));
    END IF;
    IF filters ? 'DateOfModificationFrom' OR filters ? 'date_of_modification_from' THEN
        sql_text := sql_text || format(' AND date_of_modification >= %L::timestamptz', COALESCE(filters->>'DateOfModificationFrom', filters->>'date_of_modification_from'));
    END IF;
    IF filters ? 'DateOfModificationTo' OR filters ? 'date_of_modification_to' THEN
        sql_text := sql_text || format(' AND date_of_modification < %L::timestamptz', COALESCE(filters->>'DateOfModificationTo', filters->>'date_of_modification_to'));
    END IF;

    IF NOT (filters ? 'IsArchived' OR filters ? 'is_archived') THEN sql_text := sql_text || ' AND is_archived = false'; END IF;
    IF NOT (filters ? 'IsDeleted' OR filters ? 'is_deleted') THEN sql_text := sql_text || ' AND is_deleted = false'; END IF;

    IF order_by_clause IS NOT NULL AND btrim(order_by_clause) <> '' THEN
        FOREACH order_item IN ARRAY string_to_array(order_by_clause, ',') LOOP
            order_item := btrim(order_item); order_direction := '';
            IF order_item ~* '\s+DESC$' THEN order_direction := ' DESC'; order_column := btrim(regexp_replace(order_item, '\s+DESC$', '', 'i'));
            ELSIF order_item ~* '\s+ASC$' THEN order_direction := ' ASC'; order_column := btrim(regexp_replace(order_item, '\s+ASC$', '', 'i'));
            ELSE order_column := order_item; END IF;
            order_column := trim(both '[]"' from order_column);
            column_name := CASE lower(order_column)
                WHEN 'id' THEN 'id' WHEN 'applicationlayername' THEN 'application_layer_name' WHEN 'application_layer_name' THEN 'application_layer_name'
                WHEN 'domainname' THEN 'domain_name' WHEN 'domain_name' THEN 'domain_name' WHEN 'url' THEN 'url'
                WHEN 'usecasename' THEN 'use_case_name' WHEN 'use_case_name' THEN 'use_case_name'
                WHEN 'dateofcreation' THEN 'date_of_creation' WHEN 'date_of_creation' THEN 'date_of_creation'
                WHEN 'dateofmodification' THEN 'date_of_modification' WHEN 'date_of_modification' THEN 'date_of_modification'
                WHEN 'guid' THEN 'guid' WHEN 'isarchived' THEN 'is_archived' WHEN 'is_archived' THEN 'is_archived'
                WHEN 'isdeleted' THEN 'is_deleted' WHEN 'is_deleted' THEN 'is_deleted'
                WHEN 'stringrepresentation' THEN 'string_representation' WHEN 'string_representation' THEN 'string_representation'
                WHEN 'loggedentityid' THEN 'logged_entity_id' WHEN 'logged_entity_id' THEN 'logged_entity_id'
                ELSE NULL END;
            IF column_name IS NULL THEN RAISE EXCEPTION 'Invalid OrderByClause.'; END IF;
            IF safe_order <> '' THEN safe_order := safe_order || ', '; END IF;
            safe_order := safe_order || quote_ident(column_name) || order_direction;
        END LOOP;
        sql_text := sql_text || ' ORDER BY ' || safe_order;
    END IF;
    IF limit_value IS NOT NULL THEN sql_text := sql_text || ' LIMIT ' || limit_value; END IF;
    RETURN QUERY EXECUTE sql_text;
END;
$$;

CREATE OR REPLACE FUNCTION hosted_application_layer_log.save(
    hosted_application_layer_logs jsonb,
    nullable_columns jsonb DEFAULT '[]'::jsonb,
    user_guid uuid DEFAULT NULL,
    user_name varchar(50) DEFAULT NULL)
RETURNS TABLE(id bigint, created_by_user_name varchar(50), date_of_creation timestamptz, value bigint, client_key varchar(50))
LANGUAGE plpgsql
AS $$
DECLARE
    source jsonb;
    state integer;
    source_id bigint;
    source_client_key varchar(50);
    inserted_id bigint;
    inserted_created_by_user_name varchar(50);
    inserted_date_of_creation timestamptz;
    explicit_nulls jsonb;
BEGIN
    FOR source IN SELECT * FROM jsonb_array_elements(COALESCE(hosted_application_layer_logs, '[]'::jsonb)) LOOP
        state := COALESCE(source->>'__change_state', source->>'__ChangeState')::integer;
        source_id := NULLIF(COALESCE(source->>'id', source->>'Id'), '')::bigint;
        source_client_key := COALESCE(source->>'__client_key', source->>'__ClientKey');
        SELECT COALESCE(jsonb_agg(value), '[]'::jsonb) INTO explicit_nulls
        FROM jsonb_array_elements(COALESCE(nullable_columns, '[]'::jsonb)) value
        WHERE COALESCE(value->>'id', value->>'Id')::bigint = source_id;

        IF state = 1 THEN
            INSERT INTO public.hosted_application_layer_log
            (application_layer_name, domain_name, url, use_case_name, logged_entity_id,
             logged_entity_business_guid, logged_entity_business_string_representation,
             logged_entity_created_by_user_guid, logged_entity_created_by_user_name,
             logged_entity_date_of_creation, logged_entity_date_of_modification,
             logged_entity_guid, logged_entity_is_archived, logged_entity_is_deleted,
             logged_entity_modified_by_user_guid, logged_entity_modified_by_user_name,
             logged_entity_string_representation, business_guid, business_string_representation,
             created_by_user_guid, created_by_user_name, modified_by_user_guid, modified_by_user_name)
            VALUES
            (COALESCE(source->>'application_layer_name', source->>'ApplicationLayerName'),
             COALESCE(source->>'domain_name', source->>'DomainName'), COALESCE(source->>'url', source->>'Url'),
             COALESCE(source->>'use_case_name', source->>'UseCaseName'), NULLIF(COALESCE(source->>'logged_entity_id', source->>'LoggedEntityId'), '')::bigint,
             NULLIF(COALESCE(source->>'logged_entity_business_guid', source->>'LoggedEntityBusinessGuid'), '')::uuid,
             COALESCE(source->>'logged_entity_business_string_representation', source->>'LoggedEntityBusinessStringRepresentation'),
             NULLIF(COALESCE(source->>'logged_entity_created_by_user_guid', source->>'LoggedEntityCreatedByUserGuid'), '')::uuid,
             COALESCE(source->>'logged_entity_created_by_user_name', source->>'LoggedEntityCreatedByUserName'),
             NULLIF(COALESCE(source->>'logged_entity_date_of_creation', source->>'LoggedEntityDateOfCreation'), '')::timestamptz,
             NULLIF(COALESCE(source->>'logged_entity_date_of_modification', source->>'LoggedEntityDateOfModification'), '')::timestamptz,
             NULLIF(COALESCE(source->>'logged_entity_guid', source->>'LoggedEntityGuid'), '')::uuid,
             NULLIF(COALESCE(source->>'logged_entity_is_archived', source->>'LoggedEntityIsArchived'), '')::boolean,
             NULLIF(COALESCE(source->>'logged_entity_is_deleted', source->>'LoggedEntityIsDeleted'), '')::boolean,
             NULLIF(COALESCE(source->>'logged_entity_modified_by_user_guid', source->>'LoggedEntityModifiedByUserGuid'), '')::uuid,
             COALESCE(source->>'logged_entity_modified_by_user_name', source->>'LoggedEntityModifiedByUserName'),
             COALESCE(source->>'logged_entity_string_representation', source->>'LoggedEntityStringRepresentation'),
             NULLIF(COALESCE(source->>'business_guid', source->>'BusinessGuid'), '')::uuid,
             COALESCE(source->>'business_string_representation', source->>'BusinessStringRepresentation'),
             COALESCE(user_guid, '00000000-0000-0000-0000-000000000001'), COALESCE(user_name, 'System'),
             COALESCE(user_guid, '00000000-0000-0000-0000-000000000001'), COALESCE(user_name, 'System'))
            RETURNING public.hosted_application_layer_log.id, public.hosted_application_layer_log.created_by_user_name,
                      public.hosted_application_layer_log.date_of_creation
            INTO inserted_id, inserted_created_by_user_name, inserted_date_of_creation;
            id := inserted_id; created_by_user_name := inserted_created_by_user_name;
            date_of_creation := inserted_date_of_creation; value := source_id; client_key := source_client_key; RETURN NEXT;
        ELSIF state = 2 THEN
            UPDATE public.hosted_application_layer_log destination SET
                application_layer_name = COALESCE(COALESCE(source->>'application_layer_name', source->>'ApplicationLayerName'), destination.application_layer_name),
                domain_name = COALESCE(COALESCE(source->>'domain_name', source->>'DomainName'), destination.domain_name),
                url = COALESCE(COALESCE(source->>'url', source->>'Url'), destination.url),
                use_case_name = COALESCE(COALESCE(source->>'use_case_name', source->>'UseCaseName'), destination.use_case_name),
                logged_entity_id = CASE WHEN explicit_nulls @> '["LoggedEntityId"]'::jsonb OR explicit_nulls @> '["logged_entity_id"]'::jsonb THEN NULL ELSE COALESCE(NULLIF(COALESCE(source->>'logged_entity_id', source->>'LoggedEntityId'), '')::bigint, destination.logged_entity_id) END,
                business_guid = NULLIF(COALESCE(source->>'business_guid', source->>'BusinessGuid'), '')::uuid,
                business_string_representation = COALESCE(source->>'business_string_representation', source->>'BusinessStringRepresentation'),
                date_of_modification = CASE WHEN user_name IS NULL THEN COALESCE(NULLIF(COALESCE(source->>'date_of_modification', source->>'DateOfModification'), '')::timestamptz, destination.date_of_modification) ELSE CURRENT_TIMESTAMP END,
                is_archived = COALESCE(NULLIF(COALESCE(source->>'is_archived', source->>'IsArchived'), '')::boolean, destination.is_archived),
                is_deleted = COALESCE(NULLIF(COALESCE(source->>'is_deleted', source->>'IsDeleted'), '')::boolean, destination.is_deleted),
                modified_by_user_guid = COALESCE(user_guid, NULLIF(COALESCE(source->>'modified_by_user_guid', source->>'ModifiedByUserGuid'), '')::uuid, destination.modified_by_user_guid),
                modified_by_user_name = COALESCE(user_name, COALESCE(source->>'modified_by_user_name', source->>'ModifiedByUserName'), destination.modified_by_user_name)
            WHERE destination.id = source_id;
        ELSIF state = 3 THEN
            UPDATE public.hosted_application_layer_log destination SET
                date_of_modification = CURRENT_TIMESTAMP, is_deleted = true,
                modified_by_user_guid = COALESCE(user_guid, destination.modified_by_user_guid),
                modified_by_user_name = COALESCE(user_name, destination.modified_by_user_name)
            WHERE destination.id = source_id;
        END IF;
    END LOOP;
END;
$$;
