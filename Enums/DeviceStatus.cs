/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

/// <summary>
/// Đại diện cho trạng thái hoạt động và vòng đời của một thiết bị trong hệ thống.
/// </summary>
public enum DeviceStatus
{
    /// <summary>
    /// Thiết bị bình thường, sẵn sàng để cho mượn hoặc phân bổ.
    /// </summary>
    Available,

    /// <summary>
    /// Thiết bị đang được mượn và sử dụng trong buổi học/giảng dạy.
    /// </summary>
    Borrowed,

    /// <summary>
    /// Thiết bị gặp sự cố hoặc đang trong chu kỳ bảo trì, không thể mượn.
    /// </summary>
    UnderMaintenance,

    /// <summary>
    /// Thiết bị đã hư hỏng hoàn toàn, hết hạn sử dụng hoặc đã thanh lý.
    /// </summary>
    Retired
}
