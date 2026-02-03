'use client';

import { useEffect, useState } from 'react';
import { Search, Mail, MailOpen, Trash2, Reply, Clock, User, Send } from 'lucide-react';
import AdminHeader from '@/components/admin/AdminHeader';
import DataTable from '@/components/admin/DataTable';
import Modal, { ConfirmModal } from '@/components/admin/Modal';
import { messagesService, ReplyMessageDto } from '@/lib/services/admin/messages-service';
import { ContactMessage } from '@/lib/services/admin/dashboard-service';

export default function MessagesPage() {
    const [messages, setMessages] = useState<ContactMessage[]>([]);
    const [loading, setLoading] = useState(true);
    const [totalCount, setTotalCount] = useState(0);
    const [unreadCount, setUnreadCount] = useState(0);

    // Filters
    const [page, setPage] = useState(1);
    const [readFilter, setReadFilter] = useState<boolean | undefined>();

    // Modal states
    const [selectedMessage, setSelectedMessage] = useState<ContactMessage | null>(null);
    const [isDetailOpen, setIsDetailOpen] = useState(false);
    const [isReplyOpen, setIsReplyOpen] = useState(false);
    const [isDeleteOpen, setIsDeleteOpen] = useState(false);
    const [formLoading, setFormLoading] = useState(false);
    const [replyText, setReplyText] = useState('');

    useEffect(() => {
        loadMessages();
        loadUnreadCount();
    }, [page, readFilter]);

    const loadMessages = async () => {
        setLoading(true);
        try {
            const res = await messagesService.getAll({
                page,
                pageSize: 10,
                isRead: readFilter,
            });
            setMessages(res.items);
            setTotalCount(res.totalCount);
        } catch (error) {
            console.error('Error loading messages:', error);
        } finally {
            setLoading(false);
        }
    };

    const loadUnreadCount = async () => {
        const count = await messagesService.getUnreadCount();
        setUnreadCount(count);
    };

    const openDetailModal = async (message: ContactMessage) => {
        setSelectedMessage(message);
        setIsDetailOpen(true);

        if (!message.isRead) {
            await messagesService.markAsRead(message.id);
            loadMessages();
            loadUnreadCount();
        }
    };

    const openReplyModal = (message: ContactMessage) => {
        setSelectedMessage(message);
        setReplyText('');
        setIsReplyOpen(true);
    };

    const openDeleteModal = (message: ContactMessage) => {
        setSelectedMessage(message);
        setIsDeleteOpen(true);
    };

    const handleReply = async () => {
        if (!selectedMessage || !replyText.trim()) return;
        setFormLoading(true);

        try {
            await messagesService.reply(selectedMessage.id, { replyMessage: replyText });
            setIsReplyOpen(false);
            loadMessages();
            alert('Đã gửi phản hồi thành công!');
        } catch (error) {
            console.error('Error replying:', error);
            alert('Có lỗi xảy ra!');
        } finally {
            setFormLoading(false);
        }
    };

    const handleDelete = async () => {
        if (!selectedMessage) return;
        setFormLoading(true);

        try {
            await messagesService.delete(selectedMessage.id);
            setIsDeleteOpen(false);
            loadMessages();
            loadUnreadCount();
        } catch (error) {
            console.error('Error deleting:', error);
            alert('Có lỗi xảy ra!');
        } finally {
            setFormLoading(false);
        }
    };

    const formatDate = (dateStr: string) => {
        return new Date(dateStr).toLocaleString('vi-VN', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
        });
    };

    const columns = [
        {
            key: 'status',
            header: '',
            width: '50px',
            render: (item: ContactMessage) => (
                <div className={`w-3 h-3 rounded-full ${item.isRead ? 'bg-gray-300' : 'bg-blue-500'}`} />
            ),
        },
        {
            key: 'sender',
            header: 'Người gửi',
            render: (item: ContactMessage) => (
                <div className="flex items-center gap-3">
                    <div className="w-10 h-10 rounded-full bg-gradient-to-br from-violet-500 to-purple-500 flex items-center justify-center text-white font-bold">
                        {item.name?.[0]?.toUpperCase() || 'U'}
                    </div>
                    <div>
                        <p className={`font-medium ${!item.isRead ? 'text-gray-900 dark:text-white' : 'text-gray-600 dark:text-gray-400'}`}>
                            {item.name}
                        </p>
                        <p className="text-sm text-gray-500">{item.email}</p>
                    </div>
                </div>
            ),
        },
        {
            key: 'subject',
            header: 'Tiêu đề',
            render: (item: ContactMessage) => (
                <div>
                    <p className={`font-medium ${!item.isRead ? 'text-gray-900 dark:text-white' : 'text-gray-600 dark:text-gray-400'}`}>
                        {item.subject}
                    </p>
                    <p className="text-sm text-gray-500 truncate max-w-xs">{item.message}</p>
                </div>
            ),
        },
        {
            key: 'replied',
            header: 'Trạng thái',
            render: (item: ContactMessage) => (
                <span className={`inline-flex items-center gap-1 px-2.5 py-1 text-xs font-medium rounded-full ${item.repliedAt
                        ? 'bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400'
                        : !item.isRead
                            ? 'bg-blue-100 text-blue-700 dark:bg-blue-900/30 dark:text-blue-400'
                            : 'bg-gray-100 text-gray-700 dark:bg-gray-700 dark:text-gray-400'
                    }`}>
                    {item.repliedAt ? (
                        <><Reply className="w-3 h-3" /> Đã trả lời</>
                    ) : !item.isRead ? (
                        <><Mail className="w-3 h-3" /> Mới</>
                    ) : (
                        <><MailOpen className="w-3 h-3" /> Đã đọc</>
                    )}
                </span>
            ),
        },
        {
            key: 'createdAt',
            header: 'Thời gian',
            render: (item: ContactMessage) => (
                <span className="text-sm text-gray-500">{formatDate(item.createdAt)}</span>
            ),
        },
        {
            key: 'actions',
            header: '',
            render: (item: ContactMessage) => (
                <div className="flex items-center gap-1">
                    <button
                        onClick={(e) => { e.stopPropagation(); openReplyModal(item); }}
                        className="p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 text-blue-600"
                        title="Trả lời"
                    >
                        <Reply className="w-4 h-4" />
                    </button>
                    <button
                        onClick={(e) => { e.stopPropagation(); openDeleteModal(item); }}
                        className="p-2 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 text-red-600"
                        title="Xóa"
                    >
                        <Trash2 className="w-4 h-4" />
                    </button>
                </div>
            ),
        },
    ];

    return (
        <div className="min-h-screen">
            <AdminHeader
                title="Tin nhắn liên hệ"
                subtitle={`${totalCount} tin nhắn${unreadCount > 0 ? ` (${unreadCount} chưa đọc)` : ''}`}
            />

            <div className="p-6 space-y-6">
                {/* Filter Tabs */}
                <div className="flex gap-2">
                    <button
                        onClick={() => setReadFilter(undefined)}
                        className={`px-4 py-2 rounded-xl font-medium transition-colors ${readFilter === undefined
                                ? 'bg-violet-600 text-white'
                                : 'bg-gray-100 dark:bg-gray-800 text-gray-700 dark:text-gray-300 hover:bg-gray-200 dark:hover:bg-gray-700'
                            }`}
                    >
                        Tất cả ({totalCount})
                    </button>
                    <button
                        onClick={() => setReadFilter(false)}
                        className={`px-4 py-2 rounded-xl font-medium transition-colors ${readFilter === false
                                ? 'bg-violet-600 text-white'
                                : 'bg-gray-100 dark:bg-gray-800 text-gray-700 dark:text-gray-300 hover:bg-gray-200 dark:hover:bg-gray-700'
                            }`}
                    >
                        Chưa đọc ({unreadCount})
                    </button>
                    <button
                        onClick={() => setReadFilter(true)}
                        className={`px-4 py-2 rounded-xl font-medium transition-colors ${readFilter === true
                                ? 'bg-violet-600 text-white'
                                : 'bg-gray-100 dark:bg-gray-800 text-gray-700 dark:text-gray-300 hover:bg-gray-200 dark:hover:bg-gray-700'
                            }`}
                    >
                        Đã đọc
                    </button>
                </div>

                {/* Table */}
                <DataTable
                    columns={columns}
                    data={messages}
                    loading={loading}
                    page={page}
                    pageSize={10}
                    totalCount={totalCount}
                    onPageChange={setPage}
                    onRowClick={openDetailModal}
                    keyExtractor={(item) => item.id}
                    emptyMessage="Chưa có tin nhắn nào"
                />
            </div>

            {/* Detail Modal */}
            <Modal
                isOpen={isDetailOpen}
                onClose={() => setIsDetailOpen(false)}
                title="Chi tiết tin nhắn"
                size="lg"
                footer={
                    <div className="flex justify-end gap-3">
                        <button onClick={() => setIsDetailOpen(false)} className="px-4 py-2 text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 rounded-xl">
                            Đóng
                        </button>
                        <button
                            onClick={() => { setIsDetailOpen(false); openReplyModal(selectedMessage!); }}
                            className="px-4 py-2 bg-violet-600 hover:bg-violet-700 text-white rounded-xl font-medium flex items-center gap-2"
                        >
                            <Reply className="w-4 h-4" />
                            Trả lời
                        </button>
                    </div>
                }
            >
                {selectedMessage && (
                    <div className="space-y-6">
                        {/* Sender Info */}
                        <div className="flex items-center gap-4 p-4 bg-gray-50 dark:bg-gray-700/50 rounded-xl">
                            <div className="w-12 h-12 rounded-full bg-gradient-to-br from-violet-500 to-purple-500 flex items-center justify-center text-white text-xl font-bold">
                                {selectedMessage.name?.[0]?.toUpperCase() || 'U'}
                            </div>
                            <div className="flex-1">
                                <p className="font-semibold text-gray-900 dark:text-white">{selectedMessage.name}</p>
                                <p className="text-sm text-gray-500">{selectedMessage.email}</p>
                                {selectedMessage.phone && <p className="text-sm text-gray-500">{selectedMessage.phone}</p>}
                            </div>
                            <div className="text-right text-sm text-gray-500">
                                <p>{formatDate(selectedMessage.createdAt)}</p>
                            </div>
                        </div>

                        {/* Subject */}
                        <div>
                            <h4 className="font-semibold text-lg">{selectedMessage.subject}</h4>
                        </div>

                        {/* Message */}
                        <div className="p-4 bg-gray-50 dark:bg-gray-700/50 rounded-xl">
                            <p className="whitespace-pre-wrap">{selectedMessage.message}</p>
                        </div>

                        {/* Reply if exists */}
                        {selectedMessage.repliedAt && selectedMessage.replyMessage && (
                            <div className="border-t border-gray-200 dark:border-gray-700 pt-4">
                                <div className="flex items-center gap-2 text-green-600 mb-3">
                                    <Reply className="w-4 h-4" />
                                    <span className="font-medium">Đã trả lời lúc {formatDate(selectedMessage.repliedAt)}</span>
                                </div>
                                <div className="p-4 bg-green-50 dark:bg-green-900/20 rounded-xl border-l-4 border-green-500">
                                    <p className="whitespace-pre-wrap">{selectedMessage.replyMessage}</p>
                                </div>
                            </div>
                        )}
                    </div>
                )}
            </Modal>

            {/* Reply Modal */}
            <Modal
                isOpen={isReplyOpen}
                onClose={() => setIsReplyOpen(false)}
                title={`Trả lời: ${selectedMessage?.subject}`}
                footer={
                    <div className="flex justify-end gap-3">
                        <button onClick={() => setIsReplyOpen(false)} className="px-4 py-2 text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 rounded-xl">
                            Hủy
                        </button>
                        <button
                            onClick={handleReply}
                            disabled={formLoading || !replyText.trim()}
                            className="px-4 py-2 bg-violet-600 hover:bg-violet-700 text-white rounded-xl font-medium disabled:opacity-50 flex items-center gap-2"
                        >
                            {formLoading && <div className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin" />}
                            <Send className="w-4 h-4" />
                            Gửi
                        </button>
                    </div>
                }
            >
                <div className="space-y-4">
                    <div className="p-3 bg-gray-50 dark:bg-gray-700/50 rounded-xl text-sm">
                        <p className="text-gray-500">Gửi đến: <span className="font-medium text-gray-900 dark:text-white">{selectedMessage?.email}</span></p>
                    </div>
                    <textarea
                        value={replyText}
                        onChange={(e) => setReplyText(e.target.value)}
                        className="w-full px-4 py-3 border border-gray-200 dark:border-gray-700 rounded-xl bg-white dark:bg-gray-800 focus:ring-2 focus:ring-violet-500 outline-none resize-none"
                        rows={6}
                        placeholder="Nhập nội dung phản hồi..."
                    />
                </div>
            </Modal>

            {/* Delete Modal */}
            <ConfirmModal
                isOpen={isDeleteOpen}
                onClose={() => setIsDeleteOpen(false)}
                onConfirm={handleDelete}
                title="Xóa tin nhắn"
                message={`Bạn có chắc chắn muốn xóa tin nhắn từ "${selectedMessage?.name}"?`}
                confirmText="Xóa"
                loading={formLoading}
            />
        </div>
    );
}
