using System;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal class ValidationGlobalStageImpl
    {
        public ValidationGlobalStage Current { get; private set; }

        public ValidationGlobalStageImpl() { }

        public ValidationGlobalStageImpl(ValidationGlobalStage current)
        {
            Current = current;
        }
    }

    [Flags]
    public enum ValidationGlobalStage
    {
        Unknown = 0,
        EntityModelInheritanceTree = 1
    }
}