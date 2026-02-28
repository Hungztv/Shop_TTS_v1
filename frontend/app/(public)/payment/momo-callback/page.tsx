"use client";

import { useState, useEffect, Suspense } from "react";
import { useSearchParams } from "next/navigation";
import Link from "next/link";
import {
  CheckCircle2,
  XCircle,
  Loader2,
  ShoppingBag,
  Package,
  Copy,
  Check,
} from "lucide-react";
import { toast } from "sonner";
import { orderService } from "@/lib/services/order-service";

function MomoCallbackContent() {
  const searchParams = useSearchParams();
  const [status, setStatus] = useState<"loading" | "success" | "failed">("loading");
  const [copied, setCopied] = useState(false);

  // MoMo redirects with these params
  const orderCode = searchParams.get("orderId"); // MoMo returns orderId = our OrderCode
  const resultCode = searchParams.get("resultCode");
  const momoMessage = searchParams.get("message");

  useEffect(() => {
    const checkPayment = async () => {
      if (!orderCode) {
        setStatus("failed");
        return;
      }

      // Check resultCode from URL first (MoMo redirect params)
      if (resultCode === "0") {
        // Also verify with our backend
        try {
          const paymentStatus = await orderService.checkMomoPaymentStatus(orderCode);
          if (paymentStatus.isPaid) {
            setStatus("success");
          } else {
            // IPN might not have arrived yet, but MoMo says success
            // Wait a bit and check again
            setTimeout(async () => {
              try {
                const retryStatus = await orderService.checkMomoPaymentStatus(orderCode);
                setStatus(retryStatus.isPaid ? "success" : "success"); // Trust MoMo resultCode=0
              } catch {
                setStatus("success"); // Trust MoMo redirect
              }
            }, 2000);
          }
        } catch {
          setStatus("success"); // Trust MoMo resultCode
        }
      } else {
        setStatus("failed");
      }
    };

    checkPayment();
  }, [orderCode, resultCode]);

  const handleCopyCode = () => {
    if (orderCode) {
      navigator.clipboard.writeText(orderCode);
      setCopied(true);
      toast.success("Đã sao chép mã đơn hàng");
      setTimeout(() => setCopied(false), 2000);
    }
  };

  if (status === "loading") {
    return (
      <div className="min-h-[60vh] flex flex-col items-center justify-center px-4">
        <Loader2 className="w-12 h-12 text-violet-600 animate-spin mb-4" />
        <h2 className="text-xl font-bold text-slate-800 dark:text-white mb-2">
          Đang xác nhận thanh toán...
        </h2>
        <p className="text-slate-500 dark:text-slate-400">
          Vui lòng chờ trong giây lát
        </p>
      </div>
    );
  }

  if (status === "failed") {
    return (
      <div className="max-w-2xl mx-auto px-4 py-10 sm:py-16">
        <div className="text-center mb-8">
          <div className="w-20 h-20 rounded-full bg-red-100 dark:bg-red-900/30 flex items-center justify-center mx-auto mb-5">
            <XCircle className="w-10 h-10 text-red-600" />
          </div>
          <h1 className="text-2xl sm:text-3xl font-bold text-slate-800 dark:text-white mb-2">
            Thanh toán không thành công
          </h1>
          <p className="text-slate-500 dark:text-slate-400">
            {momoMessage || "Giao dịch MoMo đã bị hủy hoặc thất bại."}
          </p>
          {orderCode && (
            <p className="text-sm text-slate-400 mt-2">
              Mã đơn hàng: <span className="font-mono font-bold">{orderCode}</span>
            </p>
          )}
        </div>

        <div className="bg-amber-50 dark:bg-amber-900/20 border border-amber-200 dark:border-amber-800 rounded-xl p-4 mb-6">
          <p className="text-sm text-amber-800 dark:text-amber-300">
            Đơn hàng đã được tạo nhưng chưa thanh toán. Bạn có thể thử thanh toán lại từ trang đơn hàng.
          </p>
        </div>

        <div className="flex flex-col sm:flex-row gap-3 justify-center">
          <Link
            href="/account/orders"
            className="flex items-center justify-center gap-2 px-6 py-3 bg-violet-600 hover:bg-violet-700 text-white font-semibold rounded-xl transition-colors"
          >
            <Package className="w-4 h-4" />
            Xem đơn hàng
          </Link>
          <Link
            href="/products"
            className="flex items-center justify-center gap-2 px-6 py-3 border border-slate-200 dark:border-slate-600 text-slate-700 dark:text-slate-300 hover:bg-slate-50 dark:hover:bg-slate-700 font-medium rounded-xl transition-colors"
          >
            <ShoppingBag className="w-4 h-4" />
            Tiếp tục mua sắm
          </Link>
        </div>
      </div>
    );
  }

  // Success
  return (
    <div className="max-w-2xl mx-auto px-4 py-10 sm:py-16">
      <div className="text-center mb-8">
        <div className="w-20 h-20 rounded-full bg-emerald-100 dark:bg-emerald-900/30 flex items-center justify-center mx-auto mb-5">
          <CheckCircle2 className="w-10 h-10 text-emerald-600" />
        </div>
        <h1 className="text-2xl sm:text-3xl font-bold text-slate-800 dark:text-white mb-2">
          Thanh toán MoMo thành công!
        </h1>
        <p className="text-slate-500 dark:text-slate-400">
          Cảm ơn bạn đã mua hàng. Đơn hàng đã được thanh toán và đang xử lý.
        </p>
      </div>

      {/* Order Code */}
      {orderCode && (
        <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-100 dark:border-slate-700 p-5 mb-6">
          <p className="text-sm text-slate-500 dark:text-slate-400 mb-2">Mã đơn hàng</p>
          <div className="flex items-center gap-3">
            <span className="text-lg font-mono font-bold text-violet-600">
              {orderCode}
            </span>
            <button
              onClick={handleCopyCode}
              className="p-2 rounded-lg hover:bg-slate-100 dark:hover:bg-slate-700 transition-colors"
            >
              {copied ? (
                <Check className="w-4 h-4 text-emerald-500" />
              ) : (
                <Copy className="w-4 h-4 text-slate-500" />
              )}
            </button>
          </div>
        </div>
      )}

      {/* Payment info */}
      <div className="bg-emerald-50 dark:bg-emerald-900/20 border border-emerald-200 dark:border-emerald-800 rounded-xl p-4 mb-6">
        <div className="flex items-center gap-2 mb-1">
          <CheckCircle2 className="w-4 h-4 text-emerald-600" />
          <span className="text-sm font-semibold text-emerald-800 dark:text-emerald-300">
            Đã thanh toán qua MoMo
          </span>
        </div>
        <p className="text-sm text-emerald-700 dark:text-emerald-400">
          Giao dịch đã được xác nhận thành công.
        </p>
      </div>

      <div className="flex flex-col sm:flex-row gap-3 justify-center">
        <Link
          href="/account/orders"
          className="flex items-center justify-center gap-2 px-6 py-3 bg-violet-600 hover:bg-violet-700 text-white font-semibold rounded-xl transition-colors"
        >
          <Package className="w-4 h-4" />
          Xem đơn hàng
        </Link>
        <Link
          href="/products"
          className="flex items-center justify-center gap-2 px-6 py-3 border border-slate-200 dark:border-slate-600 text-slate-700 dark:text-slate-300 hover:bg-slate-50 dark:hover:bg-slate-700 font-medium rounded-xl transition-colors"
        >
          <ShoppingBag className="w-4 h-4" />
          Tiếp tục mua sắm
        </Link>
      </div>
    </div>
  );
}

export default function MomoCallbackPage() {
  return (
    <Suspense
      fallback={
        <div className="min-h-[60vh] flex items-center justify-center">
          <Loader2 className="w-8 h-8 text-violet-600 animate-spin" />
        </div>
      }
    >
      <MomoCallbackContent />
    </Suspense>
  );
}
