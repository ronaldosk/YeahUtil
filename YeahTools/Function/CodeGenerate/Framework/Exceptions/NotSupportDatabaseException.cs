using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeBuilder.Exceptions
{
    using YeahTools.Properties;

    public class NotSupportDatabaseException : Exception
    {
        public NotSupportDatabaseException()
            : this(Resources.NotSupportDatabaseExceptionMessage)
        {
        }

        public NotSupportDatabaseException(string message)
            : base(message)
        {
        }
    }
}
