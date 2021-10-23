using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System.Resources;

namespace Khalifa.Framework
{
    public static class Config
    {
        public const short GMT = 2;//How many hours from GMT
        public const int CommonPageSize = 100;
        public const int SmallCommonPageSize = 10;
        public const int NoneOptionValue = -2;
        public const int AllOptionValue = -1;
        public const short AllOptionValueShort = -1;
        public const string AllOptionValueString = "-1";
        public const string ApiSessionHeader = "X-TMALL-UserSessionId";
        public const string ApiSessionObject = "TMALL-API-Session-object";
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

       
        public static class ProductStatus
        {
            public const short Nominated = 200;
            public const short Interviewed = 201;
            public const short Rejected = 202;
            public const short Hired = 203;
            public const short Accepted = 204;
            public const short MissedInterviewed = 205;
        }
    }

    public enum SupportedLanguage
    {
        None,
        Arabic,
        English,
        French
    }

    public static class SupportedLanguageExtensions
    {
        public static string Code(this SupportedLanguage self)
        {
            return self switch
            {
                SupportedLanguage.Arabic => "ar",
                SupportedLanguage.English => "en-US",
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public static SupportedLanguage ToSupportedLang(this string self)
        {
            return self switch
            {
                "ar" => SupportedLanguage.Arabic,
                "en-US" => SupportedLanguage.English,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }

    public static class Stamp
    {
        public static DateTime DateTime() => global::System.DateTime.UtcNow;

        public static string ETag() => Guid.NewGuid().ToString();

        public static string System() => "System";

        public static Guid NewGuid() => Guid.NewGuid();
    }
}
