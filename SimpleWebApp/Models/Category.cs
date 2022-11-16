using System.ComponentModel.DataAnnotations;

namespace SimpleWebApp.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public Category(int categoryId, IFormFile file)
        {
            CategoryId = categoryId;
            SavePicture(file).GetAwaiter().GetResult();
        }
        [Required] 
        public int CategoryId { get; set; }
        [Required] 
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public byte[]? Picture { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual void UpdateCategory(Category cat)
        {
            CategoryName = cat.CategoryName ?? CategoryName;
            Description = cat.Description ?? Description;
            Picture = cat.Picture ?? Picture;
        }

        public async Task SavePicture(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            Picture = AddNoise(stream.ToArray());
        }

        public virtual string GetBase64Image()
        {
            return $"data:image/gif;base64,{Convert.ToBase64String(Picture.Skip(78).ToArray())}";
        }

        public override string ToString()
        {
            var res = $"{nameof(CategoryId)} : {CategoryId}" +
                      $"{nameof(CategoryName)} : {CategoryName}" +
                      $"{nameof(Description)} : {Description}" +
                      $"{nameof(Picture)} : {Picture}";
            return res;
        }

        private byte[] AddNoise(byte[] p)
        {
            var l = new List<byte>();
            Random randNum = new Random();
            for (int i = 0; i < 78; i++)
            {
                l.Add((byte)randNum.Next(0,255));
            }
            l.AddRange(p.ToList());
            return l.ToArray();
        }
    }
}
