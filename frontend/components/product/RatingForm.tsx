"use client";

import { useState } from "react";
import { Loader2, Send } from "lucide-react";
import StarRating from "@/components/ui/StarRating";
import { ratingsService } from "@/lib/services/ratings-service";
import { useAuth } from "@/contexts/AuthContext";
import { toast } from "sonner";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";

const ratingSchema = z.object({
  name: z.string().min(2, "Tên tối thiểu 2 ký tự").max(100),
  email: z.string().email("Email không hợp lệ").max(100),
  comment: z.string().min(4, "Nhận xét tối thiểu 4 ký tự").max(1000),
});

type RatingFormData = z.infer<typeof ratingSchema>;

interface RatingFormProps {
  productId: number;
  onSuccess: () => void;
  onCancel?: () => void;
}

export default function RatingForm({
  productId,
  onSuccess,
  onCancel,
}: RatingFormProps) {
  const { user } = useAuth();
  const [star, setStar] = useState<number>(0);
  const [starError, setStarError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<RatingFormData>({
    resolver: zodResolver(ratingSchema),
    defaultValues: {
      name: user?.metadata?.full_name || "",
      email: user?.email || "",
      comment: "",
    },
  });

  const onSubmit = async (data: RatingFormData) => {
    if (star === 0) {
      setStarError("Vui lòng chọn số sao");
      return;
    }
    setStarError(null);
    setIsSubmitting(true);

    try {
      await ratingsService.createRating(productId, {
        star,
        comment: data.comment,
        name: data.name,
        email: data.email,
      });
      toast.success("Đánh giá đã được gửi thành công!");
      reset();
      setStar(0);
      onSuccess();
    } catch {
      toast.error("Không thể gửi đánh giá. Vui lòng thử lại.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <form
      onSubmit={handleSubmit(onSubmit)}
      className="bg-white dark:bg-slate-800 rounded-xl border border-slate-100 dark:border-slate-700 p-5"
    >
      <h3 className="font-bold text-slate-800 dark:text-white mb-4">
        Viết đánh giá
      </h3>

      {/* Star Select */}
      <div className="mb-4">
        <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-2">
          Đánh giá của bạn <span className="text-red-500">*</span>
        </label>
        <StarRating
          value={star}
          size="lg"
          interactive
          onChange={(v) => {
            setStar(v);
            setStarError(null);
          }}
        />
        {starError && (
          <p className="text-xs text-red-500 mt-1">{starError}</p>
        )}
      </div>

      {/* Name + Email */}
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 mb-4">
        <div>
          <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">
            Tên <span className="text-red-500">*</span>
          </label>
          <input
            {...register("name")}
            className="w-full h-10 px-4 text-sm border border-slate-200 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-800 focus:border-violet-400 focus:ring-2 focus:ring-violet-100 dark:focus:ring-violet-900/30 outline-none transition-all"
          />
          {errors.name && (
            <p className="text-xs text-red-500 mt-1">{errors.name.message}</p>
          )}
        </div>
        <div>
          <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">
            Email <span className="text-red-500">*</span>
          </label>
          <input
            {...register("email")}
            className="w-full h-10 px-4 text-sm border border-slate-200 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-800 focus:border-violet-400 focus:ring-2 focus:ring-violet-100 dark:focus:ring-violet-900/30 outline-none transition-all"
          />
          {errors.email && (
            <p className="text-xs text-red-500 mt-1">{errors.email.message}</p>
          )}
        </div>
      </div>

      {/* Comment */}
      <div className="mb-4">
        <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">
          Nhận xét <span className="text-red-500">*</span>
        </label>
        <textarea
          {...register("comment")}
          rows={4}
          placeholder="Chia sẻ trải nghiệm của bạn về sản phẩm..."
          className="w-full px-4 py-3 text-sm border border-slate-200 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-800 focus:border-violet-400 focus:ring-2 focus:ring-violet-100 dark:focus:ring-violet-900/30 outline-none transition-all resize-none"
        />
        {errors.comment && (
          <p className="text-xs text-red-500 mt-1">{errors.comment.message}</p>
        )}
      </div>

      {/* Actions */}
      <div className="flex gap-3">
        {onCancel && (
          <button
            type="button"
            onClick={onCancel}
            className="px-5 py-2.5 text-sm font-medium border border-slate-200 dark:border-slate-600 text-slate-700 dark:text-slate-300 hover:bg-slate-50 dark:hover:bg-slate-700 rounded-xl transition-colors"
          >
            Huỷ
          </button>
        )}
        <button
          type="submit"
          disabled={isSubmitting}
          className="flex items-center gap-2 px-5 py-2.5 text-sm font-semibold bg-violet-600 hover:bg-violet-700 text-white rounded-xl transition-colors disabled:opacity-70"
        >
          {isSubmitting ? (
            <Loader2 className="w-4 h-4 animate-spin" />
          ) : (
            <Send className="w-4 h-4" />
          )}
          Gửi đánh giá
        </button>
      </div>
    </form>
  );
}
