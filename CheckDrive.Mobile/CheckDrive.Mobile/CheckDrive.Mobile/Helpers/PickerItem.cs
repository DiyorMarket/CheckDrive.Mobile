using System.Collections.Generic;
using System;

namespace CheckDrive.Mobile.Helpers
{
    public class PickerItem<TValue>
    {
        public TValue Value { get; set; }
        public string DisplayText { get; set; }

        public PickerItem(TValue value, string displayText)
        {
            Value = value;
            DisplayText = displayText;
        }

        public override string ToString()
        {
            return DisplayText;
        }

        public override bool Equals(object obj)
        {
            if (obj is PickerItem<TValue> other)
            {
                return EqualityComparer<TValue>.Default.Equals(Value, other.Value) &&
                       string.Equals(DisplayText, other.DisplayText, StringComparison.Ordinal);
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + EqualityComparer<TValue>.Default.GetHashCode(Value);
            hash = hash * 23 + (DisplayText?.GetHashCode() ?? 0);
            return hash;
        }
    }
}
