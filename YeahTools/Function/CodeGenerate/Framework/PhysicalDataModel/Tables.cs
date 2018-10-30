﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeBuilder.PhysicalDataModel
{
    public class Tables : Dictionary<string, Table>
    {
        public Tables()
            : base()
        {
        }

        public Tables(int capacity)
            : base(capacity)
        {
        }
    }
}
