namespace Quid_pro_Quo.WebApiModels
{
    public class GetPostByFilterApiModel
    {
        public string keywords { get; set; }
        public string date { get; set; }
        public string geomarker { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
