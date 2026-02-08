"use client";

import { X, ArrowLeftRight } from "lucide-react";
import { useCompare } from "@/contexts/CompareContext";
import { useAuth } from "@/contexts/AuthContext";
import Link from "next/link";

/**
 * CompareBar – fixed bottom bar hiển thị khi có sản phẩm trong danh sách so sánh
 */
export default function CompareBar() {
  const { compare, compareCount, maxItems, removeItem } = useCompare();
  const { isAuthenticated } = useAuth();

  if (!isAuthenticated || compareCount === 0) return null;

  const items = compare?.items ?? [];

  return (
    <div className="fixed bottom-0 left-0 right-0 z-40 bg-white/95 dark:bg-slate-900/95 backdrop-blur-lg border-t border-slate-200 dark:border-slate-700 shadow-[0_-4px_20px_rgba(0,0,0,0.1)]">
      <div className="max-w-7xl mx-auto px-4 py-3">
        <div className="flex items-center justify-between gap-4">
          {/* Left: thumbnails */}
          <div className="flex items-center gap-3 overflow-x-auto">
            <div className="flex items-center gap-2 text-sm font-medium text-slate-600 dark:text-slate-300 flex-shrink-0">
              <ArrowLeftRight className="w-4 h-4 text-violet-600" />
              <span>
                So sánh ({compareCount}/{maxItems})
              </span>
            </div>
            <div className="flex items-center gap-2">
              {items.map((item) => (
                <div
                  key={item.id}
                  className="relative group flex-shrink-0"
                >
                  <div className="w-12 h-12 rounded-lg overflow-hidden border-2 border-violet-200 dark:border-violet-700">
                    <img
                      src={item.productImage}
                      alt={item.productName}
                      className="w-full h-full object-cover"
                    />
                  </div>
                  <button
                    onClick={() => removeItem(item.id)}
                    className="absolute -top-1.5 -right-1.5 w-5 h-5 rounded-full bg-red-500 text-white flex items-center justify-center opacity-0 group-hover:opacity-100 transition-opacity"
                  >
                    <X className="w-3 h-3" />
                  </button>
                </div>
              ))}
              {/* Empty slots */}
              {[...Array(maxItems - compareCount)].map((_, i) => (
                <div
                  key={`empty-${i}`}
                  className="w-12 h-12 rounded-lg border-2 border-dashed border-slate-200 dark:border-slate-700 flex items-center justify-center flex-shrink-0"
                >
                  <span className="text-xs text-slate-300 dark:text-slate-600">+</span>
                </div>
              ))}
            </div>
          </div>

          {/* Right: action */}
          <Link
            href="/compare"
            className="flex-shrink-0 px-6 py-2.5 text-sm font-semibold text-white bg-gradient-to-r from-violet-600 to-purple-600 hover:from-violet-700 hover:to-purple-700 rounded-xl transition-all shadow-lg shadow-violet-500/25"
          >
            So sánh ngay
          </Link>
        </div>
      </div>
    </div>
  );
}
