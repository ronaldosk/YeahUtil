using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeBuilder.Exceptions
{
    using YeahTools.Properties;

    public class NotFoundPdmDBMSException : Exception
    {
        public NotFoundPdmDBMSException()
            : this(Resources.NotFoundPdmDBMSExceptionMessage)
        {
        }

        public NotFoundPdmDBMSException(string message)
            : base(message)
        {
        }
    }
}
