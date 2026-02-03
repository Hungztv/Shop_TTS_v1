'use client';

import { useEffect, useState } from 'react';
import { Plus, Pencil, Trash2, Search, Ticket, Calendar, Percent, DollarSign } from 'lucide-react';
import AdminHeader from '@/components/admin/AdminHeader';
import DataTable from '@/components/admin/DataTable';
import Modal, { ConfirmModal } from '@/components/admin/Modal';
import { couponsService, CreateCouponDto } from '@/lib/services/admin/coupons-service';
import { Coupon } from '@/lib/services/admin/dashboard-service';

export default function CouponsPage() {
    const [coupons, setCoupons] = useState<Coupon[]>([]);
    const [loading, setLoading] = useState(true);
    const [searchQuery, setSearchQuery] = useState('');

    // Modal states
    const [isFormOpen, setIsFormOpen] = useState(false);
    const [isDeleteOpen, setIsDeleteOpen] = useState(false);
    const [selectedCoupon, setSelectedCoupon] = useState<Coupon | null>(null);
    const [formLoading, setFormLoading] = useState(false);

    // Form state
    const [formData, setFormData] = useState<CreateCouponDto>({
        name: '',
        code: '',
        description: '',
        dateStart: new Date().toISOString().split('T')[0],
        dateExpired: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0],
        discountValue: 0,
        isPercent: true,
        quantity: 100,
        minimumOrderValue: 0,
        status: 1,
    });

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

    const openCreateModal = () => {
        setSelectedCoupon(null);
        setFormData({
            name: '',
            code: '',
            description: '',
            dateStart: new Date().toISOString().split('T')[0],
            dateExpired: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0],
            discountValue: 0,
            isPercent: true,
            quantity: 100,
            minimumOrderValue: 0,
            status: 1,
        });
        setIsFormOpen(true);
    };

    const openEditModal = (coupon: Coupon) => {
        setSelectedCoupon(coupon);
        setFormData({
            name: coupon.name,
            code: coupon.code,
            description: coupon.description,
            dateStart: coupon.dateStart.split('T')[0],
            dateExpired: coupon.dateExpired.split('T')[0],
            discountValue: coupon.discountValue,
            isPercent: coupon.isPercent,
            quantity: coupon.quantity,
            minimumOrderValue: coupon.minimumOrderValue,
            status: coupon.status,
        });
        setIsFormOpen(true);
    };

    const openDeleteModal = (coupon: Coupon) => {
        setSelectedCoupon(coupon);
        setIsDeleteOpen(true);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setFormLoading(true);

        try {
            if (selectedCoupon) {
                await couponsService.update(selectedCoupon.id, formData);
            } else {
                await couponsService.create(formData);
            }
            setIsFormOpen(false);
            loadCoupons();
        } catch (error) {
            console.error('Error saving coupon:', error);
            alert('Có lỗi xảy ra!');
        } finally {
            setFormLoading(false);
        }
    };

    const handleDelete = async () => {
        if (!selectedCoupon) return;
        setFormLoading(true);

        try {
            await couponsService.delete(selectedCoupon.id);
            setIsDeleteOpen(false);
            loadCoupons();
        } catch (error) {
            console.error('Error deleting coupon:', error);
            alert('Có lỗi xảy ra!');
        } finally {
            setFormLoading(false);
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
                    <button onClick={() => openEditModal(item)} className="p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 text-blue-600">
                        <Pencil className="w-4 h-4" />
                    </button>
                    <button onClick={() => openDeleteModal(item)} className="p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 text-red-600">
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
                        onClick={openCreateModal}
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
                />
            </div>

            {/* Create/Edit Modal */}
            <Modal
                isOpen={isFormOpen}
                onClose={() => setIsFormOpen(false)}
                title={selectedCoupon ? 'Sửa mã giảm giá' : 'Thêm mã giảm giá mới'}
                size="lg"
                footer={
                    <div className="flex justify-end gap-3">
                        <button onClick={() => setIsFormOpen(false)} className="px-4 py-2 text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 rounded-xl">
                            Hủy
                        </button>
                        <button
                            onClick={handleSubmit}
                            disabled={formLoading || !formData.name || !formData.code}
                            className="px-4 py-2 bg-violet-600 hover:bg-violet-700 text-white rounded-xl font-medium disabled:opacity-50 flex items-center gap-2"
                        >
                            {formLoading && <div className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin" />}
                            {selectedCoupon ? 'Cập nhật' : 'Tạo mới'}
                        </button>
                    </div>
                }
            >
                <form className="space-y-4">
                    <div className="grid grid-cols-2 gap-4">
                        <div>
                            <label className="block text-sm font-medium mb-1">Tên *</label>
                            <input
                                type="text"
                                value={formData.name}
                                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                                className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800"
                                required
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium mb-1">Mã code *</label>
                            <input
                                type="text"
                                value={formData.code}
                                onChange={(e) => setFormData({ ...formData, code: e.target.value.toUpperCase() })}
                                className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800 font-mono"
                                required
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium mb-1">Loại giảm</label>
                            <select
                                value={formData.isPercent ? 'percent' : 'fixed'}
                                onChange={(e) => setFormData({ ...formData, isPercent: e.target.value === 'percent' })}
                                className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800"
                            >
                                <option value="percent">Giảm %</option>
                                <option value="fixed">Giảm tiền cố định</option>
                            </select>
                        </div>
                        <div>
                            <label className="block text-sm font-medium mb-1">Giá trị giảm</label>
                            <input
                                type="number"
                                value={formData.discountValue}
                                onChange={(e) => setFormData({ ...formData, discountValue: Number(e.target.value) })}
                                className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800"
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium mb-1">Ngày bắt đầu</label>
                            <input
                                type="date"
                                value={formData.dateStart}
                                onChange={(e) => setFormData({ ...formData, dateStart: e.target.value })}
                                className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800"
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium mb-1">Ngày hết hạn</label>
                            <input
                                type="date"
                                value={formData.dateExpired}
                                onChange={(e) => setFormData({ ...formData, dateExpired: e.target.value })}
                                className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800"
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium mb-1">Số lượng</label>
                            <input
                                type="number"
                                value={formData.quantity}
                                onChange={(e) => setFormData({ ...formData, quantity: Number(e.target.value) })}
                                className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800"
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium mb-1">Đơn tối thiểu</label>
                            <input
                                type="number"
                                value={formData.minimumOrderValue}
                                onChange={(e) => setFormData({ ...formData, minimumOrderValue: Number(e.target.value) })}
                                className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800"
                            />
                        </div>
                        <div className="col-span-2">
                            <label className="block text-sm font-medium mb-1">Mô tả</label>
                            <textarea
                                value={formData.description}
                                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                                className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800 resize-none"
                                rows={2}
                            />
                        </div>
                    </div>
                </form>
            </Modal>

            {/* Delete Modal */}
            <ConfirmModal
                isOpen={isDeleteOpen}
                onClose={() => setIsDeleteOpen(false)}
                onConfirm={handleDelete}
                title="Xóa mã giảm giá"
                message={`Bạn có chắc chắn muốn xóa mã "${selectedCoupon?.code}"?`}
                confirmText="Xóa"
                loading={formLoading}
            />
        </div>
    );
}
