# Use the ASP.NET runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80


# Use the .NET SDK as build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["Workhub.Api/Workhub.Api.csproj", "Workhub.Api/"]
COPY ["Workhub.Application/Workhub.Application.csproj", "Workhub.Application/"]
COPY ["Workhub.Contracts/Workhub.Contracts.csproj", "Workhub.Contracts/"]
COPY ["Workhub.Domain/Workhub.Domain.csproj", "Workhub.Domain/"]
COPY ["Workhub.Infrastructure/Workhub.Infrastructure.csproj", "Workhub.Infrastructure/"]
RUN dotnet restore "Workhub.Api/Workhub.Api.csproj"

# Copy all other source code and build the project
COPY . .
WORKDIR "/src/Workhub.Api"
RUN dotnet build "Workhub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Workhub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish 

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Workhub.Api.dll"]
