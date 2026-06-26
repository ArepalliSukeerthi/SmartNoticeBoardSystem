namespace SmartNoticeBoardSystem.Models
{
    public class Notice
    {
        public int NoticeId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Summary { get; set; }

        public string? Category { get; set; }

        public string? Priority { get; set; }

        public DateTime PublishDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string? FilePath { get; set; }

        public int CreatedBy { get; set; }

        // New Features
        public bool IsPinned { get; set; } = false;

        public int Likes { get; set; } = 0;

        public int DownloadCount { get; set; } = 0;
    }
}