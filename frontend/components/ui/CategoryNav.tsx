"use client";

import { useEffect, useState, useRef } from "react";
import Link from "next/link";
import { LayoutGrid, ChevronRight } from "lucide-react";
import { categoriesPublicService, Category } from "@/lib/services/public-api";

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

export default function CategoryNav() {
    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState(true);
    const [isOpen, setIsOpen] = useState(false);
    const menuRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        const load = async () => {
            const data = await categoriesPublicService.getAll();
            setCategories(data);
            setLoading(false);
        };
        load();
    }, []);

    // Close menu on click outside
    useEffect(() => {
        const handleClickOutside = (e: MouseEvent) => {
            if (menuRef.current && !menuRef.current.contains(e.target as Node)) {
                setIsOpen(false);
            }
        };
        document.addEventListener("mousedown", handleClickOutside);
        return () => document.removeEventListener("mousedown", handleClickOutside);
    }, []);

    if (loading || categories.length === 0) return null;

    return (
        <div className="bg-white dark:bg-slate-800 border-b border-slate-100 dark:border-slate-700">
            <div className="max-w-7xl mx-auto px-4">
                <div className="flex items-center gap-1 h-12">
                    {/* Dropdown trigger */}
                    <div ref={menuRef} className="relative">
                        <button
                            onClick={() => setIsOpen(!isOpen)}
                            onMouseEnter={() => setIsOpen(true)}
                            className={`flex items-center gap-2 px-4 py-2 rounded-lg text-sm font-semibold transition-colors ${isOpen
                                    ? "bg-violet-600 text-white"
                                    : "bg-slate-100 dark:bg-slate-700 text-slate-700 dark:text-slate-300 hover:bg-violet-100 hover:text-violet-700"
                                }`}
                        >
                            <LayoutGrid className="w-4 h-4" />
                            Danh mục
                        </button>

                        {/* Dropdown panel */}
                        {isOpen && (
                            <div
                                className="absolute top-full left-0 mt-1 w-64 bg-white dark:bg-slate-800 rounded-xl shadow-xl border border-slate-200 dark:border-slate-700 py-2 z-50 animate-fade-in-up"
                                onMouseLeave={() => setIsOpen(false)}
                            >
                                <Link
                                    href="/products"
                                    className="flex items-center gap-3 px-4 py-2.5 text-sm text-violet-600 dark:text-violet-400 font-semibold hover:bg-violet-50 dark:hover:bg-violet-900/20 transition-colors"
                                    onClick={() => setIsOpen(false)}
                                >
                                    <span className="text-base">🏷️</span>
                                    Tất cả sản phẩm
                                    <ChevronRight className="w-4 h-4 ml-auto" />
                                </Link>
                                <div className="h-px bg-slate-100 dark:bg-slate-700 my-1" />
                                {categories.map((category) => (
                                    <Link
                                        key={category.id}
                                        href={`/products?category=${category.id}`}
                                        className="flex items-center gap-3 px-4 py-2.5 text-sm text-slate-700 dark:text-slate-300 hover:bg-slate-50 dark:hover:bg-slate-700/50 hover:text-violet-600 dark:hover:text-violet-400 transition-colors group"
                                        onClick={() => setIsOpen(false)}
                                    >
                                        {category.image ? (
                                            <img src={category.image} alt="" className="w-6 h-6 rounded object-cover" />
                                        ) : (
                                            <span className="text-base">{getCategoryIcon(category.name)}</span>
                                        )}
                                        <span className="flex-1">{category.name}</span>
                                        <ChevronRight className="w-3.5 h-3.5 text-slate-400 opacity-0 group-hover:opacity-100 transition-opacity" />
                                    </Link>
                                ))}
                            </div>
                        )}
                    </div>

                    {/* Quick category links - ngang */}
                    <div className="hidden md:flex items-center gap-1 ml-2 overflow-hidden">
                        {categories.slice(0, 6).map((category) => (
                            <Link
                                key={category.id}
                                href={`/products?category=${category.id}`}
                                className="flex-shrink-0 px-3 py-1.5 rounded-lg text-sm text-slate-600 dark:text-slate-400 hover:text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 dark:hover:text-violet-400 transition-colors font-medium"
                            >
                                {category.name}
                            </Link>
                        ))}
                        {categories.length > 6 && (
                            <button
                                onClick={() => setIsOpen(true)}
                                className="flex-shrink-0 px-3 py-1.5 rounded-lg text-sm text-slate-400 hover:text-violet-600 transition-colors"
                            >
                                +{categories.length - 6}
                            </button>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}
