using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.Codeflix.Catalog.E2ETests.Base;
public class TestServerHttpMessageHandlerBuilder : HttpMessageHandlerBuilder
{
    public TestServerHttpMessageHandlerBuilder(TestServer testServer)
    {
        PrimaryHandler = testServer.CreateHandler();
    }

    public override IList<DelegatingHandler> AdditionalHandlers { get; } = new List<DelegatingHandler>();
    public override string? Name { get; set; }
    public override HttpMessageHandler PrimaryHandler { get; set; }

    public override HttpMessageHandler Build()
    {
        if (PrimaryHandler == null)
        {
            throw new InvalidOperationException($"{nameof(PrimaryHandler)} must not be null");
        }
        return CreateHandlerPipeline(PrimaryHandler, AdditionalHandlers);
    }
}
