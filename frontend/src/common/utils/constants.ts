import { CSSProperties } from "react";

const Constants = {
  ApiPrefix: "/api/v1",
  ThemeButtonLabel: "toggle theme",
  MultilineToast: { whiteSpace: "pre-line" } as CSSProperties,
  LeafletAttribution:
    "&copy; <a href='https://www.openstreetmap.org/copyright'>OpenStreetMap</a> contributors",
  LeafletUrl: "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png",
};

export default Constants;
