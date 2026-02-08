import api, { ApiResponse } from "./admin/api";

// ==================== ORDER TYPES ====================

export interface OrderDto {
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
  orderDetails: OrderDetailDto[];
}

export interface OrderDetailDto {
  id: number;
  productName: string;
  productImage: string;
  price: number;
  quantity: number;
  total: number;
}

export interface OrderListItemDto {
  id: number;
  orderCode: string;
  name: string;
  phoneNumber: string;
  total: number;
  status: number;
  statusText: string;
  paymentMethod: string;
  paymentStatus: string;
  createdAt: string;
  totalItems: number;
}

export interface CreateOrderPayload {
  name: string;
  phoneNumber: string;
  address: string;
  email?: string;
  note?: string;
  couponCode?: string;
  paymentMethod: string;
  orderDetails: CreateOrderDetailPayload[];
}

export interface CreateOrderDetailPayload {
  productId: number;
  quantity: number;
}

// ==================== COUPON TYPES ====================

export interface ValidateCouponResult {
  isValid: boolean;
  message: string;
  discountAmount: number;
  finalAmount: number;
}

// ==================== ORDER SERVICE ====================

export const orderService = {
  async createOrder(payload: CreateOrderPayload): Promise<OrderDto> {
    const res = await api.post<ApiResponse<OrderDto>>("/Orders", payload);
    return res.data.data;
  },

  async getOrders(
    page: number = 1,
    pageSize: number = 10
  ): Promise<{
    items: OrderListItemDto[];
    totalCount: number;
    totalPages: number;
  }> {
    const res = await api.get("/Orders", {
      params: { PageNumber: page, PageSize: pageSize },
    });
    return res.data.data;
  },

  async getOrderById(id: number): Promise<OrderDto> {
    const res = await api.get<ApiResponse<OrderDto>>(`/Orders/${id}`);
    return res.data.data;
  },

  async cancelOrder(id: number): Promise<boolean> {
    const res = await api.post<ApiResponse<boolean>>(`/Orders/${id}/cancel`, {
      orderId: id,
    });
    return res.data.data;
  },

  async validateCoupon(
    code: string,
    orderValue: number
  ): Promise<ValidateCouponResult> {
    const res = await api.post<ApiResponse<ValidateCouponResult>>(
      "/Coupons/validate",
      { code, orderValue }
    );
    return res.data.data;
  },
};
