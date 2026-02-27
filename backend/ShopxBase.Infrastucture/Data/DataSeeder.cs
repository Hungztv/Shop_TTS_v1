using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Enums;

namespace ShopxBase.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedSampleDataAsync(IServiceProvider serviceProvider)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ShopxBaseDbContext>();

            var productCount = await context.Set<Product>().CountAsync(p => !p.IsDeleted);
            if (productCount >= 80)
            {
                Console.WriteLine($"⏭️ Đã có {productCount} sản phẩm, bỏ qua seed.");
                return;
            }

            Console.WriteLine("🌱 Bắt đầu seed dữ liệu mẫu...");

            // ========== 1. CATEGORIES ==========
            var existingCatSlugs = await context.Set<Category>().Where(c => !c.IsDeleted).Select(c => c.Slug).ToListAsync();
            var allCategories = new List<Category>
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
                new() { Name = "Gia dụng thông minh", Description = "Robot hút bụi, máy lọc không khí, smart home", Slug = "gia-dung-thong-minh", Status = "active" },
            };
            var newCats = allCategories.Where(c => !existingCatSlugs.Contains(c.Slug)).ToList();
            if (newCats.Any()) { context.Set<Category>().AddRange(newCats); await context.SaveChangesAsync(); }
            Console.WriteLine($"✅ Danh mục: +{newCats.Count} mới");

            // ========== 2. BRANDS ==========
            var existingBrandSlugs = await context.Set<Brand>().Where(b => !b.IsDeleted).Select(b => b.Slug).ToListAsync();
            var allBrands = new List<Brand>
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
                new() { Name = "MSI", Description = "Thương hiệu laptop gaming và phần cứng PC", Slug = "msi", Status = "active", Logo = "" },
                new() { Name = "Google", Description = "Tập đoàn công nghệ với sản phẩm Pixel", Slug = "google", Status = "active", Logo = "" },
                new() { Name = "Bose", Description = "Thương hiệu âm thanh cao cấp từ Mỹ", Slug = "bose", Status = "active", Logo = "" },
                new() { Name = "TP-Link", Description = "Thương hiệu thiết bị mạng hàng đầu", Slug = "tp-link", Status = "active", Logo = "" },
                new() { Name = "Baseus", Description = "Thương hiệu phụ kiện công nghệ giá tốt", Slug = "baseus", Status = "active", Logo = "" },
            };
            var newBrands = allBrands.Where(b => !existingBrandSlugs.Contains(b.Slug)).ToList();
            if (newBrands.Any()) { context.Set<Brand>().AddRange(newBrands); await context.SaveChangesAsync(); }
            Console.WriteLine($"✅ Thương hiệu: +{newBrands.Count} mới");

            // ========== 3. SHOP ==========
            var shop = await context.Set<Shop>().FirstOrDefaultAsync(s => !s.IsDeleted);
            if (shop == null)
            {
                var bizReg = new BusinessRegistration
                {
                    UserId = "", CompanyName = "ShopTTS Official Store", TaxCode = "0000000000",
                    OwnerName = "System Admin", Email = "admin@shopxbase.com", Phone = "0900000000",
                    Address = "Hệ thống ShopTTS", Status = BusinessRegistrationStatus.Approved,
                    ReviewedAt = DateTime.UtcNow, ReviewedBy = "system",
                };
                context.Set<BusinessRegistration>().Add(bizReg);
                await context.SaveChangesAsync();

                shop = new Shop
                {
                    Name = "ShopTTS Official", Slug = "shoptts-official",
                    Description = "Cửa hàng chính thức của hệ thống ShopTTS",
                    Status = ShopStatus.Active, OwnerUserId = "", BusinessRegistrationId = bizReg.Id,
                };
                context.Set<Shop>().Add(shop);
                await context.SaveChangesAsync();
                Console.WriteLine("✅ Đã tạo shop hệ thống");
            }
            var sid = shop.Id;

            // Load IDs
            var C = await context.Set<Category>().Where(c => !c.IsDeleted).ToDictionaryAsync(c => c.Slug, c => c.Id);
            var B = await context.Set<Brand>().Where(b => !b.IsDeleted).ToDictionaryAsync(b => b.Slug, b => b.Id);

            // Helper
            string Img(string hex, string text) => $"https://placehold.co/600x600/{hex}/white?text={Uri.EscapeDataString(text)}";

            // ========== 4. PRODUCTS (100+) ==========
            var products = new List<Product>
            {
                // ===== ĐIỆN THOẠI (16 sp) =====
                P("iPhone 16 Pro Max 256GB", "iphone-16-pro-max-256gb", "iPhone 16 Pro Max chip A18 Pro, camera 48MP, màn Super Retina XDR 6.9 inch, pin cả ngày.", 34990000, 32000000, 50, Img("7c3aed","iPhone+16+Pro+Max"), C["dien-thoai"], B["apple"], sid),
                P("iPhone 16 128GB", "iphone-16-128gb", "iPhone 16 Dynamic Island, chip A18, camera 48MP, nút Action mới.", 24990000, 22500000, 80, Img("6366f1","iPhone+16"), C["dien-thoai"], B["apple"], sid),
                P("iPhone 15 128GB", "iphone-15-128gb", "iPhone 15 Dynamic Island, camera 48MP, USB-C, chip A16 Bionic.", 19990000, 17500000, 60, Img("8b5cf6","iPhone+15"), C["dien-thoai"], B["apple"], sid),
                P("Samsung Galaxy S24 Ultra", "samsung-galaxy-s24-ultra", "Galaxy S24 Ultra bút S-Pen, Snapdragon 8 Gen 3, camera 200MP, Galaxy AI.", 31990000, 28500000, 40, Img("1e40af","Galaxy+S24+Ultra"), C["dien-thoai"], B["samsung"], sid),
                P("Samsung Galaxy S24", "samsung-galaxy-s24", "Galaxy S24 chip Exynos 2400, camera AI 50MP, màn Dynamic AMOLED 2X.", 22990000, 20000000, 55, Img("2563eb","Galaxy+S24"), C["dien-thoai"], B["samsung"], sid),
                P("Samsung Galaxy A55 5G", "samsung-galaxy-a55-5g", "Galaxy A55 5G chip Exynos 1480, camera OIS 50MP, pin 5000mAh.", 9990000, 8500000, 100, Img("3b82f6","Galaxy+A55"), C["dien-thoai"], B["samsung"], sid),
                P("Samsung Galaxy A35 5G", "samsung-galaxy-a35-5g", "Galaxy A35 5G Super AMOLED 120Hz, camera 50MP OIS, chống nước IP67.", 7990000, 6800000, 90, Img("60a5fa","Galaxy+A35"), C["dien-thoai"], B["samsung"], sid),
                P("Xiaomi 14 Ultra", "xiaomi-14-ultra", "Xiaomi 14 Ultra camera Leica chuyên nghiệp, Snapdragon 8 Gen 3, sạc 90W.", 23990000, 21000000, 30, Img("dc2626","Xiaomi+14+Ultra"), C["dien-thoai"], B["xiaomi"], sid),
                P("Xiaomi 14", "xiaomi-14", "Xiaomi 14 Leica camera, Snapdragon 8 Gen 3, sạc 90W, IP68.", 18990000, 16500000, 45, Img("ef4444","Xiaomi+14"), C["dien-thoai"], B["xiaomi"], sid),
                P("Xiaomi Redmi Note 13 Pro", "xiaomi-redmi-note-13-pro", "Redmi Note 13 Pro camera 200MP, AMOLED 120Hz, sạc 67W.", 7490000, 6200000, 120, Img("f87171","Redmi+Note+13+Pro"), C["dien-thoai"], B["xiaomi"], sid),
                P("Oppo Find X7 Ultra", "oppo-find-x7-ultra", "Find X7 Ultra Hasselblad, camera periscope kép, Dimensity 9300.", 22990000, 20000000, 25, Img("16a34a","Find+X7+Ultra"), C["dien-thoai"], B["oppo"], sid),
                P("Oppo Reno 11 5G", "oppo-reno-11-5g", "Reno 11 5G thiết kế thời trang, camera AI 50MP, AMOLED 120Hz.", 10990000, 9500000, 60, Img("22c55e","Reno+11+5G"), C["dien-thoai"], B["oppo"], sid),
                P("Google Pixel 8 Pro", "google-pixel-8-pro", "Pixel 8 Pro chip Tensor G3, camera AI tốt nhất Android, 7 năm cập nhật.", 24990000, 22000000, 20, Img("4285f4","Pixel+8+Pro"), C["dien-thoai"], B["google"], sid),
                P("Google Pixel 8a", "google-pixel-8a", "Pixel 8a chip Tensor G3, camera AI 64MP, màn OLED 120Hz, giá tốt.", 12990000, 11000000, 40, Img("34a853","Pixel+8a"), C["dien-thoai"], B["google"], sid),
                P("Huawei Pura 70 Ultra", "huawei-pura-70-ultra", "Huawei Pura 70 Ultra camera XMAGE, thiết kế cao cấp, Kirin 9010.", 28990000, 25000000, 15, Img("c7254e","Pura+70+Ultra"), C["dien-thoai"], B["huawei"], sid),
                P("Samsung Galaxy Z Flip 5", "samsung-galaxy-z-flip-5", "Galaxy Z Flip 5 gập, Flex Window 3.4 inch, Snapdragon 8 Gen 2.", 25990000, 23000000, 30, Img("1d4ed8","Z+Flip+5"), C["dien-thoai"], B["samsung"], sid),

                // ===== LAPTOP (16 sp) =====
                P("MacBook Air M3 13 inch", "macbook-air-m3-13", "MacBook Air M3 siêu mỏng nhẹ, chip M3, pin 18 giờ, Liquid Retina.", 27990000, 25000000, 35, Img("a855f7","MacBook+Air+M3"), C["laptop"], B["apple"], sid),
                P("MacBook Pro 14 M3 Pro", "macbook-pro-14-m3-pro", "MacBook Pro 14 chip M3 Pro, hiệu năng đỉnh cao cho sáng tạo.", 49990000, 45000000, 20, Img("9333ea","MacBook+Pro+14"), C["laptop"], B["apple"], sid),
                P("MacBook Air M2 15 inch", "macbook-air-m2-15", "MacBook Air 15 inch M2, màn hình lớn, 8GB RAM, 256GB SSD.", 32990000, 29000000, 25, Img("7c3aed","MacBook+Air+15"), C["laptop"], B["apple"], sid),
                P("Dell XPS 15", "dell-xps-15", "Dell XPS 15 OLED 3.5K, Core i7-13700H, 16GB RAM, thiết kế cao cấp.", 39990000, 36000000, 15, Img("0ea5e9","Dell+XPS+15"), C["laptop"], B["dell"], sid),
                P("Dell Inspiron 15 3530", "dell-inspiron-15-3530", "Dell Inspiron 15 Core i5, 8GB RAM, 512GB SSD, laptop văn phòng.", 15990000, 13500000, 45, Img("38bdf8","Inspiron+15"), C["laptop"], B["dell"], sid),
                P("Dell Latitude 5540", "dell-latitude-5540", "Dell Latitude 5540 doanh nghiệp, Core i7, 16GB, bảo mật TPM 2.0.", 28990000, 25000000, 15, Img("0284c7","Latitude+5540"), C["laptop"], B["dell"], sid),
                P("ASUS ROG Strix G16", "asus-rog-strix-g16", "ROG Strix G16 RTX 4060, i7-13650HX, 165Hz, RGB keyboard.", 35990000, 32000000, 20, Img("f43f5e","ROG+Strix+G16"), C["laptop"], B["asus"], sid),
                P("ASUS Vivobook 15", "asus-vivobook-15", "Vivobook 15 Ryzen 5 7530U, 512GB SSD, mỏng nhẹ giá tốt.", 13490000, 11500000, 60, Img("fb923c","Vivobook+15"), C["laptop"], B["asus"], sid),
                P("ASUS TUF Gaming A15", "asus-tuf-gaming-a15", "TUF Gaming A15 Ryzen 7, RTX 4050, 144Hz, thiết kế bền MIL-STD.", 24990000, 22000000, 25, Img("e11d48","TUF+A15"), C["laptop"], B["asus"], sid),
                P("Lenovo ThinkPad X1 Carbon Gen 11", "lenovo-thinkpad-x1-carbon-gen11", "ThinkPad X1 Carbon 1.12kg, Intel vPro, bảo mật doanh nghiệp.", 42990000, 39000000, 10, Img("64748b","ThinkPad+X1"), C["laptop"], B["lenovo"], sid),
                P("Lenovo IdeaPad Slim 5", "lenovo-ideapad-slim-5", "IdeaPad Slim 5 Ryzen 7, 16GB RAM, 2.8K OLED, mỏng nhẹ.", 18990000, 16500000, 40, Img("94a3b8","IdeaPad+5"), C["laptop"], B["lenovo"], sid),
                P("Lenovo Legion 5 Pro", "lenovo-legion-5-pro", "Legion 5 Pro RTX 4070, Ryzen 7 7745HX, 165Hz QHD, gaming pro.", 38990000, 35000000, 15, Img("475569","Legion+5+Pro"), C["laptop"], B["lenovo"], sid),
                P("HP Pavilion 15", "hp-pavilion-15", "HP Pavilion 15 Core i5-1335U, 8GB, 512GB SSD, thiết kế hiện đại.", 16490000, 14000000, 35, Img("06b6d4","HP+Pavilion"), C["laptop"], B["hp"], sid),
                P("HP Envy x360", "hp-envy-x360", "HP Envy x360 2-in-1 OLED, Ryzen 7, cảm ứng xoay gập 360°.", 25990000, 23000000, 20, Img("0891b2","HP+Envy+x360"), C["laptop"], B["hp"], sid),
                P("MSI GF63 Thin", "msi-gf63-thin", "MSI GF63 Thin Core i5, RTX 3050, 144Hz, gaming giá rẻ.", 17990000, 15500000, 30, Img("e11d48","MSI+GF63"), C["laptop"], B["msi"], sid),
                P("MSI Katana 15", "msi-katana-15", "MSI Katana 15 i7-13620H, RTX 4060, 144Hz, thiết kế gaming.", 29990000, 27000000, 18, Img("dc2626","MSI+Katana+15"), C["laptop"], B["msi"], sid),

                // ===== MÁY TÍNH BẢNG (8 sp) =====
                P("iPad Air M2 11 inch", "ipad-air-m2-11", "iPad Air M2 Liquid Retina 11 inch, Apple Pencil Pro.", 16990000, 15000000, 40, Img("c084fc","iPad+Air+M2"), C["may-tinh-bang"], B["apple"], sid),
                P("iPad Pro M4 13 inch", "ipad-pro-m4-13", "iPad Pro M4 Ultra Retina XDR tandem OLED, mỏng nhất.", 35990000, 32000000, 15, Img("a78bfa","iPad+Pro+M4"), C["may-tinh-bang"], B["apple"], sid),
                P("iPad 10 Wi-Fi 64GB", "ipad-10-wifi-64gb", "iPad 10 chip A14, 10.9 inch Liquid Retina, USB-C, giá tốt.", 9490000, 8000000, 50, Img("d8b4fe","iPad+10"), C["may-tinh-bang"], B["apple"], sid),
                P("iPad mini 6", "ipad-mini-6", "iPad mini 6 chip A15, 8.3 inch, nhỏ gọn cầm tay, Apple Pencil.", 13990000, 12000000, 30, Img("e879f9","iPad+mini+6"), C["may-tinh-bang"], B["apple"], sid),
                P("Samsung Galaxy Tab S9 FE", "samsung-galaxy-tab-s9-fe", "Tab S9 FE S-Pen, 10.9 inch, IP68, Exynos 1380.", 10490000, 9000000, 50, Img("3b82f6","Tab+S9+FE"), C["may-tinh-bang"], B["samsung"], sid),
                P("Samsung Galaxy Tab S9 Ultra", "samsung-galaxy-tab-s9-ultra", "Tab S9 Ultra 14.6 inch AMOLED, Snapdragon 8 Gen 2, S-Pen.", 28990000, 26000000, 12, Img("1d4ed8","Tab+S9+Ultra"), C["may-tinh-bang"], B["samsung"], sid),
                P("Xiaomi Pad 6", "xiaomi-pad-6", "Xiaomi Pad 6 11 inch 144Hz, Snapdragon 870, quad loa.", 7990000, 6800000, 55, Img("f87171","Xiaomi+Pad+6"), C["may-tinh-bang"], B["xiaomi"], sid),
                P("Lenovo Tab P12 Pro", "lenovo-tab-p12-pro", "Lenovo Tab P12 Pro AMOLED 12.6 inch, JBL quad speakers.", 12990000, 11000000, 20, Img("475569","Tab+P12+Pro"), C["may-tinh-bang"], B["lenovo"], sid),

                // ===== TAI NGHE (12 sp) =====
                P("AirPods Pro 2 USB-C", "airpods-pro-2-usb-c", "AirPods Pro 2 chống ồn, Adaptive Audio, chip H2, USB-C.", 5990000, 5200000, 80, Img("e879f9","AirPods+Pro+2"), C["tai-nghe"], B["apple"], sid),
                P("AirPods 4", "airpods-4", "AirPods 4 thiết kế mới, chip H2, Personalized Spatial Audio.", 3490000, 2800000, 100, Img("c084fc","AirPods+4"), C["tai-nghe"], B["apple"], sid),
                P("AirPods Max USB-C", "airpods-max-usb-c", "AirPods Max over-ear, chống ồn, USB-C, âm thanh Hi-Fi.", 13990000, 12000000, 15, Img("a78bfa","AirPods+Max"), C["tai-nghe"], B["apple"], sid),
                P("Sony WH-1000XM5", "sony-wh-1000xm5", "Sony WH-1000XM5 chống ồn #1 thế giới, LDAC, pin 30 giờ.", 7490000, 6500000, 35, Img("818cf8","WH1000XM5"), C["tai-nghe"], B["sony"], sid),
                P("Sony WF-1000XM5", "sony-wf-1000xm5", "Sony WF-1000XM5 TWS nhỏ nhất, chống ồn đỉnh, LDAC.", 6490000, 5500000, 30, Img("6366f1","WF1000XM5"), C["tai-nghe"], B["sony"], sid),
                P("Samsung Galaxy Buds3 Pro", "samsung-galaxy-buds3-pro", "Buds3 Pro ANC, 360 Audio, thiết kế trong suốt.", 4990000, 4200000, 60, Img("60a5fa","Buds3+Pro"), C["tai-nghe"], B["samsung"], sid),
                P("Samsung Galaxy Buds FE", "samsung-galaxy-buds-fe", "Galaxy Buds FE chống ồn ANC, bass sâu, giá tốt.", 2490000, 2000000, 80, Img("93c5fd","Buds+FE"), C["tai-nghe"], B["samsung"], sid),
                P("JBL Tune 770NC", "jbl-tune-770nc", "JBL Tune 770NC chống ồn, bass mạnh, pin 44 giờ, BT 5.3.", 2190000, 1800000, 90, Img("f97316","JBL+770NC"), C["tai-nghe"], B["jbl"], sid),
                P("JBL Live Pro 2", "jbl-live-pro-2", "JBL Live Pro 2 TWS chống ồn Adaptive ANC, pin 40 giờ.", 3490000, 2900000, 45, Img("ea580c","JBL+Live+Pro+2"), C["tai-nghe"], B["jbl"], sid),
                P("Bose QuietComfort Ultra", "bose-qc-ultra-headphones", "Bose QC Ultra chống ồn hàng đầu, Immersive Audio, premium.", 8990000, 7800000, 20, Img("1e3a8a","Bose+QC+Ultra"), C["tai-nghe"], B["bose"], sid),
                P("Bose QuietComfort Earbuds II", "bose-qc-earbuds-ii", "Bose QC Earbuds II TWS chống ồn CustomTune, nhỏ gọn.", 6490000, 5500000, 25, Img("1e40af","Bose+QC+Buds+II"), C["tai-nghe"], B["bose"], sid),
                P("Razer Barracuda X", "razer-barracuda-x", "Razer Barracuda X wireless gaming headset, 50 giờ pin.", 2290000, 1800000, 40, Img("22c55e","Barracuda+X"), C["tai-nghe"], B["razer"], sid),

                // ===== ĐỒNG HỒ THÔNG MINH (8 sp) =====
                P("Apple Watch Series 9", "apple-watch-series-9", "Apple Watch S9 chip S9 SiP, Double Tap, SpO2, ECG.", 10990000, 9800000, 40, Img("f472b6","Watch+S9"), C["dong-ho-thong-minh"], B["apple"], sid),
                P("Apple Watch Ultra 2", "apple-watch-ultra-2", "Watch Ultra 2 titanium, GPS+Cellular, lặn 40m, pin 36h.", 21990000, 19500000, 15, Img("ec4899","Watch+Ultra+2"), C["dong-ho-thong-minh"], B["apple"], sid),
                P("Apple Watch SE 2", "apple-watch-se-2", "Apple Watch SE 2 chip S8, phát hiện va chạm, giá tốt.", 6990000, 6000000, 50, Img("f9a8d4","Watch+SE+2"), C["dong-ho-thong-minh"], B["apple"], sid),
                P("Samsung Galaxy Watch 6 Classic", "samsung-galaxy-watch-6-classic", "Watch 6 Classic vành xoay, BIA, đo huyết áp, Wear OS 4.", 9490000, 8200000, 30, Img("4f46e5","Watch+6+Classic"), C["dong-ho-thong-minh"], B["samsung"], sid),
                P("Samsung Galaxy Watch FE", "samsung-galaxy-watch-fe", "Galaxy Watch FE BIA, nhịp tim, giấc ngủ, giá hợp lý.", 4990000, 4000000, 45, Img("6366f1","Watch+FE"), C["dong-ho-thong-minh"], B["samsung"], sid),
                P("Xiaomi Watch S3", "xiaomi-watch-s3", "Xiaomi Watch S3 bezel thay được, GNSS, SpO2, 15 ngày pin.", 3290000, 2700000, 70, Img("dc2626","Xiaomi+Watch+S3"), C["dong-ho-thong-minh"], B["xiaomi"], sid),
                P("Xiaomi Smart Band 8 Pro", "xiaomi-smart-band-8-pro", "Mi Band 8 Pro AMOLED 1.74 inch, GPS, 150+ bài tập.", 1490000, 1100000, 100, Img("ef4444","Band+8+Pro"), C["dong-ho-thong-minh"], B["xiaomi"], sid),
                P("Huawei Watch GT 4", "huawei-watch-gt-4", "Watch GT 4 thiết kế thời trang, SpO2, GPS, pin 14 ngày.", 5990000, 5000000, 30, Img("c7254e","Watch+GT+4"), C["dong-ho-thong-minh"], B["huawei"], sid),

                // ===== PHỤ KIỆN (10 sp) =====
                P("Anker Nano II 65W", "anker-nano-ii-65w", "Sạc Anker Nano II 65W GaN nhỏ gọn, sạc nhanh laptop+điện thoại.", 890000, 650000, 200, Img("10b981","Anker+65W"), C["phu-kien"], B["anker"], sid),
                P("Anker PowerCore 20000mAh", "anker-powercore-20000", "Pin dự phòng Anker 20000mAh, PD 20W, 2 cổng USB-C.", 690000, 480000, 150, Img("34d399","Anker+20K"), C["phu-kien"], B["anker"], sid),
                P("Anker 735 Charger 65W", "anker-735-charger-65w", "Sạc Anker 735 65W 3 cổng (2C+1A), GaN II, sạc 3 thiết bị.", 1190000, 950000, 80, Img("059669","Anker+735"), C["phu-kien"], B["anker"], sid),
                P("Apple MagSafe Charger", "apple-magsafe-charger", "Đế sạc không dây MagSafe 15W chính hãng Apple.", 1190000, 950000, 100, Img("d8b4fe","MagSafe"), C["phu-kien"], B["apple"], sid),
                P("Samsung 25W Travel Adapter", "samsung-25w-adapter", "Củ sạc Samsung 25W Super Fast Charging PD 3.0.", 350000, 250000, 200, Img("93c5fd","Samsung+25W"), C["phu-kien"], B["samsung"], sid),
                P("Baseus 100W USB-C Cable", "baseus-100w-usb-c-cable", "Cáp Baseus USB-C 100W 2m, sạc nhanh laptop, bện nylon.", 250000, 150000, 300, Img("f97316","Baseus+Cable"), C["phu-kien"], B["baseus"], sid),
                P("Baseus 20000mAh 65W", "baseus-20000-65w", "Pin dự phòng Baseus 20000mAh 65W sạc nhanh laptop.", 890000, 700000, 100, Img("fb923c","Baseus+65W"), C["phu-kien"], B["baseus"], sid),
                P("Anker MagGo 5000mAh", "anker-maggo-5000", "Pin MagSafe Anker MagGo 5000mAh siêu mỏng cho iPhone.", 590000, 420000, 120, Img("22c55e","MagGo+5K"), C["phu-kien"], B["anker"], sid),
                P("Logitech Pebble Keys 2", "logitech-pebble-keys-2", "Bàn phím bluetooth Logitech Pebble Keys 2 mỏng nhẹ, multi-device.", 890000, 700000, 60, Img("64748b","Pebble+Keys+2"), C["phu-kien"], B["logitech"], sid),
                P("Logitech MX Anywhere 3S", "logitech-mx-anywhere-3s", "Chuột Logitech MX Anywhere 3S compact, 8K DPI, mọi bề mặt.", 1790000, 1500000, 40, Img("475569","MX+Anywhere+3S"), C["phu-kien"], B["logitech"], sid),

                // ===== LOA & ÂM THANH (8 sp) =====
                P("JBL Charge 5", "jbl-charge-5", "Loa JBL Charge 5 IP67, pin 20 giờ, bass mạnh, sạc điện thoại.", 3690000, 3100000, 50, Img("fb923c","JBL+Charge+5"), C["loa-am-thanh"], B["jbl"], sid),
                P("JBL Flip 6", "jbl-flip-6", "Loa JBL Flip 6 IP67, PartyBoost, pin 12 giờ, nhỏ gọn.", 2490000, 2000000, 70, Img("fdba74","JBL+Flip+6"), C["loa-am-thanh"], B["jbl"], sid),
                P("JBL Go 4", "jbl-go-4", "Loa JBL Go 4 siêu nhỏ bỏ túi, IP67, pin 7 giờ.", 990000, 750000, 100, Img("f97316","JBL+Go+4"), C["loa-am-thanh"], B["jbl"], sid),
                P("JBL Xtreme 4", "jbl-xtreme-4", "Loa JBL Xtreme 4 công suất lớn, IP67, pin 24 giờ, Party mode.", 7490000, 6500000, 20, Img("ea580c","JBL+Xtreme+4"), C["loa-am-thanh"], B["jbl"], sid),
                P("Sony SRS-XB100", "sony-srs-xb100", "Loa Sony SRS-XB100 siêu nhỏ, IP67, Extra Bass, pin 16 giờ.", 1290000, 1000000, 80, Img("a78bfa","Sony+XB100"), C["loa-am-thanh"], B["sony"], sid),
                P("Sony SRS-XG300", "sony-srs-xg300", "Loa Sony XG300 công suất lớn, X-Balanced Speaker, pin 25 giờ.", 5990000, 5000000, 25, Img("818cf8","Sony+XG300"), C["loa-am-thanh"], B["sony"], sid),
                P("Bose SoundLink Flex", "bose-soundlink-flex", "Loa Bose SoundLink Flex IP67, PositionIQ, pin 12 giờ.", 3290000, 2800000, 35, Img("1e3a8a","Bose+Flex"), C["loa-am-thanh"], B["bose"], sid),
                P("Samsung Soundbar HW-Q990D", "samsung-soundbar-hw-q990d", "Soundbar Samsung 11.1.4 kênh Dolby Atmos, không dây, sub.", 18990000, 16000000, 10, Img("1d4ed8","Samsung+Soundbar"), C["loa-am-thanh"], B["samsung"], sid),

                // ===== GAMING (10 sp) =====
                P("Logitech G Pro X Superlight 2", "logitech-g-pro-x-superlight-2", "Chuột gaming wireless 60g, HERO 2 sensor, pin 95 giờ.", 3290000, 2800000, 40, Img("f43f5e","G+Pro+X+SL2"), C["gaming"], B["logitech"], sid),
                P("Logitech G502 X Plus", "logitech-g502-x-plus", "Chuột gaming wireless G502 X Plus, LIGHTFORCE switches, RGB.", 3690000, 3100000, 30, Img("e11d48","G502+X+Plus"), C["gaming"], B["logitech"], sid),
                P("Razer DeathAdder V3", "razer-deathadder-v3", "Chuột gaming Razer DeathAdder V3 Focus Pro 30K, 59g.", 2190000, 1800000, 50, Img("22c55e","DeathAdder+V3"), C["gaming"], B["razer"], sid),
                P("Razer Viper V3 HyperSpeed", "razer-viper-v3-hyperspeed", "Razer Viper V3 wireless 82g, Focus Pro 35K, HyperSpeed.", 3490000, 2900000, 25, Img("16a34a","Viper+V3"), C["gaming"], B["razer"], sid),
                P("Logitech G713 TKL", "logitech-g713-tkl", "Bàn phím cơ Logitech G713 TKL, GX Mechanical, RGB.", 3890000, 3300000, 25, Img("f472b6","G713+TKL"), C["gaming"], B["logitech"], sid),
                P("Razer BlackWidow V4", "razer-blackwidow-v4", "Bàn phím cơ Razer BlackWidow V4 Green, macro, Chroma.", 4290000, 3700000, 20, Img("059669","BlackWidow+V4"), C["gaming"], B["razer"], sid),
                P("Razer Huntsman V3 Pro TKL", "razer-huntsman-v3-pro-tkl", "Razer Huntsman V3 Pro TKL, analog optical switch, esports.", 5490000, 4800000, 15, Img("10b981","Huntsman+V3"), C["gaming"], B["razer"], sid),
                P("Logitech G PRO X TKL Rapid", "logitech-g-pro-x-tkl-rapid", "Bàn phím Logitech G PRO X TKL Rapid Trigger, 0.1mm gõ.", 4490000, 3800000, 20, Img("dc2626","G+PRO+X+TKL"), C["gaming"], B["logitech"], sid),
                P("Razer Kraken V4", "razer-kraken-v4", "Tai nghe gaming Razer Kraken V4, haptic, THX Spatial Audio.", 4290000, 3600000, 25, Img("4f46e5","Kraken+V4"), C["gaming"], B["razer"], sid),
                P("Logitech G733 Wireless", "logitech-g733-wireless", "Tai nghe gaming wireless Logitech G733, 278g nhẹ, RGB, DTS.", 3290000, 2800000, 30, Img("ec4899","G733"), C["gaming"], B["logitech"], sid),

                // ===== TIVI (8 sp) =====
                P("Samsung 55\" QLED 4K Q80D", "samsung-tv-55-qled-q80d", "Samsung 55 inch QLED 4K Q80D, Neural Quantum 4K, Tizen.", 17990000, 15000000, 20, Img("1d4ed8","Samsung+Q80D"), C["tivi"], B["samsung"], sid),
                P("Samsung 65\" Neo QLED 4K", "samsung-tv-65-neo-qled", "Samsung 65 inch Neo QLED QN85D, Mini LED, HDR10+.", 29990000, 26000000, 12, Img("1e40af","Samsung+Neo+QLED"), C["tivi"], B["samsung"], sid),
                P("LG 65\" OLED evo C4", "lg-tv-65-oled-c4", "LG OLED evo C4 65 inch, α9 Gen7, Dolby Vision & Atmos.", 39990000, 35000000, 10, Img("1e3a8a","LG+OLED+C4"), C["tivi"], B["lg-electronics"], sid),
                P("LG 55\" OLED B4", "lg-tv-55-oled-b4", "LG OLED B4 55 inch, α8 AI processor, webOS 24, 120Hz.", 24990000, 22000000, 15, Img("3b82f6","LG+OLED+B4"), C["tivi"], B["lg-electronics"], sid),
                P("Sony Bravia 55\" 4K X90L", "sony-bravia-55-x90l", "Sony 55 inch 4K X90L, XR Processor, Google TV, Atmos.", 19990000, 17000000, 15, Img("7c3aed","Sony+X90L"), C["tivi"], B["sony"], sid),
                P("Sony Bravia 65\" OLED A80L", "sony-bravia-65-oled-a80l", "Sony 65 inch OLED A80L, XR OLED Contrast Pro, Acoustic.", 44990000, 40000000, 8, Img("6366f1","Sony+A80L"), C["tivi"], B["sony"], sid),
                P("Xiaomi TV A Pro 55\"", "xiaomi-tv-a-pro-55", "Xiaomi TV A Pro 55 inch 4K, MEMC, Dolby Vision, Google TV.", 8990000, 7500000, 30, Img("ef4444","Xiaomi+TV+55"), C["tivi"], B["xiaomi"], sid),
                P("Samsung 43\" Crystal UHD", "samsung-tv-43-crystal-uhd", "Samsung 43 inch Crystal UHD 4K, PurColor, Tizen, giá tốt.", 8490000, 7000000, 35, Img("60a5fa","Samsung+43+UHD"), C["tivi"], B["samsung"], sid),

                // ===== THIẾT BỊ MẠNG (4 sp) =====
                P("TP-Link Archer AX73", "tp-link-archer-ax73", "Router WiFi 6 AX5400, phủ sóng rộng, HomeShield bảo mật.", 2290000, 1800000, 40, Img("0891b2","Archer+AX73"), C["thiet-bi-mang"], B["tp-link"], sid),
                P("TP-Link Deco X50 (3-pack)", "tp-link-deco-x50-3pack", "Mesh WiFi 6 TP-Link Deco X50 phủ 600m², bộ 3, AX3000.", 4490000, 3800000, 25, Img("06b6d4","Deco+X50"), C["thiet-bi-mang"], B["tp-link"], sid),
                P("TP-Link Archer AX55", "tp-link-archer-ax55", "Router WiFi 6 AX3000, OneMesh, USB 3.0, giá hợp lý.", 1690000, 1300000, 50, Img("0ea5e9","Archer+AX55"), C["thiet-bi-mang"], B["tp-link"], sid),
                P("Xiaomi Router AX3000T", "xiaomi-router-ax3000t", "Xiaomi Router AX3000T WiFi 6, Mesh, 4 anten, giá rẻ.", 790000, 550000, 80, Img("f87171","Xiaomi+AX3000T"), C["thiet-bi-mang"], B["xiaomi"], sid),

                // ===== GIA DỤNG THÔNG MINH (4 sp) =====
                P("Xiaomi Robot Vacuum X10+", "xiaomi-robot-vacuum-x10-plus", "Robot hút bụi lau nhà X10+, tự giặt giẻ, LDS.", 9990000, 8500000, 20, Img("f43f5e","Robot+X10+"), C["gia-dung-thong-minh"], B["xiaomi"], sid),
                P("Xiaomi Air Purifier 4 Pro", "xiaomi-air-purifier-4-pro", "Máy lọc không khí Xiaomi 4 Pro, OLED, 60m², HEPA H13.", 4990000, 4000000, 30, Img("22c55e","Air+Purifier+4"), C["gia-dung-thong-minh"], B["xiaomi"], sid),
                P("Samsung Bespoke Jet Bot AI", "samsung-bespoke-jet-bot-ai", "Robot Samsung AI nhận diện vật thể, LiDAR, tự xả.", 18990000, 16000000, 10, Img("2563eb","Jet+Bot+AI"), C["gia-dung-thong-minh"], B["samsung"], sid),
                P("LG PuriCare AeroTower", "lg-puricare-aerotower", "Máy lọc không khí LG AeroTower, quạt, sưởi, HEPA.", 15990000, 14000000, 12, Img("64748b","LG+AeroTower"), C["gia-dung-thong-minh"], B["lg-electronics"], sid),

                // ===== MÁY ẢNH (4 sp) =====
                P("Sony Alpha A7 IV", "sony-alpha-a7-iv", "Sony A7 IV full-frame 33MP, 4K 60fps, IBIS 5 trục.", 49990000, 45000000, 8, Img("6366f1","Sony+A7+IV"), C["may-anh"], B["sony"], sid),
                P("Sony ZV-E10 II", "sony-zv-e10-ii", "Sony ZV-E10 II APS-C 26MP, vlog, 4K, mic tích hợp.", 19990000, 17500000, 20, Img("818cf8","ZV-E10+II"), C["may-anh"], B["sony"], sid),
                P("Sony Alpha A6700", "sony-alpha-a6700", "Sony A6700 APS-C 26MP, AI AF, 4K 120fps, nhỏ gọn.", 32990000, 29000000, 12, Img("a78bfa","Sony+A6700"), C["may-anh"], B["sony"], sid),
                P("Sony ZV-1F Vlog", "sony-zv-1f-vlog", "Sony ZV-1F compact vlog, 20.1MP, 4K, góc rộng 20mm.", 12990000, 11000000, 25, Img("7c3aed","Sony+ZV-1F"), C["may-anh"], B["sony"], sid),
            };

            // Stagger CreatedAt
            var now = DateTime.UtcNow;
            for (var i = 0; i < products.Count; i++)
                products[i].CreatedAt = now.AddHours(-i * 1.5);

            context.Set<Product>().AddRange(products);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Đã tạo {products.Count} sản phẩm");
            Console.WriteLine("🎉 Seed dữ liệu mẫu hoàn tất!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Lỗi seed: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"   Inner: {ex.InnerException.Message}");
        }
    }

    private static Product P(string name, string slug, string desc, decimal price, decimal capital, int qty, string image, int catId, int brandId, int shopId)
    {
        return new Product
        {
            Name = name, Slug = slug, Description = desc,
            Price = price, CapitalPrice = capital, Quantity = qty,
            Image = image, CategoryId = catId, BrandId = brandId, ShopId = shopId,
        };
    }
}
