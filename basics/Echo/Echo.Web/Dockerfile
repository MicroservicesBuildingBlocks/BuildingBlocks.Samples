FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY Echo/Echo.Web/Echo.Web.csproj Echo/Echo.Web/
COPY ../external/BuildingBlocks/src/BuildingBlocks.Mvc/BuildingBlocks.Mvc.csproj ../external/BuildingBlocks/src/BuildingBlocks.Mvc/
COPY ../external/BuildingBlocks/src/BuildingBlocks.Core/BuildingBlocks.Core.csproj ../external/BuildingBlocks/src/BuildingBlocks.Core/
COPY ../external/HealthChecks/src/Microsoft.Extensions.HealthChecks/Microsoft.Extensions.HealthChecks.csproj ../external/HealthChecks/src/Microsoft.Extensions.HealthChecks/
COPY ../external/HealthChecks/src/Microsoft.AspNetCore.HealthChecks/Microsoft.AspNetCore.HealthChecks.csproj ../external/HealthChecks/src/Microsoft.AspNetCore.HealthChecks/
COPY ../external/BuildingBlocks/src/BuindingBlocks.Resilience/BuindingBlocks.Resilience.csproj ../external/BuildingBlocks/src/BuindingBlocks.Resilience/
RUN dotnet restore
COPY . .
WORKDIR /src/Echo/Echo.Web
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Echo.Web.dll"]
