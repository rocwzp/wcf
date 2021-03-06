// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime;
using System.Runtime.Diagnostics;
using System.Xml;

namespace System.ServiceModel.Diagnostics
{
    public class ExceptionUtility
    {
        private const string ExceptionStackAsStringKey = "System.ServiceModel.Diagnostics.ExceptionUtility.ExceptionStackAsString";

#pragma warning disable 649
        // This field should be only used for debug build.
        internal static ExceptionUtility mainInstance;
#pragma warning restore 649

        private ExceptionTrace _exceptionTrace;
        private string _name;
        private string _eventSourceName;

        [ThreadStatic]
        private static Guid s_activityId;

        [Obsolete("For SMDiagnostics.dll use only. Call DiagnosticUtility.ExceptionUtility instead")]
        internal ExceptionUtility(string name, string eventSourceName, object exceptionTrace)
        {
            _exceptionTrace = (ExceptionTrace)exceptionTrace;
            _name = name;
            _eventSourceName = eventSourceName;
        }


        public ArgumentException ThrowHelperArgument(string message)
        {
            return (ArgumentException)this.ThrowHelperError(new ArgumentException(message));
        }

        public ArgumentException ThrowHelperArgument(string paramName, string message)
        {
            return (ArgumentException)this.ThrowHelperError(new ArgumentException(message, paramName));
        }

        public ArgumentNullException ThrowHelperArgumentNull(string paramName)
        {
            return (ArgumentNullException)this.ThrowHelperError(new ArgumentNullException(paramName));
        }

        public ArgumentNullException ThrowHelperArgumentNull(string paramName, string message)
        {
            return (ArgumentNullException)this.ThrowHelperError(new ArgumentNullException(paramName, message));
        }

        public ArgumentException ThrowHelperArgumentNullOrEmptyString(string arg)
        {
            return (ArgumentException)this.ThrowHelperError(new ArgumentException(SR.StringNullOrEmpty, arg));
        }

        public Exception ThrowHelperFatal(string message, Exception innerException)
        {
            return this.ThrowHelperError(new FatalException(message, innerException));
        }

        public Exception ThrowHelperInternal(bool fatal)
        {
            return fatal ? Fx.AssertAndThrowFatal("Fatal InternalException should never be thrown.") : Fx.AssertAndThrow("InternalException should never be thrown.");
        }

        public Exception ThrowHelperInvalidOperation(string message)
        {
            return ThrowHelperError(new InvalidOperationException(message));
        }

        public Exception ThrowHelperCallback(string message, Exception innerException)
        {
            return this.ThrowHelperCritical(new CallbackException(message, innerException));
        }

        public Exception ThrowHelperCallback(Exception innerException)
        {
            return this.ThrowHelperCallback(SR.GenericCallbackException, innerException);
        }

        public Exception ThrowHelperCritical(Exception exception)
        {
            return exception;
        }

        public Exception ThrowHelperError(Exception exception)
        {
            return exception;
        }

        public Exception ThrowHelperWarning(Exception exception)
        {
            return exception;
        }

        internal Exception ThrowHelperXml(XmlReader reader, string message)
        {
            return this.ThrowHelperXml(reader, message, null);
        }

        internal Exception ThrowHelperXml(XmlReader reader, string message, Exception inner)
        {
            IXmlLineInfo lineInfo = reader as IXmlLineInfo;
            return this.ThrowHelperError(new XmlException(
                message,
                inner,
                (null != lineInfo) ? lineInfo.LineNumber : 0,
                (null != lineInfo) ? lineInfo.LinePosition : 0));
        }

        // On a single thread, these functions will complete just fine
        // and don't need to worry about locking issues because the effected
        // variables are ThreadStatic.
        internal static void UseActivityId(Guid activityId)
        {
            ExceptionUtility.s_activityId = activityId;
        }

        internal static void ClearActivityId()
        {
            ExceptionUtility.s_activityId = Guid.Empty;
        }
    }
}
