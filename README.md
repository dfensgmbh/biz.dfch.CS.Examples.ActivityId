# biz.dfch.CS.Examples.ActivityId
[![License](https://img.shields.io/badge/license-Apache%20License%202.0-blue.svg)](https://github.com/dfensgmbh/biz.dfch.CS.Examples.ActivityId/blob/master/LICENSE)

Sample usage of System.Diagnostics.Trace.CorrelationManager.ActivityId in context of OData controllers


# ETW Listener

## General

There is a NuGet package called [`Semantic Logging Application Block`](https://www.nuget.org/packages/EnterpriseLibrary.SemanticLogging.Service/) that allows viewing logs written to an event source:

> The Semantic Logging Application Block provides a set of destinations (sinks) to persist application events published using a subclass of the EventSource class from the System.Diagnostics.Tracing namespace. Sinks include Azure table storage, SQL Server databases, file, console and rolling files with several formats and you can extend the block by creating your own custom formatters and sinks. The console sink is part of this nuget package. Other Sinks mentioned above are available as separate nuget packages. For the sinks that can store structured data, the block preserves the full structure of the event payload in order to facilitate analysing or processing the logged data.


## Setup

1. Install NuGet package [`Semantic Logging Application Block`](https://www.nuget.org/packages/EnterpriseLibrary.SemanticLogging.Service/) 
  
  `PS C:\DESTINATION\DIRECTORY> PATH\TO\NUGET\nuget.exe install EnterpriseLibrary.SemanticLogging.Service`

1. Execute `C:\DESTINATION\DIRECTORY\EnterpriseLibrary.SemanticLogging.Service.2.0.1406.1\tools\install-packages.ps1`
1. Open `C:\DESTINATION\DIRECTORY\EnterpriseLibrary.SemanticLogging.Service.2.0.1406.1\tools\SemanticLogging-svc.xml`
1. Add the following `flatFileSink` under `<!--[Add any built-in or custom sink definition here]-->`

```xml
<flatFileSink name="apcFlatFileSink" fileName="C:\Logs\appclusive.log" >
  <sources>
	<!-- The below settings shows a simple configuration sample for the buit-in non-transient fault tracing -->
	<!-- Remove this eventSource if you'd like, and add your own configuration according to the documentation -->
	<!-- The name attribute is from the EventSource.Name Property -->
	<eventSource name="BizDfch-Example-ActivityId-edb910e7-c953-4c79-b04f-15efa5400456" level="LogAlways" matchAnyKeyword="4294967295" />
  </sources>
  <!--[Add any built-in or custom formatter here if the sink supports text formatters]-->
  <eventTextFormatter header="----------"/>
</flatFileSink>
```

### Windows Service

1. Execute `C:\DESTINATION\DIRECTORY\EnterpriseLibrary.SemanticLogging.Service.2.0.1406.1\tools\SemanticLogging-svc.exe -install`

  Service will be installed under the following name: `Enterprise Library Semantic Logging Service`

1. Set service user to a service account
  * User has to possess `Log On As Service` right
  * User must have write access to configured log file

1. Startup type: `Automatic`
1. Start service

## Listen to source

### Console

1. Open `C:\DESTINATION\DIRECTORY\EnterpriseLibrary.SemanticLogging.Service.2.0.1406.1\tools\SemanticLogging-svc.xml`
1. Add the following `consoleSink`
  ```xml
  <consoleSink name="ConsoleEventSink">
    <sources>
  	<eventSource name="BizDfch-Example-ActivityId-edb910e7-c953-4c79-b04f-15efa5400456" level="LogAlways" matchAnyKeyword="4294967295" />
    </sources>
    <eventTextFormatter header="+=========================================+"/>
  </consoleSink>
  ```

1. Execute `C:\DESTINATION\DIRECTORY\EnterpriseLibrary.SemanticLogging.Service.2.0.1406.1\tools\SemanticLogging-svc.exe -console`
