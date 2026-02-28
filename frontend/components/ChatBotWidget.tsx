'use client';

import { useState, useRef, useEffect, useCallback } from 'react';
import { MessageCircle, X, Send, Loader2, Bot, User, Trash2 } from 'lucide-react';
import { sendChatMessage, type ChatMessage } from '@/lib/services/chatbot-service';

const WELCOME_MESSAGE: ChatMessage = {
    role: 'assistant',
    content: 'Xin chào! 👋 Tôi là trợ lý AI của ShopTTS. Tôi có thể giúp bạn:\n\n• Tìm kiếm sản phẩm\n• Hỗ trợ đặt hàng & thanh toán\n• Tra cứu đơn hàng\n• Giải đáp thắc mắc\n\nBạn cần hỗ trợ gì?',
    timestamp: new Date(),
};

export default function ChatBotWidget() {
    const [isOpen, setIsOpen] = useState(false);
    const [messages, setMessages] = useState<ChatMessage[]>([WELCOME_MESSAGE]);
    const [input, setInput] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [hasUnread, setHasUnread] = useState(false);

    const messagesEndRef = useRef<HTMLDivElement>(null);
    const inputRef = useRef<HTMLTextAreaElement>(null);

    // Auto-scroll khi có tin nhắn mới
    const scrollToBottom = useCallback(() => {
        messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
    }, []);

    useEffect(() => {
        scrollToBottom();
    }, [messages, scrollToBottom]);

    // Focus input khi mở chat
    useEffect(() => {
        if (isOpen) {
            setHasUnread(false);
            setTimeout(() => inputRef.current?.focus(), 100);
        }
    }, [isOpen]);

    const handleSend = async () => {
        const trimmed = input.trim();
        if (!trimmed || isLoading) return;

        const userMessage: ChatMessage = {
            role: 'user',
            content: trimmed,
            timestamp: new Date(),
        };

        setMessages(prev => [...prev, userMessage]);
        setInput('');
        setIsLoading(true);

        // Chuẩn bị history (bỏ tin welcome, chỉ lấy 10 tin cuối)
        const history = messages
            .filter((_, i) => i > 0) // bỏ welcome
            .slice(-10)
            .map(m => ({ role: m.role, content: m.content }));

        const result = await sendChatMessage(trimmed, history);

        const botMessage: ChatMessage = {
            role: 'assistant',
            content: result.success && result.data
                ? result.data.reply
                : (result.message || 'Xin lỗi, tôi gặp sự cố. Vui lòng thử lại sau. 😔'),
            timestamp: new Date(),
        };

        setMessages(prev => [...prev, botMessage]);
        setIsLoading(false);

        // Đánh dấu unread nếu chat đang đóng
        if (!isOpen) setHasUnread(true);
    };

    const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            handleSend();
        }
    };

    const clearChat = () => {
        setMessages([WELCOME_MESSAGE]);
    };

    const formatTime = (date: Date) => {
        return new Date(date).toLocaleTimeString('vi-VN', {
            hour: '2-digit',
            minute: '2-digit',
        });
    };

    return (
        <>
            {/* Chat Button */}
            <button
                onClick={() => setIsOpen(!isOpen)}
                className={`fixed bottom-6 right-6 z-50 w-14 h-14 rounded-full shadow-lg flex items-center justify-center transition-all duration-300 hover:scale-110 ${
                    isOpen
                        ? 'bg-gray-600 hover:bg-gray-700'
                        : 'bg-gradient-to-r from-emerald-500 to-teal-600 hover:from-emerald-600 hover:to-teal-700'
                }`}
                aria-label={isOpen ? 'Đóng chat' : 'Mở chat hỗ trợ'}
            >
                {isOpen ? (
                    <X className="w-6 h-6 text-white" />
                ) : (
                    <>
                        <MessageCircle className="w-6 h-6 text-white" />
                        {hasUnread && (
                            <span className="absolute -top-1 -right-1 w-4 h-4 bg-red-500 rounded-full animate-pulse" />
                        )}
                    </>
                )}
            </button>

            {/* Chat Window */}
            <div
                className={`fixed bottom-24 right-6 z-50 w-[380px] max-w-[calc(100vw-2rem)] transition-all duration-300 origin-bottom-right ${
                    isOpen
                        ? 'scale-100 opacity-100 pointer-events-auto'
                        : 'scale-75 opacity-0 pointer-events-none'
                }`}
            >
                <div className="bg-white dark:bg-gray-900 rounded-2xl shadow-2xl border border-gray-200 dark:border-gray-700 flex flex-col h-[520px] max-h-[70vh] overflow-hidden">
                    {/* Header */}
                    <div className="bg-gradient-to-r from-emerald-500 to-teal-600 px-4 py-3 flex items-center gap-3 rounded-t-2xl shrink-0">
                        <div className="w-9 h-9 bg-white/20 rounded-full flex items-center justify-center">
                            <Bot className="w-5 h-5 text-white" />
                        </div>
                        <div className="flex-1">
                            <h3 className="text-white font-semibold text-sm">ShopTTS AI Assistant</h3>
                            <div className="flex items-center gap-1.5">
                                <span className="w-2 h-2 bg-green-300 rounded-full animate-pulse" />
                                <span className="text-white/80 text-xs">Đang hoạt động</span>
                            </div>
                        </div>
                        <button
                            onClick={clearChat}
                            className="text-white/70 hover:text-white transition-colors p-1"
                            title="Xóa lịch sử chat"
                        >
                            <Trash2 className="w-4 h-4" />
                        </button>
                    </div>

                    {/* Messages */}
                    <div className="flex-1 overflow-y-auto p-4 space-y-4 scrollbar-thin">
                        {messages.map((msg, idx) => (
                            <div
                                key={idx}
                                className={`flex gap-2.5 ${msg.role === 'user' ? 'flex-row-reverse' : 'flex-row'}`}
                            >
                                {/* Avatar */}
                                <div
                                    className={`w-7 h-7 rounded-full flex items-center justify-center shrink-0 ${
                                        msg.role === 'user'
                                            ? 'bg-emerald-100 dark:bg-emerald-900/50'
                                            : 'bg-gray-100 dark:bg-gray-800'
                                    }`}
                                >
                                    {msg.role === 'user' ? (
                                        <User className="w-3.5 h-3.5 text-emerald-600 dark:text-emerald-400" />
                                    ) : (
                                        <Bot className="w-3.5 h-3.5 text-gray-600 dark:text-gray-400" />
                                    )}
                                </div>

                                {/* Bubble */}
                                <div
                                    className={`max-w-[75%] rounded-2xl px-3.5 py-2.5 text-sm leading-relaxed ${
                                        msg.role === 'user'
                                            ? 'bg-emerald-500 text-white rounded-br-md'
                                            : 'bg-gray-100 dark:bg-gray-800 text-gray-800 dark:text-gray-200 rounded-bl-md'
                                    }`}
                                >
                                    <p className="whitespace-pre-wrap break-words">{msg.content}</p>
                                    <span
                                        className={`block text-[10px] mt-1 ${
                                            msg.role === 'user' ? 'text-white/60' : 'text-gray-400'
                                        }`}
                                    >
                                        {formatTime(msg.timestamp)}
                                    </span>
                                </div>
                            </div>
                        ))}

                        {/* Typing indicator */}
                        {isLoading && (
                            <div className="flex gap-2.5">
                                <div className="w-7 h-7 rounded-full bg-gray-100 dark:bg-gray-800 flex items-center justify-center">
                                    <Bot className="w-3.5 h-3.5 text-gray-600 dark:text-gray-400" />
                                </div>
                                <div className="bg-gray-100 dark:bg-gray-800 rounded-2xl rounded-bl-md px-4 py-3">
                                    <div className="flex gap-1">
                                        <span className="w-2 h-2 bg-gray-400 rounded-full animate-bounce" style={{ animationDelay: '0ms' }} />
                                        <span className="w-2 h-2 bg-gray-400 rounded-full animate-bounce" style={{ animationDelay: '150ms' }} />
                                        <span className="w-2 h-2 bg-gray-400 rounded-full animate-bounce" style={{ animationDelay: '300ms' }} />
                                    </div>
                                </div>
                            </div>
                        )}

                        <div ref={messagesEndRef} />
                    </div>

                    {/* Input */}
                    <div className="border-t border-gray-200 dark:border-gray-700 p-3 shrink-0">
                        <div className="flex items-end gap-2">
                            <textarea
                                ref={inputRef}
                                value={input}
                                onChange={e => setInput(e.target.value)}
                                onKeyDown={handleKeyDown}
                                placeholder="Nhập tin nhắn..."
                                rows={1}
                                className="flex-1 resize-none rounded-xl border border-gray-300 dark:border-gray-600 bg-gray-50 dark:bg-gray-800 px-3 py-2.5 text-sm text-gray-900 dark:text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent max-h-24 overflow-y-auto"
                                style={{ minHeight: '40px' }}
                                disabled={isLoading}
                            />
                            <button
                                onClick={handleSend}
                                disabled={!input.trim() || isLoading}
                                className="w-10 h-10 rounded-xl bg-emerald-500 hover:bg-emerald-600 disabled:bg-gray-300 dark:disabled:bg-gray-700 text-white flex items-center justify-center transition-colors shrink-0"
                            >
                                {isLoading ? (
                                    <Loader2 className="w-4 h-4 animate-spin" />
                                ) : (
                                    <Send className="w-4 h-4" />
                                )}
                            </button>
                        </div>
                        <p className="text-[10px] text-gray-400 mt-1.5 text-center">
                            Powered by AI • Phản hồi có thể không chính xác 100%
                        </p>
                    </div>
                </div>
            </div>
        </>
    );
}
