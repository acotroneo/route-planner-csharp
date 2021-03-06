﻿/*
 | Version 10.1.84
 | Copyright 2013 Esri
 |
 | Licensed under the Apache License, Version 2.0 (the "License");
 | you may not use this file except in compliance with the License.
 | You may obtain a copy of the License at
 |
 |    http://www.apache.org/licenses/LICENSE-2.0
 |
 | Unless required by applicable law or agreed to in writing, software
 | distributed under the License is distributed on an "AS IS" BASIS,
 | WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 | See the License for the specific language governing permissions and
 | limitations under the License.
 */

using System;
using System.Diagnostics;

namespace ESRI.ArcLogistics.Routing
{
    /// <summary>
    /// Implements the <see cref="T:ESRI.ArcLogistics.Routing.IAsyncSolveTask"/> by running the
    /// provided delegate.
    /// </summary>
    internal sealed class DelegateAsyncSolveTask : IAsyncSolveTask
    {
        #region constructors
        /// <summary>
        /// Initializes a new instance of the DelegateAsyncSolveTask class.
        /// </summary>
        /// <param name="taskFunction">The function to be executed by the task.</param>
        public DelegateAsyncSolveTask(Func<ICancelTracker, Func<SolveTaskResult>> taskFunction)
        {
            Debug.Assert(taskFunction != null);

            _taskFunction = taskFunction;
        }
        #endregion

        #region IAsyncSolveTask Members
        /// <summary>
        /// Runs function passed to the class constructor.
        /// </summary>
        /// <param name="cancellationTracker">Cancellation tracker to be used for cancelling running
        /// solve operation.</param>
        /// <returns>A function returning asynchronous solve task result.</returns>
        public Func<SolveTaskResult> Run(ICancelTracker cancellationTracker)
        {
            return _taskFunction(cancellationTracker);
        }
        #endregion

        #region private fields
        /// <summary>
        /// Stores function to be executed by the task.
        /// </summary>
        private Func<ICancelTracker, Func<SolveTaskResult>> _taskFunction;
        #endregion
    }
}
