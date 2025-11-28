# CobbleDotNetScripting

A .NET 9.0 application that enables hot-reloading C# script execution using the Roslyn scripting API.

## Overview

CobbleDotNetScripting is a lightweight scripting host that:

1. **Executes C# scripts (.csx files)** - Runs C# script files using Microsoft's Roslyn scripting engine
2. **Watches for changes** - Automatically re-executes scripts when they are modified
3. **Provides standard imports** - Scripts have access to common namespaces like `System`, `System.IO`, `System.Collections.Generic`, `System.Linq`, and `System.Text`

## Project Structure

```
CobbleDotNetScripting/
├── CobbleDotNet/
│   ├── CobbleDotNet.csproj    # Project file targeting .NET 9.0
│   └── Program.cs             # Main application entry point
├── Scripts/
│   └── script.csx             # Example C# script file
├── CobbleDotNet.sln           # Solution file
└── README.md
```

## How It Works

1. The application loads and executes a C# script file (`Scripts/script.csx`)
2. A `FileSystemWatcher` monitors the script file for changes
3. When the script is modified, it automatically re-executes
4. Script compilation errors are caught and displayed

## Requirements

- .NET 9.0 SDK

## Usage

1. Build and run the application:
   ```bash
   dotnet build
   dotnet run --project CobbleDotNet
   ```

2. Edit `Scripts/script.csx` with your C# code
3. Save the file to see it automatically execute

## Example Script

```csharp
// Scripts/script.csx
Console.WriteLine("Hello from the script!");

var numbers = new[] { 1, 2, 3, 4, 5 };
var sum = numbers.Sum();
Console.WriteLine($"Sum: {sum}");
```

## Future?

```csharp
Events.subscribe(PayCalculatedEvent)
    .filter(e => e.Employee.Country == "NZ")
    .handler(e => {
        foreach (var day in e.Timesheet.Days())
        {
            var hours = e.Timesheet.Hours(day);
            var isHoliday  = e.Timesheet.Calendar.IsPublicHoliday(day);

            if (isHoliday)
            {
                e.Gross += hours * e.Employee.HourlyRate * 1.5m;
                e.Gross += 8 * e.Employee.HourlyRate;
            }
        }
    });

Events.subscribe(TimesheetImportedEvent)
    .filter(e => e.Employee.IsHourly)
    .handler(e => {
        foreach (var day in e.Timesheet.Days())
        {
            var hours = e.Timesheet.Hours(day);

            // Auto deduct 30-minute unpaid break for long shifts
            if (hours > 6)
                e.Timesheet.SetHours(day, hours - 0.5m);
        }
    });
```

## Dependencies

- [Microsoft.CodeAnalysis.CSharp.Scripting](https://www.nuget.org/packages/Microsoft.CodeAnalysis.CSharp.Scripting/) v5.0.0 - Roslyn C# scripting API
