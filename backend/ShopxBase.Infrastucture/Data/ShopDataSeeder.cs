using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Infrastructure.Data;

/// <summary>
/// Seeder bổ sung 120+ sản phẩm đa dạng (điện tử, thời trang, giày dép, gia dụng, …)
/// cho Shop Id = 8. Hình ảnh lấy từ nguồn public (Unsplash, picsum).
/// Mô tả chi tiết giúp AI embedding hoạt động hiệu quả hơn.
/// </summary>
public static class ShopDataSeeder
{
    private const int TargetShopId = 8;

    public static async Task SeedShop8DataAsync(IServiceProvider serviceProvider)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ShopxBaseDbContext>();

            // Kiểm tra shop tồn tại
            var shop = await context.Set<Shop>().FirstOrDefaultAsync(s => s.Id == TargetShopId && !s.IsDeleted);
            if (shop == null)
            {
                Console.WriteLine($"⚠️ Shop Id={TargetShopId} không tồn tại, bỏ qua seed ShopDataSeeder.");
                return;
            }

            // Kiểm tra đã seed chưa – nếu shop 8 đã có >= 100 sản phẩm thì bỏ qua
            var existingCount = await context.Set<Product>().CountAsync(p => p.ShopId == TargetShopId && !p.IsDeleted);
            if (existingCount >= 100)
            {
                Console.WriteLine($"⏭️ Shop {TargetShopId} đã có {existingCount} sản phẩm, bỏ qua seed.");
                return;
            }

            Console.WriteLine($"🌱 Bắt đầu seed sản phẩm cho Shop {TargetShopId}...");

            // ===== BỔ SUNG DANH MỤC MỚI =====
            var existingCatSlugs = await context.Set<Category>().Where(c => !c.IsDeleted).Select(c => c.Slug).ToListAsync();
            var newCategories = new List<Category>
            {
                new() { Name = "Áo nam", Description = "Áo thun, áo sơ mi, áo khoác nam thời trang", Slug = "ao-nam", Status = "active" },
                new() { Name = "Áo nữ", Description = "Áo thun, áo kiểu, áo khoác nữ thời trang", Slug = "ao-nu", Status = "active" },
                new() { Name = "Quần nam", Description = "Quần jeans, quần kaki, quần short nam", Slug = "quan-nam", Status = "active" },
                new() { Name = "Quần nữ", Description = "Quần jeans, chân váy, quần nữ thời trang", Slug = "quan-nu", Status = "active" },
                new() { Name = "Giày nam", Description = "Giày sneaker, giày da, giày thể thao nam", Slug = "giay-nam", Status = "active" },
                new() { Name = "Giày nữ", Description = "Giày cao gót, giày sneaker, sandal nữ", Slug = "giay-nu", Status = "active" },
                new() { Name = "Túi xách", Description = "Túi xách nữ, balo, túi đeo chéo thời trang", Slug = "tui-xach", Status = "active" },
                new() { Name = "Đồng hồ thời trang", Description = "Đồng hồ nam nữ cao cấp, đồng hồ cơ, quartz", Slug = "dong-ho-thoi-trang", Status = "active" },
                new() { Name = "Bàn phím cơ", Description = "Bàn phím cơ custom, gaming, văn phòng", Slug = "ban-phim-co", Status = "active" },
                new() { Name = "Màn hình", Description = "Màn hình máy tính gaming, đồ họa, văn phòng", Slug = "man-hinh", Status = "active" },
            };
            var catsToAdd = newCategories.Where(c => !existingCatSlugs.Contains(c.Slug)).ToList();
            if (catsToAdd.Any())
            {
                context.Set<Category>().AddRange(catsToAdd);
                await context.SaveChangesAsync();
            }
            Console.WriteLine($"✅ Danh mục mới: +{catsToAdd.Count}");

            // ===== BỔ SUNG THƯƠNG HIỆU MỚI =====
            var existingBrandSlugs = await context.Set<Brand>().Where(b => !b.IsDeleted).Select(b => b.Slug).ToListAsync();
            var newBrands = new List<Brand>
            {
                new() { Name = "Nike", Description = "Thương hiệu giày và thời trang thể thao hàng đầu thế giới từ Mỹ", Slug = "nike", Status = "active", Logo = "" },
                new() { Name = "Adidas", Description = "Thương hiệu thể thao nổi tiếng đến từ Đức", Slug = "adidas", Status = "active", Logo = "" },
                new() { Name = "Uniqlo", Description = "Thương hiệu thời trang Nhật Bản nổi tiếng với đồ cơ bản chất lượng cao", Slug = "uniqlo", Status = "active", Logo = "" },
                new() { Name = "Zara", Description = "Thương hiệu thời trang nhanh đến từ Tây Ban Nha", Slug = "zara", Status = "active", Logo = "" },
                new() { Name = "H&M", Description = "Thương hiệu thời trang Thụy Điển giá phải chăng", Slug = "hm", Status = "active", Logo = "" },
                new() { Name = "Converse", Description = "Thương hiệu giày sneaker biểu tượng với dòng Chuck Taylor", Slug = "converse", Status = "active", Logo = "" },
                new() { Name = "Puma", Description = "Thương hiệu thời trang thể thao đến từ Đức", Slug = "puma", Status = "active", Logo = "" },
                new() { Name = "New Balance", Description = "Thương hiệu giày thể thao nổi tiếng với công nghệ đệm Fresh Foam", Slug = "new-balance", Status = "active", Logo = "" },
                new() { Name = "Vans", Description = "Thương hiệu giày skateboard và streetwear biểu tượng", Slug = "vans", Status = "active", Logo = "" },
                new() { Name = "Casio", Description = "Thương hiệu đồng hồ và thiết bị điện tử Nhật Bản đáng tin cậy", Slug = "casio", Status = "active", Logo = "" },
                new() { Name = "Daniel Wellington", Description = "Thương hiệu đồng hồ thời trang minimalist từ Thụy Điển", Slug = "daniel-wellington", Status = "active", Logo = "" },
                new() { Name = "Fossil", Description = "Thương hiệu đồng hồ và phụ kiện thời trang Mỹ", Slug = "fossil", Status = "active", Logo = "" },
                new() { Name = "LG Electronics", Description = "Tập đoàn điện tử gia dụng và màn hình hàng đầu Hàn Quốc", Slug = "lg-display", Status = "active", Logo = "" },
                new() { Name = "BenQ", Description = "Thương hiệu màn hình và máy chiếu chuyên nghiệp từ Đài Loan", Slug = "benq", Status = "active", Logo = "" },
                new() { Name = "Akko", Description = "Thương hiệu bàn phím cơ custom và phụ kiện phổ biến", Slug = "akko", Status = "active", Logo = "" },
                new() { Name = "Keychron", Description = "Thương hiệu bàn phím cơ wireless cao cấp cho Mac và PC", Slug = "keychron", Status = "active", Logo = "" },
                new() { Name = "Charles & Keith", Description = "Thương hiệu giày và túi xách thời trang Singapore", Slug = "charles-keith", Status = "active", Logo = "" },
                new() { Name = "Coach", Description = "Thương hiệu túi xách và phụ kiện cao cấp từ Mỹ", Slug = "coach", Status = "active", Logo = "" },
                new() { Name = "Gucci", Description = "Thương hiệu xa xỉ hàng đầu từ Ý", Slug = "gucci", Status = "active", Logo = "" },
            };
            var brandsToAdd = newBrands.Where(b => !existingBrandSlugs.Contains(b.Slug)).ToList();
            if (brandsToAdd.Any())
            {
                context.Set<Brand>().AddRange(brandsToAdd);
                await context.SaveChangesAsync();
            }
            Console.WriteLine($"✅ Thương hiệu mới: +{brandsToAdd.Count}");

            // Load tất cả ID
            var C = await context.Set<Category>().Where(c => !c.IsDeleted).ToDictionaryAsync(c => c.Slug, c => c.Id);
            var B = await context.Set<Brand>().Where(b => !b.IsDeleted).ToDictionaryAsync(b => b.Slug, b => b.Id);
            var sid = TargetShopId;

            // ========== SẢN PHẨM (120+) ==========
            var products = new List<Product>();

            // ────────────────────────────────────────────────
            // 1. ĐIỆN THOẠI (15 sản phẩm)
            // ────────────────────────────────────────────────
            products.AddRange(new[]
            {
                P("iPhone 15 Pro Max 256GB Titan Tự Nhiên", "s8-iphone-15-pro-max-256gb",
                    "iPhone 15 Pro Max sở hữu chip A17 Pro 3nm mạnh mẽ nhất Apple từng tạo ra, hỗ trợ ray tracing phần cứng. " +
                    "Màn hình Super Retina XDR 6.7 inch, Always-On Display, Dynamic Island. Camera chính 48MP với cảm biến quad-pixel, " +
                    "camera tele periscope 5x zoom quang học – lần đầu tiên trên iPhone. Khung Titan Grade 5 siêu nhẹ, bền bỉ. " +
                    "Cổng USB-C hỗ trợ USB 3.0 truyền dữ liệu tốc độ cao. Pin cả ngày, sạc nhanh 20W, sạc không dây MagSafe 15W. " +
                    "Chống nước IP68. Bộ nhớ 256GB, có thêm phiên bản 512GB và 1TB.",
                    29990000, 26500000, 45,
                    "https://images.unsplash.com/photo-1695048133142-1a20484d2569?w=600&h=600&fit=crop",
                    C["dien-thoai"], B["apple"], sid),

                P("Samsung Galaxy S24 Ultra 512GB", "s8-samsung-galaxy-s24-ultra-512gb",
                    "Samsung Galaxy S24 Ultra trang bị chip Snapdragon 8 Gen 3 for Galaxy, hiệu năng AI mạnh mẽ với Galaxy AI tích hợp. " +
                    "Màn hình Dynamic AMOLED 2X 6.8 inch QHD+, 120Hz, độ sáng tối đa 2600 nits. Camera 200MP f/1.7 OIS, " +
                    "tele 50MP zoom quang 5x, ultrawide 12MP. Bút S-Pen tích hợp sẵn. Khung Titan bền bỉ, Gorilla Armor. " +
                    "Pin 5000mAh sạc nhanh 45W. Chống nước IP68. RAM 12GB, bộ nhớ 512GB. " +
                    "Tính năng Galaxy AI: Circle to Search, Chat Assist, Photo Assist, Live Translate.",
                    33990000, 30000000, 35,
                    "https://images.unsplash.com/photo-1707227156456-56ab2d543984?w=600&h=600&fit=crop",
                    C["dien-thoai"], B["samsung"], sid),

                P("Xiaomi 14 Pro 5G", "s8-xiaomi-14-pro-5g",
                    "Xiaomi 14 Pro trang bị chip Snapdragon 8 Gen 3, RAM 12GB LPDDR5X, bộ nhớ 256GB UFS 4.0. " +
                    "Màn hình LTPO AMOLED 6.73 inch 2K+ 120Hz, Dolby Vision. Hệ thống camera Leica Summilux: " +
                    "chính 50MP Light Fusion 900 1/1.31 inch, tele 50MP 3.2x, ultrawide 50MP. " +
                    "Pin 4880mAh sạc nhanh 120W có dây, 50W không dây. Chống nước IP68. HyperOS mượt mà.",
                    21990000, 19000000, 30,
                    "https://images.unsplash.com/photo-1598327105666-5b89351aff97?w=600&h=600&fit=crop",
                    C["dien-thoai"], B["xiaomi"], sid),

                P("Oppo Find N3 Flip", "s8-oppo-find-n3-flip",
                    "Oppo Find N3 Flip điện thoại gập kiểu vỏ sò, màn hình chính AMOLED 6.8 inch 120Hz, " +
                    "màn ngoài 3.26 inch hiển thị thông báo và chụp selfie. Chip Dimensity 9200, RAM 12GB, ROM 256GB. " +
                    "Camera Hasselblad 50MP OIS + 32MP tele 2x + 48MP ultrawide. Pin 4300mAh sạc 44W SUPERVOOC. " +
                    "Thiết kế gập bản lề Flexion, khối lượng 198g. ColorOS 13.2.",
                    19990000, 17000000, 25,
                    "https://images.unsplash.com/photo-1675453913498-b25fa1bf239e?w=600&h=600&fit=crop",
                    C["dien-thoai"], B["oppo"], sid),

                P("Google Pixel 9 Pro", "s8-google-pixel-9-pro",
                    "Google Pixel 9 Pro chip Tensor G4, RAM 16GB, bộ nhớ 128GB. Màn hình Super Actua LTPO OLED 6.3 inch " +
                    "120Hz, độ sáng 3000 nits. Camera 50MP chính, 48MP ultrawide, 48MP tele 5x zoom. " +
                    "7 năm cập nhật Android và bảo mật. Pin 4700mAh sạc 27W. Google AI tích hợp: " +
                    "Magic Eraser, Photo Unblur, Best Take, Circle to Search. Chống nước IP68, Gorilla Glass Victus 2.",
                    26990000, 24000000, 20,
                    "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=600&h=600&fit=crop",
                    C["dien-thoai"], B["google"], sid),

                P("Samsung Galaxy Z Fold 6", "s8-samsung-galaxy-z-fold-6",
                    "Samsung Galaxy Z Fold 6 điện thoại gập màn hình lớn 7.6 inch Dynamic AMOLED 2X khi mở, " +
                    "6.3 inch khi gập. Chip Snapdragon 8 Gen 3 for Galaxy. Camera 50MP + 12MP UW + 10MP tele 3x. " +
                    "RAM 12GB, ROM 256GB. Pin 4400mAh sạc 25W. Galaxy AI: lưu tóm tắt, dịch ngay, ghi chú AI. " +
                    "Armor Aluminum, Gorilla Glass Victus 2, IPX8 chống nước. Khối lượng 239g.",
                    41990000, 38000000, 15,
                    "https://images.unsplash.com/photo-1628744876497-eb30460be9f6?w=600&h=600&fit=crop",
                    C["dien-thoai"], B["samsung"], sid),

                P("iPhone 16 Pro 256GB", "s8-iphone-16-pro-256gb",
                    "iPhone 16 Pro chip A18 Pro, màn hình Super Retina XDR 6.3 inch Always-On, ProMotion 120Hz. " +
                    "Camera Fusion 48MP, ultrawide 48MP, tele 12MP 5x. Camera Control nút mới. " +
                    "Quay video 4K 120fps Dolby Vision. Apple Intelligence AI tích hợp. " +
                    "USB-C USB 3.0, WiFi 7, 5G. Pin cả ngày, sạc MagSafe 25W. Titanium Grade 5. IP68.",
                    28990000, 26000000, 40,
                    "https://images.unsplash.com/photo-1592750475338-74b7b21085ab?w=600&h=600&fit=crop",
                    C["dien-thoai"], B["apple"], sid),

                P("Xiaomi Redmi Note 13 Pro+ 5G", "s8-redmi-note-13-pro-plus-5g",
                    "Redmi Note 13 Pro+ 5G chip Dimensity 7200 Ultra, RAM 8GB, ROM 256GB. " +
                    "Màn hình CrystalRes AMOLED 6.67 inch 1.5K 120Hz, Gorilla Glass Victus. " +
                    "Camera chính 200MP OIS Samsung HP3, ultrawide 8MP, macro 2MP. " +
                    "Pin 5000mAh sạc nhanh 120W HyperCharge (19 phút đầy). Chống nước IP68. " +
                    "Stereo speakers, IR blaster, NFC. MIUI 14 / HyperOS.",
                    8990000, 7500000, 80,
                    "https://images.unsplash.com/photo-1574944985070-8f3ebc6b79d2?w=600&h=600&fit=crop",
                    C["dien-thoai"], B["xiaomi"], sid),

                P("Samsung Galaxy A55 5G 128GB", "s8-samsung-galaxy-a55-5g-128gb",
                    "Samsung Galaxy A55 5G chip Exynos 1480 Octa-core, RAM 8GB, ROM 128GB (mở rộng microSD 1TB). " +
                    "Màn hình Super AMOLED 6.6 inch FHD+ 120Hz, Vision Booster. Camera 50MP OIS + 12MP UW + 5MP Macro. " +
                    "Pin 5000mAh sạc 25W. Chống nước IP67. Samsung Knox bảo mật, 4 năm cập nhật OS. One UI 6.1.",
                    9490000, 8000000, 65,
                    "https://images.unsplash.com/photo-1610945415295-d9bbf067e59c?w=600&h=600&fit=crop",
                    C["dien-thoai"], B["samsung"], sid),

                P("Oppo Reno 12 Pro 5G", "s8-oppo-reno-12-pro-5g",
                    "Oppo Reno 12 Pro 5G chip Dimensity 9200+, RAM 12GB, ROM 256GB. " +
                    "Màn hình ProXDR AMOLED cong 6.7 inch 120Hz, 1200 nits. Camera 50MP Sony LYT-600 OIS, " +
                    "tele 50MP 2x portrait, 8MP ultrawide. Pin 5000mAh sạc 80W SUPERVOOC. " +
                    "AI Eraser, AI Clear Face, AI thông minh. NFC, stereo speakers. ColorOS 14.",
                    13990000, 12000000, 50,
                    "https://images.unsplash.com/photo-1585060544812-6b45742d762f?w=600&h=600&fit=crop",
                    C["dien-thoai"], B["oppo"], sid),

                P("Huawei Mate 60 Pro", "s8-huawei-mate-60-pro",
                    "Huawei Mate 60 Pro chip Kirin 9000S, hỗ trợ 5G. Màn hình LTPO OLED 6.82 inch 120Hz cong 4 cạnh. " +
                    "Camera XMAGE: 48MP f/1.4 variable aperture (khẩu độ thay đổi), 40MP ultrawide, 12MP tele 5x periscope. " +
                    "Pin 5000mAh sạc 88W có dây, 50W không dây. Chống nước IP68. RAM 12GB, ROM 256GB. HarmonyOS 4.",
                    26990000, 24000000, 18,
                    "https://images.unsplash.com/photo-1616348436168-de43ad0db179?w=600&h=600&fit=crop",
                    C["dien-thoai"], B["huawei"], sid),

                P("Sony Xperia 1 VI", "s8-sony-xperia-1-vi",
                    "Sony Xperia 1 VI chip Snapdragon 8 Gen 3, RAM 12GB, ROM 256GB. " +
                    "Màn hình OLED 6.5 inch FHD+ 120Hz, Bravia display, 10000:1 contrast. " +
                    "Camera Exmor T: 48MP 1/1.35 inch chính, 12MP ultrawide, 12MP tele 3.5-7.1x zoom liên tục. " +
                    "Pin 5000mAh sạc 30W. Jack 3.5mm, stereo speakers. IP68. Chụp liên tục 30fps, " +
                    "quay 4K 120fps S-Cinetone. Giao diện nhiếp ảnh chuyên nghiệp.",
                    27990000, 25000000, 12,
                    "https://images.unsplash.com/photo-1605236453806-6ff36851218e?w=600&h=600&fit=crop",
                    C["dien-thoai"], B["sony"], sid),

                P("Samsung Galaxy S24 FE", "s8-samsung-galaxy-s24-fe",
                    "Samsung Galaxy S24 FE chip Exynos 2400e, RAM 8GB, ROM 128GB. " +
                    "Màn hình Dynamic AMOLED 2X 6.7 inch FHD+ 120Hz. Camera 50MP OIS + 12MP UW + 8MP tele 3x. " +
                    "Galaxy AI đầy đủ: Circle to Search, Live Translate, Chat Assist. Pin 4700mAh sạc 25W. " +
                    "IP68 chống nước. One UI 6.1. Gorilla Glass Victus+. Phiên bản Fan Edition giá tốt.",
                    15990000, 13500000, 55,
                    "https://images.unsplash.com/photo-1610945265064-0e34e5519bbf?w=600&h=600&fit=crop",
                    C["dien-thoai"], B["samsung"], sid),

                P("Xiaomi POCO F6 Pro", "s8-xiaomi-poco-f6-pro",
                    "POCO F6 Pro chip Snapdragon 8 Gen 2, RAM 12GB LPDDR5X, ROM 256GB UFS 4.0. " +
                    "Màn hình Flow AMOLED 6.67 inch 1.5K 120Hz, 4000 nits HBM. Camera 50MP OIS Light Fusion 800, " +
                    "8MP ultrawide, 2MP macro. Pin 5000mAh sạc 120W (19 phút đầy). " +
                    "LiquidCool 4.0 tản nhiệt, stereo Dolby Atmos. NFC, IR blaster. HyperOS. Giá flagship killer.",
                    12990000, 11000000, 40,
                    "https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?w=600&h=600&fit=crop",
                    C["dien-thoai"], B["xiaomi"], sid),

                P("iPhone SE 4 128GB", "s8-iphone-se-4-128gb",
                    "iPhone SE 4 (2025) chip A18, màn hình OLED 6.1 inch, Face ID lần đầu trên SE. " +
                    "Camera 48MP hệ thống Fusion, quay 4K Dolby Vision. Apple Intelligence AI. " +
                    "USB-C, 5G mmWave. Pin cả ngày, MagSafe. Thiết kế giống iPhone 16 nhưng giá phải chăng. " +
                    "RAM 8GB, ROM 128GB. iOS 18. Lựa chọn tốt nhất phân khúc tầm trung Apple.",
                    12490000, 10500000, 60,
                    "https://images.unsplash.com/photo-1591337676887-a217a6c949e8?w=600&h=600&fit=crop",
                    C["dien-thoai"], B["apple"], sid),
            });

            // ────────────────────────────────────────────────
            // 2. LAPTOP (12 sản phẩm)
            // ────────────────────────────────────────────────
            products.AddRange(new[]
            {
                P("MacBook Pro 16 M3 Max", "s8-macbook-pro-16-m3-max",
                    "MacBook Pro 16 inch chip M3 Max, 14-core CPU, 30-core GPU, 36GB unified memory. " +
                    "Màn hình Liquid Retina XDR 16.2 inch, 3456x2234, ProMotion 120Hz, 1600 nits HDR. " +
                    "SSD 1TB cực nhanh. Pin 22 giờ. 3x Thunderbolt 4, HDMI 2.1, SDXC, MagSafe 3. " +
                    "6 loa hệ thống, Spatial Audio. macOS Sonoma. Hoàn hảo cho render video 8K, AI/ML, code nặng.",
                    74990000, 68000000, 10,
                    "https://images.unsplash.com/photo-1517336714731-489689fd1ca8?w=600&h=600&fit=crop",
                    C["laptop"], B["apple"], sid),

                P("Dell XPS 14 2024", "s8-dell-xps-14-2024",
                    "Dell XPS 14 Core Ultra 7 155H, Intel Arc GPU, RAM 16GB LPDDR5X, SSD 512GB. " +
                    "Màn hình 14.5 inch 2560x1600 IPS 120Hz, 500 nits. Thiết kế InfinityEdge tràn viền. " +
                    "Bàn phím zero-lattice mới, touchpad lớn haptic. Pin 69.5Wh ~13 giờ. " +
                    "Thunderbolt 4 x2, USB-C. Khối lượng 1.46kg. Windows 11 Pro. Copilot AI tích hợp.",
                    35990000, 32000000, 15,
                    "https://images.unsplash.com/photo-1593642632559-0c6d3fc62b89?w=600&h=600&fit=crop",
                    C["laptop"], B["dell"], sid),

                P("ASUS ROG Zephyrus G14 2024", "s8-asus-rog-zephyrus-g14-2024",
                    "ASUS ROG Zephyrus G14 Ryzen 9 8945HS, NVIDIA RTX 4060 8GB, RAM 16GB DDR5. " +
                    "Màn hình ROG Nebula OLED 14 inch 2.8K 120Hz, 100% DCI-P3, 500 nits. SSD 1TB PCIe 4.0. " +
                    "Pin 73Wh. AniMe Matrix LED nắp lưng tùy chỉnh. Khối lượng chỉ 1.5kg. " +
                    "Tản nhiệt ROG Intelligent Cooling. WiFi 7, USB-C PD 100W. Windows 11. " +
                    "Gaming laptop mỏng nhẹ nhất phân khúc.",
                    39990000, 36000000, 12,
                    "https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?w=600&h=600&fit=crop",
                    C["laptop"], B["asus"], sid),

                P("Lenovo ThinkPad X1 Carbon Gen 12", "s8-thinkpad-x1-carbon-gen12",
                    "ThinkPad X1 Carbon Gen 12 Intel Core Ultra 7 165U vPro, RAM 32GB LPDDR5X, SSD 1TB. " +
                    "Màn hình 14 inch 2.8K OLED HDR, 100% DCI-P3, 400 nits. Khối lượng chỉ 1.09kg. " +
                    "TrackPoint + touchpad kính. Bảo mật: IR camera, fingerprint, dTPM 2.0, Kensington. " +
                    "Pin 57Wh ~15 giờ, sạc nhanh 65W. 2x Thunderbolt 4, 2x USB-A, HDMI 2.1. WiFi 7. " +
                    "MIL-STD-810H. Laptop doanh nhân cao cấp nhất.",
                    45990000, 42000000, 8,
                    "https://images.unsplash.com/photo-1588872657578-7efd1f1555ed?w=600&h=600&fit=crop",
                    C["laptop"], B["lenovo"], sid),

                P("HP Spectre x360 14", "s8-hp-spectre-x360-14",
                    "HP Spectre x360 14 Core Ultra 7, RAM 16GB, SSD 1TB. Màn hình 14 inch 2.8K OLED touch 120Hz. " +
                    "Xoay 360° 2-in-1 với bút HP MPP 2.0. Pin 68Wh ~16 giờ. Thunderbolt 4 x2, USB-A. " +
                    "Webcam 9MP với AI auto-framing. Loa B&O quad speakers. Khối lượng 1.34kg. " +
                    "Thiết kế gem-cut cao cấp, CNC aluminum. Windows 11 Pro Copilot+.",
                    38990000, 35000000, 12,
                    "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=600&h=600&fit=crop",
                    C["laptop"], B["hp"], sid),

                P("MSI Titan 18 HX", "s8-msi-titan-18-hx",
                    "MSI Titan 18 HX Intel Core i9-14900HX, NVIDIA RTX 4090 16GB, RAM 64GB DDR5. " +
                    "Màn hình Mini LED 18 inch UHD+ 120Hz, 1000 nits, 100% DCI-P3. SSD 2TB NVMe Gen 5. " +
                    "Bàn phím Cherry MX cơ per-key RGB. 4 loa Dynaudio 2W+2W woofer. " +
                    "Tản nhiệt Cooler Boost Titan, 4 quạt, 7 ống đồng. Pin 99.9Wh. MST-Hub 4K multi-display. " +
                    "Laptop gaming mạnh nhất thế giới.",
                    89990000, 82000000, 5,
                    "https://images.unsplash.com/photo-1603302576837-37561b2e2302?w=600&h=600&fit=crop",
                    C["laptop"], B["msi"], sid),

                P("MacBook Air M3 15 inch", "s8-macbook-air-m3-15",
                    "MacBook Air 15 inch chip M3 8-core CPU, 10-core GPU, 8GB unified memory, SSD 256GB. " +
                    "Màn hình Liquid Retina 15.3 inch, 2880x1864, 500 nits, P3 wide color. " +
                    "Pin 18 giờ. MagSafe 3, 2x Thunderbolt, 3.5mm jack. 6 loa, Spatial Audio. " +
                    "Webcam 1080p. Khối lượng 1.51kg, mỏng 11.5mm. Fanless hoàn toàn yên tĩnh. macOS Sequoia.",
                    32990000, 29000000, 25,
                    "https://images.unsplash.com/photo-1611186871348-b1ce696e52c9?w=600&h=600&fit=crop",
                    C["laptop"], B["apple"], sid),

                P("ASUS Zenbook 14 OLED UX3405", "s8-asus-zenbook-14-oled",
                    "ASUS Zenbook 14 OLED Core Ultra 7 155U, RAM 16GB, SSD 512GB. " +
                    "Màn hình 14 inch 2.8K OLED 120Hz, 600 nits, 100% DCI-P3, Pantone Validated. " +
                    "Khối lượng 1.2kg siêu nhẹ. Pin 75Wh ~16 giờ. Thunderbolt 4, USB-A, HDMI 2.1. " +
                    "Harman Kardon speakers, Dolby Atmos. ErgoLift hinge. Intel AI Boost NPU. Windows 11.",
                    24990000, 22000000, 20,
                    "https://images.unsplash.com/photo-1541807084-5c52b6b3adef?w=600&h=600&fit=crop",
                    C["laptop"], B["asus"], sid),

                P("Lenovo Legion Pro 7i Gen 9", "s8-lenovo-legion-pro-7i-gen9",
                    "Lenovo Legion Pro 7i Core i9-14900HX, RTX 4080 12GB, RAM 32GB DDR5 5600MHz. " +
                    "Màn hình 16 inch 2.5K IPS 240Hz, 100% sRGB, 500 nits, G-SYNC. SSD 1TB Gen 4. " +
                    "Tản nhiệt Legion ColdFront 5.0, 2 quạt 87 cánh. Bàn phím TrueStrike per-key RGB. " +
                    "Pin 99.99Wh, sạc 330W. Thunderbolt 4, USB-C PD. Nahimic Audio. Windows 11.",
                    52990000, 48000000, 8,
                    "https://images.unsplash.com/photo-1593642702821-c8da6771f0c6?w=600&h=600&fit=crop",
                    C["laptop"], B["lenovo"], sid),

                P("Dell Inspiron 16 Plus 7640", "s8-dell-inspiron-16-plus-7640",
                    "Dell Inspiron 16 Plus Core Ultra 7 155H, RAM 16GB DDR5, SSD 512GB. " +
                    "Màn hình 16 inch 2.5K IPS 120Hz, 100% sRGB, 350 nits. Intel Arc GPU tích hợp. " +
                    "Pin 86Wh ~12 giờ. Type-C, HDMI 1.4, USB-A x2, SD card. Khối lượng 1.87kg. " +
                    "Bàn phím full-size backlit. ComfortView Plus giảm ánh sáng xanh. Windows 11.",
                    22990000, 20000000, 18,
                    "https://images.unsplash.com/photo-1530893609608-32a9af3aa95c?w=600&h=600&fit=crop",
                    C["laptop"], B["dell"], sid),

                P("HP OMEN 16 2024", "s8-hp-omen-16-2024",
                    "HP OMEN 16 AMD Ryzen 9 8945HS, RTX 4070 8GB, RAM 16GB DDR5, SSD 1TB. " +
                    "Màn hình 16.1 inch QHD 240Hz, 100% sRGB, 300 nits. OMEN Tempest Cooling tản nhiệt. " +
                    "Bàn phím 26-key rollover RGB per-key. Pin 83Wh. WiFi 7, Thunderbolt 4. " +
                    "B&O speakers, DTS:X Ultra. OMEN Gaming Hub tối ưu. Windows 11.",
                    36990000, 33000000, 10,
                    "https://images.unsplash.com/photo-1587614382346-4ec70e388b28?w=600&h=600&fit=crop",
                    C["laptop"], B["hp"], sid),

                P("ASUS TUF Gaming A16 2024", "s8-asus-tuf-gaming-a16-2024",
                    "ASUS TUF A16 Ryzen 7 8845HS, RTX 4060 8GB, RAM 16GB DDR5, SSD 512GB. " +
                    "Màn hình 16 inch FHD+ 165Hz, Adaptive-Sync. Chuẩn quân đội MIL-STD-810H. " +
                    "Tản nhiệt Arc Flow Fans kép. Bàn phím RGB 4-zone. Pin 90Wh. " +
                    "USB-C PD, HDMI 2.1, RJ45 Ethernet. Dolby Atmos, Hi-Res Audio. Windows 11.",
                    26990000, 24000000, 22,
                    "https://images.unsplash.com/photo-1618424181497-157f25b6ddd5?w=600&h=600&fit=crop",
                    C["laptop"], B["asus"], sid),
            });

            // ────────────────────────────────────────────────
            // 3. TAI NGHE (10 sản phẩm)
            // ────────────────────────────────────────────────
            products.AddRange(new[]
            {
                P("AirPods Pro 2 USB-C (2024)", "s8-airpods-pro-2-usbc-2024",
                    "AirPods Pro 2 chip H2, chống ồn chủ động ANC 2x hiệu quả hơn thế hệ trước. " +
                    "Adaptive Audio tự điều chỉnh theo môi trường. Conversation Awareness giảm volume khi nói chuyện. " +
                    "Spatial Audio cá nhân hóa theo hình tai. Case sạc USB-C tích hợp loa tìm kiếm, " +
                    "sạc Apple Watch, khắc laser miễn phí. Pin 6 giờ ANC, 30 giờ với case. IP54 chống bụi nước.",
                    6190000, 5300000, 70,
                    "https://images.unsplash.com/photo-1588423771073-b8903fde1c68?w=600&h=600&fit=crop",
                    C["tai-nghe"], B["apple"], sid),

                P("Sony WH-1000XM5 Wireless", "s8-sony-wh-1000xm5",
                    "Sony WH-1000XM5 tai nghe over-ear chống ồn số 1 thế giới. 8 microphone + chip V1 xử lý ANC. " +
                    "Driver 30mm carbon fiber composite. Codec LDAC Hi-Res 990kbps, DSEE Extreme AI upscale. " +
                    "Multipoint kết nối 2 thiết bị. Pin 30 giờ, sạc 3 phút nghe 3 giờ. " +
                    "Speak-to-Chat, Adaptive Sound Control. Gập phẳng, nhẹ 250g. Jack 3.5mm kèm theo.",
                    7990000, 6800000, 35,
                    "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?w=600&h=600&fit=crop",
                    C["tai-nghe"], B["sony"], sid),

                P("Samsung Galaxy Buds3 Pro", "s8-samsung-galaxy-buds3-pro",
                    "Galaxy Buds3 Pro thiết kế stem trong suốt mới. ANC chủ động 2-level, Ambient Sound. " +
                    "Driver 2-way (10.5mm woofer + 6.1mm tweeter). Codec SSC HiFi 24bit, 360 Audio head tracking. " +
                    "IP57 chống nước. Galaxy AI: Interpreter (dịch real-time), Voice Detect. " +
                    "Pin 7 giờ ANC, 30 giờ với case. Sạc không dây Qi. Blade Lights LED.",
                    5490000, 4500000, 50,
                    "https://images.unsplash.com/photo-1590658268037-6bf12f032f55?w=600&h=600&fit=crop",
                    C["tai-nghe"], B["samsung"], sid),

                P("JBL Tour One M2", "s8-jbl-tour-one-m2",
                    "JBL Tour One M2 tai nghe over-ear cao cấp, True Adaptive ANC theo thời gian thực. " +
                    "Driver 40mm, Hi-Res Audio certified, JBL Spatial Sound. " +
                    "Pin 50 giờ (ANC bật), sạc nhanh 10 phút = 5 giờ. Multipoint Bluetooth 5.3. " +
                    "4 micro beamforming cho đàm thoại rõ ràng. Gập phẳng, túi đựng cao cấp. App JBL Headphones tùy chỉnh EQ.",
                    5990000, 5000000, 25,
                    "https://images.unsplash.com/photo-1583394838336-acd977736f90?w=600&h=600&fit=crop",
                    C["tai-nghe"], B["jbl"], sid),

                P("Bose QuietComfort Ultra Earbuds", "s8-bose-qc-ultra-earbuds",
                    "Bose QC Ultra Earbuds chống ồn tốt nhất dòng TWS. Immersive Audio spatial sound cá nhân. " +
                    "CustomTune hiệu chỉnh âm thanh theo ống tai. Driver 9.3mm, codec aptX Adaptive, Qualcomm S5 Gen 2. " +
                    "Pin 6 giờ, 24 giờ với case. IPX4 chống mồ hôi. 3 chế độ: Quiet, Aware, Immersive. " +
                    "Touch control tùy chỉnh qua Bose Music app. Fit tips 3 kích cỡ.",
                    7490000, 6500000, 20,
                    "https://images.unsplash.com/photo-1606220588913-b3aacb4d2f46?w=600&h=600&fit=crop",
                    C["tai-nghe"], B["bose"], sid),

                P("Sony LinkBuds S", "s8-sony-linkbuds-s",
                    "Sony LinkBuds S TWS siêu nhẹ 4.8g mỗi bên, chống ồn LDAC Hi-Res. " +
                    "Driver 5mm, chip V1, DSEE Extreme. Speak-to-Chat, Adaptive Sound Control tự động. " +
                    "Multipoint kết nối 2 thiết bị. Pin 6 giờ ANC, 20 giờ với case. IPX4. " +
                    "Sony Headphones Connect app EQ tùy chỉnh. 360 Reality Audio.",
                    3490000, 2800000, 45,
                    "https://images.unsplash.com/photo-1631867934509-e2ec1fd7e3db?w=600&h=600&fit=crop",
                    C["tai-nghe"], B["sony"], sid),

                P("Razer Blackshark V2 Pro 2023", "s8-razer-blackshark-v2-pro-2023",
                    "Razer BlackShark V2 Pro tai nghe gaming wireless, TriForce Titanium 50mm driver. " +
                    "THX Spatial Audio 7.1 surround, chế độ FPS/Music. HyperClear Super Wideband mic có thể tháo. " +
                    "HyperSpeed Wireless < 2ms latency. Pin 70 giờ. Đệm tai FlowKnit memory foam thoáng khí. " +
                    "Bluetooth 5.2 + USB-C dongle. Khối lượng 320g. Synapse 3 EQ 10 band.",
                    4290000, 3600000, 30,
                    "https://images.unsplash.com/photo-1599669454699-248893623440?w=600&h=600&fit=crop",
                    C["tai-nghe"], B["razer"], sid),

                P("JBL Tune Flex", "s8-jbl-tune-flex",
                    "JBL Tune Flex TWS 2 chế độ đeo: in-ear và open-ear. ANC chủ động + Ambient Aware. " +
                    "Driver 12.4mm JBL Pure Bass, Bluetooth 5.3. Pin 8 giờ, 32 giờ với case. " +
                    "4 mic cho đàm thoại. Chống nước IPX4. JBL Headphones app EQ tùy chỉnh. " +
                    "Smart Ambient giữ kết nối với môi trường. Giá cực tốt cho tính năng đa dạng.",
                    1790000, 1400000, 80,
                    "https://images.unsplash.com/photo-1572569511254-d8f925fe2cbb?w=600&h=600&fit=crop",
                    C["tai-nghe"], B["jbl"], sid),

                P("Logitech G Pro X 2 Lightspeed", "s8-logitech-g-pro-x-2-lightspeed",
                    "Logitech G PRO X 2 tai nghe gaming wireless, driver graphene 50mm mới. " +
                    "LIGHTSPEED wireless + Bluetooth. DTS Headphone:X 2.0. Blue VO!CE microphone có thể tháo. " +
                    "Pin 50 giờ. Đệm tai hybrid memory foam + leatherette. Khối lượng 309g. " +
                    "G HUB EQ tùy chỉnh, profile lưu trên headset. USB-C sạc. Chứng nhận PRO esports.",
                    4990000, 4200000, 20,
                    "https://images.unsplash.com/photo-1558756520-22cfe5d382ca?w=600&h=600&fit=crop",
                    C["tai-nghe"], B["logitech"], sid),

                P("Anker Soundcore Space A40", "s8-anker-soundcore-space-a40",
                    "Anker Soundcore Space A40 TWS siêu nhỏ, ANC chủ động, LDAC Hi-Res. " +
                    "Driver 10mm custom, 6 mic đàm thoại AI. Multipoint 2 thiết bị. " +
                    "Pin 10 giờ ANC, 50 giờ tổng với case. Case sạc không dây Qi. IPX4. " +
                    "Soundcore app 22 preset EQ, HearID cá nhân hóa âm thanh. " +
                    "Giá rẻ nhất dòng TWS ANC LDAC, chất lượng vượt tầm giá.",
                    1690000, 1200000, 100,
                    "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=600&h=600&fit=crop",
                    C["tai-nghe"], B["anker"], sid),
            });

            // ────────────────────────────────────────────────
            // 4. ÁO NAM (10 sản phẩm)
            // ────────────────────────────────────────────────
            products.AddRange(new[]
            {
                P("Nike Dri-FIT Training T-shirt", "s8-nike-dri-fit-training-tee",
                    "Áo thun tập luyện Nike Dri-FIT nam, chất liệu polyester thoáng khí nhanh khô, công nghệ Dri-FIT " +
                    "hút ẩm đổ mồ hôi ra bề mặt vải. Cổ tròn, form regular fit thoải mái vận động. " +
                    "Logo Nike Swoosh in ngực trái. Phù hợp gym, chạy bộ, thể thao. Size S-2XL. Màu đen.",
                    890000, 550000, 120,
                    "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=600&h=600&fit=crop",
                    C["ao-nam"], B["nike"], sid),

                P("Adidas Essentials 3-Stripes Tee", "s8-adidas-essentials-3-stripes-tee",
                    "Áo thun nam Adidas Essentials 3-Stripes cổ điển, cotton jersey mềm mại thoải mái. " +
                    "3 sọc kẻ đặc trưng Adidas vai xuống tay áo. Cổ tròn ribbed, form regular fit. " +
                    "Logo Adidas Badge of Sport ngực trái. 100% cotton. Size S-3XL. Nhiều màu sắc.",
                    750000, 450000, 150,
                    "https://images.unsplash.com/photo-1581655353564-df123a1eb820?w=600&h=600&fit=crop",
                    C["ao-nam"], B["adidas"], sid),

                P("Uniqlo AIRism Cotton Crew Neck", "s8-uniqlo-airism-cotton-crew",
                    "Áo thun nam Uniqlo AIRism Cotton Crew Neck, kết hợp cotton mặt ngoài mềm mại với " +
                    "lớp lót AIRism bên trong thoáng mát, khô ráo. Công nghệ DRY, chống tia UV. " +
                    "Form regular fit. Đường may phẳng giảm cọ xát. Size XS-3XL. " +
                    "Phù hợp mặc hàng ngày, layering, đi làm. Sản xuất tại Việt Nam.",
                    390000, 200000, 200,
                    "https://images.unsplash.com/photo-1622445275576-721325763afe?w=600&h=600&fit=crop",
                    C["ao-nam"], B["uniqlo"], sid),

                P("Zara Linen Blend Shirt", "s8-zara-linen-blend-shirt",
                    "Áo sơ mi nam Zara chất liệu pha linen (vải lanh), thoáng mát mùa hè. " +
                    "Cổ đứng mandarin collar, nút gỗ tự nhiên. Form relaxed fit, tay dài có thể xắn lên. " +
                    "Phù hợp đi biển, đi chơi, smart casual. Size S-XL. Màu trắng kem.",
                    1290000, 800000, 60,
                    "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=600&h=600&fit=crop",
                    C["ao-nam"], B["zara"], sid),

                P("H&M Regular Fit Oxford Shirt", "s8-hm-regular-fit-oxford-shirt",
                    "Áo sơ mi Oxford nam H&M, vải cotton dệt Oxford dày dặn bền bỉ. " +
                    "Cổ button-down, túi ngực trái, gấu áo cong. Form regular fit công sở. " +
                    "Cúc nhựa tone-on-tone. Phù hợp đi làm, đi học, lịch sự. Size XS-2XL. " +
                    "Nhiều màu: trắng, xanh nhạt, hồng nhạt, sọc.",
                    590000, 350000, 100,
                    "https://images.unsplash.com/photo-1602810318383-e386cc2a3ccf?w=600&h=600&fit=crop",
                    C["ao-nam"], B["hm"], sid),

                P("Nike Sportswear Club Fleece Hoodie", "s8-nike-club-fleece-hoodie",
                    "Áo hoodie Nike Sportswear Club Fleece nam, chất liệu French Terry cotton pha polyester " +
                    "nỉ lông mề mịn bên trong ấm áp. Mũ trùm điều chỉnh dây rút, túi kangaroo phía trước. " +
                    "Logo Nike Futura ngực trái. Form regular fit. Bó gấu tay và gấu áo ribbed. " +
                    "Phù hợp mùa lạnh, thể thao, streetwear. Size S-3XL.",
                    1590000, 1000000, 80,
                    "https://images.unsplash.com/photo-1556821840-3a63f95609a7?w=600&h=600&fit=crop",
                    C["ao-nam"], B["nike"], sid),

                P("Adidas Tiro 24 Training Jacket", "s8-adidas-tiro-24-training-jacket",
                    "Áo khoác thể thao nam Adidas Tiro 24, chất polyester recycled AEROREADY thoáng khí. " +
                    "Khóa kéo full-length, 2 túi khóa kéo. 3 sọc Adidas dọc tay áo. " +
                    "Cổ đứng, bo gấu tay và eo. Form slim fit ôm cơ thể. " +
                    "Phù hợp tập luyện, warm-up, athleisure. Size S-2XL. Primegreen chất liệu tái chế.",
                    1390000, 900000, 60,
                    "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?w=600&h=600&fit=crop",
                    C["ao-nam"], B["adidas"], sid),

                P("Uniqlo Ultra Light Down Jacket", "s8-uniqlo-ultra-light-down-jacket",
                    "Áo khoác lông vũ siêu nhẹ Uniqlo nam, lông vũ 90% down / 10% feather, 640 fill power. " +
                    "Khối lượng chỉ 230g, gấp gọn vào túi đựng kèm theo. Lớp ngoài nylon chống nước nhẹ. " +
                    "Khóa kéo YKK, 2 túi tay + 1 túi trong. Form regular. " +
                    "Giữ ấm -5°C đến 10°C. Phù hợp du lịch, đi công tác, layering. Size XS-3XL.",
                    1490000, 950000, 70,
                    "https://images.unsplash.com/photo-1544923246-77307dd270b5?w=600&h=600&fit=crop",
                    C["ao-nam"], B["uniqlo"], sid),

                P("Puma Essentials Logo Tee", "s8-puma-essentials-logo-tee",
                    "Áo thun nam Puma Essentials Big Logo, cotton 100% mềm mại. Logo Puma Cat lớn in ngực. " +
                    "Cổ tròn ribbed, form regular fit. Size S-2XL. Nhiều màu: đen, trắng, navy, đỏ. " +
                    "Phù hợp mặc hàng ngày, tập gym, phối streetwear. Giặt máy được.",
                    590000, 350000, 130,
                    "https://images.unsplash.com/photo-1576566588028-4147f3842f27?w=600&h=600&fit=crop",
                    C["ao-nam"], B["puma"], sid),

                P("Zara Oversized Bomber Jacket", "s8-zara-oversized-bomber-jacket",
                    "Áo khoác bomber Zara nam form oversized, chất vải kaki cotton blend dày dặn. " +
                    "Cổ bomber ribbed, khóa kéo kim loại mạ vàng. 2 túi hông nắp đậy, 1 túi tay trái. " +
                    "Lót satin bên trong. Gấu tay và eo bo ribbed. Phù hợp streetwear, " +
                    "smart casual, phối cùng jeans và sneaker. Size S-XL. Màu xanh rêu olive.",
                    1890000, 1200000, 40,
                    "https://images.unsplash.com/photo-1551028719-00167b16eac5?w=600&h=600&fit=crop",
                    C["ao-nam"], B["zara"], sid),
            });

            // ────────────────────────────────────────────────
            // 5. ÁO NỮ (8 sản phẩm)
            // ────────────────────────────────────────────────
            products.AddRange(new[]
            {
                P("Nike Sportswear Essential Crop Tee", "s8-nike-essential-crop-tee-w",
                    "Áo thun croptop nữ Nike Sportswear Essential, cotton 100% mềm mại thoáng mát. " +
                    "Form boxy crop – ngắn trên eo, thoải mái không bó. Logo Nike Swoosh ngực trái. " +
                    "Cổ tròn ribbed. Size XS-XL. Phù hợp phối high-waist jeans, skirt, jogger. " +
                    "Nhiều màu: đen, trắng, hồng, xanh pastel.",
                    790000, 480000, 100,
                    "https://images.unsplash.com/photo-1503342217505-b0a15ec3261c?w=600&h=600&fit=crop",
                    C["ao-nu"], B["nike"], sid),

                P("Adidas Own The Run Tee Women", "s8-adidas-own-the-run-tee-w",
                    "Áo thun chạy bộ nữ Adidas Own The Run, chất liệu recycled polyester AEROREADY " +
                    "hút ẩm nhanh khô. Phản quang 3M an toàn chạy ban đêm. Form regular fit. " +
                    "3 sọc Adidas vai. Size XS-XL. Phù hợp chạy bộ, gym, yoga.",
                    690000, 420000, 90,
                    "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?w=600&h=600&fit=crop",
                    C["ao-nu"], B["adidas"], sid),

                P("Uniqlo Mercerized Cotton Blouse", "s8-uniqlo-mercerized-cotton-blouse",
                    "Áo kiểu nữ Uniqlo Mercerized Cotton, chất cotton lụa qua xử lý mercerize bóng mượt sang trọng. " +
                    "Cổ V thanh lịch, phom rộng thoải mái che khuyết điểm. Tay ngắn. " +
                    "Phù hợp mặc đi làm, đi chơi, phối blazer. Size XS-XL. Màu trắng, be, đen.",
                    590000, 350000, 80,
                    "https://images.unsplash.com/photo-1485462537746-965f33f7f6a7?w=600&h=600&fit=crop",
                    C["ao-nu"], B["uniqlo"], sid),

                P("Zara Satin Wrap Top", "s8-zara-satin-wrap-top-w",
                    "Áo kiểu nữ Zara chất satin mềm mại rủ, thiết kế wrap (đắp chéo) nữ tính. " +
                    "Tay dài phồng nhẹ, cổ V quyến rũ, thắt nơ bên hông điều chỉnh eo. " +
                    "Phù hợp mặc đi tiệc, hẹn hò, đi làm thanh lịch. Size S-L. Màu đen, emerald green.",
                    1090000, 700000, 55,
                    "https://images.unsplash.com/photo-1551803091-e20673f15770?w=600&h=600&fit=crop",
                    C["ao-nu"], B["zara"], sid),

                P("H&M Puff-Sleeve Blouse", "s8-hm-puff-sleeve-blouse-w",
                    "Áo kiểu nữ H&M tay phồng (puff sleeve) thời trang, chất cotton poplin dày dặn. " +
                    "Cổ tròn, nút bọc vải phía trước, gấu áo peplum nhẹ. Form regular fit. " +
                    "Phù hợp đi làm, đi chơi, phối với quần jeans hoặc chân váy. Size XS-XL. Màu trắng.",
                    490000, 280000, 90,
                    "https://images.unsplash.com/photo-1594938298603-c8148c4dae35?w=600&h=600&fit=crop",
                    C["ao-nu"], B["hm"], sid),

                P("Nike Dri-FIT Yoga Tank Top", "s8-nike-dri-fit-yoga-tank-w",
                    "Áo tank top yoga nữ Nike Dri-FIT, chất liệu Infinalon siêu mềm mượt, co giãn 4 chiều. " +
                    "Dri-FIT hút ẩm thoáng khí. Lưng racer-back khoét rộng thoải mái vận động. " +
                    "Logo Nike nhỏ. Form slim fit ôm nhẹ. Size XS-XL. Phù hợp yoga, pilates, gym.",
                    690000, 420000, 70,
                    "https://images.unsplash.com/photo-1518310383802-640c2de311b2?w=600&h=600&fit=crop",
                    C["ao-nu"], B["nike"], sid),

                P("Adidas Adicolor Classics Hoodie Women", "s8-adidas-adicolor-hoodie-w",
                    "Áo hoodie nữ Adidas Adicolor Classics Trefoil, cotton French Terry ấm mịn. " +
                    "Logo Trefoil thêu ngực. Mũ trùm dây rút, túi kangaroo. Bo gấu tay và eo. " +
                    "Form regular fit. Size XS-XL. Phù hợp streetwear, đi học, phối oversized. " +
                    "Nhiều màu: đen, hồng, xanh dương nhạt, trắng sữa.",
                    1290000, 850000, 55,
                    "https://images.unsplash.com/photo-1578681994506-b8f463449011?w=600&h=600&fit=crop",
                    C["ao-nu"], B["adidas"], sid),

                P("Uniqlo Ultra Stretch Leggings Pants", "s8-uniqlo-ultra-stretch-leggings",
                    "Quần leggings nữ Uniqlo Ultra Stretch, co giãn cực tốt như mặc quần tập nhưng vẫn thanh lịch. " +
                    "Vải DRY-EX thoáng khí nhanh khô. Cạp cao che bụng, pocket giả 2 bên. " +
                    "Phù hợp đi làm, đi chơi, tập nhẹ. Size XS-XL. Màu đen, navy.",
                    490000, 280000, 100,
                    "https://images.unsplash.com/photo-1506629082955-511b1aa562c8?w=600&h=600&fit=crop",
                    C["ao-nu"], B["uniqlo"], sid),
            });

            // ────────────────────────────────────────────────
            // 6. GIÀY NAM (10 sản phẩm)
            // ────────────────────────────────────────────────
            products.AddRange(new[]
            {
                P("Nike Air Force 1 '07 White", "s8-nike-air-force-1-07-white",
                    "Nike Air Force 1 '07 phiên bản iconic kinh điển, da tổng hợp premium trắng toàn bộ. " +
                    "Đế Air sole đệm êm, outsole cao su waffle bám tốt. Cổ thấp, lưỡi gà đệm foam. " +
                    "Phù hợp streetwear, casual, phối đa phong cách. Size 39-45. Unisex nhưng form nam.",
                    2690000, 1800000, 60,
                    "https://images.unsplash.com/photo-1600269452121-4f2416e55c28?w=600&h=600&fit=crop",
                    C["giay-nam"], B["nike"], sid),

                P("Adidas Ultraboost Light", "s8-adidas-ultraboost-light",
                    "Adidas Ultraboost Light giày chạy bộ nam, LIGHT BOOST đệm nhẹ hơn 30% so với Boost cũ mà vẫn " +
                    "hoàn trả năng lượng tuyệt vời. Upper Primeknit+ thoáng khí adaptive fit. " +
                    "Continental Rubber outsole bám đường ướt. Torsion System ổn định giữa bàn chân. " +
                    "LEG (Linear Energy Push) boost trả lực. Size 39-46. Parley Ocean Plastic chất liệu tái chế.",
                    4290000, 3200000, 35,
                    "https://images.unsplash.com/photo-1556906781-9a412961c28c?w=600&h=600&fit=crop",
                    C["giay-nam"], B["adidas"], sid),

                P("Converse Chuck Taylor All Star Classic", "s8-converse-chuck-taylor-classic",
                    "Converse Chuck Taylor All Star Classic cổ cao, canvas dày dặn. " +
                    "Đế vulcanized rubber truyền thống, toe cap cao su đặc trưng. " +
                    "All Star patch mắt cá chân. Lót OrthoLite êm chân. " +
                    "Biểu tượng sneaker văn hóa từ 1917. Size 36-45. Màu đen/trắng kinh điển. Unisex.",
                    1590000, 1000000, 80,
                    "https://images.unsplash.com/photo-1607522370275-f14206abe5d3?w=600&h=600&fit=crop",
                    C["giay-nam"], B["converse"], sid),

                P("New Balance 574 Core", "s8-new-balance-574-core",
                    "New Balance 574 Core đôi giày retro running biểu tượng, upper da suede + mesh thoáng khí. " +
                    "Đệm ENCAP: EVA mềm bao quanh PU bền bỉ, ổn định. Outsole cao su bền grip tốt. " +
                    "Logo N lớn bên hông. Lót NB Comfort Insert. Size 39-45. " +
                    "Nhiều phối màu classic: grey, navy, black. Sneaker lifestyle hàng ngày.",
                    2390000, 1600000, 50,
                    "https://images.unsplash.com/photo-1539185441755-769473a23570?w=600&h=600&fit=crop",
                    C["giay-nam"], B["new-balance"], sid),

                P("Vans Old Skool Black/White", "s8-vans-old-skool-black-white",
                    "Vans Old Skool phiên bản kinh điển, sọc Jazz Stripe bên hông đặc trưng. " +
                    "Upper canvas + da suede bền bỉ. Đế waffle rubber của Vans grip tốt. " +
                    "Lót UltraCush mới êm hơn. Cổ thấp, dây buộc. Size 36-45. " +
                    "Biểu tượng skate culture và streetwear từ 1977. Unisex.",
                    1890000, 1200000, 70,
                    "https://images.unsplash.com/photo-1525966222134-fcfa99b8ae77?w=600&h=600&fit=crop",
                    C["giay-nam"], B["vans"], sid),

                P("Nike Air Jordan 1 Low", "s8-nike-air-jordan-1-low",
                    "Nike Air Jordan 1 Low phiên bản cổ thấp của đôi giày huyền thoại Michael Jordan. " +
                    "Upper da tổng hợp + da thật premium. Đệm Air-Sole gót êm ái bật nhảy. " +
                    "Wings logo, Swoosh bên hông. Outsole cao su bám sân. Size 39-45. " +
                    "Đa dạng colorway. Phù hợp streetwear, casual, phối đồ hàng ngày.",
                    3290000, 2400000, 40,
                    "https://images.unsplash.com/photo-1597045566677-8cf032ed6634?w=600&h=600&fit=crop",
                    C["giay-nam"], B["nike"], sid),

                P("Adidas Stan Smith", "s8-adidas-stan-smith",
                    "Adidas Stan Smith giày tennis cổ điển từ 1971, upper da tổng hợp trắng sạch sẽ. " +
                    "Gót xanh lá đặc trưng, chân dung Stan Smith trên lưỡi gà. Đế cao su phẳng. " +
                    "Form slim clean. Primegreen chất liệu tái chế. Size 36-46. " +
                    "Biểu tượng minimalist sneaker, phối được mọi phong cách từ casual đến smart casual.",
                    2490000, 1700000, 55,
                    "https://images.unsplash.com/photo-1549298916-b41d501d3772?w=600&h=600&fit=crop",
                    C["giay-nam"], B["adidas"], sid),

                P("Puma Suede Classic XXI", "s8-puma-suede-classic-xxi",
                    "Puma Suede Classic XXI giày biểu tượng từ 1968, upper da suede genuine mềm mượt. " +
                    "Formstrip bên hông, Puma Cat logo gót. Đế cao su phẳng truyền thống. " +
                    "Lót SoftFoam+ êm chân. Size 39-44. Nhiều màu: đen, navy, đỏ, xanh. " +
                    "Huyền thoại hip-hop và street culture. Archive re-issue chất lượng cao hơn.",
                    1990000, 1300000, 45,
                    "https://images.unsplash.com/photo-1608231387042-66d1773070a5?w=600&h=600&fit=crop",
                    C["giay-nam"], B["puma"], sid),

                P("Nike Air Max 90", "s8-nike-air-max-90",
                    "Nike Air Max 90 đôi giày iconic với bọt khí Air Max visible đặc trưng ở gót. " +
                    "Upper da + mesh + TPU overlay phối màu đa sắc. Đệm Air Sole toàn bàn chân. " +
                    "Outsole waffle rubber bền bỉ. Thiết kế Tinker Hatfield kinh điển từ 1990. " +
                    "Size 39-45. Phù hợp streetwear, thời trang đường phố, daily wear.",
                    3590000, 2600000, 30,
                    "https://images.unsplash.com/photo-1605348532760-6753d2c43329?w=600&h=600&fit=crop",
                    C["giay-nam"], B["nike"], sid),

                P("New Balance 2002R", "s8-new-balance-2002r",
                    "New Balance 2002R upper ABZORB SBS đệm mềm cực kỳ thoải mái, da suede + mesh premium. " +
                    "N-ERGY outsole hấp thụ xung lực. Stability Web chống xoắn. " +
                    "Thiết kế deconstructed vintage từ dòng 2002 gốc. Size 39-45. " +
                    "Trend sneaker số 1 hiện tại, sold-out liên tục. Nhiều colorway giới hạn.",
                    3290000, 2500000, 25,
                    "https://images.unsplash.com/photo-1551107696-a4b0c5a0d9a2?w=600&h=600&fit=crop",
                    C["giay-nam"], B["new-balance"], sid),
            });

            // ────────────────────────────────────────────────
            // 7. GIÀY NỮ (8 sản phẩm)
            // ────────────────────────────────────────────────
            products.AddRange(new[]
            {
                P("Nike Air Force 1 Shadow Women", "s8-nike-af1-shadow-women",
                    "Nike Air Force 1 Shadow nữ, thiết kế layered chồng lớp độc đáo trên nền AF1 cổ điển. " +
                    "Da tổng hợp premium, đế Air sole kép tăng chiều cao 3cm. Swoosh 2 lớp. " +
                    "Pastel colorway nữ tính. Size 36-42. Phù hợp casual, streetwear, phối váy hoặc jeans.",
                    3290000, 2400000, 40,
                    "https://images.unsplash.com/photo-1595950653106-6c9ebd614d3a?w=600&h=600&fit=crop",
                    C["giay-nu"], B["nike"], sid),

                P("Adidas Samba OG Women", "s8-adidas-samba-og-women",
                    "Adidas Samba OG nữ, đôi giày trend nhất 2024-2025, upper da smooth + suede T-toe. " +
                    "3 sọc bên hông, gót Samba gold foil. Đế gum rubber truyền thống. " +
                    "Từ sân bóng đá indoor đến biểu tượng thời trang. Size 35-42. " +
                    "Màu trắng/đen kinh điển. Phối được mọi outfit từ jeans đến váy.",
                    2890000, 2000000, 45,
                    "https://images.unsplash.com/photo-1560769629-975ec94e6a86?w=600&h=600&fit=crop",
                    C["giay-nu"], B["adidas"], sid),

                P("Converse Chuck 70 Low Women", "s8-converse-chuck-70-low-women",
                    "Converse Chuck 70 Low nữ, phiên bản cao cấp hơn All Star thường. Canvas 12oz dày hơn, " +
                    "đệm OrthoLite + Zoom Air gót cực êm. Foxing tape vintage ngả vàng. " +
                    "Đế egret kem hoài cổ. Size 35-41. Phù hợp phối jeans, váy, streetwear vintage.",
                    1790000, 1200000, 55,
                    "https://images.unsplash.com/photo-1494496195158-c3becb4f2475?w=600&h=600&fit=crop",
                    C["giay-nu"], B["converse"], sid),

                P("Nike React Infinity Run 4 Women", "s8-nike-react-infinity-run-4-w",
                    "Nike React Infinity Run 4 nữ, Flyknit upper thoáng khí ôm chân. " +
                    "Đệm React foam hoàn trả năng lượng, rocker bottom giảm chấn thương. " +
                    "Wider forefoot ổn định. Outsole rubber bền bỉ. Size 36-42. " +
                    "Giày chạy bộ hàng ngày tốt nhất cho nữ. Màu pastel tươi sáng.",
                    3890000, 2900000, 30,
                    "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=600&h=600&fit=crop",
                    C["giay-nu"], B["nike"], sid),

                P("Charles & Keith Slingback Heels", "s8-charles-keith-slingback-heels",
                    "Giày cao gót slingback Charles & Keith, gót block heel 6cm vững vàng dễ đi. " +
                    "Mũi nhọn thanh lịch, quai hậu co giãn. Chất liệu faux leather mềm không cứng chân. " +
                    "Lót cushion êm. Size 35-41. Màu đen, nude, đỏ. " +
                    "Phù hợp đi làm công sở, đi tiệc, sự kiện.",
                    1290000, 850000, 50,
                    "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?w=600&h=600&fit=crop",
                    C["giay-nu"], B["charles-keith"], sid),

                P("Adidas Gazelle Bold Women", "s8-adidas-gazelle-bold-women",
                    "Adidas Gazelle Bold nữ, đế platform cao 4cm nâng chiều cao. " +
                    "Upper suede mềm, 3 sọc tương phản. T-toe truyền thống. " +
                    "Đế Trefoil rubber outsole bền grip tốt. Size 35-42. " +
                    "Trend platform sneaker hot nhất, phối outfit retro, Y2K. Nhiều colorway.",
                    2990000, 2200000, 35,
                    "https://images.unsplash.com/photo-1582588678413-dbf45f4823e9?w=600&h=600&fit=crop",
                    C["giay-nu"], B["adidas"], sid),

                P("Vans Slip-On Classic Women", "s8-vans-slip-on-classic-women",
                    "Vans Classic Slip-On nữ, canvas checkerboard pattern biểu tượng. " +
                    "Không dây buộc tiện lợi, co giãn 2 bên miệng giày. Đế waffle rubber. " +
                    "Lót ComfyCush (UltraCush) êm ái. Size 35-41. " +
                    "Phù hợp mặc hàng ngày, đi chơi, phối casual thoải mái.",
                    1490000, 950000, 60,
                    "https://images.unsplash.com/photo-1604671801908-6f0c6a092c05?w=600&h=600&fit=crop",
                    C["giay-nu"], B["vans"], sid),

                P("New Balance 530 Women", "s8-new-balance-530-women",
                    "New Balance 530 nữ, chunky retro running từ 1990s. ABZORB đệm gót + mũi siêu êm. " +
                    "Upper da tổng hợp + mesh, thiết kế dad shoe trending. " +
                    "N-ERGY outsole hấp thụ xung lực. Size 35-41. Màu trắng/bạc kinh điển. " +
                    "Phối được streetwear, Korean style, casual hàng ngày.",
                    2590000, 1800000, 40,
                    "https://images.unsplash.com/photo-1606107557195-0e29a4b5b4aa?w=600&h=600&fit=crop",
                    C["giay-nu"], B["new-balance"], sid),
            });

            // ────────────────────────────────────────────────
            // 8. TÚI XÁCH (6 sản phẩm)
            // ────────────────────────────────────────────────
            products.AddRange(new[]
            {
                P("Coach Tabby Shoulder Bag 26", "s8-coach-tabby-shoulder-bag-26",
                    "Túi xách Coach Tabby Shoulder Bag 26, da bê polished pebble leather sang trọng. " +
                    "Khóa chữ C xoay đặc trưng Coach. Dây đeo vai có thể tháo rời, đeo chéo crossbody. " +
                    "Ngăn chính rộng + 1 túi zip + 2 ngăn thẻ. Kích thước 26x18x7cm. " +
                    "Phù hợp đi làm, đi chơi, đi tiệc. Màu đen, nâu tan, cream.",
                    8990000, 6500000, 20,
                    "https://images.unsplash.com/photo-1584917865442-de89df76afd3?w=600&h=600&fit=crop",
                    C["tui-xach"], B["coach"], sid),

                P("Charles & Keith Quilted Chain Bag", "s8-charles-keith-quilted-chain-bag",
                    "Túi đeo chéo Charles & Keith chần bông (quilted) dây xích vàng sang trọng. " +
                    "Chất liệu faux leather mềm cao cấp. Nắp đậy từ tính. " +
                    "Ngăn chính + 1 ngăn zip bên trong. Kích thước 22x14x8cm. " +
                    "Dây xích có thể đeo vai hoặc đeo chéo. Phù hợp đi tiệc, hẹn hò, sự kiện.",
                    1590000, 1000000, 45,
                    "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=600&h=600&fit=crop",
                    C["tui-xach"], B["charles-keith"], sid),

                P("Adidas Original Trefoil Backpack", "s8-adidas-trefoil-backpack",
                    "Balo Adidas Originals Trefoil, chất polyester bền chống nước nhẹ. " +
                    "Logo Trefoil thêu nổi mặt trước. Ngăn laptop 15 inch + 2 túi bên. " +
                    "Quai đeo đệm foam êm vai, lưng thoáng khí mesh. Khóa kéo YKK. " +
                    "Kích thước 46x30x16cm, dung tích 25L. Phù hợp đi học, du lịch, gym.",
                    1290000, 850000, 60,
                    "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=600&h=600&fit=crop",
                    C["tui-xach"], B["adidas"], sid),

                P("Nike Brasilia Training Duffel Bag M", "s8-nike-brasilia-duffel-bag-m",
                    "Túi trống thể thao Nike Brasilia Medium 60L, polyester 600D chống nước bền bỉ. " +
                    "Ngăn giày riêng biệt thông khí. Ngăn chính rộng zippered + 3 túi phụ. " +
                    "Quai xách + dây đeo vai đệm tháo rời. Đáy cứng chống nước. " +
                    "Kích thước 61x30x33cm. Phù hợp gym, du lịch ngắn ngày, thể thao.",
                    990000, 650000, 55,
                    "https://images.unsplash.com/photo-1622560480605-d83c853bc5c3?w=600&h=600&fit=crop",
                    C["tui-xach"], B["nike"], sid),

                P("Gucci GG Marmont Mini Bag", "s8-gucci-gg-marmont-mini-bag",
                    "Túi Gucci GG Marmont Mini, da bê matelassé chần bông chevron V đặc trưng. " +
                    "Khóa GG interlocking vàng. Dây xích có thể đeo vai hoặc crossbody. " +
                    "Kích thước 22x13x6cm. Made in Italy. Ngăn chính + 1 ngăn thẻ bên trong. " +
                    "Biểu tượng xa xỉ Gucci, phù hợp sự kiện cao cấp, thời trang.",
                    45990000, 38000000, 5,
                    "https://images.unsplash.com/photo-1566150905458-1bf1fc113f0d?w=600&h=600&fit=crop",
                    C["tui-xach"], B["gucci"], sid),

                P("Coach Willow Tote Bag", "s8-coach-willow-tote-bag",
                    "Túi tote Coach Willow, da polished pebble leather bền đẹp thời gian. " +
                    "Quai xách dài đeo vai thoải mái + dây đeo crossbody kèm theo. " +
                    "Ngăn laptop 14 inch. Ngăn chính rộng rãi đựng A4, túi zip bên trong. " +
                    "Kích thước 35x26x13cm. Coach Tea Rose charm. Màu đen, nâu, taupe. " +
                    "Phù hợp đi làm, đi học, công tác.",
                    7490000, 5500000, 15,
                    "https://images.unsplash.com/photo-1590874103328-eac38a683ce7?w=600&h=600&fit=crop",
                    C["tui-xach"], B["coach"], sid),
            });

            // ────────────────────────────────────────────────
            // 9. ĐỒNG HỒ THỜI TRANG (8 sản phẩm)
            // ────────────────────────────────────────────────
            products.AddRange(new[]
            {
                P("Casio G-Shock GA-2100-1A1", "s8-casio-g-shock-ga-2100-1a1",
                    "Casio G-Shock GA-2100-1A1 \"CasiOak\" thiết kế bát giác AP Royal Oak lịch lãm. " +
                    "Vỏ carbon core guard siêu nhẹ, chống va đập. Chống nước 200m. " +
                    "Kim + digital kết hợp, LED light, đồng hồ bấm giờ, 5 alarm. " +
                    "Pin CR2016 ~3 năm. Đường kính 45.4mm, dày 11.8mm. Dây resin bền bỉ. " +
                    "Đồng hồ nam đẹp nhất tầm giá.",
                    2990000, 2200000, 50,
                    "https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&h=600&fit=crop",
                    C["dong-ho-thoi-trang"], B["casio"], sid),

                P("Casio G-Shock DW-5600BB-1", "s8-casio-g-shock-dw-5600bb-1",
                    "Casio G-Shock DW-5600BB-1 kinh điển mặt vuông, full black stealth. " +
                    "Chống va đập, chống nước 200m. EL backlight, alarm 1/100 stopwatch. " +
                    "Pin CR2016 ~2 năm. Đường kính 42.8mm, dày 13.4mm, nhẹ 53g. " +
                    "Thiết kế gốc G-Shock từ 1983. Phù hợp streetwear, phượt, quân sự.",
                    1890000, 1400000, 60,
                    "https://images.unsplash.com/photo-1614164185128-e4ec99c436d7?w=600&h=600&fit=crop",
                    C["dong-ho-thoi-trang"], B["casio"], sid),

                P("Daniel Wellington Classic 40mm", "s8-daniel-wellington-classic-40mm",
                    "Daniel Wellington Classic 40mm nam, mặt số trắng tối giản thanh lịch. " +
                    "Vỏ thép không gỉ mạ vàng hồng, kính Hardlex chống xước. " +
                    "Máy quartz Miyota Nhật Bản. Chống nước 3ATM. Dây da Italy thay nhanh 20mm. " +
                    "Dày 6mm siêu mỏng. Phù hợp đeo đi làm, sự kiện, lịch sự. " +
                    "Thương hiệu minimalist nổi tiếng khắp thế giới.",
                    3490000, 2500000, 35,
                    "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&h=600&fit=crop",
                    C["dong-ho-thoi-trang"], B["daniel-wellington"], sid),

                P("Daniel Wellington Petite 28mm Nữ", "s8-daniel-wellington-petite-28mm",
                    "Daniel Wellington Petite 28mm nữ, mặt số nhỏ thanh lịch nữ tính. " +
                    "Vỏ thép bạc, kính mineral, máy quartz Miyota. Chống nước 3ATM. " +
                    "Dây mesh thép milanese hoặc dây da interchangeable 12mm. " +
                    "Mỏng 6.2mm đeo nhẹ nhàng. Phù hợp đeo mọi dịp, phối trang sức.",
                    2990000, 2100000, 40,
                    "https://images.unsplash.com/photo-1522312346375-d1a52e2b99b3?w=600&h=600&fit=crop",
                    C["dong-ho-thoi-trang"], B["daniel-wellington"], sid),

                P("Fossil Gen 6 Hybrid Smartwatch", "s8-fossil-gen-6-hybrid-smartwatch",
                    "Fossil Gen 6 Hybrid kết hợp kim analog truyền thống với tính năng smartwatch. " +
                    "Đo nhịp tim, SpO2, tracking bước chân, giấc ngủ. Thông báo điện thoại LED ẩn. " +
                    "Pin 2 tuần (không phải sạc hàng ngày). Chống nước 3ATM. " +
                    "Máy E Ink ẩn sau mặt số. App Fossil Smartwatches. Đường kính 44mm. " +
                    "Dây da 22mm thay nhanh. Wear OS compatible.",
                    4990000, 3800000, 20,
                    "https://images.unsplash.com/photo-1509048191080-d2984bad6ae5?w=600&h=600&fit=crop",
                    C["dong-ho-thoi-trang"], B["fossil"], sid),

                P("Casio Edifice EFR-S108D-1A", "s8-casio-edifice-efr-s108d-1a",
                    "Casio Edifice EFR-S108D mỏng 8.9mm, mặt sapphire crystal. " +
                    "Vỏ thép không gỉ, dây thép solid band. Chronograph 1/1 giây, 60 phút. " +
                    "Chống nước 100m. Máy quartz chính xác. Đường kính 41mm. " +
                    "Thiết kế motorsport thanh lịch, phù hợp đeo đi làm, sự kiện.",
                    3290000, 2500000, 30,
                    "https://images.unsplash.com/photo-1587925358603-c2eea5305bbc?w=600&h=600&fit=crop",
                    C["dong-ho-thoi-trang"], B["casio"], sid),

                P("Fossil Neutra Chronograph", "s8-fossil-neutra-chronograph",
                    "Fossil Neutra Chronograph nam, mặt số tối minimalistic, vạch index mảnh. " +
                    "Vỏ 44mm thép không gỉ tone vàng hồng. Chronograph 3 mắt nhỏ. " +
                    "Dây da nâu 22mm thay nhanh. Chống nước 5ATM. Kính mineral. " +
                    "Máy quartz Nhật Bản. Phù hợp đeo sang trọng hàng ngày, business casual.",
                    3790000, 2800000, 25,
                    "https://images.unsplash.com/photo-1533139502658-0198f920d8e8?w=600&h=600&fit=crop",
                    C["dong-ho-thoi-trang"], B["fossil"], sid),

                P("Casio A168WA-1 Vintage Silver", "s8-casio-a168wa-1-vintage",
                    "Casio A168WA-1 đồng hồ digital retro cổ điển, mặt chữ nhật kim loại bạc. " +
                    "EL backlight xanh, alarm, stopwatch, auto calendar. Chống nước WR (splash). " +
                    "Pin CR2016 ~7 năm. Kích thước 36.3x33.2mm, nhẹ 32.5g. Dây thép co giãn. " +
                    "Biểu tượng thập niên 80s, Marty McFly Back to the Future. Giá siêu rẻ chất lượng Casio.",
                    690000, 450000, 100,
                    "https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&h=600&fit=crop",
                    C["dong-ho-thoi-trang"], B["casio"], sid),
            });

            // ────────────────────────────────────────────────
            // 10. MÀN HÌNH (8 sản phẩm)
            // ────────────────────────────────────────────────
            products.AddRange(new[]
            {
                P("LG UltraGear 27GP850-B 27\" QHD 165Hz", "s8-lg-ultragear-27gp850",
                    "LG UltraGear 27GP850-B màn hình gaming 27 inch QHD 2560x1440, Nano IPS 1ms GtG, 165Hz (OC 180Hz). " +
                    "HDR400, DCI-P3 98%, sRGB 99%. AMD FreeSync Premium + NVIDIA G-SYNC Compatible. " +
                    "DisplayPort 1.4, HDMI 2.0 x2, USB 3.0, headphone out. " +
                    "Chân đế tilt/height/pivot. VESA 100x100. Phù hợp gaming + đồ họa.",
                    8990000, 7500000, 20,
                    "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?w=600&h=600&fit=crop",
                    C["man-hinh"], B["lg-display"], sid),

                P("Samsung Odyssey G5 27\" QHD 165Hz", "s8-samsung-odyssey-g5-27",
                    "Samsung Odyssey G5 27 inch QHD 2560x1440, VA panel 1000R cong, 165Hz, 1ms MPRT. " +
                    "HDR10, contrast 3000:1 sâu đen. AMD FreeSync Premium. " +
                    "DisplayPort 1.2 + HDMI 2.0. Eye Saver Mode, Flicker Free. " +
                    "Game mode: FPS, RTS, RPG tối ưu. VESA 75x75. Chân V-slim.",
                    6990000, 5800000, 25,
                    "https://images.unsplash.com/photo-1593640408182-31c70c8268f5?w=600&h=600&fit=crop",
                    C["man-hinh"], B["samsung"], sid),

                P("Dell UltraSharp U2723QE 27\" 4K USB-C Hub", "s8-dell-ultrasharp-u2723qe",
                    "Dell UltraSharp U2723QE 27 inch 4K 3840x2160, IPS Black technology contrast 2000:1. " +
                    "sRGB 100%, DCI-P3 98%, Delta E < 2 factory calibrated, VESA DisplayHDR 400. " +
                    "USB-C 90W PD, USB-C upstream, RJ45 Ethernet, USB-A 3.2 x4 hub. HDMI + DP. " +
                    "KVM switch built-in. Chân đế ergonomic đầy đủ. Màn hình đồ họa chuyên nghiệp tốt nhất.",
                    14990000, 12500000, 12,
                    "https://images.unsplash.com/photo-1586210579191-33b45e38fa2c?w=600&h=600&fit=crop",
                    C["man-hinh"], B["dell"], sid),

                P("ASUS ProArt PA279CRV 27\" 4K", "s8-asus-proart-pa279crv-27",
                    "ASUS ProArt PA279CRV 27 inch 4K IPS, Delta E < 2, Calman Verified. " +
                    "sRGB 100%, DCI-P3 99%, Adobe RGB 99%. HDR10. USB-C 96W PD + USB hub. " +
                    "Hardware LUT 3D tích hợp. ProArt Palette phần mềm quản lý màu. " +
                    "HDMI 2.0, DP 1.4, USB-C, USB-A x4. Xoay Portrait. VESA 100x100. " +
                    "Phù hợp thiết kế đồ họa, video editing, nhiếp ảnh.",
                    13990000, 11500000, 10,
                    "https://images.unsplash.com/photo-1547082299-de196ea013d6?w=600&h=600&fit=crop",
                    C["man-hinh"], B["asus"], sid),

                P("BenQ MOBIUZ EX2710U 27\" 4K 144Hz", "s8-benq-mobiuz-ex2710u-27",
                    "BenQ MOBIUZ EX2710U 27 inch 4K 144Hz, IPS 1ms GtG, HDRi công nghệ HDR thông minh. " +
                    "treVolo loa 2.1 tích hợp (2 speaker + subwoofer) 5W. FreeSync Premium Pro. " +
                    "sRGB 99%, DCI-P3 98%, VESA DisplayHDR 600. DP 1.4, HDMI 2.1 x2, USB-B, USB-A x3. " +
                    "Chân đế pivit/height/tilt. Remote control. Màn hình gaming 4K 144Hz giá tốt nhất.",
                    15990000, 13500000, 8,
                    "https://images.unsplash.com/photo-1616711906333-23cf8b50f862?w=600&h=600&fit=crop",
                    C["man-hinh"], B["benq"], sid),

                P("LG UltraWide 34WP65C 34\" WQHD Curved", "s8-lg-ultrawide-34wp65c",
                    "LG UltraWide 34WP65C 34 inch WQHD 3440x1440, VA 1500R curved, 160Hz, 1ms MBR. " +
                    "HDR10, sRGB 99%, contrast 3000:1. AMD FreeSync Premium. " +
                    "DisplayPort + HDMI x2. Black Stabilizer, crosshair. " +
                    "Ultrawide 21:9 trải nghiệm immersive gaming và đa nhiệm. VESA 100x100.",
                    9990000, 8500000, 15,
                    "https://images.unsplash.com/photo-1558618666-fcd25c85f82e?w=600&h=600&fit=crop",
                    C["man-hinh"], B["lg-display"], sid),

                P("Samsung ViewFinity S8 S80UA 27\" 4K", "s8-samsung-viewfinity-s80ua-27",
                    "Samsung ViewFinity S80UA 27 inch 4K 3840x2160, IPS 60Hz, USB-C 90W PD. " +
                    "sRGB 98%, HDR10, 300 nits. Intelligent Eye Care giảm mỏi mắt. " +
                    "USB-C hub + USB 3.0 x3 + LAN + DP + HDMI. KVM switch 2 PC. " +
                    "Chân đế HAS pivot/tilt/swivel/height. VESA 100x100. Phù hợp văn phòng chuyên nghiệp.",
                    8490000, 7000000, 18,
                    "https://images.unsplash.com/photo-1551645120-d70bfe84c826?w=600&h=600&fit=crop",
                    C["man-hinh"], B["samsung"], sid),

                P("ASUS ROG Swift PG27AQN 27\" QHD 360Hz", "s8-asus-rog-swift-pg27aqn-27",
                    "ASUS ROG Swift PG27AQN 27 inch QHD 2560x1440, IPS 360Hz – màn hình gaming nhanh nhất QHD. " +
                    "1ms GtG, NVIDIA G-SYNC, Reflex Analyzer đo latency. HDR600, DCI-P3 95%. " +
                    "DisplayPort 1.4 DSC, HDMI 2.0, USB 3.0 x2. ROG Desk Mount Kit. " +
                    "Esports professional monitor. Chân đế ergonomic. VESA 100x100.",
                    22990000, 20000000, 6,
                    "https://images.unsplash.com/photo-1567603532203-ba8cc7f31024?w=600&h=600&fit=crop",
                    C["man-hinh"], B["asus"], sid),
            });

            // ────────────────────────────────────────────────
            // 11. BÀN PHÍM CƠ (8 sản phẩm)
            // ────────────────────────────────────────────────
            products.AddRange(new[]
            {
                P("Keychron Q1 Pro QMK/VIA 75%", "s8-keychron-q1-pro-75",
                    "Keychron Q1 Pro bàn phím cơ 75% layout, full CNC aluminum body 1.7kg chắc chắn. " +
                    "Gasket mount êm typing. QMK/VIA firmware tùy chỉnh layout + macro. " +
                    "Wireless Bluetooth 5.1 + USB-C. Hot-swappable switch. South-facing RGB per-key. " +
                    "Keycap PBT double-shot OSA profile. Foam đệm giảm tiếng (case + PCB). " +
                    "Pin 4000mAh ~300 giờ. Knob encoder. Gateron Jupiter switch.",
                    4290000, 3500000, 25,
                    "https://images.unsplash.com/photo-1618384887929-16ec33fab9ef?w=600&h=600&fit=crop",
                    C["ban-phim-co"], B["keychron"], sid),

                P("Keychron K2 V2 Wireless 75%", "s8-keychron-k2-v2-wireless-75",
                    "Keychron K2 V2 bàn phím cơ 75% layout nhỏ gọn, Bluetooth 5.1 + USB-C. " +
                    "Tương thích Mac + Windows, keycap kép 2 OS. Gateron G Pro mechanical switch. " +
                    "Hot-swappable (phiên bản K2 HE). RGB backlight 18 hiệu ứng. " +
                    "Khung nhôm + ABS. Pin 4000mAh ~240 giờ. Kết nối 3 thiết bị. " +
                    "Bàn phím wireless cơ bán chạy nhất cho Mac.",
                    1990000, 1400000, 40,
                    "https://images.unsplash.com/photo-1595044426077-d36d9236d54a?w=600&h=600&fit=crop",
                    C["ban-phim-co"], B["keychron"], sid),

                P("Akko 3068B Plus Multi-Mode 65%", "s8-akko-3068b-plus-65",
                    "Akko 3068B Plus 65% layout, Bluetooth 5.0 + 2.4GHz dongle + USB-C tri-mode. " +
                    "Switch Akko CS tùy chọn (Jelly Pink / Lavender Purple / Cream Yellow). " +
                    "Keycap PBT double-shot ASA profile nhiều theme: Black & Gold, Neon, World Tour. " +
                    "Hot-swappable 3-pin/5-pin. RGB South-facing. Pin 3000mAh sạc USB-C. " +
                    "Gasket mount đệm silicone êm. Giá tốt nhất phân khúc custom entry.",
                    1290000, 900000, 60,
                    "https://images.unsplash.com/photo-1587829741301-dc798b83add3?w=600&h=600&fit=crop",
                    C["ban-phim-co"], B["akko"], sid),

                P("Akko 5075B Plus V2 HE Magnetic Switch", "s8-akko-5075b-plus-v2-he",
                    "Akko 5075B Plus V2 HE bàn phím Hall Effect magnetic switch (analog), 75% layout. " +
                    "Rapid Trigger 0.1mm, adjustable actuation 0.1-4.0mm tùy chỉnh. " +
                    "Tri-mode: BT 5.0 + 2.4G + USB-C. CNC aluminum top case. Gasket structure. " +
                    "Per-key RGB South-facing. Keycap PBT ASA double-shot. Pin 3000mAh. " +
                    "Akko Cloud Driver firmware update + remap. Giá rẻ nhất dòng Hall Effect.",
                    1890000, 1400000, 35,
                    "https://images.unsplash.com/photo-1561241142-e3c9f2c8b8e7?w=600&h=600&fit=crop",
                    C["ban-phim-co"], B["akko"], sid),

                P("Logitech G915 X TKL Wireless", "s8-logitech-g915-x-tkl-wireless",
                    "Logitech G915 X TKL bàn phím gaming wireless low-profile, GX2 switch cơ mỏng (clicky/tactile/linear). " +
                    "LIGHTSPEED wireless + Bluetooth. LIGHTSYNC RGB per-key. " +
                    "Khung aluminum mỏng 23mm. Pin sạc USB-C, 36 giờ RGB bật. " +
                    "TKL layout tiết kiệm không gian. Media controls + volume roller. " +
                    "G HUB software tùy chỉnh. Phù hợp gaming esports + văn phòng cao cấp.",
                    4990000, 4200000, 15,
                    "https://images.unsplash.com/photo-1541140532154-b024d705b90a?w=600&h=600&fit=crop",
                    C["ban-phim-co"], B["logitech"], sid),

                P("Razer Huntsman V3 Pro", "s8-razer-huntsman-v3-pro-full",
                    "Razer Huntsman V3 Pro full-size, analog optical switch Gen 2 với Rapid Trigger 0.1mm. " +
                    "Adjustable actuation 0.1-4.0mm per-key. Magnetic wrist rest đệm leatherette. " +
                    "Razer Chroma RGB per-key. USB-C braided cable. Multi-function digital dial + 3 macro keys. " +
                    "Doubleshot PBT keycaps. Sound dampening foam. Polling rate 8000Hz (Snap Tap). " +
                    "Synapse 4 tùy chỉnh. Bàn phím gaming competitive tốt nhất.",
                    5990000, 5000000, 12,
                    "https://images.unsplash.com/photo-1615869442320-fd02a129c399?w=600&h=600&fit=crop",
                    C["ban-phim-co"], B["razer"], sid),

                P("Keychron V1 QMK 75% Budget", "s8-keychron-v1-qmk-75-budget",
                    "Keychron V1 QMK custom bàn phím cơ 75% layout giá rẻ nhất dòng QMK. " +
                    "Full plastic case nhẹ. Hot-swap switch Keychron K Pro mechanical. " +
                    "QMK/VIA fully programmable. South-facing RGB. Screw-in stabilizers đã lube. " +
                    "Keycap PBT double-shot KSA profile. USB-C kết nối. " +
                    "Sound dampening foam tầng. NKRO. Phù hợp entry custom keyboard.",
                    990000, 700000, 70,
                    "https://images.unsplash.com/photo-1633783714421-332b7f929891?w=600&h=600&fit=crop",
                    C["ban-phim-co"], B["keychron"], sid),

                P("Akko MOD007B V3 HE 75% Aluminum", "s8-akko-mod007b-v3-he-75",
                    "Akko MOD007B V3 HE 75% layout, full CNC aluminum case 1.8kg premium. " +
                    "Hall Effect magnetic switch, Rapid Trigger, tùy chỉnh actuation point. " +
                    "Tri-mode wireless: BT 5.0 + 2.4GHz + USB-C. Gasket mount + poron foam. " +
                    "Per-key RGB South-facing. Keycap PBT ASA. Knob encoder. " +
                    "Akko Cloud Driver. Pin 4000mAh. Cạnh tranh trực tiếp Wooting 60HE.",
                    2990000, 2400000, 20,
                    "https://images.unsplash.com/photo-1631559026593-0da3e2c5ef0f?w=600&h=600&fit=crop",
                    C["ban-phim-co"], B["akko"], sid),
            });

            // ────────────────────────────────────────────────
            // 12. QUẦN NAM (8 sản phẩm)
            // ────────────────────────────────────────────────
            products.AddRange(new[]
            {
                P("Nike Sportswear Club Jogger", "s8-nike-club-jogger-m",
                    "Quần jogger nam Nike Sportswear Club, chất French Terry cotton pha polyester nỉ mềm ấm. " +
                    "Bo gấu ống chân, cạp chun dây rút. 2 túi hông + 1 túi sau zip nhỏ. " +
                    "Logo Nike Futura đùi trái. Form standard fit. Size S-2XL. " +
                    "Phù hợp thể thao, đi chơi, loungewear. Nhiều màu: đen, xám, navy.",
                    1190000, 750000, 80,
                    "https://images.unsplash.com/photo-1624378439575-d8705ad7ae80?w=600&h=600&fit=crop",
                    C["quan-nam"], B["nike"], sid),

                P("Adidas Tiro 24 Training Pants", "s8-adidas-tiro-24-pants-m",
                    "Quần thể thao nam Adidas Tiro 24, AEROREADY polyester thoáng khí co giãn. " +
                    "3 sọc Adidas dọc ống. Khóa kéo ống chân tiện mang giày. Cạp chun dây rút. " +
                    "2 túi kéo zip. Form tapered slim fit. Size S-2XL. " +
                    "Phù hợp tập luyện, warm-up, chạy bộ, athleisure.",
                    990000, 650000, 90,
                    "https://images.unsplash.com/photo-1552902865-b72c031ac5ea?w=600&h=600&fit=crop",
                    C["quan-nam"], B["adidas"], sid),

                P("Uniqlo Stretch Selvedge Slim Jeans", "s8-uniqlo-stretch-selvedge-slim-jeans",
                    "Quần jeans nam Uniqlo Stretch Selvedge Slim Fit, denim Kaihara Nhật Bản pha stretch thoải mái. " +
                    "Selvedge dệt biên đỏ truyền thống trên khung dệt shuttle loom. " +
                    "Form slim fit thon gọn không bó. 5 pocket classic. Button fly. " +
                    "Size 28-36. Wash: dark indigo, medium blue. Made in Japan fabric.",
                    990000, 600000, 70,
                    "https://images.unsplash.com/photo-1542272604-787c3835535d?w=600&h=600&fit=crop",
                    C["quan-nam"], B["uniqlo"], sid),

                P("Zara Slim Fit Chino Pants", "s8-zara-slim-fit-chino-m",
                    "Quần chino nam Zara Slim Fit, cotton twill dày dặn mịn mềm. " +
                    "Form slim fit ôm nhẹ từ đùi xuống. Cạp có loop thắt lưng, zip + hook. " +
                    "2 túi chéo + 2 túi sau welt. Size 28-36. " +
                    "Nhiều màu: beige, navy, olive, đen, xám. Phù hợp đi làm, smart casual.",
                    790000, 480000, 65,
                    "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?w=600&h=600&fit=crop",
                    C["quan-nam"], B["zara"], sid),

                P("H&M Regular Fit Shorts", "s8-hm-regular-fit-shorts-m",
                    "Quần short nam H&M Regular Fit, cotton twill nhẹ mát mùa hè. " +
                    "Chiều dài trên gối (above knee). Cạp chun + dây rút bên trong. " +
                    "2 túi hông + 1 túi sau nút. Size XS-2XL. " +
                    "Nhiều màu: khaki, navy, trắng, đen. Phù hợp đi biển, dạo phố, casual.",
                    350000, 200000, 120,
                    "https://images.unsplash.com/photo-1591195853828-11db59a44f6b?w=600&h=600&fit=crop",
                    C["quan-nam"], B["hm"], sid),

                P("Puma Essentials Sweatpants", "s8-puma-essentials-sweatpants-m",
                    "Quần sweatpants nam Puma Essentials, cotton French Terry mềm ấm. " +
                    "Logo Puma Cat thêu đùi trái. Cạp chun dây rút, bo gấu ống. " +
                    "2 túi hông. Form regular fit thoải mái. Size S-2XL. " +
                    "Phù hợp gym, loungewear, đi chơi casual. Màu đen, xám heather.",
                    790000, 500000, 75,
                    "https://images.unsplash.com/photo-1584865288642-42078afe6942?w=600&h=600&fit=crop",
                    C["quan-nam"], B["puma"], sid),

                P("Nike Dri-FIT Challenger Running Shorts", "s8-nike-dri-fit-challenger-shorts",
                    "Quần đùi chạy bộ nam Nike Dri-FIT Challenger 7 inch, polyester Dri-FIT nhanh khô. " +
                    "Lót brief bên trong hỗ trợ. Túi zip sau đựng chìa khóa. " +
                    "Xẻ tà 2 bên thoải mái cử động. Cạp chun + dây rút nội. " +
                    "Phản quang 3M chạy đêm. Size S-2XL. Phù hợp chạy bộ, gym, thể thao.",
                    690000, 420000, 85,
                    "https://images.unsplash.com/photo-1562157873-818bc0726f68?w=600&h=600&fit=crop",
                    C["quan-nam"], B["nike"], sid),

                P("Adidas Essentials Fleece Jogger", "s8-adidas-essentials-fleece-jogger-m",
                    "Quần jogger nam Adidas Essentials Fleece, cotton blend fleece lining ấm mùa đông. " +
                    "3 sọc Adidas dọc ống. Cạp chun rộng + dây rút. Bo gấu ống chân. " +
                    "2 túi hông. Form tapered fit. Size S-3XL. Badge of Sport logo. " +
                    "Phù hợp loungewear, thể thao, đi chơi mùa lạnh.",
                    890000, 580000, 70,
                    "https://images.unsplash.com/photo-1517438476312-10d79c077509?w=600&h=600&fit=crop",
                    C["quan-nam"], B["adidas"], sid),
            });

            // ────────────────────────────────────────────────
            // 13. PHỤ KIỆN THÊM (5 sản phẩm)
            // ────────────────────────────────────────────────
            products.AddRange(new[]
            {
                P("Anker Soundcore Motion 300", "s8-anker-soundcore-motion-300",
                    "Loa bluetooth Anker Soundcore Motion 300, SmartTune tự điều chỉnh EQ theo vị trí đặt. " +
                    "Hi-Res Audio Wireless, LDAC codec. IPX7 chống nước ngập. Pin 13 giờ. " +
                    "BassUp + custom EQ trên Soundcore app. Bluetooth 5.3. Khối lượng 580g. " +
                    "Phù hợp outdoor, pool party, du lịch.",
                    1790000, 1300000, 40,
                    "https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?w=600&h=600&fit=crop",
                    C["loa-am-thanh"], B["anker"], sid),

                P("Apple AirTag 4 Pack", "s8-apple-airtag-4-pack",
                    "Apple AirTag bộ 4, chip U1 Ultra Wideband tìm chính xác Precision Finding trên iPhone. " +
                    "Mạng Find My hàng tỷ thiết bị Apple tìm kiếm toàn cầu. " +
                    "Chống nước IP67. Pin CR2032 thay được, ~1 năm. " +
                    "Loa tích hợp phát âm thanh. Khắc emoji miễn phí. NFC. " +
                    "Phù hợp gắn chìa khóa, ví, vali, thú cưng.",
                    2790000, 2300000, 50,
                    "https://images.unsplash.com/photo-1610438235354-a6ae5528385c?w=600&h=600&fit=crop",
                    C["phu-kien"], B["apple"], sid),

                P("Samsung Galaxy SmartTag2", "s8-samsung-galaxy-smarttag2",
                    "Samsung Galaxy SmartTag2 thiết bị theo dõi, SmartThings Find + UWB tìm chính xác. " +
                    "IP67 chống nước bụi. Pin CR2032 ~500 ngày. Compass View chỉ hướng AR. " +
                    "Lost Mode thông báo khi ai đó quét NFC. Nhẹ 9g, kích thước nhỏ gọn. " +
                    "Clip tích hợp gắn chìa khóa/túi xách dễ dàng.",
                    690000, 480000, 80,
                    "https://images.unsplash.com/photo-1585386959984-a4155224a1ad?w=600&h=600&fit=crop",
                    C["phu-kien"], B["samsung"], sid),

                P("Baseus Blade 100W 20000mAh", "s8-baseus-blade-100w-20000mah",
                    "Pin dự phòng Baseus Blade 100W 20000mAh, mỏng chỉ 18mm – mỏng nhất thế giới cho 20K. " +
                    "Sạc laptop 100W USB-C PD 3.0. 2 USB-C + 1 USB-A. Digital display % pin. " +
                    "Sạc MacBook Air M3 được 1 lần, iPhone 15 được 4.5 lần. " +
                    "Tương thích Steam Deck, iPad, Switch. Hàng không mang được (99.7Wh). Khối lượng 450g.",
                    1290000, 950000, 55,
                    "https://images.unsplash.com/photo-1609091839311-d5365f9ff1c5?w=600&h=600&fit=crop",
                    C["phu-kien"], B["baseus"], sid),

                P("TP-Link Tapo C220 Camera WiFi 2K", "s8-tp-link-tapo-c220-camera",
                    "Camera an ninh TP-Link Tapo C220 WiFi, độ phân giải 2K QHD 2560x1440 rõ nét. " +
                    "Xoay 360° panoramic, night vision starlight sensor. AI phân biệt người/thú cưng/xe. " +
                    "2-way audio đàm thoại. Lưu trữ microSD 512GB + cloud Tapo Care. " +
                    "Alarm siren tích hợp. Privacy mode nút vật lý. Tapo app quản lý dễ dàng. " +
                    "Phù hợp giám sát nhà, phòng bé, cửa hàng.",
                    690000, 480000, 60,
                    "https://images.unsplash.com/photo-1585771724684-38269d6639fd?w=600&h=600&fit=crop",
                    C["thiet-bi-mang"], B["tp-link"], sid),
            });

            // Stagger CreatedAt to simulate products added over time
            var now = DateTime.UtcNow;
            for (var i = 0; i < products.Count; i++)
                products[i].CreatedAt = now.AddHours(-i * 2.3);

            // Check for duplicate slugs in current batch and DB
            var slugsInBatch = products.Select(p => p.Slug).ToList();
            var existingSlugs = await context.Set<Product>()
                .Where(p => slugsInBatch.Contains(p.Slug))
                .Select(p => p.Slug)
                .ToListAsync();

            var productsToAdd = products.Where(p => !existingSlugs.Contains(p.Slug)).ToList();

            if (productsToAdd.Any())
            {
                context.Set<Product>().AddRange(productsToAdd);
                await context.SaveChangesAsync();
                Console.WriteLine($"✅ Đã tạo {productsToAdd.Count} sản phẩm cho Shop {TargetShopId}");
            }
            else
            {
                Console.WriteLine($"⏭️ Tất cả sản phẩm đã tồn tại, bỏ qua.");
            }

            Console.WriteLine($"🎉 Seed Shop {TargetShopId} hoàn tất! Tổng sản phẩm batch: {products.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Lỗi seed Shop {TargetShopId}: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"   Inner: {ex.InnerException.Message}");
        }
    }

    private static Product P(string name, string slug, string desc, decimal price, decimal capital, int qty, string image, int catId, int brandId, int shopId)
    {
        return new Product
        {
            Name = name,
            Slug = slug,
            Description = desc,
            Price = price,
            CapitalPrice = capital,
            Quantity = qty,
            Image = image,
            CategoryId = catId,
            BrandId = brandId,
            ShopId = shopId,
        };
    }
}
