'use client';

import { useEffect, useState } from 'react';
import { Plus, Pencil, Trash2, Search, FolderTree } from 'lucide-react';
import AdminHeader from '@/components/admin/AdminHeader';
import DataTable from '@/components/admin/DataTable';
import Modal, { ConfirmModal } from '@/components/admin/Modal';
import { categoriesService, CreateCategoryDto, UpdateCategoryDto } from '@/lib/services/admin/categories-service';
import { Category } from '@/lib/services/admin/dashboard-service';

export default function CategoriesPage() {
    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState(true);
    const [searchQuery, setSearchQuery] = useState('');

    // Modal states
    const [isFormOpen, setIsFormOpen] = useState(false);
    const [isDeleteOpen, setIsDeleteOpen] = useState(false);
    const [selectedCategory, setSelectedCategory] = useState<Category | null>(null);
    const [formLoading, setFormLoading] = useState(false);

    // Form state
    const [formData, setFormData] = useState<CreateCategoryDto>({
        name: '',
        description: '',
        slug: '',
        status: 'Active',
    });

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

    const generateSlug = (name: string) => {
        return name
            .toLowerCase()
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '')
            .replace(/đ/g, 'd')
            .replace(/[^a-z0-9\s-]/g, '')
            .trim()
            .replace(/\s+/g, '-');
    };

    const openCreateModal = () => {
        setSelectedCategory(null);
        setFormData({ name: '', description: '', slug: '', status: 'Active' });
        setIsFormOpen(true);
    };

    const openEditModal = (category: Category) => {
        setSelectedCategory(category);
        setFormData({
            name: category.name,
            description: category.description,
            slug: category.slug,
            status: category.status,
        });
        setIsFormOpen(true);
    };

    const openDeleteModal = (category: Category) => {
        setSelectedCategory(category);
        setIsDeleteOpen(true);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setFormLoading(true);

        try {
            if (selectedCategory) {
                await categoriesService.update(selectedCategory.id, formData);
            } else {
                await categoriesService.create(formData);
            }
            setIsFormOpen(false);
            loadCategories();
        } catch (error) {
            console.error('Error saving category:', error);
            alert('Có lỗi xảy ra!');
        } finally {
            setFormLoading(false);
        }
    };

    const handleDelete = async () => {
        if (!selectedCategory) return;
        setFormLoading(true);

        try {
            await categoriesService.delete(selectedCategory.id);
            setIsDeleteOpen(false);
            loadCategories();
        } catch (error) {
            console.error('Error deleting category:', error);
            alert('Có lỗi xảy ra!');
        } finally {
            setFormLoading(false);
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
                            openEditModal(item);
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
                        onClick={openCreateModal}
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
                />
            </div>

            {/* Create/Edit Modal */}
            <Modal
                isOpen={isFormOpen}
                onClose={() => setIsFormOpen(false)}
                title={selectedCategory ? 'Sửa danh mục' : 'Thêm danh mục mới'}
                footer={
                    <div className="flex justify-end gap-3">
                        <button
                            onClick={() => setIsFormOpen(false)}
                            className="px-4 py-2 text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 rounded-xl"
                        >
                            Hủy
                        </button>
                        <button
                            onClick={handleSubmit}
                            disabled={formLoading || !formData.name}
                            className="px-4 py-2 bg-violet-600 hover:bg-violet-700 text-white rounded-xl font-medium disabled:opacity-50 flex items-center gap-2"
                        >
                            {formLoading && (
                                <div className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin" />
                            )}
                            {selectedCategory ? 'Cập nhật' : 'Tạo mới'}
                        </button>
                    </div>
                }
            >
                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                            Tên danh mục <span className="text-red-500">*</span>
                        </label>
                        <input
                            type="text"
                            value={formData.name}
                            onChange={(e) => {
                                const name = e.target.value;
                                setFormData({
                                    ...formData,
                                    name,
                                    slug: generateSlug(name)
                                });
                            }}
                            className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800 focus:ring-2 focus:ring-violet-500 outline-none"
                            placeholder="Nhập tên danh mục"
                            required
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                            Slug <span className="text-red-500">*</span>
                        </label>
                        <input
                            type="text"
                            value={formData.slug}
                            onChange={(e) => setFormData({ ...formData, slug: e.target.value })}
                            className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800 focus:ring-2 focus:ring-violet-500 outline-none"
                            placeholder="slug-danh-muc"
                            required
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                            Mô tả
                        </label>
                        <textarea
                            value={formData.description}
                            onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                            className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800 focus:ring-2 focus:ring-violet-500 outline-none resize-none"
                            rows={3}
                            placeholder="Nhập mô tả"
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                            Trạng thái
                        </label>
                        <select
                            value={formData.status}
                            onChange={(e) => setFormData({ ...formData, status: e.target.value })}
                            className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800 focus:ring-2 focus:ring-violet-500 outline-none"
                        >
                            <option value="Active">Hoạt động</option>
                            <option value="Inactive">Tạm ẩn</option>
                        </select>
                    </div>
                </form>
            </Modal>

            {/* Delete Modal */}
            <ConfirmModal
                isOpen={isDeleteOpen}
                onClose={() => setIsDeleteOpen(false)}
                onConfirm={handleDelete}
                title="Xóa danh mục"
                message={`Bạn có chắc chắn muốn xóa danh mục "${selectedCategory?.name}"? Hành động này không thể hoàn tác.`}
                confirmText="Xóa"
                loading={formLoading}
            />
        </div>
    );
}
