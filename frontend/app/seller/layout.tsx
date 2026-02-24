'use client';

import { ReactNode, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/contexts/AuthContext';
import SellerSidebar from '@/components/seller/SellerSidebar';

export default function SellerLayout({ children }: { children: ReactNode }) {
    const { user, isAuthenticated, isLoading } = useAuth();
    const router = useRouter();

    const isSeller = user?.roles?.some(
        (role) => role.toLowerCase() === 'seller' || role.toLowerCase() === 'admin'
    ) ?? false;

    useEffect(() => {
        if (!isLoading && !isAuthenticated) {
            router.push('/login');
            return;
        }

        if (!isLoading && isAuthenticated && !isSeller) {
            router.push('/account');
        }
    }, [isLoading, isAuthenticated, isSeller, router]);

    if (isLoading) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-900">
                <div className="flex flex-col items-center gap-4">
                    <div className="w-12 h-12 border-4 border-emerald-500 border-t-transparent rounded-full animate-spin"></div>
                    <p className="text-gray-500">Đang tải...</p>
                </div>
            </div>
        );
    }

    if (!isAuthenticated || !isSeller) {
        return null;
    }

    return (
        <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
            <SellerSidebar />
            <main className="lg:ml-64 transition-all duration-300 relative z-10">
                {children}
            </main>
        </div>
    );
}
