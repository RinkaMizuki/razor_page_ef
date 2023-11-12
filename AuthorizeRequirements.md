@Đây là cách xác thực quyền bằng các tự định nghĩa các required cho User
    - B1 : Cần phải triển khai một class Implement một Interface IAuthorizationRequirement
    - B2 : Sau khi có class thì sẽ định nghĩa các Property để Required

    **Lưu ý : Các Require khi được thêm vào hệ thống thì chưa có dịch vụ (Service) nào trong
hệ thông đảm nghiệm việc check cả nên ta cần phải định nghĩa ra Service đó Implement Interface IAuthorizationHandler và Inject vào hệ thống
- Tham số context khi Implement IAuthorizationHandler tạo ra chứa các thông tin cơ bản về User đang đăng nhập, object Resource, ...
Requirements là các require đang chờ được xử lí sau khi thêm vào Requiments Options
PendingRequirements là các require chưa được đánh dấu là đã thành công (succeeded)