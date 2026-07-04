# API Reference

All public functions and globals provided by the ClientAutoloader (`client/init.cs`). Everything lives in the `LiFx` package, which is active for the whole client session.

## Callback system

### `LiFx::registerCallback(%hookArray, %function, %object)`

Registers a callback on a hook. See the [Hooks Reference](hooks.md).

| Parameter | Description |
|---|---|
| `%hookArray` | The hook to attach to, e.g. `$LiFx::hooks::onInitClientDone`. |
| `%function` | Name of the function (or object method) to call. |
| `%object` | *(optional)* A `ScriptObject`; when given, the callback is invoked as `%object.call(%function, ...)`, so the method receives `%this` first. |

### `LiFx::executeCallback(%hookArray, %arg1, ..., %arg5)`

Invokes every callback registered on a hook with up to five arguments. The framework calls this at the appropriate times; mods can define and fire their own hooks the same way:

```torquescript
// Define a custom hook (once, at file scope):
$MyMod::hooks::onThemeChanged = JettisonArray("onThemeChanged");

// Fire it:
LiFx::executeCallback($MyMod::hooks::onThemeChanged, %newTheme);
```

### `LiFx::registerExecCallback(%hookArray, %object)`

Registers an **object** (not a function) on a hook. Used with the script-loading hooks `onMaterialsLoaded` and `onDatablockLoaded`, where each registered object must expose a `path()` method — see the [Hooks Reference](hooks.md#script-loading-hooks).

## Mod loading

Run automatically at startup; documented for understanding and advanced use.

| Function | Description |
|---|---|
| `LiFx::loadModsRecursivelyInFolder(%rootFolder)` | Executes every `cmod.cs` one directory deep under `%rootFolder`. |
| `LiFx::loadZipModsRecursivelyInFolder(%rootFolder)` | Finds zip archives under `%rootFolder`'s `mods/` and `modpack/` subfolders and executes the `cmod.cs` inside each. |
| `LiFx::loadRecursivelyInFolder(%rootFolder, %pattern)` | Generic loader: executes every file matching `%rootFolder/*/%pattern`. Prefers compiled `.dso` files when the matching `.cs` source is absent. |
| `LiFx::loadZipRecursivelyInFolder(%rootFolder, %pattern)` | Collects matching zip archives into `$LiFx::zipFiles` (deduplicated) for later execution. |
| `LiFx::getLeafFolder(%filePath)` | Utility: returns the containing folder portion of a file path. |

## Web browser windows

### `LiFx::LiFxWeb(%url, %text)`

Opens `%url` in the in-game browser window (movable, saveable, with back/forward navigation), sized relative to the play GUI. Available to mods for help pages, live maps, etc.

The framework binds **F2** to open `$LiFx::kb` (the LiFx knowledge base) through this facility — see [Built-in Features](features.md).

## Utility functions

| Function | Description |
|---|---|
| `LiFx::debugEcho(%message)` | Echoes `%message` to the console only when `$LiFx::debug` is enabled. |
| `LiFx::init()` | Framework bootstrap: creates the `mods/` folders, starts the version overlay and registers the F2 knowledge-base bind. Called automatically at load. |
| `LiFx::autojoin()` | Polls for the main menu; once up, fires `onInitialized` and executes `$LiFx::autojoin` if the file exists. |
| `LiFxSHA256::hashMain(%file)` | Returns the SHA-256 hash of a file's contents (from the bundled `sha256` module). |

## Peer commands (server → client)

The framework answers these commands sent by a LiFx server, enabling modpack integrity verification — see [Built-in Features](features.md):

| Command handler | Behaviour |
|---|---|
| `peerCmdLiFxCRC(%server, %file)` | Replies with the CRC of `%file` via a `LiFxCRC` command. |
| `peerCmdLiFxSHA256(%server, %file)` | Replies with the SHA-256 of `%file` via a `LiFxSHA256` command. |
| `peerCmdDisableChat(%server, %value)` | Lets the server suppress automatic joining of default chat channels. |

## JSON

`jettisonParse`, `jettisonStringify`, `JettisonObject`, `JettisonArray` and file helpers are documented in [Jettison JSON](jettison-json.md). Hook arrays themselves are `JettisonArray` objects — that is why they expose `.Length`, `.value[%i]` and `.push()`.

## Global variables

| Global | Default | Meaning |
|---|---|---|
| `$LiFx::Version` | `"v3.0.0"` | Framework version string (shown on the main menu). |
| `$LiFx::debug` | `1` | Enables verbose `LiFx::debugEcho` output. |
| `$LiFx::autojoin` | `"yolauncher/autojoin.cs"` | Script executed once the main menu is up, if the file exists. |
| `$LiFx::kb` | `"https://kb.lifxmod.com/"` | URL opened by the F2 knowledge-base bind. |
| `$LiFx::zipFiles` | — | `JettisonArray` of discovered zip mods. |
| `$LiFx::hooks::*` | — | The hook arrays ([Hooks Reference](hooks.md)). |
