using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSDApp.com.rta.vsd.hh.manager
{
    interface ILoginManager
    {
        void SetupServiceCall();
        bool OfflineLogin(string userName, string userPass);
        bool LoginOnline(string username, string password);
    }
}
