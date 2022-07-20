namespace ProductShop.DTOs.Category
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public class ImportCategoryDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /*
    [JsonObject]
    public class ImportCategoryDto
    {
        [JsonProperty("name")]
        [Required]
        public string Name { get; set; }
    }
    */
}
