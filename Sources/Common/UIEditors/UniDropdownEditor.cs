using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Timer = System.Threading.Timer;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors
{
    public abstract class UniDropdownEditor : UITypeEditor, IListItemCollectionProvider
    {
        private IWindowsFormsEditorService editorService;
        private ListBox listbox;


        private void PrepareListBox(ListBox listbox, ITypeDescriptorContext context)
        {
            listbox.IntegralHeight = true;
            ListItemCollection collection = GetCollection(context, AllowedNull);

            if (listbox.ItemHeight > 0)
            {
                if ((collection != null) && (listbox.Height / listbox.ItemHeight < collection.Count))
                {
                    int adjHei = collection.Count * listbox.ItemHeight;
                    if (adjHei > 200)
                    {
                        adjHei = 200;
                    }
                    listbox.Height = adjHei;
                }
            }
            else
            {
                listbox.Height = 200;
            }

            listbox.Sorted = true;
            FillListBoxFromCollection(listbox, collection);
            this.AssignValueMember(listbox, context.PropertyDescriptor);
            this.AssignDisplayMember(listbox, context.PropertyDescriptor);
            listbox.SelectedIndexChanged += listbox_SelectedIndexChanged;
            listbox.MouseDoubleClick += Listbox_MouseDoubleClick;

        }



        private void FillListBoxFromCollection(ListBox lb, ICollection coll)
        {
            lb.BeginUpdate();
            lb.Items.Clear();
            foreach (object item in coll)
            {
                lb.Items.Add(item);
            }
            lb.EndUpdate();
            lb.Invalidate();
        }

        protected ListBox GetListbox(ITypeDescriptorContext context, object value)
        {
            if (listbox == null)
            {
                listbox = new ListBox();
                listbox.BorderStyle = BorderStyle.FixedSingle;
                this.PrepareListBox(listbox, context);
            }

            return listbox;
        }

        private void AssignValueMember(ListControl lc, PropertyDescriptor pd)
        {
            lc.ValueMember = "Value";
        }

        private void AssignDisplayMember(ListControl lc, PropertyDescriptor pd)
        {
            lc.DisplayMember = "Text";
        }

        private bool selectedIndexChangedDisabled = false;

        private void listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedIndexChangedDisabled)
            {
                return;
            }

            if (this.editorService == null)
            {
                return;
            }

            PropertyGrid currentGrid = this.editorService.ResolvePropertyGrid();
            this.editorService.CloseDropDown();
            RunExpandAllOnGrid(currentGrid);
        }

        private Timer timer;

        private void RunExpandAllOnGrid(PropertyGrid currentGrid)
        {
//            timer = new Timer(delegate(object state)
//                                        {
//                                            PropertyGrid grid = (PropertyGrid) state;
//
//                                            grid.Invoke(new Action<PropertyGrid>(delegate(PropertyGrid propertyGrid)
//                                                {
//                                                    GridItem selectedGridItem = propertyGrid.SelectedGridItem;
//                                                    if (selectedGridItem != null)
//                                                    {
//                                                        selectedGridItem.Expanded = true;
//                                                    }
//
//                                                }), grid);
//
//                                        }, currentGrid, 200, Timeout.Infinite);
        }

        private void Listbox_MouseDoubleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            ListBox lb = (sender as ListBox);
            int max = lb.Items.Count;
            int nextIdx = lb.SelectedIndex + 1;
            if (nextIdx >= max)
            {
                nextIdx = 0;
            }

            lb.SelectedIndex = nextIdx;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if ((context != null) && (context.Instance != null))
            {
                return UITypeEditorEditStyle.DropDown;
            }

            return UITypeEditorEditStyle.None;
        }

        [RefreshProperties(RefreshProperties.All)]
        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider, object value)
        {
            if ((context == null) || (provider == null) || (context.Instance == null))
            {
                return base.EditValue(provider, value);
            }

            this.editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (this.editorService == null)
            {
                return base.EditValue(provider, value);
            }


            this.ValidateValue(context, ref value);

            var standardValuesConverter = ResolveStandardValuesTypeConverter(value);

            ListBox listBox = GetListbox(context, value);
            this.SelectByValue(listbox, standardValuesConverter, value);
            this.editorService.DropDownControl(listBox);

            ListItem selectedListItem = listbox.SelectedItem as ListItem;
            if (standardValuesConverter != null)
            {
                value = standardValuesConverter.GetValueFromSelectedListItem(value, selectedListItem);
            }
            else
            {
                if (listbox.SelectedItem == null)
                {
                    value = null;
                }
                else
                {
                    value = selectedListItem.Value;
                }

            }

            return value;
        }

        protected virtual StandardValuesTypeConverterBase ResolveStandardValuesTypeConverter(object value)
        {
            TypeConverter typeConverter = value == null ? null : TypeDescriptor.GetConverter(value);
            StandardValuesTypeConverterBase standardValuesConverter = typeConverter as StandardValuesTypeConverterBase;
            return standardValuesConverter;
        }

        public void SelectByValue(ListBox lb, StandardValuesTypeConverterBase standardValuesConverter, object val)
        {
            selectedIndexChangedDisabled = true;
            try
            {
                lb.SelectedItem = null;
                foreach (ListItem item in lb.Items)
                {
                    if (standardValuesConverter != null)
                    {
                        if (standardValuesConverter.SelectListItemByValue(item, val))
                        {
                            lb.SelectedItem = item;
                            break;
                        }
                    }

                    if (val != null && val.Equals(item.Value))
                    {
                        lb.SelectedItem = item;
                        break;
                    }
                }
            }
            finally
            {
                selectedIndexChangedDisabled = false;
            }
        }

        public abstract ListItemCollection GetCollection(ITypeDescriptorContext context, bool allowedNull);
        public abstract void ValidateValue(ITypeDescriptorContext context, ref object value);

        public abstract bool AllowedNull { get; }
    }
}