namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class DesiredRunTimeDto : PickListDto
    {
        public bool IsDefaultSelection { get; set; }

        public int? FuelTypeID { get; set; }
    }
}
