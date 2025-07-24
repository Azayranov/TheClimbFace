namespace TheClimbFace.Common;

public static class EntityValidations
{
    public static class Climber
    {
        public const int FirstNameMaxLength = 25;
        public const int LastNameMaxLength = 25;
        public const int MiddleNameMaxLength = 25;

    }

    public static class Club
    {
        public const int ClubNameMaxLength = 100;
    }

    public static class ClimbingCompetition
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 100;
        public const int OrganiserMinLength = 3;
        public const int OrganiserMaxLength = 100;
        public const int InfoMinLength = 3;
        public const int InfoMaxLength = 2000;

        public const int TypeMinLength = 4;
        public const int TypeMaxLength = 7;
    }
}
