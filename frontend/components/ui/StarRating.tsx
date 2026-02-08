"use client";

import { Star } from "lucide-react";
import { useState } from "react";

interface StarRatingProps {
  value: number;
  maxStars?: number;
  size?: "sm" | "md" | "lg";
  interactive?: boolean;
  onChange?: (value: number) => void;
  className?: string;
}

const SIZES = {
  sm: "w-4 h-4",
  md: "w-5 h-5",
  lg: "w-6 h-6",
} as const;

export default function StarRating({
  value,
  maxStars = 5,
  size = "md",
  interactive = false,
  onChange,
  className = "",
}: StarRatingProps) {
  const [hoverValue, setHoverValue] = useState<number>(0);
  const displayValue = interactive && hoverValue > 0 ? hoverValue : value;

  return (
    <div className={`flex items-center gap-0.5 ${className}`}>
      {Array.from({ length: maxStars }, (_, i) => {
        const starIndex = i + 1;
        const isFilled = starIndex <= Math.floor(displayValue);
        const isHalf =
          !isFilled &&
          starIndex === Math.ceil(displayValue) &&
          displayValue % 1 >= 0.5;

        return (
          <button
            key={i}
            type="button"
            disabled={!interactive}
            onClick={() => interactive && onChange?.(starIndex)}
            onMouseEnter={() => interactive && setHoverValue(starIndex)}
            onMouseLeave={() => interactive && setHoverValue(0)}
            className={`${
              interactive
                ? "cursor-pointer hover:scale-125 transition-transform"
                : "cursor-default"
            }`}
          >
            <Star
              className={`${SIZES[size]} transition-colors ${
                isFilled
                  ? "text-amber-400 fill-amber-400"
                  : isHalf
                  ? "text-amber-400 fill-amber-400/50"
                  : "text-slate-200 dark:text-slate-700"
              }`}
            />
          </button>
        );
      })}
    </div>
  );
}
