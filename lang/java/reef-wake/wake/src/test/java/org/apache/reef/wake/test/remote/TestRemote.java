/*
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
package org.apache.reef.wake.test.remote;

import org.apache.reef.tang.Tang;
import org.apache.reef.tang.exceptions.InjectionException;
import org.apache.reef.wake.EventHandler;
import org.apache.reef.wake.impl.LoggingEventHandler;
import org.apache.reef.wake.remote.*;
import org.apache.reef.wake.remote.address.LocalAddressProvider;
import org.apache.reef.wake.remote.impl.DefaultRemoteIdentifierFactoryImplementation;

import javax.inject.Inject;
import java.net.UnknownHostException;

public class TestRemote implements Runnable {
  private final RemoteManagerFactory remoteManagerFactory;
  private final LocalAddressProvider localAddressProvider;

  @Inject
  public TestRemote(final LocalAddressProvider localAddressProvider,
                    final RemoteManagerFactory remoteManagerFactory) {
    this.localAddressProvider = localAddressProvider;
    this.remoteManagerFactory = remoteManagerFactory;
  }

  @Override
  public void run() {
    final String hostAddress = localAddressProvider.getLocalAddress();
    int myPort = 10011;
    int remotePort = 10001;
    Codec<TestEvent> codec = new TestEventCodec();
    try (RemoteManager rm = remoteManagerFactory.getInstance("name", myPort, codec, new LoggingEventHandler<Throwable>())) {
      // proxy handler
      RemoteIdentifierFactory factory = new DefaultRemoteIdentifierFactoryImplementation();
      RemoteIdentifier remoteId = factory.getNewInstance("socket://" + hostAddress + ":" + remotePort);
      EventHandler<TestEvent> proxyHandler = rm.getHandler(remoteId, TestEvent.class);

      proxyHandler.onNext(new TestEvent("hello", 1.0));
      // register a handler
      rm.registerHandler(TestEvent.class, new TestEventHandler(proxyHandler));

    } catch (UnknownHostException e) {
      e.printStackTrace();
    } catch (Exception e) {
      e.printStackTrace();
    }
  }

  public static void main(String[] args) throws InjectionException {
    Tang.Factory.getTang().newInjector().getInstance(TestRemote.class).run();
  }
}

class TestEventHandler implements EventHandler<RemoteMessage<TestEvent>> {

  private final EventHandler<TestEvent> proxy;

  public TestEventHandler(EventHandler<TestEvent> proxy) {
    this.proxy = proxy;
  }

  @Override
  public void onNext(RemoteMessage<TestEvent> value) {
    System.out.println(value.getMessage().getMessage() + " " + value.getMessage().getLoad());
    proxy.onNext(value.getMessage());
  }
}
