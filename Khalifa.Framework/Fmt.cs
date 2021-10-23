using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Khalifa.Framework
{
    //HtmlHelpers
    public static class Fmt
    {
        public const string Title = "Title";
        public const string IsFluid = "IsFluid";
        public const string IsDeleted = "IsDeleted";

        public const string DateInputFormat = "{0:yyyy-MM-dd}";
        public const string DateInputPlaceholder = "yyyy-mm-dd";
        public const string DateOnlyDisplayPatternCompact = "yyyy-MM-dd";
        public const string DateOnlyDisplayFormatCompact = "{0:yyyy-MM-dd}";

        public static MarkupString IfEmail(string? email) => string.IsNullOrWhiteSpace(email) ? new MarkupString(string.Empty) : new MarkupString($"<a href=\"mailto:{email}\">{email}</a>");

        public static MarkupString IfLink(string? link, string label) => string.IsNullOrWhiteSpace(link) ? new MarkupString(label) : new MarkupString($"<a href=\"{link}\">{label}</a>");

        public static string IfNotNull(string? content, string alt)
        {
            if (string.IsNullOrWhiteSpace(content))
                return alt;

            return content;
        }

        public static string Gender(string code, IStringLocalizer local)
        {
            if (string.IsNullOrWhiteSpace(code))
                return string.Empty;

            if (code.ToLower() == "m")
                return local["male"];
            else
                return local["female"];
        }

        public static decimal Round(decimal dec, int roundPlace) => decimal.Round(dec, roundPlace, MidpointRounding.AwayFromZero);

        public static string IfConcat(string? content, params string[] strings)
        {
            if (string.IsNullOrWhiteSpace(content))
                return string.Empty;

            var all = string.Join(string.Empty, strings);
            return content + all;
        }

        public static string DateDisplay(DateTime? dateTime, char separator = '/')
        {
            return $"{dateTime?.ToString($"yyyy{separator}MM{separator}dd")}";
        }

        public static string TimeDisplay(DateTime? dateTime)
        {
            return $"{dateTime?.ToString("HH:mm")}";
        }

        public static string DateTimeDisplay(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return string.Empty;

            if (dateTime.Value.Kind == DateTimeKind.Utc)
                dateTime = dateTime.Value.AddHours(Config.GMT); //

            return $"{dateTime?.ToString("yyyy/MM/dd HH:mm")}";
        }


        public static string FullDateTimeDisplay(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return string.Empty;

            if (dateTime.Value.Kind == DateTimeKind.Utc)
                dateTime = dateTime.Value.AddHours(Config.GMT); //

            return $"{dateTime?.ToString("dddd, dd MMMM yyyy HH:mm")}";
        }

        static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder().UseSoftlineBreakAsHardlineBreak().UseAdvancedExtensions().Build();

        public static MarkupString Markdown(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return new MarkupString("");

            return new MarkupString(Markdig.Markdown.ToHtml(str, Pipeline));
        }

        public static IHtmlContent Either(string content1, string content2)
        {
            if (!string.IsNullOrWhiteSpace(content1))
                return new HtmlString(content1);
            if (!string.IsNullOrWhiteSpace(content2)) return new HtmlString(content2);

            return new HtmlString("");
        }

        public static IHtmlContent Condition(bool condition, string content1, string content2)
        {
            return condition ? new HtmlString(content1) : new HtmlString(content2);
        }

        public static IHtmlContent ToPicturePath((int? width, int? height) resize, params string[] segments)
        {
            var path = "";

            foreach (var s in segments) path += "/" + s.Replace('\\', '/');

            if ((resize.width != null) & (resize.height != null)) path += $"?width={resize.width}&height={resize.height}&rmode=crop";

            return new HtmlString(path);
        }

        public static string YesNoKey(bool? yesOrNo)
        {
            if (yesOrNo is null)
                return " ";

            return yesOrNo.Value ? "yes" : "no";
        }

        public static IHtmlContent IsActive(string content, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(content)) return new HtmlString("");

            if (isActive) return new HtmlString(content);

            return new HtmlString($"<strike>{content}</strike>");
        }

        public static IHtmlContent UpdatedValue(string oldValue, string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue)) return new HtmlString(oldValue);

            return new HtmlString($"<strike>{oldValue}</strike> -> {newValue}");
        }

        public static string IsTest(string content, bool isTest)
        {
            if (string.IsNullOrWhiteSpace(content)) return content;

            if (!isTest) return content;

            return $"{content} (Test)";
        }

        public static IHtmlContent EmptyGuard(IHtmlContent content)
        {
            if (string.IsNullOrWhiteSpace(content?.ToString())) return new HtmlString("_");

            return content;
        }

        public static IHtmlContent EmptyGuard(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return new HtmlString("_");

            return new HtmlString(content);
        }

        public static string Chain(string separator, params string[] content)
        {
            var bld = new StringBuilder();

            var length = content.Length;

            var idx = 0;
            foreach (var c in content)
            {
                if (!string.IsNullOrWhiteSpace(c))
                {
                    if (idx == length - 1)
                        bld.Append(c);
                    else
                        bld.Append(c + separator);
                }

                idx++;
            }

            return bld.ToString();
        }

        public static string Chain(string separator, List<string> content)
        {
            var bld = new StringBuilder();

            var length = content.Count;

            var idx = 0;

            foreach (var c in content)
            {
                if (!string.IsNullOrWhiteSpace(c))
                {
                    if (idx == length - 1)
                        bld.Append(c);
                    else
                        bld.Append(c + separator);
                }

                idx++;
            }

            return bld.ToString();
        }

        public static IHtmlContent SplitCamelCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new HtmlString("");

            return new HtmlString(Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim());
        }

        public static IHtmlContent FromCamelCaseToUnderScore(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new HtmlString("");

            return new HtmlString(Regex.Replace(input, "([A-Z])", "_$1", RegexOptions.Compiled).Trim().Trim('_'));
        }

        public static IHtmlContent DisplayDeltaTime(double seconds)
        {
            var minutes = seconds / 60;

            if (minutes < 0f)
                return new HtmlString($"{minutes:0.##} minute");
            return new HtmlString($"{minutes:0.##} minutes");
        }

        public static bool DisableGenericHourMinuteComponentIfDateIsEmpty(DateTime? date)
        {
            if (date.HasValue)
                return false;

            return true;
        }
    }
}
