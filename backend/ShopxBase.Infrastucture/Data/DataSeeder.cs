using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Enums;

namespace ShopxBase.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedSampleDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ShopxBaseDbContext>();

        // Check nếu đã có đủ dữ liệu thì skip
        var productCount = await context.Set<Product>().CountAsync(p => !p.IsDeleted);
        if (productCount >= 30)
        {
            Console.WriteLine($"⏭️ Đã có {productCount} sản phẩm, bỏ qua seed.");
            return;
        }

        Console.WriteLine("🌱 Bắt đầu seed dữ liệu mẫu...");

        // ========================
        // 1. CATEGORIES (chỉ thêm cái chưa có)
        // ========================
        var existingSlugs = await context.Set<Category>()
            .Where(c => !c.IsDeleted)
            .Select(c => c.Slug)
            .ToListAsync();

        var categories = new List<Category>
        {
            new() { Name = "Điện thoại", Description = "Smartphone và điện thoại di động các hãng", Slug = "dien-thoai", Status = "active" },
            new() { Name = "Laptop", Description = "Laptop gaming, văn phòng, đồ họa chính hãng", Slug = "laptop", Status = "active" },
            new() { Name = "Máy tính bảng", Description = "Tablet iPad và Android các loại", Slug = "may-tinh-bang", Status = "active" },
            new() { Name = "Tai nghe", Description = "Tai nghe không dây, có dây, chống ồn", Slug = "tai-nghe", Status = "active" },
            new() { Name = "Đồng hồ thông minh", Description = "Smartwatch và vòng tay thông minh", Slug = "dong-ho-thong-minh", Status = "active" },
            new() { Name = "Phụ kiện", Description = "Ốp lưng, sạc, cáp và phụ kiện công nghệ", Slug = "phu-kien", Status = "active" },
            new() { Name = "Tivi", Description = "Smart TV, TV 4K, OLED các kích thước", Slug = "tivi", Status = "active" },
            new() { Name = "Loa & Âm thanh", Description = "Loa bluetooth, soundbar, hệ thống âm thanh", Slug = "loa-am-thanh", Status = "active" },
            new() { Name = "Máy ảnh", Description = "Máy ảnh DSLR, Mirrorless và phụ kiện", Slug = "may-anh", Status = "active" },
            new() { Name = "Gaming", Description = "Bàn phím, chuột, ghế gaming và phụ kiện", Slug = "gaming", Status = "active" },
            new() { Name = "Thiết bị mạng", Description = "Router, modem, thiết bị phát WiFi", Slug = "thiet-bi-mang", Status = "active" },
            new() { Name = "Gia dụng thông minh", Description = "Robot hút bụi, máy lọc không khí, thiết bị smart home", Slug = "gia-dung-thong-minh", Status = "active" },
        };

        var newCategories = categories.Where(c => !existingSlugs.Contains(c.Slug)).ToList();
        if (newCategories.Any())
        {
            context.Set<Category>().AddRange(newCategories);
            await context.SaveChangesAsync();
        }
        Console.WriteLine($"✅ Danh mục: {newCategories.Count} mới, {existingSlugs.Count} đã có");

        // ========================
        // 2. BRANDS (chỉ thêm cái chưa có)
        // ========================
        var existingBrandSlugs = await context.Set<Brand>()
            .Where(b => !b.IsDeleted)
            .Select(b => b.Slug)
            .ToListAsync();

        var brands = new List<Brand>
        {
            new() { Name = "Apple", Description = "Thương hiệu công nghệ hàng đầu thế giới từ Mỹ", Slug = "apple", Status = "active", Logo = "" },
            new() { Name = "Samsung", Description = "Tập đoàn công nghệ đa quốc gia Hàn Quốc", Slug = "samsung", Status = "active", Logo = "" },
            new() { Name = "Xiaomi", Description = "Thương hiệu công nghệ giá tốt từ Trung Quốc", Slug = "xiaomi", Status = "active", Logo = "" },
            new() { Name = "Sony", Description = "Thương hiệu điện tử và giải trí Nhật Bản", Slug = "sony", Status = "active", Logo = "" },
            new() { Name = "Dell", Description = "Hãng máy tính và laptop hàng đầu", Slug = "dell", Status = "active", Logo = "" },
            new() { Name = "ASUS", Description = "Thương hiệu laptop và linh kiện gaming", Slug = "asus", Status = "active", Logo = "" },
            new() { Name = "Lenovo", Description = "Hãng laptop và máy tính cá nhân toàn cầu", Slug = "lenovo", Status = "active", Logo = "" },
            new() { Name = "JBL", Description = "Thương hiệu loa và tai nghe chất lượng cao", Slug = "jbl", Status = "active", Logo = "" },
            new() { Name = "Logitech", Description = "Thương hiệu phụ kiện gaming và văn phòng", Slug = "logitech", Status = "active", Logo = "" },
            new() { Name = "Anker", Description = "Thương hiệu sạc và phụ kiện công nghệ", Slug = "anker", Status = "active", Logo = "" },
            new() { Name = "Oppo", Description = "Thương hiệu smartphone tầm trung phổ biến", Slug = "oppo", Status = "active", Logo = "" },
            new() { Name = "Huawei", Description = "Tập đoàn viễn thông và công nghệ Trung Quốc", Slug = "huawei", Status = "active", Logo = "" },
            new() { Name = "LG", Description = "Thương hiệu điện tử và gia dụng Hàn Quốc", Slug = "lg-electronics", Status = "active", Logo = "" },
            new() { Name = "HP", Description = "Hãng máy tính và máy in hàng đầu thế giới", Slug = "hp", Status = "active", Logo = "" },
            new() { Name = "Razer", Description = "Thương hiệu gaming gear cao cấp", Slug = "razer", Status = "active", Logo = "" },
        };

        var newBrands = brands.Where(b => !existingBrandSlugs.Contains(b.Slug)).ToList();
        if (newBrands.Any())
        {
            context.Set<Brand>().AddRange(newBrands);
            await context.SaveChangesAsync();
        }
        Console.WriteLine($"✅ Thương hiệu: {newBrands.Count} mới, {existingBrandSlugs.Count} đã có");

        // ========================
        // 3. SYSTEM SHOP (nếu chưa có shop nào)
        // ========================
        var shop = await context.Set<Shop>().FirstOrDefaultAsync(s => !s.IsDeleted);
        if (shop == null)
        {
            // Phải tạo BusinessRegistration trước (FK bắt buộc)
            var bizReg = new BusinessRegistration
            {
                UserId = "",
                CompanyName = "ShopTTS Official Store",
                TaxCode = "0000000000",
                OwnerName = "System Admin",
                Email = "admin@shopxbase.com",
                Phone = "0900000000",
                Address = "Hệ thống ShopTTS",
                Status = BusinessRegistrationStatus.Approved,
                ReviewedAt = DateTime.UtcNow,
                ReviewedBy = "system",
            };
            context.Set<BusinessRegistration>().Add(bizReg);
            await context.SaveChangesAsync();

            shop = new Shop
            {
                Name = "ShopTTS Official",
                Slug = "shoptts-official",
                Description = "Cửa hàng chính thức của hệ thống ShopTTS",
                Status = ShopStatus.Active,
                OwnerUserId = "",
                BusinessRegistrationId = bizReg.Id,
            };
            context.Set<Shop>().Add(shop);
            await context.SaveChangesAsync();
            Console.WriteLine("✅ Đã tạo shop hệ thống");
        }

        var shopId = shop.Id;

        // Reload category & brand IDs
        var catDict = await context.Set<Category>()
            .Where(c => !c.IsDeleted)
            .ToDictionaryAsync(c => c.Slug, c => c.Id);

        var brandDict = await context.Set<Brand>()
            .Where(b => !b.IsDeleted)
            .ToDictionaryAsync(b => b.Slug, b => b.Id);

        // ========================
        // 4. PRODUCTS
        // ========================
        var products = new List<Product>
        {
            // === ĐIỆN THOẠI ===
            new() { Name = "iPhone 16 Pro Max 256GB", Slug = "iphone-16-pro-max-256gb", Description = "iPhone 16 Pro Max với chip A18 Pro, camera 48MP, màn hình Super Retina XDR 6.9 inch, pin cả ngày dài.", Price = 34990000, CapitalPrice = 32000000, Quantity = 50, Image = "https://placehold.co/600x600/7c3aed/white?text=iPhone+16+Pro", CategoryId = catDict["dien-thoai"], BrandId = brandDict["apple"], ShopId = shopId },
            new() { Name = "iPhone 16 128GB", Slug = "iphone-16-128gb", Description = "iPhone 16 với Dynamic Island, chip A18, camera 48MP tiên tiến, thiết kế mới với nút Action.", Price = 24990000, CapitalPrice = 22500000, Quantity = 80, Image = "https://placehold.co/600x600/6366f1/white?text=iPhone+16", CategoryId = catDict["dien-thoai"], BrandId = brandDict["apple"], ShopId = shopId },
            new() { Name = "Samsung Galaxy S24 Ultra", Slug = "samsung-galaxy-s24-ultra", Description = "Galaxy S24 Ultra với bút S-Pen, chip Snapdragon 8 Gen 3, camera 200MP, Galaxy AI tích hợp.", Price = 31990000, CapitalPrice = 28500000, Quantity = 40, Image = "https://placehold.co/600x600/1e40af/white?text=Galaxy+S24+Ultra", CategoryId = catDict["dien-thoai"], BrandId = brandDict["samsung"], ShopId = shopId },
            new() { Name = "Samsung Galaxy A55 5G", Slug = "samsung-galaxy-a55-5g", Description = "Galaxy A55 5G thiết kế cao cấp, chip Exynos 1480, camera OIS 50MP, pin 5000mAh.", Price = 9990000, CapitalPrice = 8500000, Quantity = 100, Image = "https://placehold.co/600x600/2563eb/white?text=Galaxy+A55", CategoryId = catDict["dien-thoai"], BrandId = brandDict["samsung"], ShopId = shopId },
            new() { Name = "Xiaomi 14 Ultra", Slug = "xiaomi-14-ultra", Description = "Xiaomi 14 Ultra camera Leica chuyên nghiệp, Snapdragon 8 Gen 3, sạc nhanh 90W.", Price = 23990000, CapitalPrice = 21000000, Quantity = 30, Image = "https://placehold.co/600x600/dc2626/white?text=Xiaomi+14+Ultra", CategoryId = catDict["dien-thoai"], BrandId = brandDict["xiaomi"], ShopId = shopId },
            new() { Name = "Xiaomi Redmi Note 13 Pro", Slug = "xiaomi-redmi-note-13-pro", Description = "Redmi Note 13 Pro camera 200MP, màn AMOLED 120Hz, sạc nhanh 67W, giá cực tốt.", Price = 7490000, CapitalPrice = 6200000, Quantity = 120, Image = "https://placehold.co/600x600/ef4444/white?text=Redmi+Note+13", CategoryId = catDict["dien-thoai"], BrandId = brandDict["xiaomi"], ShopId = shopId },
            new() { Name = "Oppo Find X7 Ultra", Slug = "oppo-find-x7-ultra", Description = "Find X7 Ultra hợp tác Hasselblad, camera periscope kép, Dimensity 9300.", Price = 22990000, CapitalPrice = 20000000, Quantity = 25, Image = "https://placehold.co/600x600/16a34a/white?text=Find+X7+Ultra", CategoryId = catDict["dien-thoai"], BrandId = brandDict["oppo"], ShopId = shopId },
            new() { Name = "Oppo Reno 11 5G", Slug = "oppo-reno-11-5g", Description = "Reno 11 5G thiết kế thời trang, camera AI 50MP, màn AMOLED 120Hz cong.", Price = 10990000, CapitalPrice = 9500000, Quantity = 60, Image = "https://placehold.co/600x600/22c55e/white?text=Reno+11", CategoryId = catDict["dien-thoai"], BrandId = brandDict["oppo"], ShopId = shopId },

            // === LAPTOP ===
            new() { Name = "MacBook Air M3 13 inch", Slug = "macbook-air-m3-13", Description = "MacBook Air M3 siêu mỏng nhẹ, chip M3 mạnh mẽ, pin 18 giờ, màn Liquid Retina.", Price = 27990000, CapitalPrice = 25000000, Quantity = 35, Image = "https://placehold.co/600x600/a855f7/white?text=MacBook+Air+M3", CategoryId = catDict["laptop"], BrandId = brandDict["apple"], ShopId = shopId },
            new() { Name = "MacBook Pro 14 M3 Pro", Slug = "macbook-pro-14-m3-pro", Description = "MacBook Pro 14 inch với chip M3 Pro, hiệu năng đỉnh cao cho chuyên gia sáng tạo.", Price = 49990000, CapitalPrice = 45000000, Quantity = 20, Image = "https://placehold.co/600x600/9333ea/white?text=MacBook+Pro+14", CategoryId = catDict["laptop"], BrandId = brandDict["apple"], ShopId = shopId },
            new() { Name = "Dell XPS 15", Slug = "dell-xps-15", Description = "Dell XPS 15 màn OLED 3.5K, Intel Core i7-13700H, RAM 16GB, thiết kế cao cấp.", Price = 39990000, CapitalPrice = 36000000, Quantity = 15, Image = "https://placehold.co/600x600/0ea5e9/white?text=Dell+XPS+15", CategoryId = catDict["laptop"], BrandId = brandDict["dell"], ShopId = shopId },
            new() { Name = "Dell Inspiron 15 3530", Slug = "dell-inspiron-15-3530", Description = "Dell Inspiron 15 Intel Core i5, RAM 8GB, SSD 512GB, laptop văn phòng bền bỉ.", Price = 15990000, CapitalPrice = 13500000, Quantity = 45, Image = "https://placehold.co/600x600/38bdf8/white?text=Dell+Inspiron+15", CategoryId = catDict["laptop"], BrandId = brandDict["dell"], ShopId = shopId },
            new() { Name = "ASUS ROG Strix G16", Slug = "asus-rog-strix-g16", Description = "ROG Strix G16 gaming laptop, RTX 4060, Intel i7-13650HX, màn 165Hz, RGB keyboard.", Price = 35990000, CapitalPrice = 32000000, Quantity = 20, Image = "https://placehold.co/600x600/f43f5e/white?text=ROG+Strix+G16", CategoryId = catDict["laptop"], BrandId = brandDict["asus"], ShopId = shopId },
            new() { Name = "ASUS Vivobook 15", Slug = "asus-vivobook-15", Description = "Vivobook 15 mỏng nhẹ, Ryzen 5 7530U, SSD 512GB, laptop sinh viên giá tốt.", Price = 13490000, CapitalPrice = 11500000, Quantity = 60, Image = "https://placehold.co/600x600/fb923c/white?text=Vivobook+15", CategoryId = catDict["laptop"], BrandId = brandDict["asus"], ShopId = shopId },
            new() { Name = "Lenovo ThinkPad X1 Carbon Gen 11", Slug = "lenovo-thinkpad-x1-carbon-gen11", Description = "ThinkPad X1 Carbon siêu nhẹ 1.12kg, Intel vPro, bảo mật doanh nghiệp.", Price = 42990000, CapitalPrice = 39000000, Quantity = 10, Image = "https://placehold.co/600x600/64748b/white?text=ThinkPad+X1", CategoryId = catDict["laptop"], BrandId = brandDict["lenovo"], ShopId = shopId },
            new() { Name = "Lenovo IdeaPad Slim 5", Slug = "lenovo-ideapad-slim-5", Description = "IdeaPad Slim 5 Ryzen 7 7730U, 16GB RAM, màn 2.8K OLED, mỏng nhẹ.", Price = 18990000, CapitalPrice = 16500000, Quantity = 40, Image = "https://placehold.co/600x600/94a3b8/white?text=IdeaPad+Slim+5", CategoryId = catDict["laptop"], BrandId = brandDict["lenovo"], ShopId = shopId },
            new() { Name = "HP Pavilion 15", Slug = "hp-pavilion-15", Description = "HP Pavilion 15 Intel i5-1335U, 8GB RAM, 512GB SSD, thiết kế hiện đại.", Price = 16490000, CapitalPrice = 14000000, Quantity = 35, Image = "https://placehold.co/600x600/06b6d4/white?text=HP+Pavilion+15", CategoryId = catDict["laptop"], BrandId = brandDict["hp"], ShopId = shopId },

            // === MÁY TÍNH BẢNG ===
            new() { Name = "iPad Air M2 11 inch", Slug = "ipad-air-m2-11", Description = "iPad Air M2 chip cực mạnh, màn Liquid Retina 11 inch, hỗ trợ Apple Pencil Pro.", Price = 16990000, CapitalPrice = 15000000, Quantity = 40, Image = "https://placehold.co/600x600/c084fc/white?text=iPad+Air+M2", CategoryId = catDict["may-tinh-bang"], BrandId = brandDict["apple"], ShopId = shopId },
            new() { Name = "iPad Pro M4 13 inch", Slug = "ipad-pro-m4-13", Description = "iPad Pro M4 màn Ultra Retina XDR tandem OLED, mỏng nhất từ trước đến nay.", Price = 35990000, CapitalPrice = 32000000, Quantity = 15, Image = "https://placehold.co/600x600/a78bfa/white?text=iPad+Pro+M4", CategoryId = catDict["may-tinh-bang"], BrandId = brandDict["apple"], ShopId = shopId },
            new() { Name = "Samsung Galaxy Tab S9 FE", Slug = "samsung-galaxy-tab-s9-fe", Description = "Galaxy Tab S9 FE kèm S-Pen, màn 10.9 inch, chống nước IP68, chip Exynos 1380.", Price = 10490000, CapitalPrice = 9000000, Quantity = 50, Image = "https://placehold.co/600x600/3b82f6/white?text=Tab+S9+FE", CategoryId = catDict["may-tinh-bang"], BrandId = brandDict["samsung"], ShopId = shopId },
            new() { Name = "Xiaomi Pad 6", Slug = "xiaomi-pad-6", Description = "Xiaomi Pad 6 màn 11 inch 144Hz, Snapdragon 870, loa quad, giá tốt nhất phân khúc.", Price = 7990000, CapitalPrice = 6800000, Quantity = 55, Image = "https://placehold.co/600x600/f87171/white?text=Xiaomi+Pad+6", CategoryId = catDict["may-tinh-bang"], BrandId = brandDict["xiaomi"], ShopId = shopId },

            // === TAI NGHE ===
            new() { Name = "AirPods Pro 2 USB-C", Slug = "airpods-pro-2-usb-c", Description = "AirPods Pro 2 chống ồn chủ động, Adaptive Audio, chip H2, cổng USB-C.", Price = 5990000, CapitalPrice = 5200000, Quantity = 80, Image = "https://placehold.co/600x600/e879f9/white?text=AirPods+Pro+2", CategoryId = catDict["tai-nghe"], BrandId = brandDict["apple"], ShopId = shopId },
            new() { Name = "Sony WH-1000XM5", Slug = "sony-wh-1000xm5", Description = "Sony WH-1000XM5 chống ồn tốt nhất thế giới, LDAC, pin 30 giờ, thiết kế mới.", Price = 7490000, CapitalPrice = 6500000, Quantity = 35, Image = "https://placehold.co/600x600/818cf8/white?text=Sony+WH1000XM5", CategoryId = catDict["tai-nghe"], BrandId = brandDict["sony"], ShopId = shopId },
            new() { Name = "Samsung Galaxy Buds3 Pro", Slug = "samsung-galaxy-buds3-pro", Description = "Galaxy Buds3 Pro chống ồn ANC, 360 Audio, thiết kế trong suốt mới.", Price = 4990000, CapitalPrice = 4200000, Quantity = 60, Image = "https://placehold.co/600x600/60a5fa/white?text=Buds3+Pro", CategoryId = catDict["tai-nghe"], BrandId = brandDict["samsung"], ShopId = shopId },
            new() { Name = "JBL Tune 770NC", Slug = "jbl-tune-770nc", Description = "JBL Tune 770NC chống ồn, bass mạnh, pin 44 giờ, kết nối bluetooth 5.3.", Price = 2190000, CapitalPrice = 1800000, Quantity = 90, Image = "https://placehold.co/600x600/f97316/white?text=JBL+Tune+770NC", CategoryId = catDict["tai-nghe"], BrandId = brandDict["jbl"], ShopId = shopId },
            new() { Name = "JBL Live Pro 2", Slug = "jbl-live-pro-2", Description = "JBL Live Pro 2 True Wireless chống ồn, Adaptive ANC, pin 40 giờ.", Price = 3490000, CapitalPrice = 2900000, Quantity = 45, Image = "https://placehold.co/600x600/ea580c/white?text=JBL+Live+Pro+2", CategoryId = catDict["tai-nghe"], BrandId = brandDict["jbl"], ShopId = shopId },
            new() { Name = "Sony WF-1000XM5", Slug = "sony-wf-1000xm5", Description = "Sony WF-1000XM5 tai nghe true wireless nhỏ nhất, chống ồn đỉnh cao, LDAC.", Price = 6490000, CapitalPrice = 5500000, Quantity = 30, Image = "https://placehold.co/600x600/7c3aed/white?text=Sony+WF1000XM5", CategoryId = catDict["tai-nghe"], BrandId = brandDict["sony"], ShopId = shopId },

            // === ĐỒNG HỒ THÔNG MINH ===
            new() { Name = "Apple Watch Series 9", Slug = "apple-watch-series-9", Description = "Apple Watch S9 chip S9 SiP, Double Tap, đo SpO2, ECG, Always-On Retina.", Price = 10990000, CapitalPrice = 9800000, Quantity = 40, Image = "https://placehold.co/600x600/f472b6/white?text=Apple+Watch+S9", CategoryId = catDict["dong-ho-thong-minh"], BrandId = brandDict["apple"], ShopId = shopId },
            new() { Name = "Apple Watch Ultra 2", Slug = "apple-watch-ultra-2", Description = "Apple Watch Ultra 2 titanium, GPS + Cellular, lặn sâu 40m, pin 36 giờ.", Price = 21990000, CapitalPrice = 19500000, Quantity = 15, Image = "https://placehold.co/600x600/ec4899/white?text=Watch+Ultra+2", CategoryId = catDict["dong-ho-thong-minh"], BrandId = brandDict["apple"], ShopId = shopId },
            new() { Name = "Samsung Galaxy Watch 6 Classic", Slug = "samsung-galaxy-watch-6-classic", Description = "Galaxy Watch 6 Classic vành xoay, BIA, đo huyết áp, Wear OS 4.", Price = 9490000, CapitalPrice = 8200000, Quantity = 30, Image = "https://placehold.co/600x600/4f46e5/white?text=Watch+6+Classic", CategoryId = catDict["dong-ho-thong-minh"], BrandId = brandDict["samsung"], ShopId = shopId },
            new() { Name = "Xiaomi Watch S3", Slug = "xiaomi-watch-s3", Description = "Xiaomi Watch S3 vành bezel thay được, GNSS, SpO2, pin 15 ngày.", Price = 3290000, CapitalPrice = 2700000, Quantity = 70, Image = "https://placehold.co/600x600/dc2626/white?text=Xiaomi+Watch+S3", CategoryId = catDict["dong-ho-thong-minh"], BrandId = brandDict["xiaomi"], ShopId = shopId },

            // === PHỤ KIỆN ===
            new() { Name = "Anker Nano II 65W", Slug = "anker-nano-ii-65w", Description = "Sạc Anker Nano II 65W GaN nhỏ gọn, sạc nhanh laptop và điện thoại.", Price = 890000, CapitalPrice = 650000, Quantity = 200, Image = "https://placehold.co/600x600/10b981/white?text=Anker+65W", CategoryId = catDict["phu-kien"], BrandId = brandDict["anker"], ShopId = shopId },
            new() { Name = "Anker PowerCore 20000mAh", Slug = "anker-powercore-20000", Description = "Pin sạc dự phòng Anker PowerCore 20000mAh, sạc nhanh PD 20W, 2 cổng USB-C.", Price = 690000, CapitalPrice = 480000, Quantity = 150, Image = "https://placehold.co/600x600/34d399/white?text=Anker+20000mAh", CategoryId = catDict["phu-kien"], BrandId = brandDict["anker"], ShopId = shopId },
            new() { Name = "Apple MagSafe Charger", Slug = "apple-magsafe-charger", Description = "Đế sạc không dây MagSafe 15W chính hãng Apple cho iPhone.", Price = 1190000, CapitalPrice = 950000, Quantity = 100, Image = "https://placehold.co/600x600/d8b4fe/white?text=MagSafe", CategoryId = catDict["phu-kien"], BrandId = brandDict["apple"], ShopId = shopId },
            new() { Name = "Samsung 25W Travel Adapter", Slug = "samsung-25w-adapter", Description = "Củ sạc Samsung 25W Super Fast Charging, nhỏ gọn PD 3.0.", Price = 350000, CapitalPrice = 250000, Quantity = 200, Image = "https://placehold.co/600x600/93c5fd/white?text=Samsung+25W", CategoryId = catDict["phu-kien"], BrandId = brandDict["samsung"], ShopId = shopId },

            // === LOA & ÂM THANH ===
            new() { Name = "JBL Charge 5", Slug = "jbl-charge-5", Description = "Loa bluetooth JBL Charge 5 chống nước IP67, pin 20 giờ, bass mạnh mẽ.", Price = 3690000, CapitalPrice = 3100000, Quantity = 50, Image = "https://placehold.co/600x600/fb923c/white?text=JBL+Charge+5", CategoryId = catDict["loa-am-thanh"], BrandId = brandDict["jbl"], ShopId = shopId },
            new() { Name = "JBL Flip 6", Slug = "jbl-flip-6", Description = "Loa JBL Flip 6 nhỏ gọn, chống nước IP67, PartyBoost, pin 12 giờ.", Price = 2490000, CapitalPrice = 2000000, Quantity = 70, Image = "https://placehold.co/600x600/fdba74/white?text=JBL+Flip+6", CategoryId = catDict["loa-am-thanh"], BrandId = brandDict["jbl"], ShopId = shopId },
            new() { Name = "Sony SRS-XB100", Slug = "sony-srs-xb100", Description = "Loa Sony SRS-XB100 siêu nhỏ, chống nước IP67, Extra Bass, pin 16 giờ.", Price = 1290000, CapitalPrice = 1000000, Quantity = 80, Image = "https://placehold.co/600x600/a78bfa/white?text=Sony+XB100", CategoryId = catDict["loa-am-thanh"], BrandId = brandDict["sony"], ShopId = shopId },

            // === GAMING ===
            new() { Name = "Logitech G Pro X Superlight 2", Slug = "logitech-g-pro-x-superlight-2", Description = "Chuột gaming không dây siêu nhẹ 60g, cảm biến HERO 2, pin 95 giờ.", Price = 3290000, CapitalPrice = 2800000, Quantity = 40, Image = "https://placehold.co/600x600/f43f5e/white?text=G+Pro+X+SL2", CategoryId = catDict["gaming"], BrandId = brandDict["logitech"], ShopId = shopId },
            new() { Name = "Razer DeathAdder V3", Slug = "razer-deathadder-v3", Description = "Chuột gaming Razer DeathAdder V3 cảm biến Focus Pro 30K, 59g siêu nhẹ.", Price = 2190000, CapitalPrice = 1800000, Quantity = 50, Image = "https://placehold.co/600x600/22c55e/white?text=DeathAdder+V3", CategoryId = catDict["gaming"], BrandId = brandDict["razer"], ShopId = shopId },
            new() { Name = "Logitech G713 TKL", Slug = "logitech-g713-tkl", Description = "Bàn phím cơ gaming Logitech G713 TKL, switch GX Mechanical, RGB LIGHTSYNC.", Price = 3890000, CapitalPrice = 3300000, Quantity = 25, Image = "https://placehold.co/600x600/e11d48/white?text=G713+TKL", CategoryId = catDict["gaming"], BrandId = brandDict["logitech"], ShopId = shopId },
            new() { Name = "Razer BlackWidow V4", Slug = "razer-blackwidow-v4", Description = "Bàn phím cơ Razer BlackWidow V4 switch Green, macro keys, RGB Chroma.", Price = 4290000, CapitalPrice = 3700000, Quantity = 20, Image = "https://placehold.co/600x600/16a34a/white?text=BlackWidow+V4", CategoryId = catDict["gaming"], BrandId = brandDict["razer"], ShopId = shopId },

            // === TIVI ===
            new() { Name = "Samsung Smart TV 55\" QLED 4K", Slug = "samsung-tv-55-qled-4k", Description = "Smart TV Samsung 55 inch QLED 4K, Quantum Dot, Tizen OS, HDR10+.", Price = 14990000, CapitalPrice = 12500000, Quantity = 20, Image = "https://placehold.co/600x600/1d4ed8/white?text=Samsung+TV+55", CategoryId = catDict["tivi"], BrandId = brandDict["samsung"], ShopId = shopId },
            new() { Name = "LG Smart TV 65\" OLED evo", Slug = "lg-tv-65-oled-evo", Description = "LG OLED evo 65 inch, α9 Gen6 AI Processor, Dolby Vision & Atmos, webOS.", Price = 39990000, CapitalPrice = 35000000, Quantity = 10, Image = "https://placehold.co/600x600/1e3a8a/white?text=LG+OLED+65", CategoryId = catDict["tivi"], BrandId = brandDict["lg-electronics"], ShopId = shopId },
            new() { Name = "Sony Bravia 55\" 4K HDR", Slug = "sony-bravia-55-4k", Description = "Sony Bravia 55 inch 4K HDR, XR Processor, Google TV, Dolby Atmos.", Price = 19990000, CapitalPrice = 17000000, Quantity = 15, Image = "https://placehold.co/600x600/7c3aed/white?text=Sony+Bravia+55", CategoryId = catDict["tivi"], BrandId = brandDict["sony"], ShopId = shopId },
            new() { Name = "Xiaomi TV A Pro 55\"", Slug = "xiaomi-tv-a-pro-55", Description = "Xiaomi TV A Pro 55 inch 4K, MEMC, Dolby Vision, Google TV, giá rẻ nhất.", Price = 8990000, CapitalPrice = 7500000, Quantity = 30, Image = "https://placehold.co/600x600/ef4444/white?text=Xiaomi+TV+55", CategoryId = catDict["tivi"], BrandId = brandDict["xiaomi"], ShopId = shopId },

            // === THIẾT BỊ MẠNG ===
            new() { Name = "TP-Link Archer AX73", Slug = "tp-link-archer-ax73", Description = "Router WiFi 6 TP-Link AX5400, tốc độ cao, phủ sóng rộng, bảo mật HomeShield.", Price = 2290000, CapitalPrice = 1800000, Quantity = 40, Image = "https://placehold.co/600x600/0891b2/white?text=Archer+AX73", CategoryId = catDict["thiet-bi-mang"], BrandId = brandDict["xiaomi"], ShopId = shopId },

            // === GIA DỤNG THÔNG MINH ===
            new() { Name = "Xiaomi Robot Vacuum X10+", Slug = "xiaomi-robot-vacuum-x10-plus", Description = "Robot hút bụi lau nhà Xiaomi X10+, tự động giặt giẻ, LDS Navigation.", Price = 9990000, CapitalPrice = 8500000, Quantity = 20, Image = "https://placehold.co/600x600/f43f5e/white?text=Robot+X10%2B", CategoryId = catDict["gia-dung-thong-minh"], BrandId = brandDict["xiaomi"], ShopId = shopId },
            new() { Name = "Samsung Bespoke Jet Bot AI", Slug = "samsung-bespoke-jet-bot-ai", Description = "Robot hút bụi Samsung AI nhận diện vật thể, tự động hút xả, LiDAR.", Price = 18990000, CapitalPrice = 16000000, Quantity = 10, Image = "https://placehold.co/600x600/2563eb/white?text=Jet+Bot+AI", CategoryId = catDict["gia-dung-thong-minh"], BrandId = brandDict["samsung"], ShopId = shopId },

            // === MÁY ẢNH ===
            new() { Name = "Sony Alpha A7 IV", Slug = "sony-alpha-a7-iv", Description = "Máy ảnh mirrorless Sony A7 IV full-frame 33MP, quay 4K 60fps, IBIS 5 trục.", Price = 49990000, CapitalPrice = 45000000, Quantity = 8, Image = "https://placehold.co/600x600/6366f1/white?text=Sony+A7+IV", CategoryId = catDict["may-anh"], BrandId = brandDict["sony"], ShopId = shopId },
            new() { Name = "Sony ZV-E10 II", Slug = "sony-zv-e10-ii", Description = "Máy ảnh vlog Sony ZV-E10 II, cảm biến APS-C 26MP, quay 4K, mic tích hợp.", Price = 19990000, CapitalPrice = 17500000, Quantity = 20, Image = "https://placehold.co/600x600/818cf8/white?text=Sony+ZV-E10+II", CategoryId = catDict["may-anh"], BrandId = brandDict["sony"], ShopId = shopId },
        };

        // Stagger CreatedAt để sản phẩm mới về có ngày khác nhau
        var now = DateTime.UtcNow;
        for (var i = 0; i < products.Count; i++)
        {
            products[i].CreatedAt = now.AddHours(-i * 2); // Mỗi SP cách nhau 2h
        }

        context.Set<Product>().AddRange(products);
        await context.SaveChangesAsync();
        Console.WriteLine($"✅ Đã tạo {products.Count} sản phẩm");

        Console.WriteLine("🎉 Seed dữ liệu mẫu hoàn tất!");
    }
}
