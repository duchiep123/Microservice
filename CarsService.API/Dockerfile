#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CarsService.API/CarsService.API.csproj", "CarsService.API/"]
RUN dotnet restore "CarsService.API/CarsService.API.csproj"
COPY . .
WORKDIR "/src/CarsService.API"
RUN dotnet build "CarsService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarsService.API.csproj" -c Release -o /app/publish
COPY ["CarsService.API/startdb.sh", "CarsService.API/"]

FROM build AS publish
RUN dotnet tool install --global dotnet-ef
RUN chmod +x ./startdb.sh
CMD /bin/bash ./startdb.sh
