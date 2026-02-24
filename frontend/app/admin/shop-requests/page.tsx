'use client';

import { useEffect, useState } from 'react';
import { Search, Eye, Filter, Store } from 'lucide-react';
import AdminHeader from '@/components/admin/AdminHeader';
import DataTable from '@/components/admin/DataTable';
import ReviewDialog from '@/components/admin/shops/ReviewDialog';
import { shopService } from '@/lib/services/shop-service';
import type { BusinessRegistrationDto } from '@/types/shop';

const statusFilters = [
    { value: '', label: 'Tất cả' },
    { value: 'Pending', label: 'Chờ duyệt' },
    { value: 'Approved', label: 'Đã duyệt' },
    { value: 'Rejected', label: 'Từ chối' },
];

const statusBadgeStyles: Record<string, string> = {
    Pending: 'bg-amber-100 text-amber-700 dark:bg-amber-900/30 dark:text-amber-400',
    Approved: 'bg-emerald-100 text-emerald-700 dark:bg-emerald-900/30 dark:text-emerald-400',
    Rejected: 'bg-red-100 text-red-700 dark:bg-red-900/30 dark:text-red-400',
};

const statusLabels: Record<string, string> = {
    Pending: 'Chờ duyệt',
    Approved: 'Đã duyệt',
    Rejected: 'Từ chối',
};

export default function ShopRequestsPage() {
    const [registrations, setRegistrations] = useState<BusinessRegistrationDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [statusFilter, setStatusFilter] = useState('');
    const [searchQuery, setSearchQuery] = useState('');

    // Review dialog
    const [selectedReg, setSelectedReg] = useState<BusinessRegistrationDto | null>(null);
    const [isDialogOpen, setIsDialogOpen] = useState(false);

    useEffect(() => {
        loadRegistrations();
    }, [statusFilter]);

    const loadRegistrations = async () => {
        setLoading(true);
        try {
            const data = await shopService.getAdminRegistrations(statusFilter || undefined);
            setRegistrations(data);
        } catch (error) {
            console.error('Error loading registrations:', error);
        } finally {
            setLoading(false);
        }
    };

    const openReview = (reg: BusinessRegistrationDto) => {
        setSelectedReg(reg);
        setIsDialogOpen(true);
    };

    // Filtered by search
    const filtered = searchQuery
        ? registrations.filter(
            (r) =>
                r.companyName.toLowerCase().includes(searchQuery.toLowerCase()) ||
                r.taxCode.includes(searchQuery) ||
                r.ownerName.toLowerCase().includes(searchQuery.toLowerCase()) ||
                r.email.toLowerCase().includes(searchQuery.toLowerCase())
        )
        : registrations;

    const columns = [
        {
            key: 'id',
            header: 'ID',
            width: '60px',
            render: (item: BusinessRegistrationDto) => (
                <span className="text-sm font-medium text-gray-500">#{item.id}</span>
            ),
        },
        {
            key: 'companyName',
            header: 'Công ty',
            render: (item: BusinessRegistrationDto) => (
                <div>
                    <p className="font-medium text-gray-800 dark:text-white text-sm">{item.companyName}</p>
                    <p className="text-xs text-gray-400">MST: {item.taxCode}</p>
                </div>
            ),
        },
        {
            key: 'ownerName',
            header: 'Chủ sở hữu',
            render: (item: BusinessRegistrationDto) => (
                <div>
                    <p className="text-sm text-gray-700 dark:text-gray-300">{item.ownerName}</p>
                    <p className="text-xs text-gray-400">{item.email}</p>
                </div>
            ),
        },
        {
            key: 'phone',
            header: 'SĐT',
            render: (item: BusinessRegistrationDto) => (
                <span className="text-sm text-gray-600 dark:text-gray-300">{item.phone}</span>
            ),
        },
        {
            key: 'status',
            header: 'Trạng thái',
            render: (item: BusinessRegistrationDto) => (
                <span className={`px-2.5 py-1 text-xs font-semibold rounded-full ${statusBadgeStyles[item.status] || ''}`}>
                    {statusLabels[item.status] || item.status}
                </span>
            ),
        },
        {
            key: 'createdAt',
            header: 'Ngày gửi',
            render: (item: BusinessRegistrationDto) => (
                <span className="text-sm text-gray-500">
                    {new Date(item.createdAt).toLocaleDateString('vi-VN')}
                </span>
            ),
        },
        {
            key: 'actions',
            header: 'Hành động',
            width: '100px',
            render: (item: BusinessRegistrationDto) => (
                <button
                    onClick={(e) => { e.stopPropagation(); openReview(item); }}
                    className="flex items-center gap-1.5 px-3 py-1.5 text-xs font-medium text-violet-600 bg-violet-50 dark:bg-violet-900/20 rounded-lg hover:bg-violet-100 dark:hover:bg-violet-900/40 transition-colors"
                >
                    <Eye className="w-3.5 h-3.5" />
                    Xem xét
                </button>
            ),
        },
    ];

    return (
        <>
            <AdminHeader
                title="Yêu cầu mở shop"
                subtitle={`${registrations.length} đăng ký kinh doanh`}
            />

            <div className="p-6 space-y-4">
                {/* Filters Bar */}
                <div className="flex flex-col sm:flex-row gap-3">
                    {/* Search */}
                    <div className="relative flex-1">
                        <Search className="absolute left-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
                        <input
                            type="text"
                            placeholder="Tìm theo tên, MST, email..."
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                            className="w-full pl-10 pr-4 py-2.5 rounded-xl border border-gray-200 dark:border-gray-700 bg-white dark:bg-gray-800 text-sm focus:border-violet-400 focus:ring-2 focus:ring-violet-100 outline-none transition-all"
                        />
                    </div>

                    {/* Status filter */}
                    <div className="flex items-center gap-2">
                        <Filter className="w-4 h-4 text-gray-400" />
                        <div className="flex gap-1">
                            {statusFilters.map((f) => (
                                <button
                                    key={f.value}
                                    onClick={() => { setStatusFilter(f.value); }}
                                    className={`px-3 py-2 text-xs font-medium rounded-lg transition-colors ${
                                        statusFilter === f.value
                                            ? 'bg-violet-600 text-white'
                                            : 'bg-gray-100 dark:bg-gray-800 text-gray-600 dark:text-gray-300 hover:bg-gray-200 dark:hover:bg-gray-700'
                                    }`}
                                >
                                    {f.label}
                                </button>
                            ))}
                        </div>
                    </div>
                </div>

                {/* Table */}
                <DataTable
                    columns={columns}
                    data={filtered}
                    loading={loading}
                    totalCount={filtered.length}
                    keyExtractor={(item) => item.id}
                    onRowClick={openReview}
                    emptyMessage="Chưa có yêu cầu đăng ký nào"
                />
            </div>

            {/* Review Dialog */}
            {selectedReg && (
                <ReviewDialog
                    registration={selectedReg}
                    isOpen={isDialogOpen}
                    onClose={() => { setIsDialogOpen(false); setSelectedReg(null); }}
                    onSuccess={loadRegistrations}
                />
            )}
        </>
    );
}
