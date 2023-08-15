import { CSSProperties } from "react";

const Constants = {
  ApiPrefix: "/api/v1",
  MultilineToast: { whiteSpace: "pre-line" } as CSSProperties,
  AriaLabels: {
    ToggleThemeButton: "toggle theme",
    ConfirmEmailTooltip: "confirm email",
  },
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
