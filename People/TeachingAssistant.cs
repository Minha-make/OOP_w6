/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định thiết kế:
// 1. Trợ giảng (TeachingAssistant) có hạn mức mượn cố định là 2 thiết bị trên mỗi phiếu.
// 2. BorrowerId và FullName là bất biến (chỉ gán một lần trong constructor) để bảo đảm tính nhất quán định danh.

/// <summary>
/// Đại diện cho đối tượng Trợ giảng tham gia mượn thiết bị giảng dạy.
/// </summary>
public class TeachingAssistant : IBorrower
{
    /// <summary>
    /// Mã định danh trợ giảng.
    /// </summary>
    public string BorrowerId { get; }

    /// <summary>
    /// Họ tên đầy đủ của trợ giảng.
    /// </summary>
    public string FullName { get; }

    /// <summary>
    /// Khởi tạo thông tin trợ giảng kèm kiểm tra tính hợp lệ của dữ liệu.
    /// </summary>
    /// <param name="borrowerId">Mã trợ giảng (không rỗng).</param>
    /// <param name="fullName">Họ tên trợ giảng (không rỗng).</param>
    /// <exception cref="ArgumentException">Ném ra khi mã hoặc họ tên rỗng/khoảng trắng.</exception>
    public TeachingAssistant(string borrowerId, string fullName)
    {
        // Kiểm tra BorrowerId không được rỗng để bảo đảm tính toàn vẹn khóa định danh
        if (string.IsNullOrWhiteSpace(borrowerId))
        {
            throw new ArgumentException("Mã trợ giảng không được để trống hoặc chỉ chứa khoảng trắng.", nameof(borrowerId));
        }

        // Kiểm tra FullName không được rỗng để phục vụ hiển thị chính xác
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Họ tên trợ giảng không được để trống hoặc chỉ chứa khoảng trắng.", nameof(fullName));
        }

        BorrowerId = borrowerId;
        FullName = fullName;
    }

    /// <summary>
    /// Lấy số lượng thiết bị tối đa trợ giảng được phép mượn trên một phiếu (tối đa 2 thiết bị).
    /// </summary>
    /// <returns>Hạn mức 2 thiết bị.</returns>
    public int GetMaximumDevices()
    {
        return 2;
    }

    /// <summary>
    /// Trả về chuỗi đại diện cho trợ giảng.
    /// </summary>
    public override string ToString()
    {
        return $"[Trợ giảng] {FullName} (Mã: {BorrowerId}) - Hạn mức mượn: {GetMaximumDevices()} thiết bị";
    }
}
