using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevexpressReport.Web.BL.Abstract;
using DevexpressReport.Web.Data;
using DevexpressReport.Web.Models;

namespace DevexpressReport.Web.BL.Concrete
{
    public class ReportBL : IReportBL
    {
        private readonly ReportDbContext reportDbContext;

        public ReportBL(ReportDbContext _reportDbContext)
        {
            reportDbContext = _reportDbContext;
        }
    
        public bool AddReport(ReportModel model)
        {
            try
            {
                reportDbContext.ReportModels.Add(model);
                reportDbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteReport(int id)
        {
            try
            {
                ReportModel entity = reportDbContext.ReportModels.FirstOrDefault(x => x.Id == id);
                reportDbContext.ReportModels.Remove(entity);
                reportDbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ReportModel> GetAllReport()
        {
            return reportDbContext.ReportModels.ToList();
        }

        public ReportModel GetReport(int id)
        {
            return reportDbContext.ReportModels.FirstOrDefault(x => x.Id == id);
        }

        public bool UpdateReport(ReportModel model)
        {
            try
            {
                var entity = reportDbContext.ReportModels.Find(model.Id);
                //reportDbContext.Entry<ReportModel>(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                entity.Caption = model.Caption;
                entity.Dashboard = model.Dashboard;
                reportDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
