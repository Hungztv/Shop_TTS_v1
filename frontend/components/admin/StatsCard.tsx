'use client';

import { ReactNode } from 'react';
import { LucideIcon } from 'lucide-react';

interface StatsCardProps {
    title: string;
    value: string | number;
    change?: string;
    changeType?: 'increase' | 'decrease' | 'neutral';
    icon: LucideIcon;
    iconColor?: string;
    loading?: boolean;
}

export default function StatsCard({
    title,
    value,
    change,
    changeType = 'neutral',
    icon: Icon,
    iconColor = 'bg-violet-100 text-violet-600 dark:bg-violet-900/30 dark:text-violet-400',
    loading = false,
}: StatsCardProps) {
    const changeColors = {
        increase: 'text-green-500',
        decrease: 'text-red-500',
        neutral: 'text-gray-500',
    };

    if (loading) {
        return (
            <div className="bg-white dark:bg-gray-800 rounded-2xl p-6 shadow-sm border border-gray-100 dark:border-gray-700 animate-pulse">
                <div className="flex items-center justify-between mb-4">
                    <div className="h-4 w-24 bg-gray-200 dark:bg-gray-700 rounded"></div>
                    <div className="h-12 w-12 bg-gray-200 dark:bg-gray-700 rounded-xl"></div>
                </div>
                <div className="h-8 w-32 bg-gray-200 dark:bg-gray-700 rounded mb-2"></div>
                <div className="h-3 w-20 bg-gray-200 dark:bg-gray-700 rounded"></div>
            </div>
        );
    }

    return (
        <div className="bg-white dark:bg-gray-800 rounded-2xl p-6 shadow-sm border border-gray-100 dark:border-gray-700 hover:shadow-md transition-shadow">
            <div className="flex items-center justify-between mb-4">
                <span className="text-sm font-medium text-gray-500 dark:text-gray-400">{title}</span>
                <div className={`p-3 rounded-xl ${iconColor}`}>
                    <Icon className="w-6 h-6" />
                </div>
            </div>
            <div className="flex items-end gap-2">
                <span className="text-3xl font-bold text-gray-900 dark:text-white">{value}</span>
                {change && (
                    <span className={`text-sm font-medium ${changeColors[changeType]} mb-1`}>
                        {changeType === 'increase' && '↑'}
                        {changeType === 'decrease' && '↓'}
                        {change}
                    </span>
                )}
            </div>
        </div>
    );
}
