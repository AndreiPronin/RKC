using BE.Counter;
using BL.Helper;
using BL.http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using RkcTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RkcTest.http
{
    [TestClass]
    public class RequestTest
    {
        [TestMethod]
        public async Task PostRequest()
        {
            var convertJson = new ConvertJson<ModelAddPU>(ModelCreator.modelAddPU);
            var content = new StringContent(convertJson.ConverModelToJson());
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = content });

            var mockRequest = new Mock<Reuqest<ModelAddPU>>();
            mockRequest.Protected().SetupGet<HttpClient>("_httpClient")
            .Returns(new HttpClient(mockHttpMessageHandler.Object));

            var res = await mockRequest.Object.PostRequest(ModelCreator.modelAddPU);
            Assert.IsNotNull(res);
        }
    }
}
