'use client';

import { useState, useEffect, useCallback } from 'react';
import {
    Store, Edit3, Save, X, Loader2, ImageIcon,
    AlertCircle, CheckCircle2, Upload,
} from 'lucide-react';
import { toast } from 'sonner';
import { sellerShopService } from '@/lib/services/seller/shop-service';
import { uploadService } from '@/lib/services/admin/upload-service';
import type { ShopDto } from '@/types/shop';

export default function SellerShopPage() {
    const [shop, setShop] = useState<ShopDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [editing, setEditing] = useState(false);
    const [error, setError] = useState<string | null>(null);

    // Form state
    const [form, setForm] = useState({
        name: '',
        slug: '',
        description: '',
        logoUrl: '',
        coverUrl: '',
    });

    const [uploadingLogo, setUploadingLogo] = useState(false);
    const [uploadingCover, setUploadingCover] = useState(false);

    const fetchShop = useCallback(async () => {
        try {
            setLoading(true);
            setError(null);
            const data = await sellerShopService.getMyShop();
            setShop(data);
            if (data) {
                setForm({
                    name: data.name || '',
                    slug: data.slug || '',
                    description: data.description || '',
                    logoUrl: data.logoUrl || '',
                    coverUrl: data.coverUrl || '',
                });
            }
        } catch {
            setError('Không thể tải thông tin shop. Vui lòng thử lại.');
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchShop();
    }, [fetchShop]);

    const handleSave = async () => {
        if (!shop) return;
        if (!form.name.trim()) {
            toast.error('Tên shop không được để trống');
            return;
        }
        if (!form.slug.trim()) {
            toast.error('Slug không được để trống');
            return;
        }

        try {
            setSaving(true);
            const updated = await sellerShopService.updateShop(shop.id, {
                name: form.name,
                slug: form.slug,
                description: form.description || undefined,
                logoUrl: form.logoUrl || undefined,
                coverUrl: form.coverUrl || undefined,
            });
            setShop(updated);
            setEditing(false);
            toast.success('Cập nhật shop thành công!');
        } catch {
            toast.error('Cập nhật thất bại. Vui lòng thử lại.');
        } finally {
            setSaving(false);
        }
    };

    const handleCancel = () => {
        if (shop) {
            setForm({
                name: shop.name || '',
                slug: shop.slug || '',
                description: shop.description || '',
                logoUrl: shop.logoUrl || '',
                coverUrl: shop.coverUrl || '',
            });
        }
        setEditing(false);
    };

    const handleImageUpload = async (
        file: File,
        type: 'logo' | 'cover'
    ) => {
        const setter = type === 'logo' ? setUploadingLogo : setUploadingCover;
        try {
            setter(true);
            const result = await uploadService.uploadProduct(file);
            if (result.success && result.data?.url) {
                setForm(prev => ({
                    ...prev,
                    [type === 'logo' ? 'logoUrl' : 'coverUrl']: result.data!.url,
                }));
                toast.success(`Upload ${type === 'logo' ? 'logo' : 'ảnh bìa'} thành công!`);
            } else {
                toast.error('Upload thất bại');
            }
        } catch {
            toast.error('Upload thất bại');
        } finally {
            setter(false);
        }
    };

    const statusColor = (status: string) => {
        switch (status) {
            case 'Active': return 'bg-emerald-100 text-emerald-700 dark:bg-emerald-900/30 dark:text-emerald-400';
            case 'Inactive': return 'bg-gray-100 text-gray-600 dark:bg-gray-800 dark:text-gray-400';
            case 'Suspended': return 'bg-red-100 text-red-700 dark:bg-red-900/30 dark:text-red-400';
            default: return 'bg-gray-100 text-gray-600';
        }
    };

    // Loading skeleton
    if (loading) {
        return (
            <div className="p-6 lg:p-8">
                <div className="mb-8">
                    <div className="skeleton h-8 w-48 mb-2"></div>
                    <div className="skeleton h-4 w-72"></div>
                </div>
                <div className="bg-white dark:bg-gray-800 rounded-2xl p-8 border border-gray-200 dark:border-gray-700">
                    <div className="skeleton h-40 w-full mb-6 rounded-xl"></div>
                    <div className="space-y-4">
                        <div className="skeleton h-6 w-1/3"></div>
                        <div className="skeleton h-10 w-full"></div>
                        <div className="skeleton h-6 w-1/3"></div>
                        <div className="skeleton h-10 w-full"></div>
                    </div>
                </div>
            </div>
        );
    }

    // Error state
    if (error) {
        return (
            <div className="p-6 lg:p-8">
                <div className="flex flex-col items-center justify-center py-20">
                    <AlertCircle className="w-16 h-16 text-red-400 mb-4" />
                    <h2 className="text-xl font-semibold text-gray-900 dark:text-white mb-2">Lỗi</h2>
                    <p className="text-gray-500 mb-4">{error}</p>
                    <button onClick={fetchShop} className="btn-primary text-sm !py-2 !px-4">
                        Thử lại
                    </button>
                </div>
            </div>
        );
    }

    // No shop
    if (!shop) {
        return (
            <div className="p-6 lg:p-8">
                <div className="flex flex-col items-center justify-center py-20">
                    <Store className="w-16 h-16 text-gray-300 dark:text-gray-600 mb-4" />
                    <h2 className="text-xl font-semibold text-gray-900 dark:text-white mb-2">
                        Chưa có Shop
                    </h2>
                    <p className="text-gray-500">
                        Bạn chưa có shop nào. Hãy đăng ký kinh doanh trước.
                    </p>
                </div>
            </div>
        );
    }

    return (
        <div className="p-6 lg:p-8 max-w-4xl">
            {/* Header */}
            <div className="flex items-center justify-between mb-8">
                <div>
                    <h1 className="text-2xl lg:text-3xl font-bold text-gray-900 dark:text-white flex items-center gap-3">
                        <Store className="w-8 h-8 text-emerald-500" />
                        Thông tin Shop
                    </h1>
                    <p className="text-gray-500 mt-1">Quản lý thông tin cửa hàng của bạn</p>
                </div>
                {!editing ? (
                    <button
                        onClick={() => setEditing(true)}
                        className="flex items-center gap-2 px-4 py-2.5 rounded-xl bg-emerald-500 hover:bg-emerald-600 text-white font-medium transition-all hover:shadow-lg"
                    >
                        <Edit3 className="w-4 h-4" />
                        Chỉnh sửa
                    </button>
                ) : (
                    <div className="flex items-center gap-2">
                        <button
                            onClick={handleCancel}
                            className="flex items-center gap-2 px-4 py-2.5 rounded-xl border border-gray-300 dark:border-gray-600 text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-800 font-medium transition-all"
                        >
                            <X className="w-4 h-4" />
                            Hủy
                        </button>
                        <button
                            onClick={handleSave}
                            disabled={saving}
                            className="flex items-center gap-2 px-4 py-2.5 rounded-xl bg-emerald-500 hover:bg-emerald-600 text-white font-medium transition-all hover:shadow-lg disabled:opacity-50"
                        >
                            {saving ? <Loader2 className="w-4 h-4 animate-spin" /> : <Save className="w-4 h-4" />}
                            Lưu
                        </button>
                    </div>
                )}
            </div>

            {/* Cover Image */}
            <div className="bg-white dark:bg-gray-800 rounded-2xl border border-gray-200 dark:border-gray-700 overflow-hidden">
                <div className="relative h-48 bg-gradient-to-r from-emerald-400 to-teal-500 overflow-hidden">
                    {(editing ? form.coverUrl : shop.coverUrl) ? (
                        <img
                            src={editing ? form.coverUrl : (shop.coverUrl || '')}
                            alt="Cover"
                            className="w-full h-full object-cover"
                        />
                    ) : (
                        <div className="w-full h-full flex items-center justify-center">
                            <ImageIcon className="w-12 h-12 text-white/50" />
                        </div>
                    )}
                    {editing && (
                        <label className="absolute bottom-3 right-3 flex items-center gap-2 px-3 py-1.5 rounded-lg bg-black/50 text-white text-sm cursor-pointer hover:bg-black/70 transition-colors">
                            {uploadingCover ? <Loader2 className="w-4 h-4 animate-spin" /> : <Upload className="w-4 h-4" />}
                            Đổi ảnh bìa
                            <input
                                type="file"
                                accept="image/*"
                                className="hidden"
                                onChange={(e) => {
                                    const file = e.target.files?.[0];
                                    if (file) handleImageUpload(file, 'cover');
                                }}
                            />
                        </label>
                    )}

                    {/* Logo */}
                    <div className="absolute -bottom-10 left-8">
                        <div className="relative w-24 h-24 rounded-2xl border-4 border-white dark:border-gray-800 bg-white dark:bg-gray-700 overflow-hidden shadow-lg">
                            {(editing ? form.logoUrl : shop.logoUrl) ? (
                                <img
                                    src={editing ? form.logoUrl : (shop.logoUrl || '')}
                                    alt="Logo"
                                    className="w-full h-full object-cover"
                                />
                            ) : (
                                <div className="w-full h-full flex items-center justify-center bg-gray-100 dark:bg-gray-600">
                                    <Store className="w-8 h-8 text-gray-400" />
                                </div>
                            )}
                            {editing && (
                                <label className="absolute inset-0 flex items-center justify-center bg-black/40 opacity-0 hover:opacity-100 transition-opacity cursor-pointer">
                                    {uploadingLogo ? <Loader2 className="w-5 h-5 text-white animate-spin" /> : <Upload className="w-5 h-5 text-white" />}
                                    <input
                                        type="file"
                                        accept="image/*"
                                        className="hidden"
                                        onChange={(e) => {
                                            const file = e.target.files?.[0];
                                            if (file) handleImageUpload(file, 'logo');
                                        }}
                                    />
                                </label>
                            )}
                        </div>
                    </div>
                </div>

                {/* Shop Info */}
                <div className="pt-14 px-8 pb-8">
                    {/* Status badge */}
                    <div className="flex items-center gap-3 mb-6">
                        <span className={`inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-semibold ${statusColor(shop.status)}`}>
                            <CheckCircle2 className="w-3.5 h-3.5" />
                            {shop.status}
                        </span>
                        <span className="text-sm text-gray-400">
                            ID: #{shop.id}
                        </span>
                    </div>

                    {editing ? (
                        /* Edit Mode */
                        <div className="space-y-5">
                            <div>
                                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5">
                                    Tên Shop <span className="text-red-500">*</span>
                                </label>
                                <input
                                    type="text"
                                    value={form.name}
                                    onChange={(e) => setForm(prev => ({ ...prev, name: e.target.value }))}
                                    className="w-full px-4 py-2.5 rounded-xl border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none transition-all"
                                    placeholder="Nhập tên shop"
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
                                    placeholder="ten-shop-cua-ban"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5">
                                    Mô tả
                                </label>
                                <textarea
                                    value={form.description}
                                    onChange={(e) => setForm(prev => ({ ...prev, description: e.target.value }))}
                                    rows={4}
                                    className="w-full px-4 py-2.5 rounded-xl border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none transition-all resize-none"
                                    placeholder="Mô tả về shop của bạn..."
                                />
                            </div>
                        </div>
                    ) : (
                        /* View Mode */
                        <div className="space-y-4">
                            <div>
                                <h2 className="text-2xl font-bold text-gray-900 dark:text-white">{shop.name}</h2>
                                <p className="text-sm text-gray-400 mt-0.5">/{shop.slug}</p>
                            </div>
                            {shop.description && (
                                <p className="text-gray-600 dark:text-gray-300 leading-relaxed">
                                    {shop.description}
                                </p>
                            )}
                            <div className="grid grid-cols-2 gap-4 pt-4 border-t border-gray-100 dark:border-gray-700">
                                <div>
                                    <p className="text-xs text-gray-400 uppercase tracking-wider mb-1">Ngày tạo</p>
                                    <p className="text-sm font-medium text-gray-700 dark:text-gray-300">
                                        {new Date(shop.createdAt).toLocaleDateString('vi-VN')}
                                    </p>
                                </div>
                                {shop.updatedAt && (
                                    <div>
                                        <p className="text-xs text-gray-400 uppercase tracking-wider mb-1">Cập nhật lần cuối</p>
                                        <p className="text-sm font-medium text-gray-700 dark:text-gray-300">
                                            {new Date(shop.updatedAt).toLocaleDateString('vi-VN')}
                                        </p>
                                    </div>
                                )}
                            </div>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}
