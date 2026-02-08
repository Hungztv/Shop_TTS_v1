import api from "./admin/api";

// ==================== RATING TYPES ====================
// Lưu ý: Ratings API trả về raw DTO (không có envelope { success, data })

export interface RatingDto {
  id: number;
  comment: string;
  name: string;
  email: string;
  star: number;
  starDisplay: string;
  isVerifiedPurchase: boolean;
  isApproved: boolean;
  productId: number;
  productName: string;
  userId: string;
  userName: string;
  createdAt: string;
}

export interface RatingPagedDto {
  items: RatingDto[];
  totalItems: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface RatingStatsDto {
  productId: number;
  averageScore: number;
  totalRatings: number;
  fiveStarCount: number;
  fourStarCount: number;
  threeStarCount: number;
  twoStarCount: number;
  oneStarCount: number;
  fiveStarPercent: number;
  fourStarPercent: number;
  threeStarPercent: number;
  twoStarPercent: number;
  oneStarPercent: number;
}

export interface CreateRatingPayload {
  comment: string;
  name: string;
  email: string;
  star: number;
  productId: number;
}

export interface UpdateRatingPayload {
  comment: string;
  star: number;
}

// ==================== RATINGS SERVICE ====================

export const ratingsService = {
  async getProductRatings(
    productId: number,
    page: number = 1,
    pageSize: number = 10
  ): Promise<RatingPagedDto> {
    const res = await api.get<RatingPagedDto>(
      `/products/${productId}/ratings`,
      { params: { page, pageSize } }
    );
    return res.data;
  },

  async getRatingStats(productId: number): Promise<RatingStatsDto> {
    const res = await api.get<RatingStatsDto>(
      `/products/${productId}/ratings/stats`
    );
    return res.data;
  },

  async createRating(
    productId: number,
    payload: Omit<CreateRatingPayload, "productId">
  ): Promise<RatingDto> {
    const res = await api.post<RatingDto>(
      `/products/${productId}/ratings`,
      { ...payload, productId }
    );
    return res.data;
  },

  async updateRating(
    ratingId: number,
    payload: UpdateRatingPayload
  ): Promise<RatingDto> {
    const res = await api.put<RatingDto>(`/ratings/${ratingId}`, payload);
    return res.data;
  },

  async deleteRating(ratingId: number): Promise<void> {
    await api.delete(`/ratings/${ratingId}`);
  },
};
