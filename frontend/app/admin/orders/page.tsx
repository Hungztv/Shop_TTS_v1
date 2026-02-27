'use client';

import { useEffect, useState } from 'react';
import { Search, Eye, ShoppingCart, ChevronRight, X } from 'lucide-react';
import AdminHeader from '@/components/admin/AdminHeader';
import DataTable from '@/components/admin/DataTable';
import Modal from '@/components/admin/Modal';
import { ordersService, orderStatusConfig } from '@/lib/services/admin/orders-service';
import { Order } from '@/lib/services/admin/dashboard-service';

export default function OrdersPage() {
    const [orders, setOrders] = useState<Order[]>([]);
    const [loading, setLoading] = useState(true);
    const [totalCount, setTotalCount] = useState(0);

    // Filters
    const [page, setPage] = useState(1);
    const [searchQuery, setSearchQuery] = useState('');
    const [statusFilter, setStatusFilter] = useState<number | undefined>();

    // Modal states
    const [selectedOrder, setSelectedOrder] = useState<Order | null>(null);
    const [isDetailOpen, setIsDetailOpen] = useState(false);
    const [isStatusOpen, setIsStatusOpen] = useState(false);
    const [newStatus, setNewStatus] = useState(0);
    const [formLoading, setFormLoading] = useState(false);

    useEffect(() => {
        loadOrders();
    }, [page, statusFilter]);

    const loadOrders = async () => {
        setLoading(true);
        try {
            const res = await ordersService.getAll({
                page,
                pageSize: 10,
                status: statusFilter,
                search: searchQuery || undefined,
            });
            setOrders(res.items);
            setTotalCount(res.totalCount);
        } catch (error) {
            console.error('Error loading orders:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleSearch = () => {
        setPage(1);
        loadOrders();
    };

    const openDetailModal = (order: Order) => {
        setSelectedOrder(order);
        setIsDetailOpen(true);
    };

    const openStatusModal = (order: Order) => {
        setSelectedOrder(order);
        setNewStatus(order.status);
        setIsStatusOpen(true);
    };

    const handleUpdateStatus = async () => {
        if (!selectedOrder) return;
        setFormLoading(true);

        try {
            await ordersService.updateStatus(selectedOrder.id, newStatus);
            setIsStatusOpen(false);
            loadOrders();
        } catch (error) {
            console.error('Error updating status:', error);
            alert('Có lỗi xảy ra!');
        } finally {
            setFormLoading(false);
        }
    };

    const formatCurrency = (value: number) => {
        return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
    };

    const formatDate = (dateStr: string) => {
        return new Date(dateStr).toLocaleString('vi-VN', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
        });
    };

    const columns = [
        {
            key: 'orderCode',
            header: 'Mã đơn',
            render: (item: Order) => (
                <span className="font-mono font-semibold text-violet-600">#{item.orderCode}</span>
            ),
        },
        {
            key: 'customer',
            header: 'Khách hàng',
            render: (item: Order) => (
                <div>
                    <p className="font-medium text-gray-900 dark:text-white">{item.name}</p>
                    <p className="text-sm text-gray-500">{item.phoneNumber}</p>
                </div>
            ),
        },
        {
            key: 'total',
            header: 'Tổng tiền',
            render: (item: Order) => (
                <span className="font-semibold text-gray-900 dark:text-white">
                    {formatCurrency(item.total)}
                </span>
            ),
        },
        {
            key: 'status',
            header: 'Trạng thái',
            render: (item: Order) => {
                const config = orderStatusConfig[item.status as keyof typeof orderStatusConfig];
                return (
                    <button
                        onClick={(e) => { e.stopPropagation(); openStatusModal(item); }}
                        className={`inline-flex items-center gap-1.5 px-3 py-1.5 text-sm font-medium rounded-full transition-opacity hover:opacity-80 ${config?.color || 'bg-gray-100 text-gray-700'}`}
                    >
                        <span>{config?.icon}</span>
                        {config?.label || 'Unknown'}
                    </button>
                );
            },
        },
        {
            key: 'paymentMethod',
            header: 'Thanh toán',
            render: (item: Order) => (
                <span className="text-gray-600 dark:text-gray-400">{item.paymentMethod}</span>
            ),
        },
        {
            key: 'createdAt',
            header: 'Ngày đặt',
            render: (item: Order) => (
                <span className="text-gray-500 text-sm">{formatDate(item.createdAt)}</span>
            ),
        },
        {
            key: 'actions',
            header: '',
            render: (item: Order) => (
                <button
                    onClick={(e) => { e.stopPropagation(); openDetailModal(item); }}
                    className="p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 text-violet-600"
                    title="Xem chi tiết"
                >
                    <Eye className="w-5 h-5" />
                </button>
            ),
        },
    ];

    return (
        <div className="min-h-screen">
            <AdminHeader title="Đơn hàng" subtitle={`${totalCount} đơn hàng`} />

            <div className="p-6 space-y-6">
                {/* Toolbar */}
                <div className="flex flex-col sm:flex-row gap-4 justify-between">
                    <div className="flex gap-3">
                        <div className="relative">
                            <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
                            <input
                                type="text"
                                placeholder="Tìm theo mã đơn, tên KH..."
                                value={searchQuery}
                                onChange={(e) => setSearchQuery(e.target.value)}
                                onKeyDown={(e) => e.key === 'Enter' && handleSearch()}
                                className="pl-10 pr-4 py-2.5 w-64 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800"
                            />
                        </div>
                        <select
                            value={statusFilter ?? ''}
                            onChange={(e) => setStatusFilter(e.target.value ? Number(e.target.value) : undefined)}
                            className="px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800"
                        >
                            <option value="">Tất cả trạng thái</option>
                            {Object.entries(orderStatusConfig).map(([key, config]) => (
                                <option key={key} value={key}>{config.icon} {config.label}</option>
                            ))}
                        </select>
                    </div>
                </div>

                {/* Table */}
                <DataTable
                    columns={columns}
                    data={orders}
                    loading={loading}
                    page={page}
                    pageSize={10}
                    totalCount={totalCount}
                    onPageChange={setPage}
                    keyExtractor={(item) => item.id}
                    emptyMessage="Chưa có đơn hàng nào"
                />
            </div>

            {/* Order Detail Modal */}
            <Modal
                isOpen={isDetailOpen}
                onClose={() => setIsDetailOpen(false)}
                title={`Chi tiết đơn hàng #${selectedOrder?.orderCode}`}
                size="lg"
            >
                {selectedOrder && (
                    <div className="space-y-6">
                        {/* Customer Info */}
                        <div className="bg-gray-50 dark:bg-gray-700/50 rounded-xl p-4">
                            <h4 className="font-semibold mb-3">Thông tin khách hàng</h4>
                            <div className="grid grid-cols-2 gap-4 text-sm">
                                <div>
                                    <span className="text-gray-500">Tên:</span>
                                    <p className="font-medium">{selectedOrder.name}</p>
                                </div>
                                <div>
                                    <span className="text-gray-500">SĐT:</span>
                                    <p className="font-medium">{selectedOrder.phoneNumber}</p>
                                </div>
                                <div>
                                    <span className="text-gray-500">Email:</span>
                                    <p className="font-medium">{selectedOrder.email}</p>
                                </div>
                                <div>
                                    <span className="text-gray-500">Địa chỉ:</span>
                                    <p className="font-medium">{selectedOrder.address}</p>
                                </div>
                            </div>
                        </div>

                        {/* Products - Grouped by Shop */}
                        <div>
                            <h4 className="font-semibold mb-3">Sản phẩm</h4>
                            {(() => {
                                const details = selectedOrder.orderDetails || [];
                                const grouped = details.reduce((acc, item) => {
                                    const shopName = item.shopName || 'Chưa xác định';
                                    if (!acc[shopName]) acc[shopName] = [];
                                    acc[shopName].push(item);
                                    return acc;
                                }, {} as Record<string, typeof details>);
                                const shopNames = Object.keys(grouped);

                                return (
                                    <div className="space-y-4">
                                        {shopNames.map((shopName) => (
                                            <div key={shopName}>
                                                {shopNames.length > 1 && (
                                                    <div className="flex items-center gap-2 mb-2">
                                                        <span className="text-lg">🏪</span>
                                                        <span className="text-sm font-semibold text-gray-700 dark:text-gray-300">{shopName}</span>
                                                    </div>
                                                )}
                                                <div className="space-y-2">
                                                    {grouped[shopName].map((item) => (
                                                        <div key={item.id} className="flex items-center gap-4 p-3 bg-gray-50 dark:bg-gray-700/50 rounded-xl">
                                                            <img
                                                                src={item.productImage || '/placeholder.png'}
                                                                alt={item.productName}
                                                                className="w-16 h-16 rounded-lg object-cover"
                                                            />
                                                            <div className="flex-1">
                                                                <p className="font-medium">{item.productName}</p>
                                                                <p className="text-sm text-gray-500">
                                                                    {formatCurrency(item.price)} x {item.quantity}
                                                                </p>
                                                            </div>
                                                            <p className="font-semibold text-violet-600">
                                                                {formatCurrency(item.price * item.quantity)}
                                                            </p>
                                                        </div>
                                                    ))}
                                                </div>
                                            </div>
                                        ))}
                                    </div>
                                );
                            })()}
                        </div>

                        {/* Summary */}
                        <div className="border-t border-gray-200 dark:border-gray-700 pt-4 space-y-2">
                            <div className="flex justify-between text-sm">
                                <span>Tạm tính:</span>
                                <span>{formatCurrency(selectedOrder.subtotal)}</span>
                            </div>
                            <div className="flex justify-between text-sm">
                                <span>Phí ship:</span>
                                <span>{formatCurrency(selectedOrder.shippingCost)}</span>
                            </div>
                            {selectedOrder.discountAmount > 0 && (
                                <div className="flex justify-between text-sm text-green-600">
                                    <span>Giảm giá:</span>
                                    <span>-{formatCurrency(selectedOrder.discountAmount)}</span>
                                </div>
                            )}
                            <div className="flex justify-between font-bold text-lg pt-2 border-t border-gray-200 dark:border-gray-700">
                                <span>Tổng cộng:</span>
                                <span className="text-violet-600">{formatCurrency(selectedOrder.total)}</span>
                            </div>
                        </div>
                    </div>
                )}
            </Modal>

            {/* Update Status Modal */}
            <Modal
                isOpen={isStatusOpen}
                onClose={() => setIsStatusOpen(false)}
                title="Cập nhật trạng thái"
                size="sm"
                footer={
                    <div className="flex justify-end gap-3">
                        <button onClick={() => setIsStatusOpen(false)} className="px-4 py-2 text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 rounded-xl">
                            Hủy
                        </button>
                        <button
                            onClick={handleUpdateStatus}
                            disabled={formLoading || newStatus === selectedOrder?.status}
                            className="px-4 py-2 bg-violet-600 hover:bg-violet-700 text-white rounded-xl font-medium disabled:opacity-50 flex items-center gap-2"
                        >
                            {formLoading && <div className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin" />}
                            Cập nhật
                        </button>
                    </div>
                }
            >
                <div className="space-y-3">
                    {Object.entries(orderStatusConfig).map(([key, config]) => (
                        <label
                            key={key}
                            className={`flex items-center gap-3 p-3 rounded-xl cursor-pointer transition-colors ${newStatus === Number(key)
                                ? 'bg-violet-50 dark:bg-violet-900/30 border-2 border-violet-500'
                                : 'bg-gray-50 dark:bg-gray-700/50 border-2 border-transparent hover:border-gray-300'
                                }`}
                        >
                            <input
                                type="radio"
                                name="status"
                                value={key}
                                checked={newStatus === Number(key)}
                                onChange={() => setNewStatus(Number(key))}
                                className="sr-only"
                            />
                            <span className="text-xl">{config.icon}</span>
                            <span className="font-medium">{config.label}</span>
                        </label>
                    ))}
                </div>
            </Modal>
        </div>
    );
}
