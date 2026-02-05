'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { Plus, Pencil, Trash2, Search, Percent, DollarSign } from 'lucide-react';
import AdminHeader from '@/components/admin/AdminHeader';
import DataTable from '@/components/admin/DataTable';
import { ConfirmModal } from '@/components/admin/Modal';
import { couponsService } from '@/lib/services/admin/coupons-service';
import { Coupon } from '@/lib/services/admin/dashboard-service';

export default function CouponsPage() {
    const router = useRouter();
    const [coupons, setCoupons] = useState<Coupon[]>([]);
    const [loading, setLoading] = useState(true);
    const [searchQuery, setSearchQuery] = useState('');

    // Delete modal state
    const [isDeleteOpen, setIsDeleteOpen] = useState(false);
    const [selectedCoupon, setSelectedCoupon] = useState<Coupon | null>(null);
    const [deleteLoading, setDeleteLoading] = useState(false);

    useEffect(() => {
        loadCoupons();
    }, []);

    const loadCoupons = async () => {
        setLoading(true);
        try {
            const data = await couponsService.getAll();
            setCoupons(data);
        } catch (error) {
            console.error('Error loading coupons:', error);
        } finally {
            setLoading(false);
        }
    };

    const openDeleteModal = (coupon: Coupon) => {
        setSelectedCoupon(coupon);
        setIsDeleteOpen(true);
    };

    const handleDelete = async () => {
        if (!selectedCoupon) return;
        setDeleteLoading(true);

        try {
            await couponsService.delete(selectedCoupon.id);
            setIsDeleteOpen(false);
            loadCoupons();
        } catch (error) {
            console.error('Error deleting coupon:', error);
            alert('Có lỗi xảy ra!');
        } finally {
            setDeleteLoading(false);
        }
    };

    const formatCurrency = (value: number) => {
        return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
    };

    const formatDate = (dateStr: string) => {
        return new Date(dateStr).toLocaleDateString('vi-VN');
    };

    const isExpired = (dateStr: string) => new Date(dateStr) < new Date();

    const filteredCoupons = coupons.filter(
        (c) =>
            c.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
            c.code.toLowerCase().includes(searchQuery.toLowerCase())
    );

    const columns = [
        {
            key: 'code',
            header: 'Mã',
            render: (item: Coupon) => (
                <div className="flex items-center gap-3">
                    <div className={`w-10 h-10 rounded-xl flex items-center justify-center ${item.isPercent ? 'bg-green-100 dark:bg-green-900/30' : 'bg-blue-100 dark:bg-blue-900/30'
                        }`}>
                        {item.isPercent ? (
                            <Percent className="w-5 h-5 text-green-600" />
                        ) : (
                            <DollarSign className="w-5 h-5 text-blue-600" />
                        )}
                    </div>
                    <div>
                        <p className="font-mono font-bold text-violet-600">{item.code}</p>
                        <p className="text-sm text-gray-500">{item.name}</p>
                    </div>
                </div>
            ),
        },
        {
            key: 'discount',
            header: 'Giảm giá',
            render: (item: Coupon) => (
                <span className="font-semibold text-green-600">
                    {item.isPercent ? `${item.discountValue}%` : formatCurrency(item.discountValue)}
                </span>
            ),
        },
        {
            key: 'usage',
            header: 'Đã dùng',
            render: (item: Coupon) => (
                <div>
                    <p className="font-medium">{item.usedCount} / {item.quantity}</p>
                    <div className="w-20 h-1.5 bg-gray-200 dark:bg-gray-700 rounded-full mt-1">
                        <div
                            className="h-full bg-violet-500 rounded-full"
                            style={{ width: `${Math.min((item.usedCount / item.quantity) * 100, 100)}%` }}
                        />
                    </div>
                </div>
            ),
        },
        {
            key: 'validity',
            header: 'Thời hạn',
            render: (item: Coupon) => (
                <div className="text-sm">
                    <p>{formatDate(item.dateStart)} - {formatDate(item.dateExpired)}</p>
                    {isExpired(item.dateExpired) && (
                        <span className="text-xs text-red-500 font-medium">Hết hạn</span>
                    )}
                </div>
            ),
        },
        {
            key: 'status',
            header: 'Trạng thái',
            render: (item: Coupon) => (
                <span
                    className={`inline-flex px-2.5 py-1 text-xs font-medium rounded-full ${item.status === 1
                        ? 'bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400'
                        : 'bg-gray-100 text-gray-700 dark:bg-gray-700 dark:text-gray-400'
                        }`}
                >
                    {item.status === 1 ? 'Hoạt động' : 'Tạm dừng'}
                </span>
            ),
        },
        {
            key: 'actions',
            header: 'Thao tác',
            render: (item: Coupon) => (
                <div className="flex items-center gap-2">
                    <button
                        onClick={(e) => {
                            e.stopPropagation();
                            router.push(`/admin/coupons/${item.id}/edit`);
                        }}
                        className="p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 text-blue-600"
                    >
                        <Pencil className="w-4 h-4" />
                    </button>
                    <button
                        onClick={(e) => {
                            e.stopPropagation();
                            openDeleteModal(item);
                        }}
                        className="p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 text-red-600"
                    >
                        <Trash2 className="w-4 h-4" />
                    </button>
                </div>
            ),
        },
    ];

    return (
        <div className="min-h-screen">
            <AdminHeader title="Mã giảm giá" subtitle={`${coupons.length} mã`} />

            <div className="p-6 space-y-6">
                {/* Toolbar */}
                <div className="flex flex-col sm:flex-row gap-4 justify-between">
                    <div className="relative">
                        <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
                        <input
                            type="text"
                            placeholder="Tìm kiếm mã..."
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                            className="pl-10 pr-4 py-2.5 w-full sm:w-80 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800"
                        />
                    </div>
                    <button
                        onClick={() => router.push('/admin/coupons/create')}
                        className="flex items-center gap-2 px-4 py-2.5 bg-violet-600 hover:bg-violet-700 text-white rounded-xl font-medium transition-colors"
                    >
                        <Plus className="w-5 h-5" />
                        Thêm mã
                    </button>
                </div>

                {/* Table */}
                <DataTable
                    columns={columns}
                    data={filteredCoupons}
                    loading={loading}
                    keyExtractor={(item) => item.id}
                    emptyMessage="Chưa có mã giảm giá nào"
                    onRowClick={(item) => router.push(`/admin/coupons/${item.id}/edit`)}
                />
            </div>

            {/* Delete Modal */}
            <ConfirmModal
                isOpen={isDeleteOpen}
                onClose={() => setIsDeleteOpen(false)}
                onConfirm={handleDelete}
                title="Xóa mã giảm giá"
                message={`Bạn có chắc chắn muốn xóa mã "${selectedCoupon?.code}"?`}
                confirmText="Xóa"
                loading={deleteLoading}
            />
        </div>
    );
}
