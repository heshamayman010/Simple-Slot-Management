# Slot Management System

A full-stack slot management feature for an online booking system, built with **ABP.io framework**, featuring **NodaTime** for time zone–agnostic slot storage and conversion.

**Live Demo:** http://hesham00-001-site1.mtempurl.com/

## Features

- Generate time slots over a date range with configurable duration
- NodaTime-based time zone handling (slots stored as Instants)
- View next available slots in any selected time zone
- Slot status tracking (Available / Booked)
- Book a slot endpoint
- Unit tests for slot generation and time zone conversion logic

## Technologies Used

- **Backend:** ABP.io (.NET 10 / C#) with NodaTime
- **Database:** SQL Server / SQLite for testing
- **Frontend:** Angular 21 (ABP Angular template)
- **Time Zone Library:** NodaTime (TZDB format)

## Prerequisites

- .NET SDK 10.0 or later
- Node.js 22+ (for Angular 21 compatibility)
- Angular CLI 21
- SQL Server 
- ABP CLI (`dotnet tool install -g Volo.Abp.Cli`)

## How to Run the Project

### Backend Setup

1. Navigate to the `aspnet-core` folder:
```bash
cd aspnet-core
```

2. Update the connection string 

3. Restore packages and update database and Install client-side UI libraries used by the framework:
```bash
abp install-libs
dotnet restore

dotnet run --project src/Vosita.DbMigrator
or run :
dotnet ef database update -p src/Vosita.EntityFrameworkCore
```

4. Run the backend:
```bash
cd src/Vosita.HttpApi.Host
dotnet run
```

Or simply:
```bash
dotnet watch --project src/Vosita.HttpApi.Host
```

The backend will run at:
- HTTPS: `https://localhost:44370`
- HTTP: `http://localhost:44369`
- Swagger UI: `https://localhost:44370/swagger`

### Frontend Setup

1. Navigate to the `angular` folder:
```bash
cd angular
```

2. Install dependencies:
```bash
npm install

npm install @swimlane/ngx-datatable@22.0.0 --save --legacy-peer-deps

```

3. Run the application:
```bash
ng serve


4. Open your browser and navigate to `http://localhost:4200`
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/app/slots/generate` | Generate slots based on date range, time zone, and duration |
| GET | `/api/app/slots/next` | Get next available slots in specified time zone |
| POST | `/api/app/slot/{id}/book-slot` | Book a specific slot by ID |


## NodaTime Implementation Notes

- All slots are stored as `Instant` (time zone–agnostic in UTC)
- Time zone conversion only happens when displaying slots to users
- Uses `DateTimeZoneProviders.Tzdb` for IANA time zone identifiers (e.g., `Africa/Cairo`)
- Slot generation uses `ZonedDateTime` in the provided time zone, then converts to `Instant` for storage
- Retrieval converts stored `Instant` back to `ZonedDateTime` in the requested time zone

## Testing

Run unit tests from the `aspnet-core` folder:
```bash
dotnet test
```

Tests cover:
- Slot generation logic
- NodaTime-based time zone conversions
- Date range validation

## Database Migrations

To add a new migration:
```bash

cd aspnet-core
dotnet ef migrations add "MigrationName" --project src/Vosita.EntityFrameworkCore --startup-project src/Vosita.DbMigrator

# to  Apply changes and re-run seed data via the Migrator
dotnet run --project src/Vosita.DbMigrator

or run :
dotnet ef database update --project src/Vosita.EntityFrameworkCore --startup-project src/Vosita.DbMigrator
```
## Assumptions Made

- Slots are generated for full days (00:00 to 23:59) in the selected time zone
- No working hours restrictions
- No authentication/authorization required per requirements
- Default of 20 slots shown in "next available" view

## Bonus Implemented

- Unit tests for slot generation and time zone conversion logic
- Book slot endpoint

## Submission Notes

- Backend and frontend are merged in this single repository 
- ABP.io framework used with Angular template
- NodaTime used correctly (Instants for storage, ZonedDateTime for display)
- Unit tests included
