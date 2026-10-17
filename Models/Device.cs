/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định:
// 1. Số năm sử dụng tính bằng năm hiện tại trừ đi năm đưa vào sử dụng.
// 2. Mặc định trạng thái thiết bị là Available khi tạo mới.

/// <summary>
/// Lớp trừu tượng đại diện cho thiết bị trong trường học.
/// </summary>
public abstract class Device
{
    /// <summary>
    /// Mã thiết bị (chỉ gán khi khởi tạo).
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Tên thiết bị.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Năm đưa vào sử dụng.
    /// </summary>
    public int YearInUse { get; set; }

    /// <summary>
    /// Giá mua ban đầu của thiết bị.
    /// </summary>
    public decimal PurchasePrice { get; set; }

    /// <summary>
    /// Trạng thái hoạt động của thiết bị.
    /// </summary>
    public DeviceStatus Status { get; protected set; }

    /// <summary>
    /// Số năm sử dụng tính đến năm hiện tại.
    /// </summary>
    public int UsageYears => DateTime.Now.Year - YearInUse;

    /// <summary>
    /// Khởi tạo thiết bị kèm kiểm tra dữ liệu đầu vào.
    /// </summary>
    protected Device(string id, string name, int yearInUse, decimal purchasePrice, DeviceStatus status = DeviceStatus.Available)
    {
        // Kiểm tra mã thiết bị không được rỗng
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Mã thiết bị không được để trống.", nameof(id));
        }

        // Kiểm tra tên thiết bị không được rỗng
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Tên thiết bị không được để trống.", nameof(name));
        }

        // Năm đưa vào sử dụng không được lớn hơn năm hiện tại
        if (yearInUse > DateTime.Now.Year)
        {
            throw new ArgumentOutOfRangeException(nameof(yearInUse), "Năm đưa vào sử dụng không được lớn hơn năm hiện tại.");
        }

        // Giá mua phải lớn hơn 0
        if (purchasePrice <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(purchasePrice), "Giá mua phải lớn hơn 0.");
        }

        Id = id;
        Name = name;
        YearInUse = yearInUse;
        PurchasePrice = purchasePrice;
        Status = status;
    }

    /// <summary>
    /// Phương thức trừu tượng tính chi phí bảo trì dự kiến trong 1 năm.
    /// </summary>
    public abstract decimal CalculateAnnualMaintenanceCost();

    /// <summary>
    /// Chuyển thiết bị sang trạng thái bảo trì.
    /// </summary>
    public void MarkAsUnderMaintenance()
    {
        if (Status == DeviceStatus.Retired)
        {
            throw new InvalidOperationException("Thiết bị đã thanh lý, không thể bảo trì.");
        }
        Status = DeviceStatus.UnderMaintenance;
    }

    /// <summary>
    /// Thanh lý thiết bị (không dùng nữa).
    /// </summary>
    public void Retire()
    {
        Status = DeviceStatus.Retired;
    }

    /// <summary>
    /// Ghi đè ToString để in thông tin thiết bị.
    /// </summary>
    public override string ToString()
    {
        return $"[Mã: {Id}] {Name} | Năm SD: {YearInUse} ({UsageYears} năm tuổi) | Giá mua: {PurchasePrice:N0} đ | Trạng thái: {Status} | Chi phí bảo trì/năm: {CalculateAnnualMaintenanceCost():N0} đ";
    }
}
