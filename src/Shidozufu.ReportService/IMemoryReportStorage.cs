using Shidozufu.Shared;
using System.Collections.Generic;

namespace Shidozufu.ReportService
{
    public interface IMemoryReportStorage
    {
        void Add(Report report);
        IEnumerable<Report> Get();
    }
}