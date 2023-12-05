namespace WebAppRelation.ViewModel
{
    public class BasketItemVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string ImgUrl { get; set; }
        public int Count { get; set; }

        // Layout Section
        public List<Category> categories { get; set; }
    }
}
