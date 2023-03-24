namespace Artillery.Common;

public class ValidationConstants
{
    //Country
    public const int CountryNameMaxLength = 60;
    public const int CountryNameMinLength = 4;

    //Manufacturer
    public const int ManufacturerNameMaxLength = 40;
    public const int ManufacturerNameMinLength = 4;
    public const int FoundedMaxLength = 100;
    public const int FoundedMinLength = 10;

    //Shell
    public const int CaliberMaxLength = 30;
    public const int CaliberMinLength = 4;
    public const int ShellMaxWeight = 1_680;
    public const int ShellMinWeight = 2;

    //Army
    public const int ArmyMaxSize = 10_000_000;
    public const int ArmyMinSize = 50_000;

    //Gun
    public const int GunMaxWeight = 1_350_000;
    public const int GunMinWeight = 100;
    public const double BarrelMaxLength = 35.00;
    public const double BarrelMinLength = 2.00;
    public const int RangeMaxLength = 100_000;
    public const int RangeMinLength = 1;
}

