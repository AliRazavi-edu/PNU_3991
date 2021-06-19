Imports op = DotNet.Highcharts.Options
Imports DotNet.Highcharts.Enums
Imports DotNet.Highcharts.Options
Imports DotNet.Highcharts.Helpers
Imports DotNet.Highcharts.Extensions
Imports DotNet.Highcharts
Imports System.Text
Imports System.Runtime.CompilerServices
Imports System.Security.Permissions
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web

<AspNetHostingPermission(SecurityAction.Demand, Level:=AspNetHostingPermissionLevel.Minimal)>
<AspNetHostingPermission(SecurityAction.InheritanceDemand, Level:=AspNetHostingPermissionLevel.Minimal)>
Public Class DotNetHighcharts
    Inherits WebControl

#Region " Properties "

    'Public Property JsVariables As IDictionary(Of String, String)
    'Public Property JsFunctions As IDictionary(Of String, String)
    Public Property Options As GlobalOptions
    Public Property FunctionName As String
    Public Property OptionsString As String

    Public Property Chart As New op.Chart
    Public Property Credits As New op.Credits
    Public Property Labels As New op.Labels
    Public Property Legend As New op.Legend
    Public Property Loading As New op.Loading
    Public Property PlotOptions As New op.PlotOptions
    Public Property Pane As New op.Pane
    Public Property PaneArray As List(Of op.Pane)
    Public Property Series As New op.Series
    Public Property SeriesArray As New List(Of op.Series)
    Public Property Subtitle As New op.Subtitle
    Public Property Title As New op.Title
    Public Property ChartTooltip As New op.Tooltip
    Public Property XAxis As New op.XAxis
    Public Property XAxisArray As New List(Of op.XAxis)
    Public Property YAxis As New op.YAxis
    Public Property YAxisArray As New List(Of op.YAxis)
    Public Property Exporting As New op.Exporting
    Public Property Navigation As New op.Navigation
    Public Property Lang As New op.Lang
    'Protected Property Base As BaseControl
    Public ReadOnly Property Name As String
        Get
            Return Me.ID
        End Get
    End Property

    Public ReadOnly Property ContainerName As String
        Get
            Return Me.UniqueID
            Return "{0}_container".FormatWith(Me.Name)
        End Get
    End Property

#End Region

    Public Sub New()
        'JsVariables = New Dictionary(Of String, String)()
        'JsFunctions = New Dictionary(Of String, String)()

        '     With Me.Credits
        '.Enabled = True
        '.Text = "تاژان سیستم"
        '.Href = "http://www.tajan.ir"
        ' .Style = "'padding-button' : '10px'"
        '.Position = (New Position With {.Y = "-15"})
        'End With
        'Me.Base = New BaseControl(Me, Me.ViewState)
        With Me.Exporting
            .Url = "SvgExporter.axd"
            .Width = 1200
            .Filename = "Chart"
        End With

        With Me.Lang
            .ContextButtonTitle = "تولید فایل خروجی"
            .DownloadJPEG = "دریافت فایل تصویری"
            .DownloadSVG = "" 'hide svg button
            .DownloadPDF = "" 'hide pdf button
            .DownloadPNG = "" 'hide png button
            .Loading = "در حال بارگزاری"
            .PrintChart = "چاپ"
            .ResetZoom = "حذف بزرگنمایی"
            .ResetZoomTitle = "حذف بزرگنمایی"
        End With

        Me.Title.UseHTML = True
        With Me.XAxis
            .Title = New op.XAxisTitle
            .Labels = New op.XAxisLabels
            .Title.Style = "fontSize: '16px'"
            .Labels.Style = "fontSize: '16px'"
            '.Labels.Formatter = "function(){return this.value.toString().toPersianDigit()}"
        End With

        With Me.YAxis
            .Title = New op.YAxisTitle
            .Labels = New op.YAxisLabels
            .Title.Style = "fontSize: '16px'"
            .Labels.Style = "fontSize: '14px'"
            ' .Labels.Formatter = "function(){return getNumberUnit(this.value).toString().toPersianDigit()}"
        End With


        With (Me.Legend)
            .UseHTML = True
            .LabelFormatter = " function () { return this.name + ' ';}"
        End With

        With Me.Chart
            .Style = "fontFamily: 'B Nazanin ,Nazanin, B Zar , Zar, Tahoma'"
            .Shadow = True
            .ZoomType = ZoomTypes.X
        End With

        Dim defaultTooltipItems = <xml>
                                      <tr>
                                          <td class=<%= CssClassNames.HIGHCHARTS_TOOLTIP_TEXT %> style="color: {series.color}">{series.name}:</td>
                                          <td class=<%= CssClassNames.HIGHCHARTS_TOOLTIP_PIE_VALUE %>>{point.y:.2f}</td>
                                      </tr>
                                  </xml>

        Dim pieTooltopItems = <xml>
                                  <tr>
                                      <td class=<%= CssClassNames.HIGHCHARTS_TOOLTIP_PIE_HEADER %> style="color: {series.color}" colspan="2">{point.name}</td>
                                  </tr>
                                  <tr>
                                      <td class=<%= CssClassNames.HIGHCHARTS_TOOLTIP_PIE_TEXT %>>مقدار:</td>
                                      <td class=<%= CssClassNames.HIGHCHARTS_TOOLTIP_PIE_VALUE %>>{point.y:.2f}</td>
                                  </tr>
                                  <tr>
                                      <td class=<%= CssClassNames.HIGHCHARTS_TOOLTIP_PIE_TEXT %>>درصد:</td>
                                      <td class=<%= CssClassNames.HIGHCHARTS_TOOLTIP_PIE_PERCENT %>>{point.percentage:.1f} %</td>
                                  </tr>
                              </xml>

        Dim defaultTooltipTable = "<table class=""" & CssClassNames.HIGHCHARTS_TOOLTIP_HOLDER & """>"
        Dim defaultTooltipHeader = "<tr><td class=""" & CssClassNames.HIGHCHARTS_TOOLTIP_HEADER & """ colspan=""2"">{point.x}</td></tr>"

        With Me.ChartTooltip
            .UseHTML = True
            .BorderWidth = 2
            .ValueDecimals = 2
            .BorderRadius = 20
            .HeaderFormat = defaultTooltipTable & defaultTooltipHeader
            .PointFormat = defaultTooltipItems.ToString.Replace("<xml>", "").Replace("</xml>", "")
            .FooterFormat = "</table>"

        End With
        'With {.DataLabels = New PlotOptionsPieDataLabels With {.Formatter = "function(){return this.point.percentage.toString(.toPersianDigit()}"}}
        With Me.PlotOptions
            .Column = New PlotOptionsColumn
            .Pie = New PlotOptionsPie
            .Gauge = New PlotOptionsGauge With {.EnableMouseTracking = False} 'disable tooltip over guage
            .Bar = New PlotOptionsBar
            .Scatter = New PlotOptionsScatter
            .Spline = New PlotOptionsSpline
            .Line = New PlotOptionsLine
            .Area = New PlotOptionsArea
        End With

        Me.PlotOptions.Pie.Tooltip = New PlotOptionsPieTooltip() With {.PointFormat = pieTooltopItems.ToString.Replace("<xml>", "").Replace("</xml>", ""),
                                                                       .HeaderFormat = defaultTooltipTable}




    End Sub


    Private OptionsBuilder As StringBuilder
    Private Function AddOptions(ByVal key As String, ByVal obj As Object, Optional ByVal addBraces As Boolean = False) As Boolean
        If obj Is Nothing Then Return False
        Dim json As String
        Try
            json = JsonSerializer.Serialize(obj)
        Catch ex As Exception
            Throw New Exception("Error in serializing " & key)
        End Try
        If json.Length < 4 Then Return False
        Me.OptionsBuilder.AppendLine(", ")
        If addBraces Then
            Me.OptionsBuilder.Append(key & ": [{0}]".FormatWith(json), 2)
        Else
            Me.OptionsBuilder.Append(key & ": {0}".FormatWith(json), 2)
        End If
        Return True
    End Function

    Private Function GetOptions() As String

        If Not String.IsNullOrEmpty(Me.OptionsString) Then
            Return Me.OptionsString.Replace("chart: {", "chart: {  renderTo: " & Me.ContainerName & ",")
        End If

        OptionsBuilder = New StringBuilder()

        OptionsBuilder.Append(If(Me.Chart IsNot Nothing,
                          "chart: {{ renderTo:'{0}', {1} }}".FormatWith(Me.ContainerName, JsonSerializer.Serialize(Me.Chart, False)),
                          "chart: {{ renderTo:'{0}' }}".FormatWith(Me.ContainerName)))


        AddOptions("lang", Me.Lang)


        AddOptions("credits", Me.Credits)
        'If Me.Credits IsNot Nothing Then
        '    OptionsBuilder.AppendLine(", ")
        '    OptionsBuilder.Append("credits: {0}".FormatWith(json), 2)
        'End If

        AddOptions("labels", Me.Labels)
        'If Me.Labels IsNot Nothing Then
        '    OptionsBuilder.AppendLine(", ")
        '    OptionsBuilder.Append("labels: {0}".FormatWith(JsonSerializer.Serialize(Me.Labels)), 2)
        'End If

        AddOptions("legend", Me.Legend)
        'If Me.Legend IsNot Nothing Then
        '    options.AppendLine(", ")
        '    options.Append("legend: {0}".FormatWith(JsonSerializer.Serialize(Me.Legend)), 2)
        'End If

        AddOptions("loading", Me.Loading)
        'If Me.Loading IsNot Nothing Then
        '    options.AppendLine(", ")
        '    options.Append("loading: {0}".FormatWith(JsonSerializer.Serialize(Me.Loading)), 2)
        'End If

        AddOptions("plotOptions", Me.PlotOptions)
        'If Me.PlotOptions IsNot Nothing Then
        '        options.AppendLine(", ")
        '        options.Append("plotOptions: {0}".FormatWith(JsonSerializer.Serialize(Me.PlotOptions)), 2)
        'End If

        If Not AddOptions("pane", Me.Pane) Then
            AddOptions("pane", Me.PaneArray)
        End If
        'If Me.Pane IsNot Nothing Then
        '    options.AppendLine(", ")
        '    options.Append("pane: {0}".FormatWith(JsonSerializer.Serialize(Me.Pane)), 2)
        'End If

        'If Me.PaneArray IsNot Nothing AndAlso Me.PaneArray.Count > 0 Then
        '    options.AppendLine(", ")
        '    options.Append("pene: {0}".FormatWith(JsonSerializer.Serialize(Me.PaneArray)), 2)
        'End If

        AddOptions("subtitle", Me.Subtitle)
        'If Me.Subtitle IsNot Nothing Then
        '    options.AppendLine(", ")
        '    options.Append("subtitle: {0}".FormatWith(JsonSerializer.Serialize(Me.Subtitle)), 2)
        'End If

        AddOptions("title", Me.Title)
        'If Me.Title IsNot Nothing Then
        '    options.AppendLine(", ")
        '    options.Append("title: {0}".FormatWith(JsonSerializer.Serialize(Me.Title)), 2)
        'End If

        AddOptions("exporting", Me.Exporting)
        'If Me.Exporting IsNot Nothing Then
        '{exportButton: { menuItems: [,,null,] } }
        'OptionsBuilder.AppendLine(", ")
        'OptionsBuilder.Append("exporting: {0}".FormatWith(JsonSerializer.Serialize(Me.Exporting)), 2)
        'End If

        AddOptions("navigation", Me.Navigation)
        'If Me.Navigation IsNot Nothing Then
        '    options.AppendLine(", ")
        '    options.Append("navigation: {0}".FormatWith(JsonSerializer.Serialize(Me.Navigation)), 2)
        'End If

        If Me.ChartTooltip IsNot Nothing Then

            'If Chart.Type = ChartTypes.Column OrElse Chart.Type = ChartTypes.Line Then
            '    Me.ChartTooltip.Formatter = "null"

            '    If Chart.Type = ChartTypes.Column Then
            '        Me.ChartTooltip.Shared = True
            '    End If

            'End If
            If Chart.Type = ChartTypes.Column Then
                Me.ChartTooltip.Shared = True
            End If

            AddOptions("tooltip", Me.ChartTooltip)
            'options.AppendLine(", ")
            'options.Append("tooltip: {0}".FormatWith(JsonSerializer.Serialize(Me.ChartTooltip)), 2)
        End If

        If Me.XAxis IsNot Nothing AndAlso Me.XAxis.Categories IsNot Nothing AndAlso Me.XAxis.Categories.Length > 0 Then
            AddOptions("xAxis", Me.XAxis)
            'options.AppendLine(", ")
            'options.Append("xAxis: {0}".FormatWith(JsonSerializer.Serialize(Me.XAxis)), 2)
        ElseIf Me.XAxisArray IsNot Nothing AndAlso Me.XAxisArray.Count > 0 Then
            AddOptions("xAxis", Me.XAxisArray.ToArray)
            'options.AppendLine(", ")
            'options.Append("xAxis: {0}".FormatWith(JsonSerializer.Serialize(Me.XAxisArray.ToArray)), 2)
        End If

        If Me.YAxis IsNot Nothing Then
            AddOptions("yAxis", Me.YAxis)
            'options.AppendLine(", ")
            'options.Append("yAxis: {0}".FormatWith(JsonSerializer.Serialize(Me.YAxis)), 2)
        ElseIf Me.YAxisArray IsNot Nothing AndAlso Me.YAxisArray.Count > 0 Then
            AddOptions("yAxis", Me.YAxisArray.ToArray)
            'options.AppendLine(", ")
            'options.Append("yAxis: {0}".FormatWith(JsonSerializer.Serialize(Me.YAxisArray.ToArray)), 2)
        End If


        If Me.Series IsNot Nothing AndAlso Me.Series.Data IsNot Nothing Then
            AddOptions("series", Me.Series, True)
            'options.AppendLine(", ")
            'options.Append("series: [{0}]".FormatWith(JsonSerializer.Serialize(Me.Series)), 2)
        ElseIf Me.SeriesArray IsNot Nothing And Me.SeriesArray.Count > 0 Then
            AddOptions("series", Me.SeriesArray.ToArray)
            'options.AppendLine(", ")
            'options.Append("series: {0}".FormatWith(JsonSerializer.Serialize(Me.SeriesArray.ToArray)), 2)
        End If

        OptionsBuilder.AppendLine()

        Return OptionsBuilder.ToString()
    End Function

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)

        Dim scripts As New StringBuilder()

        scripts.AppendLine("<div id='{0}' class='tj-dotnet-highcharts'></div>".FormatWith(ContainerName))
        scripts.AppendLine("<script type='text/javascript'>")
        If Options IsNot Nothing Then
            scripts.AppendLine("Highcharts.setOptions({0});".FormatWith(JsonSerializer.Serialize(Options)))
        End If

        scripts.AppendLine("var {0};".FormatWith(Name))
        scripts.AppendLine(If(Not String.IsNullOrEmpty(FunctionName), String.Format("function {0}() {{", FunctionName), "$(document).ready(function() {"))
        AppendHighchart(scripts)
        scripts.AppendLine(If(Not String.IsNullOrEmpty(FunctionName), "}", "});"))

        scripts.AppendLine("</script>")

        writer.Write(scripts.ToString())

        MyBase.Render(writer)

    End Sub

    Private Sub AppendHighchart(ByVal sb As StringBuilder)
        'For Each jsVariable In Me.JsVariables
        '    sb.AppendLine("var {0} = {1};".FormatWith(jsVariable.Key, jsVariable.Value), 1)
        'Next

        sb.AppendLine(Convert.ToString(Me.Name) & " = new Highcharts.Chart({", 1)
        sb.Append(GetOptions(), 2)
        sb.AppendLine("});", 1)

        'For Each jsFunction In Me.JsFunctions
        '    sb.AppendLine()
        '    sb.AppendLine(jsFunction.Key, 1)
        '    sb.AppendLine(jsFunction.Value, 2)
        '    sb.AppendLine("}", 1)
        'Next
    End Sub



    Private Sub DotNetHighcharts_Init(sender As Object, e As EventArgs) Handles Me.Init

        If String.IsNullOrWhiteSpace(Me.Name) Then
            Throw New ArgumentException("The name of the chart must be specified.")
        End If

        Dim highChartLib = AppSettings.LibraryPath("highchart/")
        'Me.Page.RegisterScriptFile(ScriptKeys.JQuery, AppSettings.LibraryPath & "jquery/jquery.js")
        'Me.Page.RegisterScriptFile(ScriptKeys.Highcharts, highChartLib & "highcharts.js")
        'Me.Page.RegisterScriptFile(ScriptKeys.Highcharts_More, highChartLib & "highcharts-more.js")
        'Me.Page.RegisterScriptFile(ScriptKeys.Highcharts_Exporting, highChartLib & "modules/exporting.js")

        'Me.Page.RegisterStyleFile(ScriptKeys.Highcharts_Style, AppSettings.CssPath & "dotnet-highcharts.css")

    End Sub
End Class

