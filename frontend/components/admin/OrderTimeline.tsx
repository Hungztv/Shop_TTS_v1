'use client';

import { Clock, CheckCircle, Truck, Package, XCircle } from 'lucide-react';

interface OrderTimelineProps {
    currentStatus: number;
}

const statusSteps = [
    { status: 0, label: 'Chờ xử lý', description: 'Đơn hàng đang chờ xác nhận', icon: Clock, color: 'amber' },
    { status: 1, label: 'Đã xác nhận', description: 'Đơn hàng đã được xác nhận', icon: CheckCircle, color: 'blue' },
    { status: 2, label: 'Đang giao', description: 'Đơn hàng đang được vận chuyển', icon: Truck, color: 'violet' },
    { status: 3, label: 'Đã giao', description: 'Giao hàng thành công', icon: Package, color: 'emerald' },
];

const getStatusColor = (status: number, currentStatus: number) => {
    if (currentStatus === 4) {
        return 'bg-red-500 text-white'; // Cancelled
    }
    if (status <= currentStatus) {
        return 'bg-emerald-500 text-white';
    }
    return 'bg-slate-200 text-slate-400';
};

const getLineColor = (status: number, currentStatus: number) => {
    if (currentStatus === 4) {
        return 'bg-red-300';
    }
    if (status < currentStatus) {
        return 'bg-emerald-500';
    }
    return 'bg-slate-200';
};

export default function OrderTimeline({ currentStatus }: OrderTimelineProps) {
    // If cancelled, show special view
    if (currentStatus === 4) {
        return (
            <div className="text-center py-8">
                <div className="w-16 h-16 mx-auto rounded-full bg-red-100 flex items-center justify-center mb-4">
                    <XCircle className="w-8 h-8 text-red-500" />
                </div>
                <h3 className="text-lg font-semibold text-red-600 mb-2">Đơn hàng đã bị hủy</h3>
                <p className="text-slate-500">Đơn hàng này đã được hủy và không thể tiếp tục xử lý</p>
            </div>
        );
    }

    return (
        <div className="relative">
            {statusSteps.map((step, index) => {
                const IconComponent = step.icon;
                const isCompleted = currentStatus >= step.status;
                const isCurrent = currentStatus === step.status;

                return (
                    <div key={step.status} className="flex items-start gap-4 mb-6 last:mb-0">
                        {/* Icon */}
                        <div className="relative">
                            <div className={`w-12 h-12 rounded-full flex items-center justify-center transition-all ${getStatusColor(step.status, currentStatus)} ${isCurrent ? 'ring-4 ring-emerald-100' : ''}`}>
                                <IconComponent className="w-5 h-5" />
                            </div>

                            {/* Connector Line */}
                            {index < statusSteps.length - 1 && (
                                <div className={`absolute left-1/2 -translate-x-1/2 top-12 w-0.5 h-6 ${getLineColor(step.status, currentStatus)}`} />
                            )}
                        </div>

                        {/* Content */}
                        <div className="flex-1 pt-2">
                            <p className={`font-semibold ${isCompleted ? 'text-slate-800' : 'text-slate-400'}`}>
                                {step.label}
                                {isCurrent && (
                                    <span className="ml-2 inline-flex px-2 py-0.5 text-xs font-medium bg-emerald-100 text-emerald-700 rounded-full">
                                        Hiện tại
                                    </span>
                                )}
                            </p>
                            <p className="text-sm text-slate-500 mt-0.5">
                                {step.description}
                            </p>
                        </div>
                    </div>
                );
            })}
        </div>
    );
}
