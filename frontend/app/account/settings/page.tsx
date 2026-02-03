'use client';

import { useState } from 'react';
import { useAuth } from '@/contexts/AuthContext';
import { updatePassword } from '@/lib/services/auth-service';
import { Lock, Save, Loader2, ShieldCheck } from 'lucide-react';
import Cookies from 'js-cookie';

export default function SettingsPage() {
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState<{ type: 'success' | 'error'; text: string } | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setMessage(null);

        if (password.length < 6) {
            setMessage({ type: 'error', text: 'Mật khẩu phải có ít nhất 6 ký tự' });
            return;
        }

        if (password !== confirmPassword) {
            setMessage({ type: 'error', text: 'Mật khẩu xác nhận không khớp' });
            return;
        }

        setLoading(true);

        try {
            const accessToken = Cookies.get('accessToken');
            if (!accessToken) {
                setMessage({ type: 'error', text: 'Phiên đăng nhập hết hạn' });
                return;
            }

            const result = await updatePassword(password, accessToken);

            if (result.success) {
                setMessage({ type: 'success', text: 'Đổi mật khẩu thành công!' });
                setPassword('');
                setConfirmPassword('');
            } else {
                setMessage({ type: 'error', text: result.message || 'Có lỗi xảy ra' });
            }
        } catch (error) {
            setMessage({ type: 'error', text: 'Có lỗi xảy ra, vui lòng thử lại' });
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="space-y-6">
            <div className="bg-white rounded-2xl shadow-sm border border-slate-100 p-6">
                <div className="flex items-center gap-3 mb-6">
                    <div className="w-12 h-12 rounded-xl bg-gradient-to-br from-violet-500 to-purple-600 flex items-center justify-center">
                        <Lock className="w-6 h-6 text-white" />
                    </div>
                    <div>
                        <h2 className="text-xl font-bold text-slate-800">Bảo mật tài khoản</h2>
                        <p className="text-sm text-slate-500">Quản lý mật khẩu và bảo mật</p>
                    </div>
                </div>

                <div className="max-w-xl">
                    <form onSubmit={handleSubmit} className="space-y-6">
                        {message && (
                            <div className={`p-4 rounded-xl flex items-start gap-3 ${message.type === 'success'
                                    ? 'bg-green-50 text-green-700 border border-green-200'
                                    : 'bg-red-50 text-red-700 border border-red-200'
                                }`}>
                                <ShieldCheck className="w-5 h-5 flex-shrink-0 mt-0.5" />
                                <div>
                                    <p className="font-medium">{message.type === 'success' ? 'Thành công' : 'Lỗi'}</p>
                                    <p className="text-sm opacity-90">{message.text}</p>
                                </div>
                            </div>
                        )}

                        <div>
                            <label className="block text-sm font-medium text-slate-700 mb-2">
                                Mật khẩu mới
                            </label>
                            <input
                                type="password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                className="w-full px-4 py-3 border border-slate-200 rounded-xl focus:ring-2 focus:ring-violet-500 focus:border-transparent outline-none transition-all"
                                placeholder="Nhập mật khẩu mới (tối thiểu 6 ký tự)"
                                required
                            />
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-slate-700 mb-2">
                                Xác nhận mật khẩu mới
                            </label>
                            <input
                                type="password"
                                value={confirmPassword}
                                onChange={(e) => setConfirmPassword(e.target.value)}
                                className="w-full px-4 py-3 border border-slate-200 rounded-xl focus:ring-2 focus:ring-violet-500 focus:border-transparent outline-none transition-all"
                                placeholder="Nhập lại mật khẩu mới"
                                required
                            />
                        </div>

                        <div className="pt-2">
                            <button
                                type="submit"
                                disabled={loading}
                                className="inline-flex items-center gap-2 px-6 py-3 bg-violet-600 text-white font-semibold rounded-xl hover:bg-violet-700 transition-all disabled:opacity-50 disabled:cursor-not-allowed"
                            >
                                {loading ? (
                                    <>
                                        <Loader2 className="w-5 h-5 animate-spin" />
                                        Đang xử lý...
                                    </>
                                ) : (
                                    <>
                                        <Save className="w-5 h-5" />
                                        Đổi mật khẩu
                                    </>
                                )}
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}
