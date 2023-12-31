using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CS51_ASP.NET_Razor_EF_1
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc"), StringLength(255,MinimumLength = 3, ErrorMessage = "Tiêu đề phải có độ dài trong khoảng {2} đến {1} kí tự")]
        [Column(TypeName = "nvarchar")]
        public string Title { get; set; }
        [Column(TypeName = "ntext")]
        public string Content { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }
    }
}