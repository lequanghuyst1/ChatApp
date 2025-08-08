using System;

namespace ChatApp.Domain.Entities
{
    public class Media
    {
       public long ID { get; set; }
       public long MessageID { get; set; }
       public string FileUrl { get; set; }
       public string ThumbUrl { get; set; }
       public int FileType { get; set; }
       public long FileSize { get; set; }
       public string FileName { get; set; }
       public DateTime CreatedAt { get; set; }
    }
}