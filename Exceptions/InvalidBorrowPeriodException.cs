/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

/// <summary>
/// Ngoại lệ phát sinh khi số ngày mượn không hợp lệ.
/// </summary>
public class InvalidBorrowPeriodException : BorrowingException
{
    /// <summary>
    /// Mã thiết bị yêu cầu mượn.
    /// </summary>
    public string DeviceId { get; }

    /// <summary>
    /// Số ngày yêu cầu mượn.
    /// </summary>
    public int RequestedDays { get; }

    /// <summary>
    /// Số ngày tối đa được phép mượn.
    /// </summary>
    public int MaximumDays { get; }

    /// <summary>
    /// Khởi tạo ngoại lệ số ngày mượn không hợp lệ.
    /// </summary>
    public InvalidBorrowPeriodException(string deviceId, int requestedDays, int maximumDays)
        : base(requestedDays <= 0
            ? $"Thời hạn mượn ({requestedDays} ngày) cho thiết bị '{deviceId}' không hợp lệ (phải lớn hơn 0)."
            : $"Thời hạn mượn yêu cầu ({requestedDays} ngày) cho thiết bị '{deviceId}' vượt quá giới hạn tối đa cho phép ({maximumDays} ngày).")
    {
        DeviceId = deviceId;
        RequestedDays = requestedDays;
        MaximumDays = maximumDays;
    }
}
