using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSDApp.com.rta.vsd.hh.data;

namespace VSDApp.com.rta.vsd.hh.manager
{
    interface IGPSLocation
    {
        bool SubmitHandHeldGeoLocation(HandHeldGPSLocation hhLocation);
    }
}
