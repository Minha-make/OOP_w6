/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

/// <summary>
/// Ngoại lệ phát sinh khi cố gắng thêm một thiết bị đã có sẵn trong phiếu mượn.
/// </summary>
public class DuplicateDeviceException : BorrowingException
{
    /// <summary>
    /// Mã định danh của thiết bị bị trùng lặp trong phiếu.
    /// </summary>
    public string DeviceId { get; }

    /// <summary>
    /// Khởi tạo ngoại lệ với mã thiết bị, tự động sinh thông điệp tiếng Việt.
    /// </summary>
    /// <param name="deviceId">Mã thiết bị trùng lặp.</param>
    public DuplicateDeviceException(string deviceId)
        : base($"Thiết bị '{deviceId}' đã tồn tại trong phiếu mượn. Không được phép thêm cùng một thiết bị nhiều lần.")
    {
        // Gán mã thiết bị giúp xác định chính xác phần tử gây xung đột dữ liệu
        DeviceId = deviceId;
    }
}
