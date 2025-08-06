## ☎️ Phone Book Tutorial

This project consists of two microservices: `contact` and `report`.

Inter-service communication is handled via **RabbitMQ** with **MassTransit**. When a user initiates a report request through the `contact` service, a message is sent to RabbitMQ. The `report` service listens for this message, retrieves the contact data, and generates the report accordingly.

### 📤 Report Request:

<img width="1832" height="868" alt="report-request" src="https://github.com/user-attachments/assets/0bb51917-7783-494e-a473-1755b6cf886c" />

### 📥 Report Response:

<img width="912" height="597" alt="report-response" src="https://github.com/user-attachments/assets/f0df5f28-e6bd-45b6-befd-89ad88147c9b" />

---

**Thank you for reading!** 🙌
