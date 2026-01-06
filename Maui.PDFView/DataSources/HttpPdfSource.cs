using System.Diagnostics;

namespace Maui.PDFView.DataSources;

public class HttpPdfSource : IPdfSource
{
    private readonly string _url;

    public HttpPdfSource(string url)
    {
        _url = url;
    }

    public async Task<string> GetFilePathAsync()
    {
        var tempFile = PdfTempFileHelper.CreateTempPdfFilePath();

        try
        {
            using HttpClient client = new HttpClient(); 
            client.Timeout = TimeSpan.FromSeconds(15);

            // Add a browser-like User-Agent
            client.DefaultRequestHeaders.UserAgent.ParseAdd( "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " + "AppleWebKit/537.36 (KHTML, like Gecko) " + "Chrome/120.0.0.0 Safari/537.36" );

            using HttpResponseMessage response = await client.GetAsync(_url); 
            response.EnsureSuccessStatusCode(); 
            await using FileStream fs = new FileStream(tempFile, FileMode.Create); 
            await response.Content.CopyToAsync(fs);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }

        return tempFile;
    }
}