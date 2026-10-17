/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định thiết kế:
// 1. MarkAsAvailable() cho phép chuyển trạng thái về Available từ Borrowed hoặc UnderMaintenance.
// 2. Không cho phép chuyển sang Available nếu thiết bị đã Retired hoặc đã sẵn sàng (Available).

/// <summary>
/// Lớp trừu tượng cho các thiết bị phục vụ công tác giảng dạy, có hỗ trợ quy trình mượn - trả và tính phí trễ hạn.
/// </summary>
public abstract class TeachingDevice : Device
{
    /// <summary>
    /// Số ngày mượn tối đa cho phép đối với loại thiết bị này.
    /// </summary>
    public abstract int MaxBorrowDays { get; }

    /// <summary>
    /// Phí phạt trả muộn cho mỗi ngày trễ hạn so với DueDate (VNĐ/ngày).
    /// </summary>
    public abstract decimal LateFeePerDay { get; }

    /// <summary>
    /// Khởi tạo một thiết bị giảng dạy.
    /// </summary>
    protected TeachingDevice(string id, string name, int yearInUse, decimal purchasePrice, DeviceStatus status = DeviceStatus.Available)
        : base(id, name, yearInUse, purchasePrice, status)
    {
    }

    /// <summary>
    /// Đánh dấu thiết bị được mượn. Chỉ cho phép khi thiết bị đang ở trạng thái Available.
    /// </summary>
    /// <exception cref="DeviceUnavailableException">Ném ra khi thiết bị không ở trạng thái Available.</exception>
    public void MarkAsBorrowed()
    {
        // Kiểm tra tính sẵn sàng trước khi cho mượn để bảo đảm tính toàn vẹn trạng thái
        if (Status != DeviceStatus.Available)
        {
            throw new DeviceUnavailableException(Id, Status);
        }

        Status = DeviceStatus.Borrowed;
    }

    /// <summary>
    /// Đánh dấu thiết bị đã sẵn sàng (Available) sau khi được hoàn trả hoặc kết thúc bảo trì.
    /// </summary>
    /// <exception cref="InvalidOperationException">Ném ra nếu thiết bị đã thanh lý hoặc đã ở sẵn trạng thái Available.</exception>
    public void MarkAsAvailable()
    {
        // Thiết bị đã thanh lý thì không thể tiếp tục đưa vào sử dụng
        if (Status == DeviceStatus.Retired)
        {
            throw new InvalidOperationException($"Không thể chuyển thiết bị '{Id}' sang trạng thái Available vì thiết bị đã bị thanh lý (Retired).");
        }

        // Tránh thao tác thừa nếu thiết bị vốn đã sẵn sàng
        if (Status == DeviceStatus.Available)
        {
            throw new InvalidOperationException($"Thiết bị '{Id}' hiện đã ở trạng thái Available.");
        }

        Status = DeviceStatus.Available;
    }

    /// <summary>
    /// Trả về chuỗi thông tin chi tiết bao gồm quy định mượn của thiết bị giảng dạy.
    /// </summary>
    public override string ToString()
    {
        return $"{base.ToString()} | Mượn tối đa: {MaxBorrowDays} ngày | Phí trễ: {LateFeePerDay:N0} đ/ngày";
    }
}
