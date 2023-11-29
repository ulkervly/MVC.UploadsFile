namespace PustokPractice.Models
{
    public class Author:BaseEntity
    {
        public string FullName {  get; set; }
        List<Book> Books { get; set; }
    }
}
