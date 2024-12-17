namespace CheckDrive.Mobile.Models.Driver
{
    public class DriverDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int? AssignedCarId { get; set; }

        public DriverDto()
        {
        }

        public DriverDto(int id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }
    }
}
