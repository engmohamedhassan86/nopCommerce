# Nop.Plugin.Widgets.MaroonTheme

A nopCommerce 5.00 storefront widget plugin that recolors the public-facing primary
UI color (header/nav hover states, buttons, price highlights, footer accents, and
form focus states) to maroon, without editing any core nopCommerce files or
DefaultClean theme files.

## How it works

The plugin registers a widget in the `head_html_tag` public widget zone. On every
storefront page it injects a small `<style>` block built from the configured
colors, which overrides the handful of CSS custom properties and concrete
selectors DefaultClean uses for its accent color (`--accent-blue-color` /
`--accent-blue-active-color`, `.btn-primary`, header link hover states, product
price highlights, footer link hover states, and `.form-control:focus`). Because
the widget zone renders after the theme's own stylesheet in `<head>`, the override
wins on normal CSS cascade order — no `!important` or theme edits required. The
Admin area never renders this widget zone, so Admin styling is untouched
automatically.

The generated style block is cached per store (`IStaticCacheManager`) and the
cache is invalidated automatically whenever plugin settings are saved.

## Install

1. Build the solution (or just this plugin project) and copy/publish the plugin
   output to `Presentation/Nop.Web/Plugins/Widgets.MaroonTheme` if it isn't already
   there as part of a normal build.
2. Start the site and sign in to the Admin area.
3. Go to **Admin > Configuration > Local plugins**.
4. Find **Maroon Theme Color** in the list and click **Install**.
5. Click **Edit** (or **Configure**) to open its settings page.

## Configure

On the configuration page you can set, per store (use the store-scope selector
at the top of the page and the "override for store" checkboxes next to each
field to set store-specific values):

- **Enabled** — turns the storefront color override on or off.
- **Primary color** — the main accent color (default `#800000`), used for
  header/nav hover states, buttons, price highlights, and footer accents.
- **Hover color** — the accent color used for hover/active states (default
  `#5c0000`).

Click **Save**. The storefront picks up the new colors on the next page
load/refresh — no restart required, because the style block cache is cleared on
save.

## Disable / Uninstall

- **Disable** (from the Local plugins list, or by unchecking **Enabled** on the
  configuration page and saving) stops the style block from being rendered; the
  storefront immediately reverts to the stock DefaultClean colors.
- **Uninstall** removes the plugin's settings and locale resources entirely.
  Since the plugin never edits DefaultClean's own CSS files, there is nothing
  else to restore — the storefront is back to stock colors with no residue.

## Expected visual result

With the plugin enabled and the default colors, the storefront's blue accents
(header link hover, `.button-1`/`.button-2` buttons, the shopping-cart/menu
toggle, product "Add to cart" buttons, the pager, footer links, and form focus
rings) render in maroon (`#800000`) with a darker maroon (`#5c0000`) on
hover/focus. This is visible on the home, category, product, cart, checkout,
and login pages. The Admin area is unaffected because it never renders the
`head_html_tag` widget zone that this plugin hooks into.

> This environment has no way to launch a browser against a running
> nopCommerce site, so before/after screenshots could not be captured here. The
> description above describes the expected visual result; a reviewer with a
> running storefront can confirm it visually by installing the plugin and
> comparing the pages listed above with it enabled vs. disabled.
