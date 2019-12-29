using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace VSDApp.com.rta.vsd.validation
{
    interface IValidation
    {
        string Validate(UserControl control);
    }

}
