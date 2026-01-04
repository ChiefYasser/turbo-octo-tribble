# 🚀 HR Recruitment Management System

A full-stack **Recruitment & Applicant Tracking System (ATS)** built with **ASP.NET Core 8 MVC**. 
This application streamlines the hiring process, offering a modern dashboard for HR to manage job offers and analyze statistics, while allowing candidates to apply seamlessly.

![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-red)
![Bootstrap](https://img.shields.io/badge/UI-Bootstrap%205-blue)
![Status](https://img.shields.io/badge/Status-Complete-green)

---

## 🌟 Key Features

### 🏢 For HR & Admins
*   **Analytics Dashboard**: Visual charts (Pie & Bar) showing application statistics and job performance.
*   **Job Management**: Create, Edit, and Delete job offers with ease.
*   **Applicant Tracking**: Review candidates, download PDF resumes, and update status (*Accept/Reject*).
*   **Secure Access**: Role-based authentication (Admin/HR vs Candidate).

### 👤 For Candidates
*   **Smart Search**: Filter jobs by title, skill (e.g., "Java"), or location.
*   **Easy Apply**: Upload CVs (PDF validation included) and apply in one click.
*   **Application History**: Track the status of all submitted applications in real-time.

---

## 🛠️ Tech Stack

*   **Framework:** ASP.NET Core MVC (.NET 8)
*   **Language:** C#
*   **Database:** SQL Server (SSMS) / LocalDB
*   **ORM:** Entity Framework Core (Code-First)
*   **Authentication:** ASP.NET Core Identity
*   **Frontend:** Razor Views, Bootstrap 5, FontAwesome, Chart.js
*   **IDE:** Visual Studio 2022

---

## ⚙️ How to Run Locally

### 1. Prerequisites
*   Visual Studio 2022
*   .NET 8.0 SDK
*   SQL Server (or LocalDB)

### 2. Installation
1.  **Clone the repository**
    ```bash
    git clone https://github.com/ChiefYasser/turbo-octo-tribble.git
    ```
2.  **Open in Visual Studio**
    Double-click `HRRecruitmentSystem.sln`.

3.  **Configure Database**
    The app uses `(localdb)\mssqllocaldb` by default. If you have a full SQL instance, update the connection string in `appsettings.json`.

4.  **Run Migrations & Seed Data**
    Open **Package Manager Console** and run:
    ```powershell
    Update-Database
    ```
    *Note: The application will automatically seed Roles, a default Admin, and Demo Jobs upon the first run.*

5.  **Run the App**
    Press **F5** or click the Green Play button.

---

## 🔐 Default Admin Credentials
Use these credentials to access the Admin Dashboard:

*   **Email:** `admin@hr.com`
*   **Password:** `Admin@123`

---

