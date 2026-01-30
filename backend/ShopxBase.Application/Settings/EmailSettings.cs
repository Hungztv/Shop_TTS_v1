namespace ShopxBase.Application.Settings;

public class EmailSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string Mail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string DisplayName { get; set; } = "ShopX Support";
    public bool EnableSsl { get; set; } = true;
}
