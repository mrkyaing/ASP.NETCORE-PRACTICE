#pragma checksum "G:\Projects\ASP.NETCORE-PRACTICE\HelloWorld\HelloWorld\Views\Home\Me.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a790a69b511f32303276c5412d78ac853ef07824"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Me), @"mvc.1.0.view", @"/Views/Home/Me.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a790a69b511f32303276c5412d78ac853ef07824", @"/Views/Home/Me.cshtml")]
    #nullable restore
    public class Views_Home_Me : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<style>
    form {
  background-color: #f2f2f2;
  padding: 20px;
  margin: 20px 0;
}

.form-group {
  margin: 20px 0;
}

 label, input[type=""text""], input[type=""email""], input[type=""password""], input[type=""date""], select {
  display: block;
  width: 100%;
  padding: 10px;
  font-size: 16px;
  box-sizing:border-box;
}

button[type=""submit""] {
  background-color: #4CAF50;
  color: white;
  padding: 14px 20px;
  margin: 8px 0;
  border: none;
  cursor: pointer;
  width: 100%;
}
</style>
    <form action=""/home/me"" method=""post"">
<fieldset style=""width:350px"">
    <legend style=""text-align:center"">Register Here</legend>
        <div class=""form-group""> <label>Name:</label><input type=""text"" /></div>
        <div class=""form-group"">   <label>Email:</label><input type=""text"" name=""txtemail"" /></div>
       <div class=""form-group"">Password:<input type=""password"" /></div>
        <div class=""form-group"">ConfirmPassword:<input type=""password"" /></div>
        <div class=""form-group""");
            WriteLiteral(@"> 
         <label>Gender:</label>
         <input type=""radio"" value=""Male"" checked />Male
         <input type=""radio"" value=""FeMale""  />Female</div>
        <div class=""form-group""> <label>Date Of Birth:</label><input type=""date"" /></div>
        <div class=""form-group"">
            <label>City:</label>    <select>
                <option value=""ygn"">Yangon</option>
                 <option value=""mdy"">Mandalay</option>
            </select></div>
        <div class=""form-group""> <label>Address:</label><textarea cols=""50"" rows=""5""></textarea></div>
        <div class=""form-group"" style=""text-align:center"">
            <button type=""submit""/>Register
            </div>
        <div class=""form-group"">
");
#nullable restore
#line 52 "G:\Projects\ASP.NETCORE-PRACTICE\HelloWorld\HelloWorld\Views\Home\Me.cshtml"
                 if (@ViewBag.Msg != null)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                  <label>   You are successfully register with\r\n                    ");
#nullable restore
#line 55 "G:\Projects\ASP.NETCORE-PRACTICE\HelloWorld\HelloWorld\Views\Home\Me.cshtml"
               Write(ViewBag.Msg);

#line default
#line hidden
#nullable disable
            WriteLiteral(" address.</label>                   \r\n");
#nullable restore
#line 56 "G:\Projects\ASP.NETCORE-PRACTICE\HelloWorld\HelloWorld\Views\Home\Me.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </div>\r\n</fieldset>\r\n</form>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
