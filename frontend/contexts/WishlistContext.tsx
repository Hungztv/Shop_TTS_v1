"use client";

import {
  createContext,
  useContext,
  useState,
  useEffect,
  useCallback,
  type ReactNode,
} from "react";
import { useAuth } from "@/contexts/AuthContext";
import {
  wishlistService,
  type WishlistDto,
  type WishlistSummaryDto,
} from "@/lib/services/wishlist-service";
import { trackWishlist } from "@/lib/services/behavior-service";

// ==================== TYPES ====================

interface WishlistContextType {
  wishlist: WishlistSummaryDto | null;
  wishlistCount: number;
  isLoading: boolean;
  fetchWishlist: () => Promise<void>;
  addItem: (productId: number) => Promise<boolean>;
  removeItem: (wishlistId: number) => Promise<boolean>;
  removeByProductId: (productId: number) => Promise<boolean>;
  clearAll: () => Promise<boolean>;
  isInWishlist: (productId: number) => boolean;
  getWishlistItem: (productId: number) => WishlistDto | undefined;
}

// ==================== CONTEXT ====================

const WishlistContext = createContext<WishlistContextType | undefined>(
  undefined
);

// ==================== PROVIDER ====================

export function WishlistProvider({ children }: { children: ReactNode }) {
  const { isAuthenticated } = useAuth();
  const [wishlist, setWishlist] = useState<WishlistSummaryDto | null>(null);
  const [wishlistCount, setWishlistCount] = useState<number>(0);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const fetchWishlist = useCallback(async () => {
    if (!isAuthenticated) {
      setWishlist(null);
      setWishlistCount(0);
      return;
    }

    try {
      setIsLoading(true);
      const data = await wishlistService.getWishlist();
      setWishlist(data);
      setWishlistCount(data.totalItems);
    } catch {
      console.error("Lỗi tải wishlist");
    } finally {
      setIsLoading(false);
    }
  }, [isAuthenticated]);

  useEffect(() => {
    if (isAuthenticated) {
      fetchWishlist();
    } else {
      setWishlist(null);
      setWishlistCount(0);
    }
  }, [isAuthenticated, fetchWishlist]);

  const addItem = useCallback(
    async (productId: number): Promise<boolean> => {
      if (!isAuthenticated) return false;
      try {
        await wishlistService.addToWishlist(productId);
        trackWishlist(productId);
        await fetchWishlist();
        return true;
      } catch {
        return false;
      }
    },
    [isAuthenticated, fetchWishlist]
  );

  const removeItem = useCallback(
    async (wishlistId: number): Promise<boolean> => {
      if (!isAuthenticated) return false;
      try {
        await wishlistService.removeFromWishlist(wishlistId);
        await fetchWishlist();
        return true;
      } catch {
        return false;
      }
    },
    [isAuthenticated, fetchWishlist]
  );

  const removeByProductId = useCallback(
    async (productId: number): Promise<boolean> => {
      const item = wishlist?.items.find((i) => i.productId === productId);
      if (!item) return false;
      return removeItem(item.id);
    },
    [wishlist, removeItem]
  );

  const clearAll = useCallback(async (): Promise<boolean> => {
    if (!isAuthenticated) return false;
    try {
      await wishlistService.clearWishlist();
      setWishlist(null);
      setWishlistCount(0);
      return true;
    } catch {
      return false;
    }
  }, [isAuthenticated]);

  const isInWishlist = useCallback(
    (productId: number): boolean => {
      return (
        wishlist?.items?.some((item) => item.productId === productId) ?? false
      );
    },
    [wishlist]
  );

  const getWishlistItem = useCallback(
    (productId: number): WishlistDto | undefined => {
      return wishlist?.items?.find((item) => item.productId === productId);
    },
    [wishlist]
  );

  return (
    <WishlistContext.Provider
      value={{
        wishlist,
        wishlistCount,
        isLoading,
        fetchWishlist,
        addItem,
        removeItem,
        removeByProductId,
        clearAll,
        isInWishlist,
        getWishlistItem,
      }}
    >
      {children}
    </WishlistContext.Provider>
  );
}

// ==================== HOOK ====================

export function useWishlist(): WishlistContextType {
  const context = useContext(WishlistContext);
  if (!context) {
    throw new Error("useWishlist phải được sử dụng bên trong WishlistProvider");
  }
  return context;
}
