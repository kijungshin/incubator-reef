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
using System.Collections.Generic;
using Org.Apache.REEF.Common.Tasks;
using Org.Apache.REEF.Network.Group.Config;
using Org.Apache.REEF.Network.Group.Driver.Impl;
using Org.Apache.REEF.Network.NetworkService;
using Org.Apache.REEF.Tang.Annotations;
using Org.Apache.REEF.Tang.Formats;
using Org.Apache.REEF.Tang.Interface;
using Org.Apache.REEF.Wake.Remote.Impl;

namespace Org.Apache.REEF.Network.Group.Task.Impl
{
    /// <summary>
    /// Container of ommunicationGroupClients
    /// </summary>
    public class GroupCommClient : IGroupCommClient
    {
        private readonly Dictionary<string, ICommunicationGroupClient> _commGroups;

        private readonly INetworkService<GroupCommunicationMessage> _networkService;

        /// <summary>
        /// Creates a new GroupCommClient and registers the task ID with the Name Server.
        /// Currently the GroupCommClient is injected in task constructor. When work with REEF-289, we should put the injection at a proepr palce. 
        /// </summary>
        /// <param name="groupConfigs">The set of serialized Group Communication configurations</param>
        /// <param name="taskId">The identifier for this taskfor this task</param>
        /// <param name="networkService">The network service used to send messages</param>
        /// <param name="configSerializer">Used to deserialize Group Communication configuration</param>
        /// <param name="injector">injector forked from the injector that creates this instance</param>
        [Inject]
        private GroupCommClient(
            [Parameter(typeof(GroupCommConfigurationOptions.SerializedGroupConfigs))] ISet<string> groupConfigs,
            [Parameter(typeof(TaskConfigurationOptions.Identifier))] string taskId,
            NetworkService<GroupCommunicationMessage> networkService,
            AvroConfigurationSerializer configSerializer,
            IInjector injector)
        {
            _commGroups = new Dictionary<string, ICommunicationGroupClient>();
            _networkService = networkService;
            networkService.Register(new StringIdentifier(taskId));

            foreach (string serializedGroupConfig in groupConfigs)
            {
                IConfiguration groupConfig = configSerializer.FromString(serializedGroupConfig);
                IInjector groupInjector = injector.ForkInjector(groupConfig);
                ICommunicationGroupClient commGroupClient = groupInjector.GetInstance<ICommunicationGroupClient>();
                _commGroups[commGroupClient.GroupName] = commGroupClient;
            }
        }

        /// <summary>
        /// Gets the CommunicationGroupClient for the given group name.
        /// </summary>
        /// <param name="groupName">The name of the CommunicationGroupClient</param>
        /// <returns>The CommunicationGroupClient</returns>
        public ICommunicationGroupClient GetCommunicationGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentNullException("groupName");
            }
            if (!_commGroups.ContainsKey(groupName))
            {
                throw new ArgumentException("No CommunicationGroupClient with name: " + groupName);
            }

            return _commGroups[groupName];
        }

        /// <summary>
        /// Disposes of the GroupCommClient's services.
        /// </summary>
        public void Dispose()
        {
            _networkService.Unregister();
            _networkService.Dispose();
        }
    }
}
