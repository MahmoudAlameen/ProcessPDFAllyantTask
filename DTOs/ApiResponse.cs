namespace ProcessPDFAllyantTask.DTOs
{
    public class ApiResponse
    {
        public List<PDFResult> Files { get; set; }
        public Dictionary<string, int> TotalKeywordCounts { get; set; } = new Dictionary<string, int>();
    }
}
