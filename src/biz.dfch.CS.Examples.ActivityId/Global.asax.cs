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
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using Logger = biz.dfch.CS.Examples.ActivityId.Logging.BizDfchCsExamplesActivityId;

namespace biz.dfch.CS.Examples.ActivityId
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // LOGGING - Simple logging in Global.asax.cs
            Logger.Default.Start("SIMPLE-LOGGING-IN-GLOBAL-ASAX");

            // Register OData endpoints
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Initialise GlobalUpdateConfigurationEventHandler
            new GlobalUpdateConfigurationEventHandler();

            // LOGGING - Simple logging in Global.asax.cs
            Logger.Default.End(true);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Logger.Default.Start("SIMPLE-LOGGING-IN-GLOBAL-ASAX");

            Logger.Default.End(true);
        }

        //protected void Session_Start(object sender, EventArgs e)
        //{
        //}

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }

        //protected void Application_AuthenticateRequest(object sender, EventArgs e)
        //{
        //}

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //}

        //protected void Session_End(object sender, EventArgs e)
        //{
        //}
    }
}