"use client";

import { useState, useEffect, useCallback } from "react";
import { ChevronLeft, ChevronRight } from "lucide-react";
import Link from "next/link";
import { slidersPublicService, Slider } from "@/lib/services/public-api";

export default function HeroBanner() {
    const [sliders, setSliders] = useState<Slider[]>([]);
    const [currentSlide, setCurrentSlide] = useState(0);
    const [loading, setLoading] = useState(true);
    const [isTransitioning, setIsTransitioning] = useState(false);

    // Fetch sliders from API
    useEffect(() => {
        const fetchSliders = async () => {
            const data = await slidersPublicService.getActive();
            setSliders(data);
            setLoading(false);
        };
        fetchSliders();
    }, []);

    const slideCount = sliders.length || 1;

    const goToSlide = useCallback((index: number) => {
        if (isTransitioning) return;
        setIsTransitioning(true);
        setCurrentSlide(index);
        setTimeout(() => setIsTransitioning(false), 700);
    }, [isTransitioning]);

    const nextSlide = useCallback(() => {
        goToSlide((currentSlide + 1) % slideCount);
    }, [currentSlide, slideCount, goToSlide]);

    const prevSlide = useCallback(() => {
        goToSlide((currentSlide - 1 + slideCount) % slideCount);
    }, [currentSlide, slideCount, goToSlide]);

    // Auto rotate
    useEffect(() => {
        if (sliders.length <= 1) return;
        const interval = setInterval(nextSlide, 5000);
        return () => clearInterval(interval);
    }, [nextSlide, sliders.length]);

    // Loading skeleton
    if (loading) {
        return (
            <section className="relative w-full h-[clamp(160px,30vw,320px)] bg-gradient-to-r from-slate-200 to-slate-300 animate-pulse" />
        );
    }

    // No sliders — fallback
    if (sliders.length === 0) {
        return (
            <section className="relative w-full h-[clamp(160px,30vw,320px)] gradient-hero flex items-center justify-center">
                <div className="text-center text-white px-4">
                    <h1 className="text-2xl md:text-4xl lg:text-5xl font-extrabold mb-3">
                        Chào mừng đến với Shop TTS
                    </h1>
                    <p className="text-base md:text-lg text-white/80 mb-5 max-w-2xl mx-auto">
                        Hàng ngàn sản phẩm chính hãng với mức giá ưu đãi
                    </p>
                    <Link
                        href="/products"
                        className="inline-flex items-center gap-2 px-6 py-3 bg-white text-violet-600 rounded-xl font-semibold hover:bg-violet-50 transition-colors text-base"
                    >
                        Mua sắm ngay
                    </Link>
                </div>
            </section>
        );
    }

    return (
        <section className="relative w-full h-[clamp(160px,30vw,320px)] overflow-hidden bg-black group">
            {/* Slides */}
            {sliders.map((slider, index) => (
                <div
                    key={slider.id}
                    className={`absolute inset-0 transition-all duration-700 ease-in-out ${index === currentSlide
                        ? "opacity-100 scale-100"
                        : "opacity-0 scale-105"
                        }`}
                >
                    {/* Image — full cover, auto scale, clickable if link exists */}
                    {slider.link ? (
                        <Link href={slider.link} className="block w-full h-full">
                            <img
                                src={slider.image}
                                alt={slider.title || slider.name}
                                className="w-full h-full object-cover object-center"
                                loading={index === 0 ? "eager" : "lazy"}
                            />
                        </Link>
                    ) : (
                        <img
                            src={slider.image}
                            alt={slider.title || slider.name}
                            className="w-full h-full object-cover object-center"
                            loading={index === 0 ? "eager" : "lazy"}
                        />
                    )}
                </div>
            ))}

            {/* Navigation Arrows */}
            {sliders.length > 1 && (
                <>
                    <button
                        onClick={prevSlide}
                        className="absolute left-3 md:left-6 top-1/2 -translate-y-1/2 z-20 w-10 h-10 md:w-12 md:h-12 rounded-full bg-black/30 backdrop-blur-sm text-white hover:bg-black/50 transition-all flex items-center justify-center opacity-0 hover:opacity-100 group-hover:opacity-100 focus:opacity-100"
                        style={{ opacity: 0.6 }}
                    >
                        <ChevronLeft className="w-5 h-5 md:w-6 md:h-6" />
                    </button>
                    <button
                        onClick={nextSlide}
                        className="absolute right-3 md:right-6 top-1/2 -translate-y-1/2 z-20 w-10 h-10 md:w-12 md:h-12 rounded-full bg-black/30 backdrop-blur-sm text-white hover:bg-black/50 transition-all flex items-center justify-center opacity-0 hover:opacity-100 group-hover:opacity-100 focus:opacity-100"
                        style={{ opacity: 0.6 }}
                    >
                        <ChevronRight className="w-5 h-5 md:w-6 md:h-6" />
                    </button>
                </>
            )}

            {/* Floating Trust Badges - desktop only */}
            <div className="hidden xl:block absolute bottom-10 left-0 right-0 z-10 pointer-events-none">
                <div className="max-w-7xl mx-auto px-8 flex justify-center gap-4">
                    {[
                        { icon: "🚚", text: "Giao hàng nhanh" },
                        { icon: "💯", text: "Chính hãng 100%" },
                        { icon: "🔄", text: "Đổi trả 30 ngày" },
                        { icon: "💳", text: "Thanh toán an toàn" },
                    ].map((b, i) => (
                        <div
                            key={i}
                            className="flex items-center gap-2 px-3 py-2 rounded-full bg-white/15 backdrop-blur-md border border-white/20 text-white text-xs font-medium pointer-events-auto"
                        >
                            <span>{b.icon}</span>
                            <span>{b.text}</span>
                        </div>
                    ))}
                </div>
            </div>

            {/* Dots Navigation */}
            {sliders.length > 1 && (
                <div className="absolute bottom-4 md:bottom-6 left-1/2 -translate-x-1/2 flex gap-2 z-20">
                    {sliders.map((_, idx) => (
                        <button
                            key={idx}
                            onClick={() => goToSlide(idx)}
                            className={`h-2 rounded-full transition-all duration-300 ${currentSlide === idx
                                ? "w-8 bg-white shadow-lg"
                                : "w-2 bg-white/50 hover:bg-white/80"
                                }`}
                        />
                    ))}
                </div>
            )}
        </section>
    );
}
