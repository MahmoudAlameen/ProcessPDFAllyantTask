namespace ProcessPDFAllyantTask.DTOs
{
    public class PDFResult
    {
        public string FileName { get; set; }
        public PDFMetadata Metadata { get; set; }
        public Dictionary<string, int> KeywordCounts { get; set; } = new();
    }
}
