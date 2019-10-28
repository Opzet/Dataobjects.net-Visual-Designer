using System;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors
{
    /// <summary>
    /// Attribute which can be used to provide a good name for a field
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class FieldDisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
    {
        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldDisplayNameAttribute"/> class.
        /// </summary>
        /// <param name="displayName">the name which should be displayed</param>
        public FieldDisplayNameAttribute(string displayName)
            : base(displayName)
        {
        }

        #endregion

        #region Methods

        #endregion
    }
}