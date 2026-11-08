# SQL Server to PostgreSQL migration map

| SQL Server component | PostgreSQL component |
|---|---|
| `dbo.HostedApplicationLayer` | `public.hosted_application_layer` |
| `dbo.HostedApplicationLayerLog` | `public.hosted_application_layer_log` |
| `dbo.EntityRelation` | `public.entity_relation` |
| `HostedApplicationLayer.Get` | `hosted_application_layer.get` |
| `HostedApplicationLayer.Find` | `hosted_application_layer.find` |
| `HostedApplicationLayer.Save` | `hosted_application_layer.save` |
| `HostedApplicationLayerLog.Get` | `hosted_application_layer_log.get` |
| `HostedApplicationLayerLog.Find` | `hosted_application_layer_log.find` |
| `HostedApplicationLayerLog.Save` | `hosted_application_layer_log.save` |
| `FindHostedApplicationLayersByIdentifiers` | `hosted_application_layer.find_hosted_application_layers_by_identifiers` |
| `SynchronizeHostedApplicationLayers` | `hosted_application_layer.synchronize_hosted_application_layers` |
| `HostedApplicationLayerLogging` trigger | `infrastructure_server.log_hosted_application_layer_change` + trigger |
| SSDT `.sqlproj` deployment | Flyway migrations |
| SQL Server TVPs | `jsonb` arrays / PostgreSQL arrays |
