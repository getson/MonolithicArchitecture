#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/App.WebApi/App.WebApi.csproj", "src/App.WebApi/"]
COPY ["src/App.Application.Commands/App.Application.Commands.csproj", "src/App.Application.Commands/"]
COPY ["src/App.Core/App.Core.csproj", "src/App.Core/"]
COPY ["src/SeedWork/BinaryOrigin.SeedWork.Core/BinaryOrigin.SeedWork.Core.csproj", "src/SeedWork/BinaryOrigin.SeedWork.Core/"]
COPY ["src/SeedWork/BinaryOrigin.SeedWork.Messages/BinaryOrigin.SeedWork.Messages.csproj", "src/SeedWork/BinaryOrigin.SeedWork.Messages/"]
COPY ["src/SeedWork/BinaryOrigin.SeedWork.WebApi/BinaryOrigin.SeedWork.WebApi.csproj", "src/SeedWork/BinaryOrigin.SeedWork.WebApi/"]
COPY ["src/SeedWork/BinaryOrigin.SeedWork.Infrastructure/BinaryOrigin.SeedWork.Infrastructure.csproj", "src/SeedWork/BinaryOrigin.SeedWork.Infrastructure/"]
COPY ["src/SeedWork/BinaryOrigin.SeedWork.Persistence/BinaryOrigin.SeedWork.Persistence.Ef.csproj", "src/SeedWork/BinaryOrigin.SeedWork.Persistence/"]
COPY ["src/App.Application.Events/App.Application.Events.csproj", "src/App.Application.Events/"]
COPY ["src/App.Infrastructure.Persistence/App.Infrastructure.Persistence.csproj", "src/App.Infrastructure.Persistence/"]
COPY ["src/SeedWork/BinaryOrigin.SeedWork.Persistence.PostgreSql/BinaryOrigin.SeedWork.Persistence.PostgreSql.csproj", "src/SeedWork/BinaryOrigin.SeedWork.Persistence.PostgreSql/"]
COPY ["src/App.Application.Queries/App.Application.Queries.csproj", "src/App.Application.Queries/"]
RUN dotnet restore "src/App.WebApi/App.WebApi.csproj"
COPY . .
WORKDIR "/src/src/App.WebApi"
RUN dotnet build "App.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.WebApi.dll"]