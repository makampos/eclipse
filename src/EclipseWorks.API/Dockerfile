FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EclipseWorks.API/EclipseWorks.API.csproj", "EclipseWorks.API/"]
COPY ["EclipseWorks.Application/EclipseWorks.Application.csproj", "EclipseWorks.Application/"]
COPY ["EclipseWorks.Domain/EclipseWorks.Domain.csproj", "EclipseWorks.Domain/"]
COPY ["EclipseWorks.Infrastructure/EclipseWorks.Infrastructure.csproj", "EclipseWorks.Infrastructure/"]
RUN dotnet restore "./EclipseWorks.API/EclipseWorks.API.csproj"
COPY . .
WORKDIR "/src/EclipseWorks.API"
RUN dotnet build "./EclipseWorks.API.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EclipseWorks.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EclipseWorks.API.dll"]
