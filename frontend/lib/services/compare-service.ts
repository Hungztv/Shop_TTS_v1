import api from "./admin/api";

// ==================== COMPARE TYPES ====================
// Compare API trả về raw DTO (không có envelope)

export interface CompareItemDto {
  id: number;
  productId: number;

  // Product Details
  productName: string;
  productSlug: string;
  productDescription: string;
  productImage: string;
  productPrice: number;
  productCapitalPrice: number;
  productQuantity: number;
  productSoldOut: number;
  isInStock: boolean;

  // Rating info
  averageScore: number;
  ratingCount: number;

  // Brand Details
  brandId: number;
  brandName: string;
  brandLogo: string;

  // Category Details
  categoryId: number;
  categoryName: string;

  createdAt: string;
}

export interface CompareSummaryDto {
  items: CompareItemDto[];
  totalItems: number;
  maxItems: number;
  remainingSlots: number;
}

// ==================== COMPARE SERVICE ====================

export const compareService = {
  async getCompareList(): Promise<CompareSummaryDto> {
    const res = await api.get<CompareSummaryDto>("/Compare");
    return res.data;
  },

  async addToCompare(productId: number): Promise<CompareItemDto> {
    const res = await api.post<CompareItemDto>("/Compare", { productId });
    return res.data;
  },

  async removeFromCompare(id: number): Promise<void> {
    await api.delete(`/Compare/${id}`);
  },

  async clearCompare(): Promise<void> {
    await api.delete("/Compare");
  },
};
