using System;
using Microsoft.VisualStudio.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Common.Modeling
{
    public class ElementEventArgs: EventArgs
    {
        public ElementEventType EventType { get; private set; }
        public ElementPropertyChangedEventArgs ElementPropertyChangedEventArgs { get; private set; }
        public ElementAddedEventArgs ElementAddedEventArgs { get; private set; }
        public object[] CustomEventArgs { get; set; }

        public ElementEventArgs(ElementPropertyChangedEventArgs args)
        {
            EventType = ElementEventType.ElementPropertyChanged;
            ElementPropertyChangedEventArgs = args;
        }

        public ElementEventArgs(ElementAddedEventArgs args)
        {
            EventType = ElementEventType.ElementAdded;
            ElementAddedEventArgs = args;
        }

        public ElementEventArgs(params object[] args)
        {
            EventType = ElementEventType.CustomEvent;
            CustomEventArgs = args;
        }

        public ElementEventArgs(ElementEventType eventType)
        {
            EventType = eventType;
        }
    }
}