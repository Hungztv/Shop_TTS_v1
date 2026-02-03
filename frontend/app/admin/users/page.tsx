'use client';

import { useEffect, useState } from 'react';
import { Search, Trash2, User, Mail, Phone, MapPin, Calendar, Shield } from 'lucide-react';
import AdminHeader from '@/components/admin/AdminHeader';
import DataTable from '@/components/admin/DataTable';
import Modal, { ConfirmModal } from '@/components/admin/Modal';
import { usersService } from '@/lib/services/admin/users-service';
import { AppUser } from '@/lib/services/admin/dashboard-service';

export default function UsersPage() {
    const [users, setUsers] = useState<AppUser[]>([]);
    const [loading, setLoading] = useState(true);
    const [totalCount, setTotalCount] = useState(0);

    // Filters
    const [page, setPage] = useState(1);
    const [searchQuery, setSearchQuery] = useState('');

    // Modal states
    const [selectedUser, setSelectedUser] = useState<AppUser | null>(null);
    const [isDetailOpen, setIsDetailOpen] = useState(false);
    const [isDeleteOpen, setIsDeleteOpen] = useState(false);
    const [formLoading, setFormLoading] = useState(false);

    useEffect(() => {
        loadUsers();
    }, [page]);

    const loadUsers = async () => {
        setLoading(true);
        try {
            const res = await usersService.getAll({
                page,
                pageSize: 10,
                search: searchQuery || undefined,
            });
            setUsers(res.items);
            setTotalCount(res.totalCount);
        } catch (error) {
            console.error('Error loading users:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleSearch = () => {
        setPage(1);
        loadUsers();
    };

    const openDetailModal = (user: AppUser) => {
        setSelectedUser(user);
        setIsDetailOpen(true);
    };

    const openDeleteModal = (user: AppUser) => {
        setSelectedUser(user);
        setIsDeleteOpen(true);
    };

    const handleDelete = async () => {
        if (!selectedUser) return;
        setFormLoading(true);

        try {
            await usersService.delete(selectedUser.id);
            setIsDeleteOpen(false);
            loadUsers();
        } catch (error) {
            console.error('Error deleting user:', error);
            alert('Có lỗi xảy ra!');
        } finally {
            setFormLoading(false);
        }
    };

    const formatDate = (dateStr?: string) => {
        if (!dateStr) return '-';
        return new Date(dateStr).toLocaleDateString('vi-VN');
    };

    const formatDateTime = (dateStr?: string) => {
        if (!dateStr) return '-';
        return new Date(dateStr).toLocaleString('vi-VN');
    };

    const columns = [
        {
            key: 'user',
            header: 'Người dùng',
            render: (item: AppUser) => (
                <div className="flex items-center gap-3">
                    {item.avatar ? (
                        <img
                            src={item.avatar}
                            alt={item.fullName || item.userName}
                            className="w-10 h-10 rounded-full object-cover"
                        />
                    ) : (
                        <div className="w-10 h-10 rounded-full bg-gradient-to-br from-violet-500 to-purple-500 flex items-center justify-center text-white font-bold">
                            {(item.fullName || item.userName || item.email)?.[0]?.toUpperCase() || 'U'}
                        </div>
                    )}
                    <div>
                        <p className="font-medium text-gray-900 dark:text-white">
                            {item.fullName || item.userName || 'Chưa đặt tên'}
                        </p>
                        <p className="text-sm text-gray-500">@{item.userName}</p>
                    </div>
                </div>
            ),
        },
        {
            key: 'email',
            header: 'Email',
            render: (item: AppUser) => (
                <span className="text-gray-600 dark:text-gray-400">{item.email}</span>
            ),
        },
        {
            key: 'phone',
            header: 'SĐT',
            render: (item: AppUser) => (
                <span className="text-gray-600 dark:text-gray-400">{item.phoneNumber || '-'}</span>
            ),
        },
        {
            key: 'role',
            header: 'Vai trò',
            render: (item: AppUser) => {
                const roleColors: Record<string, string> = {
                    Admin: 'bg-red-100 text-red-700 dark:bg-red-900/30 dark:text-red-400',
                    Seller: 'bg-blue-100 text-blue-700 dark:bg-blue-900/30 dark:text-blue-400',
                    Customer: 'bg-gray-100 text-gray-700 dark:bg-gray-700 dark:text-gray-400',
                };
                const role = item.roleId || 'Customer';
                return (
                    <span className={`inline-flex items-center gap-1 px-2.5 py-1 text-xs font-medium rounded-full ${roleColors[role] || roleColors.Customer}`}>
                        <Shield className="w-3 h-3" />
                        {role}
                    </span>
                );
            },
        },
        {
            key: 'lastLogin',
            header: 'Đăng nhập cuối',
            render: (item: AppUser) => (
                <span className="text-sm text-gray-500">{formatDateTime(item.lastLoginAt)}</span>
            ),
        },
        {
            key: 'actions',
            header: '',
            render: (item: AppUser) => (
                <div className="flex items-center gap-1">
                    <button
                        onClick={(e) => { e.stopPropagation(); openDeleteModal(item); }}
                        className="p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 text-red-600"
                        title="Xóa"
                    >
                        <Trash2 className="w-4 h-4" />
                    </button>
                </div>
            ),
        },
    ];

    return (
        <div className="min-h-screen">
            <AdminHeader title="Người dùng" subtitle={`${totalCount} người dùng`} />

            <div className="p-6 space-y-6">
                {/* Toolbar */}
                <div className="flex gap-4">
                    <div className="relative flex-1 max-w-md">
                        <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
                        <input
                            type="text"
                            placeholder="Tìm theo tên, email..."
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                            onKeyDown={(e) => e.key === 'Enter' && handleSearch()}
                            className="pl-10 pr-4 py-2.5 w-full border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800"
                        />
                    </div>
                    <button
                        onClick={handleSearch}
                        className="px-4 py-2.5 bg-violet-600 hover:bg-violet-700 text-white rounded-xl font-medium"
                    >
                        Tìm kiếm
                    </button>
                </div>

                {/* Table */}
                <DataTable
                    columns={columns}
                    data={users}
                    loading={loading}
                    page={page}
                    pageSize={10}
                    totalCount={totalCount}
                    onPageChange={setPage}
                    onRowClick={openDetailModal}
                    keyExtractor={(item) => item.id}
                    emptyMessage="Chưa có người dùng nào"
                />
            </div>

            {/* Detail Modal */}
            <Modal
                isOpen={isDetailOpen}
                onClose={() => setIsDetailOpen(false)}
                title="Thông tin người dùng"
                size="lg"
            >
                {selectedUser && (
                    <div className="space-y-6">
                        {/* Avatar & Name */}
                        <div className="flex items-center gap-4 p-4 bg-gray-50 dark:bg-gray-700/50 rounded-xl">
                            {selectedUser.avatar ? (
                                <img
                                    src={selectedUser.avatar}
                                    alt=""
                                    className="w-16 h-16 rounded-full object-cover"
                                />
                            ) : (
                                <div className="w-16 h-16 rounded-full bg-gradient-to-br from-violet-500 to-purple-500 flex items-center justify-center text-white text-2xl font-bold">
                                    {(selectedUser.fullName || selectedUser.userName || selectedUser.email)?.[0]?.toUpperCase() || 'U'}
                                </div>
                            )}
                            <div>
                                <h3 className="text-xl font-semibold text-gray-900 dark:text-white">
                                    {selectedUser.fullName || selectedUser.userName || 'Chưa đặt tên'}
                                </h3>
                                <p className="text-gray-500">@{selectedUser.userName}</p>
                            </div>
                        </div>

                        {/* Info Grid */}
                        <div className="grid grid-cols-2 gap-4">
                            <div className="flex items-start gap-3 p-3 bg-gray-50 dark:bg-gray-700/50 rounded-xl">
                                <Mail className="w-5 h-5 text-gray-400 mt-0.5" />
                                <div>
                                    <p className="text-sm text-gray-500">Email</p>
                                    <p className="font-medium">{selectedUser.email}</p>
                                </div>
                            </div>
                            <div className="flex items-start gap-3 p-3 bg-gray-50 dark:bg-gray-700/50 rounded-xl">
                                <Phone className="w-5 h-5 text-gray-400 mt-0.5" />
                                <div>
                                    <p className="text-sm text-gray-500">Số điện thoại</p>
                                    <p className="font-medium">{selectedUser.phoneNumber || '-'}</p>
                                </div>
                            </div>
                            <div className="flex items-start gap-3 p-3 bg-gray-50 dark:bg-gray-700/50 rounded-xl">
                                <MapPin className="w-5 h-5 text-gray-400 mt-0.5" />
                                <div>
                                    <p className="text-sm text-gray-500">Địa chỉ</p>
                                    <p className="font-medium">{selectedUser.address || '-'}</p>
                                </div>
                            </div>
                            <div className="flex items-start gap-3 p-3 bg-gray-50 dark:bg-gray-700/50 rounded-xl">
                                <Calendar className="w-5 h-5 text-gray-400 mt-0.5" />
                                <div>
                                    <p className="text-sm text-gray-500">Ngày sinh</p>
                                    <p className="font-medium">{formatDate(selectedUser.dateOfBirth)}</p>
                                </div>
                            </div>
                            <div className="flex items-start gap-3 p-3 bg-gray-50 dark:bg-gray-700/50 rounded-xl">
                                <User className="w-5 h-5 text-gray-400 mt-0.5" />
                                <div>
                                    <p className="text-sm text-gray-500">Nghề nghiệp</p>
                                    <p className="font-medium">{selectedUser.occupation || '-'}</p>
                                </div>
                            </div>
                            <div className="flex items-start gap-3 p-3 bg-gray-50 dark:bg-gray-700/50 rounded-xl">
                                <Shield className="w-5 h-5 text-gray-400 mt-0.5" />
                                <div>
                                    <p className="text-sm text-gray-500">Vai trò</p>
                                    <p className="font-medium">{selectedUser.roleId || 'Customer'}</p>
                                </div>
                            </div>
                        </div>

                        {/* Timestamps */}
                        <div className="border-t border-gray-200 dark:border-gray-700 pt-4 text-sm text-gray-500">
                            <p>Ngày đăng ký: {formatDateTime(selectedUser.createdAt)}</p>
                            <p>Đăng nhập cuối: {formatDateTime(selectedUser.lastLoginAt)}</p>
                        </div>
                    </div>
                )}
            </Modal>

            {/* Delete Modal */}
            <ConfirmModal
                isOpen={isDeleteOpen}
                onClose={() => setIsDeleteOpen(false)}
                onConfirm={handleDelete}
                title="Xóa người dùng"
                message={`Bạn có chắc chắn muốn xóa người dùng "${selectedUser?.fullName || selectedUser?.userName}"? Hành động này không thể hoàn tác.`}
                confirmText="Xóa"
                loading={formLoading}
            />
        </div>
    );
}
