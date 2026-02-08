"use client";

import { useEffect, useState, Suspense } from "react";
import { useSearchParams } from "next/navigation";
import Link from "next/link";
import {
  CheckCircle2,
  ShoppingBag,
  Package,
  Copy,
  Check,
} from "lucide-react";
import { orderService, type OrderDto } from "@/lib/services/order-service";
import { toast } from "sonner";

function OrderSuccessContent() {
  const searchParams = useSearchParams();
  const orderCode = searchParams.get("code");
  const [order, setOrder] = useState<OrderDto | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [copied, setCopied] = useState<boolean>(false);

  const formatPrice = (value: number): string => {
    return new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(value);
  };

  useEffect(() => {
    // We don't have getOrderByCode, so show success with code
    setIsLoading(false);
  }, [orderCode]);

  const handleCopyCode = () => {
    if (orderCode) {
      navigator.clipboard.writeText(orderCode);
      setCopied(true);
      toast.success("Đã sao chép mã đơn hàng");
      setTimeout(() => setCopied(false), 2000);
    }
  };

  if (!orderCode) {
    return (
      <div className="min-h-[60vh] flex flex-col items-center justify-center px-4">
        <Package className="w-16 h-16 text-slate-300 mb-4" />
        <h2 className="text-xl font-bold text-slate-800 dark:text-white mb-2">
          Không tìm thấy đơn hàng
        </h2>
        <Link
          href="/products"
          className="mt-4 px-6 py-3 bg-violet-600 hover:bg-violet-700 text-white font-semibold rounded-xl transition-colors"
        >
          Tiếp tục mua sắm
        </Link>
      </div>
    );
  }

  return (
    <div className="max-w-2xl mx-auto px-4 py-10 sm:py-16">
      {/* Success Icon */}
      <div className="text-center mb-8">
        <div className="w-20 h-20 rounded-full bg-emerald-100 dark:bg-emerald-900/30 flex items-center justify-center mx-auto mb-5">
          <CheckCircle2 className="w-10 h-10 text-emerald-600" />
        </div>
        <h1 className="text-2xl sm:text-3xl font-bold text-slate-800 dark:text-white mb-2">
          Đặt hàng thành công!
        </h1>
        <p className="text-slate-500 dark:text-slate-400">
          Cảm ơn bạn đã mua hàng. Đơn hàng của bạn đang được xử lý.
        </p>
      </div>

      {/* Order Code */}
      <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-100 dark:border-slate-700 p-6 mb-6">
        <p className="text-sm text-slate-500 dark:text-slate-400 mb-2">
          Mã đơn hàng
        </p>
        <div className="flex items-center gap-3">
          <span className="text-2xl font-bold text-violet-600 tracking-wide">
            {orderCode}
          </span>
          <button
            onClick={handleCopyCode}
            className="p-2 rounded-lg hover:bg-slate-100 dark:hover:bg-slate-700 transition-colors"
            title="Sao chép mã"
          >
            {copied ? (
              <Check className="w-4 h-4 text-emerald-500" />
            ) : (
              <Copy className="w-4 h-4 text-slate-400" />
            )}
          </button>
        </div>
        <p className="text-xs text-slate-400 mt-2">
          Lưu lại mã đơn hàng để theo dõi trạng thái giao hàng
        </p>
      </div>

      {/* Info */}
      <div className="bg-violet-50 dark:bg-violet-900/20 rounded-xl p-5 mb-6">
        <h3 className="font-semibold text-violet-800 dark:text-violet-300 text-sm mb-2">
          Bước tiếp theo
        </h3>
        <ul className="text-sm text-violet-700 dark:text-violet-400 space-y-1.5">
          <li>• Chúng tôi sẽ xác nhận đơn hàng qua email</li>
          <li>• Bạn có thể theo dõi đơn hàng trong phần &ldquo;Đơn hàng&rdquo;</li>
          <li>• Dự kiến giao hàng trong 2-5 ngày làm việc</li>
        </ul>
      </div>

      {/* Actions */}
      <div className="flex flex-col sm:flex-row gap-3">
        <Link
          href="/account/orders"
          className="flex-1 flex items-center justify-center gap-2 px-6 py-3.5 bg-violet-600 hover:bg-violet-700 text-white font-semibold rounded-xl transition-colors text-sm"
        >
          <Package className="w-4 h-4" />
          Xem đơn hàng
        </Link>
        <Link
          href="/products"
          className="flex-1 flex items-center justify-center gap-2 px-6 py-3.5 border border-slate-200 dark:border-slate-600 text-slate-700 dark:text-slate-300 hover:bg-slate-50 dark:hover:bg-slate-700 font-medium rounded-xl transition-colors text-sm"
        >
          <ShoppingBag className="w-4 h-4" />
          Tiếp tục mua sắm
        </Link>
      </div>
    </div>
  );
}

export default function OrderSuccessPage() {
  return (
    <Suspense
      fallback={
        <div className="min-h-[60vh] flex items-center justify-center">
          <div className="animate-pulse text-slate-400">Đang tải...</div>
        </div>
      }
    >
      <OrderSuccessContent />
    </Suspense>
  );
}
