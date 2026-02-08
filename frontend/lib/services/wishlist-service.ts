import api from "./admin/api";

// ==================== WISHLIST TYPES ====================
// Wishlist API trả về raw DTO (không có envelope)

export interface WishlistDto {
  id: number;
  productId: number;
  productName: string;
  productSlug: string;
  productImage: string;
  productPrice: number;
  productCapitalPrice: number;
  productQuantity: number;
  isInStock: boolean;
  brandName: string;
  categoryName: string;
  createdAt: string;
}

export interface WishlistSummaryDto {
  items: WishlistDto[];
  totalItems: number;
}

// ==================== WISHLIST SERVICE ====================

export const wishlistService = {
  async getWishlist(): Promise<WishlistSummaryDto> {
    const res = await api.get<WishlistSummaryDto>("/Wishlist");
    return res.data;
  },

  async addToWishlist(productId: number): Promise<WishlistDto> {
    const res = await api.post<WishlistDto>("/Wishlist", { productId });
    return res.data;
  },

  async removeFromWishlist(id: number): Promise<void> {
    await api.delete(`/Wishlist/${id}`);
  },

  async clearWishlist(): Promise<void> {
    await api.delete("/Wishlist");
  },
};
