namespace CheckDrive.Mobile.Models
{
    public class OilMark
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
