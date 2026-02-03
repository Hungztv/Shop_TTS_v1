import api, { ApiResponse, PaginatedResponse } from './api';
import { AppUser } from './dashboard-service';

export interface UsersQuery {
    page?: number;
    pageSize?: number;
    search?: string;
}

export interface UpdateUserDto {
    fullName?: string;
    phoneNumber?: string;
    address?: string;
    dateOfBirth?: string;
    avatar?: string;
    occupation?: string;
}

export const usersService = {
    async getAll(params: UsersQuery = {}): Promise<PaginatedResponse<AppUser>> {
        const queryParams = new URLSearchParams();
        if (params.page) queryParams.append('page', params.page.toString());
        if (params.pageSize) queryParams.append('pageSize', params.pageSize.toString());
        if (params.search) queryParams.append('search', params.search);

        const res = await api.get<ApiResponse<PaginatedResponse<AppUser>>>(`/Users?${queryParams.toString()}`);
        return res.data.data || { items: [], totalCount: 0, page: 1, pageSize: 10, totalPages: 0 };
    },

    async getById(userId: string): Promise<AppUser | null> {
        try {
            const res = await api.get<ApiResponse<AppUser>>(`/Users/${userId}`);
            return res.data.data;
        } catch {
            return null;
        }
    },

    async getMe(): Promise<AppUser | null> {
        try {
            const res = await api.get<ApiResponse<AppUser>>('/Users/me');
            return res.data.data;
        } catch {
            return null;
        }
    },

    async update(userId: string, data: UpdateUserDto): Promise<AppUser> {
        const res = await api.put<ApiResponse<AppUser>>(`/Users/${userId}`, data);
        return res.data.data;
    },

    async updateMe(data: UpdateUserDto): Promise<AppUser> {
        const res = await api.put<ApiResponse<AppUser>>(`/Users/me`, data);
        return res.data.data;
    },

    async delete(userId: string): Promise<boolean> {
        await api.delete(`/Users/${userId}`);
        return true;
    },
};
