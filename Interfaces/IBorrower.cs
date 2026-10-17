/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

/// <summary>
/// Giao diện định nghĩa các thông tin và quy tắc của một đối tượng có quyền mượn thiết bị giảng dạy.
/// </summary>
public interface IBorrower
{
    /// <summary>
    /// Mã định danh duy nhất của người mượn (giảng viên hoặc trợ giảng).
    /// </summary>
    string BorrowerId { get; }

    /// <summary>
    /// Họ và tên đầy đủ của người mượn.
    /// </summary>
    string FullName { get; }

    /// <summary>
    /// Lấy số lượng thiết bị tối đa mà đối tượng này được phép mượn trên một phiếu.
    /// </summary>
    /// <returns>Số lượng thiết bị tối đa cho phép.</returns>
    int GetMaximumDevices();
}
