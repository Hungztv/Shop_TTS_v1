'use client';
import { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import Cookies from 'js-cookie';
import type { User } from '@/types/auth';
import * as authService from '@/lib/services/auth-service';
interface AuthContextType {
    user: User | null;
    isLoading: boolean;
    isAuthenticated: boolean;
    signIn: (email: string, password: string) => Promise<{ success: boolean; message?: string }>;
    signUp: (email: string, password: string, fullName?: string) => Promise<{ success: boolean; message?: string }>;
    signOut: () => Promise<void>;
}
const AuthContext = createContext<AuthContextType | undefined>(undefined);
export function AuthProvider({ children }: { children: ReactNode }) {
    const [user, setUser] = useState<User | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    // Khởi tạo: Kiểm tra token, lấy user
    useEffect(() => {
        const initAuth = async () => {
            const accessToken = Cookies.get('accessToken');
            if (accessToken) {
                const result = await authService.getMe(accessToken);
                if (result.success && result.user) {
                    setUser(result.user);
                } else {
                    // Token hết hạn, thử refresh
                    const refreshTokenValue = Cookies.get('refreshToken');
                    if (refreshTokenValue) {
                        const refreshResult = await authService.refreshToken(refreshTokenValue);
                        if (refreshResult.success && refreshResult.accessToken) {
                            Cookies.set('accessToken', refreshResult.accessToken, { expires: 1 });
                            if (refreshResult.refreshToken) {
                                Cookies.set('refreshToken', refreshResult.refreshToken, { expires: 7 });
                            }
                            // Retry get user
                            const retryResult = await authService.getMe(refreshResult.accessToken);
                            if (retryResult.success && retryResult.user) {
                                setUser(retryResult.user);
                            }
                        } else {
                            // Clear cookies
                            Cookies.remove('accessToken');
                            Cookies.remove('refreshToken');
                        }
                    }
                }
            }
            setIsLoading(false);
        };
        initAuth();
    }, []);
    // Đăng nhập
    const signIn = async (email: string, password: string) => {
        const result = await authService.signIn({ email, password });

        if (result.success && result.accessToken) {
            Cookies.set('accessToken', result.accessToken, { expires: 1 }); // 1 ngày
            if (result.refreshToken) {
                Cookies.set('refreshToken', result.refreshToken, { expires: 7 }); // 7 ngày
            }
            if (result.user) {
                setUser(result.user);
            }
        }

        return { success: result.success, message: result.message || result.error };
    };
    // Đăng ký
    const signUp = async (email: string, password: string, fullName?: string) => {
        const result = await authService.signUp({ email, password, fullName });

        if (result.success && result.accessToken) {
            Cookies.set('accessToken', result.accessToken, { expires: 1 });
            if (result.refreshToken) {
                Cookies.set('refreshToken', result.refreshToken, { expires: 7 });
            }
            if (result.user) {
                setUser(result.user);
            }
        }

        return { success: result.success, message: result.message || result.error };
    };
    // Đăng xuất
    const signOut = async () => {
        const accessToken = Cookies.get('accessToken');
        if (accessToken) {
            await authService.signOut(accessToken);
        }
        Cookies.remove('accessToken');
        Cookies.remove('refreshToken');
        setUser(null);
    };
    return (
        <AuthContext.Provider value={{ user, isLoading, isAuthenticated: !!user, signIn, signUp, signOut }}>
            {children}
        </AuthContext.Provider>
    );
}
export function useAuth() {
    const context = useContext(AuthContext);
    if (!context) throw new Error('useAuth must be used within AuthProvider');
    return context;
}