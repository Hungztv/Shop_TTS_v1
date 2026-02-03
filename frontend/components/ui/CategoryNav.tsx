"use client";

import { useRef, useEffect, useState } from "react";
import { ChevronLeft, ChevronRight } from "lucide-react";
import { categoriesPublicService, Category } from "@/lib/services/public-api";

// Fallback icons based on category name keywords
const getCategoryIcon = (name: string): string => {
    const lower = name.toLowerCase();
    if (lower.includes('điện thoại') || lower.includes('phone')) return '📱';
    if (lower.includes('laptop')) return '💻';
    if (lower.includes('máy tính bảng') || lower.includes('tablet')) return '📲';
    if (lower.includes('tai nghe') || lower.includes('headphone')) return '🎧';
    if (lower.includes('đồng hồ') || lower.includes('watch')) return '⌚';
    if (lower.includes('phụ kiện') || lower.includes('accessory')) return '🔌';
    if (lower.includes('tv') || lower.includes('tivi')) return '📺';
    if (lower.includes('máy ảnh') || lower.includes('camera')) return '📷';
    if (lower.includes('gaming') || lower.includes('game')) return '🎮';
    if (lower.includes('gia dụng') || lower.includes('home')) return '🏠';
    return '📦';
};

const getCategoryColor = (index: number): string => {
    const colors = [
        "from-blue-500 to-cyan-500",
        "from-violet-500 to-purple-500",
        "from-pink-500 to-rose-500",
        "from-amber-500 to-orange-500",
        "from-emerald-500 to-teal-500",
        "from-indigo-500 to-blue-500",
        "from-red-500 to-pink-500",
        "from-slate-600 to-slate-800",
        "from-green-500 to-emerald-500",
        "from-yellow-500 to-amber-500",
    ];
    return colors[index % colors.length];
};

export default function CategoryNav() {
    const scrollRef = useRef<HTMLDivElement>(null);
    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadCategories();
    }, []);

    const loadCategories = async () => {
        const data = await categoriesPublicService.getAll();
        setCategories(data);
        setLoading(false);
    };

    const scroll = (direction: "left" | "right") => {
        if (scrollRef.current) {
            const scrollAmount = 300;
            scrollRef.current.scrollBy({
                left: direction === "left" ? -scrollAmount : scrollAmount,
                behavior: "smooth",
            });
        }
    };

    if (loading) {
        return (
            <section className="py-12 bg-white">
                <div className="max-w-7xl mx-auto px-4">
                    <div className="animate-pulse">
                        <div className="h-8 bg-slate-200 rounded w-48 mb-4"></div>
                        <div className="flex gap-4">
                            {[...Array(6)].map((_, i) => (
                                <div key={i} className="w-32 flex-shrink-0">
                                    <div className="w-20 h-20 mx-auto bg-slate-200 rounded-2xl mb-3"></div>
                                    <div className="h-4 bg-slate-200 rounded mx-auto w-20"></div>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </section>
        );
    }

    if (categories.length === 0) {
        return null;
    }

    return (
        <section className="py-12 bg-white">
            <div className="max-w-7xl mx-auto px-4">
                {/* Section Header */}
                <div className="flex items-center justify-between mb-8">
                    <div>
                        <h2 className="text-2xl md:text-3xl font-bold text-slate-800">
                            Danh mục sản phẩm
                        </h2>
                        <p className="text-slate-500 mt-1">
                            Khám phá các danh mục phổ biến
                        </p>
                    </div>
                    <div className="flex gap-2">
                        <button
                            onClick={() => scroll("left")}
                            className="w-10 h-10 rounded-xl bg-slate-100 hover:bg-violet-100 flex items-center justify-center text-slate-600 hover:text-violet-600 transition-colors"
                        >
                            <ChevronLeft className="w-5 h-5" />
                        </button>
                        <button
                            onClick={() => scroll("right")}
                            className="w-10 h-10 rounded-xl bg-slate-100 hover:bg-violet-100 flex items-center justify-center text-slate-600 hover:text-violet-600 transition-colors"
                        >
                            <ChevronRight className="w-5 h-5" />
                        </button>
                    </div>
                </div>

                {/* Categories Scroll */}
                <div
                    ref={scrollRef}
                    className="flex gap-4 overflow-x-auto pb-4 scrollbar-hide scroll-smooth"
                    style={{ scrollbarWidth: "none", msOverflowStyle: "none" }}
                >
                    {categories.map((category, index) => (
                        <a
                            key={category.id}
                            href={`/products?category=${category.id}`}
                            className="flex-shrink-0 group"
                        >
                            <div className="w-32 text-center">
                                {/* Icon Container */}
                                <div
                                    className={`w-20 h-20 mx-auto rounded-2xl bg-gradient-to-br ${getCategoryColor(index)} flex items-center justify-center mb-3 group-hover:scale-110 group-hover:shadow-lg transition-all duration-300 overflow-hidden`}
                                >
                                    {category.image ? (
                                        <img src={category.image} alt={category.name} className="w-full h-full object-cover" />
                                    ) : (
                                        <span className="text-3xl">{getCategoryIcon(category.name)}</span>
                                    )}
                                </div>
                                {/* Name */}
                                <p className="font-medium text-slate-700 group-hover:text-violet-600 transition-colors line-clamp-2">
                                    {category.name}
                                </p>
                            </div>
                        </a>
                    ))}
                </div>
            </div>
        </section>
    );
}
