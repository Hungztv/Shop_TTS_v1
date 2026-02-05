'use client';

import { useState, useEffect } from 'react';
import { useParams, useRouter } from 'next/navigation';
import Link from 'next/link';
import { ArrowLeft, Edit, Trash2, Star, Package, Calendar, Tag } from 'lucide-react';
import { productsService } from '@/lib/services/admin/products-service';
import { Product } from '@/lib/services/admin/dashboard-service';

export default function ProductDetailPage() {
    const params = useParams();
    const router = useRouter();
    const id = Number(params.id);

    const [product, setProduct] = useState<Product | null>(null);
    const [loading, setLoading] = useState(true);
    const [deleting, setDeleting] = useState(false);

    useEffect(() => {
        loadProduct();
    }, [id]);

    const loadProduct = async () => {
        try {
            const data = await productsService.getById(id);
            setProduct(data);
        } catch (error) {
            console.error('Error loading product:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async () => {
        if (!confirm('Bạn có chắc muốn xóa sản phẩm này?')) return;

        setDeleting(true);
        try {
            await productsService.delete(id);
            router.push('/admin/products');
        } catch (error) {
            console.error('Error deleting product:', error);
            alert('Có lỗi xảy ra khi xóa sản phẩm!');
        } finally {
            setDeleting(false);
        }
    };

    const formatCurrency = (value: number) => {
        return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
    };

    const formatDate = (dateString: string) => {
        return new Date(dateString).toLocaleDateString('vi-VN', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    };

    if (loading) {
        return (
            <div className="min-h-screen bg-slate-50 p-6">
                <div className="max-w-6xl mx-auto">
                    <div className="animate-pulse">
                        <div className="h-10 bg-slate-200 rounded w-48 mb-6"></div>
                        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                            <div className="bg-slate-200 aspect-square rounded-2xl"></div>
                            <div className="space-y-4">
                                <div className="h-8 bg-slate-200 rounded w-3/4"></div>
                                <div className="h-4 bg-slate-200 rounded w-1/2"></div>
                                <div className="h-12 bg-slate-200 rounded w-1/3"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    if (!product) {
        return (
            <div className="min-h-screen bg-slate-50 p-6 flex items-center justify-center">
                <div className="text-center">
                    <div className="text-6xl mb-4">📦</div>
                    <h2 className="text-2xl font-bold text-slate-800 mb-2">Không tìm thấy sản phẩm</h2>
                    <p className="text-slate-500 mb-4">Sản phẩm có thể đã bị xóa hoặc không tồn tại</p>
                    <Link href="/admin/products">
                        <button className="px-6 py-3 bg-violet-600 text-white rounded-xl hover:bg-violet-700 transition-colors">
                            Quay lại danh sách
                        </button>
                    </Link>
                </div>
            </div>
        );
    }

    const profit = product.capitalPrice ? product.price - product.capitalPrice : 0;
    const profitPercent = product.capitalPrice && product.price > 0
        ? Math.round((profit / product.price) * 100)
        : 0;

    const isInStock = product.quantity > 0;

    return (
        <div className="min-h-screen bg-slate-50 p-6">
            <div className="max-w-6xl mx-auto">
                {/* Header */}
                <div className="mb-6 flex items-center justify-between">
                    <div className="flex items-center gap-4">
                        <Link href="/admin/products">
                            <button className="w-10 h-10 rounded-xl bg-white border border-slate-200 flex items-center justify-center hover:bg-slate-50 transition-colors">
                                <ArrowLeft className="w-5 h-5 text-slate-600" />
                            </button>
                        </Link>
                        <h1 className="text-2xl font-bold text-slate-800">Chi tiết sản phẩm</h1>
                    </div>
                    <div className="flex gap-3">
                        <Link href={`/admin/products/${id}/edit`}>
                            <button className="px-4 py-2 rounded-xl bg-violet-600 text-white font-medium hover:bg-violet-700 transition-colors flex items-center gap-2">
                                <Edit className="w-4 h-4" />
                                Sửa
                            </button>
                        </Link>
                        <button
                            onClick={handleDelete}
                            disabled={deleting}
                            className="px-4 py-2 rounded-xl bg-red-500 text-white font-medium hover:bg-red-600 transition-colors flex items-center gap-2 disabled:opacity-50"
                        >
                            <Trash2 className="w-4 h-4" />
                            {deleting ? 'Đang xóa...' : 'Xóa'}
                        </button>
                    </div>
                </div>

                <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                    {/* Left: Image */}
                    <div className="bg-white rounded-2xl shadow-sm overflow-hidden">
                        <img
                            src={product.image || 'https://placehold.co/600x600?text=No+Image'}
                            alt={product.name}
                            className="w-full aspect-square object-cover"
                        />
                    </div>

                    {/* Right: Info */}
                    <div className="space-y-6">
                        {/* Basic Info */}
                        <div className="bg-white rounded-2xl shadow-sm p-6">
                            <div className="flex items-start justify-between mb-4">
                                <div>
                                    <span className={`inline-flex px-3 py-1 rounded-full text-xs font-medium ${isInStock ? 'bg-emerald-100 text-emerald-700' : 'bg-red-100 text-red-700'}`}>
                                        {isInStock ? 'Còn hàng' : 'Hết hàng'}
                                    </span>
                                </div>
                                <div className="flex items-center gap-1">
                                    <Star className="w-5 h-5 text-amber-400 fill-amber-400" />
                                    <span className="font-medium">{product.averageScore || 0}</span>
                                    <span className="text-slate-400">({product.ratingCount || 0} đánh giá)</span>
                                </div>
                            </div>

                            <h2 className="text-2xl font-bold text-slate-800 mb-2">{product.name}</h2>
                            <p className="text-slate-500 mb-4">/{product.slug}</p>

                            <div className="flex items-baseline gap-4 mb-6">
                                <span className="text-3xl font-bold text-violet-600">
                                    {formatCurrency(product.price)}
                                </span>
                                {product.capitalPrice && (
                                    <span className="text-lg text-slate-400 line-through">
                                        {formatCurrency(product.capitalPrice)}
                                    </span>
                                )}
                            </div>

                            <div className="grid grid-cols-2 gap-4 pt-4 border-t border-slate-100">
                                <div className="flex items-center gap-3">
                                    <div className="w-10 h-10 rounded-xl bg-violet-100 flex items-center justify-center">
                                        <Package className="w-5 h-5 text-violet-600" />
                                    </div>
                                    <div>
                                        <p className="text-sm text-slate-500">Tồn kho</p>
                                        <p className="font-semibold text-slate-800">{product.quantity}</p>
                                    </div>
                                </div>
                                <div className="flex items-center gap-3">
                                    <div className="w-10 h-10 rounded-xl bg-emerald-100 flex items-center justify-center">
                                        <Tag className="w-5 h-5 text-emerald-600" />
                                    </div>
                                    <div>
                                        <p className="text-sm text-slate-500">Đã bán</p>
                                        <p className="font-semibold text-slate-800">{product.soldOut}</p>
                                    </div>
                                </div>
                            </div>
                        </div>

                        {/* Price Analysis */}
                        <div className="bg-white rounded-2xl shadow-sm p-6">
                            <h3 className="font-semibold text-slate-800 mb-4">Phân tích lợi nhuận</h3>
                            <div className="space-y-3">
                                <div className="flex justify-between">
                                    <span className="text-slate-500">Giá bán</span>
                                    <span className="font-medium text-slate-800">{formatCurrency(product.price)}</span>
                                </div>
                                <div className="flex justify-between">
                                    <span className="text-slate-500">Giá vốn</span>
                                    <span className="font-medium text-slate-800">
                                        {product.capitalPrice ? formatCurrency(product.capitalPrice) : 'Chưa cập nhật'}
                                    </span>
                                </div>
                                <div className="flex justify-between pt-3 border-t border-slate-100">
                                    <span className="font-medium text-slate-800">Lợi nhuận</span>
                                    <span className="font-bold text-emerald-600">
                                        {formatCurrency(profit)} ({profitPercent}%)
                                    </span>
                                </div>
                            </div>
                        </div>

                        {/* Category & Brand */}
                        <div className="bg-white rounded-2xl shadow-sm p-6">
                            <h3 className="font-semibold text-slate-800 mb-4">Phân loại</h3>
                            <div className="grid grid-cols-2 gap-4">
                                <div>
                                    <p className="text-sm text-slate-500 mb-1">Danh mục</p>
                                    <p className="font-medium text-slate-800">{product.category?.name || 'Chưa cập nhật'}</p>
                                </div>
                                <div>
                                    <p className="text-sm text-slate-500 mb-1">Thương hiệu</p>
                                    <p className="font-medium text-slate-800">{product.brand?.name || 'Chưa cập nhật'}</p>
                                </div>
                            </div>
                        </div>

                        {/* Timestamps */}
                        <div className="bg-white rounded-2xl shadow-sm p-6">
                            <h3 className="font-semibold text-slate-800 mb-4">Thời gian</h3>
                            <div className="space-y-3">
                                <div className="flex items-center gap-3">
                                    <Calendar className="w-5 h-5 text-slate-400" />
                                    <div>
                                        <p className="text-sm text-slate-500">Ngày tạo</p>
                                        <p className="font-medium text-slate-800">{formatDate(product.createdAt)}</p>
                                    </div>
                                </div>
                                {product.updatedAt && (
                                    <div className="flex items-center gap-3">
                                        <Calendar className="w-5 h-5 text-slate-400" />
                                        <div>
                                            <p className="text-sm text-slate-500">Cập nhật lần cuối</p>
                                            <p className="font-medium text-slate-800">{formatDate(product.updatedAt)}</p>
                                        </div>
                                    </div>
                                )}
                            </div>
                        </div>
                    </div>
                </div>

                {/* Description */}
                <div className="mt-8 bg-white rounded-2xl shadow-sm p-6">
                    <h3 className="font-semibold text-slate-800 mb-4">Mô tả sản phẩm</h3>
                    <div className="prose prose-slate max-w-none">
                        <p className="text-slate-600 whitespace-pre-wrap">
                            {product.description || 'Chưa có mô tả'}
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
}
