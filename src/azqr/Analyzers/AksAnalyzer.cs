namespace azqr;

public class AksAnalyzer : IAzureServiceAnalyzer
{
    ContainerServiceManagedClusterData[] data;
    ArmClient client;
    string subscriptionId;
    string resourceGroup;

    public AksAnalyzer(ArmClient client, string subscriptionId, string resourceGroup, ContainerServiceManagedClusterData[] data)
    {
        this.data = data;
        this.client = client;
        this.subscriptionId = subscriptionId;
        this.resourceGroup = resourceGroup;
    }

    public IEnumerable<AzureServiceResult> Review()
    {
        Console.WriteLine("Reviewing AKS...");
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
                AvaliabilityZones = "TOOD",
                PrivateEndpoints = item.PrivateLinkResources.Count() > 0,
                DiagnosticSettings = diagnosticsCount > 0,
                CAFNaming = item.Name.StartsWith("aks")
            };
        }
    }
}