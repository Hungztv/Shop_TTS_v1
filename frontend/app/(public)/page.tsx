import HeroBanner from "@/components/ui/HeroBanner";
import CategoryNav from "@/components/ui/CategoryNav";
import ProductCard from "@/components/ui/ProductCard";
import { ArrowRight, Flame, Sparkles, TrendingUp, Zap } from "lucide-react";

// Sample product data
const trendingProducts = [
  {
    id: 1,
    name: "iPhone 16 Pro Max 256GB - Titan Đen",
    image: "https://images.unsplash.com/photo-1592750475338-74b7b21085ab?w=400&h=400&fit=crop",
    price: 34990000,
    originalPrice: 41990000,
    rating: 4.9,
    reviews: 2847,
    badge: "sale" as const,
    category: "Điện thoại",
  },
  {
    id: 2,
    name: "MacBook Pro 14 inch M3 Pro",
    image: "https://images.unsplash.com/photo-1517336714731-489689fd1ca8?w=400&h=400&fit=crop",
    price: 52990000,
    rating: 4.8,
    reviews: 1253,
    badge: "hot" as const,
    category: "Laptop",
  },
  {
    id: 3,
    name: "AirPods Pro 2nd Generation",
    image: "https://images.unsplash.com/photo-1606220588913-b3aacb4d2f46?w=400&h=400&fit=crop",
    price: 5990000,
    originalPrice: 6990000,
    rating: 4.7,
    reviews: 5672,
    badge: "sale" as const,
    category: "Phụ kiện",
  },
  {
    id: 4,
    name: "Apple Watch Series 9 GPS 45mm",
    image: "https://images.unsplash.com/photo-1546868871-7041f2a55e12?w=400&h=400&fit=crop",
    price: 12990000,
    rating: 4.6,
    reviews: 892,
    badge: "new" as const,
    category: "Đồng hồ",
  },
];

const newArrivals = [
  {
    id: 5,
    name: "Samsung Galaxy S24 Ultra 512GB",
    image: "https://images.unsplash.com/photo-1610945415295-d9bbf067e59c?w=400&h=400&fit=crop",
    price: 31990000,
    originalPrice: 35990000,
    rating: 4.8,
    reviews: 1567,
    badge: "new" as const,
    category: "Điện thoại",
  },
  {
    id: 6,
    name: "Sony WH-1000XM5 Wireless",
    image: "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?w=400&h=400&fit=crop",
    price: 7990000,
    originalPrice: 9490000,
    rating: 4.9,
    reviews: 3421,
    badge: "sale" as const,
    category: "Tai nghe",
  },
  {
    id: 7,
    name: "iPad Pro 12.9 inch M2 256GB",
    image: "https://images.unsplash.com/photo-1544244015-0df4b3ffc6b0?w=400&h=400&fit=crop",
    price: 29990000,
    rating: 4.7,
    reviews: 756,
    category: "Máy tính bảng",
  },
  {
    id: 8,
    name: "DJI Mini 4 Pro Fly More Combo",
    image: "https://images.unsplash.com/photo-1473968512647-3e447244af8f?w=400&h=400&fit=crop",
    price: 24990000,
    rating: 4.8,
    reviews: 234,
    badge: "hot" as const,
    category: "Thiết bị bay",
  },
];

const flashDeals = [
  {
    id: 9,
    name: "JBL Charge 5 Bluetooth Speaker",
    image: "https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?w=400&h=400&fit=crop",
    price: 2990000,
    originalPrice: 4490000,
    rating: 4.6,
    reviews: 1823,
    badge: "sale" as const,
    category: "Loa",
  },
  {
    id: 10,
    name: "Logitech MX Master 3S",
    image: "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=400&h=400&fit=crop",
    price: 2490000,
    originalPrice: 2990000,
    rating: 4.8,
    reviews: 967,
    badge: "sale" as const,
    category: "Phụ kiện",
  },
  {
    id: 11,
    name: "Samsung T7 SSD 1TB",
    image: "https://images.unsplash.com/photo-1597872200969-2b65d56bd16b?w=400&h=400&fit=crop",
    price: 2290000,
    originalPrice: 3290000,
    rating: 4.7,
    reviews: 2145,
    badge: "sale" as const,
    category: "Lưu trữ",
  },
  {
    id: 12,
    name: "Anker 65W USB-C Charger",
    image: "https://images.unsplash.com/photo-1583863788434-e58a36330cf0?w=400&h=400&fit=crop",
    price: 890000,
    originalPrice: 1290000,
    rating: 4.5,
    reviews: 3567,
    badge: "sale" as const,
    category: "Sạc",
  },
];

export default function Home() {
  return (
    <div className="bg-slate-50">
      {/* Hero Banner */}
      <HeroBanner />

      {/* Category Navigation */}
      <CategoryNav />

      {/* Flash Deals Section */}
      <section className="py-12 bg-gradient-to-r from-rose-50 via-white to-amber-50">
        <div className="max-w-7xl mx-auto px-4">
          {/* Section Header */}
          <div className="flex items-center justify-between mb-8">
            <div className="flex items-center gap-4">
              <div className="w-12 h-12 rounded-xl bg-gradient-to-br from-rose-500 to-orange-500 flex items-center justify-center animate-pulse">
                <Zap className="w-6 h-6 text-white" />
              </div>
              <div>
                <h2 className="text-2xl md:text-3xl font-bold text-slate-800">
                  Flash Sale
                </h2>
                <p className="text-slate-500">Kết thúc sau: 02:45:30</p>
              </div>
            </div>
            <a
              href="#"
              className="hidden sm:flex items-center gap-2 text-violet-600 hover:text-violet-700 font-medium transition-colors"
            >
              Xem tất cả
              <ArrowRight className="w-4 h-4" />
            </a>
          </div>

          {/* Products Grid */}
          <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 md:gap-6">
            {flashDeals.map((product) => (
              <ProductCard key={product.id} {...product} />
            ))}
          </div>
        </div>
      </section>

      {/* Trending Products Section */}
      <section className="py-12 bg-white">
        <div className="max-w-7xl mx-auto px-4">
          {/* Section Header */}
          <div className="flex items-center justify-between mb-8">
            <div className="flex items-center gap-4">
              <div className="w-12 h-12 rounded-xl bg-gradient-to-br from-violet-500 to-purple-600 flex items-center justify-center">
                <TrendingUp className="w-6 h-6 text-white" />
              </div>
              <div>
                <h2 className="text-2xl md:text-3xl font-bold text-slate-800">
                  Xu hướng hôm nay
                </h2>
                <p className="text-slate-500">Sản phẩm được quan tâm nhiều nhất</p>
              </div>
            </div>
            <a
              href="#"
              className="hidden sm:flex items-center gap-2 text-violet-600 hover:text-violet-700 font-medium transition-colors"
            >
              Xem tất cả
              <ArrowRight className="w-4 h-4" />
            </a>
          </div>

          {/* Products Grid */}
          <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 md:gap-6">
            {trendingProducts.map((product) => (
              <ProductCard key={product.id} {...product} />
            ))}
          </div>
        </div>
      </section>

      {/* Promotional Banner */}
      <section className="py-12">
        <div className="max-w-7xl mx-auto px-4">
          <div className="relative overflow-hidden rounded-3xl bg-gradient-to-r from-violet-600 via-purple-600 to-pink-600 p-8 md:p-12">
            {/* Background Pattern */}
            <div className="absolute inset-0 opacity-10">
              <div
                className="absolute inset-0"
                style={{
                  backgroundImage: `radial-gradient(circle at 25px 25px, white 2px, transparent 0)`,
                  backgroundSize: "50px 50px",
                }}
              />
            </div>

            <div className="relative grid md:grid-cols-2 gap-8 items-center">
              <div className="text-white">
                <span className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-white/20 text-sm font-medium mb-4">
                  <Flame className="w-4 h-4" />
                  Ưu đãi đặc biệt
                </span>
                <h2 className="text-3xl md:text-4xl font-bold mb-4">
                  Giảm đến 50% cho tất cả phụ kiện
                </h2>
                <p className="text-violet-100 mb-6 text-lg">
                  Cơ hội sở hữu phụ kiện chính hãng với giá tốt nhất. Chỉ áp dụng trong tuần này!
                </p>
                <button className="bg-white text-violet-600 px-8 py-4 rounded-xl font-semibold hover:bg-violet-50 transition-colors inline-flex items-center gap-2">
                  Mua ngay
                  <ArrowRight className="w-5 h-5" />
                </button>
              </div>
              <div className="hidden md:flex justify-center">
                <div className="relative">
                  <div className="w-64 h-64 rounded-full bg-white/10 flex items-center justify-center animate-pulse-glow">
                    <span className="text-8xl">🎧</span>
                  </div>
                  <div className="absolute -top-4 -right-4 w-20 h-20 rounded-full bg-amber-400 flex items-center justify-center shadow-xl animate-float">
                    <span className="text-white font-bold text-lg">-50%</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* New Arrivals Section */}
      <section className="py-12 bg-gradient-to-b from-slate-50 to-white">
        <div className="max-w-7xl mx-auto px-4">
          {/* Section Header */}
          <div className="flex items-center justify-between mb-8">
            <div className="flex items-center gap-4">
              <div className="w-12 h-12 rounded-xl bg-gradient-to-br from-emerald-500 to-teal-600 flex items-center justify-center">
                <Sparkles className="w-6 h-6 text-white" />
              </div>
              <div>
                <h2 className="text-2xl md:text-3xl font-bold text-slate-800">
                  Hàng mới về
                </h2>
                <p className="text-slate-500">Cập nhật mới nhất mỗi ngày</p>
              </div>
            </div>
            <a
              href="#"
              className="hidden sm:flex items-center gap-2 text-violet-600 hover:text-violet-700 font-medium transition-colors"
            >
              Xem tất cả
              <ArrowRight className="w-4 h-4" />
            </a>
          </div>

          {/* Products Grid */}
          <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 md:gap-6">
            {newArrivals.map((product) => (
              <ProductCard key={product.id} {...product} />
            ))}
          </div>
        </div>
      </section>

      {/* Trust Badges */}
      <section className="py-12 bg-white border-t border-slate-100">
        <div className="max-w-7xl mx-auto px-4">
          <div className="grid grid-cols-2 md:grid-cols-4 gap-8 text-center">
            {[
              { icon: "🚚", title: "Giao hàng nhanh", desc: "Toàn quốc 1-3 ngày" },
              { icon: "💯", title: "100% Chính hãng", desc: "Cam kết hàng thật" },
              { icon: "🔄", title: "Đổi trả dễ dàng", desc: "Trong vòng 30 ngày" },
              { icon: "💳", title: "Thanh toán an toàn", desc: "Bảo mật tuyệt đối" },
            ].map((badge, index) => (
              <div key={index} className="group cursor-pointer">
                <div className="text-5xl mb-3 transform group-hover:scale-110 transition-transform">
                  {badge.icon}
                </div>
                <h3 className="font-semibold text-slate-800 mb-1">{badge.title}</h3>
                <p className="text-sm text-slate-500">{badge.desc}</p>
              </div>
            ))}
          </div>
        </div>
      </section>
    </div>
  );
}