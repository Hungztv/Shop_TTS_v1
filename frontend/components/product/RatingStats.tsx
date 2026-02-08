"use client";

import StarRating from "@/components/ui/StarRating";
import type { RatingStatsDto } from "@/lib/services/ratings-service";

interface RatingStatsProps {
  stats: RatingStatsDto;
}

export default function RatingStats({ stats }: RatingStatsProps) {
  const bars = [
    { label: "5", count: stats.fiveStarCount, percent: stats.fiveStarPercent },
    { label: "4", count: stats.fourStarCount, percent: stats.fourStarPercent },
    { label: "3", count: stats.threeStarCount, percent: stats.threeStarPercent },
    { label: "2", count: stats.twoStarCount, percent: stats.twoStarPercent },
    { label: "1", count: stats.oneStarCount, percent: stats.oneStarPercent },
  ];

  return (
    <div className="flex flex-col sm:flex-row gap-6 sm:gap-8">
      {/* Average Score */}
      <div className="text-center sm:text-left flex-shrink-0">
        <div className="text-5xl font-bold text-slate-800 dark:text-white mb-1">
          {stats.averageScore.toFixed(1)}
        </div>
        <StarRating value={stats.averageScore} size="md" />
        <p className="text-sm text-slate-500 dark:text-slate-400 mt-1">
          {stats.totalRatings.toLocaleString()} đánh giá
        </p>
      </div>

      {/* Distribution Bars */}
      <div className="flex-1 space-y-2">
        {bars.map((bar) => (
          <div key={bar.label} className="flex items-center gap-2">
            <span className="text-sm text-slate-600 dark:text-slate-400 w-4 text-right">
              {bar.label}
            </span>
            <span className="text-amber-400 text-xs">★</span>
            <div className="flex-1 h-2.5 bg-slate-100 dark:bg-slate-700 rounded-full overflow-hidden">
              <div
                className="h-full bg-amber-400 rounded-full transition-all duration-500"
                style={{ width: `${bar.percent}%` }}
              />
            </div>
            <span className="text-xs text-slate-500 w-8 text-right">
              {bar.count}
            </span>
          </div>
        ))}
      </div>
    </div>
  );
}
