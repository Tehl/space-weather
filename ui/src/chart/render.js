import Highcharts from "highcharts";

function render(startTime, endTime, now, indexData) {
  Highcharts.chart("container", {
    chart: {
      alignTicks: false,
    },
    title: {
      text:
        "K-index from " +
        startTime.toLocaleString() +
        " to " +
        endTime.toLocaleString(),
    },
    xAxis: {
      type: "datetime",
      title: {
        text: "Date",
      },
      crosshair: true,
      min: startTime.toJSDate().valueOf(),
      max: endTime.toJSDate().valueOf(),
      plotLines: [
        {
          color: "#000",
          width: 1,
          value: now.toJSDate().valueOf(),
          zIndex: 5,
        },
      ],
    },
    yAxis: [
      {
        title: {
          text: "K-index",
        },
        min: 0,
        max: 9,
        endOnTick: false,
        tickInterval: 2,
        gridLineDashStyle: "LongDash",
      },
    ],
    series: [
      {
        name: "Planetary",
        data: indexData,
        type: "column",
        pointPadding: 0.1,
        groupPadding: 0,
        borderWidth: 0,
      },
    ],
  });
}

export { render };
