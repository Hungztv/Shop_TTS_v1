'use client';

import { useState, useEffect, useCallback } from 'react';
import {
    Package, Plus, Edit3, Trash2, Loader2, Search,
    AlertCircle, ChevronLeft, ChevronRight, X, Upload, ImageIcon,
} from 'lucide-react';
import { toast } from 'sonner';
import { sellerProductsService } from '@/lib/services/seller/shop-products-service';
import { brandsService } from '@/lib/services/admin/brands-service';
import { categoriesService } from '@/lib/services/admin/categories-service';
import { uploadService } from '@/lib/services/admin/upload-service';
import type { Product, Brand, Category } from '@/lib/services/admin/dashboard-service';

interface ProductFormData {
    name: string;
    slug: string;
    description: string;
    price: number;
    capitalPrice: number;
    quantity: number;
    image: string;
    brandId: number;
    categoryId: number;
}

const emptyForm: ProductFormData = {
    name: '',
    slug: '',
    description: '',
    price: 0,
    capitalPrice: 0,
    quantity: 0,
    image: '',
    brandId: 0,
    categoryId: 0,
};

export default function SellerProductsPage() {
    // Data
    const [products, setProducts] = useState<Product[]>([]);
    const [brands, setBrands] = useState<Brand[]>([]);
    const [categories, setCategories] = useState<Category[]>([]);

    // Pagination
    const [page, setPage] = useState(1);
    const [pageSize] = useState(10);
    const [totalPages, setTotalPages] = useState(0);
    const [totalCount, setTotalCount] = useState(0);

    // UI state
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    // Modal state
    const [modalOpen, setModalOpen] = useState(false);
    const [editingProduct, setEditingProduct] = useState<Product | null>(null);
    const [form, setForm] = useState<ProductFormData>(emptyForm);
    const [submitting, setSubmitting] = useState(false);
    const [uploadingImage, setUploadingImage] = useState(false);

    // Delete confirm
    const [deleteTarget, setDeleteTarget] = useState<Product | null>(null);
    const [deleting, setDeleting] = useState(false);

    const fetchProducts = useCallback(async () => {
        try {
            setLoading(true);
            setError(null);
            const res = await sellerProductsService.getMyProducts(page, pageSize);
            setProducts(res.items);
            setTotalPages(res.totalPages);
            setTotalCount(res.totalCount);
        } catch {
            setError('Không thể tải danh sách sản phẩm.');
        } finally {
            setLoading(false);
        }
    }, [page, pageSize]);

    const fetchMeta = useCallback(async () => {
        const [b, c] = await Promise.all([
            brandsService.getAll(),
            categoriesService.getAll(),
        ]);
        setBrands(b);
        setCategories(c);
    }, []);

    useEffect(() => {
        fetchProducts();
    }, [fetchProducts]);

    useEffect(() => {
        fetchMeta();
    }, [fetchMeta]);

    // Helpers
    const generateSlug = (name: string) =>
        name
            .toLowerCase()
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '')
            .replace(/đ/g, 'd')
            .replace(/[^a-z0-9]+/g, '-')
            .replace(/(^-|-$)/g, '');

    const formatPrice = (p: number) =>
        new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(p);

    // Modal
    const openCreate = () => {
        setEditingProduct(null);
        setForm(emptyForm);
        setModalOpen(true);
    };

    const openEdit = (product: Product) => {
        setEditingProduct(product);
        setForm({
            name: product.name,
            slug: product.slug,
            description: product.description || '',
            price: product.price,
            capitalPrice: product.capitalPrice,
            quantity: product.quantity,
            image: product.image || '',
            brandId: product.brandId,
            categoryId: product.categoryId,
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setEditingProduct(null);
        setForm(emptyForm);
    };

    const handleImageUpload = async (file: File) => {
        try {
            setUploadingImage(true);
            const result = await uploadService.uploadProduct(file);
            if (result.success && result.data?.url) {
                setForm(prev => ({ ...prev, image: result.data!.url }));
                toast.success('Upload ảnh thành công!');
            } else {
                toast.error('Upload ảnh thất bại');
            }
        } catch {
            toast.error('Upload ảnh thất bại');
        } finally {
            setUploadingImage(false);
        }
    };

    const handleSubmit = async () => {
        // Validate
        if (!form.name.trim()) { toast.error('Tên sản phẩm không được để trống'); return; }
        if (!form.slug.trim()) { toast.error('Slug không được để trống'); return; }
        if (form.price <= 0) { toast.error('Giá bán phải lớn hơn 0'); return; }
        if (form.capitalPrice < 0) { toast.error('Giá vốn không hợp lệ'); return; }
        if (form.quantity < 0) { toast.error('Số lượng không hợp lệ'); return; }
        if (!form.categoryId) { toast.error('Vui lòng chọn danh mục'); return; }
        if (!form.brandId) { toast.error('Vui lòng chọn thương hiệu'); return; }

        try {
            setSubmitting(true);
            if (editingProduct) {
                await sellerProductsService.updateProduct(editingProduct.id, form);
                toast.success('Cập nhật sản phẩm thành công!');
            } else {
                await sellerProductsService.createProduct(form);
                toast.success('Tạo sản phẩm mới thành công!');
            }
            closeModal();
            fetchProducts();
        } catch {
            toast.error(editingProduct ? 'Cập nhật thất bại' : 'Tạo sản phẩm thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!deleteTarget) return;
        try {
            setDeleting(true);
            await sellerProductsService.deleteProduct(deleteTarget.id);
            toast.success('Xóa sản phẩm thành công!');
            setDeleteTarget(null);
            fetchProducts();
        } catch {
            toast.error('Xóa sản phẩm thất bại');
        } finally {
            setDeleting(false);
        }
    };

    // Error state
    if (error && !loading) {
        return (
            <div className="p-6 lg:p-8">
                <div className="flex flex-col items-center justify-center py-20">
                    <AlertCircle className="w-16 h-16 text-red-400 mb-4" />
                    <h2 className="text-xl font-semibold text-gray-900 dark:text-white mb-2">Lỗi</h2>
                    <p className="text-gray-500 mb-4">{error}</p>
                    <button onClick={fetchProducts} className="btn-primary text-sm !py-2 !px-4">
                        Thử lại
                    </button>
                </div>
            </div>
        );
    }

    return (
        <div className="p-6 lg:p-8">
            {/* Header */}
            <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 mb-8">
                <div>
                    <h1 className="text-2xl lg:text-3xl font-bold text-gray-900 dark:text-white flex items-center gap-3">
                        <Package className="w-8 h-8 text-emerald-500" />
                        Sản phẩm của Shop
                    </h1>
                    <p className="text-gray-500 mt-1">
                        {totalCount > 0 ? `${totalCount} sản phẩm` : 'Quản lý sản phẩm cửa hàng'}
                    </p>
                </div>
                <button
                    onClick={openCreate}
                    className="flex items-center gap-2 px-5 py-2.5 rounded-xl bg-emerald-500 hover:bg-emerald-600 text-white font-medium transition-all hover:shadow-lg self-start"
                >
                    <Plus className="w-4 h-4" />
                    Thêm sản phẩm
                </button>
            </div>

            {/* Table */}
            <div className="bg-white dark:bg-gray-800 rounded-2xl border border-gray-200 dark:border-gray-700 overflow-hidden">
                {loading ? (
                    <div className="p-8">
                        {[...Array(5)].map((_, i) => (
                            <div key={i} className="flex items-center gap-4 py-4 border-b border-gray-100 dark:border-gray-700 last:border-b-0">
                                <div className="skeleton w-14 h-14 rounded-lg flex-shrink-0"></div>
                                <div className="flex-1 space-y-2">
                                    <div className="skeleton h-4 w-1/3"></div>
                                    <div className="skeleton h-3 w-1/4"></div>
                                </div>
                                <div className="skeleton h-4 w-24"></div>
                            </div>
                        ))}
                    </div>
                ) : products.length === 0 ? (
                    <div className="flex flex-col items-center justify-center py-16">
                        <Package className="w-16 h-16 text-gray-300 dark:text-gray-600 mb-4" />
                        <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-1">
                            Chưa có sản phẩm nào
                        </h3>
                        <p className="text-gray-500 mb-4">Hãy thêm sản phẩm đầu tiên cho shop của bạn</p>
                        <button
                            onClick={openCreate}
                            className="flex items-center gap-2 px-4 py-2 rounded-xl bg-emerald-500 hover:bg-emerald-600 text-white font-medium transition-all"
                        >
                            <Plus className="w-4 h-4" /> Thêm sản phẩm
                        </button>
                    </div>
                ) : (
                    <>
                        <div className="overflow-x-auto">
                            <table className="w-full">
                                <thead>
                                    <tr className="border-b border-gray-200 dark:border-gray-700 bg-gray-50 dark:bg-gray-800/50">
                                        <th className="text-left px-6 py-3.5 text-xs font-semibold text-gray-500 uppercase tracking-wider">Sản phẩm</th>
                                        <th className="text-right px-6 py-3.5 text-xs font-semibold text-gray-500 uppercase tracking-wider">Giá bán</th>
                                        <th className="text-right px-6 py-3.5 text-xs font-semibold text-gray-500 uppercase tracking-wider hidden md:table-cell">Giá vốn</th>
                                        <th className="text-right px-6 py-3.5 text-xs font-semibold text-gray-500 uppercase tracking-wider hidden sm:table-cell">Tồn kho</th>
                                        <th className="text-right px-6 py-3.5 text-xs font-semibold text-gray-500 uppercase tracking-wider hidden lg:table-cell">Đã bán</th>
                                        <th className="text-center px-6 py-3.5 text-xs font-semibold text-gray-500 uppercase tracking-wider">Thao tác</th>
                                    </tr>
                                </thead>
                                <tbody className="divide-y divide-gray-100 dark:divide-gray-700">
                                    {products.map((product) => (
                                        <tr key={product.id} className="hover:bg-gray-50 dark:hover:bg-gray-700/30 transition-colors">
                                            <td className="px-6 py-4">
                                                <div className="flex items-center gap-3">
                                                    <div className="w-12 h-12 rounded-lg overflow-hidden bg-gray-100 dark:bg-gray-700 flex-shrink-0">
                                                        {product.image ? (
                                                            <img src={product.image} alt={product.name} className="w-full h-full object-cover" />
                                                        ) : (
                                                            <div className="w-full h-full flex items-center justify-center">
                                                                <ImageIcon className="w-5 h-5 text-gray-400" />
                                                            </div>
                                                        )}
                                                    </div>
                                                    <div className="min-w-0">
                                                        <p className="font-medium text-gray-900 dark:text-white truncate max-w-[200px]">{product.name}</p>
                                                        <p className="text-xs text-gray-400 truncate max-w-[200px]">/{product.slug}</p>
                                                    </div>
                                                </div>
                                            </td>
                                            <td className="px-6 py-4 text-right font-semibold text-emerald-600 dark:text-emerald-400 whitespace-nowrap">
                                                {formatPrice(product.price)}
                                            </td>
                                            <td className="px-6 py-4 text-right text-gray-500 whitespace-nowrap hidden md:table-cell">
                                                {formatPrice(product.capitalPrice)}
                                            </td>
                                            <td className="px-6 py-4 text-right text-gray-700 dark:text-gray-300 font-medium hidden sm:table-cell">
                                                {product.quantity}
                                            </td>
                                            <td className="px-6 py-4 text-right text-gray-500 hidden lg:table-cell">
                                                {product.soldOut}
                                            </td>
                                            <td className="px-6 py-4">
                                                <div className="flex items-center justify-center gap-1">
                                                    <button
                                                        onClick={() => openEdit(product)}
                                                        className="p-2 rounded-lg text-gray-500 hover:text-emerald-600 hover:bg-emerald-50 dark:hover:bg-emerald-900/20 transition-colors"
                                                        title="Chỉnh sửa"
                                                    >
                                                        <Edit3 className="w-4 h-4" />
                                                    </button>
                                                    <button
                                                        onClick={() => setDeleteTarget(product)}
                                                        className="p-2 rounded-lg text-gray-500 hover:text-red-600 hover:bg-red-50 dark:hover:bg-red-900/20 transition-colors"
                                                        title="Xóa"
                                                    >
                                                        <Trash2 className="w-4 h-4" />
                                                    </button>
                                                </div>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>

                        {/* Pagination */}
                        {totalPages > 1 && (
                            <div className="flex items-center justify-between px-6 py-4 border-t border-gray-200 dark:border-gray-700">
                                <p className="text-sm text-gray-500">
                                    Trang {page} / {totalPages} ({totalCount} sản phẩm)
                                </p>
                                <div className="flex items-center gap-2">
                                    <button
                                        onClick={() => setPage(p => Math.max(1, p - 1))}
                                        disabled={page <= 1}
                                        className="flex items-center gap-1 px-3 py-1.5 rounded-lg border border-gray-300 dark:border-gray-600 text-sm font-medium disabled:opacity-40 hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors"
                                    >
                                        <ChevronLeft className="w-4 h-4" /> Trước
                                    </button>
                                    <button
                                        onClick={() => setPage(p => Math.min(totalPages, p + 1))}
                                        disabled={page >= totalPages}
                                        className="flex items-center gap-1 px-3 py-1.5 rounded-lg border border-gray-300 dark:border-gray-600 text-sm font-medium disabled:opacity-40 hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors"
                                    >
                                        Sau <ChevronRight className="w-4 h-4" />
                                    </button>
                                </div>
                            </div>
                        )}
                    </>
                )}
            </div>

            {/* ===== CREATE / EDIT MODAL ===== */}
            {modalOpen && (
                <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
                    {/* Overlay */}
                    <div className="absolute inset-0 bg-black/50 backdrop-blur-sm" onClick={closeModal}></div>

                    {/* Modal */}
                    <div className="relative bg-white dark:bg-gray-800 rounded-2xl w-full max-w-2xl max-h-[90vh] overflow-y-auto shadow-2xl border border-gray-200 dark:border-gray-700">
                        {/* Modal Header */}
                        <div className="sticky top-0 z-10 flex items-center justify-between px-6 py-4 border-b border-gray-200 dark:border-gray-700 bg-white dark:bg-gray-800 rounded-t-2xl">
                            <h2 className="text-lg font-bold text-gray-900 dark:text-white">
                                {editingProduct ? 'Chỉnh sửa sản phẩm' : 'Thêm sản phẩm mới'}
                            </h2>
                            <button onClick={closeModal} className="p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors">
                                <X className="w-5 h-5" />
                            </button>
                        </div>

                        {/* Modal Body */}
                        <div className="p-6 space-y-5">
                            {/* Name + Slug */}
                            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                                <div>
                                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5">
                                        Tên sản phẩm <span className="text-red-500">*</span>
                                    </label>
                                    <input
                                        type="text"
                                        value={form.name}
                                        onChange={(e) => {
                                            const name = e.target.value;
                                            setForm(prev => ({
                                                ...prev,
                                                name,
                                                slug: !editingProduct ? generateSlug(name) : prev.slug,
                                            }));
                                        }}
                                        className="w-full px-4 py-2.5 rounded-xl border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none transition-all"
                                        placeholder="Tên sản phẩm"
                                    />
                                </div>
                                <div>
                                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5">
                                        Slug <span className="text-red-500">*</span>
                                    </label>
                                    <input
                                        type="text"
                                        value={form.slug}
                                        onChange={(e) => setForm(prev => ({ ...prev, slug: e.target.value }))}
                                        className="w-full px-4 py-2.5 rounded-xl border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none transition-all"
                                        placeholder="ten-san-pham"
                                    />
                                </div>
                            </div>

                            {/* Description */}
                            <div>
                                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5">
                                    Mô tả
                                </label>
                                <textarea
                                    value={form.description}
                                    onChange={(e) => setForm(prev => ({ ...prev, description: e.target.value }))}
                                    rows={3}
                                    className="w-full px-4 py-2.5 rounded-xl border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none transition-all resize-none"
                                    placeholder="Mô tả sản phẩm..."
                                />
                            </div>

                            {/* Price + Capital + Quantity */}
                            <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
                                <div>
                                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5">
                                        Giá bán (VNĐ) <span className="text-red-500">*</span>
                                    </label>
                                    <input
                                        type="number"
                                        value={form.price || ''}
                                        onChange={(e) => setForm(prev => ({ ...prev, price: Number(e.target.value) }))}
                                        className="w-full px-4 py-2.5 rounded-xl border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none transition-all"
                                        placeholder="0"
                                        min="0"
                                    />
                                </div>
                                <div>
                                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5">
                                        Giá vốn (VNĐ) <span className="text-red-500">*</span>
                                    </label>
                                    <input
                                        type="number"
                                        value={form.capitalPrice || ''}
                                        onChange={(e) => setForm(prev => ({ ...prev, capitalPrice: Number(e.target.value) }))}
                                        className="w-full px-4 py-2.5 rounded-xl border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none transition-all"
                                        placeholder="0"
                                        min="0"
                                    />
                                </div>
                                <div>
                                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5">
                                        Số lượng <span className="text-red-500">*</span>
                                    </label>
                                    <input
                                        type="number"
                                        value={form.quantity || ''}
                                        onChange={(e) => setForm(prev => ({ ...prev, quantity: Number(e.target.value) }))}
                                        className="w-full px-4 py-2.5 rounded-xl border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none transition-all"
                                        placeholder="0"
                                        min="0"
                                    />
                                </div>
                            </div>

                            {/* Category + Brand */}
                            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                                <div>
                                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5">
                                        Danh mục <span className="text-red-500">*</span>
                                    </label>
                                    <select
                                        value={form.categoryId}
                                        onChange={(e) => setForm(prev => ({ ...prev, categoryId: Number(e.target.value) }))}
                                        className="w-full px-4 py-2.5 rounded-xl border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none transition-all"
                                    >
                                        <option value={0}>-- Chọn danh mục --</option>
                                        {categories.map(c => (
                                            <option key={c.id} value={c.id}>{c.name}</option>
                                        ))}
                                    </select>
                                </div>
                                <div>
                                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5">
                                        Thương hiệu <span className="text-red-500">*</span>
                                    </label>
                                    <select
                                        value={form.brandId}
                                        onChange={(e) => setForm(prev => ({ ...prev, brandId: Number(e.target.value) }))}
                                        className="w-full px-4 py-2.5 rounded-xl border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none transition-all"
                                    >
                                        <option value={0}>-- Chọn thương hiệu --</option>
                                        {brands.map(b => (
                                            <option key={b.id} value={b.id}>{b.name}</option>
                                        ))}
                                    </select>
                                </div>
                            </div>

                            {/* Image */}
                            <div>
                                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5">
                                    Hình ảnh
                                </label>
                                <div className="flex items-start gap-4">
                                    <div className="w-24 h-24 rounded-xl border-2 border-dashed border-gray-300 dark:border-gray-600 overflow-hidden flex-shrink-0 bg-gray-50 dark:bg-gray-700">
                                        {form.image ? (
                                            <img src={form.image} alt="Preview" className="w-full h-full object-cover" />
                                        ) : (
                                            <div className="w-full h-full flex items-center justify-center">
                                                <ImageIcon className="w-8 h-8 text-gray-300 dark:text-gray-500" />
                                            </div>
                                        )}
                                    </div>
                                    <div className="flex-1 space-y-2">
                                        <label className="flex items-center gap-2 px-4 py-2 rounded-xl border border-gray-300 dark:border-gray-600 hover:bg-gray-50 dark:hover:bg-gray-700 cursor-pointer transition-colors w-fit">
                                            {uploadingImage ? <Loader2 className="w-4 h-4 animate-spin" /> : <Upload className="w-4 h-4" />}
                                            <span className="text-sm font-medium">Chọn ảnh</span>
                                            <input
                                                type="file"
                                                accept="image/*"
                                                className="hidden"
                                                onChange={(e) => {
                                                    const file = e.target.files?.[0];
                                                    if (file) handleImageUpload(file);
                                                }}
                                            />
                                        </label>
                                        <p className="text-xs text-gray-400">Hoặc nhập URL trực tiếp:</p>
                                        <input
                                            type="text"
                                            value={form.image}
                                            onChange={(e) => setForm(prev => ({ ...prev, image: e.target.value }))}
                                            className="w-full px-3 py-2 rounded-lg border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white text-sm focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none transition-all"
                                            placeholder="https://..."
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>

                        {/* Modal Footer */}
                        <div className="sticky bottom-0 flex items-center justify-end gap-3 px-6 py-4 border-t border-gray-200 dark:border-gray-700 bg-gray-50 dark:bg-gray-800/50 rounded-b-2xl">
                            <button
                                onClick={closeModal}
                                className="px-4 py-2.5 rounded-xl border border-gray-300 dark:border-gray-600 text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 font-medium transition-all"
                            >
                                Hủy
                            </button>
                            <button
                                onClick={handleSubmit}
                                disabled={submitting}
                                className="flex items-center gap-2 px-5 py-2.5 rounded-xl bg-emerald-500 hover:bg-emerald-600 text-white font-medium transition-all disabled:opacity-50"
                            >
                                {submitting ? <Loader2 className="w-4 h-4 animate-spin" /> : editingProduct ? <Edit3 className="w-4 h-4" /> : <Plus className="w-4 h-4" />}
                                {editingProduct ? 'Cập nhật' : 'Tạo mới'}
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {/* ===== DELETE CONFIRM MODAL ===== */}
            {deleteTarget && (
                <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
                    <div className="absolute inset-0 bg-black/50 backdrop-blur-sm" onClick={() => setDeleteTarget(null)}></div>
                    <div className="relative bg-white dark:bg-gray-800 rounded-2xl w-full max-w-md p-6 shadow-2xl border border-gray-200 dark:border-gray-700">
                        <div className="flex flex-col items-center text-center">
                            <div className="w-14 h-14 rounded-full bg-red-100 dark:bg-red-900/30 flex items-center justify-center mb-4">
                                <Trash2 className="w-6 h-6 text-red-500" />
                            </div>
                            <h3 className="text-lg font-bold text-gray-900 dark:text-white mb-2">
                                Xác nhận xóa?
                            </h3>
                            <p className="text-gray-500 text-sm mb-6">
                                Bạn có chắc muốn xóa sản phẩm &quot;<span className="font-medium text-gray-700 dark:text-gray-300">{deleteTarget.name}</span>&quot;? Hành động này không thể hoàn tác.
                            </p>
                            <div className="flex items-center gap-3 w-full">
                                <button
                                    onClick={() => setDeleteTarget(null)}
                                    className="flex-1 px-4 py-2.5 rounded-xl border border-gray-300 dark:border-gray-600 text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 font-medium transition-all"
                                >
                                    Hủy
                                </button>
                                <button
                                    onClick={handleDelete}
                                    disabled={deleting}
                                    className="flex-1 flex items-center justify-center gap-2 px-4 py-2.5 rounded-xl bg-red-500 hover:bg-red-600 text-white font-medium transition-all disabled:opacity-50"
                                >
                                    {deleting ? <Loader2 className="w-4 h-4 animate-spin" /> : <Trash2 className="w-4 h-4" />}
                                    Xóa
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}
