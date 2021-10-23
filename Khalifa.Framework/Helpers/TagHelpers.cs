using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace Khalifa.Framework
{
    [HtmlTargetElement("checkboxlist", Attributes = "asp-items, asp-for, asp-lowercase-id")]
    public class CheckListBoxTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-items")]
        public IEnumerable<SelectListItem> Items { get; set; }

        [HtmlAttributeName("asp-for")]
        public string ModelName { get; set; }

        [HtmlAttributeName("asp-lowercase-id")]
        public bool IsLowerCaseId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Content.AppendHtml("<ul>");
            var i = 0;
            foreach (var item in Items)
            {
                var selected = item.Selected ? @"checked=""checked""" : "";
                var disabled = item.Disabled ? @"disabled=""disabled""" : "";

                var html = "<li>";
                if (IsLowerCaseId)
                {
                    html += $@"<label><input type=""checkbox"" {selected} {disabled} id=""{ModelName}_{i}__selected"" name=""{ModelName}[{i}].Selected"" value=""true"" /> {item.Text}</label>";
                    html += $@"<input type=""hidden"" id=""{ModelName}_{i}__value"" name=""{ModelName}[{i}].Value"" value=""{item.Value}"">";
                    html += $@"<input type=""hidden"" id=""{ModelName}_{i}__text"" name=""{ModelName}[{i}].Text"" value=""{item.Text}"">";
                }
                else
                {
                    html += $@"<label><input type=""checkbox"" {selected} {disabled} id=""{ModelName}_{i}__Selected"" name=""{ModelName}[{i}].Selected"" value=""true"" /> {item.Text}</label>";
                    html += $@"<input type=""hidden"" id=""{ModelName}_{i}__Value"" name=""{ModelName}[{i}].Value"" value=""{item.Value}"">";
                    html += $@"<input type=""hidden"" id=""{ModelName}_{i}__Text"" name=""{ModelName}[{i}].Text"" value=""{item.Text}"">";
                }

                html += "</li>";

                output.Content.AppendHtml(html);

                i++;
            }
            output.Content.AppendHtml("</ul>");
        }
    }
}
