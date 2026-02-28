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
  compareService,
  type CompareItemDto,
  type CompareSummaryDto,
} from "@/lib/services/compare-service";
import { trackCompare } from "@/lib/services/behavior-service";

// ==================== TYPES ====================

interface CompareContextType {
  compare: CompareSummaryDto | null;
  compareCount: number;
  maxItems: number;
  isLoading: boolean;
  fetchCompare: () => Promise<void>;
  addItem: (productId: number) => Promise<boolean>;
  removeItem: (compareId: number) => Promise<boolean>;
  removeByProductId: (productId: number) => Promise<boolean>;
  clearAll: () => Promise<boolean>;
  isInCompare: (productId: number) => boolean;
  canAddMore: () => boolean;
  getCompareItem: (productId: number) => CompareItemDto | undefined;
}

// ==================== CONTEXT ====================

const CompareContext = createContext<CompareContextType | undefined>(undefined);

// ==================== PROVIDER ====================

export function CompareProvider({ children }: { children: ReactNode }) {
  const { isAuthenticated } = useAuth();
  const [compare, setCompare] = useState<CompareSummaryDto | null>(null);
  const [compareCount, setCompareCount] = useState(0);
  const [isLoading, setIsLoading] = useState(false);

  const maxItems = compare?.maxItems ?? 5;

  const fetchCompare = useCallback(async () => {
    if (!isAuthenticated) {
      setCompare(null);
      setCompareCount(0);
      return;
    }

    try {
      setIsLoading(true);
      const data = await compareService.getCompareList();
      setCompare(data);
      setCompareCount(data.totalItems);
    } catch {
      console.error("Lỗi tải compare");
    } finally {
      setIsLoading(false);
    }
  }, [isAuthenticated]);

  useEffect(() => {
    if (isAuthenticated) {
      fetchCompare();
    } else {
      setCompare(null);
      setCompareCount(0);
    }
  }, [isAuthenticated, fetchCompare]);

  const addItem = useCallback(
    async (productId: number): Promise<boolean> => {
      if (!isAuthenticated) return false;
      try {
        await compareService.addToCompare(productId);
        trackCompare(productId);
        await fetchCompare();
        return true;
      } catch {
        return false;
      }
    },
    [isAuthenticated, fetchCompare]
  );

  const removeItem = useCallback(
    async (compareId: number): Promise<boolean> => {
      if (!isAuthenticated) return false;
      try {
        await compareService.removeFromCompare(compareId);
        await fetchCompare();
        return true;
      } catch {
        return false;
      }
    },
    [isAuthenticated, fetchCompare]
  );

  const removeByProductId = useCallback(
    async (productId: number): Promise<boolean> => {
      const item = compare?.items.find((i) => i.productId === productId);
      if (!item) return false;
      return removeItem(item.id);
    },
    [compare, removeItem]
  );

  const clearAll = useCallback(async (): Promise<boolean> => {
    if (!isAuthenticated) return false;
    try {
      await compareService.clearCompare();
      setCompare(null);
      setCompareCount(0);
      return true;
    } catch {
      return false;
    }
  }, [isAuthenticated]);

  const isInCompare = useCallback(
    (productId: number): boolean => {
      return compare?.items?.some((i) => i.productId === productId) ?? false;
    },
    [compare]
  );

  const canAddMore = useCallback((): boolean => {
    return (compare?.remainingSlots ?? maxItems) > 0;
  }, [compare, maxItems]);

  const getCompareItem = useCallback(
    (productId: number): CompareItemDto | undefined => {
      return compare?.items?.find((i) => i.productId === productId);
    },
    [compare]
  );

  return (
    <CompareContext.Provider
      value={{
        compare,
        compareCount,
        maxItems,
        isLoading,
        fetchCompare,
        addItem,
        removeItem,
        removeByProductId,
        clearAll,
        isInCompare,
        canAddMore,
        getCompareItem,
      }}
    >
      {children}
    </CompareContext.Provider>
  );
}

// ==================== HOOK ====================

export function useCompare(): CompareContextType {
  const context = useContext(CompareContext);
  if (!context) {
    throw new Error("useCompare phải được sử dụng bên trong CompareProvider");
  }
  return context;
}
