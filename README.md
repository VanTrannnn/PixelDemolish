**Prototype for game Pixel Demolish**
## 1. **Hướng dẫn mở Project và chạy**
- Clone project or download zip.
- Khởi động Unity Hub, chọn `Add project from disk` và trỏ đến thư mục chứa project (**version 2022.3.62f1**).
- Mở Scene đầu tiên và nhấn **Play**.

## 2. **Kiến trúc Code (Core Systems)**

- Entity: Mỗi Object pixel trong game sẽ được gắn và quản lý bởi script này, nó chịu trách nhiệm quản lý tổng thể các thành phần con và khởi tạo ban đầu.
- Cube: Mỗi đối tượng Entity được cấu thành từ nhiều Cube, tương ứng với mỗi pixel. Script này quản lý logic của từng pixel riêng lẻ như va chạm, màu sắc và thông báo trạng thái bị phá hủy về progress level.
- ColorCube: Giúp thay đổi màu sắc của Cube mà không phải tạo ra 1 Material mới trong folder.
- LevelProgressManager: Quét toàn bộ scene để đếm các vật thể có tag "Cube" để quản lý thanh tiến trình và check complete level khi đã destroy đủ số Cube.
- XPManager: Quản lý điểm kinh nghiệm từ các pixel rơi xuống đáy. Khi đủ XP, game sẽ tạm dừn để người chơi Upgrade.
- UpgradeManager: Quản lý type Upgrade, lưu trữ các stats như Rotation speed, Scale (Size của vũ khí), Damage. Khi chọn các nâng cấp thì các stats sẽ được update cho các vũ khí có trên scene. Ngoài ra, còn quản lý Upgrade New Tower (Saw), dùng để đặt 1 vũ khí trên 1 ô Obstacle khả dụng.
- UpgradeUIHandle: Hiển thị UI Upgrade, Animation.

## 3. **Hướng dẫn sử dụng Level Editor Tool (kèm screenshot nếu có)**

Phần này em chưa làm kịp (Chỉ mới viết level config đơn giản và level manager nhưng chưa gọi ra để sử dụng cho mỗi level).

## 4. **Những điều em sẽ cải thiện nếu có thêm thời gian**

- Hệ thống level editor tool.
- Tool image to pixel (đưa image pixel vào -> tự đổi thành object với mỗi pixel là 1 cube và lưu vào prefab).
- Thêm các button Back, Pause.
- Thêm Scene Menu.
- Thêm các Sound, vfx.
- Lưu lại tiến trình level.
- Quản lý các Cube bằng object pooling.
- Tạo thêm nhiều Object pixel cho đa dạng.
- Responsive cho nhiều tỉ lệ màn hình.







