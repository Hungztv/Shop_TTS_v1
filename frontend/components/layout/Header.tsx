"use client";

import { useState } from "react";
import {
  Search,
  ShoppingCart,
  User,
  Heart,
  Menu,
  X,
  ChevronDown,
} from "lucide-react";

const categories = [
  "Điện thoại",
  "Laptop",
  "Máy tính bảng",
  "Phụ kiện",
  "Đồng hồ",
  "Thời trang",
];

export default function Header() {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isSearchFocused, setIsSearchFocused] = useState(false);

  return (
    <header className="sticky top-0 z-50 glass">
      {/* Top Bar */}
      <div className="bg-gradient-to-r from-violet-600 to-purple-700 text-white py-2 px-4">
        <div className="max-w-7xl mx-auto flex justify-between items-center text-sm">
          <div className="flex items-center gap-4">
            <span>🔥 Giảm giá đến 50% - Chỉ hôm nay!</span>
          </div>
          <div className="hidden md:flex items-center gap-4">
            <a href="#" className="hover:text-violet-200 transition-colors">
              Hỗ trợ
            </a>
            <a href="#" className="hover:text-violet-200 transition-colors">
              Theo dõi đơn hàng
            </a>
          </div>
        </div>
      </div>

      {/* Main Header */}
      <div className="max-w-7xl mx-auto px-4 py-4">
        <div className="flex items-center justify-between gap-4">
          {/* Logo */}
          <a href="/" className="flex items-center gap-2 flex-shrink-0">
            <div className="w-10 h-10 rounded-xl gradient-primary flex items-center justify-center shadow-lg">
              <span className="text-white font-bold text-xl">S</span>
            </div>
            <span className="text-2xl font-bold gradient-text hidden sm:block">
              ShopTTS
            </span>
          </a>

          {/* Search Bar */}
          <div className="flex-1 max-w-2xl hidden md:block">
            <div
              className={`relative transition-all duration-300 ${
                isSearchFocused ? "scale-105" : ""
              }`}
            >
              <input
                type="text"
                placeholder="Tìm kiếm sản phẩm..."
                className="w-full px-5 py-3 pl-12 rounded-xl border-2 border-violet-100 focus:border-violet-400 focus:outline-none transition-all duration-300 bg-white/80 backdrop-blur-sm"
                onFocus={() => setIsSearchFocused(true)}
                onBlur={() => setIsSearchFocused(false)}
              />
              <Search className="absolute left-4 top-1/2 -translate-y-1/2 text-violet-400 w-5 h-5" />
              <button className="absolute right-2 top-1/2 -translate-y-1/2 btn-primary py-2 px-4 text-sm">
                Tìm kiếm
              </button>
            </div>
          </div>

          {/* Right Actions */}
          <div className="flex items-center gap-2 sm:gap-4">
            {/* Wishlist */}
            <button className="relative p-2 rounded-xl hover:bg-violet-50 transition-colors group">
              <Heart className="w-6 h-6 text-slate-600 group-hover:text-violet-600 transition-colors" />
              <span className="absolute -top-1 -right-1 w-5 h-5 bg-pink-500 text-white text-xs rounded-full flex items-center justify-center">
                3
              </span>
            </button>

            {/* Cart */}
            <button className="relative p-2 rounded-xl hover:bg-violet-50 transition-colors group">
              <ShoppingCart className="w-6 h-6 text-slate-600 group-hover:text-violet-600 transition-colors" />
              <span className="absolute -top-1 -right-1 w-5 h-5 bg-violet-600 text-white text-xs rounded-full flex items-center justify-center">
                5
              </span>
            </button>

            {/* User */}
            <button className="hidden sm:flex items-center gap-2 px-4 py-2 rounded-xl hover:bg-violet-50 transition-colors group">
              <User className="w-6 h-6 text-slate-600 group-hover:text-violet-600 transition-colors" />
              <span className="text-sm font-medium text-slate-700">
                Tài khoản
              </span>
              <ChevronDown className="w-4 h-4 text-slate-400" />
            </button>

            {/* Mobile Menu Toggle */}
            <button
              className="md:hidden p-2 rounded-xl hover:bg-violet-50 transition-colors"
              onClick={() => setIsMenuOpen(!isMenuOpen)}
            >
              {isMenuOpen ? (
                <X className="w-6 h-6 text-slate-600" />
              ) : (
                <Menu className="w-6 h-6 text-slate-600" />
              )}
            </button>
          </div>
        </div>

        {/* Mobile Search */}
        <div className="mt-4 md:hidden">
          <div className="relative">
            <input
              type="text"
              placeholder="Tìm kiếm sản phẩm..."
              className="w-full px-5 py-3 pl-12 rounded-xl border-2 border-violet-100 focus:border-violet-400 focus:outline-none transition-all duration-300"
            />
            <Search className="absolute left-4 top-1/2 -translate-y-1/2 text-violet-400 w-5 h-5" />
          </div>
        </div>
      </div>

      {/* Navigation */}
      <nav className="border-t border-violet-100 hidden md:block">
        <div className="max-w-7xl mx-auto px-4">
          <ul className="flex items-center gap-1">
            <li>
              <a
                href="#"
                className="flex items-center gap-2 px-4 py-3 text-slate-700 hover:text-violet-600 hover:bg-violet-50 rounded-lg transition-all font-medium"
              >
                <Menu className="w-4 h-4" />
                Danh mục
                <ChevronDown className="w-4 h-4" />
              </a>
            </li>
            {categories.map((cat) => (
              <li key={cat}>
                <a
                  href="#"
                  className="px-4 py-3 text-slate-600 hover:text-violet-600 hover:bg-violet-50 rounded-lg transition-all block"
                >
                  {cat}
                </a>
              </li>
            ))}
          </ul>
        </div>
      </nav>

      {/* Mobile Menu */}
      {isMenuOpen && (
        <div className="md:hidden border-t border-violet-100 bg-white animate-fade-in-up">
          <nav className="max-w-7xl mx-auto px-4 py-4">
            <ul className="space-y-2">
              {categories.map((cat) => (
                <li key={cat}>
                  <a
                    href="#"
                    className="block px-4 py-3 text-slate-600 hover:text-violet-600 hover:bg-violet-50 rounded-lg transition-all"
                  >
                    {cat}
                  </a>
                </li>
              ))}
            </ul>
          </nav>
        </div>
      )}
    </header>
  );
}
