"use client";

import { useState, useEffect } from "react";
import { ArrowRight, Sparkles, Play, Star, TrendingUp } from "lucide-react";

export default function HeroBanner() {
    const [currentSlide, setCurrentSlide] = useState(0);

    // Auto-rotate slides
    useEffect(() => {
        const interval = setInterval(() => {
            setCurrentSlide((prev) => (prev + 1) % 3);
        }, 5000);
        return () => clearInterval(interval);
    }, []);

    return (
        <section className="relative gradient-hero overflow-hidden min-h-[600px] lg:min-h-[700px]">
            {/* Animated Background Elements */}
            <div className="absolute inset-0 overflow-hidden">
                {/* Gradient Orbs with Blob Animation */}
                <div
                    className="absolute top-10 left-5 w-[500px] h-[500px] bg-violet-500/20 rounded-full blur-3xl animate-blob"
                    style={{ animationDelay: "0s" }}
                />
                <div
                    className="absolute bottom-10 right-5 w-[600px] h-[600px] bg-pink-500/15 rounded-full blur-3xl animate-blob"
                    style={{ animationDelay: "2s" }}
                />
                <div
                    className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[800px] h-[800px] bg-purple-600/10 rounded-full blur-3xl animate-blob"
                    style={{ animationDelay: "4s" }}
                />

                {/* Floating particles */}
                {[...Array(20)].map((_, i) => (
                    <div
                        key={i}
                        className="absolute w-2 h-2 bg-white/20 rounded-full animate-float-slow"
                        style={{
                            left: `${Math.random() * 100}%`,
                            top: `${Math.random() * 100}%`,
                            animationDelay: `${Math.random() * 5}s`,
                            animationDuration: `${5 + Math.random() * 5}s`,
                        }}
                    />
                ))}

                {/* Grid Pattern */}
                <div
                    className="absolute inset-0 opacity-[0.03]"
                    style={{
                        backgroundImage: `linear-gradient(rgba(255,255,255,0.5) 1px, transparent 1px), linear-gradient(90deg, rgba(255,255,255,0.5) 1px, transparent 1px)`,
                        backgroundSize: "60px 60px",
                    }}
                />

                {/* Radial gradient overlay */}
                <div className="absolute inset-0 bg-[radial-gradient(ellipse_at_center,transparent_0%,rgba(0,0,0,0.4)_100%)]" />
            </div>

            <div className="relative max-w-7xl mx-auto px-4 py-16 md:py-20 lg:py-28">
                <div className="grid lg:grid-cols-2 gap-12 lg:gap-16 items-center">
                    {/* Content */}
                    <div className="text-center lg:text-left">
                        {/* Animated Badge */}
                        <div
                            className="inline-flex items-center gap-2 px-5 py-2.5 rounded-full bg-white/10 backdrop-blur-md border border-white/20 text-white text-sm mb-8 animate-fade-in-up hover:bg-white/15 transition-colors cursor-pointer group"
                        >
                            <span className="relative flex h-3 w-3">
                                <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-amber-400 opacity-75" />
                                <span className="relative inline-flex rounded-full h-3 w-3 bg-amber-400" />
                            </span>
                            <span className="font-medium">Ưu đãi đặc biệt mùa xuân 2026</span>
                            <ArrowRight className="w-4 h-4 group-hover:translate-x-1 transition-transform" />
                        </div>

                        {/* Headline with gradient animation */}
                        <h1
                            className="text-4xl md:text-5xl lg:text-6xl xl:text-7xl font-extrabold text-white mb-6 leading-[1.1] animate-fade-in-up stagger-1"
                        >
                            Khám Phá
                            <span className="block mt-2 bg-gradient-to-r from-violet-300 via-pink-300 to-amber-300 bg-clip-text text-transparent bg-[length:200%_auto] animate-gradient">
                                Xu Hướng Mới
                            </span>
                            <span className="block mt-2 text-white/90">
                                Công Nghệ 2026
                            </span>
                        </h1>

                        {/* Description */}
                        <p className="text-lg md:text-xl text-violet-200/90 mb-10 max-w-xl mx-auto lg:mx-0 animate-fade-in-up stagger-2 leading-relaxed">
                            Hàng ngàn sản phẩm chính hãng với mức giá ưu đãi.{" "}
                            <span className="text-amber-300 font-semibold">
                                Miễn phí vận chuyển
                            </span>{" "}
                            cho đơn hàng từ 500.000đ.
                        </p>

                        {/* CTAs - Enhanced */}
                        <div className="flex flex-col sm:flex-row gap-4 justify-center lg:justify-start animate-fade-in-up stagger-3">
                            <button className="group btn-primary px-8 py-4 text-lg shadow-2xl shadow-violet-500/30 hover:shadow-violet-500/50">
                                <span>Mua sắm ngay</span>
                                <ArrowRight className="w-5 h-5 group-hover:translate-x-1 transition-transform" />
                            </button>
                            <button className="group flex items-center justify-center gap-3 px-8 py-4 text-lg rounded-xl bg-white/10 backdrop-blur-md border border-white/20 text-white hover:bg-white/20 transition-all">
                                <span className="w-10 h-10 rounded-full bg-white/20 flex items-center justify-center group-hover:scale-110 transition-transform">
                                    <Play className="w-4 h-4 fill-current ml-0.5" />
                                </span>
                                Xem video
                            </button>
                        </div>

                        {/* Trust indicators */}
                        <div className="flex items-center gap-6 mt-12 justify-center lg:justify-start animate-fade-in-up stagger-4">
                            <div className="flex -space-x-3">
                                {[1, 2, 3, 4].map((i) => (
                                    <div
                                        key={i}
                                        className="w-10 h-10 rounded-full bg-gradient-to-br from-violet-400 to-pink-400 border-2 border-white/20 flex items-center justify-center text-white text-xs font-bold"
                                    >
                                        {["A", "B", "C", "D"][i - 1]}
                                    </div>
                                ))}
                            </div>
                            <div className="text-left">
                                <div className="flex items-center gap-1">
                                    {[...Array(5)].map((_, i) => (
                                        <Star
                                            key={i}
                                            className="w-4 h-4 text-amber-400 fill-amber-400"
                                        />
                                    ))}
                                    <span className="text-white font-semibold ml-1">4.9</span>
                                </div>
                                <p className="text-sm text-violet-300">50,000+ đánh giá</p>
                            </div>
                        </div>

                        {/* Stats */}
                        <div className="grid grid-cols-3 gap-6 mt-12 animate-fade-in-up stagger-5">
                            {[
                                { value: "10K+", label: "Sản phẩm", icon: "📦" },
                                { value: "50K+", label: "Khách hàng", icon: "👥" },
                                { value: "99%", label: "Hài lòng", icon: "⭐" },
                            ].map((stat, index) => (
                                <div
                                    key={index}
                                    className="text-center group cursor-pointer"
                                >
                                    <p className="text-4xl mb-1 group-hover:scale-125 transition-transform">
                                        {stat.icon}
                                    </p>
                                    <p className="text-2xl md:text-3xl font-bold text-white group-hover:text-amber-300 transition-colors">
                                        {stat.value}
                                    </p>
                                    <p className="text-sm text-violet-300">{stat.label}</p>
                                </div>
                            ))}
                        </div>
                    </div>

                    {/* Hero Image / Product Showcase - Enhanced */}
                    <div className="relative hidden lg:block animate-fade-in-right">
                        <div className="relative z-10">
                            {/* Main Product Card */}
                            <div className="relative glass-card rounded-3xl p-6 transform rotate-2 hover:rotate-0 transition-all duration-700 hover:scale-105 group">
                                {/* Glow effect */}
                                <div className="absolute inset-0 rounded-3xl bg-gradient-to-br from-violet-500/20 to-pink-500/20 opacity-0 group-hover:opacity-100 transition-opacity duration-500" />

                                <div className="relative aspect-square rounded-2xl overflow-hidden bg-gradient-to-br from-violet-100 to-pink-100 mb-4">
                                    <img
                                        src="https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=500&h=500&fit=crop"
                                        alt="Featured Product"
                                        className="w-full h-full object-cover group-hover:scale-110 transition-transform duration-700"
                                    />
                                    {/* Shimmer overlay */}
                                    <div className="absolute inset-0 bg-gradient-to-r from-transparent via-white/30 to-transparent -translate-x-full group-hover:translate-x-full transition-transform duration-1000" />
                                </div>
                                <div className="relative space-y-2">
                                    <span className="badge badge-hot">
                                        <TrendingUp className="w-3 h-3" />
                                        Bán chạy
                                    </span>
                                    <h3 className="text-xl font-bold text-slate-800">
                                        iPhone 16 Pro Max
                                    </h3>
                                    <p className="text-2xl font-bold bg-gradient-to-r from-violet-600 to-purple-600 bg-clip-text text-transparent">
                                        34.990.000đ
                                    </p>
                                </div>
                            </div>

                            {/* Floating Cards - Enhanced */}
                            <div
                                className="absolute -top-6 -left-10 glass-card rounded-2xl p-4 shadow-2xl animate-float"
                                style={{ animationDelay: "0s" }}
                            >
                                <div className="flex items-center gap-3">
                                    <div className="w-14 h-14 rounded-xl bg-gradient-to-br from-emerald-400 to-teal-500 flex items-center justify-center shadow-lg shadow-emerald-500/30">
                                        <span className="text-2xl">🎧</span>
                                    </div>
                                    <div>
                                        <p className="font-bold text-slate-800">AirPods Pro</p>
                                        <p className="text-sm text-violet-600 font-bold">
                                            5.990.000đ
                                        </p>
                                    </div>
                                </div>
                            </div>

                            <div
                                className="absolute -bottom-6 -right-10 glass-card rounded-2xl p-4 shadow-2xl animate-float"
                                style={{ animationDelay: "1s" }}
                            >
                                <div className="flex items-center gap-3">
                                    <div className="w-14 h-14 rounded-xl bg-gradient-to-br from-amber-400 to-orange-500 flex items-center justify-center shadow-lg shadow-amber-500/30">
                                        <span className="text-2xl">⌚</span>
                                    </div>
                                    <div>
                                        <p className="font-bold text-slate-800">Apple Watch</p>
                                        <p className="text-sm text-violet-600 font-bold">
                                            12.990.000đ
                                        </p>
                                    </div>
                                </div>
                            </div>

                            {/* Discount Badge - Enhanced */}
                            <div
                                className="absolute top-1/2 -right-6 transform -translate-y-1/2 animate-float"
                                style={{ animationDelay: "2s" }}
                            >
                                <div className="w-24 h-24 rounded-full bg-gradient-to-br from-pink-500 to-rose-600 flex items-center justify-center shadow-2xl shadow-pink-500/40 animate-pulse-glow">
                                    <div className="text-center text-white">
                                        <p className="text-2xl font-extrabold">-50%</p>
                                        <p className="text-xs font-medium uppercase tracking-wider">
                                            OFF
                                        </p>
                                    </div>
                                </div>
                            </div>

                            {/* New: Delivery badge */}
                            <div
                                className="absolute top-1/3 -left-16 glass-card rounded-xl p-3 shadow-xl animate-float"
                                style={{ animationDelay: "1.5s" }}
                            >
                                <div className="flex items-center gap-2">
                                    <span className="text-2xl">🚚</span>
                                    <div>
                                        <p className="text-xs text-slate-500">Giao trong</p>
                                        <p className="text-sm font-bold text-emerald-600">2 giờ</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            {/* Wave Decoration - Enhanced */}
            <div className="absolute bottom-0 left-0 right-0">
                <svg
                    viewBox="0 0 1440 120"
                    fill="none"
                    className="w-full h-auto"
                    preserveAspectRatio="none"
                >
                    <defs>
                        <linearGradient id="waveGradient" x1="0%" y1="0%" x2="100%" y2="0%">
                            <stop offset="0%" stopColor="#f8fafc" />
                            <stop offset="50%" stopColor="#f1f5f9" />
                            <stop offset="100%" stopColor="#f8fafc" />
                        </linearGradient>
                    </defs>
                    <path
                        d="M0 120L48 110C96 100 192 80 288 70C384 60 480 60 576 65C672 70 768 80 864 85C960 90 1056 90 1152 85C1248 80 1344 70 1392 65L1440 60V120H1392C1344 120 1248 120 1152 120C1056 120 960 120 864 120C768 120 672 120 576 120C480 120 384 120 288 120C192 120 96 120 48 120H0Z"
                        fill="url(#waveGradient)"
                    />
                </svg>
            </div>

            {/* Slide indicators */}
            <div className="absolute bottom-12 left-1/2 -translate-x-1/2 flex gap-2 z-10">
                {[0, 1, 2].map((idx) => (
                    <button
                        key={idx}
                        onClick={() => setCurrentSlide(idx)}
                        className={`h-2 rounded-full transition-all duration-300 ${currentSlide === idx
                                ? "w-8 bg-white"
                                : "w-2 bg-white/40 hover:bg-white/60"
                            }`}
                    />
                ))}
            </div>
        </section>
    );
}
