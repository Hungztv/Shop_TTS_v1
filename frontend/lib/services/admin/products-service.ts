import api, { ApiResponse, PaginatedResponse } from './api';
import { Product } from './dashboard-service';

export interface ProductsQuery {
    page?: number;
    pageSize?: number;
    search?: string;
    categoryId?: number;
    brandId?: number;
    minPrice?: number;
    maxPrice?: number;
    sortBy?: string;
    sortOrder?: 'asc' | 'desc';
}

export interface CreateProductDto {
    name: string;
    slug: string;
    description: string;
    price: number;
    capitalPrice: number;
    quantity: number;
    image: string;
    categoryId: number;
    brandId: number;
}

export interface UpdateProductDto {
    name?: string;
    slug?: string;
    description?: string;
    price?: number;
    capitalPrice?: number;
    quantity?: number;
    image?: string;
    categoryId?: number;
    brandId?: number;
}

export const productsService = {
    async getAll(params: ProductsQuery = {}): Promise<PaginatedResponse<Product>> {
        try {
            const queryParams = new URLSearchParams();
            if (params.page) queryParams.append('page', params.page.toString());
            if (params.pageSize) queryParams.append('pageSize', params.pageSize.toString());
            if (params.search) queryParams.append('search', params.search);
            if (params.categoryId) queryParams.append('categoryId', params.categoryId.toString());
            if (params.brandId) queryParams.append('brandId', params.brandId.toString());
            if (params.sortBy) queryParams.append('sortBy', params.sortBy);
            if (params.sortOrder) queryParams.append('sortOrder', params.sortOrder);

            const res = await api.get(`/Products?${queryParams.toString()}`);
            // Handle different response formats
            const data = res.data?.data ?? res.data;

            // If data is already paginated
            if (data?.items && Array.isArray(data.items)) {
                return data;
            }
            // If data is an array
            if (Array.isArray(data)) {
                return { items: data, totalCount: data.length, page: 1, pageSize: data.length, totalPages: 1 };
            }
            return { items: [], totalCount: 0, page: 1, pageSize: 10, totalPages: 0 };
        } catch (error) {
            console.error('Error fetching products:', error);
            return { items: [], totalCount: 0, page: 1, pageSize: 10, totalPages: 0 };
        }
    },

    async getById(id: number): Promise<Product | null> {
        try {
            const res = await api.get<ApiResponse<Product>>(`/Products/${id}`);
            return res.data.data;
        } catch {
            return null;
        }
    },

    async create(data: CreateProductDto): Promise<Product> {
        const res = await api.post<ApiResponse<Product>>('/Products', data);
        return res.data.data;
    },

    async update(id: number, data: UpdateProductDto): Promise<Product> {
        const res = await api.put<ApiResponse<Product>>(`/Products/${id}`, { Id: id, ...data });
        return res.data.data;
    },

    async delete(id: number): Promise<boolean> {
        await api.delete(`/Products/${id}`);
        return true;
    },
};
