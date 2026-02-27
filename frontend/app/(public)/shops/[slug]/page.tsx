'use client';

import { useState, useEffect, Suspense } from 'react';
import { useParams, useRouter, useSearchParams } from 'next/navigation';
import Link from 'next/link';
import {
    Search, Star, Package, CalendarDays, ChevronLeft, ChevronRight,
    Store, ArrowLeft
} from 'lucide-react';
import ProductCard from '@/components/ui/ProductCard';
import {
    shopsPublicService,
    categoriesPublicService,
    ShopPublic,
    Product,
    Category,
    PaginatedResponse
} from '@/lib/services/public-api';
import { mapProduct, formatPrice } from '@/lib/utils/product-mapper';

function ShopContent() {
    const params = useParams();
    const router = useRouter();
    const searchParams = useSearchParams();
    const slug = params.slug as string;

    // State
    const [shop, setShop] = useState<ShopPublic | null>(null);
    const [products, setProducts] = useState<PaginatedResponse<Product>>({ items: [], totalCount: 0, page: 1, pageSize: 12, totalPages: 0 });
    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState(true);
    const [productsLoading, setProductsLoading] = useState(true);

    // Filters from URL
    const page = searchParams.get('page') ? parseInt(searchParams.get('page')!) : 1;
    const categoryId = searchParams.get('category') ? parseInt(searchParams.get('category')!) : undefined;
    const search = searchParams.get('search') || '';
    const sortBy = searchParams.get('sortBy') || 'createdAt';
    const sortOrder = (searchParams.get('sortOrder') as 'asc' | 'desc') || 'desc';

    // Local state
    const [searchInput, setSearchInput] = useState(search);

    // Load shop info
    useEffect(() => {
        const loadShop = async () => {
            setLoading(true);
            const shopData = await shopsPublicService.getBySlug(slug);
            if (!shopData) {
                setLoading(false);
                return;
            }
            setShop(shopData);
            setLoading(false);
        };
        loadShop();
    }, [slug]);

    // Load categories
    useEffect(() => {
        categoriesPublicService.getAll().then(setCategories);
    }, []);

    // Load products when shop or filters change
    useEffect(() => {
        if (!shop) return;
        const loadProducts = async () => {
            setProductsLoading(true);
            const data = await shopsPublicService.getProducts(shop.id, {
                page,
                pageSize: 12,
                categoryId,
                search,
                sortBy,
                sortOrder
            });
            setProducts(data);
            setProductsLoading(false);
        };
        loadProducts();
    }, [shop, page, categoryId, search, sortBy, sortOrder]);

    // URL update helper
    const updateFilters = (params: Record<string, string | undefined>) => {
        const newParams = new URLSearchParams(searchParams.toString());
        Object.entries(params).forEach(([key, value]) => {
            if (value) newParams.set(key, value);
            else newParams.delete(key);
        });
        if (!params.page) newParams.delete('page');
        router.push(`/shops/${slug}?${newParams.toString()}`);
    };

    const handleSearch = (e: React.FormEvent) => {
        e.preventDefault();
        updateFilters({ search: searchInput || undefined });
    };

    // Loading skeleton
    if (loading) {
        return (
            <div className="min-h-screen bg-slate-50 dark:bg-slate-900">
                <div className="animate-pulse">
                    <div className="h-48 md:h-64 bg-slate-200 dark:bg-slate-800" />
                    <div className="max-w-7xl mx-auto px-4 -mt-16">
                        <div className="flex items-end gap-4 mb-8">
                            <div className="w-24 h-24 rounded-2xl bg-slate-300 dark:bg-slate-700 border-4 border-white dark:border-slate-900" />
                            <div>
                                <div className="h-8 w-48 bg-slate-300 dark:bg-slate-700 rounded mb-2" />
                                <div className="h-4 w-32 bg-slate-200 dark:bg-slate-800 rounded" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    // Not found
    if (!shop) {
        return (
            <div className="min-h-screen bg-slate-50 dark:bg-slate-900 flex items-center justify-center">
                <div className="text-center">
                    <div className="text-6xl mb-4">🏪</div>
                    <h2 className="text-2xl font-bold text-slate-800 dark:text-white mb-2">Không tìm thấy shop</h2>
                    <p className="text-slate-500 dark:text-slate-400 mb-6">Shop này không tồn tại hoặc đã ngưng hoạt động</p>
                    <Link
                        href="/"
                        className="inline-flex items-center gap-2 px-6 py-3 bg-violet-600 text-white rounded-xl hover:bg-violet-700 transition-colors"
                    >
                        <ArrowLeft className="w-4 h-4" />
                        Về trang chủ
                    </Link>
                </div>
            </div>
        );
    }

    const memberSince = new Date(shop.createdAt).toLocaleDateString('vi-VN', { month: 'long', year: 'numeric' });

    return (
        <div className="min-h-screen bg-slate-50 dark:bg-slate-900">
            {/* Shop Header / Cover */}
            <div className="relative">
                {shop.coverUrl ? (
                    <div className="h-48 md:h-64 overflow-hidden">
                        <img
                            src={shop.coverUrl}
                            alt={`${shop.name} cover`}
                            className="w-full h-full object-cover"
                        />
                        <div className="absolute inset-0 bg-gradient-to-t from-black/60 to-transparent" />
                    </div>
                ) : (
                    <div className="h-48 md:h-64 bg-gradient-to-r from-violet-600 via-purple-600 to-indigo-600">
                        <div className="absolute inset-0 bg-[url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNjAiIGhlaWdodD0iNjAiIHZpZXdCb3g9IjAgMCA2MCA2MCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48ZyBmaWxsPSJub25lIiBmaWxsLXJ1bGU9ImV2ZW5vZGQiPjxnIGZpbGw9IiNmZmYiIGZpbGwtb3BhY2l0eT0iMC4xIj48cGF0aCBkPSJNMzYgMzRjLTEuMSAwLTIgLjktMiAyIDAgMSAuOSAyIDIgMiAxLjEgMCAyLS45IDItMiAwLTEuMS0uOS0yLTItMnptMC0xNGMtMS4xIDAtMiAuOS0yIDIgMCAxIC45IDIgMiAyIDEuMSAwIDItLjkgMi0yIDAtMS4xLS45LTItMi0yek0yMiAzNGMtMS4xIDAtMiAuOS0yIDIgMCAxIC45IDIgMiAyIDEuMSAwIDItLjkgMi0yIDAtMS4xLS45LTItMi0yem0wLTE0Yy0xLjEgMC0yIC45LTIgMiAwIDEgLjkgMiAyIDIgMS4xIDAgMi0uOSAyLTIgMC0xLjEtLjktMi0yLTJ6Ii8+PC9nPjwvZz48L3N2Zz4=')] opacity-30" />
                    </div>
                )}

                {/* Shop Info Overlay */}
                <div className="max-w-7xl mx-auto px-4">
                    <div className="relative -mt-16 md:-mt-20 flex flex-col sm:flex-row items-start sm:items-end gap-4 pb-6">
                        {/* Logo */}
                        <div className="w-24 h-24 md:w-28 md:h-28 rounded-2xl overflow-hidden border-4 border-white dark:border-slate-900 shadow-xl bg-white dark:bg-slate-800 flex-shrink-0">
                            {shop.logoUrl ? (
                                <img src={shop.logoUrl} alt={shop.name} className="w-full h-full object-cover" />
                            ) : (
                                <div className="w-full h-full flex items-center justify-center bg-gradient-to-br from-violet-500 to-purple-600">
                                    <Store className="w-10 h-10 text-white" />
                                </div>
                            )}
                        </div>

                        {/* Shop Name & Stats */}
                        <div className="flex-1">
                            <h1 className="text-2xl md:text-3xl font-bold text-slate-800 dark:text-white">
                                {shop.name}
                            </h1>
                            {shop.description && (
                                <p className="text-slate-500 dark:text-slate-400 mt-1 line-clamp-2 max-w-2xl">
                                    {shop.description}
                                </p>
                            )}
                            <div className="flex flex-wrap items-center gap-4 mt-3">
                                <div className="flex items-center gap-1.5 text-sm text-slate-600 dark:text-slate-300">
                                    <Package className="w-4 h-4 text-violet-500" />
                                    <span className="font-semibold">{shop.totalProducts}</span> sản phẩm
                                </div>
                                <div className="flex items-center gap-1.5 text-sm text-slate-600 dark:text-slate-300">
                                    <Star className="w-4 h-4 text-yellow-500 fill-yellow-500" />
                                    <span className="font-semibold">{shop.averageRating > 0 ? shop.averageRating.toFixed(1) : 'Chưa có'}</span>
                                    {shop.averageRating > 0 && ' đánh giá'}
                                </div>
                                <div className="flex items-center gap-1.5 text-sm text-slate-500 dark:text-slate-400">
                                    <CalendarDays className="w-4 h-4" />
                                    Mở từ {memberSince}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            {/* Filter Bar */}
            <div className="bg-white dark:bg-slate-800 border-b border-t dark:border-slate-700 sticky top-0 z-10">
                <div className="max-w-7xl mx-auto px-4 py-3">
                    <div className="flex flex-col sm:flex-row items-stretch sm:items-center gap-3">
                        {/* Search */}
                        <form onSubmit={handleSearch} className="flex gap-2 flex-1 max-w-md">
                            <div className="relative flex-1">
                                <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400" />
                                <input
                                    type="text"
                                    value={searchInput}
                                    onChange={(e) => setSearchInput(e.target.value)}
                                    placeholder="Tìm trong shop..."
                                    className="w-full pl-9 pr-4 py-2 rounded-xl border border-slate-200 dark:border-slate-700 focus:border-violet-500 focus:ring-2 focus:ring-violet-200 dark:focus:ring-violet-800 outline-none transition-all bg-white dark:bg-slate-900 dark:text-white text-sm"
                                />
                            </div>
                            <button
                                type="submit"
                                className="px-4 py-2 bg-violet-600 text-white rounded-xl hover:bg-violet-700 transition-colors text-sm font-medium"
                            >
                                Tìm
                            </button>
                        </form>

                        <div className="flex items-center gap-3">
                            {/* Category Filter */}
                            <select
                                value={categoryId || ''}
                                onChange={(e) => updateFilters({ category: e.target.value || undefined })}
                                className="px-3 py-2 rounded-xl border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-900 dark:text-white text-sm focus:border-violet-500 outline-none"
                            >
                                <option value="">Tất cả danh mục</option>
                                {categories.map(cat => (
                                    <option key={cat.id} value={cat.id}>{cat.name}</option>
                                ))}
                            </select>

                            {/* Sort */}
                            <select
                                value={`${sortBy}-${sortOrder}`}
                                onChange={(e) => {
                                    const [sb, so] = e.target.value.split('-');
                                    updateFilters({ sortBy: sb, sortOrder: so });
                                }}
                                className="px-3 py-2 rounded-xl border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-900 dark:text-white text-sm focus:border-violet-500 outline-none"
                            >
                                <option value="createdAt-desc">Mới nhất</option>
                                <option value="soldOut-desc">Bán chạy</option>
                                <option value="price-asc">Giá thấp → cao</option>
                                <option value="price-desc">Giá cao → thấp</option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>

            {/* Products */}
            <div className="max-w-7xl mx-auto px-4 py-6">
                {/* Results count */}
                <p className="text-sm text-slate-500 dark:text-slate-400 mb-4">
                    {products.totalCount} sản phẩm
                    {search && <> cho &quot;{search}&quot;</>}
                </p>

                {productsLoading ? (
                    <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                        {[...Array(12)].map((_, i) => (
                            <div key={i} className="animate-pulse">
                                <div className="bg-slate-200 dark:bg-slate-800 rounded-2xl aspect-square mb-3" />
                                <div className="h-4 bg-slate-200 dark:bg-slate-800 rounded w-3/4 mb-2" />
                                <div className="h-6 bg-slate-200 dark:bg-slate-800 rounded w-1/2" />
                            </div>
                        ))}
                    </div>
                ) : products.items.length > 0 ? (
                    <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                        {products.items.map((product) => (
                            <ProductCard key={product.id} {...mapProduct(product)} />
                        ))}
                    </div>
                ) : (
                    <div className="text-center py-16">
                        <div className="text-6xl mb-4">📦</div>
                        <h3 className="text-xl font-semibold text-slate-700 dark:text-slate-300 mb-2">
                            Không tìm thấy sản phẩm
                        </h3>
                        <p className="text-slate-500 dark:text-slate-400 mb-4">
                            Thử thay đổi bộ lọc hoặc từ khóa tìm kiếm
                        </p>
                        <button
                            onClick={() => {
                                setSearchInput('');
                                router.push(`/shops/${slug}`);
                            }}
                            className="px-6 py-2 bg-violet-600 text-white rounded-xl hover:bg-violet-700 transition-colors"
                        >
                            Xóa bộ lọc
                        </button>
                    </div>
                )}

                {/* Pagination */}
                {products.totalPages > 1 && (
                    <div className="flex items-center justify-center gap-2 mt-8">
                        <button
                            onClick={() => updateFilters({ page: (page - 1).toString() })}
                            disabled={page <= 1}
                            className="p-2 rounded-lg border border-slate-200 dark:border-slate-700 hover:bg-slate-100 dark:hover:bg-slate-800 disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            <ChevronLeft className="w-5 h-5" />
                        </button>

                        {[...Array(Math.min(5, products.totalPages))].map((_, i) => {
                            let pageNum = i + 1;
                            if (products.totalPages > 5) {
                                if (page > 3) pageNum = page - 2 + i;
                                if (page > products.totalPages - 2) pageNum = products.totalPages - 4 + i;
                            }
                            return (
                                <button
                                    key={pageNum}
                                    onClick={() => updateFilters({ page: pageNum.toString() })}
                                    className={`w-10 h-10 rounded-lg font-medium transition-colors ${page === pageNum
                                        ? 'bg-violet-600 text-white'
                                        : 'border border-slate-200 dark:border-slate-700 hover:bg-slate-100 dark:hover:bg-slate-800 dark:text-white'
                                        }`}
                                >
                                    {pageNum}
                                </button>
                            );
                        })}

                        <button
                            onClick={() => updateFilters({ page: (page + 1).toString() })}
                            disabled={page >= products.totalPages}
                            className="p-2 rounded-lg border border-slate-200 dark:border-slate-700 hover:bg-slate-100 dark:hover:bg-slate-800 disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            <ChevronRight className="w-5 h-5" />
                        </button>
                    </div>
                )}
            </div>
        </div>
    );
}

export default function ShopPage() {
    return (
        <Suspense fallback={
            <div className="min-h-screen flex items-center justify-center">
                <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-violet-600"></div>
            </div>
        }>
            <ShopContent />
        </Suspense>
    );
}
