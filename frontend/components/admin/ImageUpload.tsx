'use client';

import { useState, useRef } from 'react';
import { Upload, X, Loader2, Image as ImageIcon } from 'lucide-react';
import { uploadService, UploadResult } from '@/lib/services/admin/upload-service';

interface ImageUploadProps {
    value?: string;
    onChange: (url: string) => void;
    type: 'product' | 'category' | 'brand' | 'avatar';
    className?: string;
    placeholder?: string;
}

export default function ImageUpload({
    value,
    onChange,
    type,
    className = '',
    placeholder = 'Chọn hoặc kéo thả ảnh',
}: ImageUploadProps) {
    const [uploading, setUploading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [dragActive, setDragActive] = useState(false);
    const inputRef = useRef<HTMLInputElement>(null);

    const handleUpload = async (file: File) => {
        if (!file) return;

        // Validate file type
        if (!file.type.startsWith('image/')) {
            setError('Chỉ chấp nhận file ảnh');
            return;
        }

        // Validate file size (max 5MB)
        if (file.size > 5 * 1024 * 1024) {
            setError('File không được vượt quá 5MB');
            return;
        }

        setUploading(true);
        setError(null);

        try {
            let result: UploadResult;
            switch (type) {
                case 'product':
                    result = await uploadService.uploadProduct(file);
                    break;
                case 'category':
                    result = await uploadService.uploadCategory(file);
                    break;
                case 'brand':
                    result = await uploadService.uploadBrand(file);
                    break;
                case 'avatar':
                    result = await uploadService.uploadAvatar(file);
                    break;
            }

            if (result.success && result.data?.url) {
                onChange(result.data.url);
            } else {
                setError(result.message || 'Upload thất bại');
            }
        } catch (err) {
            console.error('Upload error:', err);
            setError('Có lỗi xảy ra khi upload');
        } finally {
            setUploading(false);
        }
    };

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (file) handleUpload(file);
    };

    const handleDrag = (e: React.DragEvent) => {
        e.preventDefault();
        e.stopPropagation();
        if (e.type === 'dragenter' || e.type === 'dragover') {
            setDragActive(true);
        } else if (e.type === 'dragleave') {
            setDragActive(false);
        }
    };

    const handleDrop = (e: React.DragEvent) => {
        e.preventDefault();
        e.stopPropagation();
        setDragActive(false);

        if (e.dataTransfer.files && e.dataTransfer.files[0]) {
            handleUpload(e.dataTransfer.files[0]);
        }
    };

    const handleClear = () => {
        onChange('');
        if (inputRef.current) inputRef.current.value = '';
    };

    return (
        <div className={className}>
            <input
                ref={inputRef}
                type="file"
                accept="image/*"
                onChange={handleFileChange}
                className="hidden"
            />

            {value ? (
                <div className="relative group">
                    <img
                        src={value}
                        alt="Preview"
                        className="w-full h-40 object-cover rounded-xl border border-gray-200 dark:border-gray-700"
                    />
                    <div className="absolute inset-0 bg-black/50 opacity-0 group-hover:opacity-100 transition-opacity rounded-xl flex items-center justify-center gap-2">
                        <button
                            type="button"
                            onClick={() => inputRef.current?.click()}
                            className="p-2 bg-white/20 hover:bg-white/30 rounded-lg text-white"
                        >
                            <Upload className="w-5 h-5" />
                        </button>
                        <button
                            type="button"
                            onClick={handleClear}
                            className="p-2 bg-red-500/80 hover:bg-red-500 rounded-lg text-white"
                        >
                            <X className="w-5 h-5" />
                        </button>
                    </div>
                </div>
            ) : (
                <div
                    onClick={() => !uploading && inputRef.current?.click()}
                    onDragEnter={handleDrag}
                    onDragLeave={handleDrag}
                    onDragOver={handleDrag}
                    onDrop={handleDrop}
                    className={`
                        w-full h-40 border-2 border-dashed rounded-xl flex flex-col items-center justify-center cursor-pointer transition-colors
                        ${dragActive
                            ? 'border-violet-500 bg-violet-50 dark:bg-violet-900/20'
                            : 'border-gray-300 dark:border-gray-600 hover:border-violet-400 hover:bg-violet-50/50 dark:hover:bg-violet-900/10'
                        }
                        ${uploading ? 'pointer-events-none opacity-60' : ''}
                    `}
                >
                    {uploading ? (
                        <>
                            <Loader2 className="w-8 h-8 text-violet-500 animate-spin" />
                            <p className="mt-2 text-sm text-gray-500">Đang upload...</p>
                        </>
                    ) : (
                        <>
                            <ImageIcon className="w-8 h-8 text-gray-400" />
                            <p className="mt-2 text-sm text-gray-500">{placeholder}</p>
                            <p className="text-xs text-gray-400 mt-1">PNG, JPG, GIF (max 5MB)</p>
                        </>
                    )}
                </div>
            )}

            {error && (
                <p className="text-sm text-red-500 mt-2">{error}</p>
            )}
        </div>
    );
}
