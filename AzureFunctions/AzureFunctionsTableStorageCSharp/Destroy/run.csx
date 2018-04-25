#r "Newtonsoft.Json"
#r "Microsoft.WindowsAzure.Storage"
#load "..\Read\product.csx"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;


public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, CloudTable outputTable, TraceWriter log)
{
    dynamic body = await req.Content.ReadAsStringAsync();
    Product data = JsonConvert.DeserializeObject<Product>(body as string);
    Product entity = data.ToEntity();

    var operation = TableOperation.Delete(entity);
    await outputTable.ExecuteAsync(operation);

    return req.CreateResponse(HttpStatusCode.OK, entity, "application/json");
}