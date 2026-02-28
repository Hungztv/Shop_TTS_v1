'use client';

import { useState, useRef, useEffect } from 'react';
import { Bell, Check, CheckCheck, Package, Truck, XCircle, ShoppingBag, Trash2, Info } from 'lucide-react';
import { useNotifications, type AppNotification, type NotificationType } from '@/contexts/NotificationContext';
import Link from 'next/link';

const ICON_MAP: Record<NotificationType, { icon: typeof Bell; color: string; bg: string }> = {
    order_placed: { icon: ShoppingBag, color: 'text-violet-600', bg: 'bg-violet-100 dark:bg-violet-900/30' },
    order_confirmed: { icon: Check, color: 'text-blue-600', bg: 'bg-blue-100 dark:bg-blue-900/30' },
    order_shipping: { icon: Truck, color: 'text-purple-600', bg: 'bg-purple-100 dark:bg-purple-900/30' },
    order_delivered: { icon: Package, color: 'text-emerald-600', bg: 'bg-emerald-100 dark:bg-emerald-900/30' },
    order_cancelled: { icon: XCircle, color: 'text-red-600', bg: 'bg-red-100 dark:bg-red-900/30' },
    info: { icon: Info, color: 'text-slate-600', bg: 'bg-slate-100 dark:bg-slate-800' },
};

function timeAgo(dateStr: string): string {
    const now = Date.now();
    const diff = now - new Date(dateStr).getTime();
    const minutes = Math.floor(diff / 60000);
    if (minutes < 1) return 'Vừa xong';
    if (minutes < 60) return `${minutes} phút trước`;
    const hours = Math.floor(minutes / 60);
    if (hours < 24) return `${hours} giờ trước`;
    const days = Math.floor(hours / 24);
    if (days < 7) return `${days} ngày trước`;
    return new Date(dateStr).toLocaleDateString('vi-VN');
}

function NotificationItem({ notification, onRead }: { notification: AppNotification; onRead: (id: string) => void }) {
    const iconInfo = ICON_MAP[notification.type] || ICON_MAP.info;
    const Icon = iconInfo.icon;

    const content = (
        <div
            className={`flex items-start gap-3 p-3 rounded-xl transition-all cursor-pointer ${
                notification.read
                    ? 'hover:bg-slate-50 dark:hover:bg-slate-800/50'
                    : 'bg-violet-50/50 dark:bg-violet-900/10 hover:bg-violet-50 dark:hover:bg-violet-900/20'
            }`}
            onClick={() => !notification.read && onRead(notification.id)}
        >
            <div className={`w-9 h-9 rounded-xl ${iconInfo.bg} flex items-center justify-center flex-shrink-0`}>
                <Icon className={`w-4 h-4 ${iconInfo.color}`} />
            </div>
            <div className="flex-1 min-w-0">
                <p className={`text-sm leading-tight ${notification.read ? 'text-slate-600 dark:text-slate-400' : 'text-slate-800 dark:text-white font-medium'}`}>
                    {notification.title}
                </p>
                <p className="text-xs text-slate-500 dark:text-slate-400 mt-0.5 line-clamp-2">
                    {notification.message}
                </p>
                <p className="text-[11px] text-slate-400 mt-1">{timeAgo(notification.createdAt)}</p>
            </div>
            {!notification.read && (
                <div className="w-2 h-2 rounded-full bg-violet-500 flex-shrink-0 mt-2" />
            )}
        </div>
    );

    if (notification.link) {
        return <Link href={notification.link}>{content}</Link>;
    }
    return content;
}

export default function NotificationBell() {
    const { notifications, unreadCount, markAsRead, markAllAsRead, clearAll } = useNotifications();
    const [isOpen, setIsOpen] = useState(false);
    const dropdownRef = useRef<HTMLDivElement>(null);

    // Close dropdown on outside click
    useEffect(() => {
        const handleClickOutside = (e: MouseEvent) => {
            if (dropdownRef.current && !dropdownRef.current.contains(e.target as Node)) {
                setIsOpen(false);
            }
        };
        document.addEventListener('mousedown', handleClickOutside);
        return () => document.removeEventListener('mousedown', handleClickOutside);
    }, []);

    return (
        <div className="relative" ref={dropdownRef}>
            {/* Bell Button */}
            <button
                onClick={() => setIsOpen(!isOpen)}
                className="relative p-2 rounded-lg hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors"
                aria-label="Thông báo"
            >
                <Bell className="w-5 h-5 text-slate-600 dark:text-slate-300" />
                {unreadCount > 0 && (
                    <span className="absolute -top-0.5 -right-0.5 min-w-[16px] h-4 px-0.5 bg-red-500 text-white text-[10px] rounded-full flex items-center justify-center font-bold animate-pulse">
                        {unreadCount > 99 ? '99+' : unreadCount}
                    </span>
                )}
            </button>

            {/* Dropdown */}
            {isOpen && (
                <div className="absolute right-0 top-full mt-2 w-80 sm:w-96 bg-white dark:bg-slate-900 rounded-2xl shadow-2xl border border-slate-100 dark:border-slate-700 z-50 animate-fade-in-down overflow-hidden">
                    {/* Header */}
                    <div className="flex items-center justify-between px-4 py-3 border-b border-slate-100 dark:border-slate-700">
                        <div className="flex items-center gap-2">
                            <h3 className="font-bold text-slate-800 dark:text-white text-sm">Thông báo</h3>
                            {unreadCount > 0 && (
                                <span className="px-1.5 py-0.5 bg-violet-100 dark:bg-violet-900/30 text-violet-600 dark:text-violet-400 text-[11px] font-bold rounded-md">
                                    {unreadCount} mới
                                </span>
                            )}
                        </div>
                        <div className="flex items-center gap-1">
                            {unreadCount > 0 && (
                                <button
                                    onClick={() => markAllAsRead()}
                                    className="p-1.5 rounded-lg hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors"
                                    title="Đánh dấu tất cả đã đọc"
                                >
                                    <CheckCheck className="w-4 h-4 text-slate-400 hover:text-violet-500" />
                                </button>
                            )}
                            {notifications.length > 0 && (
                                <button
                                    onClick={() => { clearAll(); setIsOpen(false); }}
                                    className="p-1.5 rounded-lg hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors"
                                    title="Xóa tất cả"
                                >
                                    <Trash2 className="w-4 h-4 text-slate-400 hover:text-red-500" />
                                </button>
                            )}
                        </div>
                    </div>

                    {/* Notification List */}
                    <div className="max-h-[400px] overflow-y-auto p-2">
                        {notifications.length === 0 ? (
                            <div className="py-10 text-center">
                                <Bell className="w-10 h-10 text-slate-200 dark:text-slate-700 mx-auto mb-3" />
                                <p className="text-sm text-slate-400">Chưa có thông báo nào</p>
                            </div>
                        ) : (
                            <div className="space-y-1">
                                {notifications.map((notif) => (
                                    <NotificationItem
                                        key={notif.id}
                                        notification={notif}
                                        onRead={(id) => { markAsRead(id); }}
                                    />
                                ))}
                            </div>
                        )}
                    </div>

                    {/* Footer */}
                    {notifications.length > 0 && (
                        <div className="border-t border-slate-100 dark:border-slate-700 p-2">
                            <Link
                                href="/account/orders"
                                onClick={() => setIsOpen(false)}
                                className="block w-full text-center py-2 text-sm font-medium text-violet-600 hover:bg-violet-50 dark:hover:bg-violet-900/20 rounded-xl transition-colors"
                            >
                                Xem tất cả đơn hàng
                            </Link>
                        </div>
                    )}
                </div>
            )}
        </div>
    );
}
