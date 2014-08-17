using System;

namespace Adam_new
{

    // класс, в экземпляре которого хранятся записи для отчета по распределению
    class ReportDistributionData
    {
        public string Name
        {
            get;
            set;
        }

        public string Percent
        {
            get;
            set;
        }

        public Single Sum
        {
            get;
            set;
        }
    }
}
