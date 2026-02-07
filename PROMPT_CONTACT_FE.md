# Prompt FE Contact (Next.js 16, TS strict, Tailwind v4)

Xây dựng trang Contact cho người dùng public, kết nối API backend.

## 1) API & Payload

- Tạo contact (public): `POST /api/contact-messages`
- Payload:
  ```json
  {
    "name": "string",
    "email": "string",
    "phone": "string|null",
    "subject": "string",
    "message": "string"
  }
  ```
- Response theo chuẩn backend: `{ success: boolean, message?: string, data: ... }`

## 2) UI Requirements (nhất quán với project)

- Bố cục 2 cột (desktop), 1 cột (mobile):
  - Trái: Form liên hệ
  - Phải: thông tin cửa hàng (địa chỉ, hotline, email, giờ làm việc, map)
- Sử dụng style đã có (glass/gradient, rounded-xl/2xl, shadow, hover).
- Nút submit có trạng thái loading, disable khi submit.
- Toast/alert sau khi gửi thành công hoặc lỗi.

## 3) Form Fields

- Name (bắt buộc)
- Email (bắt buộc, format email)
- Phone (tùy chọn, validate cơ bản)
- Subject (bắt buộc)
- Message (bắt buộc, min 10 ký tự)

## 4) Validation

- Dùng `react-hook-form` + `zod`.
- Hiển thị lỗi dưới từng field.

## 5) Data Flow

- Gửi form bằng axios client đã dùng trong app.
- Nếu backend trả lỗi, hiển thị message.
- Sau success: reset form + hiển thị thông báo.

## 6) Mobile First

- Tối ưu padding, font, spacing trên mobile.
- Buttons full-width trên mobile.

## 7) Gợi ý xử lý lỗi API

- Nếu muốn chuẩn hóa lỗi, có thể thêm interceptors trong `lib/api-client.ts` để map lỗi `response.data.message` và hiển thị toast.

## 8) Backend đã có

- Controller: ContactMessagesController
- Endpoint: `POST /api/contact-messages`
- DTO: CreateContactMessageDto { Name, Email, Phone?, Subject, Message }
