FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["src/Admin/CinemaBackend.Admin.Api/CinemaBackend.Admin.Api.csproj", "src/Admin/CinemaBackend.Admin.Api/"]
COPY ["src/Shared/CinemaBackend.Communication/CinemaBackend.Communication.csproj", "src/Shared/CinemaBackend.Communication/"]
COPY ["src/Shared/CinemaBackend.SharedKernel/CinemaBackend.SharedKernel.csproj", "src/Shared/CinemaBackend.SharedKernel/"]
COPY ["src/Admin/CinemaBackend.Admin.Application/CinemaBackend.Admin.Application.csproj", "src/Admin/CinemaBackend.Admin.Application/"]
COPY ["src/Shared/CinemaBackend.Infrastructure/CinemaBackend.Infrastructure.csproj", "src/Shared/CinemaBackend.Infrastructure/"]
COPY ["src/Shared/CinemaBackend.Domain/CinemaBackend.Domain.csproj", "src/Shared/CinemaBackend.Domain/"]
RUN dotnet restore "src/Admin/CinemaBackend.Admin.Api/CinemaBackend.Admin.Api.csproj"
COPY . .
WORKDIR "/src/src/Admin/CinemaBackend.Admin.Api"
RUN dotnet build "./CinemaBackend.Admin.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CinemaBackend.Admin.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CinemaBackend.Admin.Api.dll"]
