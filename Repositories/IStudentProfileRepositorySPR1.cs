using InternLink.Api.Models;

namespace InternLink.Api.Repositories
{
    // Module 2: Student Module
    // এই Repository-র কাজ শুধু StudentProfile-এর database read/write করা, কোনো validation/business logic এখানে না
    public interface IStudentProfileRepositorySPR1
    {
        // নির্দিষ্ট UserId দিয়ে profile খুঁজে বের করা — প্রতিটা student-এর একটাই profile থাকবে
        Task<StudentProfile?> GetByUserIdAsync(int userId);

        // নতুন profile তৈরি করার সময় ব্যবহার হবে
        Task AddAsync(StudentProfile profile);

        // Profile update করার সময় ব্যবহার হবে (EF Core নিজে থেকে change track করে, তাই আলাদা "Update" method লাগে না, শুধু SaveChanges)
        Task SaveChangesAsync();
    }
}