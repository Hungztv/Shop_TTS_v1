"use client";

import { useState, useEffect, useCallback } from "react";
import { MessageSquare, ChevronLeft, ChevronRight, BadgeCheck } from "lucide-react";
import StarRating from "@/components/ui/StarRating";
import RatingStats from "@/components/product/RatingStats";
import RatingForm from "@/components/product/RatingForm";
import {
  ratingsService,
  type RatingDto,
  type RatingStatsDto,
  type RatingPagedDto,
} from "@/lib/services/ratings-service";
import { useAuth } from "@/contexts/AuthContext";

interface ProductRatingsProps {
  productId: number;
}

export default function ProductRatings({ productId }: ProductRatingsProps) {
  const { isAuthenticated } = useAuth();
  const [stats, setStats] = useState<RatingStatsDto | null>(null);
  const [ratings, setRatings] = useState<RatingPagedDto | null>(null);
  const [page, setPage] = useState<number>(1);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [showForm, setShowForm] = useState<boolean>(false);

  const fetchData = useCallback(async () => {
    try {
      setIsLoading(true);
      const [statsData, ratingsData] = await Promise.all([
        ratingsService.getRatingStats(productId),
        ratingsService.getProductRatings(productId, page, 5),
      ]);
      setStats(statsData);
      setRatings(ratingsData);
    } catch (err) {
      console.error("Error fetching ratings:", err);
    } finally {
      setIsLoading(false);
    }
  }, [productId, page]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  const handleRatingSuccess = () => {
    setShowForm(false);
    setPage(1);
    fetchData();
  };

  const formatDate = (dateStr: string): string => {
    return new Date(dateStr).toLocaleDateString("vi-VN", {
      year: "numeric",
      month: "long",
      day: "numeric",
    });
  };

  if (isLoading && !stats) {
    return (
      <div className="animate-pulse space-y-4">
        <div className="h-6 bg-slate-200 dark:bg-slate-700 rounded w-48" />
        <div className="h-32 bg-slate-200 dark:bg-slate-700 rounded-xl" />
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h2 className="text-xl font-bold text-slate-800 dark:text-white flex items-center gap-2">
          <MessageSquare className="w-5 h-5 text-violet-600" />
          Đánh giá sản phẩm
        </h2>
        {isAuthenticated && !showForm && (
          <button
            onClick={() => setShowForm(true)}
            className="px-4 py-2 text-sm font-medium text-violet-600 border border-violet-300 dark:border-violet-700 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-colors"
          >
            Viết đánh giá
          </button>
        )}
      </div>

      {/* Stats */}
      {stats && stats.totalRatings > 0 && (
        <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-100 dark:border-slate-700 p-5">
          <RatingStats stats={stats} />
        </div>
      )}

      {/* Rating Form */}
      {showForm && (
        <RatingForm
          productId={productId}
          onSuccess={handleRatingSuccess}
          onCancel={() => setShowForm(false)}
        />
      )}

      {/* Reviews List */}
      {ratings && ratings.items.length > 0 ? (
        <div className="space-y-3">
          {ratings.items.map((review) => (
            <ReviewItem key={review.id} review={review} />
          ))}

          {/* Pagination */}
          {ratings.totalPages > 1 && (
            <div className="flex items-center justify-center gap-2 pt-4">
              <button
                onClick={() => setPage((p) => Math.max(1, p - 1))}
                disabled={!ratings.hasPreviousPage}
                className="p-2 rounded-lg border border-slate-200 dark:border-slate-700 disabled:opacity-40 hover:bg-slate-50 dark:hover:bg-slate-700 transition-colors"
              >
                <ChevronLeft className="w-4 h-4" />
              </button>
              <span className="text-sm text-slate-600 dark:text-slate-400">
                {page} / {ratings.totalPages}
              </span>
              <button
                onClick={() => setPage((p) => p + 1)}
                disabled={!ratings.hasNextPage}
                className="p-2 rounded-lg border border-slate-200 dark:border-slate-700 disabled:opacity-40 hover:bg-slate-50 dark:hover:bg-slate-700 transition-colors"
              >
                <ChevronRight className="w-4 h-4" />
              </button>
            </div>
          )}
        </div>
      ) : (
        !isLoading && (
          <div className="text-center py-8 text-slate-500 dark:text-slate-400">
            <MessageSquare className="w-10 h-10 mx-auto mb-2 text-slate-300" />
            <p>Chưa có đánh giá nào.</p>
            {isAuthenticated && (
              <button
                onClick={() => setShowForm(true)}
                className="mt-3 text-sm text-violet-600 hover:underline"
              >
                Hãy là người đầu tiên đánh giá!
              </button>
            )}
          </div>
        )
      )}
    </div>
  );
}

// --- Sub-component: Review Item ---

function ReviewItem({ review }: { review: RatingDto }) {
  const formatDate = (dateStr: string): string => {
    return new Date(dateStr).toLocaleDateString("vi-VN", {
      year: "numeric",
      month: "short",
      day: "numeric",
    });
  };

  return (
    <div className="bg-white dark:bg-slate-800 rounded-xl border border-slate-100 dark:border-slate-700 p-4">
      <div className="flex items-start gap-3">
        {/* Avatar */}
        <div className="w-10 h-10 rounded-full bg-gradient-to-br from-violet-500 to-pink-500 flex items-center justify-center text-white font-bold text-sm flex-shrink-0">
          {(review.name || review.userName || "?")[0].toUpperCase()}
        </div>

        <div className="flex-1 min-w-0">
          {/* Header */}
          <div className="flex items-center gap-2 flex-wrap">
            <span className="font-semibold text-sm text-slate-800 dark:text-white">
              {review.name || review.userName}
            </span>
            {review.isVerifiedPurchase && (
              <span className="flex items-center gap-0.5 text-xs text-emerald-600 bg-emerald-50 dark:bg-emerald-900/30 px-2 py-0.5 rounded-full">
                <BadgeCheck className="w-3 h-3" />
                Đã mua
              </span>
            )}
            <span className="text-xs text-slate-400">
              {formatDate(review.createdAt)}
            </span>
          </div>

          {/* Stars */}
          <StarRating value={review.star} size="sm" className="mt-1" />

          {/* Comment */}
          <p className="text-sm text-slate-600 dark:text-slate-400 mt-2 leading-relaxed">
            {review.comment}
          </p>
        </div>
      </div>
    </div>
  );
}
