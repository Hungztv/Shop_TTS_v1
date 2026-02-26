'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import HeroBanner from "@/components/ui/HeroBanner";
import CategoryNav from "@/components/ui/CategoryNav";
import ProductCard from "@/components/ui/ProductCard";
import { ArrowRight, Sparkles, TrendingUp, Zap, Truck, ShieldCheck, RotateCcw, CreditCard } from "lucide-react";
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
      const [flashRes, trendingRes, newRes] = await Promise.all([
        // Flash Sale: giá thấp nhất
        productsPublicService.getAll({ pageSize: 8, sortBy: 'price', sortOrder: 'asc' }),
        // Xu hướng: trang 2 để đa dạng sản phẩm
        productsPublicService.getAll({ pageSize: 8, page: 2 }),
        // Hàng mới về: sắp xếp theo ngày tạo mới nhất (ngày shop đẩy lên)
        productsPublicService.getAll({ pageSize: 8, sortBy: 'createdAt', sortOrder: 'desc' }),
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

  const ProductGridSkeleton = ({ count = 4 }: { count?: number }) => (
    <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4">
      {[...Array(count)].map((_, i) => (
        <div key={i} className="animate-pulse">
          <div className="bg-slate-200 dark:bg-slate-700 rounded-2xl aspect-square mb-3"></div>
          <div className="h-4 bg-slate-200 dark:bg-slate-700 rounded w-3/4 mb-2"></div>
          <div className="h-5 bg-slate-200 dark:bg-slate-700 rounded w-1/2"></div>
        </div>
      ))}
    </div>
  );

  const SectionHeader = ({ icon: Icon, iconColor, title, subtitle, href }: {
    icon: React.ElementType;
    iconColor: string;
    title: string;
    subtitle: string;
    href?: string;
  }) => (
    <div className="flex items-center justify-between mb-6">
      <div className="flex items-center gap-3">
        <div className={`w-10 h-10 rounded-xl ${iconColor} flex items-center justify-center`}>
          <Icon className="w-5 h-5 text-white" />
        </div>
        <div>
          <h2 className="text-xl md:text-2xl font-bold text-slate-800 dark:text-white">{title}</h2>
          <p className="text-sm text-slate-500 dark:text-slate-400">{subtitle}</p>
        </div>
      </div>
      {href && (
        <Link
          href={href}
          className="hidden sm:flex items-center gap-1.5 text-sm text-violet-600 hover:text-violet-700 font-medium transition-colors"
        >
          Xem tất cả
          <ArrowRight className="w-4 h-4" />
        </Link>
      )}
    </div>
  );

  return (
    <div className="bg-slate-50 dark:bg-slate-900">
      {/* Hero Banner Slider */}
      <HeroBanner />

      {/* Trust Badges - ngay dưới banner */}
      <section className="bg-white dark:bg-slate-800 border-b border-slate-100 dark:border-slate-700">
        <div className="max-w-7xl mx-auto px-4 py-5">
          <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
            {[
              { icon: Truck, title: "Giao hàng nhanh", desc: "Toàn quốc 1-3 ngày", color: "text-emerald-500" },
              { icon: ShieldCheck, title: "100% Chính hãng", desc: "Cam kết hàng thật", color: "text-blue-500" },
              { icon: RotateCcw, title: "Đổi trả dễ dàng", desc: "Trong vòng 30 ngày", color: "text-amber-500" },
              { icon: CreditCard, title: "Thanh toán an toàn", desc: "Bảo mật tuyệt đối", color: "text-violet-500" },
            ].map((badge, index) => (
              <div key={index} className="flex items-center gap-3 py-2">
                <badge.icon className={`w-8 h-8 ${badge.color} flex-shrink-0`} />
                <div>
                  <p className="font-semibold text-sm text-slate-800 dark:text-white">{badge.title}</p>
                  <p className="text-xs text-slate-500 dark:text-slate-400">{badge.desc}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Category Navigation */}
      <CategoryNav />

      {/* Flash Sale */}
      <section className="py-8 md:py-10">
        <div className="max-w-7xl mx-auto px-4">
          <SectionHeader
            icon={Zap}
            iconColor="bg-gradient-to-br from-rose-500 to-orange-500"
            title="Flash Sale"
            subtitle="Giá tốt - Số lượng có hạn"
            href="/products?sortBy=price&sortOrder=asc"
          />
          {loading ? (
            <ProductGridSkeleton count={5} />
          ) : flashDeals.length > 0 ? (
            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4">
              {flashDeals.slice(0, 5).map((product) => (
                <ProductCard key={product.id} {...mapProduct(product)} />
              ))}
            </div>
          ) : (
            <p className="text-center text-slate-500 py-8">Chưa có sản phẩm nào</p>
          )}
          {/* Mobile "Xem tất cả" */}
          <div className="mt-4 sm:hidden text-center">
            <Link href="/products" className="text-violet-600 font-medium text-sm">
              Xem tất cả sản phẩm →
            </Link>
          </div>
        </div>
      </section>

      {/* Divider */}
      <div className="max-w-7xl mx-auto px-4"><hr className="border-slate-200 dark:border-slate-700" /></div>

      {/* Sản phẩm mới */}
      <section className="py-8 md:py-10">
        <div className="max-w-7xl mx-auto px-4">
          <SectionHeader
            icon={Sparkles}
            iconColor="bg-gradient-to-br from-emerald-500 to-teal-600"
            title="Hàng mới về"
            subtitle="Cập nhật mới nhất mỗi ngày"
            href="/products?sortBy=createdAt&sortOrder=desc"
          />
          {loading ? (
            <ProductGridSkeleton count={5} />
          ) : newArrivals.length > 0 ? (
            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4">
              {newArrivals.slice(0, 5).map((product) => (
                <ProductCard key={product.id} {...mapProduct(product)} />
              ))}
            </div>
          ) : (
            <p className="text-center text-slate-500 py-8">Chưa có sản phẩm nào</p>
          )}
          <div className="mt-4 sm:hidden text-center">
            <Link href="/products" className="text-violet-600 font-medium text-sm">
              Xem tất cả sản phẩm →
            </Link>
          </div>
        </div>
      </section>

      {/* Divider */}
      <div className="max-w-7xl mx-auto px-4"><hr className="border-slate-200 dark:border-slate-700" /></div>

      {/* Xu hướng */}
      <section className="py-8 md:py-10">
        <div className="max-w-7xl mx-auto px-4">
          <SectionHeader
            icon={TrendingUp}
            iconColor="bg-gradient-to-br from-violet-500 to-purple-600"
            title="Xu hướng hôm nay"
            subtitle="Sản phẩm được quan tâm nhiều nhất"
            href="/products"
          />
          {loading ? (
            <ProductGridSkeleton count={5} />
          ) : trendingProducts.length > 0 ? (
            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4">
              {trendingProducts.slice(0, 5).map((product) => (
                <ProductCard key={product.id} {...mapProduct(product)} />
              ))}
            </div>
          ) : (
            <p className="text-center text-slate-500 py-8">Chưa có sản phẩm nào</p>
          )}
          <div className="mt-4 sm:hidden text-center">
            <Link href="/products" className="text-violet-600 font-medium text-sm">
              Xem tất cả sản phẩm →
            </Link>
          </div>
        </div>
      </section>

      {/* Footer CTA */}
      <section className="py-10 bg-gradient-to-r from-violet-600 to-purple-600">
        <div className="max-w-7xl mx-auto px-4 text-center">
          <h2 className="text-2xl md:text-3xl font-bold text-white mb-3">
            Khám phá hàng ngàn sản phẩm chính hãng
          </h2>
          <p className="text-violet-200 mb-6 max-w-xl mx-auto">
            Miễn phí vận chuyển cho đơn hàng từ 500.000đ. Đổi trả dễ dàng trong 30 ngày.
          </p>
          <Link
            href="/products"
            className="inline-flex items-center gap-2 px-8 py-3.5 bg-white text-violet-600 rounded-xl font-semibold hover:bg-violet-50 transition-colors shadow-lg"
          >
            Xem tất cả sản phẩm
            <ArrowRight className="w-5 h-5" />
          </Link>
        </div>
      </section>
    </div>
  );
}