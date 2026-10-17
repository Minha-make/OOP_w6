/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định thiết kế:
// 1. Microphone có thời hạn mượn tối đa 1 ngày (dùng và trả trong ngày).
// 2. Chi phí bảo trì hằng năm là 8% giá trị mua ban đầu (do hao mòn củ mic, pin, mạch phát sóng).
// 3. Mức phí phạt trả muộn là 20.000 VNĐ cho mỗi ngày trễ hạn.

/// <summary>
/// Đại diện cho thiết bị micro phục vụ giảng dạy.
/// </summary>
public class Microphone : TeachingDevice
{
    /// <summary>
    /// Số ngày mượn tối đa cho micro là 1 ngày.
    /// </summary>
    public override int MaxBorrowDays => 1;

    /// <summary>
    /// Phí trả muộn của micro là 20.000 VNĐ/ngày.
    /// </summary>
    public override decimal LateFeePerDay => 20_000m;

    /// <summary>
    /// Khởi tạo một đối tượng micro.
    /// </summary>
    /// <param name="id">Mã thiết bị.</param>
    /// <param name="name">Tên thiết bị.</param>
    /// <param name="yearInUse">Năm đưa vào sử dụng.</param>
    /// <param name="purchasePrice">Giá mua thiết bị.</param>
    /// <param name="status">Trạng thái khởi tạo (mặc định: Available).</param>
    public Microphone(string id, string name, int yearInUse, decimal purchasePrice, DeviceStatus status = DeviceStatus.Available)
        : base(id, name, yearInUse, purchasePrice, status)
    {
    }

    /// <summary>
    /// Chi phí bảo trì hàng năm của micro được tính bằng 8% giá mua ban đầu.
    /// </summary>
    public override decimal CalculateAnnualMaintenanceCost()
    {
        return PurchasePrice * 0.08m;
    }
}
