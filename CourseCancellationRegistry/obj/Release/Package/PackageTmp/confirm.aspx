<%@ Page Title="" Language="C#" MasterPageFile="~/Registry.Master" AutoEventWireup="true" CodeBehind="confirm.aspx.cs" Inherits="CourseCancellationRegistry.confirm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Confirmation</h2>
    Are you sure you want to <asp:Label ID="ActionLabel" runat="server" 
        Text="Delete/Add" EnableViewState="False"></asp:Label> this course 
    <asp:Label ID="ToFromLabel" runat="server" Text="to/from" 
        EnableViewState="False"></asp:Label> your list?
    <br /><br />
    <h3 id="CourseTitle" runat="server" enableviewstate="False">CourseLabel</h3>
    <br />
    Class Schedule:
    <p id="CourseTimesContent" runat="server" enableviewstate="False"></p>
    <br /><br />
    <asp:Button ID="ConfirmButton" runat="server" Text="Confirm" 
        onclick="ConfirmButton_Click" Width="75px" />&nbsp;
    <asp:Button ID="CancelButton"
        runat="server" Text="Cancel" onclick="CancelButton_Click" Width="75px" />
</asp:Content>
