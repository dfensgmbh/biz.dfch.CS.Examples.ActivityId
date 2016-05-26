/**
 * Copyright 2016 d-fens GmbH
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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;.
using Logger = biz.dfch.CS.Examples.ActivityId.Logging.BizDfchCsExamplesActivityId;

namespace biz.dfch.CS.Examples.ActivityId.Logging
{
    // filters are processed in random order
    // see https://damienbod.wordpress.com/2014/01/04/web-api-2-using-actionfilterattribute-overrideactionfiltersattribute-and-ioc-injection/
    // therefore we process logging and validation in a single action
    public class LogggingAndModelStateValidationActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (null == actionContext)
            {
                return;
            }

            var tid = "unknown";
            Logger.Default.StartOdataAction(actionContext, tid);

            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request
                    .CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            // check for null AFTER the other filters got called
            if (null == actionExecutedContext)
            {
                return;
            }

            var activityId = System.Diagnostics.Trace.CorrelationManager.ActivityId;
            if (null != actionExecutedContext.Exception)
            {
                Logger.Default.EndOdataActionException(activityId);
            }
            else if (null != actionExecutedContext.Response)
            {
                Logger.Default.EndOdataAction(activityId, (int)actionExecutedContext.Response.StatusCode);
            }
            else
            {
                Logger.Default.EndOdataActionInvalidState(activityId);
            }
        }
    }
}