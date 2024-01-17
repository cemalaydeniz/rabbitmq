<img src="https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white" />

## Rabbit MQ
This is basic console applications utilizing the message broker system "Rabbit MQ" in order to send and receive messages
- Server: It sends reports and messages when an product is sold or refunded or when an announcement is published
- Admin: It listens to the messages related to the reports
- Branch: It listens to the messages related to the announcements
- Stock: It listens to the messages related to the sales and refunds

## Getting Started
### Dependencies
- [.Net Core](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) Version 6.0
- [RabbitMQ Client](https://www.nuget.org/packages/RabbitMQ.Client) Version 6.8.0
```
nuget install RabbitMQ.Client -Version 6.8.0
```

## License
This project is licensed under the MIT License - see the LICENSE.md file for details
