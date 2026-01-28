"use client";

import { useState } from "react";
import { Heart, ShoppingCart, Star, Eye } from "lucide-react";

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
    id,
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

    const discount = originalPrice
        ? Math.round(((originalPrice - price) / originalPrice) * 100)
        : 0;

    const formatPrice = (value: number) => {
        return new Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
        }).format(value);
    };

    return (
        <div
            className="card group relative hover-lift"
            onMouseEnter={() => setIsHovered(true)}
            onMouseLeave={() => setIsHovered(false)}
        >
            {/* Badge */}
            {badge && (
                <div className="absolute top-3 left-3 z-10">
                    <span
                        className={`badge ${badge === "sale"
                                ? "badge-sale"
                                : badge === "new"
                                    ? "badge-new"
                                    : "badge-hot"
                            }`}
                    >
                        {badge === "sale" ? `-${discount}%` : badge === "new" ? "Mới" : "Hot"}
                    </span>
                </div>
            )}

            {/* Wishlist Button */}
            <button
                onClick={() => setIsWishlisted(!isWishlisted)}
                className={`absolute top-3 right-3 z-10 w-9 h-9 rounded-full flex items-center justify-center transition-all duration-300 ${isWishlisted
                        ? "bg-pink-500 text-white"
                        : "bg-white/80 backdrop-blur-sm text-slate-400 hover:text-pink-500"
                    }`}
            >
                <Heart
                    className={`w-5 h-5 ${isWishlisted ? "fill-current" : ""}`}
                />
            </button>

            {/* Image Container */}
            <div className="relative aspect-square overflow-hidden bg-slate-100">
                <img
                    src={image}
                    alt={name}
                    className={`w-full h-full object-cover transition-transform duration-500 ${isHovered ? "scale-110" : "scale-100"
                        }`}
                />

                {/* Quick Actions Overlay */}
                <div
                    className={`absolute inset-0 bg-black/20 flex items-center justify-center gap-3 transition-opacity duration-300 ${isHovered ? "opacity-100" : "opacity-0"
                        }`}
                >
                    <button className="w-12 h-12 rounded-full bg-white text-violet-600 flex items-center justify-center hover:bg-violet-600 hover:text-white transition-colors shadow-lg hover-scale">
                        <Eye className="w-5 h-5" />
                    </button>
                    <button className="w-12 h-12 rounded-full bg-violet-600 text-white flex items-center justify-center hover:bg-violet-700 transition-colors shadow-lg hover-scale">
                        <ShoppingCart className="w-5 h-5" />
                    </button>
                </div>
            </div>

            {/* Content */}
            <div className="p-4">
                {/* Category */}
                <p className="text-xs text-violet-600 font-medium mb-1 uppercase tracking-wide">
                    {category}
                </p>

                {/* Name */}
                <h3 className="font-semibold text-slate-800 mb-2 line-clamp-2 group-hover:text-violet-600 transition-colors">
                    {name}
                </h3>

                {/* Rating */}
                <div className="flex items-center gap-1 mb-3">
                    <div className="flex items-center">
                        {[...Array(5)].map((_, i) => (
                            <Star
                                key={i}
                                className={`w-4 h-4 ${i < Math.floor(rating)
                                        ? "text-amber-400 fill-current"
                                        : "text-slate-300"
                                    }`}
                            />
                        ))}
                    </div>
                    <span className="text-sm text-slate-500">({reviews})</span>
                </div>

                {/* Price */}
                <div className="flex items-center gap-2">
                    <span className="text-lg font-bold text-violet-600">
                        {formatPrice(price)}
                    </span>
                    {originalPrice && (
                        <span className="text-sm text-slate-400 line-through">
                            {formatPrice(originalPrice)}
                        </span>
                    )}
                </div>

                {/* Add to Cart Button */}
                <button className="w-full mt-4 btn-primary py-2.5 text-sm opacity-0 group-hover:opacity-100 transition-opacity duration-300">
                    <ShoppingCart className="w-4 h-4" />
                    Thêm vào giỏ
                </button>
            </div>
        </div>
    );
}
