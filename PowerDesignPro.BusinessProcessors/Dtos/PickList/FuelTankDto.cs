namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class FuelTankDto : PickListDto
    {
        public bool IsDefaultSelection { get; set; }

        public int? FuelTypeID { get; set; }
    }
}
