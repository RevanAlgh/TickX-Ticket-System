# TickX

TickX is a Client Support Ticketing System with a 3-tier architecture, secure authentication, and role-based access. Clients submit and manage tickets; team members update and close them. Managers assign tickets, manage users, and monitor performance. Key features include email integration, error handling, and a ChatGPT chatbot.

---

## Team Members:
- Yousef Almikainzi
- Revan Alghanaym
- Aseel Shaheen
- Sultan Almalaham

---

## Supervisors:
- Nuha Abou Hajar
- Nazeeh Salah

### Overview
- Idea
- Tools
- Features
- Role Cycle
- Demo Cycle
- Conclusion
- Schema
- System Architecture

---

### Idea
TickX is a Client Support Ticketing System where clients can submit, manage tickets, and track updates. Team members manage, update, and close tickets. Managers assign tickets, manage users, and monitor performance. The system is built on a 3-tier architecture with secure authentication and role-based access. It features email integration, error handling, automated ticket closing, and a ChatGPT chatbot.

---

### Tools

**Backend:**
- .NET Core 8.0 - Framework for building the application
- SQL Server - Database management system
- Swagger - API documentation and testing
- OpenAI API - ChatGPT integration

**Frontend:**
- HTML
- CSS
- Bootstrap
- Angular - Framework for building the application

---

### Schema
![Schema Image](https://github.com/user-attachments/assets/3926c54b-ef9f-442c-b56d-691aaa41bb49)

---

### System Architecture
![System Architecture Image](https://github.com/user-attachments/assets/6dd56854-99f5-4823-8edb-6c519fca9d98)

---

### Features

- **User Management:** Registration, login, password reset, JWT authentication.
- **Ticket Management:** Create, edit, assign, filter tickets; attachments; automatic closures.
- **Product Management:** Link products to tickets.
- **Dashboard Reporting:** Ticket statistics, charts and graphs, employee productivity.
- **ChatGPT Integration:** Chat functionality, answering prompts.
- **File Logging:** Records application events and errors for debugging, troubleshooting, auditing, and monitoring purposes.
- **API Response Model**
- **Health Checks:** Monitor database connection, external APIs, and system resources.
- **Email Sending and Notifications**
- **Create Advanced and Dynamic Forms**
- **Loading Indicator:** Displays while waiting for a response.

**Automated Ticket Closures (Job Scheduler):**
- Identify tickets in “in progress" status with no updates in 2 days.
- Send a reminder email to clients after 2 idle days, warning of ticket closure in 3 days.
- Send a second reminder 1 day before closure if the ticket remains idle for 4 days.
- Automatically closes tickets if no response is received within 5 days.

**Here, we tested it using minutes instead of days, specifically for testing purposes.**
![Schema Image](https://github.com/user-attachments/assets/3e910efc-aed5-4a90-8f7e-395f709c244a)
![Image1](https://github.com/user-attachments/assets/ad84381e-ecc3-4fe1-9596-b224b0c3bca5)
![Image2](https://github.com/user-attachments/assets/b80ccdde-80c5-4ed3-baf8-08c2eb4b4fbd)
![Image3](https://github.com/user-attachments/assets/2dd48117-9372-4df9-a7e8-540f264c068b)

---

### Role Cycle
- Manager
- Employee
- Client

---

### Demo Cycle
[Demo Video](https://drive.google.com/file/d/1pUxLfeSeTeEnbQxE5kSdsQVifpmD6aFB/view?usp=sharing)

---

### Conclusion
In conclusion, TickX streamlines client support through profile management and ticket creation, facilitating collaboration via dashboards, managing ticket assignments, and incorporating ChatGPT for communication. Its features enhance the overall support experience, ensuring responsiveness and productivity. An organization that uses TickX can significantly improve its client support efforts and satisfaction.

---

Thank you!
