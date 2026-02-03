'use client';

import { useEffect, useState } from 'react';
import { Plus, Pencil, Trash2, Search, Building2 } from 'lucide-react';
import AdminHeader from '@/components/admin/AdminHeader';
import DataTable from '@/components/admin/DataTable';
import Modal, { ConfirmModal } from '@/components/admin/Modal';
import ImageUpload from '@/components/admin/ImageUpload';
import { brandsService, CreateBrandDto, UpdateBrandDto } from '@/lib/services/admin/brands-service';
import { Brand } from '@/lib/services/admin/dashboard-service';

export default function BrandsPage() {
    const [brands, setBrands] = useState<Brand[]>([]);
    const [loading, setLoading] = useState(true);
    const [searchQuery, setSearchQuery] = useState('');

    // Modal states
    const [isFormOpen, setIsFormOpen] = useState(false);
    const [isDeleteOpen, setIsDeleteOpen] = useState(false);
    const [selectedBrand, setSelectedBrand] = useState<Brand | null>(null);
    const [formLoading, setFormLoading] = useState(false);

    // Form state
    const [formData, setFormData] = useState<CreateBrandDto>({
        name: '',
        description: '',
        slug: '',
        logo: '',
        status: 'Active',
    });

    useEffect(() => {
        loadBrands();
    }, []);

    const loadBrands = async () => {
        setLoading(true);
        try {
            const data = await brandsService.getAll();
            setBrands(data);
        } catch (error) {
            console.error('Error loading brands:', error);
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
        setSelectedBrand(null);
        setFormData({ name: '', description: '', slug: '', logo: '', status: 'Active' });
        setIsFormOpen(true);
    };

    const openEditModal = (brand: Brand) => {
        setSelectedBrand(brand);
        setFormData({
            name: brand.name,
            description: brand.description,
            slug: brand.slug,
            logo: brand.logo,
            status: brand.status,
        });
        setIsFormOpen(true);
    };

    const openDeleteModal = (brand: Brand) => {
        setSelectedBrand(brand);
        setIsDeleteOpen(true);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setFormLoading(true);

        try {
            if (selectedBrand) {
                await brandsService.update(selectedBrand.id, formData);
            } else {
                await brandsService.create(formData);
            }
            setIsFormOpen(false);
            loadBrands();
        } catch (error) {
            console.error('Error saving brand:', error);
            alert('Có lỗi xảy ra!');
        } finally {
            setFormLoading(false);
        }
    };

    const handleDelete = async () => {
        if (!selectedBrand) return;
        setFormLoading(true);

        try {
            await brandsService.delete(selectedBrand.id);
            setIsDeleteOpen(false);
            loadBrands();
        } catch (error) {
            console.error('Error deleting brand:', error);
            alert('Có lỗi xảy ra!');
        } finally {
            setFormLoading(false);
        }
    };

    const filteredBrands = brands.filter(
        (b) =>
            b.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
            b.description.toLowerCase().includes(searchQuery.toLowerCase())
    );

    const columns = [
        {
            key: 'id',
            header: 'ID',
            width: '80px',
        },
        {
            key: 'name',
            header: 'Thương hiệu',
            render: (item: Brand) => (
                <div className="flex items-center gap-3">
                    {item.logo ? (
                        <img
                            src={item.logo}
                            alt={item.name}
                            className="w-10 h-10 rounded-xl object-cover bg-gray-100"
                        />
                    ) : (
                        <div className="w-10 h-10 rounded-xl bg-blue-100 dark:bg-blue-900/30 flex items-center justify-center">
                            <Building2 className="w-5 h-5 text-blue-600" />
                        </div>
                    )}
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
            render: (item: Brand) => (
                <p className="text-gray-600 dark:text-gray-400 truncate max-w-xs">
                    {item.description || '-'}
                </p>
            ),
        },
        {
            key: 'status',
            header: 'Trạng thái',
            render: (item: Brand) => {
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
            render: (item: Brand) => (
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
            <AdminHeader title="Thương hiệu" subtitle={`${brands.length} thương hiệu`} />

            <div className="p-6 space-y-6">
                {/* Toolbar */}
                <div className="flex flex-col sm:flex-row gap-4 justify-between">
                    <div className="relative">
                        <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
                        <input
                            type="text"
                            placeholder="Tìm kiếm thương hiệu..."
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
                        Thêm thương hiệu
                    </button>
                </div>

                {/* Table */}
                <DataTable
                    columns={columns}
                    data={filteredBrands}
                    loading={loading}
                    keyExtractor={(item) => item.id}
                    emptyMessage="Chưa có thương hiệu nào"
                />
            </div>

            {/* Create/Edit Modal */}
            <Modal
                isOpen={isFormOpen}
                onClose={() => setIsFormOpen(false)}
                title={selectedBrand ? 'Sửa thương hiệu' : 'Thêm thương hiệu mới'}
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
                            {selectedBrand ? 'Cập nhật' : 'Tạo mới'}
                        </button>
                    </div>
                }
            >
                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                            Tên thương hiệu <span className="text-red-500">*</span>
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
                            placeholder="Nhập tên thương hiệu"
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
                            placeholder="slug-thuong-hieu"
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
                            Logo thương hiệu
                        </label>
                        <ImageUpload
                            value={formData.logo}
                            onChange={(url) => setFormData({ ...formData, logo: url })}
                            type="brand"
                            placeholder="Upload logo thương hiệu"
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
                title="Xóa thương hiệu"
                message={`Bạn có chắc chắn muốn xóa thương hiệu "${selectedBrand?.name}"?`}
                confirmText="Xóa"
                loading={formLoading}
            />
        </div>
    );
}
