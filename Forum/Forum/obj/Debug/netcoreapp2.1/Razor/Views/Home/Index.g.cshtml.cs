#pragma checksum "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "79638c682a486e07d4077a55672e1b5abce10b9b"
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
using Forum.Web.ViewModels.Home;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"79638c682a486e07d4077a55672e1b5abce10b9b", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2ac609fd15eba99a48942b04c8579a10a24406fb", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IndexInfoViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(63, 203, true);
            WriteLiteral("\r\n    <div id=\"shoutbox\" class=\"menu-bg-forum text-center\">\r\n        <h1><a id=\"main-heading-text\" href=\"#\">Powered by MPF-Team</a> </h1>\r\n    </div>\r\n    <div class=\"content-container\">\r\n        <div>\r\n");
            EndContext();
#line 9 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
             foreach (var category in Model.Categories)
            {

#line default
#line hidden
            BeginContext(338, 64, true);
            WriteLiteral("            <div class=\"forum-container \">\r\n                <h2>");
            EndContext();
            BeginContext(403, 13, false);
#line 12 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
               Write(category.Name);

#line default
#line hidden
            EndContext();
            BeginContext(416, 115, true);
            WriteLiteral("</h2>\r\n                <div class=\"forum-information-container\">\r\n                    <div class=\"forum-content\">\r\n");
            EndContext();
#line 15 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                         if (!category.Forums.Any())
                        {

#line default
#line hidden
            BeginContext(610, 526, true);
            WriteLiteral(@"<div class=""post-image"">
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
#line 28 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"

                        }
                        else
                        {
                            

#line default
#line hidden
#line 32 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                             foreach (var forum in category.Forums)
                            {

#line default
#line hidden
            BeginContext(1322, 475, true);
            WriteLiteral(@"                                <div class=""post-image"">
                                    <i class=""fas fa-clone fa-lg forum-icon""></i>
                                </div>
                                <div class=""post-title"">
                                    <div class=""post-title"">
                                        <div class=""content-container"">
                                            <div>
                                                <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 1797, "\"", 1829, 2);
            WriteAttributeValue("", 1804, "/Forum/Posts?Id=", 1804, 16, true);
#line 41 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
WriteAttributeValue("", 1820, forum.Id, 1820, 9, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1830, 5, true);
            WriteLiteral("><h3>");
            EndContext();
            BeginContext(1836, 10, false);
#line 41 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                                                                   Write(forum.Name);

#line default
#line hidden
            EndContext();
            BeginContext(1846, 514, true);
            WriteLiteral(@"</h3></a>
                                            </div>
                                            <div>
                                                <div>
                                                    <p style=""font-size: 14px; margin:8px 0px;"">Total views: 0</p>
                                                </div>
                                                <div style=""margin:0"">
                                                    <p style=""font-size: 14px; margin:0"">Started on: ");
            EndContext();
            BeginContext(2361, 38, false);
#line 48 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                                                                                Write(forum.CreatedOn.ToString("dd-mm-yyyy"));

#line default
#line hidden
            EndContext();
            BeginContext(2399, 246, true);
            WriteLiteral("</p>\r\n                                                </div>\r\n                                            </div>\r\n                                        </div>\r\n                                    </div>\r\n                                </div>\r\n");
            EndContext();
#line 54 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                            }

#line default
#line hidden
#line 54 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                             
                        }

#line default
#line hidden
            BeginContext(2703, 72, true);
            WriteLiteral("                    </div>\r\n                </div>\r\n            </div>\r\n");
            EndContext();
#line 59 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
            }

#line default
#line hidden
            BeginContext(2790, 3009, true);
            WriteLiteral(@"        </div>

        <div>
            <div class=""forum-container"">
                <h2>Latest Topics</h2>
                <div class=""forum-information-container menu-bg-forum"">

                    <div class=""forum-content"">
                        <div class=""post-image"">
                            <i class=""fas fa-user""></i>
                        </div>
                        <div class=""post-title"">
                            <h4>Csharp Tutorials</h4>
                            <p style=""display: inline; font-size: 14px; margin:0"">by pesho</p>
                        </div>
                        <div class=""post-image"">
                            <i class=""fas fa-user""></i>
                        </div>
                        <div class=""post-title"">
                            <h4>Java Tutorials</h4>
                            <p style=""display: inline; font-size: 14px; margin:0"">by ivan</p>
                        </div>
                        <div class=""post-im");
            WriteLiteral(@"age"">
                            <i class=""fas fa-user""></i>
                        </div>
                        <div class=""post-title"">
                            <h4>Php Tutorials</h4>
                            <p style=""display: inline; font-size: 14px; margin:0"">by ivan</p>
                        </div>
                    </div>

                </div>
            </div>
            <div class=""forum-container"">
                <h2>Popular Topics</h2>
                <div class=""forum-information-container menu-bg-forum"">

                    <div class=""forum-content"">
                        <div class=""post-image"">
                            <i class=""fas fa-user""></i>
                        </div>
                        <div class=""post-title"">
                            <h4>Csharp Tutorials</h4>
                            <p style=""display: inline; font-size: 14px; margin:0"">Total views: 232</p>
                        </div>
                        <div class=""p");
            WriteLiteral(@"ost-image"">
                            <i class=""fas fa-user""></i>
                        </div>
                        <div class=""post-title"">
                            <h4>Java Tutorials</h4>
                            <p style=""display: inline; font-size: 14px; margin:0"">Total views: 232</p>
                        </div>
                        <div class=""post-image"">
                            <i class=""fas fa-user""></i>
                        </div>
                        <div class=""post-title"">
                            <h4>Php Tutorials</h4>
                            <p style=""display: inline; font-size: 14px; margin:0"">Total views: 232</p>
                        </div>
                    </div>

                </div>
            </div>
        </div>

    </div>

    <div id=""info"" class=""two-thirds-width mx-auto"">
        <div class=""info-block"">
            <p class=""text-forum"">Newest Member: ");
            EndContext();
            BeginContext(5800, 16, false);
#line 129 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                            Write(Model.NewestUser);

#line default
#line hidden
            EndContext();
            BeginContext(5816, 103, true);
            WriteLiteral("</p>\r\n        </div>\r\n        <div class=\"info-block\">\r\n            <p class=\"text-forum\">Total Posts: ");
            EndContext();
            BeginContext(5920, 21, false);
#line 132 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                          Write(Model.TotalPostsCount);

#line default
#line hidden
            EndContext();
            BeginContext(5941, 103, true);
            WriteLiteral("</p>\r\n        </div>\r\n        <div class=\"info-block\">\r\n            <p class=\"text-forum\">Total users: ");
            EndContext();
            BeginContext(6045, 21, false);
#line 135 "C:\Users\Georgi-PC\Documents\Visual Studio 2017\Projects\Forum\Forum\Views\Home\Index.cshtml"
                                          Write(Model.TotalUsersCount);

#line default
#line hidden
            EndContext();
            BeginContext(6066, 32, true);
            WriteLiteral("</p>\r\n        </div>\r\n    </div>");
            EndContext();
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
