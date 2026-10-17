/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

namespace DeviceManagement;

// Giả định:
// 1. Phí trả muộn tính theo số ngày trễ so với DueDate nhân với LateFeePerDay của thiết bị.
// 2. Nếu chưa trả thiết bị thì phí phạt chốt chưa phát sinh (bằng 0).
// 3. Mặc định FromPeriod = 1 và ToPeriod = 1 nếu không truyền vào.

/// <summary>
/// Đại diện cho một thiết bị trong phiếu mượn.
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
    /// Ngày giờ mượn.
    /// </summary>
    public DateTime BorrowedDate { get; }

    /// <summary>
    /// Hạn trả thiết bị.
    /// </summary>
    public DateTime DueDate { get; }

    /// <summary>
    /// Tiết bắt đầu mượn.
    /// </summary>
    public int FromPeriod
    {
        get => _fromPeriod;
        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Tiết bắt đầu phải >= 1.");
            }
            if (_toPeriod >= 1 && value > _toPeriod)
            {
                throw new ArgumentException("Tiết bắt đầu không được lớn hơn tiết kết thúc.", nameof(value));
            }
            _fromPeriod = value;
        }
    }

    /// <summary>
    /// Tiết kết thúc mượn.
    /// </summary>
    public int ToPeriod
    {
        get => _toPeriod;
        set
        {
            if (value < _fromPeriod)
            {
                throw new ArgumentException("Tiết kết thúc phải >= tiết bắt đầu.", nameof(value));
            }
            _toPeriod = value;
        }
    }

    /// <summary>
    /// Ngày thực tế trả thiết bị (null nếu chưa trả).
    /// </summary>
    public DateTime? ReturnedDate { get; private set; }

    /// <summary>
    /// Đã trả hay chưa.
    /// </summary>
    public bool IsReturned => ReturnedDate != null;

    /// <summary>
    /// Khởi tạo mục mượn thiết bị.
    /// </summary>
    public BorrowItem(TeachingDevice device, DateTime borrowedDate, DateTime dueDate, int fromPeriod = 1, int toPeriod = 1)
    {
        // Kiểm tra thiết bị không được null
        if (device == null)
        {
            throw new ArgumentNullException(nameof(device), "Thiết bị không được null.");
        }

        // Hạn trả phải sau hoặc bằng ngày mượn
        if (dueDate < borrowedDate)
        {
            throw new ArgumentException("Hạn trả phải lớn hơn hoặc bằng ngày mượn.", nameof(dueDate));
        }

        // Tiết mượn phải từ tiết 1 trở lên
        if (fromPeriod < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(fromPeriod), "Tiết bắt đầu phải >= 1.");
        }

        // Tiết kết thúc phải >= tiết bắt đầu
        if (toPeriod < fromPeriod)
        {
            throw new ArgumentException("Tiết kết thúc phải >= tiết bắt đầu.", nameof(toPeriod));
        }

        Device = device;
        BorrowedDate = borrowedDate;
        DueDate = dueDate;
        _fromPeriod = fromPeriod;
        _toPeriod = toPeriod;
        ReturnedDate = null;
    }

    /// <summary>
    /// Thực hiện trả thiết bị.
    /// </summary>
    public void Return(DateTime returnedDate)
    {
        // 1. Kiểm tra xem thiết bị này đã trả trước đó chưa
        // Lý do: Tránh việc trả 2 lần làm sai lệch số liệu
        if (IsReturned)
        {
            throw new DeviceAlreadyReturnedException(Device.Id);
        }

        // 2. Kiểm tra ngày trả không được trước ngày mượn
        // Lý do: Không thể trả thiết bị trước khi mượn
        if (returnedDate < BorrowedDate)
        {
            throw new InvalidReturnDateException(Device.Id, BorrowedDate, returnedDate);
        }

        // 3. Sau khi kiểm tra xong xuôi mới gán ngày trả
        // Lý do: Nếu các bước trên bị lỗi ném ngoại lệ thì ReturnedDate vẫn là null, không bị sai
        ReturnedDate = returnedDate;
    }

    /// <summary>
    /// Tính tiền phạt trễ hạn của thiết bị.
    /// </summary>
    public decimal CalculateLateFee(DateTime? asOf = null)
    {
        DateTime? checkDate = ReturnedDate ?? asOf;
        if (!checkDate.HasValue)
        {
            return 0m;
        }

        int soNgayTre = (checkDate.Value.Date - DueDate.Date).Days;
        if (soNgayTre > 0)
        {
            return soNgayTre * Device.LateFeePerDay;
        }

        return 0m;
    }

    /// <summary>
    /// Ghi đè ToString hiển thị thông tin mục mượn.
    /// </summary>
    public override string ToString()
    {
        string trangThaiTra = IsReturned
            ? "Đã trả lúc: " + ReturnedDate!.Value.ToString("dd/MM/yyyy HH:mm")
            : "Chưa hoàn trả";

        decimal phiTre = CalculateLateFee();
        string thongTinPhat = phiTre > 0 ? " | Phạt trễ: " + phiTre.ToString("N0") + " đ" : "";

        return "Thiết bị: " + Device.Name + " (Mã: " + Device.Id + ") | Tiết: " + FromPeriod + "-" + ToPeriod + " | Mượn: " + BorrowedDate.ToString("dd/MM/yyyy") + " | Hạn trả: " + DueDate.ToString("dd/MM/yyyy") + " | " + trangThaiTra + thongTinPhat;
    }
}
