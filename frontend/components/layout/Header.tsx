"use client";

import { useState, useEffect } from "react";
import Link from "next/link";
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
import { useAuth } from "@/contexts/AuthContext";

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
  const [isCategoryOpen, setIsCategoryOpen] = useState(false);
  const [isDarkMode, setIsDarkMode] = useState(false);
  const [isScrolled, setIsScrolled] = useState(false);
  const [searchQuery, setSearchQuery] = useState("");

  // Auth state
  const { user, isAuthenticated, signOut } = useAuth();

  // Handle sign out
  const handleSignOut = async () => {
    await signOut();
    setIsUserMenuOpen(false);
  };

  // Handle scroll effect
  useEffect(() => {
    const handleScroll = () => {
      setIsScrolled(window.scrollY > 20);
    };
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  // Click outside handler - đóng tất cả dropdown khi bấm ra ngoài
  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      const target = e.target as HTMLElement;
      if (!target.closest("[data-dropdown='user']")) {
        setIsUserMenuOpen(false);
      }
      if (!target.closest("[data-dropdown='category']")) {
        setIsCategoryOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  // Toggle category dropdown
  const toggleCategory = () => {
    setIsCategoryOpen((prev) => !prev);
    setIsUserMenuOpen(false); // đóng user menu nếu đang mở
  };

  // Toggle user menu
  const toggleUserMenu = () => {
    setIsUserMenuOpen((prev) => !prev);
    setIsCategoryOpen(false); // đóng category nếu đang mở
  };

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
            <div className="relative" data-dropdown="user">
              <button
                onClick={toggleUserMenu}
                className="hidden sm:flex items-center gap-2 px-4 py-2.5 rounded-xl hover:bg-violet-50 dark:hover:bg-violet-900/20 transition-all group"
              >
                <div className="w-9 h-9 rounded-xl bg-gradient-to-br from-violet-500 to-pink-500 flex items-center justify-center shadow-md group-hover:shadow-lg transition-shadow">
                  <User className="w-5 h-5 text-white" />
                </div>
                <div className="text-left hidden lg:block">
                  <p className="text-xs text-slate-500 dark:text-slate-400">Xin chào</p>
                  <p className="text-sm font-semibold text-slate-800 dark:text-white">
                    {isAuthenticated ? (user?.metadata?.full_name || user?.email?.split('@')[0] || 'Bạn') : 'Tài khoản'}
                  </p>
                </div>
                <ChevronDown className={`w-4 h-4 text-slate-400 transition-transform duration-300 ${isUserMenuOpen ? "rotate-180" : ""
                  }`} />
              </button>

              {/* User Dropdown */}
              {isUserMenuOpen && (
                <div className="absolute right-0 top-full mt-2 w-64 glass-card rounded-2xl p-2 animate-fade-in-down shadow-2xl">
                  {isAuthenticated ? (
                    // ĐÃ ĐĂNG NHẬP
                    <>
                      <div className="p-4 border-b border-slate-100 dark:border-slate-700">
                        <p className="font-semibold text-slate-800 dark:text-white">
                          {user?.metadata?.full_name || 'Xin chào!'}
                        </p>
                        <p className="text-sm text-slate-500 dark:text-slate-400">
                          {user?.email}
                        </p>
                      </div>
                      <nav className="py-2">
                        {[
                          { icon: User, label: "Tài khoản của tôi", href: "/account" },
                          { icon: Package, label: "Đơn hàng", href: "/orders" },
                          { icon: Heart, label: "Yêu thích", href: "/wishlist" },
                          { icon: Settings, label: "Cài đặt", href: "/settings" },
                        ].map((item, idx) => (
                          <Link
                            key={idx}
                            href={item.href}
                            className="flex items-center gap-3 px-4 py-3 text-slate-600 dark:text-slate-300 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-colors"
                            onClick={() => setIsUserMenuOpen(false)}
                          >
                            <item.icon className="w-5 h-5" />
                            {item.label}
                          </Link>
                        ))}
                      </nav>
                      <div className="p-2 border-t border-slate-100 dark:border-slate-700">
                        <button
                          onClick={handleSignOut}
                          className="w-full flex items-center justify-center gap-2 text-red-500 hover:bg-red-50 dark:hover:bg-red-900/20 py-3 rounded-xl transition-colors"
                        >
                          <LogOut className="w-5 h-5" />
                          Đăng xuất
                        </button>
                      </div>
                    </>
                  ) : (
                    // CHƯA ĐĂNG NHẬP
                    <>
                      <div className="p-4 border-b border-slate-100 dark:border-slate-700">
                        <p className="font-semibold text-slate-800 dark:text-white">
                          Chào mừng bạn!
                        </p>
                        <p className="text-sm text-slate-500 dark:text-slate-400">
                          Đăng nhập để xem ưu đãi
                        </p>
                      </div>
                      <div className="p-3 space-y-2">
                        <Link
                          href="/login"
                          className="w-full flex items-center justify-center gap-2 btn-primary py-3"
                          onClick={() => setIsUserMenuOpen(false)}
                        >
                          Đăng nhập
                        </Link>
                        <Link
                          href="/register"
                          className="w-full flex items-center justify-center gap-2 border-2 border-violet-500 text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 py-3 rounded-xl transition-colors font-semibold"
                          onClick={() => setIsUserMenuOpen(false)}
                        >
                          Đăng ký
                        </Link>
                      </div>
                    </>
                  )}
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
            {/* Dropdown Danh mục */}
            <li className="relative" data-dropdown="category">
              <button
                onClick={toggleCategory}
                className={`flex items-center gap-2 px-5 py-3.5 rounded-xl transition-all font-semibold group ${
                  isCategoryOpen
                    ? "text-violet-600 bg-violet-50 dark:bg-violet-900/20"
                    : "text-slate-700 dark:text-slate-200 hover:text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20"
                }`}
              >
                <Menu className={`w-5 h-5 transition-transform duration-300 ${isCategoryOpen ? "rotate-180" : "group-hover:rotate-180"}`} />
                Danh mục
                <ChevronDown className={`w-4 h-4 transition-transform duration-300 ${isCategoryOpen ? "rotate-180" : ""}`} />
              </button>

              {/* Category Dropdown Menu */}
              {isCategoryOpen && (
                <div className="absolute left-0 top-full mt-1 w-64 glass-card rounded-2xl p-2 animate-fade-in-down shadow-2xl z-50">
                  <div className="p-3 border-b border-slate-100 dark:border-slate-700">
                    <p className="font-semibold text-slate-800 dark:text-white text-sm">Tất cả danh mục</p>
                  </div>
                  <nav className="py-1">
                    {categories.map((cat, idx) => (
                      <Link
                        key={cat.name}
                        href={`/categories/${cat.name.toLowerCase().replace(/\s+/g, "-")}`}
                        className="flex items-center gap-3 px-4 py-3 text-slate-600 dark:text-slate-300 hover:text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-all group"
                        style={{ animationDelay: `${idx * 0.05}s` }}
                        onClick={() => setIsCategoryOpen(false)}
                      >
                        <span className="text-xl group-hover:scale-125 transition-transform duration-300">
                          {cat.icon}
                        </span>
                        <span className="group-hover:font-medium transition-all">
                          {cat.name}
                        </span>
                      </Link>
                    ))}
                  </nav>
                </div>
              )}
            </li>

            {/* Quick Nav Links */}
            <li>
              <Link
                href="/"
                className="flex items-center gap-2 px-4 py-3.5 text-slate-600 dark:text-slate-300 hover:text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-all font-medium"
              >
                Trang chủ
              </Link>
            </li>
            <li>
              <Link
                href="/products"
                className="flex items-center gap-2 px-4 py-3.5 text-slate-600 dark:text-slate-300 hover:text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-all font-medium"
              >
                Sản phẩm
              </Link>
            </li>
            <li>
              <Link
                href="/deals"
                className="flex items-center gap-2 px-4 py-3.5 text-slate-600 dark:text-slate-300 hover:text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-all font-medium"
              >
                🔥 Khuyến mãi
              </Link>
            </li>
            <li>
              <Link
                href="/contact"
                className="flex items-center gap-2 px-4 py-3.5 text-slate-600 dark:text-slate-300 hover:text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-all font-medium"
              >
                Liên hệ
              </Link>
            </li>
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
            {/* Mobile Navigation Links */}
            <ul className="space-y-1 mb-3">
              <li>
                <Link
                  href="/"
                  className="flex items-center gap-3 px-4 py-3 text-slate-700 dark:text-slate-200 hover:text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-all font-semibold"
                  onClick={() => setIsMenuOpen(false)}
                >
                  🏠 Trang chủ
                </Link>
              </li>
              <li>
                <Link
                  href="/products"
                  className="flex items-center gap-3 px-4 py-3 text-slate-700 dark:text-slate-200 hover:text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-all font-semibold"
                  onClick={() => setIsMenuOpen(false)}
                >
                  🛍️ Sản phẩm
                </Link>
              </li>
              <li>
                <Link
                  href="/deals"
                  className="flex items-center gap-3 px-4 py-3 text-slate-700 dark:text-slate-200 hover:text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-all font-semibold"
                  onClick={() => setIsMenuOpen(false)}
                >
                  🔥 Khuyến mãi
                </Link>
              </li>
              <li>
                <Link
                  href="/contact"
                  className="flex items-center gap-3 px-4 py-3 text-slate-700 dark:text-slate-200 hover:text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-all font-semibold"
                  onClick={() => setIsMenuOpen(false)}
                >
                  📞 Liên hệ
                </Link>
              </li>
            </ul>

            {/* Mobile Category Section */}
            <div className="border-t border-slate-100 dark:border-slate-700 pt-3">
              <p className="px-4 py-2 text-xs font-semibold text-slate-400 uppercase tracking-wider">Danh mục</p>
              <ul className="space-y-1">
                {categories.map((cat, idx) => (
                  <li
                    key={cat.name}
                    className="animate-fade-in-left"
                    style={{ animationDelay: `${idx * 0.08}s` }}
                  >
                    <Link
                      href={`/categories/${cat.name.toLowerCase().replace(/\s+/g, "-")}`}
                      className="flex items-center gap-3 px-4 py-4 text-slate-600 dark:text-slate-300 hover:text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-all"
                      onClick={() => setIsMenuOpen(false)}
                    >
                      <span className="text-2xl">{cat.icon}</span>
                      <span className="font-medium">{cat.name}</span>
                    </Link>
                  </li>
                ))}
              </ul>
            </div>
          </nav>
        </div>
      </div>
    </header>
  );
}
