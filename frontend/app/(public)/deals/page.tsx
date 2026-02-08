"use client";

import { useState, useEffect } from "react";
import Link from "next/link";
import { Flame, Tag, Clock, ArrowRight, Loader2 } from "lucide-react";
import { productsPublicService, type Product as PublicProduct } from "@/lib/services/public-api";
import ProductCard from "@/components/ui/ProductCard";

interface DealProduct extends PublicProduct {
    capitalPrice: number;
}

export default function DealsPage() {
    const [products, setProducts] = useState<DealProduct[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(true);

    useEffect(() => {
        const fetchDeals = async () => {
            setIsLoading(true);
            try {
                // Lấy các sản phẩm bán chạy và có giảm giá (capitalPrice > price)
                const data = await productsPublicService.getAll({ pageSize: 50 });
                // Lọc sản phẩm có giá giảm (capitalPrice > price)
                const dealsProducts = (data.items as DealProduct[]).filter(
                    (p) => p.capitalPrice && p.capitalPrice > p.price
                );
                setProducts(dealsProducts);
            } catch (error) {
                console.error("Error fetching deals:", error);
            } finally {
                setIsLoading(false);
            }
        };
        fetchDeals();
    }, []);

    const formatPrice = (value: number): string => {
        return new Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
        }).format(value);
    };

    const calculateDiscount = (original: number, sale: number): number => {
        return Math.round(((original - sale) / original) * 100);
    };

    return (
        <div className="min-h-screen">
            {/* Hero Section */}
            <div className="relative bg-gradient-to-br from-rose-500 via-pink-500 to-orange-400 overflow-hidden">
                <div className="absolute inset-0 bg-[url('data:image/svg+xml,%3Csvg%20width%3D%2260%22%20height%3D%2260%22%20viewBox%3D%220%200%2060%2060%22%20xmlns%3D%22http://www.w3.org/2000/svg%22%3E%3Cg%20fill%3D%22none%22%20fill-rule%3D%22evenodd%22%3E%3Cg%20fill%3D%22%23ffffff%22%20fill-opacity%3D%220.1%22%3E%3Ccircle%20cx%3D%2230%22%20cy%3D%2230%22%20r%3D%224%22/%3E%3C/g%3E%3C/g%3E%3C/svg%3E')] opacity-30" />
                <div className="max-w-7xl mx-auto px-4 py-12 sm:py-16 relative z-10">
                    <div className="text-center">
                        <div className="inline-flex items-center gap-2 px-4 py-2 bg-white/20 backdrop-blur-sm rounded-full mb-4">
                            <Flame className="w-5 h-5 text-yellow-300 animate-pulse" />
                            <span className="text-white font-semibold text-sm">
                                Ưu đãi hot nhất
                            </span>
                        </div>
                        <h1 className="text-3xl sm:text-4xl lg:text-5xl font-bold text-white mb-4">
                            🔥 Flash Sale
                        </h1>
                        <p className="text-white/90 text-lg max-w-2xl mx-auto mb-6">
                            Săn deal khủng - Giảm giá sốc lên đến 50%! Số lượng có hạn, nhanh
                            tay kẻo lỡ!
                        </p>
                        <div className="flex items-center justify-center gap-6">
                            <div className="flex items-center gap-2 text-white/90">
                                <Clock className="w-5 h-5" />
                                <span className="text-sm">Cập nhật mỗi ngày</span>
                            </div>
                            <div className="flex items-center gap-2 text-white/90">
                                <Tag className="w-5 h-5" />
                                <span className="text-sm">{products.length} sản phẩm</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            {/* Products Grid */}
            <div className="max-w-7xl mx-auto px-4 py-8 sm:py-12">
                {isLoading ? (
                    <div className="flex flex-col items-center justify-center py-20">
                        <Loader2 className="w-10 h-10 text-rose-500 animate-spin mb-4" />
                        <p className="text-slate-500">Đang tải ưu đãi...</p>
                    </div>
                ) : products.length === 0 ? (
                    <div className="text-center py-20">
                        <div className="w-20 h-20 mx-auto rounded-2xl bg-slate-100 dark:bg-slate-800 flex items-center justify-center mb-6">
                            <Tag className="w-10 h-10 text-slate-400" />
                        </div>
                        <h2 className="text-xl font-bold text-slate-800 dark:text-white mb-2">
                            Chưa có ưu đãi
                        </h2>
                        <p className="text-slate-500 dark:text-slate-400 mb-6">
                            Hiện tại chưa có sản phẩm giảm giá. Hãy quay lại sau nhé!
                        </p>
                        <Link
                            href="/products"
                            className="inline-flex items-center gap-2 px-6 py-3 bg-violet-600 hover:bg-violet-700 text-white font-semibold rounded-xl transition-colors"
                        >
                            Xem tất cả sản phẩm
                            <ArrowRight className="w-4 h-4" />
                        </Link>
                    </div>
                ) : (
                    <>
                        {/* Stats Bar */}
                        <div className="flex flex-wrap items-center justify-between gap-4 mb-8 p-4 bg-rose-50 dark:bg-rose-900/20 rounded-2xl">
                            <div className="flex items-center gap-3">
                                <div className="w-10 h-10 rounded-xl bg-rose-500 flex items-center justify-center">
                                    <Flame className="w-5 h-5 text-white" />
                                </div>
                                <div>
                                    <p className="text-sm text-slate-500 dark:text-slate-400">
                                        Tổng sản phẩm
                                    </p>
                                    <p className="font-bold text-slate-800 dark:text-white">
                                        {products.length} deals
                                    </p>
                                </div>
                            </div>
                            <div className="text-right">
                                <p className="text-sm text-slate-500 dark:text-slate-400">
                                    Giảm cao nhất
                                </p>
                                <p className="font-bold text-rose-600">
                                    {products.length > 0
                                        ? Math.max(
                                            ...products.map((p) =>
                                                calculateDiscount(p.capitalPrice, p.price)
                                            )
                                        )
                                        : 0}
                                    %
                                </p>
                            </div>
                        </div>

                        {/* Products Grid */}
                        <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 sm:gap-6">
                            {products.map((product) => (
                                <div key={product.id} className="relative">
                                    {/* Discount Badge */}
                                    <div className="absolute top-2 left-2 z-10 px-2 py-1 bg-rose-500 text-white text-xs font-bold rounded-lg">
                                        -{calculateDiscount(product.capitalPrice, product.price)}%
                                    </div>
                                    <ProductCard
                                        id={product.id}
                                        name={product.name}
                                        slug={product.slug}
                                        image={product.image || "/placeholder.jpg"}
                                        price={product.price}
                                        originalPrice={product.capitalPrice}
                                        rating={product.averageRating || 0}
                                        reviews={product.totalReviews || 0}
                                        badge="sale"
                                        category={product.categoryName || ""}
                                    />
                                </div>
                            ))}
                        </div>
                    </>
                )}
            </div>

            {/* CTA Section */}
            <div className="bg-gradient-to-r from-violet-600 to-purple-600 py-12">
                <div className="max-w-4xl mx-auto px-4 text-center">
                    <h2 className="text-2xl sm:text-3xl font-bold text-white mb-4">
                        Không tìm thấy deal ưng ý?
                    </h2>
                    <p className="text-white/90 mb-6">
                        Khám phá toàn bộ bộ sưu tập sản phẩm của chúng tôi
                    </p>
                    <Link
                        href="/products"
                        className="inline-flex items-center gap-2 px-8 py-4 bg-white text-violet-600 font-bold rounded-xl hover:bg-violet-50 transition-colors"
                    >
                        Xem tất cả sản phẩm
                        <ArrowRight className="w-5 h-5" />
                    </Link>
                </div>
            </div>
        </div>
    );
}
