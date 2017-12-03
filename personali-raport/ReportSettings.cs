using System;


namespace personali_raport
{
    public enum ReportType { PERSREP, MIDREP, ATTENDANCE };
    public class ReportSettings
    {
        public ReportType reportType;
        public DateTime startOfReport;
        public DateTime endOfReport;
        public string personnelFileName;
        public string reportTemplate;
        public string reportFileName;
        public string companyFilter;
        public string platoonFilter;
    }
}
