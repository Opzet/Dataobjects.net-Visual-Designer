using System;
using System.Collections.Generic;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public class PropertyOwnerContext
    {
        private static PropertyOwnerContext current;

        #region class Impersonation

        private class Impersonation: IDisposable
        {
            internal IEnumerable<IPropertyBase> properties;
            internal IPersistentType owner;

            internal Impersonation(IPersistentType owner, IEnumerable<IPropertyBase> properties)
            {
                this.properties = properties;
                this.owner = owner;
            }

            public void Dispose()
            {
                lock (Current.sync)
                {
                    foreach (IPropertyBase property in properties)
                    {
                        if (Current.impersonatedOwners.ContainsKey(property) && Current.impersonatedOwners[property] == this.owner)
                        {
                            Current.impersonatedOwners.Remove(property);
                        }
                    }
                }
            }
        }

        #endregion class Impersonation

        public static PropertyOwnerContext Current
        {
            get
            {
                if (current == null)
                {
                    throw new ApplicationException("PropertyOwnerContext is not initialized!");
                }

                return current;
            }
        }

        public static void Initialize()
        {
            current = new PropertyOwnerContext();
        }

        private readonly object sync = new object();
        private readonly Dictionary<IPropertyBase, IPersistentType> impersonatedOwners = new Dictionary<IPropertyBase, IPersistentType>();

        public IDisposable ImpersonateAsOwner(IPersistentType owner, IEnumerable<IPropertyBase> properties) 
        {
            lock (sync)
            {
                foreach (IPropertyBase property in properties)
                {
                    if (impersonatedOwners.ContainsKey(property))
                    {
                        throw new ApplicationException(string.Format(
                            "Property '{0}' has already impersonated an owner!", property.Name));
                    }
                    
                    impersonatedOwners.Add(property, owner);
                }
            }

            Impersonation impersonation = new Impersonation(owner, properties);
            return impersonation;
        }

        private IPersistentType InternalGetImpersonatedOwner(IPropertyBase property)
        {
            lock (sync)
            {
                if (impersonatedOwners.ContainsKey(property))
                {
                    return impersonatedOwners[property];
                }
            }

            return null;
        }

        internal static IPersistentType GetImpersonatedOwner(IPropertyBase property)
        {
            return current == null ? null : Current.InternalGetImpersonatedOwner(property);
        }
    }
}