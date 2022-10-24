<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="Container.ascx.cs" Inherits="onepaq.Container" %>
<table width="100%" cellspacing="0" cellpadding="1">
    <tr>
        <td class="Redmond_NavigationTitleCell" width="100%">
            <asp:Label ID="lblTitle" CssClass="Redmond_NavigationTitle" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="4" class="Redmond_NavigationModule" id="Td2" runat="server">
            <asp:Literal ID="ltContent" runat="server" />
        </td>
    </tr>
</table>


