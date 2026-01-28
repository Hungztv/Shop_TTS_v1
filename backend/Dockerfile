# Giai đoạn 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy file solution và các file project để restore trước (tận dụng cache)
COPY ["ShopxBase.slnx", "./"]
COPY ["ShopxBase.Api/ShopxBase.Api.csproj", "ShopxBase.Api/"]
COPY ["ShopxBase.Application/ShopxBase.Application.csproj", "ShopxBase.Application/"]
COPY ["ShopxBase.Domain/ShopxBase.Domain.csproj", "ShopxBase.Domain/"]
COPY ["ShopxBase.Infrastucture/ShopxBase.Infrastructure.csproj", "ShopxBase.Infrastucture/"]

# Chạy restore các thư viện
RUN dotnet restore "ShopxBase.slnx"

# Copy toàn bộ code còn lại vào
COPY . .

# Build và Publish bản Release
WORKDIR "/src/ShopxBase.Api"
RUN dotnet publish "ShopxBase.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Giai đoạn 2: Run (Dùng ảnh nhẹ hơn để chạy)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ShopxBase.Api.dll"]