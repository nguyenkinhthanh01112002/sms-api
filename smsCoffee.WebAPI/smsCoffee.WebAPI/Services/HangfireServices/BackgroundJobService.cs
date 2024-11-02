using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using smsCoffee.WebAPI.Interfaces;
using smsCoffee.WebAPI.Models;
using System.ComponentModel;

namespace smsCoffee.WebAPI.Services.HangfireServices
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<BackgroundJobService> _logger;
        private readonly List<string?> _recipientEmails;
        private readonly UserManager<AppUser> _userManager;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private static DateTime _lastExecutionTime = DateTime.MinValue;

        public BackgroundJobService(IEmailService emailService, ILogger<BackgroundJobService> logger,UserManager<AppUser> userManager)
        {
            _emailService = emailService;
            _logger = logger;
            _userManager = userManager;
            _recipientEmails = new List<string?>();
        }


        public void InitializeRecurringJobs()
        {

            try
            {
                // Sử dụng expression thay vì delegate
                RecurringJob.AddOrUpdate<BackgroundJobService>(
                    "daily-notification",      // Job ID
                    x => x.NewProcessDailyReport(),  // Expression chỉ định method cần gọi
                   "* * * * *"           // Schedule
                );

                _logger.LogInformation("Recurring jobs initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize recurring jobs");
                throw;
            }
        }

        // Tạo method riêng để xử lý daily report
        public async Task ProcessDailyReport()
        {        // Log với timestamp
            _logger.LogInformation($"=== Starting daily report process at {DateTime.Now} ===");
            Console.WriteLine($"=== Starting daily report process at {DateTime.Now} ===");

            // Giả lập xử lý
            await Task.Delay(10);

            // Log để test
            Console.WriteLine("Test - Process is running");
            _logger.LogInformation("Test - Process is running");

            // Log kết thúc
            Console.WriteLine($"=== Daily report completed at {DateTime.Now} ===");
            _logger.LogInformation($"=== Daily report completed at {DateTime.Now} ===");

        }
        public async Task NewProcessDailyReport()
        {
            if (!await _semaphore.WaitAsync(TimeSpan.FromSeconds(30)))
            {
                _logger.LogWarning("Another notification process is still running. Skipping this execution.");
                return;
            }
            try
            {
                // Kiểm tra xem đã đủ 1 phút từ lần gửi cuối chưa
                var currentTime = DateTime.Now;
                if (currentTime - _lastExecutionTime < TimeSpan.FromMinutes(1))
                {
                    _logger.LogInformation("Skipping execution - less than 1 minute since last run");
                    return;
                }
                _logger.LogInformation("Starting minute notification process at: {time}", currentTime);
                var users = await _userManager.Users.ToListAsync();
                foreach (var item in users)
                {
                    _recipientEmails.Add(item.Email.ToString());
                }
                _logger.LogInformation("Starting daily attendance notification process");

                string subject = "Nhắc nhở điểm danh đi làm";
                string body = GetEmailTemplate();

                foreach (var email in _recipientEmails)
                {
                    if(email != null)
                    {
                        await _emailService.SendEmailAsync(email, subject, body);
                    }                
                    _logger.LogInformation($"Sent attendance notification to {email}");
                    await Task.Delay(1000);
                }

                _logger.LogInformation("Completed daily attendance notification process");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending attendance notifications");
                throw;
            }
            finally
            {
                // Luôn đảm bảo giải phóng semaphore
                _semaphore.Release();
                // Clear list email để tránh gửi trùng lặp trong lần chạy tiếp theo
                _recipientEmails.Clear();
            }
        }
        private string GetEmailTemplate()
        {
            return @"
            <html>
            <body>
                <h2>Nhắc nhở điểm danh đi làm</h2>
                <p>Xin chào,</p>
                <p>Đây là email nhắc nhở bạn điểm danh đi làm hôm nay.</p>
                <p>Vui lòng thực hiện điểm danh trước 8h sáng.</p>
                <p>Trân trọng,<br/>HR Department</p>
            </body>
            </html>";
        }
        public string EnqueueEmailJob(string to, string subject, string body)
        {

            return BackgroundJob.Enqueue(
                () => _emailService.SendEmailAsync(to, subject, body)
            );
        }
    }
}
