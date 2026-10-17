/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định:
// 1. MarkAsAvailable() cho phép chuyển về Available khi trả thiết bị hoặc sửa xong.
// 2. Thiết bị đã thanh lý (Retired) thì không được chuyển về Available.

/// <summary>
/// Lớp trừu tượng cho thiết bị giảng dạy có thể mượn trả.
/// </summary>
public abstract class TeachingDevice : Device
{
    /// <summary>
    /// Số ngày mượn tối đa cho phép.
    /// </summary>
    public abstract int MaxBorrowDays { get; }

    /// <summary>
    /// Phí phạt trễ hạn mỗi ngày (VNĐ/ngày).
    /// </summary>
    public abstract decimal LateFeePerDay { get; }

    /// <summary>
    /// Khởi tạo thiết bị giảng dạy.
    /// </summary>
    protected TeachingDevice(string id, string name, int yearInUse, decimal purchasePrice, DeviceStatus status = DeviceStatus.Available)
        : base(id, name, yearInUse, purchasePrice, status)
    {
    }

    /// <summary>
    /// Đánh dấu thiết bị đang được mượn.
    /// </summary>
    public void MarkAsBorrowed()
    {
        // Chỉ cho mượn khi thiết bị đang sẵn sàng (Available)
        if (Status != DeviceStatus.Available)
        {
            throw new DeviceUnavailableException(Id, Status);
        }

        Status = DeviceStatus.Borrowed;
    }

    /// <summary>
    /// Đánh dấu thiết bị đã sẵn sàng sau khi trả hoặc bảo trì xong.
    /// </summary>
    public void MarkAsAvailable()
    {
        // Thiết bị đã thanh lý không thể dùng lại
        if (Status == DeviceStatus.Retired)
        {
            throw new InvalidOperationException("Thiết bị " + Id + " đã thanh lý, không thể chuyển sang Available.");
        }

        if (Status == DeviceStatus.Available)
        {
            throw new InvalidOperationException("Thiết bị " + Id + " đang ở trạng thái Available rồi.");
        }

        Status = DeviceStatus.Available;
    }

    /// <summary>
    /// Trả về chuỗi thông tin thiết bị giảng dạy.
    /// </summary>
    public override string ToString()
    {
        return base.ToString() + " | Mượn tối đa: " + MaxBorrowDays + " ngày | Phí trễ: " + LateFeePerDay.ToString("N0") + " đ/ngày";
    }
}
