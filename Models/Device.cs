/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định thiết kế:
// 1. Số năm sử dụng (UsageYears) tính bằng năm hiện tại trừ đi YearInUse (>= 0).
// 2. Trạng thái mặc định khi tạo mới thiết bị là DeviceStatus.Available nếu không truyền vào.

/// <summary>
/// Lớp trừu tượng đại diện cho một thiết bị tổng quát trong hệ thống.
/// </summary>
public abstract class Device
{
    /// <summary>
    /// Mã định danh duy nhất của thiết bị. Chỉ được thiết lập một lần khi khởi tạo.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Tên mô tả của thiết bị.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Năm thiết bị bắt đầu được đưa vào sử dụng.
    /// </summary>
    public int YearInUse { get; set; }

    /// <summary>
    /// Giá mua ban đầu của thiết bị (VNĐ).
    /// </summary>
    public decimal PurchasePrice { get; set; }

    /// <summary>
    /// Trạng thái hoạt động hiện tại của thiết bị.
    /// </summary>
    public DeviceStatus Status { get; protected set; }

    /// <summary>
    /// Tuổi thọ sử dụng của thiết bị (tính theo số năm từ khi đưa vào sử dụng đến năm hiện tại).
    /// </summary>
    public int UsageYears => Math.Max(0, DateTime.Now.Year - YearInUse);

    /// <summary>
    /// Khởi tạo một thiết bị mới với đầy đủ kiểm tra tính hợp lệ của dữ liệu đầu vào.
    /// </summary>
    /// <param name="id">Mã thiết bị (bắt buộc, không rỗng).</param>
    /// <param name="name">Tên thiết bị (bắt buộc, không rỗng).</param>
    /// <param name="yearInUse">Năm đưa vào sử dụng (không lớn hơn năm hiện tại).</param>
    /// <param name="purchasePrice">Giá mua ban đầu (phải lớn hơn 0).</param>
    /// <param name="status">Trạng thái khởi tạo (mặc định là Available).</param>
    /// <exception cref="ArgumentException">Ném ra khi mã hoặc tên thiết bị rỗng/khoảng trắng.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Ném ra khi giá mua &lt;= 0 hoặc năm sử dụng không hợp lệ.</exception>
    protected Device(string id, string name, int yearInUse, decimal purchasePrice, DeviceStatus status = DeviceStatus.Available)
    {
        // Kiểm tra Id không được rỗng để bảo đảm tính toàn vẹn khóa định danh
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Mã thiết bị không được để trống hoặc chỉ chứa khoảng trắng.", nameof(id));
        }

        // Kiểm tra Name không được rỗng để hiển thị thông tin rõ ràng
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Tên thiết bị không được để trống hoặc chỉ chứa khoảng trắng.", nameof(name));
        }

        // Năm đưa vào sử dụng không được trong tương lai
        int currentYear = DateTime.Now.Year;
        if (yearInUse <= 0 || yearInUse > currentYear)
        {
            throw new ArgumentOutOfRangeException(nameof(yearInUse), $"Năm đưa vào sử dụng ({yearInUse}) không hợp lệ (Phải từ 1 đến {currentYear}).");
        }

        // Giá mua thiết bị bắt buộc phải là số dương theo yêu cầu đề bài
        if (purchasePrice <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(purchasePrice), "Giá mua của thiết bị phải lớn hơn 0.");
        }

        Id = id;
        Name = name;
        YearInUse = yearInUse;
        PurchasePrice = purchasePrice;
        Status = status;
    }

    /// <summary>
    /// Tính toán chi phí bảo trì dự kiến trong một năm của thiết bị (triển khai cụ thể ở từng lớp con).
    /// </summary>
    /// <returns>Số tiền bảo trì ước tính (VNĐ).</returns>
    public abstract decimal CalculateAnnualMaintenanceCost();

    /// <summary>
    /// Đưa thiết bị vào trạng thái bảo trì.
    /// </summary>
    public void MarkAsUnderMaintenance()
    {
        if (Status == DeviceStatus.Retired)
        {
            throw new InvalidOperationException($"Không thể bảo trì thiết bị '{Id}' vì đã thanh lý (Retired).");
        }

        Status = DeviceStatus.UnderMaintenance;
    }

    /// <summary>
    /// Chuyển thiết bị sang trạng thái thanh lý (ngưng sử dụng hoàn toàn).
    /// </summary>
    public void Retire()
    {
        Status = DeviceStatus.Retired;
    }

    /// <summary>
    /// Trả về chuỗi thông tin chi tiết của thiết bị.
    /// </summary>
    public override string ToString()
    {
        return $"[Mã: {Id}] {Name} | Năm SD: {YearInUse} ({UsageYears} năm tuổi) | Giá mua: {PurchasePrice:N0} đ | Trạng thái: {Status} | Chi phí bảo trì/năm: {CalculateAnnualMaintenanceCost():N0} đ";
    }
}
