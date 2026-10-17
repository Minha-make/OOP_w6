/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định:
// 1. Giảng viên được mượn tối đa 3 thiết bị trên 1 phiếu.
// 2. Mã và họ tên chỉ gán khi khởi tạo.

/// <summary>
/// Lớp đại diện cho Giảng viên mượn thiết bị.
/// </summary>
public class Lecturer : IBorrower
{
    public string BorrowerId { get; }
    public string FullName { get; }

    /// <summary>
    /// Khởi tạo giảng viên.
    /// </summary>
    public Lecturer(string borrowerId, string fullName)
    {
        if (string.IsNullOrWhiteSpace(borrowerId))
        {
            throw new ArgumentException("Mã giảng viên không được để trống.", nameof(borrowerId));
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Họ tên giảng viên không được để trống.", nameof(fullName));
        }

        BorrowerId = borrowerId;
        FullName = fullName;
    }

    /// <summary>
    /// Giảng viên được mượn tối đa 3 thiết bị/phiếu.
    /// </summary>
    public int GetMaximumDevices()
    {
        return 3;
    }

    public override string ToString()
    {
        return $"[Giảng viên] {FullName} (Mã: {BorrowerId}) - Hạn mức: {GetMaximumDevices()} thiết bị";
    }
}
