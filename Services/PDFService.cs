using ProcessPDFAllyantTask.Contracts;
using ProcessPDFAllyantTask.DTOs;
using System.IO.Compression;
using System.Text;
using UglyToad.PdfPig;

namespace ProcessPDFAllyantTask.Services
{
    public class PDFService : IPDFService
    {

        public ApiResponse ProcessPDF(IFormFile file, List<string> keywords)
        {
            var responseModel = new ApiResponse();
            foreach (var key in keywords)
            {
                responseModel.TotalKeywordCounts[key] = 0;
            }

            using var zipStream = file.OpenReadStream();
            using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);

            foreach (var pdfFile in archive.Entries)
            {
                if (!pdfFile.FullName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                    continue;

                using var entryStream = pdfFile.Open();
                using var pdfDocument = PdfDocument.Open(entryStream);

                // Retrieve metadata
                var metadata = new PDFMetadata
                {
                    Title = pdfDocument.Information.Title ?? pdfFile.Name,
                    Author = pdfDocument.Information.Author ?? "Unknown",
                    PageCount = pdfDocument.NumberOfPages
                };

                // Extract all text from the PDF.
                var pdfText = new StringBuilder();
                foreach (var page in pdfDocument.GetPages())
                {
                    pdfText.AppendLine(page.Text);
                }

                var pdfKeywordCounts = new Dictionary<string, int>();
                foreach (var key in keywords)
                {
                    int count = CountKeywordOccurences(pdfText.ToString(), key);
                    pdfKeywordCounts[key] = count;

                    responseModel.TotalKeywordCounts[key] += count;
                }

                var result = new PDFResult
                {
                    FileName = pdfFile.Name,
                    Metadata = metadata,
                    KeywordCounts = pdfKeywordCounts
                };

                responseModel.Files.Add(result);
            }
            return responseModel;
        }
        private int CountKeywordOccurences(string text, string keyword)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrEmpty(keyword))
                return 0;

            int count = 0;
            int index = 0;
            while ((index = text.IndexOf(keyword, index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                count++;
                index += keyword.Length;
            }
            return count;
        }
    }
}
