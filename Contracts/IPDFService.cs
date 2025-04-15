using ProcessPDFAllyantTask.DTOs;

namespace ProcessPDFAllyantTask.Contracts
{
    public interface IPDFService
    {
        public ApiResponse ProcessPDF(IFormFile file, List<string> keywords);

    }
}
