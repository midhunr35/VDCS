using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSDApp.com.rta.vsd.hh.manager
{
    interface IDriverDetailManager
    {
        bool InquireDriverDetails(string driverCountry, string driverEmirates, string driverLicNumber);
    }
}
