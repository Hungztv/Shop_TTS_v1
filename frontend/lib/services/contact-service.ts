import { apiClient } from '../api-client';

export interface CreateContactMessageDto {
    name: string;
    email: string;
    phone?: string;
    subject: string;
    message: string;
}

export interface ApiResponse<T = void> {
    success: boolean;
    message?: string;
    data?: T;
}

export const contactService = {
    async create(data: CreateContactMessageDto): Promise<ApiResponse> {
        const response = await apiClient.post<ApiResponse>('/contact-messages', data);
        return response.data;
    },
};
