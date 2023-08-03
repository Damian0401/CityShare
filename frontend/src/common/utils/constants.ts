import { CSSProperties } from "react";

const Constants = {
  ApiPrefix: "/api/v1",
  ThemeButtonLabel: "toggle theme",
  MultilineToast: { whiteSpace: "pre-line" } as CSSProperties,
  Leaflet: {
    Attribution:
      "&copy; <a href='https://www.openstreetmap.org/copyright'>OpenStreetMap</a> contributors",
    Url: "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png",
    Zoom: {
      Default: 13,
      Search: 16,
    },
  },
};

export default Constants;
