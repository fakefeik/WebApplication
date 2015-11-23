namespace WebApplication.Models
{
    public class Article
    {
        public string Title;
        public string Content;
        public string ImageSrc;
        public string Html;
    }

    public class Picture
    {
        public string ImageSrc;
        public string ImageAlt;
        public string Description;
    }

    public class ArticlesModel
    {
        public Article[] Articles;
        public Picture[] Pictures;
    }
}