using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreDemoTest.Helpers
{
    public enum ReturnMethod
    {
        NoReturnType = 0,
        NoReturnDate,
        NoReturnQuantity,
        NoReturnPaidAmount,
        AnyReturn,
        VoucherOnly
    }
}
