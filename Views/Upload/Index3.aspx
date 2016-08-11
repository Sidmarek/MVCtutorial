<%@ Page Language="C#" MasterPageFile="~/Views/Shared/master.master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content id="content" ContentPlaceHolderID="contentBody" runat="server">

<form id="form1" runat="server">
<div>
    <asp:FileUpload runat="server" ID="fuTest" /><br />
    <asp:Button runat="server" ID="btnUpload" Text="Upload" PostBackUrl="~/Upload/Upload.aspx" />
</div>
</form>
</asp:content>    