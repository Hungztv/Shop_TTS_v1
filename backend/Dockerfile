# Giai đoạn 1: Build với SDK .NET 10
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy file solution và project
# Lưu ý: Giữ nguyên tên thư mục "Infrastucture" (thiếu chữ r) như code cũ của bạn để tránh lỗi file not found
COPY ["ShopxBase.slnx", "./"]
COPY ["ShopxBase.Api/ShopxBase.Api.csproj", "ShopxBase.Api/"]
COPY ["ShopxBase.Application/ShopxBase.Application.csproj", "ShopxBase.Application/"]
COPY ["ShopxBase.Domain/ShopxBase.Domain.csproj", "ShopxBase.Domain/"]
COPY ["ShopxBase.Infrastucture/ShopxBase.Infrastructure.csproj", "ShopxBase.Infrastucture/"]

# Restore
RUN dotnet restore "ShopxBase.Api/ShopxBase.Api.csproj"

# Copy toàn bộ code
COPY . .

# Build
WORKDIR "/src/ShopxBase.Api"
RUN dotnet publish "ShopxBase.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Giai đoạn 2: Run với Runtime .NET 10
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Cấu hình Port cho Render (Render sẽ map port này ra ngoài)
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ShopxBase.Api.dll"]