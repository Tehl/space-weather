class ApiClient {
  constructor(baseUrl) {
    this.baseUrl = baseUrl;
  }

  async getMagneticIndexReadingsAsync(
    station,
    type,
    fromTimestamp,
    toTimestamp,
  ) {
    const searchParams = new URLSearchParams({
      fromTimestamp,
      toTimestamp,
    });
    const res = await fetch(
      `${this.baseUrl}/MagneticIndexReadings/${station}/${type}?${searchParams}`,
    );
    if (!res.ok) {
      throw new Error(`Request failed with status: ${res.status}`);
    }
    return await res.json();
  }
}

export { ApiClient };
