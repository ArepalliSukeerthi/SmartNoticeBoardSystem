using System.Collections.Generic;

namespace SmartNoticeBoardSystem.Models
{
    public class DashboardViewModel
    {
        // Dashboard Statistics
        public int TotalNotices { get; set; }

        public int TotalStudents { get; set; }

        public int TotalFaculty { get; set; }

        public int TotalAdmins { get; set; }

        // Notice Statistics
        public int ActiveNotices { get; set; }

        public int ExpiredNotices { get; set; }

        public int PinnedNotices { get; set; }

        public int TotalDownloads { get; set; }

        public int TotalLikes { get; set; }

        // Recent Notices
        public List<Notice> RecentNotices { get; set; } = new();

        // Most Popular
        public Notice? MostLikedNotice { get; set; }

        public Notice? MostDownloadedNotice { get; set; }

        // Upcoming Expiry Notices
        public List<Notice> ExpiringSoon { get; set; } = new();

        // Pinned Notices
        public List<Notice> PinnedNoticeList { get; set; } = new();
    }
}