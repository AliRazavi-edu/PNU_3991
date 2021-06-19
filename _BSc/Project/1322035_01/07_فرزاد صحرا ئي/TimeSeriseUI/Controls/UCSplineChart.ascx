<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCSplineChart.ascx.cs" Inherits="TimeSeriseUI.Controls.UCSplineChart" %> 
<div id="<%=ChartNewId%>" style="height:400px"></div>
<asp:HiddenField runat="server" ID="splinedata" />
<asp:HiddenField runat="server" ID="splinecategories" />


<script type="text/javascript">
    var sc = $("#<%= splinecategories.ClientID%>").val();
    var sd = $("#<%= splinedata.ClientID%>").val();
    if (sd !== "" && sc !== "") {
        var data = JSON.parse(sd);
        var categoriesVal = sc.split(',');

        Highcharts.setOptions({
            lang: {
                thousandsSep: ','
            }
        });

        Highcharts.chart('<%=ChartNewId%>', {
            chart: {
                type: 'line'
            },
            title: {
                text: ''
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                categories: categoriesVal,
                labels: {
                    //align: 'center',
                    useHTML: true,
                    style: {
                        paddingTop: '0'
                    }
                }
            },
            yAxis: {
                title: {
                    text: ''
                }
            },
            legend: {
                backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || 'white',
                borderColor: '#CCC',
                borderWidth: 1,
                shadow: true,
                layout: 'vertical',
                align: 'center'
                //verticalAlign: 'middle'
            },
            tooltip: {
                crosshairs: false,
                shared: false,
                headerFormat: "",
                pointFormat: '<b>{point.y}</b>'
            },
            plotOptions: {
                series: {
                    label: {
                        enabled: false
                    }
                }
            },
            series: data
        });
    }
    
</script>