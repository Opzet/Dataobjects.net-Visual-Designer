using System;
using System.Collections.Generic;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    public sealed class ErrorCollection: System.Collections.ObjectModel.ReadOnlyCollection<Exception>
    {
        public ErrorCollection(IList<Exception> list): base(list) {}
    }
}