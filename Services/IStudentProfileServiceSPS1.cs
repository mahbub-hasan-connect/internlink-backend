using InternLink.Api.DTOs;

namespace InternLink.Api.Services
{
    // Module 2: Student Module
    // এই Service-এ business logic থাকবে — validation, duplicate check — Repository-তে শুধু raw database access
    public interface IStudentProfileServiceSPS1
    {
        // নতুন profile তৈরি করে — validation করে (duplicate check ইত্যাদি), তারপর Repository দিয়ে save করে
        Task<StudentProfileResponseSPR1> CreateProfileAsync(int userId, CreateProfileRequestCPR1 request);

        // নির্দিষ্ট user-এর profile খুঁজে বের করে
        Task<StudentProfileResponseSPR1?> GetProfileAsync(int userId);

        // Existing profile আপডেট করে
        Task<StudentProfileResponseSPR1> UpdateProfileAsync(int userId, CreateProfileRequestCPR1 request);
    }
}