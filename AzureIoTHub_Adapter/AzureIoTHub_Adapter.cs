using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Devices;

using Microsoft.Azure.EventHubs;

namespace BH.Adapter.AzureIoTHub
{
    public partial class AzureIoTHubAdapter : BHoMAdapter
    {
        public AzureIoTHubAdapter()
        {
            var connectionString = new EventHubsConnectionStringBuilder(new Uri(s_eventHubsCompatibleEndpoint), s_eventHubsCompatiblePath, s_iotHubSasKeyName, s_iotHubSasKey);

            EventHubClient client = EventHubClient.CreateFromConnectionString(connectionString.ToString());

            Run(client);
        }

        private readonly static string s_eventHubsCompatibleEndpoint = "sb://ihsuproddbres031dednamespace.servicebus.windows.net/";

        // Event Hub-compatible name
        // az iot hub show --query properties.eventHubEndpoints.events.path --name {your IoT Hub name}
        private readonly static string s_eventHubsCompatiblePath = "iothub-ehub-bhomhub-4322670-d57101d060";

        // az iot hub policy show --name service --query primaryKey --hub-name {your IoT Hub name}
        private readonly static string s_iotHubSasKey = "ezqBqF71POVKiMw/AQaGrBxYQNPgwvjxqhN7Oqgnp0w=";
        private readonly static string s_iotHubSasKeyName = "service";

        private async void Run(EventHubClient client)
        {
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
                if (ct.IsCancellationRequested) break;
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
            }
        }

        private static string BuildEventHubsConnectionString(string eventHubsEndpoint,
                                                             string iotHubSharedKeyName,
                                                             string iotHubSharedKey) =>
            $"Endpoint={ eventHubsEndpoint };SharedAccessKeyName={ iotHubSharedKeyName };SharedAccessKey={ iotHubSharedKey }";

    }
}
