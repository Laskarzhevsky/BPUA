-- Generated Get
SELECT * FROM hosted_application_layer.get(1);

-- Generated Find
SELECT *
FROM hosted_application_layer.find(
    top_expression => '25',
    order_by_clause => 'DomainName ASC, UseCaseName ASC',
    domain_name_value => 'BPUA');

-- Generated Save
SELECT *
FROM hosted_application_layer.save(
    '[{"ApplicationLayerName":"Business","DomainName":"BPUA","Url":"https://localhost:5001","UseCaseName":"Home","__ClientKey":"11111111-1111-1111-1111-111111111111","__ChangeState":1}]'::jsonb);

-- Business synchronization
SELECT hosted_application_layer.synchronize_hosted_application_layers(
    '[{"ApplicationLayerName":"Business","DomainName":"BPUA","Url":"https://localhost:5001","UseCaseName":"Home"}]'::jsonb);
