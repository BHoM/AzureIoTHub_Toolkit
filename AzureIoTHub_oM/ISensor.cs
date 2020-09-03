using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Base;

namespace BH.oM.Physical.Sensor
{
    interface ISensor : IBHoMObject
    {
        double Value { get; set; }
        DateTime TimeStamp { get; set; }
    }
}
