namespace azqr;

public class SignalRAnalyzer : IAzureServiceAnalyzer
{
    SignalRData[] data;
    ArmClient client;
    string subscriptionId;
    string resourceGroup;

    public SignalRAnalyzer(ArmClient client, string subscriptionId, string resourceGroup, SignalRData[] data)
    {
        this.data = data;
        this.client = client;
        this.subscriptionId = subscriptionId;
        this.resourceGroup = resourceGroup;
    }

    public IEnumerable<AzureServiceResult> Review()
    {
        Console.WriteLine("Reviewing SignalR...");
        foreach (var item in data)
        {
            var diagnostics = client.GetDiagnosticSettings(new ResourceIdentifier(item.Id!));
            var diagnosticsCount = diagnostics.Count();

            yield return new AzureServiceResult
            {
                SubscriptionId = subscriptionId,
                ResourceGroup = resourceGroup,
                ServiceName = item.Name,
                Sku = item.Sku.Name.ToString()!,
                Sla = "TODO",
                Type = item.ResourceType,
                AvaliabilityZones = item.Sku.Name.Contains("Premium") ? "Yes" : "No",
                PrivateEndpoints = item.PrivateEndpointConnections.Count() > 0,
                DiagnosticSettings = diagnosticsCount > 0,
                CAFNaming = item.Name.StartsWith("sigr")
            };
        }
    }
}