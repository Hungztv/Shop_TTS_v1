"use client";

import { useState } from "react";
import { Heart, Loader2 } from "lucide-react";
import { useWishlist } from "@/contexts/WishlistContext";
import { useAuth } from "@/contexts/AuthContext";
import { toast } from "sonner";
import Link from "next/link";

interface WishlistButtonProps {
  productId: number;
  /** "icon" = nút tròn nhỏ (trên ProductCard), "button" = nút full */
  variant?: "icon" | "button";
  className?: string;
}

export default function WishlistButton({
  productId,
  variant = "icon",
  className = "",
}: WishlistButtonProps) {
  const { isInWishlist, addItem, removeByProductId } = useWishlist();
  const { isAuthenticated } = useAuth();
  const [isProcessing, setIsProcessing] = useState(false);

  const wishlisted = isInWishlist(productId);

  const handleToggle = async (e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();

    if (!isAuthenticated) {
      toast.error("Vui lòng đăng nhập để sử dụng wishlist");
      return;
    }

    if (isProcessing) return;
    setIsProcessing(true);

    if (wishlisted) {
      const ok = await removeByProductId(productId);
      if (ok) toast.success("Đã xóa khỏi yêu thích");
      else toast.error("Không thể xóa khỏi yêu thích");
    } else {
      const ok = await addItem(productId);
      if (ok) toast.success("Đã thêm vào yêu thích ❤️");
      else toast.error("Không thể thêm vào yêu thích");
    }

    setIsProcessing(false);
  };

  // --- ICON VARIANT (used on ProductCard) ---
  if (variant === "icon") {
    if (!isAuthenticated) {
      return (
        <Link
          href="/login"
          className={`w-10 h-10 rounded-xl flex items-center justify-center glass hover:bg-white transition-all duration-300 hover:scale-110 ${className}`}
          title="Đăng nhập để thêm yêu thích"
          onClick={(e) => e.stopPropagation()}
        >
          <Heart className="w-5 h-5 text-slate-500" />
        </Link>
      );
    }

    return (
      <button
        onClick={handleToggle}
        disabled={isProcessing}
        className={`w-10 h-10 rounded-xl flex items-center justify-center transition-all duration-300 hover:scale-110 ${
          wishlisted
            ? "bg-gradient-to-br from-pink-500 to-rose-600 text-white shadow-lg shadow-pink-500/30"
            : "glass hover:bg-white"
        } ${className}`}
        title={wishlisted ? "Bỏ yêu thích" : "Thêm yêu thích"}
      >
        {isProcessing ? (
          <Loader2 className="w-5 h-5 animate-spin" />
        ) : (
          <Heart
            className={`w-5 h-5 transition-transform ${
              wishlisted ? "fill-current scale-110" : "text-slate-500"
            }`}
          />
        )}
      </button>
    );
  }

  // --- BUTTON VARIANT ---
  if (!isAuthenticated) {
    return (
      <Link
        href="/login"
        className={`flex items-center justify-center gap-2 px-4 py-2.5 text-sm font-medium border border-slate-200 dark:border-slate-700 rounded-xl hover:bg-slate-50 dark:hover:bg-slate-800 transition-all ${className}`}
      >
        <Heart className="w-4 h-4" />
        Yêu thích
      </Link>
    );
  }

  return (
    <button
      onClick={handleToggle}
      disabled={isProcessing}
      className={`flex items-center justify-center gap-2 px-4 py-2.5 text-sm font-medium rounded-xl transition-all ${
        wishlisted
          ? "bg-pink-50 dark:bg-pink-900/20 text-pink-600 border border-pink-200 dark:border-pink-800"
          : "border border-slate-200 dark:border-slate-700 text-slate-600 dark:text-slate-300 hover:bg-slate-50 dark:hover:bg-slate-800"
      } ${className}`}
    >
      {isProcessing ? (
        <Loader2 className="w-4 h-4 animate-spin" />
      ) : (
        <Heart
          className={`w-4 h-4 ${wishlisted ? "fill-current" : ""}`}
        />
      )}
      {wishlisted ? "Đã yêu thích" : "Yêu thích"}
    </button>
  );
}
