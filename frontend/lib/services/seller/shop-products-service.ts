import api, { ApiResponse, PaginatedResponse } from '../admin/api';
import { Product } from '../admin/dashboard-service';
import type { CreateProductDto, UpdateProductDto } from '../admin/products-service';

export const sellerProductsService = {
    /** Lấy danh sách sản phẩm của shop mình */
    async getMyProducts(
        page: number = 1,
        pageSize: number = 10
    ): Promise<PaginatedResponse<Product>> {
        try {
            const res = await api.get('/shops/me/products', {
                params: { page, pageSize },
            });
            const data = res.data?.data ?? res.data;

            if (data?.items && Array.isArray(data.items)) {
                return data;
            }
            if (Array.isArray(data)) {
                return {
                    items: data,
                    totalCount: data.length,
                    page: 1,
                    pageSize: data.length,
                    totalPages: 1,
                };
            }
            return { items: [], totalCount: 0, page: 1, pageSize: 10, totalPages: 0 };
        } catch (error) {
            console.error('Error fetching seller products:', error);
            return { items: [], totalCount: 0, page: 1, pageSize: 10, totalPages: 0 };
        }
    },

    /** Tạo sản phẩm mới cho shop */
    async createProduct(data: CreateProductDto): Promise<Product> {
        const res = await api.post<ApiResponse<Product>>(
            '/shops/me/products',
            data
        );
        return res.data.data;
    },

    /** Cập nhật sản phẩm */
    async updateProduct(id: number, data: UpdateProductDto): Promise<Product> {
        const res = await api.put<ApiResponse<Product>>(
            `/shops/me/products/${id}`,
            { Id: id, ...data }
        );
        return res.data.data;
    },

    /** Xóa sản phẩm */
    async deleteProduct(id: number): Promise<boolean> {
        await api.delete(`/shops/me/products/${id}`);
        return true;
    },
};
