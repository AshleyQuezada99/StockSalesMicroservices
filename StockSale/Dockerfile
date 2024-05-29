#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StockSale/Stock.csproj", "StockSale/"]
COPY ["StockSale.RabbitMQ.Bus/StockSale.RabbitMQ.Bus.csproj", "StockSale.RabbitMQ.Bus/"]
RUN dotnet restore "./StockSale/Stock.csproj"
COPY . .
WORKDIR "/src/StockSale"
RUN dotnet build "./Stock.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Stock.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stock.dll"]