"use client";

import { useState } from "react";
import { Tag, Loader2, X, Check } from "lucide-react";
import { orderService, type ValidateCouponResult } from "@/lib/services/order-service";
import { toast } from "sonner";

interface CouponInputProps {
  orderValue: number;
  onApply: (result: ValidateCouponResult, code: string) => void;
  onRemove: () => void;
  appliedCode: string | null;
}

export default function CouponInput({
  orderValue,
  onApply,
  onRemove,
  appliedCode,
}: CouponInputProps) {
  const [code, setCode] = useState<string>("");
  const [isValidating, setIsValidating] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const formatPrice = (value: number): string => {
    return new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(value);
  };

  const handleApply = async () => {
    if (!code.trim()) {
      setError("Vui lòng nhập mã giảm giá");
      return;
    }

    setIsValidating(true);
    setError(null);

    try {
      const result = await orderService.validateCoupon(code.trim(), orderValue);
      if (result.isValid) {
        onApply(result, code.trim());
        toast.success(`Áp dụng mã thành công! Giảm ${formatPrice(result.discountAmount)}`);
      } else {
        setError(result.message || "Mã giảm giá không hợp lệ");
      }
    } catch {
      setError("Mã giảm giá không hợp lệ hoặc đã hết hạn");
    } finally {
      setIsValidating(false);
    }
  };

  const handleRemove = () => {
    setCode("");
    setError(null);
    onRemove();
  };

  // Đã áp dụng mã
  if (appliedCode) {
    return (
      <div className="flex items-center justify-between p-3 bg-emerald-50 dark:bg-emerald-900/20 border border-emerald-200 dark:border-emerald-800 rounded-xl">
        <div className="flex items-center gap-2">
          <Check className="w-4 h-4 text-emerald-600" />
          <span className="text-sm font-medium text-emerald-700 dark:text-emerald-400">
            Mã: {appliedCode}
          </span>
        </div>
        <button
          onClick={handleRemove}
          className="p-1 text-slate-400 hover:text-red-500 transition-colors"
        >
          <X className="w-4 h-4" />
        </button>
      </div>
    );
  }

  return (
    <div className="space-y-2">
      <div className="flex gap-2">
        <div className="relative flex-1">
          <Tag className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400" />
          <input
            type="text"
            value={code}
            onChange={(e) => {
              setCode(e.target.value.toUpperCase());
              setError(null);
            }}
            placeholder="Nhập mã giảm giá..."
            className="w-full h-10 pl-9 pr-3 text-sm border border-slate-200 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-800 focus:border-violet-400 focus:ring-2 focus:ring-violet-100 dark:focus:ring-violet-900/30 outline-none transition-all"
            onKeyDown={(e) => e.key === "Enter" && handleApply()}
          />
        </div>
        <button
          onClick={handleApply}
          disabled={isValidating || !code.trim()}
          className="px-4 h-10 text-sm font-medium text-violet-600 border border-violet-300 dark:border-violet-700 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-colors disabled:opacity-50"
        >
          {isValidating ? (
            <Loader2 className="w-4 h-4 animate-spin" />
          ) : (
            "Áp dụng"
          )}
        </button>
      </div>
      {error && (
        <p className="text-xs text-red-500 pl-1">{error}</p>
      )}
    </div>
  );
}
