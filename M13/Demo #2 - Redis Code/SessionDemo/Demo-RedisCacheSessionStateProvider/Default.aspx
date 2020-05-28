<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Demo_RedisCacheSessionStateProvider.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>Insert into Session</td>
                    <td>
                        <asp:TextBox ID="txtinput" runat="server"></asp:TextBox></td>
                    <td>
                        <asp:Button ID="btnPut" runat="server" Text="Add To Session" OnClick="btnPut_Click" /></td>
                </tr>
                <tr>
                    <td>Retrieve From Session</td>
                    <td>
                        <asp:TextBox ID="txtoutput" ReadOnly="true" Enabled="false" runat="server"></asp:TextBox></td>
                    <td>
                        <asp:Button ID="btnGet" runat="server" Text="Retrieve From Session" OnClick="btnGet_Click" /></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
