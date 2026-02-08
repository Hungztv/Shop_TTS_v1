import api, { ApiResponse } from "./admin/api";

// ==================== CART TYPES ====================

export interface CartItemDto {
  id: number;
  productId: number;
  productName: string;
  productImage: string;
  productSlug: string;
  price: number;
  capitalPrice: number;
  quantity: number;
  maxQuantity: number;
  subtotal: number;
  createdAt: string;
}

export interface CartSummaryDto {
  items: CartItemDto[];
  totalItems: number;
  totalPrice: number;
  uniqueProducts: number;
}

export interface AddToCartPayload {
  productId: number;
  quantity: number;
}

export interface UpdateCartQuantityPayload {
  quantity: number;
}

// ==================== CART SERVICE ====================

export const cartService = {
  /**
   * Lấy giỏ hàng của user hiện tại
   */
  async getCart(): Promise<CartSummaryDto> {
    const res = await api.get<ApiResponse<CartSummaryDto>>("/Cart");
    return res.data.data;
  },

  /**
   * Lấy số lượng sản phẩm trong giỏ hàng
   */
  async getCartCount(): Promise<number> {
    const res = await api.get<ApiResponse<number>>("/Cart/count");
    return res.data.data;
  },

  /**
   * Thêm sản phẩm vào giỏ hàng
   */
  async addToCart(payload: AddToCartPayload): Promise<CartItemDto> {
    const res = await api.post<ApiResponse<CartItemDto>>("/Cart", payload);
    return res.data.data;
  },

  /**
   * Cập nhật số lượng sản phẩm
   */
  async updateQuantity(
    cartId: number,
    payload: UpdateCartQuantityPayload
  ): Promise<CartItemDto> {
    const res = await api.put<ApiResponse<CartItemDto>>(
      `/Cart/${cartId}`,
      payload
    );
    return res.data.data;
  },

  /**
   * Xoá một sản phẩm khỏi giỏ hàng
   */
  async removeFromCart(cartId: number): Promise<boolean> {
    const res = await api.delete<ApiResponse<boolean>>(`/Cart/${cartId}`);
    return res.data.data;
  },

  /**
   * Xoá toàn bộ giỏ hàng
   */
  async clearCart(): Promise<boolean> {
    const res = await api.delete<ApiResponse<boolean>>("/Cart");
    return res.data.data;
  },
};
