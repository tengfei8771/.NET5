using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Common
{
    public class AllowAnyJwtVerificationAttribute:Attribute
    {
        public bool NeedNotValidate = true;
        public AllowAnyJwtVerificationAttribute(bool NeedNotValidate=true)
        {
            this.NeedNotValidate = NeedNotValidate;
        }
    }
}
