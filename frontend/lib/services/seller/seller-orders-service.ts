import api, { ApiResponse, PaginatedResponse } from '../admin/api';

export interface SellerOrder {
    orderId: number;
    orderCode: string;
    customerName: string;
    customerPhone: string;
    address: string;
    status: number;
    statusText: string;
    paymentMethod: string;
    paymentStatus: string;
    createdAt: string;
    itemCount: number;
    shopSubtotal: number;
    shopOrderDetails: SellerOrderDetail[];
}

export interface SellerOrderDetail {
    id: number;
    productName: string;
    productImage: string;
    price: number;
    quantity: number;
    total: number;
}

export const sellerOrdersService = {
    async getOrders(params: {
        page?: number;
        pageSize?: number;
        status?: number;
        search?: string;
    } = {}): Promise<PaginatedResponse<SellerOrder>> {
        try {
            const queryParams = new URLSearchParams();
            if (params.page) queryParams.append('pageNumber', params.page.toString());
            if (params.pageSize) queryParams.append('pageSize', params.pageSize.toString());
            if (params.status !== undefined) queryParams.append('status', params.status.toString());
            if (params.search) queryParams.append('search', params.search);

            const res = await api.get<ApiResponse<PaginatedResponse<SellerOrder>>>(`/seller/orders?${queryParams.toString()}`);
            return res.data.data || { items: [], totalCount: 0, page: 1, pageSize: 10, totalPages: 0 };
        } catch (error) {
            console.error('Error fetching seller orders:', error);
            return { items: [], totalCount: 0, page: 1, pageSize: 10, totalPages: 0 };
        }
    },

    async getById(id: number): Promise<SellerOrder | null> {
        try {
            const res = await api.get<ApiResponse<SellerOrder>>(`/seller/orders/${id}`);
            return res.data.data;
        } catch {
            return null;
        }
    },

    async updateStatus(orderId: number, newStatus: number): Promise<boolean> {
        try {
            await api.put(`/seller/orders/${orderId}/status`, { orderId, newStatus });
            return true;
        } catch (error) {
            console.error('Error updating order status:', error);
            throw error;
        }
    }
};

export const sellerOrderStatusConfig = {
    0: { label: 'Chờ xử lý', color: 'bg-yellow-100 text-yellow-700 dark:bg-yellow-900/30 dark:text-yellow-400', icon: '⏳' },
    1: { label: 'Đã xác nhận', color: 'bg-blue-100 text-blue-700 dark:bg-blue-900/30 dark:text-blue-400', icon: '✓' },
    2: { label: 'Đang giao', color: 'bg-purple-100 text-purple-700 dark:bg-purple-900/30 dark:text-purple-400', icon: '🚚' },
    3: { label: 'Đã giao', color: 'bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400', icon: '✅' },
    4: { label: 'Đã hủy', color: 'bg-red-100 text-red-700 dark:bg-red-900/30 dark:text-red-400', icon: '✗' },
} as const;
