using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Common
{
    public class JwtVerificationAttribute:Attribute
    {
        public bool NeedValidate;
        public JwtVerificationAttribute(bool NeedValidate = true)
        {
            this.NeedValidate = NeedValidate;
        }
    }
}
