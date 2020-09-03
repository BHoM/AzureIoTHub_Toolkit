using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Base;
using System.ComponentModel;
using BH.oM.Quantities.Attributes;

namespace BH.oM.Physical.Sensor
{
    [Description("A measure of temperature collected from a sensor device")]
    public class Temperature : BHoMObject, ISensor
    {
        [BH.oM.Quantities.Attributes.Temperature]
        [Description("The temperature recorded by the sensor")]
        public virtual double Value { get; set; } = 0.0;

        [Description("The time the temperature was recorded")]
        public virtual DateTime TimeStamp { get; set; } = new DateTime();

    }
}
