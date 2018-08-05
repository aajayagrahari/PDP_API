namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class StartingCodeDto : PickListDto
    {
        public decimal? KVAHPStarting { get; set; }

        public string AmpsDescription { get; set; }

        public string KWMDescription { get; set; }

        public string LanguageKeyKWM { get; set; }

        public string LanguageKeyHP { get; set; }
    }
}
