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
using System.Diagnostics.Contracts;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace biz.dfch.CS.Examples.ActivityId.Logging
{
    [EventSource(Name = "BizDfch-Example-ActivityId-edb910e7-c953-4c79-b04f-15efa5400456")]
    public partial class BizDfchCsExamplesActivityId : EventSource
    {
        private const int GENERAL_BASE_EVENT_ID = 32;

        private static readonly BizDfchCsExamplesActivityId _log = new BizDfchCsExamplesActivityId();

        private BizDfchCsExamplesActivityId()
        {
            // Enforce singleton pattern
        }

        static BizDfchCsExamplesActivityId()
        {
            Contract.Assert(null != _log);
        }

        public static BizDfchCsExamplesActivityId Default
        {
            get
            {
                return _log;
            }
        }

        public class Keywords
        {
            // Keywords are bitfields, so make sure you correctly assign their values
            public const EventKeywords Start = (EventKeywords)0x0000000000000001;
            public const EventKeywords End = (EventKeywords)0x0000000000000002;
            public const EventKeywords Exception = (EventKeywords)0x0000000000000004;
            public const EventKeywords Debug = (EventKeywords)0x0000000000000008;
            public const EventKeywords Trace = (EventKeywords)0x0000000000000010;
            public const EventKeywords Notice = (EventKeywords)0x0000000000000020;
            public const EventKeywords Alert = (EventKeywords)0x0000000000000040;
            public const EventKeywords Emergency = (EventKeywords)0x0000000000000080;
            public const EventKeywords Diagnostics = (EventKeywords)0x0000000000000100;
            public const EventKeywords Performance = (EventKeywords)0x0000000000000200;
            public const EventKeywords Legacy = (EventKeywords)0x0000000000000400;
            public const EventKeywords Sensitive = (EventKeywords)0x0000000000000800;
        }

        public class Tasks
        {
            // use incremental numbers for Tasks
            // use Tasks as "categories" for modules or classes
            public const EventTask OData = (EventTask)1;
        }

        //public class OpCodes
        //{
        //    // Opcode must start at 11 and consist of incremental numbers
        //    // see https://msdn.microsoft.com/en-us/library/system.diagnostics.tracing.eventopcode(v=vs.110).aspx for predefined OpCodes
        //    public const EventOpcode General = (EventOpcode)11;
        //    public const EventOpcode Exception = (EventOpcode)12;
        //}

        [Event(GENERAL_BASE_EVENT_ID + 0, Message = "{2}-START {0}", Level = EventLevel.Verbose, Keywords = Keywords.Start, Opcode = EventOpcode.Start)]
        internal void Start(string message = "", string activityId = "", [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            if ("" == activityId) activityId = System.Diagnostics.Trace.CorrelationManager.ActivityId.ToString();
            if (this.IsEnabled()) this.WriteEvent(GENERAL_BASE_EVENT_ID + 0, message, activityId, memberName);
        }

        [Event(GENERAL_BASE_EVENT_ID + 1, Message = "{2}-END {0}", Level = EventLevel.Verbose, Keywords = Keywords.End, Opcode = EventOpcode.Stop)]
        internal void End(string message = "", string activityId = "", [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            if ("" == activityId) activityId = System.Diagnostics.Trace.CorrelationManager.ActivityId.ToString();
            if (this.IsEnabled()) this.WriteEvent(GENERAL_BASE_EVENT_ID + 1, message, activityId, memberName);
        }

        [Event(GENERAL_BASE_EVENT_ID + 2, Message = "{2}-END [SUCCEEDED] {0}", Level = EventLevel.Verbose, Keywords = Keywords.End, Opcode = EventOpcode.Stop)]
        internal void EndSucceeded(string message = "", string activityId = "", [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            if ("" == activityId) activityId = System.Diagnostics.Trace.CorrelationManager.ActivityId.ToString();
            if (this.IsEnabled()) this.WriteEvent(GENERAL_BASE_EVENT_ID + 2, message, activityId, memberName);
        }

        [Event(GENERAL_BASE_EVENT_ID + 3, Message = "{2}-END [FAILED] {0}", Level = EventLevel.Verbose, Keywords = Keywords.End, Opcode = EventOpcode.Stop)]
        internal void EndFailed(string message = "", string activityId = "", [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            if ("" == activityId) activityId = System.Diagnostics.Trace.CorrelationManager.ActivityId.ToString();
            if (this.IsEnabled()) this.WriteEvent(GENERAL_BASE_EVENT_ID + 3, message, activityId, memberName);
        }

        [NonEvent]
        internal void End(bool hasSucceeded, string message = "", string activityId = "", [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            if (this.IsEnabled())
            {
                if ("" == activityId) activityId = System.Diagnostics.Trace.CorrelationManager.ActivityId.ToString();
                if (hasSucceeded)
                {
                    this.EndSucceeded(message, activityId, memberName);
                }
                else
                {
                    this.EndFailed(message, activityId, memberName);
                }
            }
        }

        [Event(GENERAL_BASE_EVENT_ID + 4, Message = "{0}-EX {1}: '{2}'\r\n[{3}]\r\n{4}", Level = EventLevel.Error, Keywords = Keywords.Exception)]
        internal void ContractException(Guid activityId, string name, string source, string message, string stackTrace)
        {
            if (this.IsEnabled()) this.WriteEvent(GENERAL_BASE_EVENT_ID + 4, activityId, name, source, message, stackTrace);
        }

        [NonEvent]
        internal void ContractException(Guid activityId, Exception ex)
        {
            if (this.IsEnabled()) this.ContractException(activityId, ex.GetType().Name, ex.Source, ex.Message, ex.StackTrace);
            var message = string.Format(
                "{0}-EX {1}"
                ,
                activityId.ToString()
                ,
                ex.Message
                );
        }

        [Event(GENERAL_BASE_EVENT_ID + 5, Message = "{0}-EX-INNER-{5} {1}: '{2}'\r\n[{3}]\r\n{4}", Level = EventLevel.Error, Keywords = Keywords.Exception)]
        internal void ContractInnerException(Guid activityId, string name, string source, string message, string stackTrace, int innerExceptionCount)
        {
            if (this.IsEnabled()) this.WriteEvent(GENERAL_BASE_EVENT_ID + 5, activityId, name, source, message, stackTrace, innerExceptionCount);
        }

        [NonEvent]
        internal void ContractInnerException(Guid activityId, Exception ex, int innerExceptionCount)
        {
            if (this.IsEnabled()) this.ContractInnerException(activityId, ex.GetType().Name, ex.Source, ex.Message, ex.StackTrace, innerExceptionCount);
        }

        [Event(GENERAL_BASE_EVENT_ID + 6, Message = "{0}-END", Level = EventLevel.Verbose, Keywords = Keywords.End | Keywords.Exception, Task = Tasks.OData, Opcode = EventOpcode.Stop)]
        internal void EndOdataActionException(Guid activityId)
        {
            // only write "END" statement as thereis a separate exception handler
            // writing out the exception details
            if (this.IsEnabled()) this.WriteEvent(GENERAL_BASE_EVENT_ID + 6, activityId);

            var message = string.Format(
                "{0}-END"
                ,
                activityId.ToString()
                );
        }

        [Event(GENERAL_BASE_EVENT_ID + 7, Message = "{0}-END [{1}]", Level = EventLevel.Verbose, Keywords = Keywords.End, Task = Tasks.OData, Opcode = EventOpcode.Stop)]
        internal void EndOdataAction(Guid activityId, int statusCode)
        {
            // write out HTTP response by default
            if (this.IsEnabled()) this.WriteEvent(GENERAL_BASE_EVENT_ID + 7, activityId, statusCode);

            var message = string.Format(
                "{0}-END [{1}]"
                ,
                activityId.ToString()
                ,
                statusCode
                );
        }

        [Event(GENERAL_BASE_EVENT_ID + 8, Message = "{0}-END", Level = EventLevel.Error, Keywords = Keywords.End, Task = Tasks.OData, Opcode = EventOpcode.Stop)]
        internal void EndOdataActionInvalidState(Guid activityId)
        {
            // unlikely that this will ever be executed
            // just in case - only write out "END" statement
            if (this.IsEnabled()) this.WriteEvent(GENERAL_BASE_EVENT_ID + 8, activityId);

            var message = string.Format(
                "{0}-END"
                ,
                activityId.ToString()
                );
        }

        [Event(GENERAL_BASE_EVENT_ID + 9, Message = "{0}-START [{1} {2}] [{3}.{4}] [{5}@{6}]", Level = EventLevel.Verbose, Keywords = Keywords.Start, Task = Tasks.OData, Opcode = EventOpcode.Stop)]
        internal void StartOdataAction(Guid activityId, string method, string requestUri, string controllerName, string actionName, string userName, string tenantId)
        {
            if (this.IsEnabled()) this.WriteEvent(GENERAL_BASE_EVENT_ID + 9, activityId, method, requestUri, controllerName, actionName, string.IsNullOrWhiteSpace(userName) ? "anonymous" : userName, tenantId);
        }

        [NonEvent]
        internal void StartOdataAction(System.Web.Http.Controllers.HttpActionContext actionContext, string tenantId)
        {
            var userName = actionContext.RequestContext.Principal.Identity.Name;
            if (this.IsEnabled())
            {
                this.StartOdataAction(
                    System.Diagnostics.Trace.CorrelationManager.ActivityId,
                    actionContext.Request.Method.Method,
                    actionContext.Request.RequestUri.ToString(),
                    actionContext.ControllerContext.ControllerDescriptor.ControllerName,
                    actionContext.ActionDescriptor.ActionName,
                    string.IsNullOrWhiteSpace(userName) ? "anonymous" : userName,
                    tenantId
                    );
            }
        }

        [Event(GENERAL_BASE_EVENT_ID + 10, Message = "{0}-EX [{1} {2}] [{3}@{4}] {5} [{7}@{8}]", Level = EventLevel.Error, Keywords = Keywords.Exception)]
        internal void TraceException(Guid activityId, string method, string requestUri, string name, string source, string message, string stackTrace, string userName, string tenantId)
        {
            if (this.IsEnabled()) this.WriteEvent(GENERAL_BASE_EVENT_ID + 10, activityId, method, requestUri, name, source, message, stackTrace, userName, tenantId);
        }

        [NonEvent]
        internal void TraceException(Guid activityId, ExceptionLoggerContext context, string userName, string tenantId)
        {
            if (this.IsEnabled()) this.TraceException(
                System.Diagnostics.Trace.CorrelationManager.ActivityId,
                context.Request.Method.Method,
                context.Request.RequestUri.ToString(),
                context.Exception.GetType().Name,
                context.Exception.Source,
                context.Exception.Message,
                context.Exception.StackTrace,
                string.IsNullOrWhiteSpace(userName) ? "anonymous" : userName,
                tenantId
                );
        }

        [Event(GENERAL_BASE_EVENT_ID + 11, Message = "{0}-EX-INNER-{9} [{1} {2}] [{3}@{4}] {5} [{7}@{8}]", Level = EventLevel.Error, Keywords = Keywords.Exception)]
        internal void TraceInnerException(Guid activityId, string method, string requestUri, string name, string source, string message, string stackTrace, string userName, string tenantId, int innerExceptionCount)
        {
            if (this.IsEnabled()) this.WriteEvent(GENERAL_BASE_EVENT_ID + 11, activityId, method, requestUri, name, source, message, stackTrace, userName, tenantId, innerExceptionCount);
        }

        [NonEvent]
        internal void TraceInnerException(Guid activityId, ExceptionLoggerContext context, Exception ex, string userName, string tenantId, int innerExceptionCount)
        {
            if (this.IsEnabled()) this.TraceInnerException(activityId, context.Request.Method.Method, context.Request.RequestUri.ToString(), ex.GetType().Name, ex.Source, ex.Message, ex.StackTrace, userName, tenantId, innerExceptionCount);
        }
    }
}