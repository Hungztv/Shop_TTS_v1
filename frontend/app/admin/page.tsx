'use client';

import { useEffect, useState } from 'react';
import {
    ShoppingCart,
    DollarSign,
    Users,
    Package,
    MessageSquare,
    TrendingUp,
    Clock,
    ArrowRight,
} from 'lucide-react';
import Link from 'next/link';
import AdminHeader from '@/components/admin/AdminHeader';
import StatsCard from '@/components/admin/StatsCard';
import { dashboardService, DashboardStats, RecentOrder, TopProduct } from '@/lib/services/admin/dashboard-service';

const orderStatusLabels: Record<number, { label: string; color: string }> = {
    0: { label: 'Chờ xử lý', color: 'bg-yellow-100 text-yellow-700' },
    1: { label: 'Đã xác nhận', color: 'bg-blue-100 text-blue-700' },
    2: { label: 'Đang giao', color: 'bg-purple-100 text-purple-700' },
    3: { label: 'Đã giao', color: 'bg-green-100 text-green-700' },
    4: { label: 'Đã hủy', color: 'bg-red-100 text-red-700' },
};

export default function AdminDashboard() {
    const [stats, setStats] = useState<DashboardStats | null>(null);
    const [recentOrders, setRecentOrders] = useState<RecentOrder[]>([]);
    const [topProducts, setTopProducts] = useState<TopProduct[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadDashboardData();
    }, []);

    const loadDashboardData = async () => {
        setLoading(true);
        try {
            const [statsData, ordersData, productsData] = await Promise.all([
                dashboardService.getStats(),
                dashboardService.getRecentOrders(5),
                dashboardService.getTopProducts(5),
            ]);
            setStats(statsData);
            setRecentOrders(ordersData);
            setTopProducts(productsData);
        } catch (error) {
            console.error('Error loading dashboard:', error);
        } finally {
            setLoading(false);
        }
    };

    const formatCurrency = (value: number) => {
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND',
        }).format(value);
    };

    const formatDate = (dateStr: string) => {
        return new Date(dateStr).toLocaleDateString('vi-VN', {
            day: '2-digit',
            month: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
        });
    };

    return (
        <div className="min-h-screen">
            <AdminHeader title="Dashboard" subtitle="Tổng quan hệ thống" />

            <div className="p-6 space-y-6">
                {/* Stats Cards */}
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                    <StatsCard
                        title="Tổng đơn hàng"
                        value={stats?.totalOrders || 0}
                        change="+12%"
                        changeType="increase"
                        icon={ShoppingCart}
                        iconColor="bg-blue-100 text-blue-600 dark:bg-blue-900/30 dark:text-blue-400"
                        loading={loading}
                    />
                    <StatsCard
                        title="Doanh thu"
                        value={formatCurrency(stats?.totalRevenue || 0)}
                        change="+8%"
                        changeType="increase"
                        icon={DollarSign}
                        iconColor="bg-green-100 text-green-600 dark:bg-green-900/30 dark:text-green-400"
                        loading={loading}
                    />
                    <StatsCard
                        title="Người dùng"
                        value={stats?.totalUsers || 0}
                        change="+5%"
                        changeType="increase"
                        icon={Users}
                        iconColor="bg-purple-100 text-purple-600 dark:bg-purple-900/30 dark:text-purple-400"
                        loading={loading}
                    />
                    <StatsCard
                        title="Tin nhắn mới"
                        value={stats?.newMessages || 0}
                        icon={MessageSquare}
                        iconColor="bg-orange-100 text-orange-600 dark:bg-orange-900/30 dark:text-orange-400"
                        loading={loading}
                    />
                </div>

                <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
                    {/* Recent Orders */}
                    <div className="bg-white dark:bg-gray-800 rounded-2xl shadow-sm border border-gray-100 dark:border-gray-700 overflow-hidden">
                        <div className="flex items-center justify-between p-6 border-b border-gray-100 dark:border-gray-700">
                            <div className="flex items-center gap-3">
                                <div className="p-2 rounded-xl bg-blue-100 text-blue-600 dark:bg-blue-900/30 dark:text-blue-400">
                                    <Clock className="w-5 h-5" />
                                </div>
                                <h3 className="font-semibold text-gray-900 dark:text-white">Đơn hàng gần đây</h3>
                            </div>
                            <Link
                                href="/admin/orders"
                                className="text-sm text-violet-600 hover:text-violet-700 flex items-center gap-1"
                            >
                                Xem tất cả <ArrowRight className="w-4 h-4" />
                            </Link>
                        </div>
                        <div className="divide-y divide-gray-100 dark:divide-gray-700">
                            {loading ? (
                                Array.from({ length: 5 }).map((_, i) => (
                                    <div key={i} className="p-4 animate-pulse">
                                        <div className="flex justify-between">
                                            <div className="h-4 w-24 bg-gray-200 dark:bg-gray-700 rounded" />
                                            <div className="h-4 w-20 bg-gray-200 dark:bg-gray-700 rounded" />
                                        </div>
                                    </div>
                                ))
                            ) : recentOrders.length === 0 ? (
                                <div className="p-8 text-center text-gray-500">
                                    Chưa có đơn hàng nào
                                </div>
                            ) : (
                                recentOrders.map((order) => (
                                    <div key={order.id} className="p-4 hover:bg-gray-50 dark:hover:bg-gray-700/50 transition-colors">
                                        <div className="flex items-center justify-between">
                                            <div>
                                                <p className="font-medium text-gray-900 dark:text-white">
                                                    #{order.orderCode}
                                                </p>
                                                <p className="text-sm text-gray-500">{order.name}</p>
                                            </div>
                                            <div className="text-right">
                                                <p className="font-semibold text-gray-900 dark:text-white">
                                                    {formatCurrency(order.total)}
                                                </p>
                                                <span
                                                    className={`inline-block px-2 py-0.5 text-xs font-medium rounded-full ${orderStatusLabels[order.status]?.color || 'bg-gray-100 text-gray-700'
                                                        }`}
                                                >
                                                    {orderStatusLabels[order.status]?.label || 'Unknown'}
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                ))
                            )}
                        </div>
                    </div>

                    {/* Top Products */}
                    <div className="bg-white dark:bg-gray-800 rounded-2xl shadow-sm border border-gray-100 dark:border-gray-700 overflow-hidden">
                        <div className="flex items-center justify-between p-6 border-b border-gray-100 dark:border-gray-700">
                            <div className="flex items-center gap-3">
                                <div className="p-2 rounded-xl bg-green-100 text-green-600 dark:bg-green-900/30 dark:text-green-400">
                                    <TrendingUp className="w-5 h-5" />
                                </div>
                                <h3 className="font-semibold text-gray-900 dark:text-white">Sản phẩm bán chạy</h3>
                            </div>
                            <Link
                                href="/admin/products"
                                className="text-sm text-violet-600 hover:text-violet-700 flex items-center gap-1"
                            >
                                Xem tất cả <ArrowRight className="w-4 h-4" />
                            </Link>
                        </div>
                        <div className="divide-y divide-gray-100 dark:divide-gray-700">
                            {loading ? (
                                Array.from({ length: 5 }).map((_, i) => (
                                    <div key={i} className="p-4 animate-pulse flex items-center gap-4">
                                        <div className="w-12 h-12 bg-gray-200 dark:bg-gray-700 rounded-xl" />
                                        <div className="flex-1">
                                            <div className="h-4 w-32 bg-gray-200 dark:bg-gray-700 rounded" />
                                        </div>
                                    </div>
                                ))
                            ) : topProducts.length === 0 ? (
                                <div className="p-8 text-center text-gray-500">
                                    Chưa có sản phẩm nào
                                </div>
                            ) : (
                                topProducts.map((product, index) => (
                                    <div
                                        key={product.id}
                                        className="p-4 hover:bg-gray-50 dark:hover:bg-gray-700/50 transition-colors flex items-center gap-4"
                                    >
                                        <span className="w-6 text-center font-bold text-gray-400">
                                            #{index + 1}
                                        </span>
                                        <img
                                            src={product.image || '/placeholder.png'}
                                            alt={product.name}
                                            className="w-12 h-12 rounded-xl object-cover bg-gray-100"
                                        />
                                        <div className="flex-1 min-w-0">
                                            <p className="font-medium text-gray-900 dark:text-white truncate">
                                                {product.name}
                                            </p>
                                            <p className="text-sm text-gray-500">
                                                Đã bán: {product.soldOut}
                                            </p>
                                        </div>
                                        <p className="font-semibold text-violet-600">
                                            {formatCurrency(product.price)}
                                        </p>
                                    </div>
                                ))
                            )}
                        </div>
                    </div>
                </div>

                {/* Quick Actions */}
                <div className="bg-white dark:bg-gray-800 rounded-2xl shadow-sm border border-gray-100 dark:border-gray-700 p-6">
                    <h3 className="font-semibold text-gray-900 dark:text-white mb-4">Thao tác nhanh</h3>
                    <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
                        <Link
                            href="/admin/products?action=new"
                            className="flex items-center gap-3 p-4 rounded-xl border border-gray-200 dark:border-gray-700 hover:border-violet-500 hover:bg-violet-50 dark:hover:bg-violet-900/20 transition-colors"
                        >
                            <Package className="w-5 h-5 text-violet-500" />
                            <span className="font-medium">Thêm sản phẩm</span>
                        </Link>
                        <Link
                            href="/admin/orders"
                            className="flex items-center gap-3 p-4 rounded-xl border border-gray-200 dark:border-gray-700 hover:border-blue-500 hover:bg-blue-50 dark:hover:bg-blue-900/20 transition-colors"
                        >
                            <ShoppingCart className="w-5 h-5 text-blue-500" />
                            <span className="font-medium">Xem đơn hàng</span>
                        </Link>
                        <Link
                            href="/admin/messages"
                            className="flex items-center gap-3 p-4 rounded-xl border border-gray-200 dark:border-gray-700 hover:border-orange-500 hover:bg-orange-50 dark:hover:bg-orange-900/20 transition-colors"
                        >
                            <MessageSquare className="w-5 h-5 text-orange-500" />
                            <span className="font-medium">Tin nhắn</span>
                        </Link>
                        <Link
                            href="/admin/users"
                            className="flex items-center gap-3 p-4 rounded-xl border border-gray-200 dark:border-gray-700 hover:border-purple-500 hover:bg-purple-50 dark:hover:bg-purple-900/20 transition-colors"
                        >
                            <Users className="w-5 h-5 text-purple-500" />
                            <span className="font-medium">Người dùng</span>
                        </Link>
                    </div>
                </div>
            </div>
        </div>
    );
}
