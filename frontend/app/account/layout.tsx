'use client';

import { useEffect } from 'react';
import { useRouter, usePathname } from 'next/navigation';
import Link from 'next/link';
import { useAuth } from '@/contexts/AuthContext';
import { User, Package, Settings, LogOut, ChevronRight } from 'lucide-react';

const menuItems = [
    { href: '/account', label: 'Tài khoản', icon: User, exact: true },
    { href: '/account/orders', label: 'Đơn hàng', icon: Package },
    { href: '/account/settings', label: 'Cài đặt', icon: Settings },
];

export default function AccountLayout({ children }: { children: React.ReactNode }) {
    const { user, isLoading, isAuthenticated, signOut } = useAuth();
    const router = useRouter();
    const pathname = usePathname();

    useEffect(() => {
        if (!isLoading && !isAuthenticated) {
            router.push('/login?redirect=' + encodeURIComponent(pathname));
        }
    }, [isLoading, isAuthenticated, router, pathname]);

    if (isLoading) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-slate-50">
                <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-violet-600"></div>
            </div>
        );
    }

    if (!isAuthenticated) {
        return null;
    }

    const handleSignOut = async () => {
        await signOut();
        router.push('/');
    };

    return (
        <div className="min-h-screen bg-slate-50">
            <div className="max-w-7xl mx-auto px-4 py-8">
                {/* Header */}
                <div className="mb-8">
                    <h1 className="text-2xl font-bold text-slate-800">Tài khoản của tôi</h1>
                    <p className="text-slate-500 mt-1">Xin chào, {user?.metadata?.full_name || user?.email}</p>
                </div>

                <div className="flex flex-col lg:flex-row gap-8">
                    {/* Sidebar */}
                    <aside className="lg:w-64 flex-shrink-0">
                        <nav className="bg-white rounded-2xl shadow-sm border border-slate-100 overflow-hidden">
                            {menuItems.map((item) => {
                                const isActive = item.exact
                                    ? pathname === item.href
                                    : pathname.startsWith(item.href);

                                return (
                                    <Link
                                        key={item.href}
                                        href={item.href}
                                        className={`flex items-center gap-3 px-5 py-4 border-b border-slate-100 last:border-0 transition-colors ${isActive
                                                ? 'bg-violet-50 text-violet-700'
                                                : 'text-slate-600 hover:bg-slate-50'
                                            }`}
                                    >
                                        <item.icon className={`w-5 h-5 ${isActive ? 'text-violet-600' : 'text-slate-400'}`} />
                                        <span className="font-medium">{item.label}</span>
                                        <ChevronRight className={`w-4 h-4 ml-auto ${isActive ? 'text-violet-400' : 'text-slate-300'}`} />
                                    </Link>
                                );
                            })}

                            <button
                                onClick={handleSignOut}
                                className="flex items-center gap-3 px-5 py-4 w-full text-left text-red-600 hover:bg-red-50 transition-colors"
                            >
                                <LogOut className="w-5 h-5" />
                                <span className="font-medium">Đăng xuất</span>
                            </button>
                        </nav>
                    </aside>

                    {/* Main Content */}
                    <main className="flex-1">
                        {children}
                    </main>
                </div>
            </div>
        </div>
    );
}
