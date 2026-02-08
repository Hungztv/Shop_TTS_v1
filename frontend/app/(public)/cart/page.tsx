"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import {
  ShoppingCart,
  Trash2,
  Minus,
  Plus,
  ArrowLeft,
  ShoppingBag,
  PackageX,
  Loader2,
} from "lucide-react";
import { useCart } from "@/contexts/CartContext";
import { useAuth } from "@/contexts/AuthContext";
import { toast } from "sonner";

export default function CartPage() {
  const { cart, isLoading, fetchCart, updateItem, removeItem, clearAll } =
    useCart();
  const { isAuthenticated } = useAuth();
  const [updatingIds, setUpdatingIds] = useState<Set<number>>(new Set());
  const [removingIds, setRemovingIds] = useState<Set<number>>(new Set());
  const [isClearing, setIsClearing] = useState<boolean>(false);

  // Tải giỏ hàng khi mount
  useEffect(() => {
    if (isAuthenticated) {
      fetchCart();
    }
  }, [isAuthenticated, fetchCart]);

  const formatPrice = (value: number): string => {
    return new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(value);
  };

  // Cập nhật số lượng
  const handleUpdateQuantity = async (
    cartId: number,
    newQuantity: number,
    maxQuantity: number
  ) => {
    if (newQuantity < 1 || newQuantity > maxQuantity) return;

    setUpdatingIds((prev) => new Set(prev).add(cartId));
    const success = await updateItem(cartId, newQuantity);
    setUpdatingIds((prev) => {
      const next = new Set(prev);
      next.delete(cartId);
      return next;
    });

    if (success) {
      toast.success("Đã cập nhật số lượng");
    } else {
      toast.error("Không thể cập nhật số lượng");
    }
  };

  // Xoá sản phẩm
  const handleRemoveItem = async (cartId: number, productName: string) => {
    setRemovingIds((prev) => new Set(prev).add(cartId));
    const success = await removeItem(cartId);
    setRemovingIds((prev) => {
      const next = new Set(prev);
      next.delete(cartId);
      return next;
    });

    if (success) {
      toast.success(`Đã xoá "${productName}" khỏi giỏ hàng`);
    } else {
      toast.error("Không thể xoá sản phẩm");
    }
  };

  // Xoá toàn bộ
  const handleClearCart = async () => {
    if (!confirm("Bạn có chắc muốn xoá toàn bộ giỏ hàng?")) return;

    setIsClearing(true);
    const success = await clearAll();
    setIsClearing(false);

    if (success) {
      toast.success("Đã xoá toàn bộ giỏ hàng");
    } else {
      toast.error("Không thể xoá giỏ hàng");
    }
  };

  // Chưa đăng nhập
  if (!isAuthenticated) {
    return (
      <div className="min-h-[60vh] flex flex-col items-center justify-center px-4">
        <div className="w-20 h-20 rounded-2xl bg-violet-100 dark:bg-violet-900/30 flex items-center justify-center mb-6">
          <ShoppingCart className="w-10 h-10 text-violet-500" />
        </div>
        <h2 className="text-2xl font-bold text-slate-800 dark:text-white mb-2">
          Đăng nhập để xem giỏ hàng
        </h2>
        <p className="text-slate-500 dark:text-slate-400 mb-6 text-center">
          Bạn cần đăng nhập để sử dụng giỏ hàng
        </p>
        <Link
          href="/login?redirect=/cart"
          className="px-6 py-3 bg-violet-600 hover:bg-violet-700 text-white font-semibold rounded-xl transition-colors"
        >
          Đăng nhập ngay
        </Link>
      </div>
    );
  }

  // Loading
  if (isLoading && !cart) {
    return (
      <div className="max-w-7xl mx-auto px-4 py-8">
        <div className="animate-pulse space-y-4">
          <div className="h-8 bg-slate-200 dark:bg-slate-700 rounded-lg w-48" />
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            <div className="lg:col-span-2 space-y-4">
              {[1, 2, 3].map((i) => (
                <div
                  key={i}
                  className="h-32 bg-slate-200 dark:bg-slate-700 rounded-xl"
                />
              ))}
            </div>
            <div className="h-64 bg-slate-200 dark:bg-slate-700 rounded-xl" />
          </div>
        </div>
      </div>
    );
  }

  // Giỏ trống
  if (!cart || cart.items.length === 0) {
    return (
      <div className="min-h-[60vh] flex flex-col items-center justify-center px-4">
        <div className="w-24 h-24 rounded-2xl bg-slate-100 dark:bg-slate-800 flex items-center justify-center mb-6">
          <PackageX className="w-12 h-12 text-slate-400" />
        </div>
        <h2 className="text-2xl font-bold text-slate-800 dark:text-white mb-2">
          Giỏ hàng trống
        </h2>
        <p className="text-slate-500 dark:text-slate-400 mb-6 text-center max-w-md">
          Bạn chưa có sản phẩm nào trong giỏ hàng. Hãy khám phá các sản phẩm
          hấp dẫn!
        </p>
        <Link
          href="/products"
          className="flex items-center gap-2 px-6 py-3 bg-violet-600 hover:bg-violet-700 text-white font-semibold rounded-xl transition-colors"
        >
          <ShoppingBag className="w-5 h-5" />
          Khám phá sản phẩm
        </Link>
      </div>
    );
  }

  return (
    <div className="max-w-7xl mx-auto px-4 py-6 sm:py-8">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 mb-6">
        <div>
          <h1 className="text-2xl sm:text-3xl font-bold text-slate-800 dark:text-white">
            Giỏ hàng
          </h1>
          <p className="text-slate-500 dark:text-slate-400 text-sm mt-1">
            {cart.totalItems} sản phẩm trong giỏ
          </p>
        </div>
        <button
          onClick={handleClearCart}
          disabled={isClearing}
          className="flex items-center gap-2 px-4 py-2.5 text-sm font-medium text-red-500 border border-red-200 dark:border-red-800 hover:bg-red-50 dark:hover:bg-red-900/20 rounded-xl transition-colors disabled:opacity-50"
        >
          {isClearing ? (
            <Loader2 className="w-4 h-4 animate-spin" />
          ) : (
            <Trash2 className="w-4 h-4" />
          )}
          Xoá tất cả
        </button>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 lg:gap-8">
        {/* Danh sách sản phẩm */}
        <div className="lg:col-span-2 space-y-3">
          {cart.items.map((item) => {
            const isUpdating = updatingIds.has(item.id);
            const isRemoving = removingIds.has(item.id);

            return (
              <div
                key={item.id}
                className={`flex gap-4 p-4 bg-white dark:bg-slate-800 rounded-xl border border-slate-100 dark:border-slate-700 transition-all ${
                  isRemoving ? "opacity-50 scale-[0.98]" : ""
                }`}
              >
                {/* Ảnh sản phẩm */}
                <Link
                  href={`/products/${item.productSlug || item.productId}`}
                  className="flex-shrink-0"
                >
                  <div className="w-20 h-20 sm:w-24 sm:h-24 rounded-xl overflow-hidden bg-slate-100 dark:bg-slate-700">
                    <img
                      src={item.productImage}
                      alt={item.productName}
                      className="w-full h-full object-cover hover:scale-105 transition-transform"
                    />
                  </div>
                </Link>

                {/* Thông tin */}
                <div className="flex-1 min-w-0">
                  <Link
                    href={`/products/${item.productSlug || item.productId}`}
                  >
                    <h3 className="font-semibold text-slate-800 dark:text-white text-sm sm:text-base line-clamp-2 hover:text-violet-600 transition-colors">
                      {item.productName}
                    </h3>
                  </Link>

                  <p className="text-sm text-violet-600 font-medium mt-1">
                    {formatPrice(item.price)}
                  </p>

                  {/* Số lượng + Hành động */}
                  <div className="flex items-center justify-between mt-3 flex-wrap gap-2">
                    {/* Nút +/- */}
                    <div className="flex items-center gap-0.5">
                      <button
                        onClick={() =>
                          handleUpdateQuantity(
                            item.id,
                            item.quantity - 1,
                            item.maxQuantity
                          )
                        }
                        disabled={isUpdating || item.quantity <= 1}
                        className="w-8 h-8 flex items-center justify-center rounded-lg border border-slate-200 dark:border-slate-600 hover:bg-slate-100 dark:hover:bg-slate-700 transition-colors disabled:opacity-40"
                      >
                        <Minus className="w-3.5 h-3.5" />
                      </button>

                      <span className="w-10 h-8 flex items-center justify-center text-sm font-semibold text-slate-800 dark:text-white">
                        {isUpdating ? (
                          <Loader2 className="w-3.5 h-3.5 animate-spin" />
                        ) : (
                          item.quantity
                        )}
                      </span>

                      <button
                        onClick={() =>
                          handleUpdateQuantity(
                            item.id,
                            item.quantity + 1,
                            item.maxQuantity
                          )
                        }
                        disabled={
                          isUpdating || item.quantity >= item.maxQuantity
                        }
                        className="w-8 h-8 flex items-center justify-center rounded-lg border border-slate-200 dark:border-slate-600 hover:bg-slate-100 dark:hover:bg-slate-700 transition-colors disabled:opacity-40"
                      >
                        <Plus className="w-3.5 h-3.5" />
                      </button>
                    </div>

                    {/* Subtotal + Xoá */}
                    <div className="flex items-center gap-3">
                      <span className="font-bold text-slate-800 dark:text-white text-sm sm:text-base">
                        {formatPrice(item.subtotal)}
                      </span>
                      <button
                        onClick={() =>
                          handleRemoveItem(item.id, item.productName)
                        }
                        disabled={isRemoving}
                        className="p-2 text-slate-400 hover:text-red-500 hover:bg-red-50 dark:hover:bg-red-900/20 rounded-lg transition-colors"
                        title="Xoá sản phẩm"
                      >
                        <Trash2 className="w-4 h-4" />
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            );
          })}
        </div>

        {/* Tóm tắt đơn hàng */}
        <div className="lg:col-span-1">
          <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-100 dark:border-slate-700 p-5 sticky top-24">
            <h2 className="text-lg font-bold text-slate-800 dark:text-white mb-4">
              Tóm tắt đơn hàng
            </h2>

            <div className="space-y-3 text-sm">
              <div className="flex justify-between text-slate-600 dark:text-slate-400">
                <span>Tạm tính ({cart.totalItems} sản phẩm)</span>
                <span>{formatPrice(cart.totalPrice)}</span>
              </div>
              <div className="flex justify-between text-slate-600 dark:text-slate-400">
                <span>Phí vận chuyển</span>
                <span className="text-emerald-500 font-medium">Miễn phí</span>
              </div>
              <div className="border-t border-slate-100 dark:border-slate-700 pt-3">
                <div className="flex justify-between items-center">
                  <span className="font-bold text-slate-800 dark:text-white text-base">
                    Tổng cộng
                  </span>
                  <span className="text-xl font-bold text-violet-600">
                    {formatPrice(cart.totalPrice)}
                  </span>
                </div>
              </div>
            </div>

            {/* CTA */}
            <div className="mt-6 space-y-3">
              <Link
                href="/checkout"
                className="w-full flex items-center justify-center gap-2 py-3.5 bg-violet-600 hover:bg-violet-700 text-white font-semibold rounded-xl transition-colors text-sm"
              >
                <ShoppingBag className="w-4 h-4" />
                Tiến hành thanh toán
              </Link>
              <Link
                href="/products"
                className="w-full flex items-center justify-center gap-2 py-3 border border-slate-200 dark:border-slate-600 text-slate-700 dark:text-slate-300 hover:bg-slate-50 dark:hover:bg-slate-700 font-medium rounded-xl transition-colors text-sm"
              >
                <ArrowLeft className="w-4 h-4" />
                Tiếp tục mua sắm
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
