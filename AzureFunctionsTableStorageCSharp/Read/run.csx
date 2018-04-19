#r "Microsoft.WindowsAzure.Storage"
#load "product.csx"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, IQueryable<Product> inputTable, TraceWriter log)
{
    if (req.Method == HttpMethod.Get)
    {
        return req.CreateResponse(HttpStatusCode.OK, inputTable.ToList(), "application/json");
    }
    else
    {
        return req.CreateResponse(HttpStatusCode.BadRequest, "This route accepts only GET requests.");
    }
}
