import {
    Facebook,
    Instagram,
    Twitter,
    Youtube,
    Mail,
    Phone,
    MapPin,
    CreditCard,
    Truck,
    Shield,
    HeadphonesIcon,
} from "lucide-react";

const footerLinks = {
    company: [
        { label: "Về chúng tôi", href: "#" },
        { label: "Tuyển dụng", href: "#" },
        { label: "Tin tức", href: "#" },
        { label: "Liên hệ", href: "#" },
    ],
    support: [
        { label: "Trung tâm trợ giúp", href: "#" },
        { label: "Hướng dẫn mua hàng", href: "#" },
        { label: "Chính sách đổi trả", href: "#" },
        { label: "Chính sách bảo hành", href: "#" },
    ],
    legal: [
        { label: "Điều khoản sử dụng", href: "#" },
        { label: "Chính sách bảo mật", href: "#" },
        { label: "Cookie Policy", href: "#" },
    ],
};

const features = [
    {
        icon: Truck,
        title: "Giao hàng miễn phí",
        description: "Đơn hàng từ 500k",
    },
    {
        icon: Shield,
        title: "Bảo hành chính hãng",
        description: "Lên đến 24 tháng",
    },
    {
        icon: CreditCard,
        title: "Thanh toán an toàn",
        description: "Đa dạng phương thức",
    },
    {
        icon: HeadphonesIcon,
        title: "Hỗ trợ 24/7",
        description: "Tư vấn tận tâm",
    },
];

export default function Footer() {
    return (
        <footer className="bg-slate-900 text-white">
            {/* Features Bar */}
            <div className="border-b border-slate-800">
                <div className="max-w-7xl mx-auto px-4 py-8">
                    <div className="grid grid-cols-2 md:grid-cols-4 gap-6">
                        {features.map((feature, index) => (
                            <div
                                key={index}
                                className="flex items-center gap-4 group cursor-pointer"
                            >
                                <div className="w-14 h-14 rounded-xl bg-gradient-to-br from-violet-600 to-purple-700 flex items-center justify-center group-hover:scale-110 transition-transform">
                                    <feature.icon className="w-6 h-6 text-white" />
                                </div>
                                <div>
                                    <h4 className="font-semibold text-white">{feature.title}</h4>
                                    <p className="text-sm text-slate-400">
                                        {feature.description}
                                    </p>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>

            {/* Main Footer */}
            <div className="max-w-7xl mx-auto px-4 py-12">
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-8">
                    {/* Brand Info */}
                    <div className="lg:col-span-2">
                        <a href="/" className="flex items-center gap-3 mb-6">
                            <div className="w-12 h-12 rounded-xl bg-gradient-to-br from-violet-500 to-purple-600 flex items-center justify-center shadow-lg">
                                <span className="text-white font-bold text-2xl">S</span>
                            </div>
                            <span className="text-2xl font-bold bg-gradient-to-r from-violet-400 to-pink-400 bg-clip-text text-transparent">
                                ShopTTS
                            </span>
                        </a>
                        <p className="text-slate-400 mb-6 leading-relaxed">
                            ShopTTS - Nền tảng thương mại điện tử hàng đầu Việt Nam, mang đến
                            trải nghiệm mua sắm tuyệt vời với hàng triệu sản phẩm chính hãng.
                        </p>
                        <div className="space-y-3">
                            <a
                                href="tel:1900xxxx"
                                className="flex items-center gap-3 text-slate-400 hover:text-violet-400 transition-colors"
                            >
                                <Phone className="w-5 h-5" />
                                <span>1900 xxxx (Miễn phí)</span>
                            </a>
                            <a
                                href="mailto:support@shoptts.vn"
                                className="flex items-center gap-3 text-slate-400 hover:text-violet-400 transition-colors"
                            >
                                <Mail className="w-5 h-5" />
                                <span>support@shoptts.vn</span>
                            </a>
                            <div className="flex items-start gap-3 text-slate-400">
                                <MapPin className="w-5 h-5 flex-shrink-0 mt-0.5" />
                                <span>123 Đường ABC, Quận 1, TP. Hồ Chí Minh</span>
                            </div>
                        </div>
                    </div>

                    {/* Company Links */}
                    <div>
                        <h4 className="font-semibold text-lg mb-4 text-white">Công ty</h4>
                        <ul className="space-y-3">
                            {footerLinks.company.map((link) => (
                                <li key={link.label}>
                                    <a
                                        href={link.href}
                                        className="text-slate-400 hover:text-violet-400 transition-colors"
                                    >
                                        {link.label}
                                    </a>
                                </li>
                            ))}
                        </ul>
                    </div>

                    {/* Support Links */}
                    <div>
                        <h4 className="font-semibold text-lg mb-4 text-white">Hỗ trợ</h4>
                        <ul className="space-y-3">
                            {footerLinks.support.map((link) => (
                                <li key={link.label}>
                                    <a
                                        href={link.href}
                                        className="text-slate-400 hover:text-violet-400 transition-colors"
                                    >
                                        {link.label}
                                    </a>
                                </li>
                            ))}
                        </ul>
                    </div>

                    {/* Newsletter */}
                    <div>
                        <h4 className="font-semibold text-lg mb-4 text-white">
                            Đăng ký nhận tin
                        </h4>
                        <p className="text-slate-400 mb-4 text-sm">
                            Nhận thông tin khuyến mãi và ưu đãi mới nhất
                        </p>
                        <div className="space-y-3">
                            <input
                                type="email"
                                placeholder="Email của bạn"
                                className="w-full px-4 py-3 rounded-xl bg-slate-800 border border-slate-700 focus:border-violet-500 focus:outline-none text-white placeholder:text-slate-500 transition-colors"
                            />
                            <button className="w-full btn-primary">Đăng ký</button>
                        </div>

                        {/* Social Links */}
                        <div className="mt-6">
                            <h5 className="text-sm font-medium text-slate-400 mb-3">
                                Theo dõi chúng tôi
                            </h5>
                            <div className="flex gap-3">
                                {[Facebook, Instagram, Twitter, Youtube].map((Icon, index) => (
                                    <a
                                        key={index}
                                        href="#"
                                        className="w-10 h-10 rounded-xl bg-slate-800 flex items-center justify-center hover:bg-violet-600 transition-colors group"
                                    >
                                        <Icon className="w-5 h-5 text-slate-400 group-hover:text-white transition-colors" />
                                    </a>
                                ))}
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            {/* Bottom Bar */}
            <div className="border-t border-slate-800">
                <div className="max-w-7xl mx-auto px-4 py-6">
                    <div className="flex flex-col md:flex-row justify-between items-center gap-4">
                        <p className="text-slate-400 text-sm">
                            © 2026 ShopTTS. Tất cả quyền được bảo lưu.
                        </p>
                        <div className="flex items-center gap-4">
                            {footerLinks.legal.map((link, index) => (
                                <a
                                    key={index}
                                    href={link.href}
                                    className="text-slate-400 hover:text-violet-400 text-sm transition-colors"
                                >
                                    {link.label}
                                </a>
                            ))}
                        </div>
                    </div>
                </div>
            </div>
        </footer>
    );
}
