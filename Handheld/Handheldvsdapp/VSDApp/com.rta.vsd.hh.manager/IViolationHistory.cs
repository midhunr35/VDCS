using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using VSDApp.com.rta.vsd.hh.data;

namespace VSDApp.com.rta.vsd.hh.manager
{
    interface IViolationHistory
    {
        Vehicle GetViolationHistoryByID(string vioID);
        Vehicle GetOfflineViolationHistoryByID(string vioID);
        Vehicle GetViolationHistoryByPlateNumber(string country, string emirate, string plateCategory, string plateNumber, string plateCode);
        DataTable GetOfflineViolationHistoryAllByID(string PrivisionalViolationID);
    }
}

