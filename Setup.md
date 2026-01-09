# Setup & Run Instructions

## Prerequisites

- .NET 9 SDK
- Node.js (v16+)
- npm

---

## Run the Backend

```bash
cd server
dotnet restore
dotnet build
dotnet test
dotnet run
Backend will run at:

arduino
Copy code
https://localhost:5001
Run the Frontend
bash
Copy code
cd client
npm install
npm start
Open:

arduino
Copy code
http://localhost:3000
Ensure CORS is enabled in the backend for http://localhost:3000.

yaml
Copy code

---

## ✅ `ASSIGNMENT.md` (what you built & why)

This file focuses on **requirements, design, and evaluation**:

```md
# Assignment Details – Smart Inbox Engine

## Problem Statement

Users receive many emails and need help identifying what is most important.  
The Smart Inbox Engine assigns a priority score to emails and sorts them accordingly.

---

## Priority Scoring Rules

- VIP sender: +50
- Urgency keywords: +30
- Time decay: +1 per hour since received
- Spam keywords: −20
- Final score clamped between 0 and 100

---

## Architecture & Design

- Business logic isolated in service classes
- Controllers kept thin
- Dependency Injection used throughout
- Defensive coding applied

---

## Testing

- Backend: xUnit unit tests for scoring logic
- Frontend: React Testing Library for UI behavior

---

## AI Usage

AI tools were used as a productivity aid (unit test drafting, documentation cleanup, and technical questions).  
All final design and code decisions were made and reviewed by me.