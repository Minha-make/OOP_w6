/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định:
// 1. SlipId, Borrower và TeachingRoom là bắt buộc khi tạo phiếu.
// 2. Xóa phiếu thì các mục mượn mất đi nhưng người mượn và thiết bị vẫn tồn tại (kết tập/hợp thành).
// 3. Toàn bộ validation được thực hiện trước khi đổi trạng thái để đảm bảo dữ liệu nhất quán.

/// <summary>
/// Đại diện cho phiếu mượn thiết bị của giảng viên/trợ giảng.
/// </summary>
public class BorrowingSlip
{
    private List<BorrowItem> _items = new List<BorrowItem>();

    /// <summary>
    /// Mã phiếu mượn.
    /// </summary>
    public string SlipId { get; }

    /// <summary>
    /// Người mượn thiết bị.
    /// </summary>
    public IBorrower Borrower { get; }

    /// <summary>
    /// Phòng dạy sử dụng thiết bị.
    /// </summary>
    public string TeachingRoom { get; }

    /// <summary>
    /// Ngày mượn thiết bị.
    /// </summary>
    public DateTime BorrowedDate { get; }

    /// <summary>
    /// Danh sách thiết bị trong phiếu.
    /// </summary>
    public IReadOnlyList<BorrowItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Khởi tạo phiếu mượn.
    /// </summary>
    public BorrowingSlip(string slipId, IBorrower borrower, string teachingRoom, DateTime borrowedDate)
    {
        if (string.IsNullOrWhiteSpace(slipId))
        {
            throw new ArgumentException("Mã phiếu không được để trống.", nameof(slipId));
        }

        if (borrower == null)
        {
            throw new ArgumentNullException(nameof(borrower), "Người mượn không được null.");
        }

        if (string.IsNullOrWhiteSpace(teachingRoom))
        {
            throw new ArgumentException("Phòng dạy không được để trống.", nameof(teachingRoom));
        }

        SlipId = slipId;
        Borrower = borrower;
        TeachingRoom = teachingRoom;
        BorrowedDate = borrowedDate;
    }

    /// <summary>
    /// Khởi tạo phiếu mượn với ngày mượn là hiện tại.
    /// </summary>
    public BorrowingSlip(string slipId, IBorrower borrower, string teachingRoom)
        : this(slipId, borrower, teachingRoom, DateTime.Now)
    {
    }

    /// <summary>
    /// Thêm thiết bị vào phiếu mượn.
    /// </summary>
    public void AddDevice(TeachingDevice device, int borrowDays)
    {
        // =====================================================================
        // GIẢI THÍCH TÍNH NHẤT QUÁN:
        // Ta gọi ValidateBorrowing() trước để kiểm tra toàn bộ 6 điều kiện.
        // Nếu có lỗi (ném ngoại lệ), hàm sẽ dừng ngay lập tức, không chạy xuống
        // các dòng dưới. Nhờ đó thiết bị chưa bị đổi sang Borrowed và danh sách
        // _items cũng không bị thêm dở dang. Dữ liệu được bảo toàn nguyên vẹn.
        // =====================================================================

        ValidateBorrowing(device, borrowDays);

        // Sau khi đã hợp lệ toàn bộ mới thực hiện thay đổi:
        DateTime hanTra = BorrowedDate.AddDays(borrowDays);
        BorrowItem itemMoi = new BorrowItem(device, BorrowedDate, hanTra);

        // Đổi trạng thái thiết bị thành Borrowed
        device.MarkAsBorrowed();

        // Thêm vào danh sách phiếu
        _items.Add(itemMoi);
    }

    /// <summary>
    /// Kiểm tra hợp lệ khi mượn theo đúng thứ tự đề bài yêu cầu.
    /// </summary>
    private void ValidateBorrowing(TeachingDevice device, int borrowDays)
    {
        // Thứ tự (1): Kiểm tra thiết bị không được null
        if (device == null)
        {
            throw new ArgumentNullException(nameof(device), "Thiết bị không được null.");
        }

        // Thứ tự (2): Số ngày mượn phải > 0
        if (borrowDays <= 0)
        {
            throw new InvalidBorrowPeriodException(device.Id, borrowDays, device.MaxBorrowDays);
        }

        // Thứ tự (3): Thiết bị chưa có trong phiếu (không mượn 2 lần)
        foreach (var item in _items)
        {
            if (item.Device.Id.ToLower() == device.Id.ToLower())
            {
                throw new DuplicateDeviceException(device.Id);
            }
        }

        // Thứ tự (4): Người mượn chưa đạt giới hạn
        if (_items.Count >= Borrower.GetMaximumDevices())
        {
            throw new BorrowLimitExceededException(Borrower.BorrowerId, Borrower.GetMaximumDevices());
        }

        // Thứ tự (5): Thiết bị phải đang Available
        if (device.Status != DeviceStatus.Available)
        {
            throw new DeviceUnavailableException(device.Id, device.Status);
        }

        // Thứ tự (6): Số ngày mượn không được quá giới hạn tối đa của thiết bị
        if (borrowDays > device.MaxBorrowDays)
        {
            throw new InvalidBorrowPeriodException(device.Id, borrowDays, device.MaxBorrowDays);
        }
    }

    /// <summary>
    /// Trả thiết bị và tính tiền phạt nếu trễ hạn.
    /// </summary>
    public decimal ReturnDevice(string deviceId, DateTime returnedDate)
    {
        // =====================================================================
        // GIẢI THÍCH TÍNH NHẤT QUÁN:
        // Các bước kiểm tra mã thiết bị, tìm item, kiểm tra đã trả hay chưa,
        // và kiểm tra ngày trả đều nằm ở trên. Nếu vi phạm, ngoại lệ sẽ ném ra
        // TRƯỚC KHI gọi item.Return() và device.MarkAsAvailable().
        // Nhờ vậy nếu có lỗi thì không thiết bị nào bị đổi trạng thái sai.
        // =====================================================================

        // 1. Kiểm tra mã thiết bị
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            throw new ArgumentException("Mã thiết bị không được để trống.", nameof(deviceId));
        }

        // 2. Tìm thiết bị trong phiếu
        BorrowItem? timItem = null;
        foreach (var item in _items)
        {
            if (item.Device.Id.ToLower() == deviceId.ToLower())
            {
                timItem = item;
                break;
            }
        }

        // Nếu không có trong phiếu
        if (timItem == null)
        {
            throw new DeviceNotInSlipException(deviceId);
        }

        // 3. Nếu đã trả rồi thì báo lỗi
        if (timItem.IsReturned)
        {
            throw new DeviceAlreadyReturnedException(deviceId);
        }

        // 4. Kiểm tra ngày trả không được trước ngày mượn
        if (returnedDate < timItem.BorrowedDate)
        {
            throw new InvalidReturnDateException(deviceId, timItem.BorrowedDate, returnedDate);
        }

        // 5. Kiểm tra trạng thái thiết bị trước khi thực hiện trả
        // Thiết bị phải đang Borrowed mới có thể hoàn trả hợp lệ
        if (timItem.Device.Status != DeviceStatus.Borrowed)
        {
            throw new InvalidOperationException("Không thể trả thiết bị " + deviceId + " vì thiết bị không ở trạng thái đang mượn (Hiện tại: " + timItem.Device.Status + ").");
        }

        // 6. Chuyển thiết bị về Available trước
        // Lý do: Nếu bước đổi trạng thái này có lỗi phát sinh thì item vẫn CHƯA bị đánh dấu trả,
        // giúp bảo toàn tính nhất quán tuyệt đối, không bị lỗi dở dang (như AI rà soát đã chỉ ra).
        timItem.Device.MarkAsAvailable();

        // 7. Ghi nhận ngày trả cho item sau khi thiết bị đã chuyển trạng thái thành công
        timItem.Return(returnedDate);

        // 8. Trả về tiền phạt nếu có
        return timItem.CalculateLateFee();
    }

    /// <summary>
    /// Tính tổng tiền phạt trễ của tất cả thiết bị trong phiếu.
    /// </summary>
    public decimal CalculateTotalLateFee(DateTime? asOf = null)
    {
        decimal tongPhat = 0;
        foreach (var item in _items)
        {
            tongPhat += item.CalculateLateFee(asOf);
        }
        return tongPhat;
    }

    /// <summary>
    /// Lấy danh sách các thiết bị trễ hạn tính đến thời điểm asOf.
    /// </summary>
    public List<BorrowItem> GetOverdueItems(DateTime asOf)
    {
        List<BorrowItem> dsTre = new List<BorrowItem>();
        foreach (var item in _items)
        {
            // TH1: Đã trả nhưng ngày trả trễ hơn hạn trả
            if (item.IsReturned && item.ReturnedDate!.Value.Date > item.DueDate.Date)
            {
                dsTre.Add(item);
            }
            // TH2: Chưa trả và thời điểm xem xét đã quá hạn trả
            else if (!item.IsReturned && asOf.Date > item.DueDate.Date)
            {
                dsTre.Add(item);
            }
        }
        return dsTre;
    }

    /// <summary>
    /// Lấy danh sách các thiết bị chưa trả trong phiếu.
    /// </summary>
    public List<BorrowItem> GetUnreturnedItems()
    {
        List<BorrowItem> dsChuaTra = new List<BorrowItem>();
        foreach (var item in _items)
        {
            if (!item.IsReturned)
            {
                dsChuaTra.Add(item);
            }
        }
        return dsChuaTra;
    }

    /// <summary>
    /// Ghi đè ToString hiển thị thông tin phiếu mượn.
    /// </summary>
    public override string ToString()
    {
        return $"Phiếu mượn: {SlipId} | Người mượn: {Borrower.FullName} ({Borrower.BorrowerId}) | Phòng: {TeachingRoom} | Ngày mượn: {BorrowedDate:dd/MM/yyyy} | Số món: {_items.Count} | Phạt trễ: {CalculateTotalLateFee():N0} đ";
    }
}
