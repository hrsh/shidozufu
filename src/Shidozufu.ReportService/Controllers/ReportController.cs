using Microsoft.AspNetCore.Mvc;
using Shidozufu.Shared;
using System.Collections.Generic;

namespace Shidozufu.ReportService.Controllers
{
    [ApiController, Route("api/v1/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IMemoryReportStorage _memoryReportStorage;

        public ReportController(IMemoryReportStorage memoryReportStorage) =>
            _memoryReportStorage = memoryReportStorage;

        [HttpGet]
        public IEnumerable<Report> Get() =>
            _memoryReportStorage.Get();
    }
}
