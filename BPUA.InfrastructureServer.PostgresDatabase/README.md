# BPUA.InfrastructureServer.PostgresDatabase

PostgreSQL equivalent of `BPUA.InfrastructureServer.Database`.

## Design decisions

- Flyway migrations replace the SQL Server Database Project / DACPAC deployment model.
- Database identifiers use lowercase `snake_case` and are unquoted.
- PostgreSQL native `uuid`, `boolean`, identity columns, generated columns, and `timestamptz` are used.
- SQL Server table-valued parameters are represented by JSON arrays passed as `jsonb`.
- Generated CRUD operations are preserved as PostgreSQL functions in the
  `hosted_application_layer` and `hosted_application_layer_log` schemas.
- Writes use soft delete, matching the SQL Server implementation.

## Apply migrations

Update the local environment in `flyway.toml`, then run:

```text
flyway migrate
```

## JSON conventions

Input JSON accepts the original PascalCase property names and PostgreSQL
snake_case property names. Generated `save` functions expect `__ChangeState`:

- `1`: insert
- `2`: update
- `3`: soft delete

`nullable_columns` is a JSON array of property/column names explicitly set to
SQL NULL during an update.
