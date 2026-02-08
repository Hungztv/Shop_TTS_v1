"use client";

import { useEffect, useState } from "react";
import {
  ArrowLeftRight,
  Trash2,
  Loader2,
  ArrowLeft,
  Star,
  ShoppingCart,
  X,
} from "lucide-react";
import { useCompare } from "@/contexts/CompareContext";
import { useCart } from "@/contexts/CartContext";
import { useAuth } from "@/contexts/AuthContext";
import { toast } from "sonner";
import Link from "next/link";

export default function ComparePage() {
  const { compare, isLoading, removeItem, clearAll, fetchCompare } = useCompare();
  const { addItem: addToCart, isInCart } = useCart();
  const { isAuthenticated } = useAuth();
  const [isClearing, setIsClearing] = useState(false);
  const [addingToCartIds, setAddingToCartIds] = useState<Set<number>>(new Set());

  useEffect(() => {
    if (isAuthenticated) fetchCompare();
  }, [isAuthenticated, fetchCompare]);

  const formatPrice = (v: number) =>
    new Intl.NumberFormat("vi-VN", { style: "currency", currency: "VND" }).format(v);

  const handleClearAll = async () => {
    if (!confirm("Xóa tất cả sản phẩm so sánh?")) return;
    setIsClearing(true);
    const ok = await clearAll();
    if (ok) toast.success("Đã xóa tất cả");
    else toast.error("Không thể xóa");
    setIsClearing(false);
  };

  const handleAddToCart = async (productId: number) => {
    setAddingToCartIds((prev) => new Set(prev).add(productId));
    const ok = await addToCart(productId, 1);
    if (ok) toast.success("Đã thêm vào giỏ hàng!");
    else toast.error("Không thể thêm vào giỏ");
    setAddingToCartIds((prev) => {
      const n = new Set(prev);
      n.delete(productId);
      return n;
    });
  };

  // --- NOT AUTHENTICATED ---
  if (!isAuthenticated) {
    return (
      <div className="min-h-screen bg-slate-50 dark:bg-slate-950">
        <div className="max-w-7xl mx-auto px-4 py-20 text-center">
          <ArrowLeftRight className="w-16 h-16 text-slate-300 mx-auto mb-4" />
          <h2 className="text-2xl font-bold text-slate-800 dark:text-white mb-2">
            Đăng nhập để so sánh sản phẩm
          </h2>
          <p className="text-slate-500 mb-6">
            Bạn cần đăng nhập để sử dụng tính năng so sánh
          </p>
          <Link href="/login" className="btn-primary px-8 py-3">
            Đăng nhập
          </Link>
        </div>
      </div>
    );
  }

  // --- LOADING ---
  if (isLoading) {
    return (
      <div className="min-h-screen bg-slate-50 dark:bg-slate-950 flex items-center justify-center">
        <Loader2 className="w-8 h-8 animate-spin text-violet-600" />
      </div>
    );
  }

  const items = compare?.items ?? [];

  // --- EMPTY ---
  if (items.length === 0) {
    return (
      <div className="min-h-screen bg-slate-50 dark:bg-slate-950">
        <div className="max-w-7xl mx-auto px-4 py-20 text-center">
          <ArrowLeftRight className="w-16 h-16 text-slate-300 mx-auto mb-4" />
          <h2 className="text-2xl font-bold text-slate-800 dark:text-white mb-2">
            Chưa có sản phẩm để so sánh
          </h2>
          <p className="text-slate-500 mb-6">
            Thêm sản phẩm vào danh sách so sánh từ trang sản phẩm
          </p>
          <Link
            href="/products"
            className="btn-primary px-8 py-3 inline-flex items-center gap-2"
          >
            <ShoppingCart className="w-4 h-4" />
            Xem sản phẩm
          </Link>
        </div>
      </div>
    );
  }

  // --- Comparison Rows ---
  const rows: {
    label: string;
    render: (item: (typeof items)[0]) => React.ReactNode;
  }[] = [
    {
      label: "Hình ảnh",
      render: (item) => (
        <Link href={item.productSlug ? `/products/${item.productSlug}` : `/products/${item.productId}`}>
          <img
            src={item.productImage}
            alt={item.productName}
            className="w-full h-40 object-cover rounded-xl hover:scale-105 transition-transform"
          />
        </Link>
      ),
    },
    {
      label: "Tên sản phẩm",
      render: (item) => (
        <Link
          href={item.productSlug ? `/products/${item.productSlug}` : `/products/${item.productId}`}
          className="font-semibold text-slate-800 dark:text-white hover:text-violet-600 transition-colors line-clamp-2"
        >
          {item.productName}
        </Link>
      ),
    },
    {
      label: "Giá",
      render: (item) => (
        <div>
          <span className="text-lg font-bold bg-gradient-to-r from-violet-600 to-purple-600 bg-clip-text text-transparent">
            {formatPrice(item.productPrice)}
          </span>
          {item.productCapitalPrice > item.productPrice && (
            <span className="block text-sm text-slate-400 line-through">
              {formatPrice(item.productCapitalPrice)}
            </span>
          )}
        </div>
      ),
    },
    {
      label: "Thương hiệu",
      render: (item) => (
        <span className="text-sm text-slate-600 dark:text-slate-300">
          {item.brandName}
        </span>
      ),
    },
    {
      label: "Danh mục",
      render: (item) => (
        <span className="text-sm text-violet-600 dark:text-violet-400 font-medium">
          {item.categoryName}
        </span>
      ),
    },
    {
      label: "Đánh giá",
      render: (item) => (
        <div className="flex items-center gap-1.5">
          <Star className="w-4 h-4 text-amber-400 fill-amber-400" />
          <span className="font-medium text-sm">{item.averageScore.toFixed(1)}</span>
          <span className="text-xs text-slate-400">({item.ratingCount})</span>
        </div>
      ),
    },
    {
      label: "Tình trạng",
      render: (item) => (
        <span
          className={`px-2.5 py-1 text-xs font-semibold rounded-full ${
            item.isInStock
              ? "bg-emerald-50 dark:bg-emerald-900/20 text-emerald-600"
              : "bg-red-50 dark:bg-red-900/20 text-red-500"
          }`}
        >
          {item.isInStock ? "Còn hàng" : "Hết hàng"}
        </span>
      ),
    },
    {
      label: "Đã bán",
      render: (item) => (
        <span className="text-sm text-slate-600 dark:text-slate-300">
          {item.productSoldOut.toLocaleString()}
        </span>
      ),
    },
    {
      label: "Mô tả",
      render: (item) => (
        <p className="text-sm text-slate-500 line-clamp-4">
          {item.productDescription || "—"}
        </p>
      ),
    },
  ];

  return (
    <div className="min-h-screen bg-slate-50 dark:bg-slate-950">
      <div className="max-w-7xl mx-auto px-4 py-8">
        {/* Header */}
        <div className="flex items-center justify-between mb-8">
          <div className="flex items-center gap-3">
            <Link
              href="/products"
              className="p-2 rounded-xl hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors"
            >
              <ArrowLeft className="w-5 h-5" />
            </Link>
            <div>
              <h1 className="text-2xl font-bold text-slate-800 dark:text-white">
                So sánh sản phẩm
              </h1>
              <p className="text-sm text-slate-500">
                {items.length}/{compare?.maxItems ?? 5} sản phẩm
              </p>
            </div>
          </div>
          <button
            onClick={handleClearAll}
            disabled={isClearing}
            className="flex items-center gap-2 px-4 py-2 text-sm text-red-500 hover:bg-red-50 dark:hover:bg-red-900/20 rounded-xl transition-all"
          >
            {isClearing ? (
              <Loader2 className="w-4 h-4 animate-spin" />
            ) : (
              <Trash2 className="w-4 h-4" />
            )}
            Xóa tất cả
          </button>
        </div>

        {/* Comparison Table */}
        <div className="card overflow-x-auto">
          <table className="w-full min-w-[600px]">
            <thead>
              <tr>
                <th className="text-left p-4 w-32 text-sm font-semibold text-slate-500 border-b border-slate-100 dark:border-slate-800">
                  Tiêu chí
                </th>
                {items.map((item) => (
                  <th
                    key={item.id}
                    className="p-4 text-center border-b border-slate-100 dark:border-slate-800 relative"
                    style={{ minWidth: 200 }}
                  >
                    <button
                      onClick={() => removeItem(item.id)}
                      className="absolute top-2 right-2 w-7 h-7 rounded-full bg-slate-100 dark:bg-slate-800 flex items-center justify-center hover:bg-red-100 hover:text-red-500 transition-colors"
                      title="Xóa"
                    >
                      <X className="w-3.5 h-3.5" />
                    </button>
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {rows.map((row, idx) => (
                <tr
                  key={row.label}
                  className={
                    idx % 2 === 0
                      ? "bg-slate-50/50 dark:bg-slate-800/20"
                      : ""
                  }
                >
                  <td className="p-4 text-sm font-medium text-slate-500 border-r border-slate-100 dark:border-slate-800">
                    {row.label}
                  </td>
                  {items.map((item) => (
                    <td
                      key={item.id}
                      className="p-4 text-center align-middle border-r last:border-r-0 border-slate-100 dark:border-slate-800"
                    >
                      {row.render(item)}
                    </td>
                  ))}
                </tr>
              ))}
              {/* Add to cart row */}
              <tr className="border-t border-slate-200 dark:border-slate-700">
                <td className="p-4 text-sm font-medium text-slate-500 border-r border-slate-100 dark:border-slate-800">
                  Hành động
                </td>
                {items.map((item) => {
                  const alreadyInCart = isInCart(item.productId);
                  return (
                    <td key={item.id} className="p-4 text-center">
                      <button
                        onClick={() => handleAddToCart(item.productId)}
                        disabled={
                          !item.isInStock ||
                          addingToCartIds.has(item.productId) ||
                          alreadyInCart
                        }
                        className={`w-full py-2.5 text-sm font-medium rounded-xl transition-all ${
                          alreadyInCart
                            ? "bg-emerald-50 dark:bg-emerald-900/20 text-emerald-600 border border-emerald-200 dark:border-emerald-800"
                            : !item.isInStock
                            ? "bg-slate-100 dark:bg-slate-800 text-slate-400 cursor-not-allowed"
                            : "btn-primary"
                        }`}
                      >
                        {addingToCartIds.has(item.productId) ? (
                          <Loader2 className="w-4 h-4 animate-spin mx-auto" />
                        ) : alreadyInCart ? (
                          "✓ Đã trong giỏ"
                        ) : item.isInStock ? (
                          "Thêm vào giỏ"
                        ) : (
                          "Hết hàng"
                        )}
                      </button>
                    </td>
                  );
                })}
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
