using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using IMS_Library;

namespace IMS_Interface
{
    public class IMSPreferenceBinding
    {
        public Type TargetBindingType;
        public List<PreferenceDisplay> PreferenceLayout = new List<PreferenceDisplay>();

        public abstract class PreferenceDisplay
        {
            public string FieldName;
            public string DisplayName;
            public string Description;

            public PreferenceDisplay() { }
            public PreferenceDisplay(string name, string display, string description)
            {
                FieldName = name;
                DisplayName = display;
                Description = description;
            }

            public abstract void SetData(IMSConfiguration configuration);
            public abstract void PutData(IMSConfiguration configuration);
        }

        public class ConditionalDisplay : PreferenceDisplay
        {
            public BooleanDisplay ControllingDisplay;
            public PreferenceDisplay InternalDisplay;

            public ConditionalDisplay(BooleanDisplay display, PreferenceDisplay displayToShow)
            {
                ControllingDisplay = display;
                InternalDisplay = displayToShow;
            }

            public override Panel GenerateMainControl(int id)
            {
                Panel toReturn = InternalDisplay.GenerateMainControl(id);
                ControllingDisplay.BooleanToggle.ShowPanelsForTrueButton.Add(toReturn);
                return toReturn;
            }

            public override void PutData(IMSConfiguration configuration)
            {
                InternalDisplay.PutData(configuration);
            }

            public override void SetData(IMSConfiguration configuration)
            {
                InternalDisplay.SetData(configuration);
            }
        }

        public class StringDisplay : PreferenceDisplay
        {
            protected TextBox StringBox;

            public StringDisplay(Page parent, string name, string display, string description) : base(parent, name, display, description)
            {
            }

            public override void PutData(IMSConfiguration configuration)
            {
                configuration.GetType().GetField(FieldName, BindingFlags.Public | BindingFlags.Instance).SetValue(configuration, StringBox.Text);
            }

            public override Panel GenerateMainControl(int id)
            {
                Panel panel = base.GenerateMainControl(id);
                TextBox box = new TextBox();
                box.Enabled = true;
                box.ID = "WebPortEntryBox-" + id;
                box.CssClass = "form-control";
                box.Attributes.Add("style", "margin-top:5px;margin-bottom:5px");
                box.Attributes.Add("data-toggle", "tooltip");
                box.Attributes.Add("data-placement", "right");
                box.Attributes.Add("data-title", Description);
                panel.Controls.Add(box);

                StringBox = box;
                panel.Controls.Add(new LiteralControl("<br/>"));
                return panel;
            }

            public override void SetData(IMSConfiguration configuration)
            {
                StringBox.Text = (string)configuration.GetType().GetField(FieldName, BindingFlags.Public | BindingFlags.Instance).GetValue(configuration);
            }
        }

        public class IntegerDisplay : PreferenceDisplay
        {
            public int Minimum, Maximum;

            protected TextBox IntBox;

            public IntegerDisplay(Page parent, string name, string display, string description, int min, int max) : base(parent, name, display, description)
            {
                Minimum = min;
                Maximum = max;
            }

            public override void PutData(IMSConfiguration configuration)
            {
                configuration.GetType().GetField(FieldName, BindingFlags.Public | BindingFlags.Instance).SetValue(configuration, int.Parse(IntBox.Text));
            }

            public override Panel GenerateMainControl(int id)
            {
                Panel panel = base.GenerateMainControl(id);
                TextBox box = new TextBox();
                box.Enabled = true;
                box.ID = "WebPortEntryBox-" + id;
                box.CssClass = "form-control";
                box.Attributes.Add("style", "margin-top:5px;margin-bottom:5px");
                box.Attributes.Add("data-toggle", "tooltip");
                box.Attributes.Add("data-placement", "right");
                box.Attributes.Add("data-title", Description);
                box.Attributes.Add("type", "number");
                box.Attributes.Add("min", Minimum.ToString());
                box.Attributes.Add("max", Maximum.ToString());
                panel.Controls.Add(box);
                RangeValidator validator = new RangeValidator();
                validator.Type = ValidationDataType.Integer;
                validator.MinimumValue = Minimum.ToString();
                validator.MaximumValue = Maximum.ToString();
                validator.Enabled = true;
                validator.ControlToValidate = box.ID;
                panel.Controls.Add(validator);
                panel.Controls.Add(box);

                IntBox = box;
                panel.Controls.Add(new LiteralControl("<br/>"));
                return panel;
            }

            public override void SetData(IMSConfiguration configuration)
            {
                IntBox.Text = configuration.GetType().GetField(FieldName, BindingFlags.Public | BindingFlags.Instance).GetValue(configuration).ToString();
            }
        }

        public class PortDisplay : PreferenceDisplay
        {
            protected TextBox PortBox;
            protected MultiToggle ForwardToggle;

            public PortDisplay(Page parent, string name, string display, string description) : base(parent, name, display, description)
            {
            }

            public override void PutData(IMSConfiguration configuration)
            {
                WebPort port = new WebPort(int.Parse(PortBox.Text), ForwardToggle.SelectedOption == 0);
                configuration.GetType().GetField(FieldName, BindingFlags.Public | BindingFlags.Instance).SetValue(configuration, port);
            }

            public override Panel GenerateMainControl(int id)
            {
                Panel panel = base.GenerateMainControl(id);
                TextBox box = new TextBox();
                box.Enabled = true;
                box.ID = "WebPortEntryBox-" + id;
                box.CssClass = "form-control";
                box.Attributes.Add("style", "margin-top:5px;margin-bottom:5px");
                box.Attributes.Add("data-toggle", "tooltip");
                box.Attributes.Add("data-placement", "right");
                box.Attributes.Add("data-title", Description);
                box.Attributes.Add("type", "number");
                box.Attributes.Add("min", ushort.MinValue.ToString());
                box.Attributes.Add("max", ushort.MaxValue.ToString());
                panel.Controls.Add(box);
                RangeValidator validator = new RangeValidator();
                validator.Type = ValidationDataType.Integer;
                validator.MinimumValue = ushort.MinValue.ToString();
                validator.MaximumValue = ushort.MaxValue.ToString();
                validator.Enabled = true;
                validator.ControlToValidate = box.ID;
                panel.Controls.Add(validator);
                MultiToggle buttonList = (MultiToggle)ParentPage.LoadControl("~/MultiToggle.ascx");
                buttonList.AddOption("Forward port");
                buttonList.AddOption("Don't forward");
                buttonList.ID = "WebPortBooleanBox-" + id;
                panel.Controls.Add(buttonList);

                PortBox = box;
                ForwardToggle = buttonList;
                panel.Controls.Add(new LiteralControl("<br/>"));
                return panel;
            }

            public override void SetData(IMSConfiguration configuration)
            {
                WebPort port = (WebPort)configuration.GetType().GetField(FieldName, BindingFlags.Public | BindingFlags.Instance).GetValue(configuration);
                PortBox.Text = port.Port.ToString();
                ForwardToggle.SelectedOption = port.AttemptUPnPForwarding ? 0 : 1;
            }
        }

        public class BooleanDisplay : PreferenceDisplay
        {
            public string WhenTrue, WhenFalse;

            public bool IsEnabled { get => BooleanToggle.SelectedOption == 0; }
            public MultiToggle BooleanToggle;

            public BooleanDisplay(Page parent, string name, string display, string description, string whenTrue, string whenFalse) : base(parent, name, display, description)
            {
                WhenTrue = whenTrue;
                WhenFalse = whenFalse;
            }

            public override Panel GenerateMainControl(int id)
            {
                Panel panel = base.GenerateMainControl(id);
                MultiToggle buttonList = (MultiToggle)ParentPage.LoadControl("~/MultiToggle.ascx");
                buttonList.HolderPanel.Attributes.Add("style", "margin-top:5px;margin-bottom:5px");
                buttonList.HolderPanel.Attributes.Add("data-toggle", "tooltip");
                buttonList.HolderPanel.Attributes.Add("data-placement", "right");
                buttonList.HolderPanel.Attributes.Add("data-title", Description);
                buttonList.AddOption(WhenTrue);
                buttonList.AddOption(WhenFalse);
                buttonList.ID = "BooleanEntryBox-" + id;
                panel.Controls.Add(buttonList);
                BooleanToggle = buttonList;
                panel.Controls.Add(new LiteralControl("<br/>"));
                return panel;
            }

            public override void PutData(IMSConfiguration configuration)
            {
                configuration.GetType().GetField(FieldName, BindingFlags.Public | BindingFlags.Instance).SetValue(configuration, BooleanToggle.SelectedOption == 0);
            }

            public override void SetData(IMSConfiguration configuration)
            {
                BooleanToggle.SelectedOption = (bool)configuration.GetType().GetField(FieldName, BindingFlags.Public | BindingFlags.Instance).GetValue(configuration) ? 0 : 1;
            }
        }

        public class MultiToggleDisplay : PreferenceDisplay
        {
            public string[] Values;
            public int IndexOffset = 0;

            protected MultiToggle BooleanToggle;

            public MultiToggleDisplay(Page parent, string name, string display, string description, string[] values) : base(parent, name, display, description)
            {
                Values = values;
            }

            public override Panel GenerateMainControl(int id)
            {
                Panel panel = base.GenerateMainControl(id);
                MultiToggle buttonList = (MultiToggle)ParentPage.LoadControl("~/MultiToggle.ascx");
                buttonList.HolderPanel.Attributes.Add("style", "margin-top:5px;margin-bottom:5px");
                buttonList.HolderPanel.Attributes.Add("data-toggle", "tooltip");
                buttonList.HolderPanel.Attributes.Add("data-placement", "right");
                buttonList.HolderPanel.Attributes.Add("data-title", Description);
                foreach(string value in Values)
                {
                    buttonList.AddOption(value);
                }
                buttonList.ID = "BooleanEntryBox-" + id;
                panel.Controls.Add(buttonList);
                BooleanToggle = buttonList;
                panel.Controls.Add(new LiteralControl("<br/>"));
                return panel;
            }

            public override void PutData(IMSConfiguration configuration)
            {
                configuration.GetType().GetField(FieldName, BindingFlags.Public | BindingFlags.Instance).SetValue(configuration, BooleanToggle.SelectedOption + IndexOffset);
            }

            public override void SetData(IMSConfiguration configuration)
            {
                BooleanToggle.SelectedOption = (int)configuration.GetType().GetField(FieldName, BindingFlags.Public | BindingFlags.Instance).GetValue(configuration) - IndexOffset;
            }
        }
    }
}