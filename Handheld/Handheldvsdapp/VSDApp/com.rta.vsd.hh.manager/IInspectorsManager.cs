using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace VSDApp.com.rta.vsd.hh.manager
{
    interface IInspectorsManager
    {
        DataTable GetInspactorsSummery(string startDate, string endDate,string inspactorID);
        DataTable GetGoodByeInspactorSummery(string startDate, string endDate, string inspactorID);
    }
}
