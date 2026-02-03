# Stage 1: Build với SDK .NET 9 (stable)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy file solution và project files
COPY ["Shopping.slnx", "./"]
COPY ["ShopxBase.Api/ShopxBase.Api.csproj", "ShopxBase.Api/"]
COPY ["ShopxBase.Application/ShopxBase.Application.csproj", "ShopxBase.Application/"]
COPY ["ShopxBase.Domain/ShopxBase.Domain.csproj", "ShopxBase.Domain/"]
COPY ["ShopxBase.Infrastucture/ShopxBase.Infrastructure.csproj", "ShopxBase.Infrastucture/"]

# Restore dependencies
RUN dotnet restore "ShopxBase.Api/ShopxBase.Api.csproj"

# Copy toàn bộ source code
COPY . .

# Build và publish
WORKDIR "/src/ShopxBase.Api"
RUN dotnet publish "ShopxBase.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime với ASP.NET 9
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Render.com sử dụng port 10000 mặc định
ENV ASPNETCORE_HTTP_PORTS=10000
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 10000

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ShopxBase.Api.dll"]