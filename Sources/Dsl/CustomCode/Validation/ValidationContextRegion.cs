using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal static class ValidationContextRegion
    {
        private static readonly List<Guid> elementsWithDisabledValidation = new List<Guid>();
        private static readonly object sync = new object();
        private static int disableAllCounter = 0;

        #region class DisableRegion

        private class DisableRegion: IDisposable
        {
            private readonly IEnumerable<Guid> elements;
            private readonly bool isDisableForall = false;

            internal DisableRegion(IEnumerable<Guid> elements)
            {
                this.elements = elements;
            }

            internal DisableRegion(bool isDisableForall)
            {
                this.isDisableForall = isDisableForall;
            }

            public void Dispose()
            {
                lock(sync)
                {
                    if (isDisableForall)
                    {
                        if (disableAllCounter == 0)
                        {
                            throw new InvalidOperationException(
                                "There are not disable for all regions which has to be disposed.");
                        }
                        disableAllCounter--;
                    }
                    else
                    {
                        elementsWithDisabledValidation.RemoveAll(element => elements.Contains(element));
                    }
                }
            }
        }

        #endregion class DisableRegion

        internal static IDisposable DisableFor(params ModelElement[] elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }

            IEnumerable<Guid> modelElements;
            lock (sync)
            {
                modelElements = elements.Select(item => item.Id).Except(elementsWithDisabledValidation).ToArray();
                elementsWithDisabledValidation.AddRange(modelElements);
            }

            return new DisableRegion(modelElements);
        }

        internal static IDisposable DisableAll()
        {
            lock(sync)
            {
                disableAllCounter++;
            }

            return new DisableRegion(true);
        }

        internal static bool IsValidationDisabled<T>(this T modelElement) where T: ModelElement
        {
            lock(sync)
            {
                return disableAllCounter > 0 || elementsWithDisabledValidation.Contains(modelElement.Id);
            }
        }
    }
}