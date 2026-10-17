/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

/// <summary>
/// Ngoại lệ cơ sở cho toàn bộ hệ thống phân cấp lỗi nghiệp vụ mượn và trả thiết bị.
/// </summary>
public class BorrowingException : Exception
{
    /// <summary>
    /// Khởi tạo ngoại lệ mượn trả với thông điệp lỗi cụ thể.
    /// </summary>
    /// <param name="message">Thông điệp chi tiết mô tả lỗi nghiệp vụ.</param>
    public BorrowingException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Khởi tạo ngoại lệ mượn trả với thông điệp lỗi và ngoại lệ gốc đi kèm.
    /// </summary>
    /// <param name="message">Thông điệp chi tiết mô tả lỗi nghiệp vụ.</param>
    /// <param name="innerException">Ngoại lệ bên trong gây ra lỗi này (nếu có).</param>
    public BorrowingException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
