#pragma checksum "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8ac7e12b54414a3850f0b82d7f83a1f20be93f56"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\_ViewImports.cshtml"
using Forum;

#line default
#line hidden
#line 2 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\_ViewImports.cshtml"
using Forum.Models;

#line default
#line hidden
#line 1 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
using Forum.ViewModels.Home;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8ac7e12b54414a3850f0b82d7f83a1f20be93f56", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2ac609fd15eba99a48942b04c8579a10a24406fb", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IndexInfoViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/toggle.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(59, 183, true);
            WriteLiteral("\r\n<div id=\"shoutbox\" class=\"menu-bg-forum text-center\">\r\n    <h1><a id=\"main-heading-text\" href=\"#\">Powered by MPF-Team</a> </h1>\r\n</div>\r\n<div class=\"content-container\">\r\n    <div>\r\n");
            EndContext();
#line 9 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
         foreach (var category in Model.Categories)
        {

#line default
#line hidden
            BeginContext(306, 186, true);
            WriteLiteral("            <div class=\"forum-container\">\r\n                <a class=\"toggleBtn text-forum\"><i class=\"fas fa-caret-up float-right font-30 m-10px toggleIcon\"></i></a>\r\n                <h2>");
            EndContext();
            BeginContext(493, 13, false);
#line 13 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
               Write(category.Name);

#line default
#line hidden
            EndContext();
            BeginContext(506, 126, true);
            WriteLiteral("</h2>\r\n                <div class=\"forum-information-container toggle-div\">\r\n                    <div class=\"forum-content\">\r\n");
            EndContext();
#line 16 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                         if (!category.Forums.Any())
                        {

#line default
#line hidden
            BeginContext(713, 554, true);
            WriteLiteral(@"                            <div class=""post-image"">
                            </div>
                            <div class=""post-title"">

                                <div class=""post-title"">
                                    <div class=""content-container"">
                                        <div>
                                            <p>No subforums.</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
");
            EndContext();
#line 30 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                        }
                        else
                        {
                            

#line default
#line hidden
#line 33 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                             foreach (var forum in category.Forums)
                            {

#line default
#line hidden
            BeginContext(1451, 475, true);
            WriteLiteral(@"                                <div class=""post-image"">
                                    <i class=""fas fa-clone fa-lg forum-icon""></i>
                                </div>
                                <div class=""post-title"">
                                    <div class=""post-title"">
                                        <div class=""content-container"">
                                            <div>
                                                <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 1926, "\"", 1958, 2);
            WriteAttributeValue("", 1933, "/Forum/Posts?Id=", 1933, 16, true);
#line 42 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
WriteAttributeValue("", 1949, forum.Id, 1949, 9, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1959, 24, true);
            WriteLiteral("><h3 class=\"text-forum\">");
            EndContext();
            BeginContext(1984, 10, false);
#line 42 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                                                                                      Write(forum.Name);

#line default
#line hidden
            EndContext();
            BeginContext(1994, 169, true);
            WriteLiteral("</h3></a>\r\n                                            </div>\r\n                                            <div>\r\n                                                <div>\r\n");
            EndContext();
            BeginContext(2244, 109, true);
            WriteLiteral("                                                    <p style=\"font-size: 14px; margin:8px 0px;\">Total views: ");
            EndContext();
            BeginContext(2354, 29, false);
#line 47 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                                                                                        Write(forum.Posts.Sum(p => p.Views));

#line default
#line hidden
            EndContext();
            BeginContext(2383, 235, true);
            WriteLiteral("</p>\r\n                                                </div>\r\n                                                <div style=\"margin:0\">\r\n                                                    <p style=\"font-size: 14px; margin:0\">Created on: ");
            EndContext();
            BeginContext(2619, 38, false);
#line 50 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                                                                                Write(forum.CreatedOn.ToString("dd-MM-yyyy"));

#line default
#line hidden
            EndContext();
            BeginContext(2657, 246, true);
            WriteLiteral("</p>\r\n                                                </div>\r\n                                            </div>\r\n                                        </div>\r\n                                    </div>\r\n                                </div>\r\n");
            EndContext();
#line 56 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                            }

#line default
#line hidden
#line 56 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                             
                        }

#line default
#line hidden
            BeginContext(2961, 72, true);
            WriteLiteral("                    </div>\r\n                </div>\r\n            </div>\r\n");
            EndContext();
#line 61 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
        }

#line default
#line hidden
            BeginContext(3044, 360, true);
            WriteLiteral(@"    </div>

    <div>
        <div class=""forum-container"">
            <a class=""toggleBtn text-forum""><i class=""fas fa-caret-up float-right font-30 m-10px toggleIcon""></i></a>
            <h2 class=""font-18"">Latest Topics</h2>
            <div class=""forum-information-container menu-bg-forum toggle-div"">
                <div class=""forum-content"">
");
            EndContext();
#line 70 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                     foreach (var latestPost in Model.LatestPosts)
                    {

#line default
#line hidden
            BeginContext(3495, 219, true);
            WriteLiteral("                        <div class=\"post-image\">\r\n                            <i class=\"fas fa-user\"></i>\r\n                        </div>\r\n                        <div class=\"post-title\">\r\n                            <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 3714, "\"", 3752, 2);
            WriteAttributeValue("", 3721, "/Post/Details?id=", 3721, 17, true);
#line 76 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
WriteAttributeValue("", 3738, latestPost.Id, 3738, 14, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3753, 31, true);
            WriteLiteral("><h4 class=\"m-10px text-forum\">");
            EndContext();
            BeginContext(3785, 15, false);
#line 76 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                                                                               Write(latestPost.Name);

#line default
#line hidden
            EndContext();
            BeginContext(3800, 74, true);
            WriteLiteral("</h4></a>\r\n                            <p class=\"m-0 font-14 \">Started on ");
            EndContext();
            BeginContext(3875, 20, false);
#line 77 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                                          Write(latestPost.StartedOn);

#line default
#line hidden
            EndContext();
            BeginContext(3895, 61, true);
            WriteLiteral("</p>\r\n                            <p class=\"m-0 font-14 \">by ");
            EndContext();
            BeginContext(3957, 21, false);
#line 78 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                                  Write(latestPost.AuthorName);

#line default
#line hidden
            EndContext();
            BeginContext(3978, 38, true);
            WriteLiteral("</p>\r\n                        </div>\r\n");
            EndContext();
#line 80 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                    }

#line default
#line hidden
            BeginContext(4039, 396, true);
            WriteLiteral(@"                </div>
            </div>
        </div>
        <div class=""forum-container"">
            <a class=""toggleBtn text-forum""><i class=""fas fa-caret-up float-right font-30 m-10px toggleIcon""></i></a>
            <h2 class=""font-18"">Popular Topics</h2>
            <div class=""forum-information-container menu-bg-forum toggle-div"">
                <div class=""forum-content"">
");
            EndContext();
#line 89 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                     foreach (var popularPost in Model.PopularPosts)
                    {

#line default
#line hidden
            BeginContext(4528, 219, true);
            WriteLiteral("                        <div class=\"post-image\">\r\n                            <i class=\"fas fa-user\"></i>\r\n                        </div>\r\n                        <div class=\"post-title\">\r\n                            <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 4747, "\"", 4786, 2);
            WriteAttributeValue("", 4754, "/Post/Details?id=", 4754, 17, true);
#line 95 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
WriteAttributeValue("", 4771, popularPost.Id, 4771, 15, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(4787, 31, true);
            WriteLiteral("><h4 class=\"m-10px text-forum\">");
            EndContext();
            BeginContext(4819, 16, false);
#line 95 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                                                                                Write(popularPost.Name);

#line default
#line hidden
            EndContext();
            BeginContext(4835, 100, true);
            WriteLiteral("</h4></a>\r\n                            <p style=\"display: inline; font-size: 14px; margin:0\">Views: ");
            EndContext();
            BeginContext(4936, 17, false);
#line 96 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                                                                    Write(popularPost.Views);

#line default
#line hidden
            EndContext();
            BeginContext(4953, 38, true);
            WriteLiteral("</p>\r\n                        </div>\r\n");
            EndContext();
#line 98 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                    }

#line default
#line hidden
            BeginContext(5014, 205, true);
            WriteLiteral("                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n<div id=\"info\" class=\"two-thirds-width mx-auto\">\r\n    <div class=\"info-block\">\r\n        <p class=\"text-forum\">Newest Member: ");
            EndContext();
            BeginContext(5220, 16, false);
#line 106 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                        Write(Model.NewestUser);

#line default
#line hidden
            EndContext();
            BeginContext(5236, 91, true);
            WriteLiteral("</p>\r\n    </div>\r\n    <div class=\"info-block\">\r\n        <p class=\"text-forum\">Total Posts: ");
            EndContext();
            BeginContext(5328, 21, false);
#line 109 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                      Write(Model.TotalPostsCount);

#line default
#line hidden
            EndContext();
            BeginContext(5349, 91, true);
            WriteLiteral("</p>\r\n    </div>\r\n    <div class=\"info-block\">\r\n        <p class=\"text-forum\">Total users: ");
            EndContext();
            BeginContext(5441, 21, false);
#line 112 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                      Write(Model.TotalUsersCount);

#line default
#line hidden
            EndContext();
            BeginContext(5462, 26, true);
            WriteLiteral("</p>\r\n    </div>\r\n</div>\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(5505, 5, true);
                WriteLiteral("\r\n   ");
                EndContext();
                BeginContext(5510, 38, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "046eee3df3c64c7c959e2b659c51ba76", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(5548, 2, true);
                WriteLiteral("\r\n");
                EndContext();
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IndexInfoViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
