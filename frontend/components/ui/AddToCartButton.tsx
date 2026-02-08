"use client";

import { useState } from "react";
import { ShoppingCart, Check, Loader2, LogIn } from "lucide-react";
import { useCart } from "@/contexts/CartContext";
import { useAuth } from "@/contexts/AuthContext";
import { toast } from "sonner";
import Link from "next/link";

interface AddToCartButtonProps {
  productId: number;
  quantity?: number;
  /** "icon" = nút tròn nhỏ, "button" = nút full-width */
  variant?: "icon" | "button";
  className?: string;
}

export default function AddToCartButton({
  productId,
  quantity = 1,
  variant = "button",
  className = "",
}: AddToCartButtonProps) {
  const { addItem, isInCart } = useCart();
  const { isAuthenticated } = useAuth();
  const [isAdding, setIsAdding] = useState<boolean>(false);
  const [justAdded, setJustAdded] = useState<boolean>(false);

  const alreadyInCart = isInCart(productId);

  const handleAddToCart = async (e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();

    if (!isAuthenticated) {
      toast.error("Vui lòng đăng nhập để thêm vào giỏ hàng");
      return;
    }

    if (isAdding) return;

    setIsAdding(true);
    const success = await addItem(productId, quantity);
    setIsAdding(false);

    if (success) {
      setJustAdded(true);
      toast.success("Đã thêm vào giỏ hàng!");
      setTimeout(() => setJustAdded(false), 2000);
    } else {
      toast.error("Không thể thêm vào giỏ hàng");
    }
  };

  // --- ICON VARIANT ---
  if (variant === "icon") {
    if (!isAuthenticated) {
      return (
        <Link
          href="/login"
          className={`w-12 h-12 rounded-xl flex items-center justify-center bg-gradient-to-br from-violet-600 to-purple-700 text-white hover:scale-110 transition-all shadow-xl ${className}`}
          title="Đăng nhập để thêm vào giỏ"
        >
          <LogIn className="w-5 h-5" />
        </Link>
      );
    }

    return (
      <button
        onClick={handleAddToCart}
        disabled={isAdding}
        className={`w-12 h-12 rounded-xl flex items-center justify-center transition-all shadow-xl ${
          justAdded || alreadyInCart
            ? "bg-emerald-500 text-white scale-110"
            : "bg-gradient-to-br from-violet-600 to-purple-700 text-white hover:scale-110"
        } ${className}`}
        title={alreadyInCart ? "Đã có trong giỏ" : "Thêm vào giỏ"}
      >
        {isAdding ? (
          <Loader2 className="w-5 h-5 animate-spin" />
        ) : justAdded || alreadyInCart ? (
          <Check className="w-5 h-5" />
        ) : (
          <ShoppingCart className="w-5 h-5" />
        )}
      </button>
    );
  }

  // --- BUTTON VARIANT ---
  if (!isAuthenticated) {
    return (
      <Link
        href="/login"
        className={`w-full flex items-center justify-center gap-2 btn-primary py-3 text-sm ${className}`}
      >
        <LogIn className="w-4 h-4" />
        Đăng nhập để mua
      </Link>
    );
  }

  return (
    <button
      onClick={handleAddToCart}
      disabled={isAdding}
      className={`w-full flex items-center justify-center gap-2 py-3 text-sm font-semibold rounded-xl transition-all ${
        justAdded || alreadyInCart
          ? "bg-emerald-500 text-white"
          : "bg-gradient-to-r from-violet-600 to-purple-600 text-white hover:from-violet-700 hover:to-purple-700"
      } disabled:opacity-70 ${className}`}
    >
      {isAdding ? (
        <>
          <Loader2 className="w-4 h-4 animate-spin" />
          Đang thêm...
        </>
      ) : justAdded || alreadyInCart ? (
        <>
          <Check className="w-4 h-4" />
          {alreadyInCart ? "Đã có trong giỏ" : "Đã thêm!"}
        </>
      ) : (
        <>
          <ShoppingCart className="w-4 h-4" />
          Thêm vào giỏ
        </>
      )}
    </button>
  );
}
