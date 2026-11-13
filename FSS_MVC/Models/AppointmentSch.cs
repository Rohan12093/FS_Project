namespace FSS_MVC.Models
{
    public class AppointmentSch
    {
        public int AppoScheId { get; set; }
        public DateOnly AppoSchDate { get; set; }
        public TimeOnly AppoSchStime { get; set; }
        public TimeOnly AppoSchEtime { get; set; }
        public bool AppoSchIsBooked { get; set; }
        public int? DoctorId { get; set; }

    }
}
