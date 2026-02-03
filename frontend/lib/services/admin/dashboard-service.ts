import api, { ApiResponse, PaginatedResponse } from './api';

// ==================== DASHBOARD ====================
export interface DashboardStats {
    totalOrders: number;
    totalRevenue: number;
    totalUsers: number;
    totalProducts: number;
    newMessages: number;
    ordersToday: number;
    revenueToday: number;
}

export interface RecentOrder {
    id: number;
    orderCode: string;
    name: string;
    total: number;
    status: number;
    createdAt: string;
}

export interface TopProduct {
    id: number;
    name: string;
    image: string;
    soldOut: number;
    price: number;
}

export const dashboardService = {
    // For now, we'll aggregate data from multiple endpoints
    async getStats(): Promise<DashboardStats> {
        try {
            const [ordersRes, usersRes, productsRes, messagesRes] = await Promise.all([
                api.get('/Orders?pageSize=1').catch(() => ({ data: { data: { totalCount: 0 } } })),
                api.get('/Users?pageSize=1').catch(() => ({ data: { data: { totalCount: 0 } } })),
                api.get('/Products?pageSize=1').catch(() => ({ data: { data: { totalCount: 0 } } })),
                api.get('/contact-messages/unread-count').catch(() => ({ data: { data: { count: 0 } } })),
            ]);

            return {
                totalOrders: ordersRes.data?.data?.totalCount || 0,
                totalRevenue: 0, // Will be calculated from orders
                totalUsers: usersRes.data?.data?.totalCount || 0,
                totalProducts: productsRes.data?.data?.totalCount || 0,
                newMessages: messagesRes.data?.data?.count || 0,
                ordersToday: 0,
                revenueToday: 0,
            };
        } catch {
            return {
                totalOrders: 0,
                totalRevenue: 0,
                totalUsers: 0,
                totalProducts: 0,
                newMessages: 0,
                ordersToday: 0,
                revenueToday: 0,
            };
        }
    },

    async getRecentOrders(limit: number = 5): Promise<RecentOrder[]> {
        try {
            const res = await api.get(`/Orders?pageSize=${limit}&page=1`);
            return res.data?.data?.items || [];
        } catch {
            return [];
        }
    },

    async getTopProducts(limit: number = 5): Promise<TopProduct[]> {
        try {
            const res = await api.get(`/Products?pageSize=${limit}&page=1&sortBy=soldOut&sortOrder=desc`);
            return res.data?.data?.items || [];
        } catch {
            return [];
        }
    },
};

// ==================== COMMON INTERFACES ====================
export interface Product {
    id: number;
    name: string;
    slug: string;
    description: string;
    price: number;
    capitalPrice: number;
    quantity: number;
    soldOut: number;
    image: string;
    averageScore: number;
    ratingCount: number;
    brandId: number;
    categoryId: number;
    brand?: Brand;
    category?: Category;
    createdAt: string;
    updatedAt?: string;
    isDeleted: boolean;
}

export interface Category {
    id: number;
    name: string;
    description: string;
    slug: string;
    status: string;
    createdAt: string;
    updatedAt?: string;
    isDeleted: boolean;
}

export interface Brand {
    id: number;
    name: string;
    description: string;
    slug: string;
    status: string;
    logo: string;
    createdAt: string;
    updatedAt?: string;
    isDeleted: boolean;
}

export interface Order {
    id: number;
    orderCode: string;
    name: string;
    phoneNumber: string;
    address: string;
    email: string;
    note: string;
    shippingCost: number;
    subtotal: number;
    discountAmount: number;
    total: number;
    couponCode?: string;
    couponId?: number;
    paymentMethod: string;
    paymentStatus: string;
    status: number;
    userId: string;
    orderDetails: OrderDetail[];
    createdAt: string;
    updatedAt?: string;
}

export interface OrderDetail {
    id: number;
    productId: number;
    productName: string;
    productImage: string;
    quantity: number;
    price: number;
}

export interface Coupon {
    id: number;
    name: string;
    code: string;
    description: string;
    dateStart: string;
    dateExpired: string;
    discountValue: number;
    isPercent: boolean;
    quantity: number;
    usedCount: number;
    minimumOrderValue: number;
    status: number;
    createdAt: string;
}

export interface Slider {
    id: number;
    name: string;
    title: string;
    image: string;
    description: string;
    link: string;
    displayOrder: number;
    status: number;
    createdAt: string;
}

export interface ContactMessage {
    id: number;
    name: string;
    email: string;
    phone?: string;
    subject: string;
    message: string;
    isRead: boolean;
    repliedAt?: string;
    replyMessage?: string;
    createdAt: string;
}

export interface AppUser {
    id: string;
    userName: string;
    email: string;
    phoneNumber?: string;
    fullName?: string;
    address?: string;
    dateOfBirth?: string;
    avatar?: string;
    occupation?: string;
    roleId?: string;
    createdAt: string;
    lastLoginAt?: string;
    isDeleted: boolean;
}
