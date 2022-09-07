namespace BDMall.Model
{
    interface IDimensions
    {
        decimal Width { get; set; }
        decimal Heigth { get; set; }
        decimal Length { get; set; }
        int Unit { get; set; }
    }
}
