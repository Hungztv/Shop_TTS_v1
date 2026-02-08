"use client";

import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import {
  ShoppingBag,
  MapPin,
  CreditCard,
  CheckCircle2,
  ArrowLeft,
  ArrowRight,
  Loader2,
  PackageX,
  Truck,
  Banknote,
  Smartphone,
} from "lucide-react";
import { useCart } from "@/contexts/CartContext";
import { useAuth } from "@/contexts/AuthContext";
import {
  orderService,
  type ValidateCouponResult,
} from "@/lib/services/order-service";
import CouponInput from "@/components/checkout/CouponInput";
import { toast } from "sonner";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";

// ==================== ZOD SCHEMA ====================

const shippingSchema = z.object({
  name: z.string().min(2, "Họ tên tối thiểu 2 ký tự").max(100),
  phoneNumber: z
    .string()
    .min(9, "Số điện thoại không hợp lệ")
    .max(20)
    .regex(/^[0-9+\-\s()]+$/, "Số điện thoại không hợp lệ"),
  email: z.string().email("Email không hợp lệ").max(100).or(z.literal("")).optional(),
  address: z.string().min(5, "Địa chỉ tối thiểu 5 ký tự").max(500),
  note: z.string().max(1000).optional(),
});

type ShippingFormData = z.infer<typeof shippingSchema>;

// ==================== STEPS ====================

const STEPS = [
  { id: 1, label: "Giao hàng", icon: MapPin },
  { id: 2, label: "Thanh toán", icon: CreditCard },
  { id: 3, label: "Xác nhận", icon: CheckCircle2 },
] as const;

const PAYMENT_METHODS = [
  {
    id: "COD",
    label: "Thanh toán khi nhận hàng",
    description: "Thanh toán bằng tiền mặt khi nhận hàng",
    icon: Banknote,
  },
  {
    id: "BANK_TRANSFER",
    label: "Chuyển khoản ngân hàng",
    description: "Chuyển khoản qua tài khoản ngân hàng",
    icon: CreditCard,
  },
  {
    id: "MOMO",
    label: "Ví MoMo",
    description: "Thanh toán qua ví điện tử MoMo",
    icon: Smartphone,
  },
] as const;

// ==================== COMPONENT ====================

export default function CheckoutPage() {
  const router = useRouter();
  const { cart, fetchCart, clearAll } = useCart();
  const { isAuthenticated, user } = useAuth();

  const [currentStep, setCurrentStep] = useState<number>(1);
  const [paymentMethod, setPaymentMethod] = useState<string>("COD");
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

  // Coupon
  const [couponCode, setCouponCode] = useState<string | null>(null);
  const [discountAmount, setDiscountAmount] = useState<number>(0);

  // Form
  const {
    register,
    handleSubmit,
    formState: { errors },
    getValues,
    trigger,
  } = useForm<ShippingFormData>({
    resolver: zodResolver(shippingSchema),
    defaultValues: {
      name: "",
      phoneNumber: "",
      email: user?.email || "",
      address: "",
      note: "",
    },
  });

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

  const subtotal = cart?.totalPrice ?? 0;
  const shippingCost = 0; // Free shipping
  const finalTotal = subtotal - discountAmount + shippingCost;

  // Coupon handlers
  const handleCouponApply = (result: ValidateCouponResult, code: string) => {
    setCouponCode(code);
    setDiscountAmount(result.discountAmount);
  };

  const handleCouponRemove = () => {
    setCouponCode(null);
    setDiscountAmount(0);
  };

  // Step navigation
  const goToStep = async (step: number) => {
    if (step === 2 && currentStep === 1) {
      const valid = await trigger();
      if (!valid) return;
    }
    setCurrentStep(step);
  };

  // Submit order
  const handlePlaceOrder = async () => {
    if (!cart || cart.items.length === 0) return;

    const formData = getValues();
    setIsSubmitting(true);

    try {
      const order = await orderService.createOrder({
        name: formData.name,
        phoneNumber: formData.phoneNumber,
        address: formData.address,
        email: formData.email || undefined,
        note: formData.note || undefined,
        paymentMethod,
        couponCode: couponCode || undefined,
        orderDetails: cart.items.map((item) => ({
          productId: item.productId,
          quantity: item.quantity,
        })),
      });

      await clearAll();
      toast.success("Đặt hàng thành công!");
      router.push(`/order-success?code=${order.orderCode}`);
    } catch (err) {
      console.error("Order error:", err);
      toast.error("Không thể đặt hàng. Vui lòng thử lại.");
    } finally {
      setIsSubmitting(false);
    }
  };

  // Guard
  if (!isAuthenticated) {
    return (
      <div className="min-h-[60vh] flex flex-col items-center justify-center px-4">
        <ShoppingBag className="w-16 h-16 text-slate-300 mb-4" />
        <h2 className="text-xl font-bold text-slate-800 dark:text-white mb-2">
          Vui lòng đăng nhập
        </h2>
        <Link
          href="/login?redirect=/checkout"
          className="mt-4 px-6 py-3 bg-violet-600 hover:bg-violet-700 text-white font-semibold rounded-xl transition-colors"
        >
          Đăng nhập
        </Link>
      </div>
    );
  }

  if (!cart || cart.items.length === 0) {
    return (
      <div className="min-h-[60vh] flex flex-col items-center justify-center px-4">
        <PackageX className="w-16 h-16 text-slate-300 mb-4" />
        <h2 className="text-xl font-bold text-slate-800 dark:text-white mb-2">
          Giỏ hàng trống
        </h2>
        <p className="text-slate-500 mb-4">Hãy thêm sản phẩm trước khi thanh toán</p>
        <Link
          href="/products"
          className="px-6 py-3 bg-violet-600 hover:bg-violet-700 text-white font-semibold rounded-xl transition-colors"
        >
          Xem sản phẩm
        </Link>
      </div>
    );
  }

  const formData = getValues();

  return (
    <div className="max-w-7xl mx-auto px-4 py-6 sm:py-8">
      {/* Stepper */}
      <div className="flex items-center justify-center mb-8">
        {STEPS.map((step, idx) => (
          <div key={step.id} className="flex items-center">
            <button
              onClick={() => step.id < currentStep && goToStep(step.id)}
              className={`flex items-center gap-2 px-4 py-2 rounded-xl text-sm font-medium transition-all ${
                currentStep === step.id
                  ? "bg-violet-600 text-white shadow-md"
                  : currentStep > step.id
                  ? "bg-emerald-100 dark:bg-emerald-900/30 text-emerald-700 dark:text-emerald-400 cursor-pointer"
                  : "bg-slate-100 dark:bg-slate-800 text-slate-400"
              }`}
            >
              <step.icon className="w-4 h-4" />
              <span className="hidden sm:inline">{step.label}</span>
            </button>
            {idx < STEPS.length - 1 && (
              <div
                className={`w-8 sm:w-16 h-0.5 mx-1 ${
                  currentStep > step.id
                    ? "bg-emerald-400"
                    : "bg-slate-200 dark:bg-slate-700"
                }`}
              />
            )}
          </div>
        ))}
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 lg:gap-8">
        {/* Main Content */}
        <div className="lg:col-span-2">
          {/* Step 1: Thông tin giao hàng */}
          {currentStep === 1 && (
            <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-100 dark:border-slate-700 p-5 sm:p-6">
              <h2 className="text-lg font-bold text-slate-800 dark:text-white mb-5 flex items-center gap-2">
                <Truck className="w-5 h-5 text-violet-600" />
                Thông tin giao hàng
              </h2>

              <div className="space-y-4">
                {/* Họ tên */}
                <div>
                  <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">
                    Họ và tên <span className="text-red-500">*</span>
                  </label>
                  <input
                    {...register("name")}
                    placeholder="Nguyễn Văn A"
                    className="w-full h-11 px-4 text-sm border border-slate-200 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-800 focus:border-violet-400 focus:ring-2 focus:ring-violet-100 dark:focus:ring-violet-900/30 outline-none transition-all"
                  />
                  {errors.name && (
                    <p className="text-xs text-red-500 mt-1">{errors.name.message}</p>
                  )}
                </div>

                {/* SĐT + Email */}
                <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">
                      Số điện thoại <span className="text-red-500">*</span>
                    </label>
                    <input
                      {...register("phoneNumber")}
                      placeholder="0901234567"
                      className="w-full h-11 px-4 text-sm border border-slate-200 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-800 focus:border-violet-400 focus:ring-2 focus:ring-violet-100 dark:focus:ring-violet-900/30 outline-none transition-all"
                    />
                    {errors.phoneNumber && (
                      <p className="text-xs text-red-500 mt-1">{errors.phoneNumber.message}</p>
                    )}
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">
                      Email
                    </label>
                    <input
                      {...register("email")}
                      placeholder="email@example.com"
                      className="w-full h-11 px-4 text-sm border border-slate-200 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-800 focus:border-violet-400 focus:ring-2 focus:ring-violet-100 dark:focus:ring-violet-900/30 outline-none transition-all"
                    />
                    {errors.email && (
                      <p className="text-xs text-red-500 mt-1">{errors.email.message}</p>
                    )}
                  </div>
                </div>

                {/* Địa chỉ */}
                <div>
                  <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">
                    Địa chỉ giao hàng <span className="text-red-500">*</span>
                  </label>
                  <input
                    {...register("address")}
                    placeholder="Số nhà, đường, phường/xã, quận/huyện, tỉnh/thành phố"
                    className="w-full h-11 px-4 text-sm border border-slate-200 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-800 focus:border-violet-400 focus:ring-2 focus:ring-violet-100 dark:focus:ring-violet-900/30 outline-none transition-all"
                  />
                  {errors.address && (
                    <p className="text-xs text-red-500 mt-1">{errors.address.message}</p>
                  )}
                </div>

                {/* Ghi chú */}
                <div>
                  <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">
                    Ghi chú
                  </label>
                  <textarea
                    {...register("note")}
                    rows={3}
                    placeholder="Ghi chú cho đơn hàng (tùy chọn)..."
                    className="w-full px-4 py-3 text-sm border border-slate-200 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-800 focus:border-violet-400 focus:ring-2 focus:ring-violet-100 dark:focus:ring-violet-900/30 outline-none transition-all resize-none"
                  />
                </div>
              </div>

              <button
                onClick={() => goToStep(2)}
                className="mt-6 w-full sm:w-auto flex items-center justify-center gap-2 px-6 py-3 bg-violet-600 hover:bg-violet-700 text-white font-semibold rounded-xl transition-colors"
              >
                Tiếp tục
                <ArrowRight className="w-4 h-4" />
              </button>
            </div>
          )}

          {/* Step 2: Phương thức thanh toán */}
          {currentStep === 2 && (
            <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-100 dark:border-slate-700 p-5 sm:p-6">
              <h2 className="text-lg font-bold text-slate-800 dark:text-white mb-5 flex items-center gap-2">
                <CreditCard className="w-5 h-5 text-violet-600" />
                Phương thức thanh toán
              </h2>

              <div className="space-y-3">
                {PAYMENT_METHODS.map((method) => (
                  <label
                    key={method.id}
                    className={`flex items-center gap-4 p-4 rounded-xl border-2 cursor-pointer transition-all ${
                      paymentMethod === method.id
                        ? "border-violet-500 bg-violet-50 dark:bg-violet-900/20"
                        : "border-slate-100 dark:border-slate-700 hover:border-slate-200 dark:hover:border-slate-600"
                    }`}
                  >
                    <input
                      type="radio"
                      name="paymentMethod"
                      value={method.id}
                      checked={paymentMethod === method.id}
                      onChange={(e) => setPaymentMethod(e.target.value)}
                      className="w-4 h-4 text-violet-600 accent-violet-600"
                    />
                    <method.icon
                      className={`w-6 h-6 ${
                        paymentMethod === method.id
                          ? "text-violet-600"
                          : "text-slate-400"
                      }`}
                    />
                    <div>
                      <p className="font-medium text-slate-800 dark:text-white text-sm">
                        {method.label}
                      </p>
                      <p className="text-xs text-slate-500 dark:text-slate-400">
                        {method.description}
                      </p>
                    </div>
                  </label>
                ))}
              </div>

              <div className="flex gap-3 mt-6">
                <button
                  onClick={() => goToStep(1)}
                  className="flex items-center gap-2 px-5 py-3 border border-slate-200 dark:border-slate-600 text-slate-700 dark:text-slate-300 hover:bg-slate-50 dark:hover:bg-slate-700 font-medium rounded-xl transition-colors text-sm"
                >
                  <ArrowLeft className="w-4 h-4" />
                  Quay lại
                </button>
                <button
                  onClick={() => goToStep(3)}
                  className="flex items-center gap-2 px-6 py-3 bg-violet-600 hover:bg-violet-700 text-white font-semibold rounded-xl transition-colors text-sm"
                >
                  Tiếp tục
                  <ArrowRight className="w-4 h-4" />
                </button>
              </div>
            </div>
          )}

          {/* Step 3: Xác nhận */}
          {currentStep === 3 && (
            <div className="space-y-4">
              {/* Thông tin giao hàng summary */}
              <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-100 dark:border-slate-700 p-5">
                <div className="flex items-center justify-between mb-3">
                  <h3 className="font-bold text-slate-800 dark:text-white flex items-center gap-2 text-sm">
                    <MapPin className="w-4 h-4 text-violet-600" />
                    Thông tin giao hàng
                  </h3>
                  <button
                    onClick={() => goToStep(1)}
                    className="text-xs text-violet-600 hover:underline"
                  >
                    Chỉnh sửa
                  </button>
                </div>
                <div className="text-sm text-slate-600 dark:text-slate-400 space-y-1">
                  <p>
                    <span className="font-medium text-slate-800 dark:text-white">{formData.name}</span>
                    {" · "}{formData.phoneNumber}
                  </p>
                  <p>{formData.address}</p>
                  {formData.email && <p>{formData.email}</p>}
                  {formData.note && (
                    <p className="italic text-slate-500">Ghi chú: {formData.note}</p>
                  )}
                </div>
              </div>

              {/* Phương thức thanh toán summary */}
              <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-100 dark:border-slate-700 p-5">
                <div className="flex items-center justify-between mb-3">
                  <h3 className="font-bold text-slate-800 dark:text-white flex items-center gap-2 text-sm">
                    <CreditCard className="w-4 h-4 text-violet-600" />
                    Phương thức thanh toán
                  </h3>
                  <button
                    onClick={() => goToStep(2)}
                    className="text-xs text-violet-600 hover:underline"
                  >
                    Chỉnh sửa
                  </button>
                </div>
                <p className="text-sm text-slate-700 dark:text-slate-300">
                  {PAYMENT_METHODS.find((m) => m.id === paymentMethod)?.label}
                </p>
              </div>

              {/* Sản phẩm */}
              <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-100 dark:border-slate-700 p-5">
                <h3 className="font-bold text-slate-800 dark:text-white flex items-center gap-2 text-sm mb-3">
                  <ShoppingBag className="w-4 h-4 text-violet-600" />
                  Sản phẩm ({cart.totalItems})
                </h3>
                <div className="space-y-3">
                  {cart.items.map((item) => (
                    <div key={item.id} className="flex gap-3 items-center">
                      <div className="w-14 h-14 rounded-lg overflow-hidden bg-slate-100 dark:bg-slate-700 flex-shrink-0">
                        <img
                          src={item.productImage}
                          alt={item.productName}
                          className="w-full h-full object-cover"
                        />
                      </div>
                      <div className="flex-1 min-w-0">
                        <p className="text-sm font-medium text-slate-800 dark:text-white line-clamp-1">
                          {item.productName}
                        </p>
                        <p className="text-xs text-slate-500">
                          {formatPrice(item.price)} × {item.quantity}
                        </p>
                      </div>
                      <span className="text-sm font-semibold text-slate-800 dark:text-white">
                        {formatPrice(item.subtotal)}
                      </span>
                    </div>
                  ))}
                </div>
              </div>

              {/* Actions */}
              <div className="flex gap-3">
                <button
                  onClick={() => goToStep(2)}
                  className="flex items-center gap-2 px-5 py-3 border border-slate-200 dark:border-slate-600 text-slate-700 dark:text-slate-300 hover:bg-slate-50 dark:hover:bg-slate-700 font-medium rounded-xl transition-colors text-sm"
                >
                  <ArrowLeft className="w-4 h-4" />
                  Quay lại
                </button>
                <button
                  onClick={handlePlaceOrder}
                  disabled={isSubmitting}
                  className="flex-1 flex items-center justify-center gap-2 px-6 py-3.5 bg-violet-600 hover:bg-violet-700 text-white font-semibold rounded-xl transition-colors disabled:opacity-70 text-sm"
                >
                  {isSubmitting ? (
                    <>
                      <Loader2 className="w-4 h-4 animate-spin" />
                      Đang xử lý...
                    </>
                  ) : (
                    <>
                      <CheckCircle2 className="w-4 h-4" />
                      Đặt hàng
                    </>
                  )}
                </button>
              </div>
            </div>
          )}
        </div>

        {/* Sidebar: Tóm tắt đơn hàng */}
        <div className="lg:col-span-1">
          <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-100 dark:border-slate-700 p-5 sticky top-24">
            <h2 className="text-lg font-bold text-slate-800 dark:text-white mb-4">
              Tóm tắt đơn hàng
            </h2>

            {/* Items mini */}
            <div className="space-y-2 mb-4 max-h-48 overflow-y-auto">
              {cart.items.map((item) => (
                <div key={item.id} className="flex justify-between items-center text-sm">
                  <span className="text-slate-600 dark:text-slate-400 truncate flex-1 mr-2">
                    {item.productName} ×{item.quantity}
                  </span>
                  <span className="text-slate-800 dark:text-white font-medium whitespace-nowrap">
                    {formatPrice(item.subtotal)}
                  </span>
                </div>
              ))}
            </div>

            {/* Coupon */}
            <div className="border-t border-slate-100 dark:border-slate-700 pt-4 mb-4">
              <CouponInput
                orderValue={subtotal}
                onApply={handleCouponApply}
                onRemove={handleCouponRemove}
                appliedCode={couponCode}
              />
            </div>

            {/* Totals */}
            <div className="space-y-2 text-sm border-t border-slate-100 dark:border-slate-700 pt-4">
              <div className="flex justify-between text-slate-600 dark:text-slate-400">
                <span>Tạm tính</span>
                <span>{formatPrice(subtotal)}</span>
              </div>
              <div className="flex justify-between text-slate-600 dark:text-slate-400">
                <span>Phí vận chuyển</span>
                <span className="text-emerald-500 font-medium">Miễn phí</span>
              </div>
              {discountAmount > 0 && (
                <div className="flex justify-between text-emerald-600">
                  <span>Giảm giá</span>
                  <span>-{formatPrice(discountAmount)}</span>
                </div>
              )}
              <div className="border-t border-slate-100 dark:border-slate-700 pt-3">
                <div className="flex justify-between items-center">
                  <span className="font-bold text-slate-800 dark:text-white">Tổng cộng</span>
                  <span className="text-xl font-bold text-violet-600">
                    {formatPrice(finalTotal)}
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
