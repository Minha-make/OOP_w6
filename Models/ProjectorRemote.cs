/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định thiết kế:
// 1. ProjectorRemote có thời hạn mượn tối đa 2 ngày.
// 2. Chi phí bảo trì hằng năm là 6% giá trị mua ban đầu.
// 3. Mức phí phạt trả muộn là 10.000 VNĐ cho mỗi ngày trễ hạn.

/// <summary>
/// Đại diện cho thiết bị điều khiển máy chiếu trong phòng học.
/// </summary>
public class ProjectorRemote : TeachingDevice
{
    /// <summary>
    /// Số ngày mượn tối đa cho điều khiển máy chiếu là 2 ngày.
    /// </summary>
    public override int MaxBorrowDays => 2;

    /// <summary>
    /// Phí trả muộn của điều khiển máy chiếu là 10.000 VNĐ/ngày.
    /// </summary>
    public override decimal LateFeePerDay => 10_000m;

    /// <summary>
    /// Khởi tạo một đối tượng điều khiển máy chiếu.
    /// </summary>
    /// <param name="id">Mã thiết bị.</param>
    /// <param name="name">Tên thiết bị.</param>
    /// <param name="yearInUse">Năm đưa vào sử dụng.</param>
    /// <param name="purchasePrice">Giá mua thiết bị.</param>
    /// <param name="status">Trạng thái khởi tạo (mặc định: Available).</param>
    public ProjectorRemote(string id, string name, int yearInUse, decimal purchasePrice, DeviceStatus status = DeviceStatus.Available)
        : base(id, name, yearInUse, purchasePrice, status)
    {
    }

    /// <summary>
    /// Chi phí bảo trì hàng năm của điều khiển máy chiếu được tính bằng 6% giá mua ban đầu.
    /// </summary>
    public override decimal CalculateAnnualMaintenanceCost()
    {
        return PurchasePrice * 0.06m;
    }
}
