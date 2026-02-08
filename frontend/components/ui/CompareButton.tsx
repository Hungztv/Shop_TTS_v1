"use client";

import { useState } from "react";
import { ArrowLeftRight, Loader2, Check } from "lucide-react";
import { useCompare } from "@/contexts/CompareContext";
import { useAuth } from "@/contexts/AuthContext";
import { toast } from "sonner";
import Link from "next/link";

interface CompareButtonProps {
  productId: number;
  /** "icon" = nút tròn nhỏ (trên ProductCard), "button" = nút full */
  variant?: "icon" | "button";
  className?: string;
}

export default function CompareButton({
  productId,
  variant = "icon",
  className = "",
}: CompareButtonProps) {
  const { isInCompare, addItem, removeByProductId, canAddMore } = useCompare();
  const { isAuthenticated } = useAuth();
  const [isProcessing, setIsProcessing] = useState(false);

  const inCompare = isInCompare(productId);

  const handleToggle = async (e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();

    if (!isAuthenticated) {
      toast.error("Vui lòng đăng nhập để so sánh sản phẩm");
      return;
    }

    if (isProcessing) return;
    setIsProcessing(true);

    if (inCompare) {
      const ok = await removeByProductId(productId);
      if (ok) toast.success("Đã bỏ khỏi so sánh");
      else toast.error("Không thể bỏ khỏi so sánh");
    } else {
      if (!canAddMore()) {
        toast.error("Chỉ so sánh tối đa 5 sản phẩm");
        setIsProcessing(false);
        return;
      }
      const ok = await addItem(productId);
      if (ok) toast.success("Đã thêm vào so sánh");
      else toast.error("Không thể thêm vào so sánh");
    }

    setIsProcessing(false);
  };

  // --- ICON VARIANT ---
  if (variant === "icon") {
    if (!isAuthenticated) {
      return (
        <Link
          href="/login"
          className={`w-12 h-12 rounded-xl flex items-center justify-center bg-white/90 backdrop-blur-sm text-slate-600 hover:bg-white hover:scale-110 transition-all shadow-xl ${className}`}
          title="Đăng nhập để so sánh"
          onClick={(e) => e.stopPropagation()}
        >
          <ArrowLeftRight className="w-5 h-5" />
        </Link>
      );
    }

    return (
      <button
        onClick={handleToggle}
        disabled={isProcessing}
        className={`w-12 h-12 rounded-xl flex items-center justify-center transition-all shadow-xl ${
          inCompare
            ? "bg-violet-600 text-white scale-110"
            : "bg-white/90 backdrop-blur-sm text-slate-600 hover:bg-white hover:scale-110"
        } ${className}`}
        title={inCompare ? "Bỏ so sánh" : "So sánh"}
      >
        {isProcessing ? (
          <Loader2 className="w-5 h-5 animate-spin" />
        ) : inCompare ? (
          <Check className="w-5 h-5" />
        ) : (
          <ArrowLeftRight className="w-5 h-5" />
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
        <ArrowLeftRight className="w-4 h-4" />
        So sánh
      </Link>
    );
  }

  return (
    <button
      onClick={handleToggle}
      disabled={isProcessing || (!inCompare && !canAddMore())}
      className={`flex items-center justify-center gap-2 px-4 py-2.5 text-sm font-medium rounded-xl transition-all ${
        inCompare
          ? "bg-violet-50 dark:bg-violet-900/20 text-violet-600 border border-violet-200 dark:border-violet-800"
          : "border border-slate-200 dark:border-slate-700 text-slate-600 dark:text-slate-300 hover:bg-slate-50 dark:hover:bg-slate-800"
      } ${className}`}
    >
      {isProcessing ? (
        <Loader2 className="w-4 h-4 animate-spin" />
      ) : (
        <ArrowLeftRight className="w-4 h-4" />
      )}
      {inCompare ? "Đang so sánh" : "So sánh"}
    </button>
  );
}
