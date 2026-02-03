'use client';

import { useEffect, useState } from 'react';
import { Plus, Pencil, Trash2, GripVertical, Image, Eye, EyeOff, ExternalLink } from 'lucide-react';
import AdminHeader from '@/components/admin/AdminHeader';
import Modal, { ConfirmModal } from '@/components/admin/Modal';
import { slidersService, CreateSliderDto } from '@/lib/services/admin/sliders-service';
import { Slider } from '@/lib/services/admin/dashboard-service';

export default function SlidersPage() {
    const [sliders, setSliders] = useState<Slider[]>([]);
    const [loading, setLoading] = useState(true);

    // Modal states
    const [isFormOpen, setIsFormOpen] = useState(false);
    const [isDeleteOpen, setIsDeleteOpen] = useState(false);
    const [selectedSlider, setSelectedSlider] = useState<Slider | null>(null);
    const [formLoading, setFormLoading] = useState(false);

    // Form state
    const [formData, setFormData] = useState<CreateSliderDto>({
        name: '',
        title: '',
        image: '',
        description: '',
        link: '',
        displayOrder: 0,
        status: 1,
    });

    useEffect(() => {
        loadSliders();
    }, []);

    const loadSliders = async () => {
        setLoading(true);
        try {
            const data = await slidersService.getAll();
            setSliders(data.sort((a, b) => a.displayOrder - b.displayOrder));
        } catch (error) {
            console.error('Error loading sliders:', error);
        } finally {
            setLoading(false);
        }
    };

    const openCreateModal = () => {
        setSelectedSlider(null);
        setFormData({
            name: '',
            title: '',
            image: '',
            description: '',
            link: '',
            displayOrder: sliders.length,
            status: 1,
        });
        setIsFormOpen(true);
    };

    const openEditModal = (slider: Slider) => {
        setSelectedSlider(slider);
        setFormData({
            name: slider.name,
            title: slider.title,
            image: slider.image,
            description: slider.description,
            link: slider.link,
            displayOrder: slider.displayOrder,
            status: slider.status,
        });
        setIsFormOpen(true);
    };

    const openDeleteModal = (slider: Slider) => {
        setSelectedSlider(slider);
        setIsDeleteOpen(true);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setFormLoading(true);

        try {
            if (selectedSlider) {
                await slidersService.update(selectedSlider.id, formData);
            } else {
                await slidersService.create(formData);
            }
            setIsFormOpen(false);
            loadSliders();
        } catch (error) {
            console.error('Error saving slider:', error);
            alert('Có lỗi xảy ra!');
        } finally {
            setFormLoading(false);
        }
    };

    const handleDelete = async () => {
        if (!selectedSlider) return;
        setFormLoading(true);

        try {
            await slidersService.delete(selectedSlider.id);
            setIsDeleteOpen(false);
            loadSliders();
        } catch (error) {
            console.error('Error deleting slider:', error);
            alert('Có lỗi xảy ra!');
        } finally {
            setFormLoading(false);
        }
    };

    const toggleStatus = async (slider: Slider) => {
        try {
            await slidersService.update(slider.id, { status: slider.status === 1 ? 0 : 1 });
            loadSliders();
        } catch (error) {
            console.error('Error toggling status:', error);
        }
    };

    const moveSlider = async (index: number, direction: 'up' | 'down') => {
        const newIndex = direction === 'up' ? index - 1 : index + 1;
        if (newIndex < 0 || newIndex >= sliders.length) return;

        const newSliders = [...sliders];
        [newSliders[index], newSliders[newIndex]] = [newSliders[newIndex], newSliders[index]];

        const reorderData = newSliders.map((s, i) => ({ id: s.id, newOrder: i }));

        try {
            await slidersService.reorder({ sliders: reorderData });
            loadSliders();
        } catch (error) {
            console.error('Error reordering:', error);
        }
    };

    return (
        <div className="min-h-screen">
            <AdminHeader title="Sliders" subtitle={`${sliders.length} banner`} />

            <div className="p-6 space-y-6">
                {/* Toolbar */}
                <div className="flex justify-end">
                    <button
                        onClick={openCreateModal}
                        className="flex items-center gap-2 px-4 py-2.5 bg-violet-600 hover:bg-violet-700 text-white rounded-xl font-medium transition-colors"
                    >
                        <Plus className="w-5 h-5" />
                        Thêm slider
                    </button>
                </div>

                {/* Slider List */}
                {loading ? (
                    <div className="space-y-4">
                        {[1, 2, 3].map((i) => (
                            <div key={i} className="bg-white dark:bg-gray-800 rounded-2xl p-4 animate-pulse">
                                <div className="flex gap-4">
                                    <div className="w-48 h-28 bg-gray-200 dark:bg-gray-700 rounded-xl" />
                                    <div className="flex-1 space-y-3">
                                        <div className="h-5 w-48 bg-gray-200 dark:bg-gray-700 rounded" />
                                        <div className="h-4 w-32 bg-gray-200 dark:bg-gray-700 rounded" />
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                ) : sliders.length === 0 ? (
                    <div className="bg-white dark:bg-gray-800 rounded-2xl p-12 text-center">
                        <Image className="w-16 h-16 text-gray-300 mx-auto mb-4" />
                        <p className="text-gray-500">Chưa có slider nào</p>
                    </div>
                ) : (
                    <div className="space-y-4">
                        {sliders.map((slider, index) => (
                            <div
                                key={slider.id}
                                className={`bg-white dark:bg-gray-800 rounded-2xl shadow-sm border border-gray-100 dark:border-gray-700 p-4 flex gap-4 ${slider.status === 0 ? 'opacity-60' : ''
                                    }`}
                            >
                                {/* Drag Handle & Order Controls */}
                                <div className="flex flex-col items-center gap-1">
                                    <button
                                        onClick={() => moveSlider(index, 'up')}
                                        disabled={index === 0}
                                        className="p-1 rounded hover:bg-gray-100 dark:hover:bg-gray-700 disabled:opacity-30"
                                    >
                                        ▲
                                    </button>
                                    <GripVertical className="w-5 h-5 text-gray-400" />
                                    <button
                                        onClick={() => moveSlider(index, 'down')}
                                        disabled={index === sliders.length - 1}
                                        className="p-1 rounded hover:bg-gray-100 dark:hover:bg-gray-700 disabled:opacity-30"
                                    >
                                        ▼
                                    </button>
                                </div>

                                {/* Image */}
                                <div className="w-48 h-28 rounded-xl overflow-hidden bg-gray-100 dark:bg-gray-700 flex-shrink-0">
                                    {slider.image ? (
                                        <img
                                            src={slider.image}
                                            alt={slider.name}
                                            className="w-full h-full object-cover"
                                        />
                                    ) : (
                                        <div className="w-full h-full flex items-center justify-center">
                                            <Image className="w-8 h-8 text-gray-400" />
                                        </div>
                                    )}
                                </div>

                                {/* Info */}
                                <div className="flex-1 min-w-0">
                                    <div className="flex items-start justify-between gap-4">
                                        <div>
                                            <h3 className="font-semibold text-gray-900 dark:text-white">
                                                {slider.title || slider.name}
                                            </h3>
                                            <p className="text-sm text-gray-500 mt-1">{slider.description}</p>
                                            {slider.link && (
                                                <a
                                                    href={slider.link}
                                                    target="_blank"
                                                    rel="noreferrer"
                                                    className="inline-flex items-center gap-1 text-sm text-violet-600 hover:underline mt-2"
                                                >
                                                    <ExternalLink className="w-3 h-3" />
                                                    {slider.link}
                                                </a>
                                            )}
                                        </div>

                                        {/* Actions */}
                                        <div className="flex items-center gap-2">
                                            <button
                                                onClick={() => toggleStatus(slider)}
                                                className={`p-2 rounded-lg transition-colors ${slider.status === 1
                                                        ? 'bg-green-100 text-green-600 hover:bg-green-200'
                                                        : 'bg-gray-100 text-gray-400 hover:bg-gray-200'
                                                    }`}
                                                title={slider.status === 1 ? 'Đang hiển thị' : 'Đang ẩn'}
                                            >
                                                {slider.status === 1 ? <Eye className="w-4 h-4" /> : <EyeOff className="w-4 h-4" />}
                                            </button>
                                            <button
                                                onClick={() => openEditModal(slider)}
                                                className="p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 text-blue-600"
                                            >
                                                <Pencil className="w-4 h-4" />
                                            </button>
                                            <button
                                                onClick={() => openDeleteModal(slider)}
                                                className="p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 text-red-600"
                                            >
                                                <Trash2 className="w-4 h-4" />
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>

            {/* Create/Edit Modal */}
            <Modal
                isOpen={isFormOpen}
                onClose={() => setIsFormOpen(false)}
                title={selectedSlider ? 'Sửa slider' : 'Thêm slider mới'}
                footer={
                    <div className="flex justify-end gap-3">
                        <button onClick={() => setIsFormOpen(false)} className="px-4 py-2 text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 rounded-xl">
                            Hủy
                        </button>
                        <button
                            onClick={handleSubmit}
                            disabled={formLoading || !formData.name || !formData.image}
                            className="px-4 py-2 bg-violet-600 hover:bg-violet-700 text-white rounded-xl font-medium disabled:opacity-50 flex items-center gap-2"
                        >
                            {formLoading && <div className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin" />}
                            {selectedSlider ? 'Cập nhật' : 'Tạo mới'}
                        </button>
                    </div>
                }
            >
                <form className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium mb-1">Tên slider *</label>
                        <input
                            type="text"
                            value={formData.name}
                            onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                            className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800"
                            required
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-medium mb-1">Tiêu đề</label>
                        <input
                            type="text"
                            value={formData.title}
                            onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                            className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800"
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-medium mb-1">URL hình ảnh *</label>
                        <input
                            type="text"
                            value={formData.image}
                            onChange={(e) => setFormData({ ...formData, image: e.target.value })}
                            className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800"
                            placeholder="https://example.com/image.jpg"
                            required
                        />
                        {formData.image && (
                            <img src={formData.image} alt="Preview" className="mt-2 h-32 rounded-lg object-cover" />
                        )}
                    </div>
                    <div>
                        <label className="block text-sm font-medium mb-1">Link điều hướng</label>
                        <input
                            type="text"
                            value={formData.link}
                            onChange={(e) => setFormData({ ...formData, link: e.target.value })}
                            className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800"
                            placeholder="https://example.com/products"
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-medium mb-1">Mô tả</label>
                        <textarea
                            value={formData.description}
                            onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                            className="w-full px-4 py-2.5 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800 resize-none"
                            rows={2}
                        />
                    </div>
                </form>
            </Modal>

            {/* Delete Modal */}
            <ConfirmModal
                isOpen={isDeleteOpen}
                onClose={() => setIsDeleteOpen(false)}
                onConfirm={handleDelete}
                title="Xóa slider"
                message={`Bạn có chắc chắn muốn xóa slider "${selectedSlider?.name}"?`}
                confirmText="Xóa"
                loading={formLoading}
            />
        </div>
    );
}
