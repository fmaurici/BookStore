#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CRUD3/CRUD3.csproj", "CRUD3/"]
COPY ["IRepositories/IRepositories.csproj", "IRepositories/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Database/Database.csproj", "Database/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]
RUN dotnet restore "CRUD3/CRUD3.csproj"
COPY . .
WORKDIR "/src/CRUD3"
RUN dotnet build "CRUD3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CRUD3.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRUD3.dll"]