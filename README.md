src/APBD_Task10.API/appsettings.json template
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DatabaseConnection": "--your connection string--"
  }
}
```

# Reason of splitting code
1. Easier to maintain
2. Abides SOLID principles
3. Easier to replace database
4. Better structure, easier to navigate
5. Low coupling
6. Clear layers of logic and code
