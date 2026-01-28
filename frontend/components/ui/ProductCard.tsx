"use client";

import { useState, useRef } from "react";
import { Heart, ShoppingCart, Star, Eye, Zap } from "lucide-react";

interface ProductCardProps {
    id: number;
    name: string;
    image: string;
    price: number;
    originalPrice?: number;
    rating: number;
    reviews: number;
    badge?: "sale" | "new" | "hot";
    category: string;
}

export default function ProductCard({
    name,
    image,
    price,
    originalPrice,
    rating,
    reviews,
    badge,
    category,
}: ProductCardProps) {
    const [isWishlisted, setIsWishlisted] = useState(false);
    const [isHovered, setIsHovered] = useState(false);
    const [isAddingToCart, setIsAddingToCart] = useState(false);
    const cardRef = useRef<HTMLDivElement>(null);
    const [tiltStyle, setTiltStyle] = useState({ transform: "" });

    const discount = originalPrice
        ? Math.round(((originalPrice - price) / originalPrice) * 100)
        : 0;

    const formatPrice = (value: number) => {
        return new Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
        }).format(value);
    };

    // 3D Tilt Effect
    const handleMouseMove = (e: React.MouseEvent<HTMLDivElement>) => {
        if (!cardRef.current) return;
        const rect = cardRef.current.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;
        const centerX = rect.width / 2;
        const centerY = rect.height / 2;
        const rotateX = (y - centerY) / 20;
        const rotateY = (centerX - x) / 20;

        setTiltStyle({
            transform: `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg) scale(1.02)`,
        });
    };

    const handleMouseLeave = () => {
        setIsHovered(false);
        setTiltStyle({ transform: "" });
    };

    const handleAddToCart = () => {
        setIsAddingToCart(true);
        setTimeout(() => setIsAddingToCart(false), 1000);
    };

    return (
        <div
            ref={cardRef}
            className="card group relative"
            onMouseEnter={() => setIsHovered(true)}
            onMouseMove={handleMouseMove}
            onMouseLeave={handleMouseLeave}
            style={{
                ...tiltStyle,
                transition: isHovered ? "none" : "transform 0.5s ease",
            }}
        >
            {/* Enhanced Badge */}
            {badge && (
                <div className="absolute top-3 left-3 z-10">
                    <span
                        className={`badge flex items-center gap-1 ${badge === "sale"
                                ? "badge-sale"
                                : badge === "new"
                                    ? "badge-new"
                                    : "badge-hot"
                            }`}
                    >
                        {badge === "hot" && <Zap className="w-3 h-3" />}
                        {badge === "sale" ? `-${discount}%` : badge === "new" ? "Mới" : "Hot"}
                    </span>
                </div>
            )}

            {/* Wishlist Button - Enhanced */}
            <button
                onClick={(e) => {
                    e.stopPropagation();
                    setIsWishlisted(!isWishlisted);
                }}
                className={`absolute top-3 right-3 z-10 w-10 h-10 rounded-xl flex items-center justify-center transition-all duration-300 hover:scale-110 ${isWishlisted
                        ? "bg-gradient-to-br from-pink-500 to-rose-600 text-white shadow-lg shadow-pink-500/30"
                        : "glass hover:bg-white"
                    }`}
            >
                <Heart
                    className={`w-5 h-5 transition-transform ${isWishlisted ? "fill-current scale-110" : "text-slate-500"
                        }`}
                />
            </button>

            {/* Image Container */}
            <div className="relative aspect-square overflow-hidden bg-gradient-to-br from-slate-100 to-slate-50 dark:from-slate-800 dark:to-slate-900">
                {/* Gradient overlay */}
                <div className="absolute inset-0 bg-gradient-to-t from-black/5 to-transparent z-[1]" />

                <img
                    src={image}
                    alt={name}
                    className={`w-full h-full object-cover transition-all duration-700 ${isHovered ? "scale-110 brightness-105" : "scale-100"
                        }`}
                />

                {/* Quick Actions Overlay */}
                <div
                    className={`absolute inset-0 z-[2] bg-gradient-to-t from-black/60 via-black/20 to-transparent flex items-end justify-center pb-6 gap-3 transition-all duration-400 ${isHovered ? "opacity-100 translate-y-0" : "opacity-0 translate-y-4"
                        }`}
                >
                    <button className="w-12 h-12 rounded-xl bg-white/90 backdrop-blur-sm text-violet-600 flex items-center justify-center hover:bg-white hover:scale-110 transition-all shadow-xl">
                        <Eye className="w-5 h-5" />
                    </button>
                    <button
                        onClick={handleAddToCart}
                        className={`w-12 h-12 rounded-xl flex items-center justify-center transition-all shadow-xl ${isAddingToCart
                                ? "bg-emerald-500 text-white scale-110"
                                : "bg-gradient-to-br from-violet-600 to-purple-700 text-white hover:scale-110"
                            }`}
                    >
                        {isAddingToCart ? (
                            <span className="text-lg">✓</span>
                        ) : (
                            <ShoppingCart className="w-5 h-5" />
                        )}
                    </button>
                </div>

                {/* Shimmer effect on hover */}
                <div
                    className={`absolute inset-0 z-[3] pointer-events-none transition-opacity duration-300 ${isHovered ? "opacity-100" : "opacity-0"
                        }`}
                >
                    <div className="absolute inset-0 bg-gradient-to-r from-transparent via-white/20 to-transparent -translate-x-full animate-[shimmer_1.5s_ease-in-out_infinite]" />
                </div>
            </div>

            {/* Content */}
            <div className="p-4 sm:p-5">
                {/* Category */}
                <p className="text-xs text-violet-600 dark:text-violet-400 font-semibold mb-1.5 uppercase tracking-wider">
                    {category}
                </p>

                {/* Name */}
                <h3 className="font-bold text-slate-800 dark:text-white mb-3 line-clamp-2 group-hover:text-violet-600 dark:group-hover:text-violet-400 transition-colors leading-snug">
                    {name}
                </h3>

                {/* Rating - Enhanced */}
                <div className="flex items-center gap-2 mb-3">
                    <div className="flex items-center gap-0.5">
                        {[...Array(5)].map((_, i) => (
                            <Star
                                key={i}
                                className={`w-4 h-4 transition-all ${i < Math.floor(rating)
                                        ? "text-amber-400 fill-amber-400"
                                        : "text-slate-200 dark:text-slate-700"
                                    }`}
                            />
                        ))}
                    </div>
                    <span className="text-sm font-medium text-slate-600 dark:text-slate-400">
                        {rating}
                    </span>
                    <span className="text-sm text-slate-400">
                        ({reviews.toLocaleString()})
                    </span>
                </div>

                {/* Price - Enhanced with animation */}
                <div className="flex items-baseline gap-2 flex-wrap">
                    <span className="text-xl sm:text-2xl font-bold bg-gradient-to-r from-violet-600 to-purple-600 bg-clip-text text-transparent">
                        {formatPrice(price)}
                    </span>
                    {originalPrice && (
                        <>
                            <span className="text-sm text-slate-400 line-through">
                                {formatPrice(originalPrice)}
                            </span>
                            <span className="text-xs font-bold text-rose-500 bg-rose-50 dark:bg-rose-500/10 px-2 py-0.5 rounded-full">
                                Tiết kiệm {formatPrice(originalPrice - price)}
                            </span>
                        </>
                    )}
                </div>

                {/* Add to Cart Button - Enhanced */}
                <button
                    onClick={handleAddToCart}
                    className={`w-full mt-4 btn-primary py-3 text-sm transform transition-all duration-300 ${isHovered
                            ? "opacity-100 translate-y-0"
                            : "opacity-0 translate-y-4 pointer-events-none"
                        } ${isAddingToCart ? "bg-emerald-500 animate-pulse" : ""}`}
                >
                    {isAddingToCart ? (
                        <>
                            <span className="text-lg">✓</span>
                            Đã thêm!
                        </>
                    ) : (
                        <>
                            <ShoppingCart className="w-4 h-4" />
                            Thêm vào giỏ
                        </>
                    )}
                </button>
            </div>

            {/* Card glow effect */}
            <div
                className={`absolute inset-0 rounded-[inherit] pointer-events-none transition-opacity duration-500 ${isHovered ? "opacity-100" : "opacity-0"
                    }`}
                style={{
                    boxShadow: "0 0 40px rgba(139, 92, 246, 0.15), 0 0 80px rgba(139, 92, 246, 0.05)",
                }}
            />
        </div>
    );
}
