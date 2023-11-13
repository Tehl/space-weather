import { DateTime } from "luxon";
import { ApiClient } from "./api";
import { parseIndexReading } from "./data/magneticIndex";
import {
  getColorActual,
  getColorForecast,
  transform,
} from "./chart/magneticIndex";
import { render } from "./chart/render";

if (module.hot) {
  module.hot.accept();
}

document.addEventListener("DOMContentLoaded", async function () {
  const apiClient = new ApiClient("/api");

  const now = DateTime.utc();
  const startTime = now.minus({ days: 3 }).startOf("day");
  const endTime = now.plus({ days: 3 }).endOf("day");

  const actual = (
    await apiClient.getMagneticIndexReadingsAsync(
      "Planetary",
      "K",
      startTime,
      now,
    )
  ).map(parseIndexReading);

  const forecast = (
    await apiClient.getMagneticIndexReadingsAsync(
      "Forecast",
      "K",
      actual[actual.length - 1].endTimeUtc,
      endTime,
    )
  ).map(parseIndexReading);

  const indexData = transform(actual, getColorActual).concat(
    transform(forecast, getColorForecast),
  );

  render(startTime, endTime, now, indexData);
});
