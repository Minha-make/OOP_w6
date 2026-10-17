/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

/// <summary>
/// Ngoại lệ phát sinh khi người mượn đã đạt hoặc vượt quá số lượng thiết bị tối đa được phép mượn trên một phiếu.
/// </summary>
public class BorrowLimitExceededException : BorrowingException
{
    /// <summary>
    /// Mã định danh của người mượn vi phạm hạn mức.
    /// </summary>
    public string BorrowerId { get; }

    /// <summary>
    /// Số lượng thiết bị tối đa người mượn được phép mượn.
    /// </summary>
    public int MaximumDevices { get; }

    /// <summary>
    /// Khởi tạo ngoại lệ với mã người mượn và số lượng tối đa, tự động sinh thông điệp tiếng Việt.
    /// </summary>
    /// <param name="borrowerId">Mã người mượn.</param>
    /// <param name="maximumDevices">Số lượng thiết bị tối đa cho phép.</param>
    public BorrowLimitExceededException(string borrowerId, int maximumDevices)
        : base($"Người mượn '{borrowerId}' đã đạt giới hạn mượn tối đa ({maximumDevices} thiết bị/phiếu). Không thể thêm thiết bị mới.")
    {
        // Gán ngữ cảnh lỗi hỗ trợ tầng UI/Application hiển thị chi tiết cho người dùng
        BorrowerId = borrowerId;
        MaximumDevices = maximumDevices;
    }
}
