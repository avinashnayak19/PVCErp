# PVC ERP

Angular 19 + .NET 8 + SQL Server 2019 ERP scaffold for PVC pipe manufacturing.

## Architecture

- `PVCErp.Domain`: entities, enums, and business model.
- `PVCErp.Application`: DTOs, service contracts, use-case services, repository abstractions.
- `PVCErp.Infrastructure`: EF Core SQL Server `DbContext`, configurations, generic repository, unit of work.
- `PVCErp.Api`: REST API controllers and dependency wiring.
- `frontend`: Angular 19 standalone app.

## Main Modules

- Inventory and GRN
- Supplier-wise and batch-wise stock
- Batch formula and raw material consumption
- Pipe production and socketing
- Stage-wise scrap and reuse tracking
- Screw barrel efficiency
- Dispatch challan and invoice linkage
- Dashboard reports

## Run

Backend:

```powershell
dotnet restore .\PVCErp.sln --configfile .\NuGet.config
dotnet ef database update --project .\src\PVCErp.Infrastructure --startup-project .\src\PVCErp.Api
dotnet run --project .\src\PVCErp.Api
```

Frontend:

```powershell
cd .\frontend
npm install
npm start
```
