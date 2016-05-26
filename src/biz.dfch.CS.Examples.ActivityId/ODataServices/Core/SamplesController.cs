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
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using biz.dfch.CS.Examples.ActivityId.ODataServices.Core;
using Logger = biz.dfch.CS.Examples.ActivityId.Logging.BizDfchCsExamplesActivityId;

namespace biz.dfch.CS.Examples.ActivityId.OdataServices.Core
{
    public class SamplesController : ODataController
    {
        private readonly static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        public SamplesController()
        {
            // N/A
        }

        internal static void ModelBuilder(System.Web.Http.OData.Builder.ODataConventionModelBuilder builder)
        {
            var entitySetName = "Samples";

            builder.EntitySet<Sample>(entitySetName);
        }

        // GET http://HOST:PORT/Core/Samples
        [EnableQuery(PageSize = 45)]
        public IHttpActionResult GetSamples(ODataQueryOptions<Sample> queryOptions)
        {
            Contract.Requires(null != queryOptions, "|400|");

            // LOGGING - Simple synchronous logging
            Logger.Default.Start("SIMPLE-LOGGING - Getting samples");

            queryOptions.Validate(_validationSettings);

            var entitiesResult = new List<Sample>();

            for (int i = 1; i < 50; i ++)
            {
                var entity = new Sample();
                entity.Name = string.Format("Sample-{0}", i);
                entitiesResult.Add(entity);
            }

            // LOGGING - Simple synchronous logging
            Logger.Default.End("SIMPLE-LOGGING - Getting samples");

            return Ok<IEnumerable<Sample>>(entitiesResult);
        }

        // GET http://HOST:PORT/Core/Samples(ID)
        public async Task<IHttpActionResult> GetSample([FromODataUri] long key, ODataQueryOptions<Sample> queryOptions)
        {
            Contract.Requires(0 < key, "|400|");
            Contract.Requires(null != queryOptions, "|400|");

            throw new NotImplementedException();
        }

        public async Task<IHttpActionResult> Put([FromODataUri] long key, Sample entityToBeUpdated)
        {
            Contract.Requires(0 < key, "|400|");
            Contract.Requires(null != entityToBeUpdated, "|400|");

            throw new NotImplementedException();
        }

        public async Task<IHttpActionResult> Post(Sample entityToBeCreated)
        {
            Contract.Requires(null != entityToBeCreated);

            throw new NotImplementedException();
        }

        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] long key, Delta<Sample> delta)
        {
            Contract.Requires(0 < key, "|400|");
            Contract.Requires(null != delta, "|400|");

            throw new NotImplementedException();
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            Contract.Requires(0 < key, "|400|");

            throw new NotImplementedException();
        }
    }
}