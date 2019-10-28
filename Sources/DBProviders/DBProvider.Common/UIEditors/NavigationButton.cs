using System;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider.UIEditors
{
    public enum NavigationButtonType
    {
        Previous,
        Next
    }

    public class NavigationButton
    {
        private string text;
        private bool visible;
        private bool enabled;

        public NavigationButtonType ButtonType { get; private set; }

        public string Text
        {
            get { return this.text; }
            set
            {
                if (value != this.text)
                {
                    this.text = value;
                    FireOnUpdate();
                }
            }
        }

        public bool Visible
        {
            get { return this.visible; }
            set
            {
                if (value != this.visible)
                {
                    this.visible = value;
                    FireOnUpdate();
                }
            }
        }

        public bool Enabled
        {
            get { return this.enabled; }
            set
            {
                if (value != this.enabled)
                {
                    this.enabled = value;
                    FireOnUpdate();
                }
            }
        }

        internal Action<NavigationButton> OnUpdate;

        internal NavigationButton(string text, NavigationButtonType buttonType, Action<NavigationButton> onUpdate)
        {
            this.Text = text;
            this.ButtonType = buttonType;
            this.Visible = true;
            this.Enabled = true;
            this.OnUpdate = onUpdate;
        }

        private void FireOnUpdate()
        {
            if (OnUpdate != null)
            {
                OnUpdate(this);
            }
        }
    }
}