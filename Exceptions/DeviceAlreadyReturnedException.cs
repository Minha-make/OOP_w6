/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

/// <summary>
/// Ngoại lệ phát sinh khi yêu cầu trả một thiết bị trong phiếu nhưng thiết bị đó đã được trả trước đó.
/// </summary>
public class DeviceAlreadyReturnedException : BorrowingException
{
    /// <summary>
    /// Mã định danh của thiết bị đã được trả trước đó.
    /// </summary>
    public string DeviceId { get; }

    /// <summary>
    /// Khởi tạo ngoại lệ với mã thiết bị, tự động sinh thông điệp tiếng Việt.
    /// </summary>
    /// <param name="deviceId">Mã thiết bị đã được trả.</param>
    public DeviceAlreadyReturnedException(string deviceId)
        : base($"Thiết bị '{deviceId}' trong phiếu mượn đã được hoàn trả trước đó. Không thể thực hiện trả lại.")
    {
        // Gán mã thiết bị giúp kiểm soát trạng thái hoàn trả của từng mục mượn
        DeviceId = deviceId;
    }
}
