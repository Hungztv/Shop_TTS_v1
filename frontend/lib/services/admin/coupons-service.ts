import api, { ApiResponse, PaginatedResponse } from './api';
import { Coupon } from './dashboard-service';

export interface CreateCouponDto {
    name: string;
    code: string;
    description: string;
    dateStart: string;
    dateExpired: string;
    discountValue: number;
    isPercent: boolean;
    quantity: number;
    minimumOrderValue: number;
    status: number;
}

export interface UpdateCouponDto {
    name?: string;
    code?: string;
    description?: string;
    dateStart?: string;
    dateExpired?: string;
    discountValue?: number;
    isPercent?: boolean;
    quantity?: number;
    minimumOrderValue?: number;
    status?: number;
}

export const couponsService = {
    async getAll(): Promise<Coupon[]> {
        try {
            const res = await api.get('/Coupons');
            const data = res.data?.data ?? res.data;
            if (Array.isArray(data)) {
                return data;
            }
            if (data?.items && Array.isArray(data.items)) {
                return data.items;
            }
            return [];
        } catch (error) {
            console.error('Error fetching coupons:', error);
            return [];
        }
    },

    async getById(id: number): Promise<Coupon | null> {
        try {
            const res = await api.get<ApiResponse<Coupon>>(`/Coupons/${id}`);
            return res.data.data;
        } catch {
            return null;
        }
    },

    async getByCode(code: string): Promise<Coupon | null> {
        try {
            const res = await api.get<ApiResponse<Coupon>>(`/Coupons/code/${code}`);
            return res.data.data;
        } catch {
            return null;
        }
    },

    async create(data: CreateCouponDto): Promise<Coupon> {
        // Convert date strings to ISO format for backend
        const payload = {
            ...data,
            dateStart: new Date(data.dateStart).toISOString(),
            dateExpired: new Date(data.dateExpired).toISOString(),
        };
        const res = await api.post<ApiResponse<Coupon>>('/Coupons', payload);
        return res.data.data;
    },

    async update(id: number, data: UpdateCouponDto): Promise<Coupon> {
        // Convert date strings to ISO format if provided
        const payload = {
            id,
            ...data,
            ...(data.dateStart && { dateStart: new Date(data.dateStart).toISOString() }),
            ...(data.dateExpired && { dateExpired: new Date(data.dateExpired).toISOString() }),
        };
        const res = await api.put<ApiResponse<Coupon>>(`/Coupons/${id}`, payload);
        return res.data.data;
    },

    async delete(id: number): Promise<boolean> {
        await api.delete(`/Coupons/${id}`);
        return true;
    },
};
