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
using System.Linq;
using System.Threading;
using System.Web;
using Logger = biz.dfch.CS.Examples.ActivityId.Logging.BizDfchCsExamplesActivityId;

namespace biz.dfch.CS.Examples.ActivityId
{
    public class GlobalUpdateConfigurationEventHandler
    {
        private static int _timerCallbackPeriodMilliseconds = 10 * 1000;

        private static Timer _timer;

        /// <summary>
        /// This event can be used to receive update notification events every time a configuration changes.
        /// The state (of what has changed) will be passed to the subscriber as 'object e'. Peek Definition to see an example.
        /// </summary>
        public static event EventHandler<object> RaiseUpdateConfigurationEvent;

        static GlobalUpdateConfigurationEventHandler()
        {
            // LOGGING - Case 2 - Logging in static constructor if class got initiated via Global.asax.cs
            Logger.Default.Start("CASE-2-LOGGING-IN-STATIC-CONSTRUCTOR-IF-CLASS-GOT-INITIATED-VIA-GLOBAL-ASAX-CS");

            TimerCallback tcb = RunRaiseUpdateConfigurationEvent;
            _timer = new Timer(tcb, null, 0, _timerCallbackPeriodMilliseconds);

            // LOGGING - Case 2 - Logging in static constructor if class got initiated via Global.asax.cs
            Logger.Default.End("CASE-2-LOGGING-IN-STATIC-CONSTRUCTOR-IF-CLASS-GOT-INITIATED-VIA-GLOBAL-ASAX-CS");
        }

        internal static void RunRaiseUpdateConfigurationEvent(object state)
        {
            RunRaiseUpdateConfigurationEvent(null, state);
        }

        /// <summary>
        /// Raising a configuration update event
        /// </summary>
        /// <param name="sender">Type of the sender or null for global updates</param>
        /// <param name="state">Object with state information or null for stateless updates</param>
        internal static void RunRaiseUpdateConfigurationEvent(object sender, object state)
        {
            // LOGGING - Case 1 - Logging in methods executed via TimerCallback
            Logger.Default.Start("CASE-1-LOGGING-IN-METHODS-EXECUTED-VIA-TIMERCALLBACK");

            var _state = "Arbitrary";

            if (null != RaiseUpdateConfigurationEvent)
            {
                Logger.Default.Start(string.Format("Raising event for '{0}' subscribers", RaiseUpdateConfigurationEvent.GetInvocationList().Length));
                RaiseUpdateConfigurationEvent(sender, _state);
                Logger.Default.EndSucceeded(string.Format("Raising event for '{0}' subscribers", RaiseUpdateConfigurationEvent.GetInvocationList().Length));
            }

            // LOGGING - Case 1 - Logging in methods executed via TimerCallback
            Logger.Default.End("CASE-1-LOGGING-IN-METHODS-EXECUTED-VIA-TIMERCALLBACK");
        }
    }
}