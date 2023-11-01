import { DateTime } from "luxon";

function midpoint(time1, time2) {
  return time1.plus({ milliseconds: time2.diff(time1).milliseconds / 2 });
}

function parse(raw) {
  const reading = {
    ...raw,
    startTimeUtc: DateTime.fromISO(raw.startTimeUtc),
    endTimeUtc: DateTime.fromISO(raw.endTimeUtc),
  };

  reading.midpointUtc = midpoint(reading.startTimeUtc, reading.endTimeUtc);

  return reading;
}

export { parse as parseIndexReading };
