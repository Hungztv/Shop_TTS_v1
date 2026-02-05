# 📋 PROMPT: HOÀN THIỆN FRONTEND ADMIN - FULL PAGE FORMS

## 🎯 MỤC TIÊU

Tạo các trang Full Page (trang riêng) cho Products và Orders Detail để thay thế/bổ sung cho giao diện Modal hiện tại.

---

## 📂 CẤU TRÚC DỰ ÁN HIỆN TẠI

### Đã có:

```
frontend/
├── app/admin/
│   ├── layout.tsx                    ✅ Layout chính
│   ├── page.tsx                      ✅ Dashboard
│   ├── products/
│   │   ├── page.tsx                  ✅ Danh sách (với Modal form)
│   │   └── create/                   ⚠️ Folder rỗng
│   ├── orders/page.tsx               ✅ Danh sách (với Modal detail)
│   ├── categories/page.tsx           ✅ CRUD hoàn chỉnh (Modal)
│   ├── brands/page.tsx               ✅ CRUD hoàn chỉnh (Modal)
│   ├── coupons/page.tsx              ✅ CRUD hoàn chỉnh (Modal)
│   ├── sliders/page.tsx              ✅ CRUD hoàn chỉnh (Modal)
│   ├── messages/page.tsx             ✅ Danh sách (Modal detail)
│   └── users/page.tsx                ✅ Danh sách (Modal detail)
├── components/admin/
│   ├── Sidebar.tsx                   ✅
│   ├── AdminHeader.tsx               ✅
│   ├── DataTable.tsx                 ✅
│   ├── Modal.tsx                     ✅
│   ├── ImageUpload.tsx               ✅
│   └── StatsCard.tsx                 ✅
└── lib/services/admin/
    ├── products-service.ts           ✅
    ├── orders-service.ts             ✅
    ├── categories-service.ts         ✅
    ├── brands-service.ts             ✅
    └── ... (tất cả services đã có)
```

## 📋 BACKEND DTOs - REFERENCE (QUAN TRỌNG!)

### 1. **Products**

#### CreateProductCommand (Backend)

```csharp
public class CreateProductCommand : IRequest<ProductDto>
{
    public string Name { get; set; }          // Required, MinLength(4), MaxLength(100)
    public string Slug { get; set; }          // Required, Regex: ^[a-z0-9]+(?:-[a-z0-9]+)*$
    public string Description { get; set; }   // Required, MinLength(10), MaxLength(5000)
    public decimal Price { get; set; }        // Required, GreaterThan(0)
    public decimal CapitalPrice { get; set; } // Required, GreaterThan(0), LessThan(Price)
    public int Quantity { get; set; }         // GreaterThanOrEqualTo(0)
    public string Image { get; set; }         // Required, MaxLength(500)
    public int BrandId { get; set; }          // Required, GreaterThan(0)
    public int CategoryId { get; set; }       // Required, GreaterThan(0)
}
```

#### UpdateProductCommand (Backend)

```csharp
public class UpdateProductCommand : IRequest<ProductDto>
{
    public int Id { get; set; }               // Required (từ route)
    public string Name { get; set; }          // Required, MinLength(4), MaxLength(100)
    public string Slug { get; set; }          // Required, Regex
    public string Description { get; set; }   // Required, MinLength(10), MaxLength(5000)
    public decimal Price { get; set; }        // Required, GreaterThan(0)
    public decimal CapitalPrice { get; set; } // Required, GreaterThan(0), LessThan(Price)
    public int Quantity { get; set; }         // GreaterThanOrEqualTo(0)
    public string Image { get; set; }         // Required, MaxLength(500)
    public int BrandId { get; set; }          // Required, GreaterThan(0)
    public int CategoryId { get; set; }       // Required, GreaterThan(0)
}
```

#### ProductDto (Response từ Backend)

```csharp
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int SoldOut { get; set; }
    public string Image { get; set; }
    public int BrandId { get; set; }
    public string BrandName { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public decimal AverageRating { get; set; }  // Từ ratings
    public int TotalReviews { get; set; }       // Từ ratings
    public bool IsInStock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

**⚠️ LƯU Ý:**

- Backend KHÔNG có field `CapitalPrice` trong ProductDto response (chỉ có khi Create/Update)
- `AverageRating` và `TotalReviews` được tính từ bảng Ratings
- Frontend KHÔNG được gửi `Id` khi Create (auto-generated)
- Frontend PHẢI gửi `Id` khi Update

---

### 2. **Orders**

#### CreateOrderCommand (Backend)

```csharp
public class CreateOrderCommand : IRequest<OrderDto>
{
    public string UserId { get; set; }         // Required (override từ token)
    public string Name { get; set; }           // Required, MinLength(2), MaxLength(100)
    public string PhoneNumber { get; set; }    // Required, Phone format, MaxLength(20)
    public string Address { get; set; }        // Required, MinLength(5), MaxLength(500)
    public string Email { get; set; }          // Optional, Email format, MaxLength(100)
    public string Note { get; set; }           // Optional, MaxLength(1000)
    public string PaymentMethod { get; set; }  // Required, MaxLength(50)
    public string CouponCode { get; set; }     // Optional, MaxLength(50)

    // Required, MinLength(1)
    public List<CreateOrderDetailCommand> OrderDetails { get; set; }
}

public class CreateOrderDetailCommand
{
    public int ProductId { get; set; }  // Required, Range(1, int.MaxValue)
    public int Quantity { get; set; }   // Required, Range(1, int.MaxValue)
}
```

**⚠️ LƯU Ý:**

- `UserId` sẽ được **override từ JWT token** tại Controller, Frontend KHÔNG gửi
- `Subtotal`, `ShippingCost`, `DiscountAmount`, `Total` được **tính tự động** bởi Backend
- `OrderCode` được **generate tự động**
- Frontend chỉ gửi: Name, PhoneNumber, Address, Email, Note, PaymentMethod, CouponCode, OrderDetails[]

#### UpdateOrderStatusCommand (Backend)

```csharp
public class UpdateOrderStatusCommand : IRequest<OrderDto>
{
    public int OrderId { get; set; }     // Required (từ route)
    public int NewStatus { get; set; }   // Required, 0-4
    public string? UpdatedBy { get; set; } // Optional
}
```

**Trạng thái hợp lệ:**

- 0: Chờ xử lý
- 1: Đã xác nhận
- 2: Đang giao hàng
- 3: Đã giao hàng
- 4: Đã hủy

#### OrderDto (Response từ Backend)

```csharp
public class OrderDto
{
    public int Id { get; set; }
    public string OrderCode { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Note { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Total { get; set; }
    public string CouponCode { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public int Status { get; set; }
    public string StatusText { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderDetailDto> OrderDetails { get; set; }
}

public class OrderDetailDto
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
}
```

---

## 🚨 QUY TẮC VÀNG KHI TẠO FORM

### 1. **KHÔNG được tự tạo field mới**

```typescript
// ❌ SAI - Field không tồn tại trong backend
const formData = {
  sku: "", // Backend KHÔNG có field này!
  barcode: "", // Backend KHÔNG có field này!
  featured: false, // Backend KHÔNG có field này!
};

// ✅ ĐÚNG - Chỉ dùng fields từ Backend DTOs
const formData = {
  name: "",
  slug: "",
  description: "",
  price: 0,
  capitalPrice: 0,
  quantity: 0,
  image: "",
  brandId: 0,
  categoryId: 0,
};
```

### 2. **PHẢI validate đúng quy tắc Backend**

```typescript
// Backend Validator:
// RuleFor(x => x.Name).MinimumLength(4).MaximumLength(100)

// ❌ SAI
if (name.length < 3) { ... }  // Min length sai!

// ✅ ĐÚNG
if (name.length < 4 || name.length > 100) { ... }
```

### 3. **KHÔNG gửi fields read-only**

```typescript
// ❌ SAI - Gửi fields không cần thiết
await api.post("/products", {
  ...formData,
  id: 0, // Auto-generated, KHÔNG gửi
  createdAt: new Date(), // Auto-generated, KHÔNG gửi
  soldOut: 0, // Calculated field, KHÔNG gửi
  averageRating: 0, // Calculated field, KHÔNG gửi
});

// ✅ ĐÚNG - Chỉ gửi CreateProductCommand fields
await api.post("/products", {
  name: formData.name,
  slug: formData.slug,
  description: formData.description,
  price: formData.price,
  capitalPrice: formData.capitalPrice,
  quantity: formData.quantity,
  image: formData.image,
  brandId: formData.brandId,
  categoryId: formData.categoryId,
});
```

### 4. **Type Mapping chính xác**

```typescript
// Backend: decimal Price
// ❌ SAI
price: "100000"; // String

// ✅ ĐÚNG
price: 100000; // Number

// Backend: int CategoryId
// ❌ SAI
categoryId: "5"; // String

// ✅ ĐÚNG
categoryId: 5; // Number (integer)
```

### 5. **Required vs Optional**

```typescript
// Backend: public string Email { get; set; } // Optional
// ❌ SAI - Yêu cầu bắt buộc
if (!email) {
  alert("Email là bắt buộc");
  return;
}

// ✅ ĐÚNG - Chỉ validate format nếu có
if (email && !isValidEmail(email)) {
  alert("Email không hợp lệ");
  return;
}
```

---

## 📝 TYPESCRIPT INTERFACE MAPPING

### Frontend TypeScript Types (phải match Backend DTOs):

```typescript
// frontend/types/product.ts

// Tương ứng với CreateProductCommand
export interface CreateProductDto {
  name: string; // Required
  slug: string; // Required
  description: string; // Required
  price: number; // decimal -> number
  capitalPrice: number; // decimal -> number
  quantity: number; // int -> number
  image: string; // Required
  brandId: number; // int -> number
  categoryId: number; // int -> number
}

// Tương ứng với UpdateProductCommand
export interface UpdateProductDto extends CreateProductDto {
  id: number; // Thêm id cho update
}

// Tương ứng với ProductDto (Response)
export interface Product {
  id: number;
  name: string;
  slug: string;
  description: string;
  price: number;
  quantity: number;
  soldOut: number;
  image: string;
  brandId: number;
  brandName: string;
  categoryId: number;
  categoryName: string;
  averageRating: number;
  totalReviews: number;
  isInStock: boolean;
  createdAt: string; // DateTime -> string (ISO)
  updatedAt?: string; // DateTime? -> string | undefined
}
```

```typescript
// frontend/types/order.ts

// Tương ứng với CreateOrderDetailCommand
export interface CreateOrderDetailDto {
  productId: number;
  quantity: number;
}

// Tương ứng với CreateOrderCommand
export interface CreateOrderDto {
  // UserId KHÔNG gửi từ frontend (auto từ token)
  name: string;
  phoneNumber: string;
  address: string;
  email?: string; // Optional
  note?: string; // Optional
  paymentMethod: string;
  couponCode?: string; // Optional
  orderDetails: CreateOrderDetailDto[];
}

// Tương ứng với OrderDto (Response)
export interface Order {
  id: number;
  orderCode: string;
  name: string;
  phoneNumber: string;
  address: string;
  email: string;
  note: string;
  shippingCost: number;
  subtotal: number;
  discountAmount: number;
  total: number;
  couponCode: string;
  paymentMethod: string;
  paymentStatus: string;
  status: number;
  statusText: string;
  userId: string;
  createdAt: string;
  orderDetails: OrderDetail[];
}

export interface OrderDetail {
  id: number;
  productName: string;
  productImage: string;
  price: number;
  quantity: number;
  total: number;
}
```

---

### File cần tạo:

#### 1. `/frontend/app/admin/products/create/page.tsx`

**Mục đích:** Trang tạo sản phẩm mới với form đầy đủ

**Yêu cầu:**

- Form lớn với nhiều không gian
- Upload nhiều ảnh (1 thumbnail + gallery 4-6 ảnh)
- Rich text editor cho mô tả (hoặc textarea lớn)
- Các trường:
  - Tên sản phẩm (auto-generate slug)
  - Slug (editable)
  - Mô tả ngắn (textarea)
  - Mô tả chi tiết (textarea lớn hoặc rich editor)
  - Giá bán (price)
  - Giá vốn (capitalPrice)
  - Số lượng tồn kho (quantity)
  - Danh mục (categoryId) - dropdown
  - Thương hiệu (brandId) - dropdown
  - Ảnh chính (image) - ImageUpload component
  - Gallery ảnh (images[]) - Multiple ImageUpload
  - Trạng thái (status) - select hoặc toggle
- Validation đầy đủ
- Loading state khi submit
- Redirect về danh sách sau khi tạo thành công

**UI/UX:**

```tsx
<div className="max-w-5xl mx-auto">
  <div className="bg-white rounded-2xl shadow-sm p-8">
    <div className="mb-8">
      <h1>Tạo sản phẩm mới</h1>
      <p className="text-gray-500">Điền thông tin sản phẩm</p>
    </div>

    <form>
      {/* Section: Thông tin cơ bản */}
      <div className="grid grid-cols-2 gap-6">
        <div className="col-span-2">
          <label>Tên sản phẩm *</label>
          <input type="text" ... />
        </div>
        <div>
          <label>Slug *</label>
          <input type="text" ... />
        </div>
        <div>
          <label>SKU (optional)</label>
          <input type="text" ... />
        </div>
      </div>

      {/* Section: Giá & Kho */}
      <div className="grid grid-cols-3 gap-6 mt-6">
        <div>
          <label>Giá bán *</label>
          <input type="number" ... />
        </div>
        <div>
          <label>Giá vốn</label>
          <input type="number" ... />
        </div>
        <div>
          <label>Số lượng</label>
          <input type="number" ... />
        </div>
      </div>

      {/* Section: Phân loại */}
      <div className="grid grid-cols-2 gap-6 mt-6">
        <div>
          <label>Danh mục *</label>
          <select ... />
        </div>
        <div>
          <label>Thương hiệu *</label>
          <select ... />
        </div>
      </div>

      {/* Section: Mô tả */}
      <div className="mt-6">
        <label>Mô tả ngắn</label>
        <textarea rows={3} ... />
      </div>
      <div className="mt-6">
        <label>Mô tả chi tiết</label>
        <textarea rows={8} ... />
      </div>

      {/* Section: Hình ảnh */}
      <div className="mt-6">
        <label>Ảnh đại diện *</label>
        <ImageUpload type="product" ... />
      </div>
      <div className="mt-6">
        <label>Gallery ảnh (tối đa 6 ảnh)</label>
        {/* Multiple image upload */}
      </div>

      {/* Buttons */}
      <div className="flex justify-end gap-3 mt-8 pt-6 border-t">
        <Link href="/admin/products">
          <button type="button">Hủy</button>
        </Link>
        <button type="submit" disabled={loading}>
          {loading ? 'Đang lưu...' : 'Tạo sản phẩm'}
        </button>
      </div>
    </form>
  </div>
</div>
```

**Logic:**

```typescript
'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { productsService } from '@/lib/services/admin/products-service';
import { categoriesService } from '@/lib/services/admin/categories-service';
import { brandsService } from '@/lib/services/admin/brands-service';
import ImageUpload from '@/components/admin/ImageUpload';

export default function CreateProductPage() {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [categories, setCategories] = useState([]);
  const [brands, setBrands] = useState([]);

  // DTO CHÍNH XÁC từ Backend: CreateProductCommand
  const [formData, setFormData] = useState({
    name: '',           // Required, MinLength(4), MaxLength(100)
    slug: '',           // Required, regex: ^[a-z0-9]+(?:-[a-z0-9]+)*$
    description: '',    // Required, MinLength(10), MaxLength(5000)
    price: 0,          // Required, GreaterThan(0)
    capitalPrice: 0,   // Required, GreaterThan(0), LessThan(price)
    quantity: 0,       // GreaterThanOrEqualTo(0)
    image: '',         // Required, MaxLength(500)
    brandId: 0,        // Required, GreaterThan(0)
    categoryId: 0,     // Required, GreaterThan(0)
  });

  useEffect(() => {
    loadFilters();
  }, []);

  const loadFilters = async () => {
    const [cats, brds] = await Promise.all([
      categoriesService.getAll(),
      brandsService.getAll()
    ]);
    setCategories(cats);
    setBrands(brds);
    if (cats.length) setFormData(prev => ({ ...prev, categoryId: cats[0].id }));
    if (brds.length) setFormData(prev => ({ ...prev, brandId: brds[0].id }));
  };

  const generateSlug = (name: string) => {
    return name
      .toLowerCase()
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .replace(/đ/g, 'd')
      .replace(/[^a-z0-9\s-]/g, '')
      .trim()
      .replace(/\s+/g, '-');
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    // ===== VALIDATION THEO BACKEND VALIDATOR =====

    // Name: Required, MinLength(4), MaxLength(100)
    if (!formData.name || formData.name.trim().length < 4) {
      alert('Tên sản phẩm phải có ít nhất 4 ký tự');
      return;
    }
    if (formData.name.length > 100) {
      alert('Tên sản phẩm không được vượt quá 100 ký tự');
      return;
    }

    // Slug: Required, regex pattern
    if (!formData.slug) {
      alert('Slug không được để trống');
      return;
    }
    const slugPattern = /^[a-z0-9]+(?:-[a-z0-9]+)*$/;
    if (!slugPattern.test(formData.slug)) {
      alert('Slug chỉ chứa chữ thường, số và dấu gạch ngang');
      return;
    }

    // Description: Required, MinLength(10), MaxLength(5000)
    if (!formData.description || formData.description.trim().length < 10) {
      alert('Mô tả phải có ít nhất 10 ký tự');
      return;
    }
    if (formData.description.length > 5000) {
      alert('Mô tả không được vượt quá 5000 ký tự');
      return;
    }

    // Price: GreaterThan(0)
    if (formData.price <= 0) {
      alert('Giá bán phải lớn hơn 0');
      return;
    }

    // CapitalPrice: GreaterThan(0), LessThan(price)
    if (formData.capitalPrice <= 0) {
      alert('Giá vốn phải lớn hơn 0');
      return;
    }
    if (formData.capitalPrice >= formData.price) {
      alert('Giá vốn phải nhỏ hơn giá bán');
      return;
    }

    // Quantity: GreaterThanOrEqualTo(0)
    if (formData.quantity < 0) {
      alert('Số lượng không được âm');
      return;
    }

    // Image: Required, MaxLength(500)
    if (!formData.image) {
      alert('Vui lòng upload ảnh sản phẩm');
      return;
    }
    if (formData.image.length > 500) {
      alert('Đường dẫn ảnh không được vượt quá 500 ký tự');
      return;
    }

    // BrandId: GreaterThan(0)
    if (!formData.brandId || formData.brandId <= 0) {
      alert('Vui lòng chọn thương hiệu');
      return;
    }

    // CategoryId: GreaterThan(0)
    if (!formData.categoryId || formData.categoryId <= 0) {
      alert('Vui lòng chọn danh mục');
      return;
    }

    setLoading(true);
    try {
      await productsService.create(formData);
      router.push('/admin/products');
    } catch (error) {
      console.error(error);
      alert('Có lỗi xảy ra!');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen p-6">
      {/* Form JSX here */}
    </div>
  );
}
```

---

#### 2. `/frontend/app/admin/products/[id]/page.tsx`

**Mục đích:** Trang xem chi tiết sản phẩm (read-only)

**Yêu cầu:**

- Hiển thị tất cả thông tin sản phẩm
- Layout đẹp với ảnh lớn
- Hiển thị:
  - Gallery ảnh
  - Thông tin cơ bản (tên, slug, giá, tồn kho)
  - Danh mục, thương hiệu
  - Mô tả chi tiết
  - Thống kê: Đã bán, Rating, Lượt xem
  - Ngày tạo, cập nhật
- Nút: "Sửa", "Xóa", "Quay lại"

**UI/UX:**

```tsx
<div className="max-w-6xl mx-auto p-6">
  <div className="mb-6 flex justify-between items-center">
    <button onClick={() => router.back()}>← Quay lại</button>
    <div className="flex gap-3">
      <Link href={`/admin/products/${product.id}/edit`}>
        <button className="btn-primary">Sửa</button>
      </Link>
      <button onClick={handleDelete} className="btn-danger">
        Xóa
      </button>
    </div>
  </div>

  <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
    {/* Left: Images */}
    <div>
      <img src={product.image} className="w-full rounded-2xl" />
      {/* Gallery thumbnails */}
    </div>

    {/* Right: Info */}
    <div>
      <h1 className="text-3xl font-bold">{product.name}</h1>
      <p className="text-gray-500">{product.slug}</p>

      <div className="mt-6 space-y-4">
        <div className="flex justify-between">
          <span>Giá bán:</span>
          <strong className="text-2xl text-violet-600">
            {formatCurrency(product.price)}
          </strong>
        </div>
        <div className="flex justify-between">
          <span>Giá vốn:</span>
          <span>{formatCurrency(product.capitalPrice)}</span>
        </div>
        <div className="flex justify-between">
          <span>Tồn kho:</span>
          <span
            className={
              product.quantity > 10 ? "text-green-600" : "text-red-600"
            }
          >
            {product.quantity}
          </span>
        </div>
        <div className="flex justify-between">
          <span>Đã bán:</span>
          <span>{product.soldOut}</span>
        </div>
      </div>

      <div className="mt-6 pt-6 border-t">
        <p>
          <strong>Danh mục:</strong> {product.category?.name}
        </p>
        <p>
          <strong>Thương hiệu:</strong> {product.brand?.name}
        </p>
      </div>

      <div className="mt-6">
        <h3 className="font-semibold mb-2">Mô tả:</h3>
        <p className="text-gray-600">{product.description}</p>
      </div>
    </div>
  </div>
</div>
```

---

#### 3. `/frontend/app/admin/products/[id]/edit/page.tsx`

**Mục đích:** Trang sửa sản phẩm

**Yêu cầu:**

- Tương tự create/page.tsx
- Load dữ liệu sản phẩm từ API theo ID
- Pre-fill form với data hiện tại
- Submit để update
- Redirect về detail hoặc list sau khi update

**Logic khác:**

```typescript
'use client';

import { useParams, useRouter } from 'next/navigation';

export default function EditProductPage() {
  const { id } = useParams();
  const router = useRouter();
  const [product, setProduct] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadProduct();
  }, [id]);

  const loadProduct = async () => {
    const data = await productsService.getById(Number(id));
    setProduct(data);
    setFormData({
      name: data.name,
      slug: data.slug,
      description: data.description,
      price: data.price,
      capitalPrice: data.capitalPrice,
      quantity: data.quantity,
      image: data.image,
      categoryId: data.categoryId,
      brandId: data.brandId,
    });
    setLoading(false);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      await productsService.update(Number(id), formData);
      router.push(`/admin/products/${id}`); // hoặc /admin/products
    } catch (error) {
      alert('Có lỗi xảy ra!');
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div>Loading...</div>;

  return (
    {/* Form giống create, nhưng pre-filled */}
  );
}
```

---

#### 4. `/frontend/components/admin/forms/ProductForm.tsx` (Optional - Tái sử dụng)

**Mục đích:** Component form tái sử dụng cho cả Create và Edit

**Yêu cầu:**

- Nhận props: formData, onChange, onSubmit, loading
- Render form fields
- Có thể dùng cho cả create và edit page

```typescript
interface ProductFormProps {
  formData: CreateProductDto;
  onChange: (data: CreateProductDto) => void;
  onSubmit: (e: React.FormEvent) => void;
  loading: boolean;
  categories: Category[];
  brands: Brand[];
  submitText?: string;
}

export default function ProductForm({
  formData,
  onChange,
  onSubmit,
  loading,
  categories,
  brands,
  submitText = 'Lưu'
}: ProductFormProps) {
  return (
    <form onSubmit={onSubmit}>
      {/* All form fields */}
    </form>
  );
}
```

---

## 🚀 PHASE 2: ORDERS DETAIL FULL PAGE

### File cần tạo:

#### 1. `/frontend/app/admin/orders/[id]/page.tsx`

**Mục đích:** Trang chi tiết đơn hàng + Cập nhật trạng thái

**Yêu cầu:**

- Hiển thị đầy đủ thông tin đơn hàng:
  - Mã đơn hàng (orderCode)
  - Thông tin khách hàng (name, phone, email, address)
  - Danh sách sản phẩm trong đơn (OrderDetails)
  - Tổng tiền (subtotal, shippingCost, discount, total)
  - Phương thức thanh toán
  - Trạng thái thanh toán
  - Ghi chú
  - Ngày tạo đơn
- Timeline trạng thái đơn hàng (0 -> 1 -> 2 -> 3)
- Nút cập nhật trạng thái (chỉ cho phép chuyển sang trạng thái tiếp theo)
- Nút hủy đơn (nếu status <= 1)
- Nút in hóa đơn (optional)

**UI/UX:**

```tsx
<div className="max-w-6xl mx-auto p-6">
  <div className="mb-6">
    <button onClick={() => router.back()}>← Quay lại</button>
  </div>

  {/* Order Header */}
  <div className="bg-white rounded-2xl shadow-sm p-6 mb-6">
    <div className="flex justify-between items-start">
      <div>
        <h1 className="text-2xl font-bold">Đơn hàng #{order.orderCode}</h1>
        <p className="text-gray-500">Ngày đặt: {formatDate(order.createdAt)}</p>
      </div>
      <div>
        <StatusBadge status={order.status} />
      </div>
    </div>
  </div>

  <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
    {/* Left Column: Order Details */}
    <div className="lg:col-span-2 space-y-6">
      {/* Customer Info */}
      <div className="bg-white rounded-2xl shadow-sm p-6">
        <h3 className="font-semibold mb-4">Thông tin khách hàng</h3>
        <div className="space-y-2">
          <p>
            <strong>Họ tên:</strong> {order.name}
          </p>
          <p>
            <strong>Số điện thoại:</strong> {order.phoneNumber}
          </p>
          <p>
            <strong>Email:</strong> {order.email}
          </p>
          <p>
            <strong>Địa chỉ:</strong> {order.address}
          </p>
        </div>
      </div>

      {/* Order Items */}
      <div className="bg-white rounded-2xl shadow-sm p-6">
        <h3 className="font-semibold mb-4">Sản phẩm đã đặt</h3>
        <table className="w-full">
          <thead>
            <tr className="border-b">
              <th>Sản phẩm</th>
              <th>Đơn giá</th>
              <th>SL</th>
              <th>Thành tiền</th>
            </tr>
          </thead>
          <tbody>
            {order.orderDetails?.map((item) => (
              <tr key={item.id} className="border-b">
                <td>
                  <div className="flex items-center gap-3">
                    <img src={item.product?.image} className="w-12 h-12" />
                    <span>{item.product?.name}</span>
                  </div>
                </td>
                <td>{formatCurrency(item.price)}</td>
                <td>{item.quantity}</td>
                <td>{formatCurrency(item.price * item.quantity)}</td>
              </tr>
            ))}
          </tbody>
        </table>

        <div className="mt-4 space-y-2 border-t pt-4">
          <div className="flex justify-between">
            <span>Tạm tính:</span>
            <span>{formatCurrency(order.subtotal)}</span>
          </div>
          <div className="flex justify-between">
            <span>Phí vận chuyển:</span>
            <span>{formatCurrency(order.shippingCost)}</span>
          </div>
          {order.discountAmount > 0 && (
            <div className="flex justify-between text-green-600">
              <span>Giảm giá:</span>
              <span>-{formatCurrency(order.discountAmount)}</span>
            </div>
          )}
          <div className="flex justify-between text-xl font-bold border-t pt-2">
            <span>Tổng cộng:</span>
            <span className="text-violet-600">
              {formatCurrency(order.total)}
            </span>
          </div>
        </div>
      </div>

      {/* Payment Info */}
      <div className="bg-white rounded-2xl shadow-sm p-6">
        <h3 className="font-semibold mb-4">Thông tin thanh toán</h3>
        <p>
          <strong>Phương thức:</strong> {order.paymentMethod}
        </p>
        <p>
          <strong>Trạng thái:</strong> {order.paymentStatus}
        </p>
      </div>
    </div>

    {/* Right Column: Timeline & Actions */}
    <div className="space-y-6">
      {/* Order Timeline */}
      <div className="bg-white rounded-2xl shadow-sm p-6">
        <h3 className="font-semibold mb-4">Trạng thái đơn hàng</h3>
        <OrderTimeline currentStatus={order.status} />
      </div>

      {/* Actions */}
      <div className="bg-white rounded-2xl shadow-sm p-6">
        <h3 className="font-semibold mb-4">Thao tác</h3>
        <div className="space-y-3">
          {/* Update Status Buttons */}
          {order.status === 0 && (
            <button
              onClick={() => updateStatus(1)}
              className="w-full btn-primary"
            >
              Xác nhận đơn hàng
            </button>
          )}
          {order.status === 1 && (
            <button
              onClick={() => updateStatus(2)}
              className="w-full btn-primary"
            >
              Bắt đầu giao hàng
            </button>
          )}
          {order.status === 2 && (
            <button
              onClick={() => updateStatus(3)}
              className="w-full btn-success"
            >
              Đã giao hàng
            </button>
          )}

          {/* Cancel Order */}
          {order.status <= 1 && (
            <button onClick={handleCancelOrder} className="w-full btn-danger">
              Hủy đơn hàng
            </button>
          )}

          {/* Print Invoice */}
          <button className="w-full btn-outline">In hóa đơn</button>
        </div>
      </div>
    </div>
  </div>
</div>
```

**Logic:**

```typescript
'use client';

import { useState, useEffect } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { ordersService } from '@/lib/services/admin/orders-service';

export default function OrderDetailPage() {
  const { id } = useParams();
  const router = useRouter();
  const [order, setOrder] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadOrder();
  }, [id]);

  const loadOrder = async () => {
    const data = await ordersService.getById(Number(id));
    setOrder(data);
    setLoading(false);
  };

  const updateStatus = async (newStatus: number) => {
    if (!confirm('Xác nhận thay đổi trạng thái?')) return;

    try {
      await ordersService.updateStatus(Number(id), newStatus);
      loadOrder(); // Reload
    } catch (error) {
      alert('Có lỗi xảy ra!');
    }
  };

  const handleCancelOrder = async () => {
    if (!confirm('Xác nhận hủy đơn hàng?')) return;

    try {
      await ordersService.cancel(Number(id));
      loadOrder();
    } catch (error) {
      alert('Có lỗi xảy ra!');
    }
  };

  if (loading) return <div>Loading...</div>;

  return (
    {/* JSX here */}
  );
}
```

---

#### 2. `/frontend/components/admin/OrderTimeline.tsx`

**Mục đích:** Component hiển thị timeline trạng thái đơn hàng

**Yêu cầu:**

- Hiển thị các bước: Chờ xử lý -> Đã xác nhận -> Đang giao -> Đã giao
- Highlight bước hiện tại
- Hiển thị icon check cho các bước đã hoàn thành
- Màu sắc khác nhau cho từng trạng thái

```typescript
interface OrderTimelineProps {
  currentStatus: number;
}

const statusSteps = [
  { status: 0, label: 'Chờ xử lý', icon: Clock },
  { status: 1, label: 'Đã xác nhận', icon: CheckCircle },
  { status: 2, label: 'Đang giao', icon: Truck },
  { status: 3, label: 'Đã giao', icon: Package },
];

export default function OrderTimeline({ currentStatus }: OrderTimelineProps) {
  return (
    <div className="relative">
      {statusSteps.map((step, index) => (
        <div key={step.status} className="flex items-start gap-3 mb-6 last:mb-0">
          {/* Icon */}
          <div className={`
            w-10 h-10 rounded-full flex items-center justify-center
            ${currentStatus >= step.status
              ? 'bg-green-500 text-white'
              : 'bg-gray-200 text-gray-400'
            }
          `}>
            <step.icon className="w-5 h-5" />
          </div>

          {/* Label */}
          <div className="flex-1">
            <p className={`font-medium ${
              currentStatus >= step.status ? 'text-gray-900' : 'text-gray-400'
            }`}>
              {step.label}
            </p>
            <p className="text-sm text-gray-500">
              {currentStatus >= step.status ? 'Đã hoàn thành' : 'Chờ xử lý'}
            </p>
          </div>

          {/* Connector Line */}
          {index < statusSteps.length - 1 && (
            <div className={`
              absolute left-5 w-0.5 h-6 mt-10
              ${currentStatus > step.status ? 'bg-green-500' : 'bg-gray-200'}
            `} style={{ top: `${index * 4}rem` }} />
          )}
        </div>
      ))}
    </div>
  );
}
```

---

## 🎨 DESIGN SYSTEM

### Màu sắc:

- Primary: `violet-600` (#7c3aed)
- Success: `green-500`
- Danger: `red-500`
- Warning: `yellow-500`
- Info: `blue-500`

### Buttons:

```css
.btn-primary: bg-violet-600 hover:bg-violet-700 text-white px-6 py-2.5 rounded-xl
.btn-success: bg-green-500 hover:bg-green-600 text-white px-6 py-2.5 rounded-xl
.btn-danger: bg-red-500 hover:bg-red-600 text-white px-6 py-2.5 rounded-xl
.btn-outline: border-2 border-gray-300 hover:bg-gray-50 px-6 py-2.5 rounded-xl
```

### Cards:

```css
bg-white dark:bg-gray-800 rounded-2xl shadow-sm border border-gray-100 dark:border-gray-700 p-6
```

---

## 📝 LƯU Ý QUAN TRỌNG

### 1. **Routing:**

- Products: `/admin/products/create`, `/admin/products/[id]`, `/admin/products/[id]/edit`
- Orders: `/admin/orders/[id]`

### 2. **API Integration:**

- Sử dụng services đã có: `productsService`, `ordersService`, `categoriesService`, `brandsService`
- Đã có sẵn:
  - `productsService.getAll()`, `.getById()`, `.create()`, `.update()`, `.delete()`
  - `ordersService.getAll()`, `.getById()`, `.updateStatus()`, `.cancel()`

### 3. **Form Validation - PHẢI MATCH BACKEND:**

- **Products:**
  - Name: Required, 4-100 chars
  - Slug: Required, regex `^[a-z0-9]+(?:-[a-z0-9]+)*$`
  - Description: Required, 10-5000 chars
  - Price: Required, > 0
  - CapitalPrice: Required, > 0, < Price
  - Quantity: >= 0
  - Image: Required, max 500 chars
  - BrandId: Required, > 0
  - CategoryId: Required, > 0

- **Orders:**
  - Name: Required, 2-100 chars
  - PhoneNumber: Required, Phone format, max 20 chars
  - Address: Required, 5-500 chars
  - Email: Optional, Email format, max 100 chars
  - Note: Optional, max 1000 chars
  - PaymentMethod: Required, max 50 chars
  - OrderDetails: Required, min 1 item
    - ProductId: Required, > 0
    - Quantity: Required, > 0

### 4. **Data Type Mapping:**

```typescript
Backend (C#)  →  Frontend (TypeScript)
decimal       →  number
int           →  number
string        →  string
bool          →  boolean
DateTime      →  string (ISO format)
List<T>       →  T[]
```

### 5. **KHÔNG gửi fields auto-generated:**

- ❌ `Id` (khi Create)
- ❌ `CreatedAt`, `UpdatedAt`
- ❌ `OrderCode` (auto-generated)
- ❌ `Subtotal`, `Total`, `ShippingCost`, `DiscountAmount` (calculated by backend)
- ❌ `SoldOut`, `AverageRating`, `TotalReviews` (calculated fields)

### 6. **UserId handling cho Orders:**

```typescript
// Backend sẽ OVERRIDE UserId từ JWT token
// Frontend KHÔNG cần gửi userId trong CreateOrderDto
const orderData = {
  // userId: currentUser.id,  ❌ KHÔNG gửi
  name: formData.name, // ✅ Gửi
  phoneNumber: formData.phone, // ✅ Gửi
  // ... other fields
};
```

### 7. **Loading States:**

- Skeleton hoặc spinner khi load data
- Disable form khi đang submit
- Loading indicator trong button submit

### 8. **Error Handling:**

```typescript
try {
  await productsService.create(formData);
  router.push("/admin/products");
} catch (error: any) {
  // Backend trả về: { success: false, message: "...", errors: {...} }
  const message =
    error?.response?.data?.message ||
    error?.response?.data?.title ||
    "Có lỗi xảy ra!";
  alert(message);
  console.error("Error:", error);
}
```

### 9. **Responsive:**

- Mobile-friendly
- Grid layout responsive (col-span-2, lg:col-span-3)

### 10. **TypeScript:**

- Type safety cho tất cả components và functions
- Import types từ services hoặc tạo types riêng match với Backend DTOs

---

## ✅ CHECKLIST HOÀN THÀNH

### Products:

- [ ] `/admin/products/create/page.tsx` - Trang tạo sản phẩm
- [ ] `/admin/products/[id]/page.tsx` - Xem chi tiết sản phẩm
- [ ] `/admin/products/[id]/edit/page.tsx` - Sửa sản phẩm
- [ ] `/components/admin/forms/ProductForm.tsx` (optional) - Form component

### Orders:

- [ ] `/admin/orders/[id]/page.tsx` - Chi tiết đơn hàng
- [ ] `/components/admin/OrderTimeline.tsx` - Timeline component
- [ ] `/components/admin/OrderStatusBadge.tsx` (optional) - Status badge

### Testing:

- [ ] Test tạo sản phẩm mới
- [ ] Test sửa sản phẩm
- [ ] Test xóa sản phẩm
- [ ] Test xem chi tiết đơn hàng
- [ ] Test cập nhật trạng thái đơn hàng
- [ ] Test hủy đơn hàng

---

## 🚀 CÁCH SỬ DỤNG PROMPT NÀY

1. Copy toàn bộ nội dung prompt này
2. Paste vào AI assistant (ChatGPT, Claude, v.v.)
3. Yêu cầu: "Hãy tạo các file theo prompt này, từng file một"
4. Hoặc: "Bắt đầu với Products Create Page"
5. Review code generated
6. Test từng tính năng

---

## 📌 KẾT LUẬN

Sau khi hoàn thành prompt này, bạn sẽ có:

- ✅ Full Page CRUD cho Products (Create, Read, Update, Delete)
- ✅ Chi tiết đơn hàng với timeline trạng thái
- ✅ Cập nhật trạng thái đơn hàng
- ✅ UI/UX chuyên nghiệp, nhất quán

Tổng số file mới: **5-7 files**
Thời gian ước tính: **4-6 giờ** (nếu làm thủ công)
