# ClientAutoloader Documentation

Welcome to the LiFx **ClientAutoloader** documentation. The ClientAutoloader is the client-side mod framework for *Life is Feudal: Your Own* — it discovers and loads client mods, exposes hooks around client initialization, and ships quality-of-life features like autojoin and an in-game knowledge base.

## Where to start

1. **[Getting Started](getting-started.md)** — what a client mod looks like, where mods load from, and a complete working template.
2. **[Hooks Reference](hooks.md)** — every callback hook the framework fires, with parameters and timing.
3. **[Built-in Features](features.md)** — the features the framework provides out of the box.

## All pages

| Page | What it covers |
|---|---|
| [Getting Started](getting-started.md) | Client mod requirements, all load locations (folders, YoLauncher, zips), full mod template |
| [Hooks Reference](hooks.md) | All client `$LiFx::hooks::*` callbacks: when they fire and what arguments they receive |
| [API Reference](api-reference.md) | Every public `LiFx::` client function and global variable |
| [Built-in Features](features.md) | Autojoin, F2 knowledge base browser, version overlay, CRC/SHA-256 file verification |
| [Jettison JSON](jettison-json.md) | The bundled JSON parse/serialize library |

## Related resources

- [LiFx website & docs](https://lifxmod.com)
- [LiFx knowledge base](https://kb.lifxmod.com/)
- [ServerAutoloader](https://github.com/LiF-x/ServerAutoloader) — the server-side counterpart, including its own docs on hooks, custom objects and recipes

## License

The ClientAutoloader is licensed under the **GNU General Public License v3**. Mods that build on it and are distributed publicly must comply with the license's source-sharing requirements.
