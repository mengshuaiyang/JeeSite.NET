FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Layer 1: project files only — cached until a .csproj changes
COPY JeeSiteNET.sln .
COPY src/JeeSiteNET.Core/JeeSiteNET.Core.csproj src/JeeSiteNET.Core/
COPY src/JeeSiteNET.Infrastructure/JeeSiteNET.Infrastructure.csproj src/JeeSiteNET.Infrastructure/
COPY src/JeeSiteNET.Web.Api/JeeSiteNET.Web.Api.csproj src/JeeSiteNET.Web.Api/
COPY src/JeeSiteNET.Web.Host/JeeSiteNET.Web.Host.csproj src/JeeSiteNET.Web.Host/
COPY modules/JeeSiteNET.Modules.App/JeeSiteNET.Modules.App.csproj modules/JeeSiteNET.Modules.App/
COPY modules/JeeSiteNET.Modules.Bpm/JeeSiteNET.Modules.Bpm.csproj modules/JeeSiteNET.Modules.Bpm/
COPY modules/JeeSiteNET.Modules.Cms/JeeSiteNET.Modules.Cms.csproj modules/JeeSiteNET.Modules.Cms/
COPY modules/JeeSiteNET.Modules.CodeGen/JeeSiteNET.Modules.CodeGen.csproj modules/JeeSiteNET.Modules.CodeGen/
COPY modules/JeeSiteNET.Modules.Sys/JeeSiteNET.Modules.Sys.csproj modules/JeeSiteNET.Modules.Sys/
COPY modules/JeeSiteNET.Modules.Tasks/JeeSiteNET.Modules.Tasks.csproj modules/JeeSiteNET.Modules.Tasks/
RUN dotnet restore src/JeeSiteNET.Web.Api/JeeSiteNET.Web.Api.csproj

# Layer 2: full source + publish
COPY . .
RUN dotnet publish src/JeeSiteNET.Web.Api/JeeSiteNET.Web.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app .
ENTRYPOINT ["dotnet", "JeeSiteNET.Web.Api.dll"]
