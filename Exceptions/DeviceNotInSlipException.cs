/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

/// <summary>
/// Ngoại lệ phát sinh khi yêu cầu trả một thiết bị không có trong danh sách mượn của phiếu.
/// </summary>
public class DeviceNotInSlipException : BorrowingException
{
    /// <summary>
    /// Mã định danh của thiết bị không tìm thấy trong phiếu.
    /// </summary>
    public string DeviceId { get; }

    /// <summary>
    /// Khởi tạo ngoại lệ với mã thiết bị, tự động sinh thông điệp tiếng Việt.
    /// </summary>
    /// <param name="deviceId">Mã thiết bị không thuộc phiếu.</param>
    public DeviceNotInSlipException(string deviceId)
        : base($"Thiết bị '{deviceId}' không thuộc danh sách thiết bị đã mượn trong phiếu này.")
    {
        // Gán mã thiết bị phục vụ kiểm tra đối chiếu dữ liệu phiếu
        DeviceId = deviceId;
    }
}
