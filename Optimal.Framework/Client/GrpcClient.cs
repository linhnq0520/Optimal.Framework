// using Grpc.Core;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text.Json;
// using System.Threading.Tasks;

// namespace Optimal.Framework.Client
// {
//     public class GrpcClient: IDisposable
//     {
//         private class __SafePingService
//         {
//             internal static object GetServiceInfoLockObject = new object();

//             private readonly long GrpcTimeoutInSeconds;

//             public __SafePingService(long pGrpcTimeoutInSeconds)
//             {
//                 GrpcTimeoutInSeconds = pGrpcTimeoutInSeconds;
//             }

//             public ServiceInfo GrpcSafePingNeptune(string pGrpcURL, string pServiceID, string pInstanceID)
//             {
//                 lock (GetServiceInfoLockObject)
//                 {
//                     HttpClientHandler httpClientHandler = new HttpClientHandler();
//                     string token = ClientConfig.NeptuneGrpcToken;
//                     httpClientHandler.ServerCertificateCustomValidationCallback = delegate (HttpRequestMessage httpRequestMessage, X509Certificate2? cert, X509Chain? chain, SslPolicyErrors errors)
//                     {
//                         if (httpRequestMessage.Headers.Authorization.ToString().EndsWith(token))
//                         {
//                             return true;
//                         }
//                         throw new Exception("The server's token is invalid.");
//                     };
//                     CallCredentials callCredentials = CallCredentials.FromInterceptor(delegate (AuthInterceptorContext context, Metadata meta)
//                     {
//                         meta.Add("Authorization", "bearer " + token);
//                         return Task.CompletedTask;
//                     });
//                     NeptuneGrpcService.NeptuneGrpcServiceClient neptuneGrpcServiceClient = new NeptuneGrpcService.NeptuneGrpcServiceClient(GrpcChannel.ForAddress(pGrpcURL, new GrpcChannelOptions
//                     {
//                         HttpHandler = httpClientHandler,
//                         Credentials = ChannelCredentials.Create(new SslCredentials(), callCredentials)
//                     }));
//                     MethodSpec request = new MethodSpec
//                     {
//                         FullClassName = "JITS.Neptune.Lib.GrpcLib.NeptuneServices",
//                         MethodName = "SafePing",
//                         Parameters = { pServiceID, pInstanceID }
//                     };
//                     Metadata headers = new Metadata
//                 {
//                     { "NeptuneGrpcToken", ClientConfig.NeptuneGrpcToken },
//                     {
//                         "Authorization",
//                         "bearer " + token
//                     }
//                 };
//                     return JsonSerializer.Deserialize<ServiceInfo>(NeptuneTextEncryptor.AESDecryptString(JsonSerializer.Deserialize<DataScheme>(neptuneGrpcServiceClient.Invoke(request, headers, DateTime.UtcNow.AddSeconds(GrpcTimeoutInSeconds)).ReturnValue).data));
//                 }
//             }
//         }

//         private Timer timer;

//         private readonly object __lockServiceInfoObject = new object();

//         private ServiceInfo __ServiceInfo = new ServiceInfo();

//         private readonly NeptuneConfiguration __ClientConfiguration = new NeptuneConfiguration();

//         private static Dictionary<string, ServiceInfo> ServiceInfoDictionay = new Dictionary<string, ServiceInfo>();

//         public string ToServiceCode;

//         public static NeptuneConfiguration ClientConfig { get; set; } = new NeptuneConfiguration();


//         public ServiceInfo ServiceInfo
//         {
//             get
//             {
//                 lock (__lockServiceInfoObject)
//                 {
//                     return __ServiceInfo;
//                 }
//             }
//             set
//             {
//                 lock (__lockServiceInfoObject)
//                 {
//                     __ServiceInfo = value;
//                 }
//             }
//         }

//         public GrpcClient()
//         {
//             __ClientConfiguration = ClientConfig.Clone();
//             lock (__lockServiceInfoObject)
//             {
//                 if (!ServiceInfoDictionay.ContainsKey(__ClientConfiguration.YourServiceID))
//                 {
//                     ServiceInfo.QueryServiceInfo(__ClientConfiguration.YourServiceID, __ClientConfiguration.YourServiceID, __ClientConfiguration.YourInstanceID);
//                     ServiceInfoDictionay.Add(__ClientConfiguration.YourServiceID, ServiceInfo);
//                 }
//                 else
//                 {
//                     ServiceInfo = ServiceInfoDictionay[__ClientConfiguration.YourServiceID];
//                 }
//             }
//         }

//         public GrpcClient(string pToServiceCode)
//             : this(pToServiceCode, ClientConfig)
//         {
//         }

//         public GrpcClient(string pToServiceCode, NeptuneConfiguration pClientConfig)
//         {
//             __ClientConfiguration = pClientConfig.Clone();
//             ToServiceCode = pToServiceCode;
//             lock (__lockServiceInfoObject)
//             {
//                 if (!ServiceInfoDictionay.ContainsKey(ToServiceCode))
//                 {
//                     ServiceInfo.QueryServiceInfo(__ClientConfiguration.YourServiceID, ToServiceCode, pClientConfig.YourInstanceID);
//                     ServiceInfoDictionay.Add(ToServiceCode, ServiceInfo);
//                 }
//                 else
//                 {
//                     ServiceInfo = ServiceInfoDictionay[ToServiceCode];
//                 }
//             }
//             if (ServiceInfo.service_code == null)
//             {
//                 throw new Exception("Service [" + pToServiceCode + "] is not found.");
//             }
//         }

//         public void RegisterGrpcService()
//         {
//             HttpClientHandler httpClientHandler = new HttpClientHandler();
//             string token = ClientConfig.NeptuneGrpcToken;
//             httpClientHandler.ServerCertificateCustomValidationCallback = delegate (HttpRequestMessage httpRequestMessage, X509Certificate2? cert, X509Chain? chain, SslPolicyErrors errors)
//             {
//                 if (httpRequestMessage.Headers.Authorization.ToString().EndsWith(token))
//                 {
//                     return true;
//                 }
//                 throw new Exception("The server's token is invalid.");
//             };
//             CallCredentials callCredentials = CallCredentials.FromInterceptor(delegate (AuthInterceptorContext context, Metadata meta)
//             {
//                 meta.Add("Authorization", "Bearer " + token);
//                 return Task.CompletedTask;
//             });
//             NeptuneGrpcService.NeptuneGrpcServiceClient neptuneGrpcServiceClient = new NeptuneGrpcService.NeptuneGrpcServiceClient(GrpcChannel.ForAddress(ClientConfig.NeptuneGrpcURL, new GrpcChannelOptions
//             {
//                 HttpHandler = httpClientHandler,
//                 Credentials = ChannelCredentials.Create(new SslCredentials(), callCredentials)
//             }));
//             MethodSpec request = new MethodSpec
//             {
//                 FullClassName = "JITS.Neptune.Lib.GrpcLib.NeptuneServices",
//                 MethodName = "RegisterServiceGrpcEndpoint",
//                 Parameters = { ClientConfig.YourServiceID, ClientConfig.YourGrpcURL, ClientConfig.YourInstanceID }
//             };
//             Metadata headers = new Metadata { { "NeptuneGrpcToken", ClientConfig.NeptuneGrpcToken } };
//             MethodResult methodResult = neptuneGrpcServiceClient.Invoke(request, headers, DateTime.UtcNow.AddSeconds(ServiceInfo.neptune_grpc_timeout_seconds));
//             if (!methodResult.ReturnValue.ToLower().Equals("ok"))
//             {
//                 throw new NeptuneException("Failed to register service [" + ClientConfig.YourGrpcURL + "]: " + methodResult.ReturnValue);
//             }
//             if (timer != null)
//             {
//                 timer.Dispose();
//             }
//             timer = new Timer(delegate
//             {
//                 try
//                 {
//                     __SafePingService _SafePingService = new __SafePingService(string.IsNullOrEmpty(ToServiceCode) ? ServiceInfo.neptune_grpc_timeout_seconds : ServiceInfo.service_grpc_timeout_seconds);
//                     ServiceInfo = _SafePingService.GrpcSafePingNeptune(ClientConfig.NeptuneGrpcURL, ClientConfig.YourServiceID, ClientConfig.YourInstanceID);
//                 }
//                 catch (Exception ex)
//                 {
//                     Console.WriteLine("Error: " + ex.Message);
//                 }
//             }, null, TimeSpan.Zero, TimeSpan.FromSeconds(ServiceInfo.service_ping_interval_seconds));
//         }

//         public async Task<MethodResult> InvokeAsync(MethodSpec method)
//         {
//             long num = (string.IsNullOrEmpty(ToServiceCode) ? ServiceInfo.neptune_grpc_timeout_seconds : ServiceInfo.service_grpc_timeout_seconds);
//             return await InvokeAsync(method, num * 1000);
//         }

//         public async Task<MethodResult> InvokeAsync(MethodSpec method, long pTimeout_millis)
//         {
//             MethodResult result = new MethodResult();
//             try
//             {
//                 if (ServiceInfo.service_grpc_active != null && !ServiceInfo.service_grpc_active.ToLower().Equals("active"))
//                 {
//                     throw new NeptuneException("Grpc is not active.");
//                 }
//                 HttpClientHandler httpClientHandler = new HttpClientHandler();
//                 string token = ClientConfig.NeptuneGrpcToken;
//                 httpClientHandler.ServerCertificateCustomValidationCallback = delegate (HttpRequestMessage httpRequestMessage, X509Certificate2? cert, X509Chain? chain, SslPolicyErrors errors)
//                 {
//                     if (httpRequestMessage.Headers.Authorization.ToString().EndsWith(token))
//                     {
//                         return true;
//                     }
//                     throw new Exception("The server's token is invalid.");
//                 };
//                 CallCredentials callCredentials = CallCredentials.FromInterceptor(delegate (AuthInterceptorContext context, Metadata meta)
//                 {
//                     meta.Add("Authorization", "bearer " + token);
//                     return Task.CompletedTask;
//                 });
//                 string address = (string.IsNullOrEmpty(ToServiceCode) ? ServiceInfo.neptune_grpc_url : ServiceInfo.service_grpc_url);
//                 using GrpcChannel channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
//                 {
//                     HttpHandler = httpClientHandler,
//                     Credentials = ChannelCredentials.Create(new SslCredentials(), callCredentials),
//                     MaxSendMessageSize = ServiceInfo.grpc_Max_Send_Message_Size_In_MB * 1024 * 1024,
//                     MaxReceiveMessageSize = ServiceInfo.grpc_Max_Receive_Message_Size_In_MB * 1024 * 1024
//                 });
//                 result = await new NeptuneGrpcService.NeptuneGrpcServiceClient(channel).InvokeAsync(headers: new Metadata
//             {
//                 { "NeptuneGrpcToken", ClientConfig.NeptuneGrpcToken },
//                 {
//                     "Authorization",
//                     "bearer " + token
//                 }
//             }, request: method, deadline: DateTime.UtcNow.AddMilliseconds(pTimeout_millis));
//                 Task.Run(delegate
//                 {
//                     __TrackGrpcOnNeptune(method, result);
//                 });
//             }
//             catch (Exception ex)
//             {
//                 result.HasException = true;
//                 result.ExceptionMessage = ex.Message;
//             }
//             return result;
//         }

//         public async Task<MethodResult> InvokeNeptuneGrpcAsync(MethodSpec method)
//         {
//             try
//             {
//                 HttpClientHandler httpClientHandler = new HttpClientHandler();
//                 string token = ClientConfig.NeptuneGrpcToken;
//                 httpClientHandler.ServerCertificateCustomValidationCallback = delegate (HttpRequestMessage httpRequestMessage, X509Certificate2? cert, X509Chain? chain, SslPolicyErrors errors)
//                 {
//                     if (httpRequestMessage.Headers.Authorization.ToString().EndsWith(token))
//                     {
//                         return true;
//                     }
//                     throw new Exception("The server's token is invalid.");
//                 };
//                 CallCredentials callCredentials = CallCredentials.FromInterceptor(delegate (AuthInterceptorContext context, Metadata meta)
//                 {
//                     meta.Add("Authorization", "bearer " + token);
//                     return Task.CompletedTask;
//                 });
//                 NeptuneGrpcService.NeptuneGrpcServiceClient neptuneGrpcServiceClient = new NeptuneGrpcService.NeptuneGrpcServiceClient(GrpcChannel.ForAddress(ClientConfig.NeptuneGrpcURL, new GrpcChannelOptions
//                 {
//                     HttpHandler = httpClientHandler,
//                     Credentials = ChannelCredentials.Create(new SslCredentials(), callCredentials)
//                 }));
//                 Metadata headers = new Metadata
//             {
//                 { "NeptuneGrpcToken", ClientConfig.NeptuneGrpcToken },
//                 {
//                     "Authorization",
//                     "bearer " + token
//                 }
//             };
//                 return await neptuneGrpcServiceClient.InvokeAsync(method, headers, DateTime.UtcNow.AddSeconds(ServiceInfo.neptune_grpc_timeout_seconds));
//             }
//             catch (Exception)
//             {
//                 throw;
//             }
//         }

//         public static async Task<MethodResult> InvokeNeptuneInstanceAsync(string pGrpcURL, MethodSpec method, int pInvokeTimeoutInSeconds = 60)
//         {
//             try
//             {
//                 HttpClientHandler httpClientHandler = new HttpClientHandler();
//                 string token = ClientConfig.NeptuneGrpcToken;
//                 httpClientHandler.ServerCertificateCustomValidationCallback = delegate (HttpRequestMessage httpRequestMessage, X509Certificate2? cert, X509Chain? chain, SslPolicyErrors errors)
//                 {
//                     if (httpRequestMessage.Headers.Authorization.ToString().EndsWith(token))
//                     {
//                         return true;
//                     }
//                     throw new Exception("The server's token is invalid.");
//                 };
//                 CallCredentials callCredentials = CallCredentials.FromInterceptor(delegate (AuthInterceptorContext context, Metadata meta)
//                 {
//                     meta.Add("Authorization", "bearer " + token);
//                     return Task.CompletedTask;
//                 });
//                 NeptuneGrpcService.NeptuneGrpcServiceClient neptuneGrpcServiceClient = new NeptuneGrpcService.NeptuneGrpcServiceClient(GrpcChannel.ForAddress(pGrpcURL, new GrpcChannelOptions
//                 {
//                     HttpHandler = httpClientHandler,
//                     Credentials = ChannelCredentials.Create(new SslCredentials(), callCredentials)
//                 }));
//                 Metadata headers = new Metadata
//             {
//                 { "NeptuneGrpcToken", ClientConfig.NeptuneGrpcToken },
//                 {
//                     "Authorization",
//                     "bearer " + token
//                 }
//             };
//                 return await neptuneGrpcServiceClient.InvokeAsync(method, headers, DateTime.UtcNow.AddSeconds(pInvokeTimeoutInSeconds));
//             }
//             catch (Exception)
//             {
//                 throw;
//             }
//         }

//         private void __TrackGrpcOnNeptune(MethodSpec pCalledMethod, MethodResult pResult)
//         {
//             string text = ServiceInfo.log_write_grpc_log.ToUpper();
//             if (text.Equals("Y") || text.Equals("YES"))
//             {
//                 MethodSpec methodSpec = new MethodSpec();
//                 methodSpec.FullClassName = "JITS.Neptune.Lib.GrpcLib.NeptuneServices";
//                 methodSpec.MethodName = "TrackGrpc";
//                 methodSpec.Parameters.Add(ClientConfig.YourServiceID);
//                 if (string.IsNullOrEmpty(ToServiceCode))
//                 {
//                     methodSpec.Parameters.Add("Neptune");
//                 }
//                 else
//                 {
//                     methodSpec.Parameters.Add(ToServiceCode);
//                 }
//                 methodSpec.Parameters.Add(JsonSerializer.Serialize(pCalledMethod));
//                 methodSpec.Parameters.Add(JsonSerializer.Serialize(pResult));
//                 InvokeNeptuneGrpcAsync(methodSpec);
//             }
//         }

//         public WorkflowExecutionInquiry InquireWorkflowExecution(string pExecutionID)
//         {
//             MethodSpec methodSpec = new MethodSpec();
//             methodSpec.FullClassName = "JITS.Neptune.Lib.GrpcLib.WorkflowExecutionInquiry";
//             methodSpec.MethodName = "GetExecutionInfo";
//             methodSpec.Parameters.Add(pExecutionID);
//             Task<MethodResult> task = InvokeNeptuneGrpcAsync(methodSpec);
//             task.Wait();
//             MethodResult result = task.Result;
//             if (!result.HasException)
//             {
//                 return JsonSerializer.Deserialize<WorkflowExecutionInquiry>(result.ReturnValue);
//             }
//             return null;
//         }

//         public WorkflowResponse<object> ExecuteWorkflow(string pToken, string pMessageContent)
//         {
//             MethodSpec methodSpec = new MethodSpec
//             {
//                 FullClassName = "JITS.Neptune.Lib.GrpcLib.WorkflowExecution",
//                 MethodName = "ExecuteWorkflow"
//             };
//             methodSpec.Parameters.Add(pToken);
//             methodSpec.Parameters.Add(pMessageContent);
//             Task<MethodResult> task = InvokeNeptuneGrpcAsync(methodSpec);
//             task.Wait();
//             MethodResult result = task.Result;
//             if (!result.HasException)
//             {
//                 return JsonSerializer.Deserialize<WorkflowResponse<object>>(result.ReturnValue);
//             }
//             return null;
//         }

//         public string ReloadGlobalCache(string pNeptuneInstanceID)
//         {
//             MethodSpec methodSpec = new MethodSpec();
//             methodSpec.FullClassName = "JITS.Neptune.Lib.GrpcLib.NeptuneServices";
//             methodSpec.MethodName = "ReloadGlobalCache";
//             methodSpec.Parameters.Add(pNeptuneInstanceID);
//             Task<MethodResult> task = InvokeNeptuneGrpcAsync(methodSpec);
//             task.Wait();
//             MethodResult result = task.Result;
//             if (!result.HasException)
//             {
//                 return result.ReturnValue;
//             }
//             return null;
//         }

//         public void RaiseServiceToServiceEvent(object pObjectData, string pFromServiceCode, string pToServiceCode)
//         {
//             if (pObjectData == null)
//             {
//                 throw new NullReferenceException();
//             }
//             RaiseServiceToServiceEvents(JsonSerializer.Serialize(pObjectData), pFromServiceCode, new List<string> { pToServiceCode });
//         }

//         public void RaiseServiceToServiceEvent(string pTextData, string pFromServiceCode, string pToServiceCode)
//         {
//             RaiseServiceToServiceEvents(pTextData, pFromServiceCode, new List<string> { pToServiceCode });
//         }

//         public void RaiseServiceToServiceEvents(object pObjectData, string pFromServiceCode, List<string> pToServiceCodeList)
//         {
//             if (pObjectData == null)
//             {
//                 throw new NullReferenceException();
//             }
//             RaiseServiceToServiceEvents(JsonSerializer.Serialize(pObjectData), pFromServiceCode, pToServiceCodeList);
//         }

//         public void RaiseServiceToServiceEvents(string pTextData, string pFromServiceCode, List<string> pToServiceCodeList)
//         {
//             List<NeptuneEvent<ServiceToServiceEventData>> list = new List<NeptuneEvent<ServiceToServiceEventData>>();
//             foreach (string pToServiceCode in pToServiceCodeList)
//             {
//                 NeptuneEvent<ServiceToServiceEventData> neptuneEvent = new NeptuneEvent<ServiceToServiceEventData>(NeptuneWorkflowEventTypeEnum.EventServiceToService);
//                 neptuneEvent.EventData.data = new ServiceToServiceEventData();
//                 neptuneEvent.EventData.data.from_service_code = pFromServiceCode;
//                 neptuneEvent.EventData.data.to_service_code = pToServiceCode;
//                 neptuneEvent.EventData.data.text_data = pTextData;
//                 list.Add(neptuneEvent);
//             }
//             RaiseServiceToServiceEvents(list);
//         }

//         private void RaiseServiceToServiceEvent(NeptuneEvent<ServiceToServiceEventData> pEventData)
//         {
//             if (pEventData != null)
//             {
//                 List<NeptuneEvent<ServiceToServiceEventData>> list = new List<NeptuneEvent<ServiceToServiceEventData>>();
//                 list.Add(pEventData);
//                 RaiseServiceToServiceEvents(list);
//             }
//         }

//         private void RaiseServiceToServiceEvents(List<NeptuneEvent<ServiceToServiceEventData>> pEventData)
//         {
//             if (pEventData == null || pEventData.Count == 0)
//             {
//                 return;
//             }
//             try
//             {
//                 pEventData.ForEach(delegate (NeptuneEvent<ServiceToServiceEventData> i)
//                 {
//                     i.EventType = NeptuneWorkflowEventTypeEnum.EventServiceToService;
//                 });
//                 string item = JsonSerializer.Serialize(pEventData);
//                 MethodSpec methodSpec = new MethodSpec
//                 {
//                     FullClassName = "JITS.Neptune.Lib.GrpcLib.NeptuneServices",
//                     MethodName = "RaiseServiceToServiceEvents"
//                 };
//                 methodSpec.Parameters.Add(item);
//                 InvokeAsync(methodSpec).Wait();
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine("ERROR: " + ex.Message);
//             }
//         }

//         public void RaiseServiceToServiceEventsByServiceInstanceID(string pServiceInstanceID, List<NeptuneEvent<ServiceToServiceEventData>> pEventData)
//         {
//             if (pEventData == null || pEventData.Count == 0)
//             {
//                 return;
//             }
//             try
//             {
//                 pEventData.ForEach(delegate (NeptuneEvent<ServiceToServiceEventData> i)
//                 {
//                     i.EventType = NeptuneWorkflowEventTypeEnum.EventServiceToService;
//                 });
//                 string item = JsonSerializer.Serialize(pEventData);
//                 MethodSpec methodSpec = new MethodSpec
//                 {
//                     FullClassName = "JITS.Neptune.Lib.GrpcLib.NeptuneServices",
//                     MethodName = "RaiseServiceToServiceEventsByServiceInstanceID"
//                 };
//                 methodSpec.Parameters.Add(pServiceInstanceID);
//                 methodSpec.Parameters.Add(item);
//                 InvokeAsync(methodSpec).Wait();
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine("ERROR: " + ex.Message);
//             }
//         }

//         public void RaiseServiceToServiceEventByServiceInstanceID(string pServiceInstanceID, NeptuneEvent<ServiceToServiceEventData> pEventData)
//         {
//             RaiseServiceToServiceEventsByServiceInstanceID(pServiceInstanceID, new List<NeptuneEvent<ServiceToServiceEventData>> { pEventData });
//         }

//         public void SendCentralizedLog(CentralizedLog pCentralizedLogs)
//         {
//             SendCentralizedLogs(new List<CentralizedLog> { pCentralizedLogs });
//         }

//         public void SendCentralizedLogs(List<CentralizedLog> pCentralizedLogs)
//         {
//             NeptuneEvent<ServiceToServiceEventData> neptuneEvent = new NeptuneEvent<ServiceToServiceEventData>();
//             neptuneEvent.EventData.data = new ServiceToServiceEventData();
//             neptuneEvent.EventData.data.event_type = typeof(CentralizedLog).FullName;
//             neptuneEvent.EventData.data.text_data = JsonSerializer.Serialize(pCentralizedLogs);
//             neptuneEvent.EventData.data.from_service_code = __ServiceInfo.service_code;
//             neptuneEvent.EventData.data.to_service_code = "CMS";
//             RaiseServiceToServiceEvent(neptuneEvent);
//         }

//         void IDisposable.Dispose()
//         {
//         }
//     }
// }