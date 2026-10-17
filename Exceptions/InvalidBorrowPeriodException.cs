/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

/// <summary>
/// Ngoại lệ phát sinh khi số ngày mượn không hợp lệ (không dương hoặc vượt quá số ngày cho phép tối đa của thiết bị).
/// </summary>
public class InvalidBorrowPeriodException : BorrowingException
{
    /// <summary>
    /// Mã định danh của thiết bị được yêu cầu mượn.
    /// </summary>
    public string DeviceId { get; }

    /// <summary>
    /// Số ngày mượn được yêu cầu.
    /// </summary>
    public int RequestedDays { get; }

    /// <summary>
    /// Số ngày mượn tối đa được cấu hình cho thiết bị này.
    /// </summary>
    public int MaximumDays { get; }

    /// <summary>
    /// Khởi tạo ngoại lệ với mã thiết bị, số ngày yêu cầu và số ngày tối đa, tự động sinh thông điệp tiếng Việt.
    /// </summary>
    /// <param name="deviceId">Mã thiết bị.</param>
    /// <param name="requestedDays">Số ngày yêu cầu mượn.</param>
    /// <param name="maximumDays">Số ngày tối đa cho phép.</param>
    public InvalidBorrowPeriodException(string deviceId, int requestedDays, int maximumDays)
        : base(FormatMessage(deviceId, requestedDays, maximumDays))
    {
        DeviceId = deviceId;
        RequestedDays = requestedDays;
        MaximumDays = maximumDays;
    }

    /// <summary>
    /// Tự động sinh thông điệp lỗi tiếng Việt phù hợp theo từng trường hợp vi phạm thời gian mượn.
    /// </summary>
    private static string FormatMessage(string deviceId, int requestedDays, int maximumDays)
    {
        if (requestedDays <= 0)
        {
            return $"Thời hạn mượn yêu cầu ({requestedDays} ngày) cho thiết bị '{deviceId}' không hợp lệ (Phải lớn hơn 0).";
        }

        return $"Thời hạn mượn yêu cầu ({requestedDays} ngày) cho thiết bị '{deviceId}' vượt quá giới hạn tối đa cho phép ({maximumDays} ngày).";
    }
}
