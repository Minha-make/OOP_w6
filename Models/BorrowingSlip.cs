/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định thiết kế:
// 1. SlipId, Borrower và TeachingRoom là bắt buộc và bất biến sau khi khởi tạo phiếu.
// 2. Danh sách thiết bị mượn (_items) được bảo vệ bằng IReadOnlyList để chống sửa đổi trực tiếp từ bên ngoài.
// 3. Quan hệ hợp thành (Composition) giữa BorrowingSlip và BorrowItem: Xóa phiếu thì các mục BorrowItem trong phiếu mất đi,
//    nhưng thiết bị TeachingDevice và người mượn Borrower vẫn tồn tại độc lập (theo đúng quy tắc mục 7 của đề bài).
// 4. Các phương thức lọc GetOverdueItems(asOf) và GetUnreturnedItems() trả về danh sách mới, không làm thay đổi trạng thái phiếu.

/// <summary>
/// Đại diện cho một phiếu mượn thiết bị giảng dạy của giảng viên hoặc trợ giảng cho một phòng học cụ thể.
/// </summary>
public class BorrowingSlip
{
    private readonly List<BorrowItem> _items = new();

    /// <summary>
    /// Mã định danh phiếu mượn.
    /// </summary>
    public string SlipId { get; }

    /// <summary>
    /// Đối tượng người mượn thiết bị (Giảng viên hoặc Trợ giảng).
    /// </summary>
    public IBorrower Borrower { get; }

    /// <summary>
    /// Tên hoặc mã phòng học sử dụng thiết bị.
    /// </summary>
    public string TeachingRoom { get; }

    /// <summary>
    /// Ngày giờ tạo phiếu mượn thiết bị.
    /// </summary>
    public DateTime BorrowedDate { get; }

    /// <summary>
    /// Danh sách các mục thiết bị trong phiếu (chỉ đọc để bảo đảm tính đóng gói).
    /// </summary>
    public IReadOnlyList<BorrowItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Khởi tạo một phiếu mượn thiết bị kèm kiểm tra tính hợp lệ của thông tin ban đầu.
    /// </summary>
    /// <param name="slipId">Mã phiếu mượn (không rỗng).</param>
    /// <param name="borrower">Người mượn (không null).</param>
    /// <param name="teachingRoom">Phòng học sử dụng (không rỗng).</param>
    /// <param name="borrowedDate">Ngày giờ mượn thiết bị.</param>
    /// <exception cref="ArgumentException">Ném ra khi SlipId hoặc TeachingRoom rỗng/chỉ chứa khoảng trắng.</exception>
    /// <exception cref="ArgumentNullException">Ném ra khi Borrower là null.</exception>
    public BorrowingSlip(string slipId, IBorrower borrower, string teachingRoom, DateTime borrowedDate)
    {
        // 1. Kiểm tra mã phiếu mượn
        if (string.IsNullOrWhiteSpace(slipId))
        {
            throw new ArgumentException("Mã phiếu mượn không được để trống hoặc chỉ chứa khoảng trắng.", nameof(slipId));
        }

        // 2. Kiểm tra đối tượng người mượn
        Borrower = borrower ?? throw new ArgumentNullException(nameof(borrower), "Thông tin người mượn không được là null.");

        // 3. Kiểm tra phòng dạy học
        if (string.IsNullOrWhiteSpace(teachingRoom))
        {
            throw new ArgumentException("Phòng dạy học không được để trống hoặc chỉ chứa khoảng trắng.", nameof(teachingRoom));
        }

        SlipId = slipId;
        TeachingRoom = teachingRoom;
        BorrowedDate = borrowedDate;
    }

    /// <summary>
    /// Khởi tạo một phiếu mượn với thời điểm mượn là thời điểm hiện tại.
    /// </summary>
    public BorrowingSlip(string slipId, IBorrower borrower, string teachingRoom)
        : this(slipId, borrower, teachingRoom, DateTime.Now)
    {
    }

    /// <summary>
    /// Thêm một thiết bị vào phiếu mượn với số ngày mượn yêu cầu.
    /// </summary>
    /// <param name="device">Thiết bị giảng dạy cần mượn.</param>
    /// <param name="borrowDays">Số ngày yêu cầu mượn.</param>
    public void AddDevice(TeachingDevice device, int borrowDays)
    {
        // =========================================================================
        // NGUYÊN TẮC BẢO ĐẢM TÍNH NHẤT QUÁN (Consistency & Transactional Integrity):
        // Toàn bộ các điều kiện tiên quyết phải được kiểm tra (Validate) TRƯỚC TIÊN.
        // Nếu có bất kỳ điều kiện nào không thỏa mãn, một ngoại lệ sẽ được ném ra ngay lập tức.
        // Khi đó, luồng thực thi bị ngắt, KHÔNG CÓ bất kỳ sự thay đổi trạng thái nào xảy ra
        // đối với thiết bị (vẫn giữ nguyên trạng thái cũ) hay danh sách phiếu (_items không bị thêm dở dang).
        // =========================================================================

        ValidateBorrowing(device, borrowDays);

        // Chỉ khi TẤT CẢ các điều kiện đã hoàn toàn hợp lệ, ta mới thực hiện các bước thay đổi dữ liệu:
        // 1. Tạo đối tượng BorrowItem với DueDate được tính toán chính xác
        DateTime dueDate = BorrowedDate.AddDays(borrowDays);
        var item = new BorrowItem(device, BorrowedDate, dueDate);

        // 2. Chuyển trạng thái thiết bị sang Borrowed
        device.MarkAsBorrowed();

        // 3. Thêm mục mượn vào danh sách nội bộ của phiếu
        _items.Add(item);
    }

    /// <summary>
    /// Kiểm tra tính hợp lệ của yêu cầu mượn thiết bị theo ĐÚNG thứ tự quy định tại mục 9.1 và yêu cầu hệ thống.
    /// </summary>
    /// <param name="device">Thiết bị cần mượn.</param>
    /// <param name="borrowDays">Số ngày mượn.</param>
    private void ValidateBorrowing(TeachingDevice device, int borrowDays)
    {
        // Thứ tự (1): Kiểm tra thiết bị không được null
        // Lý do: Cần đối tượng hợp lệ để truy xuất các thuộc tính Id, Status, MaxBorrowDays ở các bước sau
        if (device == null)
        {
            throw new ArgumentNullException(nameof(device), "Đối tượng thiết bị không được phép là null.");
        }

        // Thứ tự (2): Kiểm tra số ngày mượn phải lớn hơn 0
        // Lý do: Số ngày mượn không dương (<= 0) là vi phạm quy tắc thời gian mượn cơ bản
        if (borrowDays <= 0)
        {
            throw new InvalidBorrowPeriodException(device.Id, borrowDays, device.MaxBorrowDays);
        }

        // Thứ tự (3): Kiểm tra thiết bị chưa tồn tại trong phiếu
        // Lý do: Không cho phép mượn cùng một thiết bị hai lần trên cùng một phiếu mượn
        if (_items.Any(i => string.Equals(i.Device.Id, device.Id, StringComparison.OrdinalIgnoreCase)))
        {
            throw new DuplicateDeviceException(device.Id);
        }

        // Thứ tự (4): Kiểm tra người mượn chưa đạt giới hạn số lượng thiết bị cho phép
        // Lý do: Giảng viên tối đa 3, trợ giảng tối đa 2 thiết bị/phiếu theo quy định của trường
        if (_items.Count >= Borrower.GetMaximumDevices())
        {
            throw new BorrowLimitExceededException(Borrower.BorrowerId, Borrower.GetMaximumDevices());
        }

        // Thứ tự (5): Kiểm tra thiết bị đang ở trạng thái sẵn sàng (Available)
        // Lý do: Thiết bị đang mượn, đang bảo trì hoặc đã thanh lý thì không thể cấp phát
        if (device.Status != DeviceStatus.Available)
        {
            throw new DeviceUnavailableException(device.Id, device.Status);
        }

        // Thứ tự (6): Kiểm tra thời hạn mượn không vượt quá giới hạn tối đa của riêng thiết bị đó
        // Lý do: Micro tối đa 1 ngày, Máy chiếu 2 ngày, Điều hòa 3 ngày theo chính sách thiết bị
        if (borrowDays > device.MaxBorrowDays)
        {
            throw new InvalidBorrowPeriodException(device.Id, borrowDays, device.MaxBorrowDays);
        }
    }

    /// <summary>
    /// Thực hiện trả thiết bị thuộc phiếu mượn, cập nhật trạng thái thiết bị về Available và tính phí phạt nếu trễ hạn.
    /// </summary>
    /// <param name="deviceId">Mã thiết bị cần trả.</param>
    /// <param name="returnedDate">Ngày giờ trả thiết bị thực tế.</param>
    /// <returns>Số tiền phạt trả muộn của thiết bị này (VNĐ).</returns>
    /// <exception cref="ArgumentException">Ném ra khi mã thiết bị rỗng.</exception>
    /// <exception cref="DeviceNotInSlipException">Ném ra khi thiết bị không thuộc phiếu mượn này.</exception>
    /// <exception cref="DeviceAlreadyReturnedException">Ném ra khi thiết bị đã được trả trước đó.</exception>
    /// <exception cref="InvalidReturnDateException">Ném ra khi ngày trả trước ngày mượn.</exception>
    public decimal ReturnDevice(string deviceId, DateTime returnedDate)
    {
        // =========================================================================
        // THỨ TỰ THAO TÁC ĐẢM BẢO TÍNH NHẤT QUÁN KHI TRẢ THIẾT BỊ:
        // 1. Kiểm tra deviceId hợp lệ.
        // 2. Tìm mục BorrowItem theo mã thiết bị (nếu không thấy -> ném DeviceNotInSlipException).
        // 3. Kiểm tra xem mục này đã trả chưa (nếu rồi -> ném DeviceAlreadyReturnedException).
        // 4. Kiểm tra ngày trả so với ngày mượn (nếu trả trước ngày mượn -> ném InvalidReturnDateException).
        // -> CẢ 4 BƯỚC TRÊN ĐỀU LÀ VALIDATION, CHƯA THAY ĐỔI TRẠNG THÁI NÀO.
        // 5. Ghi nhận ngày trả vào item (item.Return).
        // 6. Chuyển thiết bị về Available (device.MarkAsAvailable).
        // 7. Trả về mức phí phạt nếu có.
        // Nếu xảy ra lỗi ở bất kỳ bước nào từ 1 đến 4, thiết bị và phiếu hoàn toàn không bị đổi trạng thái!
        // =========================================================================

        // Bước 1: Kiểm tra mã thiết bị đầu vào
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            throw new ArgumentException("Mã thiết bị không được để trống hoặc chỉ chứa khoảng trắng.", nameof(deviceId));
        }

        // Bước 2: Tìm kiếm BorrowItem theo mã thiết bị trong phiếu
        var item = _items.FirstOrDefault(i => string.Equals(i.Device.Id, deviceId, StringComparison.OrdinalIgnoreCase));
        if (item == null)
        {
            throw new DeviceNotInSlipException(deviceId);
        }

        // Bước 3: Kiểm tra thiết bị đã hoàn trả hay chưa
        if (item.IsReturned)
        {
            throw new DeviceAlreadyReturnedException(deviceId);
        }

        // Bước 4: Kiểm tra ngày trả không được trước ngày mượn
        if (returnedDate < item.BorrowedDate)
        {
            throw new InvalidReturnDateException(deviceId, item.BorrowedDate, returnedDate);
        }

        // Bước 5: Ghi nhận ngày trả cho BorrowItem (thực thi an toàn vì đã qua bước 3 & 4)
        item.Return(returnedDate);

        // Bước 6: Chuyển trạng thái của thiết bị về Available
        item.Device.MarkAsAvailable();

        // Bước 7: Tính toán và trả về tiền phạt trễ hạn của thiết bị
        return item.CalculateLateFee();
    }

    /// <summary>
    /// Tính tổng tiền phạt trễ hạn của toàn bộ các thiết bị trong phiếu mượn này.
    /// </summary>
    /// <returns>Tổng số tiền phạt trễ hạn (VNĐ).</returns>
    public decimal CalculateTotalLateFee()
    {
        return _items.Sum(item => item.CalculateLateFee());
    }

    /// <summary>
    /// Lọc và trả về danh sách các mục thiết bị bị trễ hạn tính đến thời điểm được chỉ định.
    /// </summary>
    /// <param name="asOf">Thời điểm mốc để đánh giá trễ hạn.</param>
    /// <returns>Danh sách các BorrowItem trễ hạn (bao gồm đã trả trễ hoặc chưa trả mà đã quá hạn).</returns>
    public List<BorrowItem> GetOverdueItems(DateTime asOf)
    {
        return _items.Where(item =>
            // Trường hợp 1: Đã hoàn trả nhưng ngày trả thực tế trễ hơn hạn trả DueDate
            (item.IsReturned && item.ReturnedDate!.Value.Date > item.DueDate.Date) ||
            // Trường hợp 2: Chưa hoàn trả và thời điểm mốc asOf đã vượt quá hạn trả DueDate
            (!item.IsReturned && asOf.Date > item.DueDate.Date)
        ).ToList();
    }

    /// <summary>
    /// Lọc và trả về danh sách các mục thiết bị chưa được hoàn trả trong phiếu mượn.
    /// </summary>
    /// <returns>Danh sách các BorrowItem chưa trả.</returns>
    public List<BorrowItem> GetUnreturnedItems()
    {
        return _items.Where(item => !item.IsReturned).ToList();
    }

    /// <summary>
    /// Trả về chuỗi tổng quan thông tin của phiếu mượn.
    /// </summary>
    public override string ToString()
    {
        return $"Phiếu mượn: {SlipId} | Người mượn: {Borrower.FullName} ({Borrower.BorrowerId}) | Phòng dạy: {TeachingRoom} | Ngày mượn: {BorrowedDate:dd/MM/yyyy HH:mm} | Thiết bị: {_items.Count} món | Tổng phạt trễ: {CalculateTotalLateFee():N0} đ";
    }
}
