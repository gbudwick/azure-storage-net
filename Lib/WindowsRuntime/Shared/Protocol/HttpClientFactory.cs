﻿// -----------------------------------------------------------------------------------------
// <copyright file="HttpClientFactory.cs" company="Microsoft">
//    Copyright 2013 Microsoft Corporation
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// -----------------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
    using Microsoft.WindowsAzure.Storage.Core.Executor;
    using System.Net.Http;
    using System.Net.Http.Headers;

    internal static class HttpClientFactory
    {
        public static HttpClient BuildHttpClient<T>(RESTCommand<T> cmd, HttpMessageHandler handler, bool useVersionHeader, OperationContext operationContext)
        {
            HttpClient client = handler != null ? new HttpClient(handler, false) : new HttpClient();
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(Constants.HeaderConstants.UserAgentProductName, Constants.HeaderConstants.UserAgentProductVersion));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(Constants.HeaderConstants.UserAgentComment));

            if (useVersionHeader)
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(Constants.HeaderConstants.StorageVersionHeader, Constants.HeaderConstants.TargetStorageVersion);
            }

            if (operationContext != null && operationContext.UserHeaders != null)
            {
                foreach (string key in operationContext.UserHeaders.Keys)
                {
                    client.DefaultRequestHeaders.Add(key, operationContext.UserHeaders[key]);
                }
            }

            return client;
        }
    }
}
