// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Globalization;

namespace System.ServiceModel.Channels
{
    internal class UriGenerator
    {
        private long _id;
        private string _prefix;

        public UriGenerator()
            : this("uuid")
        {
        }

        public UriGenerator(string scheme)
            : this(scheme, ";")
        {
        }

        public UriGenerator(string scheme, string delimiter)
        {
            if (scheme == null)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("scheme"));

            if (scheme.Length == 0)
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.UriGeneratorSchemeMustNotBeEmpty, "scheme"));

            _prefix = string.Concat(scheme, ":", Guid.NewGuid().ToString(), delimiter, "id=");
        }

        public string Next()
        {
            long nextId = Interlocked.Increment(ref _id);
            return _prefix + nextId.ToString(CultureInfo.InvariantCulture);
        }
    }
}
