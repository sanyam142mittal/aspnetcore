// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing.Tree;

namespace Microsoft.AspNetCore.Routing.Matching
{
    // This is an adapter to use TreeRouter in the conformance tests
    internal class TreeRouterMatcher : Matcher
    {
        private readonly TreeRouter _inner;

        internal TreeRouterMatcher(TreeRouter inner)
        {
            _inner = inner;
        }

        public async override Task MatchAsync(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var routeContext = new RouteContext(httpContext);
            await _inner.RouteAsync(routeContext);

            if (routeContext.Handler != null)
            {
                httpContext.Request.RouteValues = routeContext.RouteData.Values;
                await routeContext.Handler(httpContext);
            }
        }
    }
}

