﻿/**
 * Copyright 2015 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Diagnostics.Contracts;
using System.Web.Http;
using Logger = biz.dfch.CS.Examples.ActivityId.Logging.BizDfchCsExamplesActivityId;
using biz.dfch.CS.Examples.ActivityId.Logging;
using System.Web.Http.OData.Builder;
using biz.dfch.CS.Examples.ActivityId.OdataServices.Core;

namespace biz.dfch.CS.Examples.ActivityId
{
    public static class WebApiConfig
    {
        private static string _apiBase = "api";

        public static void Register(HttpConfiguration config)
        {
            Contract.Requires(null != config);

            // LOGGING - Logging from static method called via Global.asax.cs
            Logger.Default.Start("LOGGING-FROM-STATIC-METHOD-CALLED-VIA-GLOBAL-ASAX-CS");
            
            // Action filters
            config.Filters.Add(new LogggingAndModelStateValidationActionFilterAttribute());

            config.MapHttpAttributeRoutes();

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            SamplesController.ModelBuilder(builder);
            config.Routes.MapODataRoute("Core", "Core", builder.GetEdmModel());

            // LOGGING - Logging from static method called via Global.asax.cs
            Logger.Default.End("LOGGING-FROM-STATIC-METHOD-CALLED-VIA-GLOBAL-ASAX-CS");
        }
    }
}
