import api, { ApiResponse, PaginatedResponse } from './api';
import { ContactMessage } from './dashboard-service';

export interface MessagesQuery {
    page?: number;
    pageSize?: number;
    isRead?: boolean;
}

export interface ReplyMessageDto {
    replyMessage: string;
}

export const messagesService = {
    async getAll(params: MessagesQuery = {}): Promise<PaginatedResponse<ContactMessage>> {
        const queryParams = new URLSearchParams();
        if (params.page) queryParams.append('page', params.page.toString());
        if (params.pageSize) queryParams.append('pageSize', params.pageSize.toString());
        if (params.isRead !== undefined) queryParams.append('isRead', params.isRead.toString());

        const res = await api.get<ApiResponse<PaginatedResponse<ContactMessage>>>(`/contact-messages?${queryParams.toString()}`);
        return res.data.data || { items: [], totalCount: 0, page: 1, pageSize: 10, totalPages: 0 };
    },

    async getById(id: number): Promise<ContactMessage | null> {
        try {
            const res = await api.get<ApiResponse<ContactMessage>>(`/contact-messages/${id}`);
            return res.data.data;
        } catch {
            return null;
        }
    },

    async getUnreadCount(): Promise<number> {
        try {
            const res = await api.get<ApiResponse<{ count: number }>>('/contact-messages/unread-count');
            return res.data.data?.count || 0;
        } catch {
            return 0;
        }
    },

    async markAsRead(id: number): Promise<boolean> {
        await api.put(`/contact-messages/${id}/read`);
        return true;
    },

    async reply(id: number, data: ReplyMessageDto): Promise<boolean> {
        await api.post(`/contact-messages/${id}/reply`, data);
        return true;
    },

    async delete(id: number): Promise<boolean> {
        await api.delete(`/contact-messages/${id}`);
        return true;
    },
};
