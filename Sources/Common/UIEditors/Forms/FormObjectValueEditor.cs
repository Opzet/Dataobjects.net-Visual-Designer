using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TXSoftware.DataObjectsNetEntityModel.Common.Properties;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    public partial class FormObjectValueEditor : Form, IModalDialogForm
    {
        private const int DEFAULT_HEIGHT = 235;
        private const int SETTING_CONTROL_LOCATION_Y = 89;
        private readonly Point VALUE_CONTROL_LOCATION = new Point(12, SETTING_CONTROL_LOCATION_Y);
        private readonly Point LOCATION_BUTTON_ACTION1 = new Point(285, SETTING_CONTROL_LOCATION_Y);
        private readonly Point LOCATION_BUTTON_ACTION2 = new Point(343, SETTING_CONTROL_LOCATION_Y);
        private const int VALUE_TEXT_BOX_WIDTH_SMALL = 260;
        private const int VALUE_TEXT_BOX_WIDTH_SMALL_WITH_NULL = 307;
        private const int VALUE_TEXT_BOX_WIDTH_NORMAL = 406;

        private const int FILE_SIZE_100k = 1024*100;

        private const string BYTE_ARRAY_TEXT = "{0} bytes. Use Load file to set value, or Clear to clear value.";
        private const string TEXT_BYTE_ARRAY_ALTERNATIVE = "Byte[]";
        private const string BUTTON_TEXT_LOAD_FILE = "Load file...";
        private const string BUTTON_TEXT_NEW_GUID = "New Guid...";
        private const string BUTTON_TEXT_VALIDATE_VALUE = "Validate value";
        private const string BUTTON_TEXT_CLEAR_FILE = "Clear";
        private const string HINT_BOOLEAN = "Select boolean value.";
        private const string HINT_BYTE = "Enter 1 byte value, min: 0 , max: 255.";
        private const string HINT_BYTE_ARRAY = "Set byte array by loading file.";
        private const string HINT_DATE_TIME = "Enter valid date/time value.";
        private const string HINT_NUMERIC_VALUE = "Enter valid {0} value, min: {1} , max: {2}";
        private const string HINT_GUID = "Enter valid Guid value, in format {00000000-0000-0000-0000-000000000000}";
        private const string HINT_CHAR = "Enter 1 character string.";
        private const string HINT_STRING = "Enter any string value.";
        private const string HINT_TIMESPAN = "Enter valid time span value, in format 00:00.";
        private const int VALUE_TYPE_CUSTOM = 999;
        private const string HINT_CUSTOM_EXPRESSION = "Enter custom value expression, like 'System.Drawing.Color.Black'.";
        private readonly List<Control> allSettingControls = new List<Control>();
        private readonly Dictionary<int, object> typeValues = new Dictionary<int, object>();
        private readonly Dictionary<int, string> typeHints = new Dictionary<int, string>();
        private bool valueTypeChangeNotificationEnabled = true;
        private int? lastSelectedTypeValue = null;
        private bool useNullValueAllowed = false;

        private List<IFilterObjectValueEditorTypes> filterStandardTypes = new List<IFilterObjectValueEditorTypes>();

        public FormObjectValueEditor()
        {
            InitializeComponent();
        }

        private void PopulateForm() {
            this.Size = new Size(this.Width, DEFAULT_HEIGHT);
            allSettingControls.AddRange(new Control[]
                                        {
                                            valueTextBox, valueComboBox, panelDateTime, valueUpDown,
                                            btnAction1, btnAction2
                                        });

            allSettingControls.ForEach(control =>
                                       {
                                           if (!control.In(btnAction1, btnAction2))
                                           {
                                               control.Location = VALUE_CONTROL_LOCATION;
                                           }
                                       });

            btnAction1.Location = LOCATION_BUTTON_ACTION1;
            btnAction2.Location = LOCATION_BUTTON_ACTION2;
            chUseNullValue.Location = new Point(chUseNullValue.Location.X, SETTING_CONTROL_LOCATION_Y);


            PopulateStandartTypes();

            typeHints[(int)StandartType.Boolean] = HINT_BOOLEAN;
            typeHints[(int)StandartType.Byte] = HINT_BYTE;
            typeHints[(int)StandartType.ByteArray] = HINT_BYTE_ARRAY;
            typeHints[(int)StandartType.DateTime] = HINT_DATE_TIME;
            typeHints[(int)StandartType.Decimal] = string.Format(HINT_NUMERIC_VALUE, "decimal", decimal.MinValue, decimal.MaxValue);
            typeHints[(int)StandartType.Double] = string.Format(HINT_NUMERIC_VALUE, "double", double.MinValue, double.MaxValue);
            typeHints[(int)StandartType.Guid] = HINT_GUID;
            typeHints[(int)StandartType.Char] = HINT_CHAR;
            typeHints[(int)StandartType.Int16] = string.Format(HINT_NUMERIC_VALUE, "Int16", Int16.MinValue, Int16.MaxValue);
            typeHints[(int)StandartType.Int32] = string.Format(HINT_NUMERIC_VALUE, "Int32", Int32.MinValue, Int32.MaxValue);
            typeHints[(int)StandartType.Int64] = string.Format(HINT_NUMERIC_VALUE, "Int64", Int64.MinValue, Int64.MaxValue);
            typeHints[(int)StandartType.SByte] = string.Format(HINT_NUMERIC_VALUE, "SByte", SByte.MinValue, SByte.MaxValue);
            typeHints[(int)StandartType.Single] = string.Format(HINT_NUMERIC_VALUE, "Single", Single.MinValue, Single.MaxValue);
            typeHints[(int)StandartType.String] = HINT_STRING;
            typeHints[(int)StandartType.TimeSpan] = HINT_TIMESPAN;
            typeHints[(int)StandartType.UInt16] = string.Format(HINT_NUMERIC_VALUE, "UInt16", UInt16.MinValue, UInt16.MaxValue);
            typeHints[(int)StandartType.UInt32] = string.Format(HINT_NUMERIC_VALUE, "UInt32", UInt32.MinValue, UInt32.MaxValue);
            typeHints[(int)StandartType.UInt64] = string.Format(HINT_NUMERIC_VALUE, "UInt64", UInt64.MinValue, UInt64.MaxValue);
            typeHints[VALUE_TYPE_CUSTOM] = HINT_CUSTOM_EXPRESSION;
        }

        private readonly Type iFilterType = typeof (IFilterObjectValueEditorTypes);
        private List<StandartType> allowedStandardTypes = new List<StandartType>();

        public void BindData(ITypeDescriptorContext context, object value, object[] attributeArguments)
        {
            filterStandardTypes.Clear();
            if (attributeArguments != null && attributeArguments.Length > 0)
            {
                foreach (Type type in attributeArguments.OfType<Type>().Where(item => iFilterType.IsAssignableFrom(item)))
                {
                    IFilterObjectValueEditorTypes filterTypeObj =
                        (IFilterObjectValueEditorTypes) Activator.CreateInstance(type);
                    filterTypeObj.Initialize(context);
                    filterStandardTypes.Add(filterTypeObj);
                }
            }

            //allowedStandardTypes.AddRange(EnumType<StandartType>.Values);
            IEnumerable<StandartType> _stdTypes = EnumType<StandartType>.Values;
            foreach (var filterStandardTypeObj in filterStandardTypes)
            {
                _stdTypes = filterStandardTypeObj.FilterStandartTypes(_stdTypes);
            }
            allowedStandardTypes.AddRange(_stdTypes);

            PopulateForm();

            ObjectValue objectValue = (ObjectValue) value;
            object objValue = objectValue.Value;
            StandartType resolvedStandardType = objValue == null ? StandartType.String : StandardValues.ResolveStandardType(objValue.GetType());
            StandartType valueType = objValue == null ? StandartType.String : resolvedStandardType;
            if (!allowedStandardTypes.Contains(valueType))
            {
                valueType = StandartType.String;
                objValue = objValue.ToString();
            }

            CurrentValueType = valueType;
            if (objectValue.UseCustomExpression)
            {
                CurrentValueType = null;
                objValue = objectValue.CustomExpression;
                valueTextBox.Text = objectValue.CustomExpression;
            }
            else if (objValue != null && valueType == StandartType.String && objValue.GetType() != StandardValues.TYPE_STRING)
            {
                objValue = objValue.ToString();
            }

            this.CurrentValue = objValue;
            UpdateControls();
        }

        private bool IsSelectedCustomExpression
        {
            get { return CurrentValueType == null; }
        }

        public object SaveData()
        {
            object value = SaveData(CurrentValueType.HasValue ? (int) CurrentValueType.Value : VALUE_TYPE_CUSTOM);
            return value;
        }

        private object SaveData(int typeValue)
        {
            string valueText = valueTextBox.Text;
            bool isSelectedCustomExpression = IsSelectedCustomExpression;
            string customExpression = isSelectedCustomExpression ? valueText : null;

            object result = null;
            if (!useNullValueAllowed || !UseNullValueChecked)
            {
                StandartType standartType = (StandartType) typeValue;
                switch (standartType)
                {
                    case StandartType.String:
                    case StandartType.Char:
                    case StandartType.ByteArray:
                    case StandartType.Guid:
                    case StandartType.Double:
                    case StandartType.Single:
                    case StandartType.Decimal:
                    {
                        result = valueText;

                        if (!string.IsNullOrEmpty(valueText))
                        {
                            if (standartType == StandartType.Char)
                            {
                                result = valueText[0];
                            }
                            else if (standartType == StandartType.ByteArray)
                            {
                                result = (byte[])valueTextBox.Tag;
                            }
                            else if (standartType == StandartType.Guid)
                            {
                                result = new Guid(valueText);
                            }
                            else if (standartType == StandartType.Double)
                            {
                                result = double.Parse(valueText);
                            }
                            else if (standartType == StandartType.Single)
                            {
                                result = Single.Parse(valueText);
                            }
                            else if (standartType == StandartType.Decimal)
                            {
                                result = Decimal.Parse(valueText);
                            }
                        }

                        break;
                    }
                    case StandartType.Boolean:
                    {
                        result = Convert.ToBoolean(valueComboBox.SelectedIndex);
                        break;
                    }
                    case StandartType.Byte:
                    case StandartType.Int16:
                    case StandartType.Int32:
                    case StandartType.Int64:
                    case StandartType.SByte:
                    case StandartType.UInt16:
                    case StandartType.UInt32:
                    case StandartType.UInt64:
                    {
                        decimal currentValue = valueUpDown.Value;

                        if (standartType == StandartType.Byte)
                        {
                            result = Convert.ToByte(currentValue);
                        }
                        else if (standartType == StandartType.Int16)
                        {
                            result = Convert.ToInt16(currentValue);
                        }
                        else if (standartType == StandartType.Int32)
                        {
                            result = Convert.ToInt32(currentValue);
                        }
                        else if (standartType == StandartType.Int64)
                        {
                            result = Convert.ToInt64(currentValue);
                        }
                        else if (standartType == StandartType.SByte)
                        {
                            result = Convert.ToSByte(currentValue);
                        }
                        else if (standartType == StandartType.UInt16)
                        {
                            result = Convert.ToUInt16(currentValue);
                        }
                        else if (standartType == StandartType.UInt32)
                        {
                            result = Convert.ToUInt32(currentValue);
                        }
                        else if (standartType == StandartType.UInt64)
                        {
                            result = Convert.ToUInt64(currentValue);
                        }

                        break;
                    }
                    case StandartType.DateTime:
                    case StandartType.TimeSpan:
                    {
                        DateTime date = valueDate.Value;
                        TimeSpan time = valueTime.Value.TimeOfDay;

                        if (standartType == StandartType.DateTime)
                        {
                            result = date;
                            if (valueTime.Checked)
                            {
                                result = date.Add(time);
                            }
                        }
                        else
                        {
                            result = time;
                        }

                        break;
                    }
                }
            }

            ObjectValue objectValue = new ObjectValue(isSelectedCustomExpression, customExpression, result);
            return objectValue;
        }

        private void PopulateStandartTypes()
        {
            //var standardTypes = EnumType<StandartType>.Values;

            foreach (var standardType in allowedStandardTypes)
            {
                typeValues.Add((int)standardType, null);
                typeHints.Add((int)standardType, string.Empty);
            }
            typeValues.Add(VALUE_TYPE_CUSTOM, null);
            typeHints.Add(VALUE_TYPE_CUSTOM, string.Empty);

//            List<ListItem> standartTypeItems = standardTypes.Select(delegate(StandartType type)
//                                    {
//                                        string name = type == StandartType.ByteArray
//                                                        ? TEXT_BYTE_ARRAY_ALTERNATIVE
//                                                        : type.ToString();
//                                        return new ListItem(name, (int)type);
//                                    }).ToList();
//
//            standartTypeItems.Add(new ListItem("Custom Value Expression", VALUE_TYPE_CUSTOM));

            valueTypeChangeNotificationEnabled = false;
            try
            {
                cmbValueTypes.Items.Clear();
                //cmbValueTypes.Items.Add(new IconListEntry("Entity", EntityKind.Entity, Resources.bullet));
                foreach (StandartType standardType in allowedStandardTypes)
                {
                    string typeName = standardType == StandartType.ByteArray
                        ? TEXT_BYTE_ARRAY_ALTERNATIVE : standardType.ToString();

                    cmbValueTypes.Items.Add(new IconListEntry(typeName, (int)standardType, Resources.bullet));
                }

                cmbValueTypes.Items.Add(new IconListEntry("Custom Value Expression", int.MaxValue, Resources.bullet));
                

                //cmbValueTypes.DataSource = standartTypeItems;
            }
            finally
            {
                valueTypeChangeNotificationEnabled = true;
            }
        }

        private object CurrentValue
        {
            get
            {
                int currentValueType = CurrentValueType.HasValue ? (int) CurrentValueType.Value : VALUE_TYPE_CUSTOM;
                return typeValues[currentValueType];
            }
            set
            {
                int currentValueType = CurrentValueType.HasValue ? (int)CurrentValueType.Value : VALUE_TYPE_CUSTOM;
                typeValues[currentValueType] = value;
            }
        }

        private StandartType? CurrentValueType
        {
            get
            {
                IconListEntry listItem = (IconListEntry)cmbValueTypes.SelectedItem;
                int value = (int) listItem.Value;
                if (value != int.MaxValue)
                {
                    return (StandartType) value;
                }

                return null;
            }
            set
            {
                int itemIdx = cmbValueTypes.Items.Count - 1;

                if (value != null)
                {
                    int intValue = (int) value.Value;
//                    string text = value.ToString();
//                    if (value == StandartType.ByteArray)
//                    {
//                        text = TEXT_BYTE_ARRAY_ALTERNATIVE;
//                    }
//
//                    itemIdx = cmbValueTypes.FindString(text);
                    itemIdx = cmbValueTypes.Items.OfType<IconListEntry>().ToList().FindIndex(item => (int) item.Value == intValue);
                }

                if (itemIdx > -1)
                {
                    cmbValueTypes.SelectedIndex = itemIdx;
                }
            }
        }

        private bool UseNullValueChecked
        {
            get { return chUseNullValue.Checked; }
            set { chUseNullValue.Checked = value; }
        }

        private void UpdateControls()
        {
            HideAllSettingControls();

            chUseNullValue.Visible = !CurrentValueType.HasValue || CurrentValueType.Value == StandartType.String;
            useNullValueAllowed = chUseNullValue.Visible;

            lbValueHint.Visible = !UseNullValueChecked;

            if (UseNullValueChecked)
            {
                if (CurrentValueType.HasValue && CurrentValueType.Value != StandartType.String)
                {
                    UseNullValueChecked = false;
                }
                else
                {
                    return;
                }
            }

            Action resetValueTextBox = () =>
                        {
                            valueTextBox.MaxLength = Int16.MaxValue;
                            valueTextBox.ReadOnly = false;
                            valueTextBox.Text = string.Empty;
                            valueTextBox.Size = new Size(VALUE_TEXT_BOX_WIDTH_NORMAL, valueTextBox.Size.Height);
                        };

            string hintText;
            object currentValue = CurrentValue;

            if (!CurrentValueType.HasValue)
            {
                valueTextBox.Visible = true;
                resetValueTextBox();
                valueTextBox.Text = currentValue == null ? string.Empty : currentValue.ToString();
                valueTextBox.Size = new Size(VALUE_TEXT_BOX_WIDTH_SMALL_WITH_NULL, valueTextBox.Size.Height);

                hintText = typeHints[VALUE_TYPE_CUSTOM];
            }
            else
            {
                StandartType currentValueType = CurrentValueType.Value;
                hintText = typeHints[(int)currentValueType];

                switch (currentValueType)
                {
                    case StandartType.String:
                    case StandartType.Char:
                    case StandartType.ByteArray:
                    case StandartType.Guid:
                        { // handle as string in textbox
                            valueTextBox.Visible = true;
                            resetValueTextBox();

                            int textMaxLength = Int16.MaxValue;
                            if (currentValueType == StandartType.Char)
                            {
                                textMaxLength = 1;
                            }
                            else if (currentValueType == StandartType.Guid)
                            {
                                textMaxLength = 38;
                            }

                            bool buttonsVisible = currentValueType.In(StandartType.ByteArray, StandartType.Guid);

                            valueTextBox.MaxLength = textMaxLength;
                            valueTextBox.ReadOnly = currentValueType == StandartType.ByteArray;
                            int sizeWidth = buttonsVisible ? VALUE_TEXT_BOX_WIDTH_SMALL : VALUE_TEXT_BOX_WIDTH_NORMAL;

                            if (currentValueType == StandartType.String)
                            {
                                sizeWidth = VALUE_TEXT_BOX_WIDTH_SMALL_WITH_NULL;
                            }

                            valueTextBox.Size = new Size(sizeWidth, valueTextBox.Size.Height);

                            if (buttonsVisible)
                            {
                                btnAction1.Visible = true;
                                btnAction1.Text = currentValueType == StandartType.ByteArray
                                                        ? BUTTON_TEXT_LOAD_FILE
                                                        : BUTTON_TEXT_NEW_GUID;
                                btnAction2.Visible = true;
                                btnAction2.Text = currentValueType == StandartType.ByteArray
                                                         ? BUTTON_TEXT_CLEAR_FILE
                                                         : BUTTON_TEXT_VALIDATE_VALUE;
                            }

                            if (currentValueType == StandartType.ByteArray)
                            {
                                int byteLength = 0;
                                byte[] byteArray = null;
                                if (currentValue != null)
                                {
                                    byteArray = (byte[])currentValue;
                                    byteLength = byteArray.Length;
                                }

                                valueTextBox.Text = string.Format(BYTE_ARRAY_TEXT, byteLength);
                                valueTextBox.Tag = byteArray;
                            }
                            else
                            {
                                valueTextBox.Text = currentValue != null ? currentValue.ToString() : string.Empty;
                            }
                        
                            break;
                        }
                    case StandartType.Boolean:
                        { // handle boolean in combobox as True/False items
                            valueComboBox.Visible = true;
                            valueComboBox.Items.Clear();
                            valueComboBox.Items.AddRange(new[] {"false", "true"});

                            int idx = 0;
                            if (currentValue != null)
                            {
                                idx = Convert.ToInt32((bool) currentValue);
                            }
                            valueComboBox.SelectedIndex = idx;

                            break;
                        }
                    case StandartType.Byte:
                    case StandartType.Int16:
                    case StandartType.Int32:
                    case StandartType.Int64:
                    case StandartType.SByte:
                    case StandartType.UInt16:
                    case StandartType.UInt32:
                    case StandartType.UInt64:
                        {
                            valueUpDown.Visible = true;
                            decimal maxValue = 0;
                            decimal minValue = 0;

                            Action<object, Func<object, decimal>> setValue =
                                (value, castValue) => valueUpDown.Value = value == null ? 0 : castValue(value);

                            if (currentValueType == StandartType.Byte)
                            {
                                minValue = byte.MinValue;
                                maxValue = byte.MaxValue;
                                //valueUpDown.Value = (byte)currentValue;
                                setValue(currentValue, value => (byte) value);
                            }
                            else if (currentValueType == StandartType.Int16)
                            {
                                minValue = Int16.MinValue;
                                maxValue = Int16.MaxValue;
                                //valueUpDown.Value = (Int16)currentValue;
                                setValue(currentValue, value => (Int16)value);
                            }
                            else if (currentValueType == StandartType.Int32)
                            {
                                minValue = Int32.MinValue;
                                maxValue = Int32.MaxValue;
                                //valueUpDown.Value = (Int32)currentValue;
                                setValue(currentValue, value => (Int32)value);
                            }
                            else if (currentValueType == StandartType.Int64)
                            {
                                minValue = Int64.MinValue;
                                maxValue = Int64.MaxValue;
                                //valueUpDown.Value = (Int64)currentValue;
                                setValue(currentValue, value => (Int64)value);
                            }
                            else if (currentValueType == StandartType.SByte)
                            {
                                minValue = SByte.MinValue;
                                maxValue = SByte.MaxValue;
                                //valueUpDown.Value = (sbyte)currentValue;
                                setValue(currentValue, value => (sbyte)value);
                            }
                            else if (currentValueType == StandartType.UInt16)
                            {
                                minValue = UInt16.MinValue;
                                maxValue = UInt16.MaxValue;
                                //valueUpDown.Value = (UInt16)currentValue;
                                setValue(currentValue, value => (UInt16)value);
                            }
                            else if (currentValueType == StandartType.UInt32)
                            {
                                minValue = UInt32.MinValue;
                                maxValue = UInt32.MaxValue;
                                //valueUpDown.Value = (UInt32)currentValue;
                                setValue(currentValue, value => (UInt32)value);
                            }
                            else if (currentValueType == StandartType.UInt64)
                            {
                                minValue = UInt64.MinValue;
                                maxValue = UInt64.MaxValue;
                                //valueUpDown.Value = (UInt64)currentValue;
                                setValue(currentValue, value => (UInt64)value);
                            }
                        
                            valueUpDown.Minimum = minValue;
                            valueUpDown.Maximum = maxValue;

                            break;
                        }
                    case StandartType.DateTime:
                    case StandartType.TimeSpan:
                        {
                            panelDateTime.Visible = true;
                            valueDate.Visible = currentValueType == StandartType.DateTime;
                            lbDate.Visible = currentValueType == StandartType.DateTime;
                            valueTime.ShowCheckBox = currentValueType == StandartType.DateTime;

                            TimeSpan timeValue;
                            DateTime dateValue;
                            if (currentValueType == StandartType.TimeSpan)
                            {
                                timeValue = currentValue == null ? TimeSpan.Zero : (TimeSpan)currentValue;
                                dateValue = DateTime.Today;
                            }
                            else
                            {
                                DateTime dateTime = currentValue == null ? DateTime.Now : (DateTime) currentValue;
                                timeValue = dateTime.TimeOfDay;
                                dateValue = dateTime.Date;
                            }

                            valueDate.Value = dateValue;
                            valueTime.Value = dateValue.Add(timeValue);

                            break;
                        }
                    case StandartType.Double:
                    case StandartType.Single:
                    case StandartType.Decimal:
                        {
                            valueTextBox.Visible = true;
                            resetValueTextBox();
                            btnAction2.Visible = true;
                            btnAction2.Text = BUTTON_TEXT_VALIDATE_VALUE;

                            valueTextBox.Text = currentValue == null ? "0" : currentValue.ToString();

                            break;
                        }
                }
            }

            lbValueHint.Text = hintText;
        }

        private void HideAllSettingControls()
        {
            allSettingControls.ForEach(control => control.Visible = false);
        }

        private void cmbValueTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!valueTypeChangeNotificationEnabled)
            {
                return;
            }

            if (lastSelectedTypeValue.HasValue)
            {
                typeValues[lastSelectedTypeValue.Value] = SaveData(lastSelectedTypeValue.Value);
            }
            else
            {
                UpdateLastSelectedValue();
            }
            
            UpdateControls();

            UpdateLastSelectedValue();
        }

        private void UpdateLastSelectedValue()
        {
            StandartType? currentValueType = CurrentValueType;
            lastSelectedTypeValue = currentValueType.HasValue ? (int) currentValueType.Value : VALUE_TYPE_CUSTOM;
        }

        private void chUseNullValue_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void btnAction1_Click(object sender, EventArgs e)
        {
            StandartType? currentValueType = CurrentValueType;
            if (!currentValueType.HasValue)
            {
                return;
            }

            if (currentValueType.Value == StandartType.ByteArray)
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string fileName = ofd.FileName;
                    if (File.Exists(fileName))
                    {
                        FileInfo fileInfo = new FileInfo(fileName);
                        bool canLoadFile = true;
                        if (fileInfo.Length > FILE_SIZE_100k)
                        {
                            canLoadFile = MessageBox.Show(
                                "Size of file is more than 100 Kb, do you still want to load file ?",
                                "Import byte array from file...", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2) == DialogResult.Yes;
                        }

                        if (canLoadFile)
                        {
                            byte[] fileContent = File.ReadAllBytes(fileName);
                            //string fileContentAsHexString = Util.BytesToHexString(fileContent);
                            valueTextBox.Tag = fileContent;
                            valueTextBox.Text = string.Format(BYTE_ARRAY_TEXT, fileContent.Length);
                        }
                    }
                }
            }
            else if (currentValueType.Value == StandartType.Guid)
            {
                bool canGenerateGuid = string.IsNullOrEmpty(valueTextBox.Text);
                if (!canGenerateGuid)
                {
                    canGenerateGuid = MessageBox.Show("Do you want to generate new Guid value?", "Generate Guid value",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                                      MessageBoxDefaultButton.Button2) == DialogResult.Yes;
                }

                if (canGenerateGuid)
                {
                    valueTextBox.Text = Guid.NewGuid().ToString();
                }
            }
        }

        private void btnAction2_Click(object sender, EventArgs e)
        {
            StandartType? currentValueType = CurrentValueType;
            if (!currentValueType.HasValue)
            {
                return;
            }

            if (currentValueType.Value == StandartType.ByteArray)
            { // clear byte array value
                byte[] fileContent = (byte[])valueTextBox.Tag;
                if (fileContent != null)
                {
                    if (MessageBox.Show("Do you want to clear associated byte array?", "Clear byte array value",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        valueTextBox.Tag = null;
                        valueTextBox.Text = string.Format(BYTE_ARRAY_TEXT, 0);
                    }
                }
            }
            else if (currentValueType.Value == StandartType.Guid)
            { // validate guid
                bool isValidGuid = !string.IsNullOrEmpty(valueTextBox.Text);

                if (isValidGuid)
                {
                    Guid g;
                    isValidGuid = Guid.TryParse(valueTextBox.Text, out g);
                }

                MessageBox.Show(string.Format("Entered value '{0}' is {1}valid Guid value!", valueTextBox.Text,
                                              isValidGuid ? string.Empty : "not "),
                                "Validate Guid value", MessageBoxButtons.OK,
                                isValidGuid ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
            else if (currentValueType.Value.In(StandartType.Double, StandartType.Single, StandartType.Decimal))
            {
                bool isValidValue = !string.IsNullOrEmpty(valueTextBox.Text);

                if (isValidValue)
                {
                    if (currentValueType.Value == StandartType.Double)
                    {
                        double d;
                        isValidValue = double.TryParse(valueTextBox.Text, out d);
                    }
                    else if (currentValueType.Value == StandartType.Single)
                    {
                        Single d;
                        isValidValue = Single.TryParse(valueTextBox.Text, out d);
                    }
                    else if (currentValueType.Value == StandartType.Decimal)
                    {
                        Decimal d;
                        isValidValue = Decimal.TryParse(valueTextBox.Text, out d);
                    }
                }

                string valueTypeName = currentValueType.Value.ToString();

                MessageBox.Show(string.Format("Entered value '{0}' is {1}valid {2} value!", valueTextBox.Text,
                                              isValidValue ? string.Empty : "not ",
                                              valueTypeName),
                                string.Format("Validate {0} value", valueTypeName), MessageBoxButtons.OK,
                                isValidValue ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
        }
    }

    public interface IFilterObjectValueEditorTypes
    {
        void Initialize(ITypeDescriptorContext context);
        IEnumerable<StandartType> FilterStandartTypes(IEnumerable<StandartType> standartTypes);
    }
}
