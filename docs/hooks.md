# Hooks Reference

Each hook is a named array of callbacks (`$LiFx::hooks::<name>`) that the framework fires at a specific point during client startup.

## Registering a callback

```torquescript
// As an object method (recommended — see Getting Started):
LiFx::registerCallback($LiFx::hooks::onInitClientDone, "onReady", ExampleClientMod);

// As a global function:
LiFx::registerCallback($LiFx::hooks::onInitClientDone, "myGlobalFunction");
```

Object-method callbacks receive `%this` (the object) as their first parameter, followed by the hook's arguments. Callbacks may receive up to five arguments.

## Client lifecycle hooks

In firing order during client start:

| Hook | Fires | Arguments |
|---|---|---|
| `mods` | After every `cmod.cs` has been executed, before any client initialization. This is your `setup` function. | — |
| `onMaterialsLoad` | After the engine loads materials (and after registered material paths execute). Use it to add or override materials. | `%this` |
| `onDatablockLoad` | After the engine executes datablocks (and after registered datablock paths execute). Use it to add or override datablocks. | `%this` |
| `beforeInitClientDone` | Near the end of client init, right before the engine's `onInitClientDone()`. GUIs, keybinds and managers exist at this point. | `%this` |
| `onInitClientDone` | After the engine's `onInitClientDone()`. The client is fully initialized. | `%this` |
| `onInitialized` | When the main menu (multiplayer window) is up. Fires from the autojoin loop; the framework itself uses it to bind the F2 knowledge-base key. | — |

## Script-loading hooks

Two hooks work differently: instead of functions, you register **objects that expose a `path()` method**, using `LiFx::registerExecCallback`. When materials/datablocks load, the framework resolves each registered object's path and executes the matching scripts from that mod folder:

| Hook | Loads |
|---|---|
| `onMaterialsLoaded` | `materials.cs` files one directory deep under the registered path's folder |
| `onDatablockLoaded` | datablock scripts one directory deep under the registered path's folder |

```torquescript
if (!isObject(ExampleAssets))
{
    new ScriptObject(ExampleAssets) {};
}

function ExampleAssets::path(%this)
{
    return "mods/ExampleClientMod/assets/";
}

// At file scope or in setup():
LiFx::registerExecCallback($LiFx::hooks::onMaterialsLoaded, ExampleAssets);
```

This lets a mod ship its own material and datablock scripts and have them loaded at exactly the right point in engine initialization, alongside the vanilla ones.

## Example: putting it together

```torquescript
function ExampleClientMod::setup(%this)
{
    LiFx::registerCallback($LiFx::hooks::onMaterialsLoad,   "onMaterials", ExampleClientMod);
    LiFx::registerCallback($LiFx::hooks::beforeInitClientDone, "onGuiInit", ExampleClientMod);
    LiFx::registerCallback($LiFx::hooks::onInitClientDone,  "onReady",     ExampleClientMod);
}

function ExampleClientMod::onMaterials(%this)
{
    // Materials are loaded — safe to tweak or add materials here.
}

function ExampleClientMod::onGuiInit(%this)
{
    // GUIs and keybinds exist — add custom GUI elements or binds here.
}

function ExampleClientMod::onReady(%this)
{
    echo("Client fully initialized.");
}
```
