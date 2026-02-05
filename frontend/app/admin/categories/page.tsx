'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { Plus, Pencil, Trash2, Search, FolderTree } from 'lucide-react';
import AdminHeader from '@/components/admin/AdminHeader';
import DataTable from '@/components/admin/DataTable';
import { ConfirmModal } from '@/components/admin/Modal';
import { categoriesService } from '@/lib/services/admin/categories-service';
import { Category } from '@/lib/services/admin/dashboard-service';

export default function CategoriesPage() {
    const router = useRouter();
    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState(true);
    const [searchQuery, setSearchQuery] = useState('');

    // Delete modal state
    const [isDeleteOpen, setIsDeleteOpen] = useState(false);
    const [selectedCategory, setSelectedCategory] = useState<Category | null>(null);
    const [deleteLoading, setDeleteLoading] = useState(false);

    useEffect(() => {
        loadCategories();
    }, []);

    const loadCategories = async () => {
        setLoading(true);
        try {
            const data = await categoriesService.getAll();
            setCategories(data);
        } catch (error) {
            console.error('Error loading categories:', error);
        } finally {
            setLoading(false);
        }
    };

    const openDeleteModal = (category: Category) => {
        setSelectedCategory(category);
        setIsDeleteOpen(true);
    };

    const handleDelete = async () => {
        if (!selectedCategory) return;
        setDeleteLoading(true);

        try {
            await categoriesService.delete(selectedCategory.id);
            setIsDeleteOpen(false);
            loadCategories();
        } catch (error) {
            console.error('Error deleting category:', error);
            alert('Có lỗi xảy ra!');
        } finally {
            setDeleteLoading(false);
        }
    };

    const filteredCategories = categories.filter(
        (cat) =>
            cat.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
            cat.description.toLowerCase().includes(searchQuery.toLowerCase())
    );

    const columns = [
        {
            key: 'id',
            header: 'ID',
            width: '80px',
        },
        {
            key: 'name',
            header: 'Tên danh mục',
            render: (item: Category) => (
                <div className="flex items-center gap-3">
                    <div className="w-10 h-10 rounded-xl bg-violet-100 dark:bg-violet-900/30 flex items-center justify-center">
                        <FolderTree className="w-5 h-5 text-violet-600" />
                    </div>
                    <div>
                        <p className="font-medium text-gray-900 dark:text-white">{item.name}</p>
                        <p className="text-sm text-gray-500">{item.slug}</p>
                    </div>
                </div>
            ),
        },
        {
            key: 'description',
            header: 'Mô tả',
            render: (item: Category) => (
                <p className="text-gray-600 dark:text-gray-400 truncate max-w-xs">
                    {item.description || '-'}
                </p>
            ),
        },
        {
            key: 'status',
            header: 'Trạng thái',
            render: (item: Category) => {
                const isActive = item.status?.toLowerCase() === 'active';
                return (
                    <span
                        className={`inline-flex px-2.5 py-1 text-xs font-medium rounded-full ${isActive
                            ? 'bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400'
                            : 'bg-gray-100 text-gray-700 dark:bg-gray-700 dark:text-gray-400'
                            }`}
                    >
                        {isActive ? 'Hoạt động' : 'Tạm ẩn'}
                    </span>
                );
            },
        },
        {
            key: 'actions',
            header: 'Thao tác',
            render: (item: Category) => (
                <div className="flex items-center gap-2">
                    <button
                        onClick={(e) => {
                            e.stopPropagation();
                            router.push(`/admin/categories/${item.id}/edit`);
                        }}
                        className="p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 text-blue-600"
                        title="Sửa"
                    >
                        <Pencil className="w-4 h-4" />
                    </button>
                    <button
                        onClick={(e) => {
                            e.stopPropagation();
                            openDeleteModal(item);
                        }}
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
            <AdminHeader title="Danh mục" subtitle={`${categories.length} danh mục`} />

            <div className="p-6 space-y-6">
                {/* Toolbar */}
                <div className="flex flex-col sm:flex-row gap-4 justify-between">
                    <div className="relative">
                        <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
                        <input
                            type="text"
                            placeholder="Tìm kiếm danh mục..."
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                            className="pl-10 pr-4 py-2.5 w-full sm:w-80 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800 focus:ring-2 focus:ring-violet-500 outline-none"
                        />
                    </div>
                    <button
                        onClick={() => router.push('/admin/categories/create')}
                        className="flex items-center gap-2 px-4 py-2.5 bg-violet-600 hover:bg-violet-700 text-white rounded-xl font-medium transition-colors"
                    >
                        <Plus className="w-5 h-5" />
                        Thêm danh mục
                    </button>
                </div>

                {/* Table */}
                <DataTable
                    columns={columns}
                    data={filteredCategories}
                    loading={loading}
                    keyExtractor={(item) => item.id}
                    emptyMessage="Chưa có danh mục nào"
                    onRowClick={(item) => router.push(`/admin/categories/${item.id}/edit`)}
                />
            </div>

            {/* Delete Modal */}
            <ConfirmModal
                isOpen={isDeleteOpen}
                onClose={() => setIsDeleteOpen(false)}
                onConfirm={handleDelete}
                title="Xóa danh mục"
                message={`Bạn có chắc chắn muốn xóa danh mục "${selectedCategory?.name}"? Hành động này không thể hoàn tác.`}
                confirmText="Xóa"
                loading={deleteLoading}
            />
        </div>
    );
}
