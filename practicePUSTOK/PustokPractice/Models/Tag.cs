namespace PustokPractice.Models
{
    public class Tag
    {
        public int id { get; set; }
        public string Name { get; set; }
        public List<BookTag> BookTags { get; set; }
    }
}
