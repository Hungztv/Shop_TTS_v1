"use client";

import { useRef } from "react";
import { ChevronLeft, ChevronRight } from "lucide-react";

const categories = [
    { id: 1, name: "Điện thoại", icon: "📱", color: "from-blue-500 to-cyan-500" },
    { id: 2, name: "Laptop", icon: "💻", color: "from-violet-500 to-purple-500" },
    { id: 3, name: "Máy tính bảng", icon: "📲", color: "from-pink-500 to-rose-500" },
    { id: 4, name: "Tai nghe", icon: "🎧", color: "from-amber-500 to-orange-500" },
    { id: 5, name: "Đồng hồ", icon: "⌚", color: "from-emerald-500 to-teal-500" },
    { id: 6, name: "Phụ kiện", icon: "🔌", color: "from-indigo-500 to-blue-500" },
    { id: 7, name: "TV", icon: "📺", color: "from-red-500 to-pink-500" },
    { id: 8, name: "Máy ảnh", icon: "📷", color: "from-slate-600 to-slate-800" },
    { id: 9, name: "Gaming", icon: "🎮", color: "from-green-500 to-emerald-500" },
    { id: 10, name: "Gia dụng", icon: "🏠", color: "from-yellow-500 to-amber-500" },
];

export default function CategoryNav() {
    const scrollRef = useRef<HTMLDivElement>(null);

    const scroll = (direction: "left" | "right") => {
        if (scrollRef.current) {
            const scrollAmount = 300;
            scrollRef.current.scrollBy({
                left: direction === "left" ? -scrollAmount : scrollAmount,
                behavior: "smooth",
            });
        }
    };

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
                    {categories.map((category) => (
                        <a
                            key={category.id}
                            href="#"
                            className="flex-shrink-0 group"
                        >
                            <div className="w-32 text-center">
                                {/* Icon Container */}
                                <div
                                    className={`w-20 h-20 mx-auto rounded-2xl bg-gradient-to-br ${category.color} flex items-center justify-center mb-3 group-hover:scale-110 group-hover:shadow-lg transition-all duration-300`}
                                >
                                    <span className="text-3xl">{category.icon}</span>
                                </div>
                                {/* Name */}
                                <p className="font-medium text-slate-700 group-hover:text-violet-600 transition-colors">
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
