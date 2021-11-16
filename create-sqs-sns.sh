
queue_name="first-queue"
topic_name="first-topic"
dlq_queue="${queue_name}-dlq"



aws --endpoint-url=http://localhost:4566 sns create-topic --name first-topic
aws --endpoint-url=http://localhost:4566 sqs create-queue --queue-name first-queue
aws --endpoint-url=http://localhost:4566 sqs create-queue --queue-name first-queue-dlq
aws --endpoint-url=http://localhost:4566 sns subscribe --topic-arn arn:aws:sns:us-east-1:000000000000:first-topic --protocol sqs --notification-endpoint arn:aws:sqs:us-east-1:000000000000:first-queue
aws --endpoint-url=http://localhost:4566 sqs set-queue-attributes --queue-url http://localhost:4566/000000000000/first-queue --attributes file://set-queue-attributes.json
