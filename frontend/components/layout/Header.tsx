"use client";

import { useState, useEffect } from "react";
import {
  Search,
  ShoppingCart,
  User,
  Heart,
  Menu,
  X,
  ChevronDown,
  Moon,
  Sun,
  Bell,
  Package,
  LogOut,
  Settings,
  Sparkles,
} from "lucide-react";

const categories = [
  { name: "Điện thoại", icon: "📱", href: "#" },
  { name: "Laptop", icon: "💻", href: "#" },
  { name: "Máy tính bảng", icon: "📲", href: "#" },
  { name: "Phụ kiện", icon: "🎧", href: "#" },
  { name: "Đồng hồ", icon: "⌚", href: "#" },
  { name: "Thời trang", icon: "👔", href: "#" },
];

export default function Header() {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isSearchFocused, setIsSearchFocused] = useState(false);
  const [isUserMenuOpen, setIsUserMenuOpen] = useState(false);
  const [isDarkMode, setIsDarkMode] = useState(false);
  const [isScrolled, setIsScrolled] = useState(false);
  const [searchQuery, setSearchQuery] = useState("");

  // Handle scroll effect
  useEffect(() => {
    const handleScroll = () => {
      setIsScrolled(window.scrollY > 20);
    };
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  // Toggle dark mode
  const toggleDarkMode = () => {
    setIsDarkMode(!isDarkMode);
    document.documentElement.classList.toggle("dark");
  };

  return (
    <header
      className={`sticky top-0 z-50 transition-all duration-500 ${isScrolled
        ? "glass shadow-lg"
        : "bg-transparent"
        }`}
    >
      {/* Animated Top Bar */}
      <div className="relative overflow-hidden bg-gradient-to-r from-violet-600 via-purple-600 to-pink-600 text-white py-2.5 px-4">
        {/* Animated background shimmer */}
        <div className="absolute inset-0 bg-gradient-to-r from-transparent via-white/10 to-transparent animate-shimmer" />

        <div className="max-w-7xl mx-auto flex justify-between items-center text-sm relative">
          <div className="flex items-center gap-2 animate-fade-in-left">
            <Sparkles className="w-4 h-4 text-amber-300 animate-scale-pulse" />
            <span className="font-medium">
              🔥 Flash Sale - Giảm đến 50% toàn bộ sản phẩm!
            </span>
          </div>
          <div className="hidden md:flex items-center gap-6">
            <a
              href="#"
              className="hover:text-violet-200 transition-colors flex items-center gap-1.5 group"
            >
              <Package className="w-4 h-4 group-hover:animate-bounce-subtle" />
              Theo dõi đơn hàng
            </a>
            <a
              href="#"
              className="hover:text-violet-200 transition-colors"
            >
              Hỗ trợ 24/7
            </a>
          </div>
        </div>
      </div>

      {/* Main Header */}
      <div className="max-w-7xl mx-auto px-4 py-4">
        <div className="flex items-center justify-between gap-4 lg:gap-8">
          {/* Logo */}
          <a href="/" className="flex items-center gap-3 flex-shrink-0 group">
            <div className="relative">
              <div className="w-12 h-12 rounded-2xl gradient-primary flex items-center justify-center shadow-lg group-hover:shadow-xl transition-shadow group-hover:scale-105 transition-transform duration-300">
                <span className="text-white font-bold text-2xl">S</span>
              </div>
              {/* Glow effect on hover */}
              <div className="absolute inset-0 rounded-2xl bg-violet-500/30 blur-xl opacity-0 group-hover:opacity-100 transition-opacity duration-300" />
            </div>
            <div className="hidden sm:block">
              <span className="text-2xl font-bold gradient-text">ShopTTS</span>
              <p className="text-xs text-slate-500 dark:text-slate-400 -mt-1">
                Premium Shopping
              </p>
            </div>
          </a>

          {/* Search Bar - Enhanced */}
          <div className="flex-1 max-w-2xl hidden md:block">
            <div
              className={`relative flex items-center transition-all duration-400 ${isSearchFocused ? "scale-[1.02]" : ""
                }`}
            >
              <div className="relative flex-1">
                <input
                  type="text"
                  placeholder="Tìm kiếm sản phẩm, thương hiệu..."
                  value={searchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                  className="w-full px-5 py-3 pl-12 pr-4 rounded-l-xl border-2 border-r-0 border-slate-200 dark:border-slate-700 focus:border-violet-400 focus:outline-none transition-all duration-300 bg-white dark:bg-slate-800 text-slate-800 dark:text-white"
                  onFocus={() => setIsSearchFocused(true)}
                  onBlur={() => setIsSearchFocused(false)}
                />
                <Search
                  className={`absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 transition-colors duration-300 ${isSearchFocused ? "text-violet-500" : "text-slate-400"
                    }`}
                />
              </div>
              <button className="h-[50px] px-6 rounded-r-xl bg-gradient-to-r from-violet-600 to-purple-600 text-white font-semibold flex items-center gap-2 hover:from-violet-700 hover:to-purple-700 transition-all whitespace-nowrap">
                <Search className="w-4 h-4" />
                Tìm kiếm
              </button>

              {/* Search suggestions dropdown */}
              {isSearchFocused && searchQuery.length > 0 && (
                <div className="absolute top-full left-0 right-0 mt-2 glass-card rounded-xl p-2 animate-fade-in-down z-50">
                  <div className="p-3 text-sm text-slate-500">
                    Tìm kiếm: &ldquo;{searchQuery}&rdquo;
                  </div>
                </div>
              )}
            </div>
          </div>

          {/* Right Actions */}
          <div className="flex items-center gap-2 sm:gap-3">
            {/* Dark Mode Toggle */}
            <button
              onClick={toggleDarkMode}
              className="btn-icon relative group"
              aria-label="Toggle dark mode"
            >
              <div className="relative w-6 h-6">
                <Sun className={`absolute inset-0 w-6 h-6 text-amber-500 transition-all duration-500 ${isDarkMode ? "rotate-90 scale-0 opacity-0" : "rotate-0 scale-100 opacity-100"
                  }`} />
                <Moon className={`absolute inset-0 w-6 h-6 text-violet-400 transition-all duration-500 ${isDarkMode ? "rotate-0 scale-100 opacity-100" : "-rotate-90 scale-0 opacity-0"
                  }`} />
              </div>
            </button>

            {/* Notifications */}
            <button className="btn-icon relative group">
              <Bell className="w-6 h-6 text-slate-600 dark:text-slate-300 group-hover:text-violet-600 transition-colors" />
              <span className="absolute -top-0.5 -right-0.5 w-5 h-5 bg-rose-500 text-white text-xs rounded-full flex items-center justify-center font-medium animate-scale-pulse">
                2
              </span>
            </button>

            {/* Wishlist */}
            <button className="btn-icon relative group">
              <Heart className="w-6 h-6 text-slate-600 dark:text-slate-300 group-hover:text-pink-500 transition-colors" />
              <span className="absolute -top-0.5 -right-0.5 w-5 h-5 bg-pink-500 text-white text-xs rounded-full flex items-center justify-center font-medium">
                3
              </span>
            </button>

            {/* Cart */}
            <button className="btn-icon relative group">
              <ShoppingCart className="w-6 h-6 text-slate-600 dark:text-slate-300 group-hover:text-violet-600 transition-colors" />
              <span className="absolute -top-0.5 -right-0.5 w-5 h-5 bg-violet-600 text-white text-xs rounded-full flex items-center justify-center font-medium">
                5
              </span>
            </button>

            {/* User Menu */}
            <div className="relative">
              <button
                onClick={() => setIsUserMenuOpen(!isUserMenuOpen)}
                className="hidden sm:flex items-center gap-2 px-4 py-2.5 rounded-xl hover:bg-violet-50 dark:hover:bg-violet-900/20 transition-all group"
              >
                <div className="w-9 h-9 rounded-xl bg-gradient-to-br from-violet-500 to-pink-500 flex items-center justify-center shadow-md group-hover:shadow-lg transition-shadow">
                  <User className="w-5 h-5 text-white" />
                </div>
                <div className="text-left hidden lg:block">
                  <p className="text-xs text-slate-500 dark:text-slate-400">Xin chào</p>
                  <p className="text-sm font-semibold text-slate-800 dark:text-white">
                    Tài khoản
                  </p>
                </div>
                <ChevronDown className={`w-4 h-4 text-slate-400 transition-transform duration-300 ${isUserMenuOpen ? "rotate-180" : ""
                  }`} />
              </button>

              {/* User Dropdown */}
              {isUserMenuOpen && (
                <div className="absolute right-0 top-full mt-2 w-64 glass-card rounded-2xl p-2 animate-fade-in-down shadow-2xl">
                  <div className="p-4 border-b border-slate-100 dark:border-slate-700">
                    <p className="font-semibold text-slate-800 dark:text-white">
                      Chào mừng bạn!
                    </p>
                    <p className="text-sm text-slate-500 dark:text-slate-400">
                      Đăng nhập để xem ưu đãi
                    </p>
                  </div>
                  <nav className="py-2">
                    {[
                      { icon: User, label: "Tài khoản của tôi", href: "#" },
                      { icon: Package, label: "Đơn hàng", href: "#" },
                      { icon: Heart, label: "Yêu thích", href: "#" },
                      { icon: Settings, label: "Cài đặt", href: "#" },
                    ].map((item, idx) => (
                      <a
                        key={idx}
                        href={item.href}
                        className="flex items-center gap-3 px-4 py-3 text-slate-600 dark:text-slate-300 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-colors"
                      >
                        <item.icon className="w-5 h-5" />
                        {item.label}
                      </a>
                    ))}
                  </nav>
                  <div className="p-2 border-t border-slate-100 dark:border-slate-700">
                    <button className="w-full flex items-center justify-center gap-2 btn-primary py-3">
                      Đăng nhập
                    </button>
                  </div>
                </div>
              )}
            </div>

            {/* Mobile Menu Toggle */}
            <button
              className="md:hidden btn-icon"
              onClick={() => setIsMenuOpen(!isMenuOpen)}
            >
              <div className="relative w-6 h-6">
                <Menu className={`absolute inset-0 w-6 h-6 text-slate-600 transition-all duration-300 ${isMenuOpen ? "rotate-90 scale-0 opacity-0" : "rotate-0 scale-100 opacity-100"
                  }`} />
                <X className={`absolute inset-0 w-6 h-6 text-slate-600 transition-all duration-300 ${isMenuOpen ? "rotate-0 scale-100 opacity-100" : "-rotate-90 scale-0 opacity-0"
                  }`} />
              </div>
            </button>
          </div>
        </div>

        {/* Mobile Search */}
        <div className="mt-4 md:hidden">
          <div className="relative">
            <input
              type="text"
              placeholder="Tìm kiếm sản phẩm..."
              className="input-search pl-12"
            />
            <Search className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 w-5 h-5" />
          </div>
        </div>
      </div>

      {/* Navigation - Desktop */}
      <nav className="border-t border-slate-100 dark:border-slate-800 hidden md:block">
        <div className="max-w-7xl mx-auto px-4">
          <ul className="flex items-center gap-1">
            <li>
              <button
                className="flex items-center gap-2 px-5 py-3.5 text-slate-700 dark:text-slate-200 hover:text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-all font-semibold group"
              >
                <Menu className="w-5 h-5 group-hover:rotate-180 transition-transform duration-300" />
                Danh mục
                <ChevronDown className="w-4 h-4" />
              </button>
            </li>
            {categories.map((cat, idx) => (
              <li key={cat.name}>
                <a
                  href={cat.href}
                  className="flex items-center gap-2 px-4 py-3.5 text-slate-600 dark:text-slate-300 hover:text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-all group"
                  style={{ animationDelay: `${idx * 0.05}s` }}
                >
                  <span className="text-lg group-hover:scale-125 transition-transform duration-300">
                    {cat.icon}
                  </span>
                  <span className="group-hover:font-medium transition-all">
                    {cat.name}
                  </span>
                </a>
              </li>
            ))}
          </ul>
        </div>
      </nav>

      {/* Mobile Menu */}
      <div
        className={`md:hidden overflow-hidden transition-all duration-500 ${isMenuOpen ? "max-h-screen opacity-100" : "max-h-0 opacity-0"
          }`}
      >
        <div className="glass border-t border-slate-100 dark:border-slate-800">
          <nav className="max-w-7xl mx-auto px-4 py-4">
            <ul className="space-y-1">
              {categories.map((cat, idx) => (
                <li
                  key={cat.name}
                  className="animate-fade-in-left"
                  style={{ animationDelay: `${idx * 0.08}s` }}
                >
                  <a
                    href={cat.href}
                    className="flex items-center gap-3 px-4 py-4 text-slate-600 dark:text-slate-300 hover:text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-all"
                  >
                    <span className="text-2xl">{cat.icon}</span>
                    <span className="font-medium">{cat.name}</span>
                  </a>
                </li>
              ))}
            </ul>
          </nav>
        </div>
      </div>
    </header>
  );
}
