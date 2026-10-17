/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

/// <summary>
/// Ngoại lệ phát sinh khi ngày trả thiết bị không hợp lệ (xảy ra trước ngày mượn).
/// </summary>
public class InvalidReturnDateException : BorrowingException
{
    /// <summary>
    /// Mã định danh của thiết bị có ngày trả không hợp lệ.
    /// </summary>
    public string DeviceId { get; }

    /// <summary>
    /// Ngày thiết bị bắt đầu được mượn.
    /// </summary>
    public DateTime BorrowedDate { get; }

    /// <summary>
    /// Ngày thực hiện trả thiết bị được ghi nhận.
    /// </summary>
    public DateTime ReturnedDate { get; }

    /// <summary>
    /// Khởi tạo ngoại lệ với mã thiết bị, ngày mượn và ngày trả, tự động sinh thông điệp tiếng Việt.
    /// </summary>
    /// <param name="deviceId">Mã thiết bị.</param>
    /// <param name="borrowedDate">Ngày mượn thiết bị.</param>
    /// <param name="returnedDate">Ngày trả thiết bị.</param>
    public InvalidReturnDateException(string deviceId, DateTime borrowedDate, DateTime returnedDate)
        : base($"Ngày trả ({returnedDate:dd/MM/yyyy HH:mm}) của thiết bị '{deviceId}' không hợp lệ vì xảy ra trước ngày mượn ({borrowedDate:dd/MM/yyyy HH:mm}).")
    {
        // Lưu trữ đầy đủ thông tin thời gian để kiểm tra logic tính toán thời hạn và phí trễ
        DeviceId = deviceId;
        BorrowedDate = borrowedDate;
        ReturnedDate = returnedDate;
    }
}
