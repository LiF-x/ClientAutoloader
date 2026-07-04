# ClientAutoloader

The **LiFx ClientAutoloader** is the client-side mod framework for *Life is Feudal: Your Own*. It is the counterpart of the [ServerAutoloader](https://github.com/LiF-x/ServerAutoloader): where the server framework loads gameplay mods, the ClientAutoloader loads **client mods** — custom GUIs, materials, datablocks, keybinds and quality-of-life features — automatically at game start.

## Features

- **Automatic mod loading** — drop a folder with a `cmod.cs` into `mods/` (or ship it via YoLauncher, plain or zipped) and it loads when the client starts.
- **Hook system** — callbacks around client initialization, materials loading and datablock loading, so mods extend the client without overwriting game scripts.
- **Zip mod support** — mods can ship as zip archives in `yolauncher/mods/` or `yolauncher/modpack/`.
- **Autojoin** — executes `yolauncher/autojoin.cs` once the main menu is up, enabling launcher-driven direct connect.
- **In-game knowledge base** — press **F2** to open the [LiFx knowledge base](https://kb.lifxmod.com/) in the in-game browser; mods can open their own web windows.
- **Version overlay** — shows the framework version on the main menu.
- **File verification** — answers server-issued CRC/SHA-256 hash requests so servers can verify client modpack integrity.
- **Bundled JSON library** — [Jettison](docs/jettison-json.md) for config files and structured data.

## Documentation

**[📚 Full documentation](docs/README.md)**

| Page | What it covers |
|---|---|
| [Getting Started](docs/getting-started.md) | Client mod requirements, load locations, full mod template |
| [Hooks Reference](docs/hooks.md) | Every client callback hook, with parameters and timing |
| [API Reference](docs/api-reference.md) | All `LiFx::` client functions and globals |
| [Built-in Features](docs/features.md) | Autojoin, F2 knowledge base, version overlay, file verification |
| [Jettison JSON](docs/jettison-json.md) | The bundled JSON library |

## Quick example

```torquescript
// mods/ExampleClientMod/cmod.cs
if (!isObject(ExampleClientMod))
{
    new ScriptObject(ExampleClientMod) {};
}

package ExampleClientMod
{
    function ExampleClientMod::setup(%this)
    {
        LiFx::registerCallback($LiFx::hooks::onInitClientDone, "onReady", ExampleClientMod);
    }

    function ExampleClientMod::onReady(%this)
    {
        echo("ExampleClientMod is ready!");
    }
};

activatePackage(ExampleClientMod);
LiFx::registerCallback($LiFx::hooks::mods, "setup", ExampleClientMod);
```

See [Getting Started](docs/getting-started.md) for the full walkthrough.

## Repository layout

| Path | Purpose |
|---|---|
| `client/init.cs` | The framework: mod loader, hook system, built-in features |
| `client/jettison.cs` | JSON parser/serializer |
| `client/sha256.cs.dso` | SHA-256 implementation (compiled) for file verification |
| `client/lifx.png` | LiFx logo |
| `scripts.bat` | Packs the release `scripts.zip` |

## Related projects

- [ServerAutoloader](https://github.com/LiF-x/ServerAutoloader) — the server-side counterpart
- [lifxmod.com](https://lifxmod.com) — project website

## License

[GNU General Public License v3](LICENSE). Publicly distributed mods built on this framework must comply with the license's source-sharing requirements.
