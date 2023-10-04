import { CSSProperties } from "react";

const Constants = {
  ApiPrefix: "/api/v1",
  MultilineToast: { whiteSpace: "pre-line" } as CSSProperties,
  AriaLabels: {
    ToggleThemeButton: "toggle theme",
    ConfirmEmailTooltip: "confirm email",
    RemoveImage: "remove image",
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
  RedirectTimeout: 3000,
  CSS: {
    Inherit: "inherit",
    None: "none",
  },
  FileTypes: {
    Image: "image/*",
  },
  FileSizes: {
    MB: 1024 * 1024,
  },
  Strings: {
    Empty: "",
    ImagePlaceholder: "https://placehold.co/600x600?text=no+image",
  },
};

export default Constants;
