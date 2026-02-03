'use client';

import { useEffect, useState } from 'react';
import HeroBanner from "@/components/ui/HeroBanner";
import CategoryNav from "@/components/ui/CategoryNav";
import ProductCard from "@/components/ui/ProductCard";
import { ArrowRight, Flame, Sparkles, TrendingUp, Zap } from "lucide-react";
import { productsPublicService, Product } from "@/lib/services/public-api";

export default function Home() {
  const [flashDeals, setFlashDeals] = useState<Product[]>([]);
  const [trendingProducts, setTrendingProducts] = useState<Product[]>([]);
  const [newArrivals, setNewArrivals] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadProducts();
  }, []);

  const loadProducts = async () => {
    try {
      // Load different product sets
      const [flashRes, trendingRes, newRes] = await Promise.all([
        // Flash deals: random products (could be based on discount later)
        productsPublicService.getAll({ pageSize: 4, sortBy: 'price', sortOrder: 'asc' }),
        // Trending: most reviewed/rated products
        productsPublicService.getAll({ pageSize: 4, sortBy: 'createdAt', sortOrder: 'desc' }),
        // New arrivals: newest products
        productsPublicService.getAll({ pageSize: 4, page: 2 }),
      ]);

      setFlashDeals(flashRes.items);
      setTrendingProducts(trendingRes.items);
      setNewArrivals(newRes.items);
    } catch (error) {
      console.error('Error loading products:', error);
    } finally {
      setLoading(false);
    }
  };

  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('vi-VN').format(price);
  };

  // Transform Product to ProductCard props
  const mapProduct = (product: Product) => ({
    id: product.id,
    name: product.name,
    image: product.image || 'https://placehold.co/400x400?text=No+Image',
    price: product.price,
    originalPrice: product.capitalPrice && product.capitalPrice > product.price ? product.capitalPrice : undefined,
    rating: product.averageRating || 0,
    reviews: product.totalReviews || 0,
    category: product.categoryName || '',
    badge: product.capitalPrice && product.capitalPrice > product.price ? 'sale' as const : undefined,
    slug: product.slug,
  });

  const ProductGridSkeleton = () => (
    <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 md:gap-6">
      {[...Array(4)].map((_, i) => (
        <div key={i} className="animate-pulse">
          <div className="bg-slate-200 rounded-2xl aspect-square mb-3"></div>
          <div className="h-4 bg-slate-200 rounded w-3/4 mb-2"></div>
          <div className="h-6 bg-slate-200 rounded w-1/2"></div>
        </div>
      ))}
    </div>
  );

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
                <p className="text-slate-500">Ưu đãi có giới hạn!</p>
              </div>
            </div>
            <a
              href="/products"
              className="hidden sm:flex items-center gap-2 text-violet-600 hover:text-violet-700 font-medium transition-colors"
            >
              Xem tất cả
              <ArrowRight className="w-4 h-4" />
            </a>
          </div>

          {/* Products Grid */}
          {loading ? (
            <ProductGridSkeleton />
          ) : flashDeals.length > 0 ? (
            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 md:gap-6">
              {flashDeals.map((product) => (
                <ProductCard key={product.id} {...mapProduct(product)} />
              ))}
            </div>
          ) : (
            <p className="text-center text-slate-500 py-8">Chưa có sản phẩm nào</p>
          )}
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
              href="/products"
              className="hidden sm:flex items-center gap-2 text-violet-600 hover:text-violet-700 font-medium transition-colors"
            >
              Xem tất cả
              <ArrowRight className="w-4 h-4" />
            </a>
          </div>

          {/* Products Grid */}
          {loading ? (
            <ProductGridSkeleton />
          ) : trendingProducts.length > 0 ? (
            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 md:gap-6">
              {trendingProducts.map((product) => (
                <ProductCard key={product.id} {...mapProduct(product)} />
              ))}
            </div>
          ) : (
            <p className="text-center text-slate-500 py-8">Chưa có sản phẩm nào</p>
          )}
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
                <a href="/products" className="bg-white text-violet-600 px-8 py-4 rounded-xl font-semibold hover:bg-violet-50 transition-colors inline-flex items-center gap-2">
                  Mua ngay
                  <ArrowRight className="w-5 h-5" />
                </a>
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
              href="/products"
              className="hidden sm:flex items-center gap-2 text-violet-600 hover:text-violet-700 font-medium transition-colors"
            >
              Xem tất cả
              <ArrowRight className="w-4 h-4" />
            </a>
          </div>

          {/* Products Grid */}
          {loading ? (
            <ProductGridSkeleton />
          ) : newArrivals.length > 0 ? (
            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 md:gap-6">
              {newArrivals.map((product) => (
                <ProductCard key={product.id} {...mapProduct(product)} />
              ))}
            </div>
          ) : (
            <p className="text-center text-slate-500 py-8">Chưa có sản phẩm nào</p>
          )}
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