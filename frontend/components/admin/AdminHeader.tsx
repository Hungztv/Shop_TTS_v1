'use client';

import { useState } from 'react';
import { Bell, Search, Menu } from 'lucide-react';
import { useAuth } from '@/contexts/AuthContext';

interface AdminHeaderProps {
    title: string;
    subtitle?: string;
    onMenuClick?: () => void;
}

export default function AdminHeader({ title, subtitle, onMenuClick }: AdminHeaderProps) {
    const { user } = useAuth();
    const [showNotifications, setShowNotifications] = useState(false);

    return (
        <header className="sticky top-0 z-30 bg-white/80 dark:bg-gray-900/80 backdrop-blur-sm border-b border-gray-200 dark:border-gray-800">
            <div className="flex items-center justify-between h-16 px-6">
                {/* Left: Title */}
                <div className="flex items-center gap-4">
                    <button
                        onClick={onMenuClick}
                        className="lg:hidden p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-800"
                    >
                        <Menu className="w-5 h-5" />
                    </button>
                    <div>
                        <h1 className="text-xl font-bold text-gray-900 dark:text-white">{title}</h1>
                        {subtitle && (
                            <p className="text-sm text-gray-500 dark:text-gray-400">{subtitle}</p>
                        )}
                    </div>
                </div>

                {/* Right: Search + Notifications */}
                <div className="flex items-center gap-4">
                    {/* Search */}
                    <div className="hidden md:flex items-center gap-2 px-4 py-2 bg-gray-100 dark:bg-gray-800 rounded-xl">
                        <Search className="w-4 h-4 text-gray-400" />
                        <input
                            type="text"
                            placeholder="Tìm kiếm..."
                            className="bg-transparent border-none outline-none text-sm w-40"
                        />
                    </div>

                    {/* Notifications */}
                    <div className="relative">
                        <button
                            onClick={() => setShowNotifications(!showNotifications)}
                            className="relative p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-800"
                        >
                            <Bell className="w-5 h-5 text-gray-600 dark:text-gray-400" />
                            <span className="absolute top-1 right-1 w-2 h-2 bg-red-500 rounded-full"></span>
                        </button>

                        {showNotifications && (
                            <div className="absolute right-0 top-full mt-2 w-80 bg-white dark:bg-gray-800 rounded-2xl shadow-xl border border-gray-200 dark:border-gray-700 p-4">
                                <h3 className="font-semibold mb-3">Thông báo</h3>
                                <div className="space-y-3">
                                    <div className="flex items-start gap-3 p-2 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700">
                                        <div className="w-2 h-2 mt-2 bg-blue-500 rounded-full"></div>
                                        <div>
                                            <p className="text-sm font-medium">Đơn hàng mới #1234</p>
                                            <p className="text-xs text-gray-500">5 phút trước</p>
                                        </div>
                                    </div>
                                    <div className="flex items-start gap-3 p-2 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700">
                                        <div className="w-2 h-2 mt-2 bg-green-500 rounded-full"></div>
                                        <div>
                                            <p className="text-sm font-medium">Tin nhắn mới từ khách hàng</p>
                                            <p className="text-xs text-gray-500">10 phút trước</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        )}
                    </div>

                    {/* User Avatar */}
                    <div className="w-10 h-10 rounded-full bg-gradient-to-r from-violet-500 to-purple-500 flex items-center justify-center text-white font-bold">
                        {user?.email?.[0]?.toUpperCase() || 'A'}
                    </div>
                </div>
            </div>
        </header>
    );
}
