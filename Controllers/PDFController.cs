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
    public async Task<IActionResult> ProcessPDFs(ProcessPDFDTO request)
    {
        if (request.file == null || request.file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }
        // Check the file signature
        using var reader = new BinaryReader(request.file.OpenReadStream());
        var signatureBytes = reader.ReadBytes(4);
        var isZip = signatureBytes[0] == 0x50 && signatureBytes[1] == 0x4B &&
                    (signatureBytes[2] == 0x03 || signatureBytes[2] == 0x05 || signatureBytes[2] == 0x07) &&
                    (signatureBytes[3] == 0x04 || signatureBytes[3] == 0x06 || signatureBytes[3] == 0x08);

        if (!isZip)
            return BadRequest("Uploaded file is not a valid ZIP archive.");

        try
        {
            var result = PDFService.ProcessPDF(request.file, request.Keywords);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest("Invalid keywords format: " + ex.Message);
        }


    }
}
