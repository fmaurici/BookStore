﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class AuthenticationToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
