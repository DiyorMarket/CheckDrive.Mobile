using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace CheckDrive.Mobile.ViewModels
{
    public class TestViewModel : BaseViewModel
    {
        public ObservableCollection<Step> Steps { get; set; }

        public TestViewModel()
        {
            Steps = new ObservableCollection<Step>
            {
                new Step("Doctor's Check", "Driver passed health check Driver passed health check Driver passed health check", "08:30", StepStatus.Completed, true),
                new Step("Mechanic's Check", "Car is ready for use", "09:00", StepStatus.Completed, true),
                new Step("Fueling", "Received 50 liters of fuel", "--:--", StepStatus.InProgress, true),
                new Step("Mechanic Return", "Pending car return", "--:--", StepStatus.Pending, false)
            };
        }
    }

    public class Step : BaseViewModel
    {
        public string Title { get; set; }
        public string Notes { get; set; }
        public string Time { get; set; }
        public Color CircleColor { get; set; }
        public string Icon { get; set; }
        public Color LineColor { get; set; }
        public bool ShowLineBelow { get; set; }

        // Constructor
        public Step(string title, string notes, string time, StepStatus status, bool showLineBelow)
        {
            Title = title;
            Notes = notes;
            Time = time;
            ShowLineBelow = showLineBelow;

            // Assign color and icon based on status
            switch (status)
            {
                case StepStatus.Completed:
                    CircleColor = Color.Green;
                    Icon = "icon_success.png";  // Add your check icon here
                    LineColor = Color.Green;
                    break;
                case StepStatus.Failed:
                    CircleColor = Color.Red;
                    Icon = "icon_failed.png";  // Add your x icon here
                    LineColor = Color.Red;
                    break;
                case StepStatus.InProgress:
                    CircleColor = Color.Green;
                    Icon = "icon_circle.png";  // Add your in-progress icon here
                    LineColor = Color.Gray;
                    break;
                default:
                    CircleColor = Color.Gray;
                    Icon = "icon_circle.png";  // Add your empty circle here
                    LineColor = Color.Gray;
                    break;
            }
        }
    }

    public enum StepStatus
    {
        Completed,
        Failed,
        InProgress,
        Pending
    }
}
