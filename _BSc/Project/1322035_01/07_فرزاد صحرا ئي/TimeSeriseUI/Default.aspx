<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<%@ Register TagPrefix="uc1" TagName="UCSplineChart" Src="~/Controls/UCSplineChart.ascx" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>سری زمانی تک متغیره</h1>
        <h2>گام اول:</h2>
        <p class="lead">در این گام اطلاعات مورد نظر را به صورت CSV  در محل تعیین شده وارد نمایید.</p>
        <div class="row">
            <div class="col-md-2">
                مقادیر عملکرد
            </div>
            <div class="col-md-8">
                <asp:TextBox ID="txtInputvalues" runat="server" TextMode="MultiLine" Width="100%" Height="100px" />
            </div>
        </div>
        <h2>گام دوم:</h2>
        <p class="lead">با زدن دکمه تایید زیر نمودارهای تحلیلی دوره ها میانگین متحرک و اتورگرسیون و عملکردی فصلی اطلاعات را مشاهده می‌نمایید.</p>
        <p>
            <asp:Button ID="btnSubmitTrend" Text="تایید" CssClass="btn btn-primary btn-lg" OnClick="btnSubmitTrend_Click" Font-Bold="true" Width="100px" Height="20px" runat="server" />
        </p>
        <div class="row">
            <div class="col-md-12">
                <h2>روند فصلی و خطاهای روند فصلی</h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <uc1:UCSplineChart runat="server" ID="UCSplineChartResid" />
                <br />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h2>نمودار تحلیلی دوره ها میانگین متحرک و اتورگرسیون</h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:Image ID="imgPacf" runat="server" Width="100%" Visible="false" />
                <br />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h2>روند عملکرد</h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <uc1:UCSplineChart runat="server" ID="UCSplineChart" />
                <br />
            </div>
        </div>
        <h2>گام سوم:</h2>
        <p class="lead">در گام آخر با تعیین نوع سری زمانی و تعداد دوره‌های مورد نیاز، پیشبینی عملکرد شاخص مورد نظر در آینده را می توانید مشاهده نمایید.</p>

        <div class="row">
            <div class="col-md-2">
                انتخاب نوع سری زمانی
            </div>
            <div class="col-md-4">
                <asp:DropDownList ID="drpForecatsType" runat="server" Width="100%" />
            </div>
            <div class="col-md-2">
                تعداد پیشبینی
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="txtPredictCount" runat="server" Width="100%" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                تعداد دوره های اتورگرسیو
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="txtp" runat="server" Width="100%" />
            </div>
            <div class="col-md-2">
                تعداد دوره های میانگین متحرک
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="txtq" runat="server" Width="100%" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                تعداد دوره های فصلی
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="txts" runat="server" Width="100%" />
            </div>
            <div class="col-md-2">
                تعداد دوره های تفاضل
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="txtd" runat="server" Width="100%" />
            </div>
        </div>
        <p>
            <asp:Button ID="btnforcast" Text="پیشبینی" CssClass="btn btn-primary btn-lg" OnClick="btnForecast_Click" Font-Bold="true" Width="100px" Height="20px" runat="server" />
        </p>
        <div class="row">
            <div class="col-md-12">
                <h2>روند پیشبینی</h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <uc1:UCSplineChart runat="server" ID="UCSplineForcast" />
                <br />
            </div>
        </div>
        <div class="alert alert-danger" role="alert">
            <asp:Label ID="lbMessage" runat="server" />
        </div>


    </div>



</asp:Content>
