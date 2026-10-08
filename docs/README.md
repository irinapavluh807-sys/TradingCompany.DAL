# TradingCompany DAL

This folder contains the database artifacts required for the warehouse manager variant.

- `database-diagram.svg` is the image diagram with all tables, columns, types and relationships.
- `database-diagram.mmd` is the Mermaid ER source for the same schema.

The executable project creates a SQL Server LocalDB database from embedded SQL scripts:

```powershell
dotnet run --project .\TradingCompany.ConsoleDemo\TradingCompany.ConsoleDemo.csproj
```

Default connection string:

```text
Server=(localdb)\MSSQLLocalDB;Initial Catalog=TradingCompanyDalDemo;Integrated Security=True;TrustServerCertificate=True;
```

To use another SQL Server instance, set `TRADING_COMPANY_SQLSERVER` before running the console app.

Run all DAL tests with:

```powershell
dotnet test .\TradingCompany.DAL.sln
```
