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

## Screenshots

Real before/after screenshots (plugin disabled = stock DefaultClean colors vs.
plugin enabled with the default `#800000`/`#5c0000` colors), captured with
headless Chromium (Playwright) against a live install of this plugin on sample
data, at both a mobile (375px) and desktop (1280px) viewport width. Files are
in [`screenshots/`](screenshots/):

| Page | 375px disabled | 375px enabled | 1280px disabled | 1280px enabled |
|---|---|---|---|---|
| Home | [screenshots/home_375w_disabled.png](screenshots/home_375w_disabled.png) | [screenshots/home_375w_enabled.png](screenshots/home_375w_enabled.png) | [screenshots/home_1280w_disabled.png](screenshots/home_1280w_disabled.png) | [screenshots/home_1280w_enabled.png](screenshots/home_1280w_enabled.png) |
| Category (`/computers`) | [screenshots/category_375w_disabled.png](screenshots/category_375w_disabled.png) | [screenshots/category_375w_enabled.png](screenshots/category_375w_enabled.png) | [screenshots/category_1280w_disabled.png](screenshots/category_1280w_disabled.png) | [screenshots/category_1280w_enabled.png](screenshots/category_1280w_enabled.png) |
| Product (`/apple-macbook-pro`) | [screenshots/product_375w_disabled.png](screenshots/product_375w_disabled.png) | [screenshots/product_375w_enabled.png](screenshots/product_375w_enabled.png) | [screenshots/product_1280w_disabled.png](screenshots/product_1280w_disabled.png) | [screenshots/product_1280w_enabled.png](screenshots/product_1280w_enabled.png) |
| Cart | [screenshots/cart_375w_disabled.png](screenshots/cart_375w_disabled.png) | [screenshots/cart_375w_enabled.png](screenshots/cart_375w_enabled.png) | [screenshots/cart_1280w_disabled.png](screenshots/cart_1280w_disabled.png) | [screenshots/cart_1280w_enabled.png](screenshots/cart_1280w_enabled.png) |
| Checkout | [screenshots/checkout_375w_disabled.png](screenshots/checkout_375w_disabled.png) | [screenshots/checkout_375w_enabled.png](screenshots/checkout_375w_enabled.png) | [screenshots/checkout_1280w_disabled.png](screenshots/checkout_1280w_disabled.png) | [screenshots/checkout_1280w_enabled.png](screenshots/checkout_1280w_enabled.png) |
| Login | [screenshots/login_375w_disabled.png](screenshots/login_375w_disabled.png) | [screenshots/login_375w_enabled.png](screenshots/login_375w_enabled.png) | [screenshots/login_1280w_disabled.png](screenshots/login_1280w_disabled.png) | [screenshots/login_1280w_enabled.png](screenshots/login_1280w_enabled.png) |

Additional supporting screenshots:

- [screenshots/checkout-focus_1280w_disabled.png](screenshots/checkout-focus_1280w_disabled.png) /
  [screenshots/checkout-focus_1280w_enabled.png](screenshots/checkout-focus_1280w_enabled.png) — a
  focused checkout "First name" field, close up, showing the form focus
  border/glow go from the theme's default gray to maroon.
- [screenshots/admin-dashboard_1280w.png](screenshots/admin-dashboard_1280w.png),
  [screenshots/admin-configure_1280w_before-toggle.png](screenshots/admin-configure_1280w_before-toggle.png),
  [screenshots/admin-configure_1280w_after-toggle-disabled.png](screenshots/admin-configure_1280w_after-toggle-disabled.png),
  [screenshots/admin-configure_1280w_after-toggle-enabled.png](screenshots/admin-configure_1280w_after-toggle-enabled.png)
  — the Admin dashboard and this plugin's own configuration page, confirming
  the Admin area keeps its stock blue/teal styling regardless of this plugin's
  state (it never renders the `head_html_tag` public widget zone).

At both widths, the storefront layout holds up with no overlapping or clipped
elements in either plugin state. Confirmed visually in the screenshots above:
header/menu hover, `ADD TO CART`/`CHECKOUT`/`CONTINUE` buttons, price
highlights, mobile footer accordion headers, and checkout form focus borders
all render maroon when enabled, and stock nopCommerce blue when disabled.

During this visual pass, a real bug was found and fixed: the plugin's form
focus-border CSS originally targeted `.form-control:focus`, a Bootstrap/Admin
class that DefaultClean's storefront inputs don't use (its own `styles.css`
hard-codes bare `input[type="text"]:focus` etc. selectors instead), so the
maroon focus border never actually applied on the storefront. The style block
now also targets those bare input/textarea/select focus selectors; this is
confirmed working in the `checkout-focus` screenshots above.
