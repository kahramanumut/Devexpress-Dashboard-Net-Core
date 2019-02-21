using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevexpressReport.Web.Models;

namespace DevexpressReport.Web.BL.Abstract
{
    public interface IReportBL
    {
        ReportModel GetReport(int id);
        List<ReportModel> GetAllReport();
        bool DeleteReport(int id);
        bool AddReport(ReportModel model);
        bool UpdateReport(ReportModel model);

    }
}
