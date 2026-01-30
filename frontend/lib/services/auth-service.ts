import axios from "axios";
import type { SignUpRequest, SignInRequest, AuthResponse, User } from '@/types/auth';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000/api';

// Đăng Kí
export async function signUp(data: SignUpRequest): Promise<AuthResponse> {
    try {
        // Endpoint: POST /api/SupabaseAuth/signup
        const response = await axios.post(`${API_URL}/SupabaseAuth/signup`, {
            email: data.email,
            password: data.password,
            fullName: data.fullName,
            phone: data.phone,
        });
        return response.data;
    } catch (error: any) {
        return {
            success: false,
            message: error.response?.data?.message || 'Đăng ký thất bại',
            error: error.response?.data?.error,
        };
    }
}

// Đăng Nhập
export async function signIn(data: SignInRequest): Promise<AuthResponse> {
    try {
        // Endpoint: POST /api/SupabaseAuth/signin
        const response = await axios.post(`${API_URL}/SupabaseAuth/signin`, {
            email: data.email,
            password: data.password,
        });
        return response.data;
    } catch (error: any) {
        return {
            success: false,
            message: error.response?.data?.message || 'Đăng nhập thất bại',
            error: error.response?.data?.error,
        };
    }
}

// Lấy User
export async function getMe(accessToken: string): Promise<{ success: boolean; user?: User }> {
    try {
        // Endpoint: GET /api/SupabaseAuth/me
        const response = await axios.get(`${API_URL}/SupabaseAuth/me`, {
            headers: { Authorization: `Bearer ${accessToken}` },
        });
        return response.data;
    } catch (error: any) {
        return { success: false };
    }
}

// Refresh token
export async function refreshToken(refreshToken: string): Promise<AuthResponse> {
    try {
        // Endpoint: POST /api/SupabaseAuth/refresh
        const response = await axios.post(`${API_URL}/SupabaseAuth/refresh`, {
            refreshToken,
        });
        return response.data;
    } catch (error: any) {
        return { success: false, error: 'refresh_failed' };
    }
}

// Đăng Xuất
export async function signOut(accessToken: string): Promise<boolean> {
    try {
        // Endpoint: POST /api/SupabaseAuth/signout
        await axios.post(
            `${API_URL}/SupabaseAuth/signout`,
            {},
            { headers: { Authorization: `Bearer ${accessToken}` } }
        );
        return true;
    } catch {
        return false;
    }
}


// quên mật khẩu 
export async function forgotPassword(email: string): Promise<{ success: boolean }> {
    try {
        const response = await axios.post(`${API_URL}/SupabaseAuth/forgot-password`, { email });
        return response.data;
    } catch {
        return { success: true }; // Luôn trả true để tránh enumeration
    }
}