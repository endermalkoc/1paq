<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="Default.aspx.cs" Inherits="onepaq._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="style/main.css" rel="stylesheet" type="text/css" />
    <link href="style/container.css" rel="stylesheet" type="text/css" />
    <link href="style/youtube.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <table class="pagemaster" cellspacing="0" cellpadding="0">
            <tr valign="top">
                <td>
                    <table cellspacing="0" cellpadding="2" width="100%" bgcolor="#999999" height="24px" style="border-bottom: 1px solid #666666;">
                        <tr>
                            <td width="50%">&nbsp;</td>
                            <td align="right" width="50%"></td>
                        </tr>
                    </table>
                    <table cellspacing="0" cellpadding="2" width="100%">
                        <tr valign="top">
                            <td id="TopMenu" width="100%">
                                <asp:Label ID="lblTitle" runat="server" Text="" CssClass="Title" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            <tr>
                <td height="100%">
                    <table height="100%" cellspacing="0" cellpadding="2" width="100%">
                        <tr>
                            <td id="Td1" style="width: 200px;" valign="top" align="middle" runat="server" 
                                rowspan="2">
                                <asp:Panel ID="pnlLeftPane" runat="server" />
                            </td>
                            <td>
                                <table height="100%" cellspacing="0" cellpadding="2" width="100%">
                                    <tr>
                                        <td id="Td2" align="left" valign="top" runat="server" colspan="2">
                                            <asp:Panel ID="pnlTopPane" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="ContentPane" valign="top" runat="server" width="400px">
                                            <asp:Panel ID="pnlCenterPane" runat="server" />
                                        </td>
                                        <td id="Td3" valign="top" runat="server">
                                            <asp:Panel ID="pnlRightPane" runat="server" />                                
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="Td4" class="BottomPane" align="left" runat="server">
                                            <asp:Panel ID="pnlBottomPane" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" width="100%">
                        <tr>
                            <td align="left" bgcolor="#999999" height="30px" style="border-top: 1px solid #666666; border-bottom: 1px solid #666666;">&nbsp;
                                Copyright
                                &nbsp;
                                Terms
                                &nbsp;
                                Privacy
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>




