import api from './api';

export interface UploadResult {
    success: boolean;
    message: string;
    data?: {
        url: string;
        filePath: string;
        fileName: string;
        fileSize: number;
    };
}

export interface MultiUploadResult {
    success: boolean;
    message: string;
    data?: { url: string; fileName: string }[];
    errors?: string[];
}

export const uploadService = {
    async uploadProduct(file: File): Promise<UploadResult> {
        const formData = new FormData();
        formData.append('file', file);
        const res = await api.post<UploadResult>('/Upload/product', formData, {
            headers: { 'Content-Type': 'multipart/form-data' },
        });
        return res.data;
    },

    async uploadCategory(file: File): Promise<UploadResult> {
        const formData = new FormData();
        formData.append('file', file);
        const res = await api.post<UploadResult>('/Upload/category', formData, {
            headers: { 'Content-Type': 'multipart/form-data' },
        });
        return res.data;
    },

    async uploadBrand(file: File): Promise<UploadResult> {
        const formData = new FormData();
        formData.append('file', file);
        const res = await api.post<UploadResult>('/Upload/brand', formData, {
            headers: { 'Content-Type': 'multipart/form-data' },
        });
        return res.data;
    },

    async uploadAvatar(file: File): Promise<UploadResult> {
        const formData = new FormData();
        formData.append('file', file);
        const res = await api.post<UploadResult>('/Upload/avatar', formData, {
            headers: { 'Content-Type': 'multipart/form-data' },
        });
        return res.data;
    },

    async uploadSlider(file: File): Promise<UploadResult> {
        const formData = new FormData();
        formData.append('file', file);
        const res = await api.post<UploadResult>('/Upload/slider', formData, {
            headers: { 'Content-Type': 'multipart/form-data' },
        });
        return res.data;
    },

    async uploadMultiple(files: File[], bucket: string = 'products', folder?: string): Promise<MultiUploadResult> {
        const formData = new FormData();
        files.forEach((file) => formData.append('files', file));

        let url = `/Upload/multiple?bucket=${bucket}`;
        if (folder) url += `&folder=${folder}`;

        const res = await api.post<MultiUploadResult>(url, formData, {
            headers: { 'Content-Type': 'multipart/form-data' },
        });
        return res.data;
    },

    async deleteFile(filePath: string, bucket: string): Promise<boolean> {
        try {
            await api.delete('/Upload', { params: { filePath, bucket } });
            return true;
        } catch {
            return false;
        }
    },
};
