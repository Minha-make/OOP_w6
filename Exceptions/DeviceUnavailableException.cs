/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

/// <summary>
/// Ngoại lệ phát sinh khi cố gắng mượn thiết bị không ở trạng thái sẵn sàng (Available).
/// </summary>
public class DeviceUnavailableException : BorrowingException
{
    /// <summary>
    /// Mã định danh của thiết bị không sẵn sàng.
    /// </summary>
    public string DeviceId { get; }

    /// <summary>
    /// Trạng thái hiện tại của thiết bị tại thời điểm yêu cầu mượn.
    /// </summary>
    public DeviceStatus CurrentStatus { get; }

    /// <summary>
    /// Khởi tạo ngoại lệ với mã thiết bị và trạng thái hiện tại, tự động sinh thông điệp tiếng Việt.
    /// </summary>
    /// <param name="deviceId">Mã thiết bị.</param>
    /// <param name="currentStatus">Trạng thái hiện tại của thiết bị.</param>
    public DeviceUnavailableException(string deviceId, DeviceStatus currentStatus)
        : base($"Thiết bị '{deviceId}' không ở trạng thái sẵn sàng để mượn (Trạng thái hiện tại: {currentStatus}).")
    {
        // Lưu trữ thuộc tính ngữ cảnh để phục vụ tra cứu và xử lý lỗi ở tầng cao hơn
        DeviceId = deviceId;
        CurrentStatus = currentStatus;
    }
}
