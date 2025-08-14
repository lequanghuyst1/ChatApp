import { useMemo } from "react";
import merge from "lodash/merge";

import CssBaseline from "@mui/material/CssBaseline";
import {
  createTheme,
  ThemeOptions,
  ThemeProvider as MuiThemeProvider,
} from "@mui/material/styles";

// system
import { palette } from "./palette";
import { shadows } from "./shadows";
import { typography } from "./typography";
// options
import RTL from "./options/right-to-left";
import { customShadows } from "./custom-shadows";
import { componentsOverrides } from "./overrides";
import { createPresets } from "./options/presets";
import { createContrast } from "./options/contrast";

// ----------------------------------------------------------------------

type Props = {
  children: React.ReactNode;
};

export default function ThemeProvider({ children }: Props) {
  const presets = createPresets("cyan");

  const contrast = createContrast("default", "light");

  const memoizedValue = useMemo(
    () => ({
      palette: {
        ...palette("light"),
        ...presets.palette,
        ...contrast.palette,
      },
      customShadows: {
        ...customShadows("light"),
        ...presets.customShadows,
      },
      direction: "ltr",
      shadows: shadows("light"),
      shape: { borderRadius: 8 },
      typography,
    }),
    [presets.palette, presets.customShadows, contrast.palette]
  );

  const theme = createTheme(memoizedValue as ThemeOptions);

  theme.components = merge(componentsOverrides(theme), contrast.components);

  const themeWithLocale = useMemo(() => createTheme(theme), [theme]);

  return (
    <MuiThemeProvider theme={themeWithLocale}>
      <RTL themeDirection="ltr">
        <CssBaseline />
        {children}
      </RTL>
    </MuiThemeProvider>
  );
}
