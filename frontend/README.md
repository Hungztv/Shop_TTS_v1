# 🛍️ ShopX Frontend - Next.js 16

## 📋 Mục lục
- [Tổng quan](#tổng-quan)
- [Cấu trúc thư mục](#cấu-trúc-thư-mục)
- [Bắt đầu](#bắt-đầu)
- [Các thành phần chính](#các-thành-phần-chính)
- [Hướng dẫn phát triển](#hướng-dẫn-phát-triển)
- [Kết nối Backend API](#kết-nối-backend-api)
- [Tài nguyên học tập](#tài-nguyên-học-tập)

---

## 🎯 Tổng quan

| Công nghệ | Phiên bản | Mục đích |
|-----------|-----------|----------|
| Next.js | 16.1.5 | Framework React với SSR/SSG |
| React | 19.2.3 | UI Library |
| TypeScript | 5.x | Type safety |
| Tailwind CSS | 4.x | Styling |
| Lucide React | 0.563 | Icons |
| Axios | 1.13 | HTTP Client |

---

## 📁 Cấu trúc thư mục

```
frontend/
├── app/                       # 🔹 ROUTES - Các trang của web
│   ├── layout.tsx             # Layout chung (Header + Footer)
│   ├── page.tsx               # Trang chủ (/)
│   ├── globals.css            # CSS toàn cục
│   └── [folder]/page.tsx      # Thêm route mới
│
├── components/                # 🔹 COMPONENTS - React components
│   ├── layout/                # Header, Footer
│   ├── ui/                    # Button, Card, Modal...
│   └── theme-provider.tsx     # Dark mode
│
├── lib/                       # 🔹 UTILITIES
│   ├── api-client.ts          # Axios instance
│   └── utils.ts               # Helper functions
│
└── public/                    # 🔹 STATIC FILES (images, icons)
```

---

## 🚀 Bắt đầu

### Cài đặt
```bash
cd frontend
npm install
```

### Chạy development server
```bash
npm run dev
```
Mở http://localhost:3000

### Build production
```bash
npm run build
npm run start
```

---

## 🧩 Các thành phần chính

### 1. Layout (`app/layout.tsx`)
Wrap toàn bộ app với Header, Footer, fonts, và theme provider.

### 2. Trang chủ (`app/page.tsx`)
Gồm: HeroBanner, CategoryNav, ProductCard grid.

### 3. Components quan trọng

| Component | Vị trí | Chức năng |
|-----------|--------|-----------|
| Header | `components/layout/` | Navigation, search, cart, user menu |
| Footer | `components/layout/` | Links, contact, copyright |
| ProductCard | `components/ui/` | Hiển thị sản phẩm (hover effects, add to cart) |
| HeroBanner | `components/ui/` | Banner quảng cáo trang chủ |
| Button | `components/ui/` | Button với nhiều variants |

---

## 📝 Hướng dẫn phát triển

### Thêm trang mới

```bash
# Tạo route /products
mkdir app/products
```

```tsx
// app/products/page.tsx
export default function ProductsPage() {
  return (
    <div className="container mx-auto p-4">
      <h1>Sản phẩm</h1>
    </div>
  );
}
```

### Thêm component mới

```tsx
// components/ui/Modal.tsx
interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  children: React.ReactNode;
}

export function Modal({ isOpen, onClose, children }: ModalProps) {
  if (!isOpen) return null;
  
  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center">
      <div className="bg-white rounded-lg p-6">
        {children}
        <button onClick={onClose}>Đóng</button>
      </div>
    </div>
  );
}
```

### Sử dụng Tailwind CSS

```tsx
// Ví dụ styling với Tailwind
<div className="bg-gradient-to-r from-purple-500 to-pink-500 p-4 rounded-lg shadow-lg">
  <h2 className="text-white text-2xl font-bold">Hello</h2>
</div>
```

### Dark Mode
App đã hỗ trợ dark mode qua `theme-provider.tsx`. Sử dụng:

```tsx
// Tự động đổi màu theo theme
<div className="bg-white dark:bg-gray-900 text-black dark:text-white">
  Nội dung
</div>
```

---

## 🔌 Kết nối Backend API

### Cấu hình `.env`
```env
NEXT_PUBLIC_API_URL=http://localhost:5000/api
```

### Sử dụng API Client

```tsx
// lib/api-client.ts - Đã có sẵn Axios instance

// Ví dụ gọi API
import axios from 'axios';

const API_URL = process.env.NEXT_PUBLIC_API_URL;

// Lấy danh sách sản phẩm
export async function getProducts() {
  const response = await axios.get(`${API_URL}/products`);
  return response.data;
}

// Lấy sliders
export async function getActiveSliders() {
  const response = await axios.get(`${API_URL}/sliders/active`);
  return response.data;
}
```

### Gọi API trong component

```tsx
// Server Component (mặc định)
export default async function ProductsPage() {
  const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/products`);
  const products = await response.json();
  
  return (
    <div>
      {products.data.map(product => (
        <ProductCard key={product.id} product={product} />
      ))}
    </div>
  );
}
```

---

## 📚 Tài nguyên học tập

### Next.js
- [Next.js Docs](https://nextjs.org/docs) - Tài liệu chính thức
- [App Router Tutorial](https://nextjs.org/learn) - Hướng dẫn từng bước

### React
- [React Docs](https://react.dev/) - Tài liệu React 19

### Tailwind CSS
- [Tailwind Docs](https://tailwindcss.com/docs) - Tham khảo classes
- [Tailwind Play](https://play.tailwindcss.com/) - Playground online

### TypeScript
- [TypeScript Handbook](https://www.typescriptlang.org/docs/handbook/)

---

## 🎨 Quy tắc code

### Naming
- Components: `PascalCase` (ProductCard.tsx)
- Functions/hooks: `camelCase` (useProducts, formatPrice)
- Files: Component files dùng PascalCase, utilities dùng kebab-case

### Components
```tsx
// ✅ Đúng - Có types rõ ràng
interface ProductCardProps {
  product: Product;
  onAddToCart?: () => void;
}

export function ProductCard({ product, onAddToCart }: ProductCardProps) {
  return (/* JSX */);
}
```

### Import order
```tsx
// 1. React/Next
import { useState } from 'react';
import Link from 'next/link';

// 2. External packages
import { ShoppingCart } from 'lucide-react';

// 3. Local imports
import { Button } from '@/components/ui/button';
import { formatPrice } from '@/lib/utils';
```

---

## ❓ FAQ

**Q: Làm sao thêm trang admin?**
```bash
mkdir -p app/admin
# Tạo app/admin/layout.tsx cho layout riêng
# Tạo app/admin/page.tsx cho dashboard
```

**Q: Làm sao bảo vệ route cần đăng nhập?**
- Tạo middleware kiểm tra auth token
- Hoặc dùng Supabase Auth

**Q: Làm sao deploy?**
- Vercel: `npx vercel` (recommended for Next.js)
- Docker: Tạo Dockerfile

---

> 📌 **Tip**: Khi phát triển, luôn chạy `npm run dev` và mở http://localhost:3000 để xem thay đổi real-time!
