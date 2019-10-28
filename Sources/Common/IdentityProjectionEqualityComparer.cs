using System;
using System.Collections.Generic;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    // Original code was copied from: http://codepaste.net/5x7x6w

    /// <summary>
    /// Implements a projection strategy that select an "identity" property from an object 
    /// and uses the default comparer to implement IEqualityComparer<typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Type of object to extract identity from</typeparam>
    /// <typeparam name="TRet">Type of the extracted identity value</typeparam>
    public class IdentityProjectionEqualityComparer<T, TRet> : IEqualityComparer<T> where TRet : IComparable<TRet>
    {
        private readonly Func<T, TRet> _projector;
        private readonly IEqualityComparer<TRet> _comparer;
        private static readonly EqualityComparer<TRet> DefaultComparer = EqualityComparer<TRet>.Default;

        public IdentityProjectionEqualityComparer(Func<T, TRet> projector)
            : this(projector, DefaultComparer)
        {
        }

        public IdentityProjectionEqualityComparer(Func<T, TRet> projector, IEqualityComparer<TRet> comparer)
        {
            if (projector == null)
                throw new ArgumentNullException("projector");

            if (comparer == null)
                throw new ArgumentNullException("comparer");

            _projector = projector;
            _comparer = comparer;
        }

        public bool Equals(T x, T y)
        {
            return _comparer.Equals(_projector(x), _projector(y));
        }

        public int GetHashCode(T obj)
        {
            return _comparer.GetHashCode(_projector(obj));
        }
    }
}