﻿/**
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

using System;
using System.CodeDom;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Org.Apache.REEF.Common;
using Org.Apache.REEF.Common.Context;
using Org.Apache.REEF.Common.Files;
using Org.Apache.REEF.Driver.Bridge.Clr2java;
using Org.Apache.REEF.Driver.Bridge.Events;
using Org.Apache.REEF.Driver.Context;
using Org.Apache.REEF.Driver.Evaluator;
using Org.Apache.REEF.Driver.Task;
using Org.Apache.REEF.Utilities.Diagnostics;
using Org.Apache.REEF.Utilities.Logging;
using Org.Apache.REEF.Wake.Time.Event;
using ContextMessage = Org.Apache.REEF.Driver.Bridge.Events.ContextMessage;

namespace Org.Apache.REEF.Driver.Bridge
{
    public class ClrSystemHandlerWrapper
    {
        private static readonly Logger LOGGER = Logger.GetLogger(typeof(ClrSystemHandlerWrapper));

        private static DriverBridge _driverBridge;

        public static void Call_ClrSystemAllocatedEvaluatorHandler_OnNext(ulong handle, IAllocatedEvaluaotrClr2Java clr2Java)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemAllocatedEvaluatorHandler_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<IAllocatedEvaluator> obj = (ClrSystemHandler<IAllocatedEvaluator>)gc.Target;
                obj.OnNext(new AllocatedEvaluator(clr2Java, _driverBridge.ConfigurationProviders));
            }       
        }

        public static void Call_ClrSystemActiveContextHandler_OnNext(ulong handle, IActiveContextClr2Java clr2Java)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemActiveContextHandler_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<IActiveContext> obj = (ClrSystemHandler<IActiveContext>)gc.Target;
                obj.OnNext(new ActiveContext(clr2Java));
            }            
        }

        public static void Call_ClrSystemDriverRestartActiveContextHandler_OnNext(ulong handle, IActiveContextClr2Java clr2Java)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemDriverRestartActiveContextHandler_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<IActiveContext> obj = (ClrSystemHandler<IActiveContext>)gc.Target;
                obj.OnNext(new ActiveContext(clr2Java));
            }
        }

        public static void Call_ClrSystemEvaluatorRequestor_OnNext(ulong handle, IEvaluatorRequestorClr2Java clr2Java)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemEvaluatorRequestor_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<IEvaluatorRequestor> obj = (ClrSystemHandler<IEvaluatorRequestor>)gc.Target;
                obj.OnNext(new EvaluatorRequestor(clr2Java));
            }   
        }

        public static void Call_ClrSystemTaskMessage_OnNext(ulong handle, ITaskMessageClr2Java clr2Java, byte[] message)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemTaskMessage_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<ITaskMessage> obj = (ClrSystemHandler<ITaskMessage>)gc.Target;
                obj.OnNext(new TaskMessage(clr2Java, message));
            }   
        }

        public static void Call_ClrSystemFailedTask_OnNext(ulong handle, IFailedTaskClr2Java clr2Java)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemFailedTask_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<IFailedTask> obj = (ClrSystemHandler<IFailedTask>)gc.Target;
                obj.OnNext(new FailedTask(clr2Java));
            }
        }

        public static void Call_ClrSystemRunningTask_OnNext(ulong handle, IRunningTaskClr2Java clr2Java)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemRunningTask_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<IRunningTask> obj = (ClrSystemHandler<IRunningTask>)gc.Target;
                obj.OnNext(new RunningTask(clr2Java));
            }        
        }

        public static void Call_ClrSystemDriverRestartRunningTask_OnNext(ulong handle, IRunningTaskClr2Java clr2Java)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemDriverRestartRunningTask_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<IRunningTask> obj = (ClrSystemHandler<IRunningTask>)gc.Target;
                obj.OnNext(new RunningTask(clr2Java));
            }
        }

        public static void Call_ClrSystemFailedEvaluator_OnNext(ulong handle, IFailedEvaluatorClr2Java clr2Java)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemFailedEvaluator_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<IFailedEvaluator> obj = (ClrSystemHandler<IFailedEvaluator>)gc.Target;
                obj.OnNext(new FailedEvaluator(clr2Java));
            }     
        }

        public static void Call_ClrSystemCompletedTask_OnNext(ulong handle, ICompletedTaskClr2Java clr2Java)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemCompletedTask_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<ICompletedTask> obj = (ClrSystemHandler<ICompletedTask>)gc.Target;
                obj.OnNext(new CompletedTask(clr2Java));
            }         
        }

        public static void Call_ClrSystemSuspendedTask_OnNext(ulong handle, ISuspendedTaskClr2Java clr2Java)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemSuspendedTask_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<ISuspendedTask> obj = (ClrSystemHandler<ISuspendedTask>)gc.Target;
                obj.OnNext(new SuspendedTask(clr2Java));
            }
        }

        public static void Call_ClrSystemCompletedEvaluator_OnNext(ulong handle, ICompletedEvaluatorClr2Java clr2Java)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemCompletedEvaluator_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<ICompletedEvaluator> obj = (ClrSystemHandler<ICompletedEvaluator>)gc.Target;
                obj.OnNext(new CompletedEvaluator(clr2Java));
            }
        }

        public static void Call_ClrSystemHttpServer_OnNext(ulong handle, IHttpServerBridgeClr2Java clr2Java)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemHttpServer_OnNext"))
            {
                try
                {
                    GCHandle gc = GCHandle.FromIntPtr((IntPtr) handle);
                    if (!gc.IsAllocated)
                    {
                        LOGGER.Log(Level.Warning, "gc is not allocated.");
                    } 
                    ClrSystemHandler<IHttpMessage> obj = (ClrSystemHandler<IHttpMessage>) gc.Target;
                    obj.OnNext(new HttpMessage(clr2Java));
                }
                catch (Exception ex)
                {
                  
                    LOGGER.Log(Level.Info, "Caught exception: " + ex.Message + ex.StackTrace );
                    Exceptions.CaughtAndThrow(ex, Level.Warning,  LOGGER);
                }}
        }

        public static void Call_ClrSystemClosedContext_OnNext(ulong handle, IClosedContextClr2Java clr2Java)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemClosedContext_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<IClosedContext> obj = (ClrSystemHandler<IClosedContext>)gc.Target;
                obj.OnNext(new ClosedContext(clr2Java));
            }
        }

        public static void Call_ClrSystemFailedContext_OnNext(ulong handle, IFailedContextClr2Java clr2Java)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemFailedContext_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<IFailedContext> obj = (ClrSystemHandler<IFailedContext>)gc.Target;
                obj.OnNext(new FailedContext(clr2Java));
            }
        }

        public static void Call_ClrSystemContextMessage_OnNext(ulong handle, IContextMessageClr2Java clr2Java)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemContextMessage_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<IContextMessage> obj = (ClrSystemHandler<IContextMessage>)gc.Target;
                obj.OnNext(new ContextMessage(clr2Java));
            }
        }

        public static void Call_ClrSystemDriverRestart_OnNext(ulong handle)
        {
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemDriverRestart_OnNext"))
            {
                GCHandle gc = GCHandle.FromIntPtr((IntPtr)handle);
                ClrSystemHandler<StartTime> obj = (ClrSystemHandler<StartTime>)gc.Target;
                obj.OnNext(new StartTime(DateTime.Now.Ticks));
            }
        }

        //Deprecate, remove after both Java and C# code gets checked in
        public static ulong[] Call_ClrSystemStartHandler_OnStart(
            DateTime startTime,
            IEvaluatorRequestorClr2Java  evaluatorRequestorClr2Java)
        {
            IEvaluatorRequestor evaluatorRequestor = new EvaluatorRequestor(evaluatorRequestorClr2Java);
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemStartHandler_OnStart"))
            {
                LOGGER.Log(Level.Info, "*** Start time is " + startTime);
                return GetHandlers(null, evaluatorRequestor);
            }
        }

        public static ulong[] Call_ClrSystemStartHandler_OnStart(
            DateTime startTime, 
            string httpServerPort,
            IEvaluatorRequestorClr2Java evaluatorRequestorClr2Java)
        {
            IEvaluatorRequestor evaluatorRequestor = new EvaluatorRequestor(evaluatorRequestorClr2Java);
            using (LOGGER.LogFunction("ClrSystemHandlerWrapper::Call_ClrSystemStartHandler_OnStart"))
            {
                LOGGER.Log(Level.Info, "*** Start time is " + startTime);
                LOGGER.Log(Level.Info, "*** httpServerPort: " + httpServerPort);
                var handlers = GetHandlers(httpServerPort, evaluatorRequestor);
                _driverBridge.StartHandlersOnNext(startTime);
                _driverBridge.ObsoleteEvaluatorRequestorOnNext(evaluatorRequestor);

                return handlers;
            }   
        }

        private static ulong[] GetHandlers(string httpServerPortNumber, IEvaluatorRequestor evaluatorRequestor)
        {
            var injector = BridgeConfigurationProvider.GetBridgeInjector(evaluatorRequestor);

            try
            {
                var port = injector.GetInstance<HttpServerPort>();
                port.PortNumber = httpServerPortNumber == null
                    ? 0
                    : int.Parse(httpServerPortNumber, CultureInfo.InvariantCulture);

                //TODO: Remove next 2 lines after Obsolete period
                var startHandler = injector.GetInstance<IStartHandler>();
                LOGGER.Log(Level.Info, "Start handler set to be " + startHandler.Identifier);
                _driverBridge = injector.GetInstance<DriverBridge>();
            }
            catch (Exception e)
            {
                Exceptions.CaughtAndThrow(e, Level.Error, "Cannot get instance.", LOGGER);
            }

            return _driverBridge.Subscribe();
        }
    }
}