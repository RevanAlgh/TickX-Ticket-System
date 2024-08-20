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
![Schema Image](https://drive.google.com/file/d/1qP9ibk1LSVQ2-F_ZI0X0VnZ0XgxM_eJ7/view?usp=drive_link)

---

### System Architecture
![System Architecture Image](https://drive.google.com/file/d/10_P1_SXInSnrNPJKv3AgbaIufRiZFvph/view?usp=drive_link)

---

### System Layers:
- Web
- API
- Service
- DB

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
![Schema Image](https://drive.google.com/file/d/1UcyVM05qxFH0QKRLax5ErIgPKEXeoC7C/view?usp=drive_link)
![Schema Image](https://drive.google.com/file/d/1LPQd1Bx3VxQQllBK5-CwWZuuisJmILFk/view?usp=drive_link)
![Schema Image](https://drive.google.com/file/d/1Z1kH0sl6VgaRGjyZxTCmjvUikYknBR1b/view?usp=drive_link)
![Schema Image](https://drive.google.com/file/d/1eiyx8pCV-kWLEgzfVK-ad_OL27CCnkBC/view?usp=drive_link)

---

### Role Cycle
- Manager
- Employee
- Client

---

### Demo Cycle
[Video Link](https://drive.google.com/file/d/1pUxLfeSeTeEnbQxE5kSdsQVifpmD6aFB/view?usp=sharing)

---

### Conclusion
In conclusion, TickX streamlines client support through profile management and ticket creation, facilitating collaboration via dashboards, managing ticket assignments, and incorporating ChatGPT for communication. Its features enhance the overall support experience, ensuring responsiveness and productivity. An organization that uses TickX can significantly improve its client support efforts and satisfaction.

---

Thank you!
