/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định thiết kế:
// 1. Phí trả muộn (CalculateLateFee) tính theo số ngày trễ lịch (Date):
//    - Nếu đã trả, căn cứ theo ReturnedDate so với DueDate.
//    - Nếu chưa trả, căn cứ theo ngày hiện tại (DateTime.Now) so với DueDate.
// 2. Mặc định FromPeriod = 1 và ToPeriod = 1 nếu không truyền tiết cụ thể khi khởi tạo.
// 3. Tiết học FromPeriod và ToPeriod phải từ tiết 1 trở lên và ToPeriod >= FromPeriod.

/// <summary>
/// Đại diện cho một mục thiết bị được mượn trong phiếu mượn (BorrowingSlip).
/// </summary>
public class BorrowItem
{
    private int _fromPeriod;
    private int _toPeriod;

    /// <summary>
    /// Thiết bị giảng dạy được mượn.
    /// </summary>
    public TeachingDevice Device { get; }

    /// <summary>
    /// Thời điểm bắt đầu mượn thiết bị.
    /// </summary>
    public DateTime BorrowedDate { get; }

    /// <summary>
    /// Hạn chót phải hoàn trả thiết bị.
    /// </summary>
    public DateTime DueDate { get; }

    /// <summary>
    /// Tiết học bắt đầu mượn (tối thiểu từ tiết 1).
    /// </summary>
    public int FromPeriod
    {
        get => _fromPeriod;
        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Tiết bắt đầu mượn (FromPeriod) phải lớn hơn hoặc bằng 1.");
            }

            if (_toPeriod >= 1 && value > _toPeriod)
            {
                throw new ArgumentException("Tiết bắt đầu (FromPeriod) không được lớn hơn tiết kết thúc (ToPeriod).", nameof(value));
            }

            _fromPeriod = value;
        }
    }

    /// <summary>
    /// Tiết học kết thúc mượn (phải lớn hơn hoặc bằng FromPeriod).
    /// </summary>
    public int ToPeriod
    {
        get => _toPeriod;
        set
        {
            if (value < _fromPeriod)
            {
                throw new ArgumentException("Tiết kết thúc (ToPeriod) phải lớn hơn hoặc bằng tiết bắt đầu (FromPeriod).", nameof(value));
            }

            _toPeriod = value;
        }
    }

    /// <summary>
    /// Ngày giờ thực tế hoàn trả thiết bị (ban đầu là null khi chưa trả).
    /// </summary>
    public DateTime? ReturnedDate { get; private set; }

    /// <summary>
    /// Cho biết thiết bị này đã được hoàn trả hay chưa (thuộc tính tính toán từ ReturnedDate).
    /// </summary>
    public bool IsReturned => ReturnedDate.HasValue;

    /// <summary>
    /// Khởi tạo một mục mượn thiết bị kèm kiểm tra chặt chẽ các điều kiện nghiệp vụ.
    /// </summary>
    /// <param name="device">Thiết bị giảng dạy cần mượn (không null).</param>
    /// <param name="borrowedDate">Ngày mượn thiết bị.</param>
    /// <param name="dueDate">Hạn trả thiết bị (phải &gt;= BorrowedDate).</param>
    /// <param name="fromPeriod">Tiết bắt đầu mượn (&gt;= 1, mặc định: 1).</param>
    /// <param name="toPeriod">Tiết kết thúc mượn (&gt;= fromPeriod, mặc định: 1).</param>
    /// <exception cref="ArgumentNullException">Ném ra khi device là null.</exception>
    /// <exception cref="ArgumentException">Ném ra khi DueDate trước BorrowedDate hoặc tiết học không hợp lệ.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Ném ra khi FromPeriod &lt; 1.</exception>
    public BorrowItem(TeachingDevice device, DateTime borrowedDate, DateTime dueDate, int fromPeriod = 1, int toPeriod = 1)
    {
        // 1. Kiểm tra đối tượng thiết bị không được null
        Device = device ?? throw new ArgumentNullException(nameof(device), "Thiết bị mượn không được phép là null.");

        // 2. Kiểm tra DueDate phải sau hoặc bằng BorrowedDate
        if (dueDate < borrowedDate)
        {
            throw new ArgumentException($"Hạn trả ({dueDate:dd/MM/yyyy HH:mm}) phải lớn hơn hoặc bằng ngày mượn ({borrowedDate:dd/MM/yyyy HH:mm}).", nameof(dueDate));
        }

        // 3. Kiểm tra tiết học bắt đầu phải từ tiết 1 trở lên
        if (fromPeriod < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(fromPeriod), "Tiết bắt đầu mượn (FromPeriod) phải lớn hơn hoặc bằng 1.");
        }

        // 4. Kiểm tra tiết kết thúc phải >= tiết bắt đầu
        if (toPeriod < fromPeriod)
        {
            throw new ArgumentException($"Tiết kết thúc ({toPeriod}) phải lớn hơn hoặc bằng tiết bắt đầu ({fromPeriod}).", nameof(toPeriod));
        }

        BorrowedDate = borrowedDate;
        DueDate = dueDate;
        _fromPeriod = fromPeriod;
        _toPeriod = toPeriod;
        ReturnedDate = null; // Trạng thái ban đầu chưa trả
    }

    /// <summary>
    /// Thực hiện ghi nhận trả thiết bị cho mục mượn này.
    /// </summary>
    /// <param name="returnedDate">Thời điểm hoàn trả thiết bị thực tế.</param>
    /// <exception cref="DeviceAlreadyReturnedException">Ném ra khi thiết bị đã được trả trước đó.</exception>
    /// <exception cref="InvalidReturnDateException">Ném ra khi ngày trả trước ngày mượn.</exception>
    public void Return(DateTime returnedDate)
    {
        // Bước 1: Kiểm tra xem thiết bị đã được trả trước đó chưa
        // Lý do: Ngăn chặn thao tác trả trùng lặp làm sai lệch số liệu phạt và lịch sử
        if (IsReturned)
        {
            throw new DeviceAlreadyReturnedException(Device.Id);
        }

        // Bước 2: Kiểm tra logic thời gian mượn - trả
        // Lý do: Ngày trả thực tế không thể diễn ra trước thời điểm bắt đầu mượn
        if (returnedDate < BorrowedDate)
        {
            throw new InvalidReturnDateException(Device.Id, BorrowedDate, returnedDate);
        }

        // Bước 3: Chỉ ghi nhận ngày trả sau khi toàn bộ điều kiện trên đã thỏa mãn
        // Lý do: Bảo đảm an toàn tính nhất quán (nếu ném ngoại lệ ở các bước trên, ReturnedDate vẫn giữ nguyên giá trị null)
        ReturnedDate = returnedDate;
    }

    /// <summary>
    /// Tính phí phạt trả muộn cho thiết bị này nếu có.
    /// </summary>
    /// <returns>Số tiền phạt trả muộn (VNĐ). Trả về 0 nếu chưa trễ hạn hoặc trả đúng/trước hạn.</returns>
    public decimal CalculateLateFee()
    {
        // Xác định mốc thời điểm đánh giá: nếu đã trả thì theo ngày trả thực tế; nếu chưa trả thì theo ngày hiện tại
        DateTime actualCheckDate = ReturnedDate ?? DateTime.Now;

        // Tính khoảng cách ngày trễ hạn theo ngày lịch (Date)
        int lateDays = (actualCheckDate.Date - DueDate.Date).Days;

        if (lateDays > 0)
        {
            return lateDays * Device.LateFeePerDay;
        }

        return 0m;
    }

    /// <summary>
    /// Trả về chuỗi đại diện chi tiết của mục mượn thiết bị.
    /// </summary>
    public override string ToString()
    {
        string statusInfo = IsReturned
            ? $"Đã trả lúc: {ReturnedDate:dd/MM/yyyy HH:mm}"
            : "Chưa hoàn trả";

        decimal lateFee = CalculateLateFee();
        string feeInfo = lateFee > 0 ? $" | Phạt trễ: {lateFee:N0} đ" : "";

        return $"Thiết bị: {Device.Name} (Mã: {Device.Id}) | Tiết: {FromPeriod}-{ToPeriod} | Mượn: {BorrowedDate:dd/MM/yyyy} | Hạn trả: {DueDate:dd/MM/yyyy} | {statusInfo}{feeInfo}";
    }
}
