import api, { ApiResponse } from './api';
import { Brand } from './dashboard-service';

export interface CreateBrandDto {
    name: string;
    description: string;
    slug: string;
    logo?: string;
    status?: string;
}

export interface UpdateBrandDto {
    name?: string;
    description?: string;
    slug?: string;
    logo?: string;
    status?: string;
}

export const brandsService = {
    async getAll(): Promise<Brand[]> {
        try {
            const res = await api.get('/Brands');
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
            console.error('Error fetching brands:', error);
            return [];
        }
    },

    async getById(id: number): Promise<Brand | null> {
        try {
            const res = await api.get<ApiResponse<Brand>>(`/Brands/${id}`);
            return res.data.data;
        } catch {
            return null;
        }
    },

    async create(data: CreateBrandDto): Promise<Brand> {
        const res = await api.post<ApiResponse<Brand>>('/Brands', data);
        return res.data.data;
    },

    async update(id: number, data: UpdateBrandDto): Promise<Brand> {
        const res = await api.put<ApiResponse<Brand>>(`/Brands/${id}`, { id, ...data });
        return res.data.data;
    },

    async delete(id: number): Promise<boolean> {
        await api.delete(`/Brands/${id}`);
        return true;
    },

    async deletePermanent(id: number): Promise<boolean> {
        await api.delete(`/Brands/${id}/permanent`);
        return true;
    },
};
