/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Adapter;

using System.ComponentModel;

namespace BH.oM.Adapters.AzureIoTHub
{
    public class AzureIoTHubConfig : ActionConfig
    {
        /*[Description("To find the end point run (az iot hub show --query properties.eventHubEndpoints.events.endpoint --name {your IoT Hub name} ) (without the brackets) in an Azure Cloud Shell")]
        public virtual string EventHubsCompatibleEndPoint { get; set; } = "";

        [Description("To find the compatible path run (az iot hub show --query properties.eventHubEndpoints.events.path --name {your IoT Hub name} ) (without the brackets) in an Azure Cloud Shell")]
        public virtual string EventHubsCompatiblePath { get; set; } = "";

        [Description("To find the SaS Key run (az iot hub policy show --name service --query primaryKey --hub-name {your IoT Hub name} ) (without the brackets) in an Azure Cloude Shell")]
        public virtual string IoTHubSasKey { get; set; } = "";

        [Description("This should typically be set to service (default) as the Key Name for the SaS")]
        public virtual string IoTHubSasKeyName { get; set; } = "service";*/

        public virtual string ConnectionString { get; set; } = "";

        // az iot hub show --query properties.eventHubEndpoints.events.endpoint --name {your IoT Hub name}
        /*private readonly static string s_eventHubsCompatibleEndpoint = "sb://ihsuproddbres031dednamespace.servicebus.windows.net/";

        // Event Hub-compatible name
        // az iot hub show --query properties.eventHubEndpoints.events.path --name {your IoT Hub name}
        private readonly static string s_eventHubsCompatiblePath = "iothub-ehub-bhomhub-4322670-d57101d060";

        // az iot hub policy show --name service --query primaryKey --hub-name {your IoT Hub name}
        private readonly static string s_iotHubSasKey = "ezqBqF71POVKiMw/AQaGrBxYQNPgwvjxqhN7Oqgnp0w=";
        private readonly static string s_iotHubSasKeyName = "service";*/
    }
}


