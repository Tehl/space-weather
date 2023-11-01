function getColorActual(value) {
  if (value >= 5) {
    return "#FF0000";
  }
  if (value >= 4) {
    return "#ffc600";
  }
  return "#00FF00";
}

function getColorForecast(value) {
  if (value >= 5) {
    return "#ff8080";
  }
  if (value >= 4) {
    return "#ffe380";
  }
  return "#80ff80";
}

function transform(data, colorise) {
  return data.map((item) => ({
    x: item.midpointUtc.toJSDate().valueOf(),
    y: item.value,
    color: (colorise && colorise(item.value)) || undefined,
  }));
}

export { getColorActual, getColorForecast, transform };
