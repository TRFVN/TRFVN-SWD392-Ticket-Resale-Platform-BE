#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ticket_Hub.API/Ticket_Hub.API.csproj", "Ticket_Hub.API/"]
COPY ["Ticket_Hub.DataAccess/Ticket_Hub.DataAccess.csproj", "Ticket_Hub.DataAccess/"]
COPY ["Ticket_Hub.Models/Ticket_Hub.Models.csproj", "Ticket_Hub.Models/"]
COPY ["Ticket_Hub.Utility/Ticket_Hub.Utility.csproj", "Ticket_Hub.Utility/"]
COPY ["Ticket_Hub.Services/Ticket_Hub.Services.csproj", "Ticket_Hub.Services/"]
RUN dotnet restore "./Ticket_Hub.API/Ticket_Hub.API.csproj"
COPY . .
WORKDIR "/src/Ticket_Hub.API"
RUN dotnet build "./Ticket_Hub.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ticket_Hub.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ticket_Hub.API.dll"]