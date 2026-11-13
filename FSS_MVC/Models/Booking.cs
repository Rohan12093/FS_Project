namespace FSS_MVC.Models
{
    public class Booking
    {

        public int BookingId { get; set; }

        public DateOnly BookingDate { get; set; }

        public TimeOnly BookingTime { get; set; }

        public decimal BookingFee { get; set; }

        public int? PatientId { get; set; }

        public int? DoctorId { get; set; }
        public string DoctorFname { get; set; } = "";

    }
}
