﻿/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BH.oM.Base;
using BH.oM.Adapter;

using BH.oM.Adapters.AzureIoTHub;

using Microsoft.Azure.EventHubs;

namespace BH.Adapter.AzureIoTHub.CRUD
{
    public partial class AzureIoTHubAdapter : BHoMAdapter
    {

        protected override IEnumerable<IBHoMObject> IRead(Type type, IList indices = null, ActionConfig actionConfig = null)
        {
            if (actionConfig == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please provide configuration settings to push to an XML file");
                return new List<IBHoMObject>();
            }

            AzureIoTHubConfig config = actionConfig as AzureIoTHubConfig;
            if (config == null)
            {
                BH.Engine.Reflection.Compute.RecordError("Please provide valid a AzureIoTHubConfig object for connecting to an IoT device");
                return new List<IBHoMObject>();
            }


            return Read(config).Result;
        }

        private async Task<List<IBHoMObject>> Read(AzureIoTHubConfig config)
        {
            var connectionString = new EventHubsConnectionStringBuilder(new Uri(config.EventHubsCompatibleEndPoint), config.EventHubsCompatiblePath, config.IoTHubSasKeyName, config.IoTHubSasKey);

            EventHubClient client = EventHubClient.CreateFromConnectionString(connectionString.ToString());

            CancellationToken ct = new CancellationToken();
            var runtimeInfo = await client.GetRuntimeInformationAsync();
            var d2cPartitions = runtimeInfo.PartitionIds;

            CancellationTokenSource cts = new CancellationTokenSource();

            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                BH.Engine.Reflection.Compute.RecordNote("Exiting...");
            };

            var tasks = new List<Task>();
            var partition = d2cPartitions[0];

            var eventHubReceiver = client.CreateReceiver("$Default", partition, EventPosition.FromEnqueuedTime(DateTime.Now));
            BH.Engine.Reflection.Compute.RecordNote("Create receiver on partition: " + partition);
            while (true)
            {
                if (ct.IsCancellationRequested)
                    break;

                BH.Engine.Reflection.Compute.RecordNote("Listening for messages on: " + partition);
                // Check for EventData - this methods times out if there is nothing to retrieve.
                var events = await eventHubReceiver.ReceiveAsync(100);

                // If there is data in the batch, process it.
                if (events == null) continue;

                foreach (EventData eventData in events)
                {
                    string data = Encoding.UTF8.GetString(eventData.Body.Array);
                    BH.Engine.Reflection.Compute.RecordNote(string.Format("Message received on partition {0}:", partition));
                    BH.Engine.Reflection.Compute.RecordNote(string.Format("  {0}:", data));
                    BH.Engine.Reflection.Compute.RecordNote("Application properties (set by device):");

                    foreach (var prop in eventData.Properties)
                    {
                        BH.Engine.Reflection.Compute.RecordNote(string.Format("  {0}: {1}", prop.Key, prop.Value));
                    }

                    BH.Engine.Reflection.Compute.RecordNote("System properties (set by IoT Hub):");
                    foreach (var prop in eventData.SystemProperties)
                    {
                        BH.Engine.Reflection.Compute.RecordNote(string.Format("  {0}: {1}", prop.Key, prop.Value));
                    }
                }

                break;
            }

            return new List<IBHoMObject>();
        }
    }
}
