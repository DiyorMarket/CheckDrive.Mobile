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
    }
}
