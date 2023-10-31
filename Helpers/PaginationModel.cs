namespace PageitaionModel.Helper
{
    public class PageitaionModel
    {
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public Func<int?, string> urlGenerate { get; set; }
    }
}