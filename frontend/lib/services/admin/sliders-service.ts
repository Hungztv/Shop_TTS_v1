import api, { ApiResponse } from './api';
import { Slider } from './dashboard-service';

export interface CreateSliderDto {
    name: string;
    title: string;
    image: string;
    description?: string;
    link?: string;
    displayOrder?: number;
    status: number;
}

export interface UpdateSliderDto {
    name?: string;
    title?: string;
    image?: string;
    description?: string;
    link?: string;
    displayOrder?: number;
    status?: number;
}

export interface ReorderSliderDto {
    sliders: { id: number; newOrder: number }[];
}

export const slidersService = {
    async getAll(): Promise<Slider[]> {
        try {
            const res = await api.get('/Sliders');
            const data = res.data?.data ?? res.data;
            if (Array.isArray(data)) {
                return data;
            }
            if (data?.items && Array.isArray(data.items)) {
                return data.items;
            }
            return [];
        } catch (error) {
            console.error('Error fetching sliders:', error);
            return [];
        }
    },

    async getActive(): Promise<Slider[]> {
        try {
            const res = await api.get('/Sliders/active');
            const data = res.data?.data ?? res.data;
            if (Array.isArray(data)) {
                return data;
            }
            return [];
        } catch (error) {
            console.error('Error fetching active sliders:', error);
            return [];
        }
    },

    async getById(id: number): Promise<Slider | null> {
        try {
            const res = await api.get<ApiResponse<Slider>>(`/Sliders/${id}`);
            return res.data.data;
        } catch {
            return null;
        }
    },

    async create(data: CreateSliderDto): Promise<Slider> {
        const res = await api.post<ApiResponse<Slider>>('/Sliders', data);
        return res.data.data;
    },

    async update(id: number, data: UpdateSliderDto): Promise<Slider> {
        const res = await api.put<ApiResponse<Slider>>(`/Sliders/${id}`, { id, ...data });
        return res.data.data;
    },

    async delete(id: number): Promise<boolean> {
        await api.delete(`/Sliders/${id}`);
        return true;
    },

    async reorder(data: ReorderSliderDto): Promise<boolean> {
        await api.put('/Sliders/reorder', data);
        return true;
    },
};
