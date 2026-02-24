import { z } from "zod";

/**
 * Schema tạo đăng ký kinh doanh — khớp FluentValidation bên BE
 */
export const createRegistrationSchema = z.object({
  companyName: z
    .string()
    .min(1, "Tên công ty là bắt buộc")
    .max(200, "Tên công ty tối đa 200 ký tự"),
  taxCode: z
    .string()
    .min(1, "Mã số thuế là bắt buộc")
    .max(50, "Mã số thuế tối đa 50 ký tự"),
  ownerName: z
    .string()
    .min(1, "Tên chủ sở hữu là bắt buộc")
    .max(150, "Tên chủ sở hữu tối đa 150 ký tự"),
  email: z
    .string()
    .min(1, "Email là bắt buộc")
    .email("Email không hợp lệ")
    .max(150, "Email tối đa 150 ký tự"),
  phone: z
    .string()
    .min(1, "Số điện thoại là bắt buộc")
    .max(30, "Số điện thoại tối đa 30 ký tự"),
  address: z
    .string()
    .min(1, "Địa chỉ là bắt buộc")
    .max(300, "Địa chỉ tối đa 300 ký tự"),
});

export type CreateRegistrationFormValues = z.infer<typeof createRegistrationSchema>;

/**
 * Schema cập nhật shop — khớp FluentValidation bên BE
 */
export const updateShopSchema = z.object({
  id: z.number().min(1, "Id phải lớn hơn 0"),
  name: z
    .string()
    .min(1, "Tên shop là bắt buộc")
    .max(120, "Tên shop tối đa 120 ký tự"),
  slug: z
    .string()
    .min(1, "Slug là bắt buộc")
    .max(120, "Slug tối đa 120 ký tự")
    .regex(
      /^[a-z0-9]+(?:-[a-z0-9]+)*$/,
      "Slug chỉ chứa chữ thường, số và dấu gạch ngang"
    ),
  description: z
    .string()
    .max(1000, "Mô tả tối đa 1000 ký tự")
    .optional()
    .or(z.literal("")),
  logoUrl: z
    .string()
    .max(500, "LogoUrl tối đa 500 ký tự")
    .optional()
    .or(z.literal("")),
  coverUrl: z
    .string()
    .max(500, "CoverUrl tối đa 500 ký tự")
    .optional()
    .or(z.literal("")),
});

export type UpdateShopFormValues = z.infer<typeof updateShopSchema>;

/**
 * Schema reject — reason bắt buộc
 */
export const rejectReasonSchema = z.object({
  rejectReason: z
    .string()
    .min(1, "Lý do từ chối là bắt buộc")
    .max(500, "Lý do tối đa 500 ký tự"),
});

export type RejectReasonFormValues = z.infer<typeof rejectReasonSchema>;
