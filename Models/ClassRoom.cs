/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định thiết kế:
// 1. Quan hệ kết tập (Aggregation): ClassRoom chỉ lưu giữ tham chiếu đến Device. Thao tác xóa thiết bị khỏi phòng
//    hoặc hủy phòng không hủy các đối tượng Device trong bộ nhớ.
// 2. So sánh mã thiết bị không phân biệt hoa thường (StringComparison.OrdinalIgnoreCase).
// 3. Thiết bị cần bảo trì khi: Status == DeviceStatus.UnderMaintenance hoặc thời gian sử dụng > 5 năm (UsageYears > 5).
// 4. Nếu thiết bị đã tồn tại trong phòng, ném InvalidOperationException để báo lỗi nghiệp vụ trùng lặp.

/// <summary>
/// Đại diện cho một phòng học kết tập danh sách các thiết bị phục vụ giảng dạy và học tập.
/// </summary>
public class ClassRoom
{
    private readonly List<Device> _devices;

    /// <summary>
    /// Mã định danh phòng học.
    /// </summary>
    public string RoomId { get; }

    /// <summary>
    /// Tên phòng học.
    /// </summary>
    public string RoomName { get; set; }

    /// <summary>
    /// Sức chứa tối đa của phòng học (số lượng người, phải &gt; 0).
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Danh sách thiết bị hiện có trong phòng học (chỉ đọc để bảo vệ tính đóng gói).
    /// </summary>
    public IReadOnlyList<Device> Devices => _devices.AsReadOnly();

    /// <summary>
    /// Khởi tạo một phòng học mới với đầy đủ kiểm tra dữ liệu đầu vào.
    /// </summary>
    /// <param name="roomId">Mã phòng học (không rỗng).</param>
    /// <param name="roomName">Tên phòng học (không rỗng).</param>
    /// <param name="capacity">Sức chứa (lớn hơn 0).</param>
    /// <exception cref="ArgumentException">Ném ra khi mã hoặc tên phòng rỗng/chỉ chứa khoảng trắng.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Ném ra khi sức chứa &lt;= 0.</exception>
    public ClassRoom(string roomId, string roomName, int capacity)
    {
        // Kiểm tra RoomId để bảo đảm định danh duy nhất cho phòng học
        if (string.IsNullOrWhiteSpace(roomId))
        {
            throw new ArgumentException("Mã phòng học không được để trống hoặc chỉ chứa khoảng trắng.", nameof(roomId));
        }

        // Kiểm tra RoomName phục vụ định danh trực quan
        if (string.IsNullOrWhiteSpace(roomName))
        {
            throw new ArgumentException("Tên phòng học không được để trống hoặc chỉ chứa khoảng trắng.", nameof(roomName));
        }

        // Sức chứa bắt buộc phải là số dương
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Sức chứa của phòng học phải lớn hơn 0.");
        }

        RoomId = roomId;
        RoomName = roomName;
        Capacity = capacity;
        _devices = new List<Device>();
    }

    /// <summary>
    /// Thêm một thiết bị vào phòng học.
    /// </summary>
    /// <param name="device">Đối tượng thiết bị cần thêm.</param>
    /// <exception cref="ArgumentNullException">Ném ra khi device là null.</exception>
    /// <exception cref="InvalidOperationException">Ném ra khi thiết bị có mã đã tồn tại trong phòng.</exception>
    public void AddDevice(Device device)
    {
        // Quy tắc 2 của đề bài: Không chấp nhận đối tượng thiết bị null
        if (device == null)
        {
            throw new ArgumentNullException(nameof(device), "Đối tượng thiết bị không được là null.");
        }

        // Quy tắc 1 của đề bài: Không thêm hai thiết bị có cùng mã vào một phòng
        if (_devices.Any(d => string.Equals(d.Id, device.Id, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"Thiết bị có mã '{device.Id}' đã tồn tại trong phòng học '{RoomId}'. Không thể thêm trùng lặp.");
        }

        _devices.Add(device);
    }

    /// <summary>
    /// Xóa một thiết bị khỏi phòng học dựa theo mã thiết bị.
    /// </summary>
    /// <param name="deviceId">Mã thiết bị cần gỡ bỏ.</param>
    /// <returns>True nếu xóa thành công; False nếu không tìm thấy thiết bị.</returns>
    public bool RemoveDevice(string deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            return false;
        }

        var device = FindDevice(deviceId);
        if (device != null)
        {
            // Quan hệ kết tập: Chỉ xóa liên kết trong danh sách của phòng, không làm ảnh hưởng đối tượng gốc
            return _devices.Remove(device);
        }

        return false;
    }

    /// <summary>
    /// Tìm kiếm thiết bị trong phòng học theo mã thiết bị.
    /// </summary>
    /// <param name="deviceId">Mã thiết bị cần tìm.</param>
    /// <returns>Đối tượng Device nếu tìm thấy; ngược lại trả về null.</returns>
    public Device? FindDevice(string deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            return null;
        }

        return _devices.FirstOrDefault(d => string.Equals(d.Id, deviceId, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tính tổng chi phí bảo trì dự kiến trong một năm cho tất cả thiết bị trong phòng học.
    /// </summary>
    /// <returns>Tổng chi phí bảo trì dự kiến (VNĐ).</returns>
    public decimal CalculateAnnualMaintenanceCost()
    {
        // Quy tắc 3: Áp dụng tính ĐA HÌNH (Polymorphism) thuần túy
        // Mỗi đối tượng thiết bị tự tính toán dựa trên cài đặt override của chính nó.
        // Tuyệt đối không kiểm tra kiểu cụ thể bằng if/switch/is/GetType.
        return _devices.Sum(d => d.CalculateAnnualMaintenanceCost());
    }

    /// <summary>
    /// Lấy danh sách các thiết bị đang cần bảo dưỡng theo quy định của hệ thống.
    /// </summary>
    /// <returns>Danh sách các thiết bị đang UnderMaintenance hoặc có thời gian sử dụng trên 5 năm.</returns>
    public List<Device> GetDevicesRequiringMaintenance()
    {
        // Quy tắc 4: Lọc thiết bị đang ở trạng thái UnderMaintenance hoặc sử dụng trên 5 năm (> 5 năm)
        return _devices
            .Where(d => d.Status == DeviceStatus.UnderMaintenance || d.UsageYears > 5)
            .ToList();
    }

    /// <summary>
    /// Trả về chuỗi đại diện mô tả phòng học và số lượng thiết bị.
    /// </summary>
    public override string ToString()
    {
        return $"Phòng học: {RoomName} [Mã: {RoomId}] | Sức chứa: {Capacity} chỗ | Số lượng thiết bị: {_devices.Count} | Tổng chi phí bảo trì/năm: {CalculateAnnualMaintenanceCost():N0} đ";
    }
}
