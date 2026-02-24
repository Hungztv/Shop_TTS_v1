import api, { ApiResponse } from '../admin/api';
import type { ShopDto, UpdateShopRequest } from '@/types/shop';

export const sellerShopService = {
  /** Lấy shop của seller hiện tại */
  async getMyShop(): Promise<ShopDto | null> {
    try {
      const res = await api.get<ApiResponse<ShopDto>>('/Shops/me');
      return res.data.data;
    } catch (err: unknown) {
      const error = err as { response?: { status?: number } };
      if (error.response?.status === 404) return null;
      throw err;
    }
  },

  /** Cập nhật thông tin shop */
  async updateShop(
    id: number,
    data: Omit<UpdateShopRequest, 'id'>
  ): Promise<ShopDto> {
    const res = await api.patch<ApiResponse<ShopDto>>(`/Shops/${id}`, {
      id,
      ...data,
    });
    return res.data.data;
  },
};
