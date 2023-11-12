@Với mỗi role thì ta có thể định nghĩa thêm các Claim(Đặc tính, đặc điểm) cho nó 
@Với mỗi user thì ta cũng có thể định nghĩa ra các Claim riêng cho user đó
=> Claim là gì ?
    - Claim nó tương đương như là một đặc tính , đặc điểm của đội tượng đó.
    VD: CCCD (Role) và người để được đi làm thì phải có CCCD thì dựa vào Role đó 
    thì đã có được sự tin cậy nhưng trong CCCD có các trường (Ngày sinh, Nơi sinh,..)
    thì đó là Claim của CCCD đó mà điều kiện để đi làm là phải trên 18 tuổi 
    => Dựa vào Ngày sinh (Claim) để biết được số tuổi có đủ hay không?
    
    - Trong lập trình các Role sẽ có tương ứng cái Claim để có thể linh hoạt hơn trong việc
    Authorize phân quyền cho từng Role và dựa vào cả Claim để xác định có được phép access hay 
    không?