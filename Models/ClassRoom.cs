/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định:
// 1. Quan hệ kết tập: Xóa phòng học thì không xóa các đối tượng thiết bị.
// 2. Thiết bị cần bảo trì là thiết bị có trạng thái UnderMaintenance hoặc đã dùng trên 5 năm.

/// <summary>
/// Lớp phòng học chứa danh sách thiết bị (kết tập).
/// </summary>
public class ClassRoom
{
    private List<Device> _devices;

    /// <summary>
    /// Mã phòng học.
    /// </summary>
    public string RoomId { get; }

    /// <summary>
    /// Tên phòng học.
    /// </summary>
    public string RoomName { get; set; }

    /// <summary>
    /// Sức chứa phòng học (phải lớn hơn 0).
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Danh sách thiết bị trong phòng.
    /// </summary>
    public IReadOnlyList<Device> Devices => _devices;

    /// <summary>
    /// Khởi tạo phòng học.
    /// </summary>
    public ClassRoom(string roomId, string roomName, int capacity)
    {
        // Kiểm tra mã phòng không được rỗng
        if (string.IsNullOrWhiteSpace(roomId))
        {
            throw new ArgumentException("Mã phòng không được để trống.", nameof(roomId));
        }

        // Kiểm tra tên phòng không được rỗng
        if (string.IsNullOrWhiteSpace(roomName))
        {
            throw new ArgumentException("Tên phòng không được để trống.", nameof(roomName));
        }

        // Sức chứa phải lớn hơn 0
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Sức chứa phải lớn hơn 0.");
        }

        RoomId = roomId;
        RoomName = roomName;
        Capacity = capacity;
        _devices = new List<Device>();
    }

    /// <summary>
    /// Thêm thiết bị vào phòng.
    /// </summary>
    public void AddDevice(Device device)
    {
        // Không nhận thiết bị null
        if (device == null)
        {
            throw new ArgumentNullException(nameof(device), "Thiết bị không được null.");
        }

        // Không thêm hai thiết bị cùng mã vào một phòng
        foreach (var d in _devices)
        {
            if (d.Id.ToLower() == device.Id.ToLower())
            {
                throw new InvalidOperationException("Thiết bị mã " + device.Id + " đã tồn tại trong phòng " + RoomId);
            }
        }

        _devices.Add(device);
    }

    /// <summary>
    /// Xóa thiết bị khỏi phòng theo mã.
    /// </summary>
    public bool RemoveDevice(string deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            return false;
        }

        Device? d = FindDevice(deviceId);
        if (d != null)
        {
            // Kết tập: chỉ xóa tham chiếu khỏi phòng, thiết bị vẫn còn tồn tại
            return _devices.Remove(d);
        }

        return false;
    }

    /// <summary>
    /// Tìm thiết bị theo mã.
    /// </summary>
    public Device? FindDevice(string deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            return null;
        }

        foreach (var d in _devices)
        {
            if (d.Id.ToLower() == deviceId.ToLower())
            {
                return d;
            }
        }

        return null;
    }

    /// <summary>
    /// Tính tổng chi phí bảo trì dự kiến trong 1 năm bằng tính đa hình.
    /// </summary>
    public decimal CalculateAnnualMaintenanceCost()
    {
        // Sử dụng tính đa hình: mỗi đối tượng thiết bị tự gọi hàm tính của chính nó
        // Không dùng if/switch kiểm tra kiểu
        decimal tongChiPhi = 0;
        foreach (var d in _devices)
        {
            tongChiPhi += d.CalculateAnnualMaintenanceCost();
        }
        return tongChiPhi;
    }

    /// <summary>
    /// Lấy danh sách thiết bị cần bảo trì (UnderMaintenance hoặc dùng trên 5 năm).
    /// </summary>
    public List<Device> GetDevicesRequiringMaintenance()
    {
        List<Device> danhSach = new List<Device>();
        foreach (var d in _devices)
        {
            if (d.Status == DeviceStatus.UnderMaintenance || d.UsageYears > 5)
            {
                danhSach.Add(d);
            }
        }
        return danhSach;
    }

    /// <summary>
    /// Ghi đè ToString hiển thị thông tin phòng.
    /// </summary>
    public override string ToString()
    {
        return $"Phòng học: {RoomName} [Mã: {RoomId}] | Sức chứa: {Capacity} chỗ | Số lượng thiết bị: {_devices.Count} | Tổng chi phí bảo trì/năm: {CalculateAnnualMaintenanceCost():N0} đ";
    }
}
