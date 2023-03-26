namespace Theatre.Common;

public class ValidationConstants
{
    //Theathre
    public const int TheathreNameMaxLength = 30;
    public const int TheathreNameMinLength = 4;
    public const sbyte TheathreNumberOfHallsMaxLength = 10;
    public const sbyte TheathreNumberOfHallsMinLength = 1;
    public const int DirectorMaxLength = 30;
    public const int DirectorMinLength = 4;

    //Play
    public const int TitleMaxLength = 50;
    public const int TitleMinLength = 4;
    public const double RatingMaxLength = 10.00;
    public const double RatingMinLength = 0.00;
    public const int DescriptionMaxLength = 700;
    public const int ScreenwriterMaxLength = 30;
    public const int ScreenwriterMinLength = 4;

    //Cast
    public const int FullNameMaxLength = 30;
    public const int FullNameMinLength = 4;

    //Ticket
    public const double PriceMaxLength = 100.00;
    public const double PriceMinLength = 1.00;
}

