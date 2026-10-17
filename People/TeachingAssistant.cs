/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định:
// 1. Trợ giảng được mượn tối đa 2 thiết bị trên 1 phiếu.
// 2. Mã và họ tên chỉ gán khi khởi tạo.

/// <summary>
/// Lớp đại diện cho Trợ giảng mượn thiết bị.
/// </summary>
public class TeachingAssistant : IBorrower
{
    public string BorrowerId { get; }
    public string FullName { get; }

    /// <summary>
    /// Khởi tạo trợ giảng.
    /// </summary>
    public TeachingAssistant(string borrowerId, string fullName)
    {
        if (string.IsNullOrWhiteSpace(borrowerId))
        {
            throw new ArgumentException("Mã trợ giảng không được để trống.", nameof(borrowerId));
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Họ tên trợ giảng không được để trống.", nameof(fullName));
        }

        BorrowerId = borrowerId;
        FullName = fullName;
    }

    /// <summary>
    /// Trợ giảng được mượn tối đa 2 thiết bị/phiếu.
    /// </summary>
    public int GetMaximumDevices()
    {
        return 2;
    }

    public override string ToString()
    {
        return $"[Trợ giảng] {FullName} (Mã: {BorrowerId}) - Hạn mức: {GetMaximumDevices()} thiết bị";
    }
}
