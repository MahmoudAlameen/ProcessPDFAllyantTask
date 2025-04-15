using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using System.Text.Json;
using ProcessPDFAllyantTask.DTOs;
using System.Text;
using ProcessPDFAllyantTask.Contracts;

namespace ProcessPDFAllyantTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PDFController : ControllerBase
{
    IPDFService PDFService;

    public PDFController(IPDFService pDFService)
    {
        PDFService = pDFService;
    }

    [HttpPost("process-pdf")]
    public async Task<IActionResult> ProcessPDFs([FromForm] IFormFile file, [FromForm] string keywords)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        KeywordRequest? keywordRequest;
        try
        {
            keywordRequest = JsonSerializer.Deserialize<KeywordRequest>(keywords, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (keywordRequest == null || keywordRequest.Keywords == null || keywordRequest.Keywords.Count == 0)
                return BadRequest("No keywords provided.");

            
        }
        catch (Exception ex)
        {
            return BadRequest("Invalid keywords format: " + ex.Message);
        }

        var result = PDFService.ProcessPDF(file, keywordRequest.Keywords);

        return Ok(result);
    }
}
