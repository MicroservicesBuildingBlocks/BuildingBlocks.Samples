FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY Identity/Identity.Web/Identity.Web.csproj Identity/Identity.Web/
COPY ../external/BuildingBlocks/src/BuildingBlocks.Mediatr/BuildingBlocks.Mediatr.csproj ../external/BuildingBlocks/src/BuildingBlocks.Mediatr/
COPY ../external/BuildingBlocks/src/BuildingBlocks.Idempotency/BuildingBlocks.Idempotency.csproj ../external/BuildingBlocks/src/BuildingBlocks.Idempotency/
COPY ../external/BuildingBlocks/src/BuildingBlocks.Autofac/BuildingBlocks.Autofac.csproj ../external/BuildingBlocks/src/BuildingBlocks.Autofac/
COPY Identity/Identity.Application/Identity.Application.csproj Identity/Identity.Application/
COPY ../external/BuildingBlocks/src/BuildingBlocks.AspnetCoreIdentity.RavenDB/BuildingBlocks.AspnetCoreIdentity.RavenDB.csproj ../external/BuildingBlocks/src/BuildingBlocks.AspnetCoreIdentity.RavenDB/
COPY ../external/BuildingBlocks/src/BuildingBlocks.IdentityServer4.RavenDB/BuildingBlocks.IdentityServer4.RavenDB.csproj ../external/BuildingBlocks/src/BuildingBlocks.IdentityServer4.RavenDB/
COPY ../external/BuildingBlocks/src/BuildingBlocks.Mvc/BuildingBlocks.Mvc.csproj ../external/BuildingBlocks/src/BuildingBlocks.Mvc/
COPY ../external/BuildingBlocks/src/BuildingBlocks.Core/BuildingBlocks.Core.csproj ../external/BuildingBlocks/src/BuildingBlocks.Core/
COPY ../external/HealthChecks/src/Microsoft.Extensions.HealthChecks/Microsoft.Extensions.HealthChecks.csproj ../external/HealthChecks/src/Microsoft.Extensions.HealthChecks/
COPY ../external/HealthChecks/src/Microsoft.AspNetCore.HealthChecks/Microsoft.AspNetCore.HealthChecks.csproj ../external/HealthChecks/src/Microsoft.AspNetCore.HealthChecks/
RUN dotnet restore
COPY . .
WORKDIR /src/Identity/Identity.Web
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Identity.Web.dll"]
