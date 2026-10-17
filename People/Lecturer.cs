/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định thiết kế:
// 1. Giảng viên (Lecturer) có hạn mức mượn cố định là 3 thiết bị trên mỗi phiếu.
// 2. BorrowerId và FullName là bất biến (chỉ gán một lần trong constructor) để bảo đảm tính nhất quán định danh.

/// <summary>
/// Đại diện cho đối tượng Giảng viên tham gia mượn thiết bị giảng dạy.
/// </summary>
public class Lecturer : IBorrower
{
    /// <summary>
    /// Mã định danh giảng viên.
    /// </summary>
    public string BorrowerId { get; }

    /// <summary>
    /// Họ tên đầy đủ của giảng viên.
    /// </summary>
    public string FullName { get; }

    /// <summary>
    /// Khởi tạo thông tin giảng viên kèm kiểm tra tính hợp lệ của dữ liệu.
    /// </summary>
    /// <param name="borrowerId">Mã giảng viên (không rỗng).</param>
    /// <param name="fullName">Họ tên giảng viên (không rỗng).</param>
    /// <exception cref="ArgumentException">Ném ra khi mã hoặc họ tên rỗng/khoảng trắng.</exception>
    public Lecturer(string borrowerId, string fullName)
    {
        // Kiểm tra BorrowerId không được rỗng để bảo đảm tính toàn vẹn khóa định danh
        if (string.IsNullOrWhiteSpace(borrowerId))
        {
            throw new ArgumentException("Mã giảng viên không được để trống hoặc chỉ chứa khoảng trắng.", nameof(borrowerId));
        }

        // Kiểm tra FullName không được rỗng để phục vụ hiển thị chính xác
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Họ tên giảng viên không được để trống hoặc chỉ chứa khoảng trắng.", nameof(fullName));
        }

        BorrowerId = borrowerId;
        FullName = fullName;
    }

    /// <summary>
    /// Lấy số lượng thiết bị tối đa giảng viên được phép mượn trên một phiếu (tối đa 3 thiết bị).
    /// </summary>
    /// <returns>Hạn mức 3 thiết bị.</returns>
    public int GetMaximumDevices()
    {
        return 3;
    }

    /// <summary>
    /// Trả về chuỗi đại diện cho giảng viên.
    /// </summary>
    public override string ToString()
    {
        return $"[Giảng viên] {FullName} (Mã: {BorrowerId}) - Hạn mức mượn: {GetMaximumDevices()} thiết bị";
    }
}
