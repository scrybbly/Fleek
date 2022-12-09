using Microsoft.AspNetCore.Mvc;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Fleek.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public Dictionary<string, string> Get()
        {
            string pdf_path = "D://Code/Fleek/Stock_Lists.pdf";

            //PDFDataToText(pdf_path);

            return PDFDataToText(pdf_path);
        }

        public static Dictionary<string, string> PDFDataToText(string path)
        {
            PdfReader reader = new PdfReader(path);
            //string text = string.Empty;
            List<string>  List_Component = new List<string>();
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                var text = PdfTextExtractor.GetTextFromPage(reader, page);
                List_Component = text.Split("\n").ToList();
            }
            reader.Close();
            return GetPriceList(List_Component);
        }

        public static Dictionary<string, string> GetPriceList(List<string> pdf_data)
        {
            Dictionary<string,string> priceList = new Dictionary<string, string>();
            foreach (string pdf in pdf_data)
            {
                if (pdf.Contains('$'))
                {
                    string[] items = pdf.Split('$');
                    priceList[items[0]] = items[1];
                }
                else if(pdf.Contains("Price on request"))
                {
                    string[] items_with_priceonrequest = pdf.Split("Price on request");
                    priceList[items_with_priceonrequest[0]] = "Price on request";
                }
            }
            return priceList;
        }
    }
}