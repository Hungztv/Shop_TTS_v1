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
  cartService,
  type CartItemDto,
  type CartSummaryDto,
} from "@/lib/services/cart-service";

// ==================== TYPES ====================

interface CartContextType {
  /** Dữ liệu giỏ hàng */
  cart: CartSummaryDto | null;
  /** Số lượng sản phẩm (unique) */
  cartCount: number;
  /** Đang tải giỏ hàng */
  isLoading: boolean;
  /** Lỗi nếu có */
  error: string | null;
  /** Tải lại giỏ hàng */
  fetchCart: () => Promise<void>;
  /** Thêm sản phẩm vào giỏ */
  addItem: (productId: number, quantity?: number) => Promise<boolean>;
  /** Cập nhật số lượng */
  updateItem: (cartId: number, quantity: number) => Promise<boolean>;
  /** Xoá 1 sản phẩm */
  removeItem: (cartId: number) => Promise<boolean>;
  /** Xoá toàn bộ giỏ */
  clearAll: () => Promise<boolean>;
  /** Kiểm tra sản phẩm có trong giỏ không */
  isInCart: (productId: number) => boolean;
  /** Lấy cart item theo productId */
  getCartItem: (productId: number) => CartItemDto | undefined;
}

// ==================== CONTEXT ====================

const CartContext = createContext<CartContextType | undefined>(undefined);

// ==================== PROVIDER ====================

export function CartProvider({ children }: { children: ReactNode }) {
  const { isAuthenticated } = useAuth();
  const [cart, setCart] = useState<CartSummaryDto | null>(null);
  const [cartCount, setCartCount] = useState<number>(0);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  // Fetch giỏ hàng từ server
  const fetchCart = useCallback(async () => {
    if (!isAuthenticated) {
      setCart(null);
      setCartCount(0);
      return;
    }

    try {
      setIsLoading(true);
      setError(null);
      const data = await cartService.getCart();
      setCart(data);
      setCartCount(data.uniqueProducts ?? data.items?.length ?? 0);
    } catch (err) {
      console.error("Lỗi tải giỏ hàng:", err);
      setError("Không thể tải giỏ hàng");
    } finally {
      setIsLoading(false);
    }
  }, [isAuthenticated]);

  // Fetch cart count nhanh (badge trên header)
  const fetchCartCount = useCallback(async () => {
    if (!isAuthenticated) {
      setCartCount(0);
      return;
    }

    try {
      const count = await cartService.getCartCount();
      setCartCount(count);
    } catch {
      // Silent fail - không block UI
    }
  }, [isAuthenticated]);

  // Auto-fetch khi đăng nhập
  useEffect(() => {
    if (isAuthenticated) {
      fetchCartCount();
    } else {
      setCart(null);
      setCartCount(0);
    }
  }, [isAuthenticated, fetchCartCount]);

  // Thêm sản phẩm vào giỏ
  const addItem = useCallback(
    async (productId: number, quantity: number = 1): Promise<boolean> => {
      if (!isAuthenticated) return false;

      try {
        await cartService.addToCart({ productId, quantity });
        await fetchCart(); // reload toàn bộ giỏ
        return true;
      } catch (err) {
        console.error("Lỗi thêm vào giỏ:", err);
        setError("Không thể thêm vào giỏ hàng");
        return false;
      }
    },
    [isAuthenticated, fetchCart]
  );

  // Cập nhật số lượng
  const updateItem = useCallback(
    async (cartId: number, quantity: number): Promise<boolean> => {
      if (!isAuthenticated) return false;

      try {
        await cartService.updateQuantity(cartId, { quantity });
        await fetchCart();
        return true;
      } catch (err) {
        console.error("Lỗi cập nhật giỏ:", err);
        setError("Không thể cập nhật giỏ hàng");
        return false;
      }
    },
    [isAuthenticated, fetchCart]
  );

  // Xoá 1 sản phẩm
  const removeItem = useCallback(
    async (cartId: number): Promise<boolean> => {
      if (!isAuthenticated) return false;

      try {
        await cartService.removeFromCart(cartId);
        await fetchCart();
        return true;
      } catch (err) {
        console.error("Lỗi xoá sản phẩm:", err);
        setError("Không thể xoá sản phẩm");
        return false;
      }
    },
    [isAuthenticated, fetchCart]
  );

  // Xoá toàn bộ giỏ
  const clearAll = useCallback(async (): Promise<boolean> => {
    if (!isAuthenticated) return false;

    try {
      await cartService.clearCart();
      setCart(null);
      setCartCount(0);
      return true;
    } catch (err) {
      console.error("Lỗi xoá giỏ:", err);
      setError("Không thể xoá giỏ hàng");
      return false;
    }
  }, [isAuthenticated]);

  // Kiểm tra sản phẩm có trong giỏ không
  const isInCart = useCallback(
    (productId: number): boolean => {
      return cart?.items?.some((item) => item.productId === productId) ?? false;
    },
    [cart]
  );

  // Lấy cart item theo productId
  const getCartItem = useCallback(
    (productId: number): CartItemDto | undefined => {
      return cart?.items?.find((item) => item.productId === productId);
    },
    [cart]
  );

  return (
    <CartContext.Provider
      value={{
        cart,
        cartCount,
        isLoading,
        error,
        fetchCart,
        addItem,
        updateItem,
        removeItem,
        clearAll,
        isInCart,
        getCartItem,
      }}
    >
      {children}
    </CartContext.Provider>
  );
}

// ==================== HOOK ====================

export function useCart(): CartContextType {
  const context = useContext(CartContext);
  if (!context) {
    throw new Error("useCart phải được sử dụng bên trong CartProvider");
  }
  return context;
}
