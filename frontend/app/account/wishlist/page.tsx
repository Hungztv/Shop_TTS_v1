"use client";

import { useEffect, useState } from "react";
import { Heart, Trash2, ShoppingCart, Loader2, ArrowLeft, Package } from "lucide-react";
import { useWishlist } from "@/contexts/WishlistContext";
import { useCart } from "@/contexts/CartContext";
import { useAuth } from "@/contexts/AuthContext";
import { toast } from "sonner";
import Link from "next/link";
import { formatPrice } from "@/lib/utils/product-mapper";

export default function WishlistPage() {
  const { wishlist, isLoading, removeItem, clearAll, fetchWishlist } = useWishlist();
  const { addItem: addToCart, isInCart } = useCart();
  const { isAuthenticated } = useAuth();
  const [removingIds, setRemovingIds] = useState<Set<number>>(new Set());
  const [addingToCartIds, setAddingToCartIds] = useState<Set<number>>(new Set());
  const [isClearing, setIsClearing] = useState(false);

  useEffect(() => {
    if (isAuthenticated) fetchWishlist();
  }, [isAuthenticated, fetchWishlist]);

  const handleRemove = async (wishlistId: number) => {
    setRemovingIds((prev) => new Set(prev).add(wishlistId));
    const ok = await removeItem(wishlistId);
    if (ok) toast.success("Đã xóa khỏi yêu thích");
    else toast.error("Không thể xóa");
    setRemovingIds((prev) => {
      const n = new Set(prev);
      n.delete(wishlistId);
      return n;
    });
  };

  const handleClearAll = async () => {
    if (!confirm("Xóa tất cả sản phẩm yêu thích?")) return;
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
        <div className="max-w-7xl mx-auto px-4 py-20">
          <div className="text-center">
            <Heart className="w-16 h-16 text-slate-300 mx-auto mb-4" />
            <h2 className="text-2xl font-bold text-slate-800 dark:text-white mb-2">
              Đăng nhập để xem Wishlist
            </h2>
            <p className="text-slate-500 mb-6">
              Bạn cần đăng nhập để lưu sản phẩm yêu thích
            </p>
            <Link href="/login" className="btn-primary px-8 py-3">
              Đăng nhập
            </Link>
          </div>
        </div>
      </div>
    );
  }

  // --- LOADING ---
  if (isLoading) {
    return (
      <div className="min-h-screen bg-slate-50 dark:bg-slate-950">
        <div className="max-w-7xl mx-auto px-4 py-10">
          <div className="flex items-center gap-3 mb-8">
            <div className="h-8 w-48 bg-slate-200 dark:bg-slate-800 rounded-lg animate-pulse" />
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
            {[...Array(4)].map((_, i) => (
              <div key={i} className="card animate-pulse">
                <div className="aspect-square bg-slate-200 dark:bg-slate-800 rounded-t-2xl" />
                <div className="p-5 space-y-3">
                  <div className="h-4 w-20 bg-slate-200 dark:bg-slate-800 rounded" />
                  <div className="h-5 bg-slate-200 dark:bg-slate-800 rounded" />
                  <div className="h-6 w-32 bg-slate-200 dark:bg-slate-800 rounded" />
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    );
  }

  const items = wishlist?.items ?? [];

  // --- EMPTY ---
  if (items.length === 0) {
    return (
      <div className="min-h-screen bg-slate-50 dark:bg-slate-950">
        <div className="max-w-7xl mx-auto px-4 py-20">
          <div className="text-center">
            <Heart className="w-16 h-16 text-slate-300 mx-auto mb-4" />
            <h2 className="text-2xl font-bold text-slate-800 dark:text-white mb-2">
              Chưa có sản phẩm yêu thích
            </h2>
            <p className="text-slate-500 mb-6">
              Khám phá các sản phẩm và thêm vào danh sách yêu thích của bạn
            </p>
            <Link href="/products" className="btn-primary px-8 py-3 inline-flex items-center gap-2">
              <Package className="w-4 h-4" />
              Xem sản phẩm
            </Link>
          </div>
        </div>
      </div>
    );
  }

  // --- MAIN ---
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
                Sản phẩm yêu thích
              </h1>
              <p className="text-sm text-slate-500">
                {items.length} sản phẩm
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

        {/* Grid */}
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {items.map((item) => {
            const productUrl = item.productSlug
              ? `/products/${item.productSlug}`
              : `/products/${item.productId}`;
            const alreadyInCart = isInCart(item.productId);

            return (
              <div key={item.id} className="card group relative overflow-hidden">
                {/* Remove button */}
                <button
                  onClick={() => handleRemove(item.id)}
                  disabled={removingIds.has(item.id)}
                  className="absolute top-3 right-3 z-10 w-9 h-9 rounded-lg bg-white/80 dark:bg-slate-800/80 backdrop-blur-sm flex items-center justify-center hover:bg-red-50 dark:hover:bg-red-900/30 hover:text-red-500 transition-all"
                  title="Xóa khỏi yêu thích"
                >
                  {removingIds.has(item.id) ? (
                    <Loader2 className="w-4 h-4 animate-spin" />
                  ) : (
                    <Heart className="w-4 h-4 fill-pink-500 text-pink-500" />
                  )}
                </button>

                {/* Stock badge */}
                {!item.isInStock && (
                  <div className="absolute top-3 left-3 z-10">
                    <span className="px-2.5 py-1 text-xs font-semibold bg-slate-900/80 text-white rounded-lg">
                      Hết hàng
                    </span>
                  </div>
                )}

                {/* Image */}
                <Link href={productUrl}>
                  <div className="aspect-square overflow-hidden bg-gradient-to-br from-slate-100 to-slate-50 dark:from-slate-800 dark:to-slate-900">
                    <img
                      src={item.productImage}
                      alt={item.productName}
                      className="w-full h-full object-cover group-hover:scale-110 transition-transform duration-500"
                    />
                  </div>
                </Link>

                {/* Content */}
                <div className="p-4">
                  <p className="text-xs text-violet-600 dark:text-violet-400 font-semibold mb-1 uppercase tracking-wider">
                    {item.categoryName}
                  </p>
                  <Link href={productUrl}>
                    <h3 className="font-bold text-slate-800 dark:text-white mb-2 line-clamp-2 hover:text-violet-600 dark:hover:text-violet-400 transition-colors leading-snug">
                      {item.productName}
                    </h3>
                  </Link>
                  <p className="text-xs text-slate-500 mb-3">{item.brandName}</p>
                  <div className="flex items-baseline gap-2 mb-4">
                    <span className="text-lg font-bold bg-gradient-to-r from-violet-600 to-purple-600 bg-clip-text text-transparent">
                      {formatPrice(item.productPrice)}
                    </span>
                    {item.productCapitalPrice > item.productPrice && (
                      <span className="text-sm text-slate-400 line-through">
                        {formatPrice(item.productCapitalPrice)}
                      </span>
                    )}
                  </div>

                  {/* Add to cart */}
                  <button
                    onClick={() => handleAddToCart(item.productId)}
                    disabled={
                      !item.isInStock ||
                      addingToCartIds.has(item.productId) ||
                      alreadyInCart
                    }
                    className={`w-full flex items-center justify-center gap-2 py-2.5 text-sm font-medium rounded-xl transition-all ${
                      alreadyInCart
                        ? "bg-emerald-50 dark:bg-emerald-900/20 text-emerald-600 border border-emerald-200 dark:border-emerald-800"
                        : !item.isInStock
                        ? "bg-slate-100 dark:bg-slate-800 text-slate-400 cursor-not-allowed"
                        : "btn-primary"
                    }`}
                  >
                    {addingToCartIds.has(item.productId) ? (
                      <Loader2 className="w-4 h-4 animate-spin" />
                    ) : alreadyInCart ? (
                      <>✓ Đã trong giỏ</>
                    ) : (
                      <>
                        <ShoppingCart className="w-4 h-4" />
                        {item.isInStock ? "Thêm vào giỏ" : "Hết hàng"}
                      </>
                    )}
                  </button>
                </div>
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
}
