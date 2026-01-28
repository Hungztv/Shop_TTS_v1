"use client";

import { ArrowRight, Sparkles } from "lucide-react";

export default function HeroBanner() {
    return (
        <section className="relative gradient-hero overflow-hidden">
            {/* Background Decorations */}
            <div className="absolute inset-0 overflow-hidden">
                {/* Gradient Orbs */}
                <div className="absolute top-20 left-10 w-72 h-72 bg-violet-500/30 rounded-full blur-3xl animate-float" />
                <div
                    className="absolute bottom-20 right-10 w-96 h-96 bg-pink-500/20 rounded-full blur-3xl animate-float"
                    style={{ animationDelay: "1s" }}
                />
                <div
                    className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[600px] h-[600px] bg-purple-600/20 rounded-full blur-3xl"
                />

                {/* Grid Pattern */}
                <div
                    className="absolute inset-0 opacity-10"
                    style={{
                        backgroundImage: `linear-gradient(rgba(255,255,255,0.1) 1px, transparent 1px), linear-gradient(90deg, rgba(255,255,255,0.1) 1px, transparent 1px)`,
                        backgroundSize: "50px 50px",
                    }}
                />
            </div>

            <div className="relative max-w-7xl mx-auto px-4 py-16 md:py-24 lg:py-32">
                <div className="grid lg:grid-cols-2 gap-12 items-center">
                    {/* Content */}
                    <div className="text-center lg:text-left">
                        {/* Badge */}
                        <div className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-white/10 backdrop-blur-sm border border-white/20 text-white text-sm mb-6 animate-fade-in-up">
                            <Sparkles className="w-4 h-4 text-amber-400" />
                            <span>Ưu đãi đặc biệt mùa xuân 2026</span>
                        </div>

                        {/* Headline */}
                        <h1
                            className="text-4xl md:text-5xl lg:text-6xl font-bold text-white mb-6 leading-tight animate-fade-in-up"
                            style={{ animationDelay: "0.1s" }}
                        >
                            Khám Phá
                            <span className="block bg-gradient-to-r from-violet-300 via-pink-300 to-amber-300 bg-clip-text text-transparent">
                                Xu Hướng Mới
                            </span>
                            Công Nghệ 2026
                        </h1>

                        {/* Description */}
                        <p
                            className="text-lg text-violet-200 mb-8 max-w-lg mx-auto lg:mx-0 animate-fade-in-up"
                            style={{ animationDelay: "0.2s" }}
                        >
                            Hàng ngàn sản phẩm chính hãng với mức giá ưu đãi. Miễn phí vận
                            chuyển cho đơn hàng từ 500.000đ.
                        </p>

                        {/* CTAs */}
                        <div
                            className="flex flex-col sm:flex-row gap-4 justify-center lg:justify-start animate-fade-in-up"
                            style={{ animationDelay: "0.3s" }}
                        >
                            <button className="btn-primary px-8 py-4 text-lg animate-pulse-glow">
                                Mua sắm ngay
                                <ArrowRight className="w-5 h-5" />
                            </button>
                            <button className="btn-secondary px-8 py-4 text-lg bg-white/10 border-white/20 text-white hover:bg-white/20">
                                Xem ưu đãi
                            </button>
                        </div>

                        {/* Stats */}
                        <div
                            className="flex justify-center lg:justify-start gap-8 mt-12 animate-fade-in-up"
                            style={{ animationDelay: "0.4s" }}
                        >
                            {[
                                { value: "10K+", label: "Sản phẩm" },
                                { value: "50K+", label: "Khách hàng" },
                                { value: "99%", label: "Hài lòng" },
                            ].map((stat, index) => (
                                <div key={index} className="text-center">
                                    <p className="text-3xl font-bold text-white">{stat.value}</p>
                                    <p className="text-sm text-violet-300">{stat.label}</p>
                                </div>
                            ))}
                        </div>
                    </div>

                    {/* Hero Image / Product Showcase */}
                    <div
                        className="relative hidden lg:block animate-fade-in-up"
                        style={{ animationDelay: "0.2s" }}
                    >
                        <div className="relative">
                            {/* Main Product Card */}
                            <div className="relative z-10 glass rounded-3xl p-6 transform rotate-3 hover:rotate-0 transition-transform duration-500">
                                <div className="aspect-square rounded-2xl overflow-hidden bg-gradient-to-br from-violet-100 to-pink-100 mb-4">
                                    <img
                                        src="https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=500&h=500&fit=crop"
                                        alt="Featured Product"
                                        className="w-full h-full object-cover"
                                    />
                                </div>
                                <div className="space-y-2">
                                    <span className="badge badge-hot">Bán chạy</span>
                                    <h3 className="text-xl font-bold text-slate-800">
                                        iPhone 16 Pro Max
                                    </h3>
                                    <p className="text-2xl font-bold text-violet-600">
                                        34.990.000đ
                                    </p>
                                </div>
                            </div>

                            {/* Floating Cards */}
                            <div className="absolute -top-4 -left-8 glass rounded-2xl p-4 shadow-xl animate-float">
                                <div className="flex items-center gap-3">
                                    <div className="w-12 h-12 rounded-xl bg-gradient-to-br from-emerald-400 to-emerald-600 flex items-center justify-center">
                                        <span className="text-white text-xl">🎧</span>
                                    </div>
                                    <div>
                                        <p className="font-semibold text-slate-800">AirPods Pro</p>
                                        <p className="text-sm text-violet-600 font-bold">
                                            5.990.000đ
                                        </p>
                                    </div>
                                </div>
                            </div>

                            <div
                                className="absolute -bottom-4 -right-8 glass rounded-2xl p-4 shadow-xl animate-float"
                                style={{ animationDelay: "0.5s" }}
                            >
                                <div className="flex items-center gap-3">
                                    <div className="w-12 h-12 rounded-xl bg-gradient-to-br from-amber-400 to-orange-500 flex items-center justify-center">
                                        <span className="text-white text-xl">⌚</span>
                                    </div>
                                    <div>
                                        <p className="font-semibold text-slate-800">Apple Watch</p>
                                        <p className="text-sm text-violet-600 font-bold">
                                            12.990.000đ
                                        </p>
                                    </div>
                                </div>
                            </div>

                            {/* Discount Badge */}
                            <div
                                className="absolute top-1/2 -right-4 transform -translate-y-1/2 animate-float"
                                style={{ animationDelay: "1s" }}
                            >
                                <div className="w-20 h-20 rounded-full bg-gradient-to-br from-pink-500 to-rose-600 flex items-center justify-center shadow-xl">
                                    <div className="text-center text-white">
                                        <p className="text-xl font-bold">-50%</p>
                                        <p className="text-xs">OFF</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            {/* Wave Decoration */}
            <div className="absolute bottom-0 left-0 right-0">
                <svg
                    viewBox="0 0 1440 120"
                    fill="none"
                    className="w-full h-auto"
                    preserveAspectRatio="none"
                >
                    <path
                        d="M0 120L48 110C96 100 192 80 288 70C384 60 480 60 576 65C672 70 768 80 864 85C960 90 1056 90 1152 85C1248 80 1344 70 1392 65L1440 60V120H1392C1344 120 1248 120 1152 120C1056 120 960 120 864 120C768 120 672 120 576 120C480 120 384 120 288 120C192 120 96 120 48 120H0Z"
                        fill="#f8fafc"
                    />
                </svg>
            </div>
        </section>
    );
}
