# Common. Logging Project

## Setup

```shell
dotnet new classlib -n Common.Logging

dotnet add package OpenTelemetry.Extensions.Hosting
dotnet add package OpenTelemetry.Instrumentation.AspNetCore 
dotnet add package OpenTelemetry.Instrumentation.Http
dotnet add package OpenTelemetry.Exporter.Console

dotnet add package Serilog
dotnet add package Serilog.Enrichers.Environment 
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.Debug
dotnet add package Serilog.Exceptions
dotnet add package Serilog.Enrichers.Thread

dotnet add package Serilog.Enrichers.Process

dotnet add package Serilog.Enrichers.AspNetCore.HttpContext
dotnet add package Serilog.AspNetCore

dotnet add package Serilog.Sinks.Seq

dotnet add package Serilog.Sinks.Elasticsearch
```
