-- ==========================================================
-- Rename business ownership columns to tenant ownership
-- ==========================================================

-- ----------------------------------------------------------
-- hosted_application_layer
-- ----------------------------------------------------------

ALTER TABLE public.hosted_application_layer
    RENAME COLUMN business_guid
    TO tenant_guid;

ALTER TABLE public.hosted_application_layer
    RENAME COLUMN business_string_representation
    TO tenant_string_representation;


-- ----------------------------------------------------------
-- hosted_application_layer_log
-- ----------------------------------------------------------

ALTER TABLE public.hosted_application_layer_log
    RENAME COLUMN business_guid
    TO tenant_guid;

ALTER TABLE public.hosted_application_layer_log
    RENAME COLUMN business_string_representation
    TO tenant_string_representation;