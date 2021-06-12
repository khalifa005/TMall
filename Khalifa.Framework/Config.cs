using System;

namespace Khalifa.Framework
{
    public static class Config
    {
        public const int CommonPageSize = 100;
        public const int SmallCommonPageSize = 10;
        public const int NoneOptionValue = -2;
        public const int AllOptionValue = -1;
        public const short AllOptionValueShort = -1;
        public const string AllOptionValueString = "-1";
        public const string ApiSessionHeader = "X-LMAP-UserSessionId";
        public const string ApiSessionObject = "LMAP-API-Session-object";
        public const int TrueValue = 1;
        public const int FalseValue = 0;
        public const string DefaultCountry = "EGY";
        public const int GeneratedPasswordLength = 20;
        public const int PaginationSize = 12;
        public const short OtherComputerSkillId = 5;
        public const short OtherDisabilityId = 5;
        public const short OtherAdditionalBenefits = 3;
        public const short MealAllowanceBenefitAllowance = 1;
        public const short TransportationBenefitAllowance = 2;
        public const short HousingBenefitAllowance = 2;
        public const short HousingBenefitNo = 3;

        public const short MilitaryStatusNotApplicable = 10; //check military_status table

      
        public static class Gender
        {
            public const string Male = "M";
            public const string Female = "F";
        }

 
        public static class Timeouts
        {
            public const short EmploymentCenterSessionInDays = 1;
            public const short JobSeekerSessionInMinutes = 40;
            public const short EmployerSessioninDays = 1;
        }

    

        public static int GetPercentage(int current, int total)
        {
            var result = (int)Math.Round((double)(100 * current) / total);
            if (result < 1)
                return 0;

            return result;
        }

       
        public static class NominationStatus
        {
            public const short Nominated = 200;
            public const short Interviewed = 201;
            public const short Rejected = 202;
            public const short Hired = 203;
            public const short Accepted = 204;
            public const short MissedInterviewed = 205;
        }
    }
}
