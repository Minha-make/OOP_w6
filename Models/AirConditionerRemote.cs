/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định thiết kế:
// 1. AirConditionerRemote có thời hạn mượn tối đa 3 ngày.
// 2. Chi phí bảo trì hằng năm là 5% giá trị mua ban đầu.
// 3. Mức phí phạt trả muộn là 10.000 VNĐ cho mỗi ngày trễ hạn.

/// <summary>
/// Đại diện cho thiết bị điều khiển điều hòa trong phòng học.
/// </summary>
public class AirConditionerRemote : TeachingDevice
{
    /// <summary>
    /// Số ngày mượn tối đa cho điều khiển điều hòa là 3 ngày.
    /// </summary>
    public override int MaxBorrowDays => 3;

    /// <summary>
    /// Phí trả muộn của điều khiển điều hòa là 10.000 VNĐ/ngày.
    /// </summary>
    public override decimal LateFeePerDay => 10_000m;

    /// <summary>
    /// Khởi tạo một đối tượng điều khiển điều hòa.
    /// </summary>
    /// <param name="id">Mã thiết bị.</param>
    /// <param name="name">Tên thiết bị.</param>
    /// <param name="yearInUse">Năm đưa vào sử dụng.</param>
    /// <param name="purchasePrice">Giá mua thiết bị.</param>
    /// <param name="status">Trạng thái khởi tạo (mặc định: Available).</param>
    public AirConditionerRemote(string id, string name, int yearInUse, decimal purchasePrice, DeviceStatus status = DeviceStatus.Available)
        : base(id, name, yearInUse, purchasePrice, status)
    {
    }

    /// <summary>
    /// Chi phí bảo trì hàng năm của điều khiển điều hòa được tính bằng 5% giá mua ban đầu.
    /// </summary>
    public override decimal CalculateAnnualMaintenanceCost()
    {
        return PurchasePrice * 0.05m;
    }
}
