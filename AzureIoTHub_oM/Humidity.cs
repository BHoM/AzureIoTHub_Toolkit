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
    [Description("A measure of humidity collected from a sensor device")]
    public class Humidity : BHoMObject, ISensor
    {
        [Description("The humidity recorded by the sensor")]
        public virtual double Value { get; set; } = 0.0;

        [Description("The time the humidity was recorded")]
        public virtual DateTime TimeStamp { get; set; } = new DateTime();

    }
}
