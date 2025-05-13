# EventEase 🎉  
A web-based event booking management system for organizing events, managing venues, and handling bookings with image uploads and Azure storage integration.

---

## 📌 Project Description

**EventEase** is an ASP.NET Core MVC application that allows users to:

- Create, edit, and view **events** and **venues**
- Manage **bookings** of venues for specific events
- Upload and store **venue images** in **Azure Blob Storage**
- View consolidated data using **SQL Server views**

---

## 🚀 Key Features

- ✅ Create and edit events and venues
- ✅ Upload and update venue images via Azure Blob Storage
- ✅ Prevent image loss during venue edits
- ✅ View bookings with joined venue/event info using a database view
- ✅ User-friendly web UI built with Razor pages
- ✅ Secure file handling and validation

---

## 🛠️ Technologies Used

- ASP.NET Core MVC (.NET 6 or later)
- Entity Framework Core
- SQL Server & SSMS
- Azure Blob Storage
- Bootstrap 5
- C#

---

## 🗃️ Database Structure

Tables:
- `Event` – stores event details
- `Venue` – stores venue information and images
- `Booking` – links venues and events with start and end dates

View:
- `dbo.FilterBookingView` – combines Booking, Event, and Venue data for easier reporting

### Sample View Query
```sql
SELECT * FROM dbo.FilterBookingView;
