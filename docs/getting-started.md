# Getting Started

This page walks you through building a client mod for *Life is Feudal: Your Own* with the LiFx ClientAutoloader.

## What the framework does

The ClientAutoloader wraps the game client's `initClient()` entry point. Before the vanilla client initializes, it:

1. Scans several locations for mods (see below) and executes every `cmod.cs` it finds.
2. Runs each mod's registered `setup` function.
3. Proceeds with normal client initialization, firing **hooks** at well-defined points (materials loaded, datablocks loaded, client ready, …) that your mod can attach to.

This means client mods — custom GUIs, keybinds, HUD elements, materials, datablocks — install by dropping a folder (or zip) in place, with no game files edited.

## Where mods load from

At client start, mods are loaded from these locations, in order:

| Location | Format | Typical use |
|---|---|---|
| `yolauncher/mods/*/cmod.cs` | folder | Mods installed by YoLauncher |
| `yolauncher/mods/*/*.zip` and `yolauncher/modpack/*/*.zip` | zip archive containing a `cmod.cs` | Zipped mods / server modpacks distributed via launcher |
| `yolauncher/modpack/*/cmod.cs` | folder | Server modpack contents |
| `mods/*/cmod.cs` | folder | Manually installed mods |

All paths are relative to the game client root. In every case the entry file **must be named `cmod.cs`** and sit one directory deep inside the location (compiled `cmod.cs.dso` is also supported). Zip archives are mounted by the engine, so a `cmod.cs` inside a zip loads exactly like one in a folder.

## Requirements for a valid client mod

1. **GPL v3 compliance** — the framework is GPL v3; publicly distributed mods must comply with the license's sharing requirements.
2. **A unique package** — your mod's code lives in a TorqueScript `package` with a name no other mod uses.
3. **A `cmod.cs` entry point** — the framework only discovers files named `cmod.cs`.
4. **A `setup` function registered on the `mods` hook** — unlike the ServerAutoloader, the client framework does not auto-register your setup; register it explicitly at the end of your `cmod.cs`.

## A complete mod template

```torquescript
// mods/ExampleClientMod/cmod.cs

// The ScriptObject should have the same name as the package.
if (!isObject(ExampleClientMod))
{
    new ScriptObject(ExampleClientMod) {};
}

package ExampleClientMod
{
    // Called once all mods are loaded, before the client initializes.
    // Register your hooks here.
    function ExampleClientMod::setup(%this)
    {
        LiFx::registerCallback($LiFx::hooks::onInitClientDone, "onReady", ExampleClientMod);
    }

    // Hook handlers registered with an object receive %this first,
    // then the hook's own arguments.
    function ExampleClientMod::onReady(%this)
    {
        echo("ExampleClientMod loaded — client is ready.");
    }
};

activatePackage(ExampleClientMod);

// Required: register your setup on the mods hook.
LiFx::registerCallback($LiFx::hooks::mods, "setup", ExampleClientMod);
```

Drop the folder into `mods/`, start the client, and your mod runs.

## The load sequence

1. **Client starts** — the framework's `initClient()` takes over.
2. **Mods are discovered and executed** from all locations listed above. Your file-scope code runs now.
3. **All `setup` functions run** (the `mods` hook).
4. **Vanilla client init begins** — config, GUI profiles, base client.
5. **Materials load** — registered material paths execute, then `onMaterialsLoad` fires.
6. **Datablocks load** — registered datablock paths execute, then `onDatablockLoad` fires.
7. **Skills, titles, GUIs, keybinds and managers initialize.**
8. **`beforeInitClientDone`** fires, then the engine's `onInitClientDone()`, then **`onInitClientDone`**.
9. **The main menu appears** — autojoin logic runs and `onInitialized` fires; `yolauncher/autojoin.cs` is executed if present.

See the [Hooks Reference](hooks.md) for details on each hook.

## Debugging

The client framework ships with `$LiFx::debug = 1`, so `LiFx::debugEcho()` output (including which mod files are found and executed) appears in the console log. Use `LiFx::debugEcho("message");` in your own code for output that respects the same switch.

## Pairing with a server mod

Client mods often accompany a server mod (custom items need client-side data and icons; custom mechanics need server logic). See the [ServerAutoloader documentation](https://github.com/LiF-x/ServerAutoloader/tree/main/docs) — in particular its data-export page, which generates the `data/*.xml` files a client modpack needs for custom objects and recipes.
