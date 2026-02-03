import api, { ApiResponse, PaginatedResponse } from './api';
import { Category } from './dashboard-service';

export interface CreateCategoryDto {
    name: string;
    description: string;
    slug: string;
    status?: string;
}

export interface UpdateCategoryDto {
    name?: string;
    description?: string;
    slug?: string;
    status?: string;
}

export const categoriesService = {
    async getAll(): Promise<Category[]> {
        try {
            const res = await api.get('/Categories');
            // Handle different response formats
            const data = res.data?.data ?? res.data;
            if (Array.isArray(data)) {
                return data;
            }
            if (data?.items && Array.isArray(data.items)) {
                return data.items;
            }
            return [];
        } catch (error) {
            console.error('Error fetching categories:', error);
            return [];
        }
    },

    async getById(id: number): Promise<Category | null> {
        try {
            const res = await api.get<ApiResponse<Category>>(`/Categories/${id}`);
            return res.data.data;
        } catch {
            return null;
        }
    },

    async create(data: CreateCategoryDto): Promise<Category> {
        const res = await api.post<ApiResponse<Category>>('/Categories', data);
        return res.data.data;
    },

    async update(id: number, data: UpdateCategoryDto): Promise<Category> {
        const res = await api.put<ApiResponse<Category>>(`/Categories/${id}`, { id, ...data });
        return res.data.data;
    },

    async delete(id: number): Promise<boolean> {
        await api.delete(`/Categories/${id}`);
        return true;
    },

    async deletePermanent(id: number): Promise<boolean> {
        await api.delete(`/Categories/${id}/permanent`);
        return true;
    },
};
