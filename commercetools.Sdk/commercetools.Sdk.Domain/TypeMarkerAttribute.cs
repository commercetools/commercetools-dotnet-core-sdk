﻿using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class TypeMarkerAttribute : System.Attribute
    {
        public string Value { get; protected set; }
    }
}