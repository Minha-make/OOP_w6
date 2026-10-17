/****************/
// Mã sinh viên: 202418890
// Họ tên: Nguyễn Minh Hà
/****************/

using System.Text;

namespace DeviceManagement;

internal class Program
{
    private static void Main(string[] args)
    {
        // Thiết lập font UTF-8 để không bị lỗi tiếng Việt khi in ra console
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("=== BÀI TẬP OOP: QUẢN LÝ THIẾT BỊ VÀ HỢP ĐỒNG BẢO TRÌ ===");

        // Gọi chạy phần A: Luồng thành công
        ChayPhanA();

        // Gọi chạy phần B: 7 tình huống phát sinh ngoại lệ
        ChayPhanB();

        Console.WriteLine("\n=== KẾT THÚC CHƯƠNG TRÌNH ===");
    }

    // ==========================================
    // PHẦN A: LUỒNG THÀNH CÔNG
    // ==========================================
    private static void ChayPhanA()
    {
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("PHẦN A: KIỂM THỬ LUỒNG THÀNH CÔNG");
        Console.WriteLine("--------------------------------------------------");

        // 1. Tạo một giảng viên
        Lecturer gv = new Lecturer("GV01", "TS. Nguyễn Minh Triết");
        Console.WriteLine("1. Đã tạo giảng viên: " + gv.FullName + " (Mã: " + gv.BorrowerId + ")");

        // 2. Tạo một điều khiển máy chiếu, một điều khiển điều hòa và một micro không dây
        ProjectorRemote dkMayChieu = new ProjectorRemote("PR01", "Điều khiển máy chiếu Panasonic", 2022, 500000m);
        AirConditionerRemote dkDieuHoa = new AirConditionerRemote("AC01", "Điều khiển điều hòa Daikin", 2021, 600000m);
        Microphone mic = new Microphone("MC01", "Microphone không dây Shure", 2023, 1500000m);

        Console.WriteLine("2. Đã tạo 3 thiết bị:");
        Console.WriteLine("   + " + dkMayChieu);
        Console.WriteLine("   + " + dkDieuHoa);
        Console.WriteLine("   + " + mic);

        // 3. Tạo phòng học và thêm thiết bị vào phòng (kết tập)
        ClassRoom phongHoc = new ClassRoom("P301", "Phòng thực hành 301", 50);
        phongHoc.AddDevice(dkMayChieu);
        phongHoc.AddDevice(dkDieuHoa);
        phongHoc.AddDevice(mic);
        Console.WriteLine("3. Tạo phòng học và thêm thiết bị: " + phongHoc.RoomName + " (" + phongHoc.Devices.Count + " thiết bị)");

        // 4. Tạo phiếu mượn cho một phòng dạy
        DateTime ngayMuon = new DateTime(2026, 10, 10, 8, 0, 0);
        BorrowingSlip phieu = new BorrowingSlip("PM01", gv, phongHoc.RoomName, ngayMuon);
        Console.WriteLine("4. Tạo phiếu mượn " + phieu.SlipId + " vào ngày " + ngayMuon.ToString("dd/MM/yyyy"));

        // 5. Thêm điều khiển máy chiếu và micro vào phiếu
        phieu.AddDevice(dkMayChieu, 2); // Mượn 2 ngày -> hạn trả 12/10
        phieu.AddDevice(mic, 1);        // Mượn 1 ngày -> hạn trả 11/10
        Console.WriteLine("5. Đã thêm điều khiển máy chiếu và micro vào phiếu mượn:");
        for (int i = 0; i < phieu.Items.Count; i++)
        {
            Console.WriteLine("   - " + phieu.Items[i].ToString());
        }

        // 6. Trả điều khiển máy chiếu đúng hạn (12/10/2026)
        DateTime ngayTraMayChieu = new DateTime(2026, 10, 12, 15, 0, 0);
        decimal phiTraMayChieu = phieu.ReturnDevice("PR01", ngayTraMayChieu);
        Console.WriteLine("6. Trả điều khiển máy chiếu đúng hạn (12/10/2026):");
        Console.WriteLine("   -> Trạng thái thiết bị PR01 sau khi trả: " + dkMayChieu.Status);
        Console.WriteLine("   -> Phí phạt trễ hạn: " + phiTraMayChieu + " đ");

        // 7. Trả micro muộn 2 ngày (hạn 11/10, trả ngày 13/10)
        DateTime ngayTraMic = new DateTime(2026, 10, 13, 9, 0, 0);
        decimal phiTraMic = phieu.ReturnDevice("MC01", ngayTraMic);
        Console.WriteLine("7. Trả micro muộn 2 ngày (hạn: 11/10, trả: 13/10):");
        Console.WriteLine("   -> Trạng thái thiết bị MC01 sau khi trả: " + mic.Status);
        Console.WriteLine("   -> Phí phạt trễ: " + phiTraMic + " đ");

        // 8. Hiển thị báo cáo thống kê
        Console.WriteLine("8. Báo cáo thống kê sau khi thực hiện:");
        DateTime mocThoiGian = new DateTime(2026, 10, 14);

        var dsTraMuon = phieu.GetOverdueItems(mocThoiGian);
        Console.WriteLine("   * Danh sách thiết bị trả muộn (" + dsTraMuon.Count + " món):");
        foreach (var item in dsTraMuon)
        {
            Console.WriteLine("     + " + item.Device.Name + " - Tiền phạt: " + item.CalculateLateFee() + " đ");
        }

        var dsChuaTra = phieu.GetUnreturnedItems();
        Console.WriteLine("   * Danh sách thiết bị chưa trả: " + (dsChuaTra.Count == 0 ? "Không có" : dsChuaTra.Count + " món"));

        Console.WriteLine("   * Tổng phí trễ của phiếu: " + phieu.CalculateTotalLateFee() + " đ");
        Console.WriteLine("   * Tổng chi phí bảo trì phòng " + phongHoc.RoomName + ": " + phongHoc.CalculateAnnualMaintenanceCost() + " đ");
        Console.WriteLine("   * Số thiết bị cần bảo trì trong phòng: " + phongHoc.GetDevicesRequiringMaintenance().Count);
    }

    // ==========================================
    // PHẦN B: 7 TÌNH HUỐNG NGOẠI LỆ
    // ==========================================
    private static void ChayPhanB()
    {
        Console.WriteLine("\n--------------------------------------------------");
        Console.WriteLine("PHẦN B: KIỂM THỬ 7 TÌNH HUỐNG NGOẠI LỆ NGHIỆP VỤ");
        Console.WriteLine("--------------------------------------------------");

        Lecturer gv = new Lecturer("GV99", "Thầy Hoàng");

        // --- Tình huống 1: Mượn điều khiển điều hòa đang bảo trì ---
        Console.WriteLine("\n--- Tình huống 1: Mượn điều khiển điều hòa đang UnderMaintenance ---");
        AirConditionerRemote acBaoTri = new AirConditionerRemote("AC-BT", "Điều hòa Casper", 2021, 400000m);
        acBaoTri.MarkAsUnderMaintenance(); // Thiết bị đang bảo trì
        BorrowingSlip phieu1 = new BorrowingSlip("PM-EX01", gv, "Phòng 101", DateTime.Now);

        try
        {
            Console.WriteLine("Cố gắng thêm thiết bị đang bảo trì AC-BT vào phiếu...");
            phieu1.AddDevice(acBaoTri, 2);
        }
        catch (DeviceUnavailableException ex)
        {
            Console.WriteLine(">> Bắt được lỗi: " + ex.GetType().Name);
            Console.WriteLine(">> Thông báo: " + ex.Message);
            Console.WriteLine(">> Thuộc tính: DeviceId = " + ex.DeviceId + ", CurrentStatus = " + ex.CurrentStatus);
        }
        finally
        {
            Console.WriteLine("[Finally] Kiểm tra tính nhất quán:");
            Console.WriteLine("   - Trạng thái thiết bị AC-BT: " + acBaoTri.Status);
            Console.WriteLine("   - Số thiết bị trong phiếu 1: " + phieu1.Items.Count);
        }

        // --- Tình huống 2: Trợ giảng mượn thiết bị thứ 3 ---
        Console.WriteLine("\n--- Tình huống 2: Trợ giảng mượn thiết bị thứ ba ---");
        TeachingAssistant ta = new TeachingAssistant("TA01", "Trợ giảng Nam");
        BorrowingSlip phieuTA = new BorrowingSlip("PM-TA", ta, "Lab Máy tính", DateTime.Now);
        Microphone mic1 = new Microphone("M1", "Mic 1", 2023, 500000m);
        Microphone mic2 = new Microphone("M2", "Mic 2", 2023, 500000m);
        Microphone mic3 = new Microphone("M3", "Mic 3", 2023, 500000m);

        phieuTA.AddDevice(mic1, 1);
        phieuTA.AddDevice(mic2, 1);
        Console.WriteLine("Trợ giảng đã mượn 2 thiết bị (đạt hạn mức 2 thiết bị).");

        try
        {
            Console.WriteLine("Thử mượn thêm thiết bị thứ 3 (M3)...");
            phieuTA.AddDevice(mic3, 1);
        }
        catch (BorrowLimitExceededException ex)
        {
            Console.WriteLine(">> Bắt được lỗi: " + ex.GetType().Name);
            Console.WriteLine(">> Thông báo: " + ex.Message);
            Console.WriteLine(">> Thuộc tính: BorrowerId = " + ex.BorrowerId + ", MaximumDevices = " + ex.MaximumDevices);
        }
        finally
        {
            Console.WriteLine("[Finally] Kiểm tra tính nhất quán:");
            Console.WriteLine("   - Trạng thái thiết bị M3: " + mic3.Status);
            Console.WriteLine("   - Số thiết bị trong phiếu TA: " + phieuTA.Items.Count);
        }

        // --- Tình huống 3: Thêm cùng một micro hai lần ---
        Console.WriteLine("\n--- Tình huống 3: Thêm cùng một micro hai lần vào phiếu ---");
        Microphone micTrung = new Microphone("MC-TRUNG", "Mic Shure", 2024, 1000000m);
        BorrowingSlip phieu3 = new BorrowingSlip("PM-EX03", gv, "Phòng 201", DateTime.Now);
        phieu3.AddDevice(micTrung, 1);
        Console.WriteLine("Đã thêm mic MC-TRUNG lần 1 vào phiếu.");

        try
        {
            Console.WriteLine("Thử thêm lại chính mic MC-TRUNG lần 2...");
            phieu3.AddDevice(micTrung, 1);
        }
        catch (DuplicateDeviceException ex)
        {
            Console.WriteLine(">> Bắt được lỗi: " + ex.GetType().Name);
            Console.WriteLine(">> Thông báo: " + ex.Message);
            Console.WriteLine(">> Thuộc tính: DeviceId = " + ex.DeviceId);
        }
        finally
        {
            Console.WriteLine("[Finally] Kiểm tra tính nhất quán:");
            Console.WriteLine("   - Trạng thái thiết bị MC-TRUNG: " + micTrung.Status);
            Console.WriteLine("   - Số thiết bị trong phiếu 3: " + phieu3.Items.Count);
        }

        // --- Tình huống 4: Mượn micro trong 3 ngày ---
        Console.WriteLine("\n--- Tình huống 4: Mượn micro trong 3 ngày (quá giới hạn 1 ngày) ---");
        Microphone micQuaNgay = new Microphone("MC-NGAY", "Mic Sony", 2024, 1200000m);
        BorrowingSlip phieu4 = new BorrowingSlip("PM-EX04", gv, "Phòng 202", DateTime.Now);

        try
        {
            Console.WriteLine("Thử mượn mic trong 3 ngày (giới hạn của mic là " + micQuaNgay.MaxBorrowDays + " ngày)...");
            phieu4.AddDevice(micQuaNgay, 3);
        }
        catch (InvalidBorrowPeriodException ex)
        {
            Console.WriteLine(">> Bắt được lỗi: " + ex.GetType().Name);
            Console.WriteLine(">> Thông báo: " + ex.Message);
            Console.WriteLine(">> Thuộc tính: DeviceId = " + ex.DeviceId + ", RequestedDays = " + ex.RequestedDays + ", MaximumDays = " + ex.MaximumDays);
        }
        finally
        {
            Console.WriteLine("[Finally] Kiểm tra tính nhất quán:");
            Console.WriteLine("   - Trạng thái thiết bị MC-NGAY: " + micQuaNgay.Status);
            Console.WriteLine("   - Số thiết bị trong phiếu 4: " + phieu4.Items.Count);
        }

        // --- Tình huống 5: Trả điều khiển máy chiếu không thuộc phiếu ---
        Console.WriteLine("\n--- Tình huống 5: Trả điều khiển máy chiếu không thuộc phiếu ---");
        BorrowingSlip phieu5 = new BorrowingSlip("PM-EX05", gv, "Phòng 203", DateTime.Now);
        ProjectorRemote prThiet = new ProjectorRemote("PR-THIET", "Điều khiển máy chiếu", 2022, 500000m);
        phieu5.AddDevice(prThiet, 2);

        try
        {
            Console.WriteLine("Thử trả thiết bị không có trong phiếu: PR-AO-999...");
            phieu5.ReturnDevice("PR-AO-999", DateTime.Now);
        }
        catch (DeviceNotInSlipException ex)
        {
            Console.WriteLine(">> Bắt được lỗi: " + ex.GetType().Name);
            Console.WriteLine(">> Thông báo: " + ex.Message);
            Console.WriteLine(">> Thuộc tính: DeviceId = " + ex.DeviceId);
        }
        finally
        {
            Console.WriteLine("[Finally] Kiểm tra tính nhất quán:");
            Console.WriteLine("   - Trạng thái thiết bị PR-THIET: " + prThiet.Status);
            Console.WriteLine("   - Số thiết bị trong phiếu 5: " + phieu5.Items.Count);
        }

        // --- Tình huống 6: Trả cùng một thiết bị hai lần ---
        Console.WriteLine("\n--- Tình huống 6: Trả cùng một thiết bị hai lần ---");
        BorrowingSlip phieu6 = new BorrowingSlip("PM-EX06", gv, "Phòng 204", DateTime.Now);
        ProjectorRemote prTraHaiLan = new ProjectorRemote("PR-2LAN", "Điều khiển Sony", 2023, 500000m);
        phieu6.AddDevice(prTraHaiLan, 2);
        // Trả lần 1
        phieu6.ReturnDevice("PR-2LAN", DateTime.Now);
        Console.WriteLine("Đã trả thiết bị PR-2LAN lần 1 thành công.");

        try
        {
            Console.WriteLine("Cố tình trả lại thiết bị PR-2LAN lần thứ 2...");
            phieu6.ReturnDevice("PR-2LAN", DateTime.Now);
        }
        catch (DeviceAlreadyReturnedException ex)
        {
            Console.WriteLine(">> Bắt được lỗi: " + ex.GetType().Name);
            Console.WriteLine(">> Thông báo: " + ex.Message);
            Console.WriteLine(">> Thuộc tính: DeviceId = " + ex.DeviceId);
        }
        finally
        {
            Console.WriteLine("[Finally] Kiểm tra tính nhất quán:");
            Console.WriteLine("   - Trạng thái thiết bị PR-2LAN: " + prTraHaiLan.Status);
            Console.WriteLine("   - Số thiết bị trong phiếu 6: " + phieu6.Items.Count);
        }

        // --- Tình huống 7: Trả thiết bị có ngày trả trước ngày mượn ---
        Console.WriteLine("\n--- Tình huống 7: Trả thiết bị với ngày trả trước ngày mượn ---");
        DateTime ngayMuonPhieu7 = new DateTime(2026, 10, 15, 8, 0, 0);
        DateTime ngayTraLoi = new DateTime(2026, 10, 14, 8, 0, 0); // Trước 1 ngày

        BorrowingSlip phieu7 = new BorrowingSlip("PM-EX07", gv, "Phòng 205", ngayMuonPhieu7);
        AirConditionerRemote acLoiNgay = new AirConditionerRemote("AC-NGAYLOI", "Điều khiển Funiki", 2022, 450000m);
        phieu7.AddDevice(acLoiNgay, 2);
        Console.WriteLine("Đã mượn thiết bị vào ngày: " + ngayMuonPhieu7.ToString("dd/MM/yyyy"));

        try
        {
            Console.WriteLine("Thử trả thiết bị với ngày trả là: " + ngayTraLoi.ToString("dd/MM/yyyy") + "...");
            phieu7.ReturnDevice("AC-NGAYLOI", ngayTraLoi);
        }
        catch (InvalidReturnDateException ex)
        {
            Console.WriteLine(">> Bắt được lỗi: " + ex.GetType().Name);
            Console.WriteLine(">> Thông báo: " + ex.Message);
            Console.WriteLine(">> Thuộc tính: DeviceId = " + ex.DeviceId + ", BorrowedDate = " + ex.BorrowedDate.ToString("dd/MM/yyyy") + ", ReturnedDate = " + ex.ReturnedDate.ToString("dd/MM/yyyy"));
        }
        finally
        {
            Console.WriteLine("[Finally] Kiểm tra tính nhất quán:");
            Console.WriteLine("   - Trạng thái thiết bị AC-NGAYLOI: " + acLoiNgay.Status);
            Console.WriteLine("   - Số thiết bị trong phiếu 7: " + phieu7.Items.Count);
        }
    }
}
