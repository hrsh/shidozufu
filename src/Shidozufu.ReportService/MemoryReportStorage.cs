using Shidozufu.Shared;
using System.Collections.Generic;

namespace Shidozufu.ReportService
{
    public class MemoryReportStorage : IMemoryReportStorage
    {
        private readonly IList<Report> _reports = new List<Report>();

        public void Add(Report report)
        {
            _reports.Add(report);
        }

        public IEnumerable<Report> Get() => _reports;
    }
}
