using System.Collections.Generic;

namespace IrisClient.Models
{
    public class sweetsFull
    {
        public float sweets { get; set; }
        public int donate_score { get; set; }
        public string? result { get; set; }

        public List<HistoryEntry> history { get; set; } = new();

    }
    public class HistoryEntry
    {
        public long date { get; set; }
        public float amount { get; set; }
        public float balance { get; set; }
        public long to_user_id { get; set; }
        public long id { get; set; }
        public string? type { get; set; }
        public HistoryInfo? info { get; set; }
    }

    public class HistoryInfo
    {
        public int donateScore { get; set; }
        public float sweets { get; set; }
        public float commission { get; set; }
        public long id_chat { get; set; }
        public long id_local { get; set; }
    }
}