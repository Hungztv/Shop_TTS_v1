import api, { ApiResponse, PaginatedResponse } from './api';
import { Order } from './dashboard-service';

export interface OrdersQuery {
    page?: number;
    pageSize?: number;
    status?: number;
    search?: string;
}

export interface UpdateOrderStatusDto {
    status: number;
}

export const ordersService = {
    async getAll(params: OrdersQuery = {}): Promise<PaginatedResponse<Order>> {
        const queryParams = new URLSearchParams();
        if (params.page) queryParams.append('page', params.page.toString());
        if (params.pageSize) queryParams.append('pageSize', params.pageSize.toString());
        if (params.status !== undefined) queryParams.append('status', params.status.toString());
        if (params.search) queryParams.append('search', params.search);

        const res = await api.get<ApiResponse<PaginatedResponse<Order>>>(`/Orders?${queryParams.toString()}`);
        return res.data.data || { items: [], totalCount: 0, page: 1, pageSize: 10, totalPages: 0 };
    },

    async getById(id: number): Promise<Order | null> {
        try {
            const res = await api.get<ApiResponse<Order>>(`/Orders/${id}`);
            return res.data.data;
        } catch {
            return null;
        }
    },

    async updateStatus(id: number, status: number): Promise<Order> {
        const res = await api.put<ApiResponse<Order>>(`/Orders/${id}/status`, { orderId: id, newStatus: status });
        return res.data.data;
    },

    async cancel(id: number): Promise<boolean> {
        await api.post(`/Orders/${id}/cancel`, { orderId: id });
        return true;
    },
};

export const orderStatusConfig = {
    0: { label: 'Chờ xử lý', color: 'bg-yellow-100 text-yellow-700 dark:bg-yellow-900/30 dark:text-yellow-400', icon: '⏳' },
    1: { label: 'Đã xác nhận', color: 'bg-blue-100 text-blue-700 dark:bg-blue-900/30 dark:text-blue-400', icon: '✓' },
    2: { label: 'Đang giao', color: 'bg-purple-100 text-purple-700 dark:bg-purple-900/30 dark:text-purple-400', icon: '🚚' },
    3: { label: 'Đã giao', color: 'bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400', icon: '✅' },
    4: { label: 'Đã hủy', color: 'bg-red-100 text-red-700 dark:bg-red-900/30 dark:text-red-400', icon: '✗' },
} as const;
