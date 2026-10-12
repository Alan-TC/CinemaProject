FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["src/Client/CinemaBackend.Client.Api/CinemaBackend.Client.Api.csproj", "src/Client/CinemaBackend.Client.Api/"]
COPY ["src/Client/CinemaBackend.Client.Application/CinemaBackend.Client.Application.csproj", "src/Client/CinemaBackend.Client.Application/"]
COPY ["src/Shared/CinemaBackend.Communication/CinemaBackend.Communication.csproj", "src/Shared/CinemaBackend.Communication/"]
COPY ["src/Shared/CinemaBackend.Domain/CinemaBackend.Domain.csproj", "src/Shared/CinemaBackend.Domain/"]
COPY ["src/Shared/CinemaBackend.SharedKernel/CinemaBackend.SharedKernel.csproj", "src/Shared/CinemaBackend.SharedKernel/"]
COPY ["src/Shared/CinemaBackend.Infrastructure/CinemaBackend.Infrastructure.csproj", "src/Shared/CinemaBackend.Infrastructure/"]
RUN dotnet restore "src/Client/CinemaBackend.Client.Api/CinemaBackend.Client.Api.csproj"
COPY . .
WORKDIR "/src/src/Client/CinemaBackend.Client.Api"
RUN dotnet build "./CinemaBackend.Client.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CinemaBackend.Client.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CinemaBackend.Client.Api.dll"]
