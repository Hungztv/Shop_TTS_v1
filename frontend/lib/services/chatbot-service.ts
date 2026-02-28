import axios from 'axios';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000/api';

export interface ChatMessage {
    role: 'user' | 'assistant';
    content: string;
    timestamp: Date;
}

export interface ChatBotResponse {
    success: boolean;
    data?: {
        reply: string;
        model: string;
        usage?: {
            prompt_tokens: number;
            completion_tokens: number;
            total_tokens: number;
        };
    };
    message?: string;
}

export async function sendChatMessage(
    message: string,
    history: { role: string; content: string }[]
): Promise<ChatBotResponse> {
    try {
        const res = await axios.post<ChatBotResponse>(`${API_URL}/ChatBot/send`, {
            message,
            history,
        });
        return res.data;
    } catch (error: unknown) {
        if (axios.isAxiosError(error) && error.response?.data) {
            return error.response.data as ChatBotResponse;
        }
        return {
            success: false,
            message: 'Không thể kết nối đến chatbot. Vui lòng thử lại sau.',
        };
    }
}
