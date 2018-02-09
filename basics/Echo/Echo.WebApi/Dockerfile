FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY Echo/Echo.WebApi/Echo.WebApi.csproj Echo/Echo.WebApi/
COPY ../external/BuildingBlocks/src/BuildingBlocks.Mediatr/BuildingBlocks.Mediatr.csproj ../external/BuildingBlocks/src/BuildingBlocks.Mediatr/
COPY ../external/BuildingBlocks/src/BuildingBlocks.Idempotency/BuildingBlocks.Idempotency.csproj ../external/BuildingBlocks/src/BuildingBlocks.Idempotency/
COPY ../external/BuildingBlocks/src/BuildingBlocks.Autofac/BuildingBlocks.Autofac.csproj ../external/BuildingBlocks/src/BuildingBlocks.Autofac/
COPY ../external/BuildingBlocks/src/BuildingBlocks.Mvc/BuildingBlocks.Mvc.csproj ../external/BuildingBlocks/src/BuildingBlocks.Mvc/
COPY ../external/BuildingBlocks/src/BuildingBlocks.Core/BuildingBlocks.Core.csproj ../external/BuildingBlocks/src/BuildingBlocks.Core/
COPY ../external/HealthChecks/src/Microsoft.Extensions.HealthChecks/Microsoft.Extensions.HealthChecks.csproj ../external/HealthChecks/src/Microsoft.Extensions.HealthChecks/
COPY ../external/HealthChecks/src/Microsoft.AspNetCore.HealthChecks/Microsoft.AspNetCore.HealthChecks.csproj ../external/HealthChecks/src/Microsoft.AspNetCore.HealthChecks/
COPY ../external/BuildingBlocks/src/BuildingBlocks.Swagger/BuildingBlocks.Swagger.csproj ../external/BuildingBlocks/src/BuildingBlocks.Swagger/
COPY Echo/Echo.Application/Echo.Application.csproj Echo/Echo.Application/
RUN dotnet restore
COPY . .
WORKDIR /src/Echo/Echo.WebApi
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Echo.WebApi.dll"]
