namespace InternLink.Api.DTOs
{
    // Recruiter → Applicant direct communication (UR3).
    // Recruiter এই body দিয়ে একটা custom message পাঠাবে — এটা student-এর
    // Notification হিসেবে দেখা যাবে।
    public class SendMessageRequestSMR1
    {
        public string Message { get; set; } = string.Empty;
    }
}
