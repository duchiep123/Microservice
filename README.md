Start <br/>
docker-compose build <br/>
docker-compose up <br/>
<br/>
Create sqs, sns subscribing sqs to sns and config dead letter queue <br/>
run script: sh create-sqs-sns.shs
<br/>
Cloudwatch log <br/>
aws --endpoint-url=http://localhost:4566 logs get-log-events --log-group-name dev-logs --log-stream-name YYYY-MM-DD