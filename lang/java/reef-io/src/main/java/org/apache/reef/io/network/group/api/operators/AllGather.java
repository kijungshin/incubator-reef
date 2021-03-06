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
package org.apache.reef.io.network.group.api.operators;

import org.apache.reef.exception.evaluator.NetworkException;
import org.apache.reef.wake.Identifier;

import java.util.List;

/**
 * MPI AllGather Operator.
 * <p/>
 * Each task applies this operator on an element of type T. The result will be
 * a list of elements constructed using the elements all-gathered at each
 * task.
 */
public interface AllGather<T> extends GroupCommOperator {

  /**
   * Apply the operation on element.
   *
   * @return List of all elements on which the operation was applied using default order
   */
  List<T> apply(T element) throws NetworkException,
      InterruptedException;

  /**
   * Apply the operation on element.
   *
   * @return List of all elements on which the operation was applied using order specified
   */
  List<T> apply(T element, List<? extends Identifier> order)
      throws NetworkException, InterruptedException;
}
