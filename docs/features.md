# Built-in Features

Beyond mod loading and hooks, the ClientAutoloader ships several features that work out of the box.

## Autojoin

Once the main menu is up, the framework executes the script at `$LiFx::autojoin` (default: `yolauncher/autojoin.cs`) if it exists. A launcher can drop a script there to connect the player straight to a server — no manual server-list navigation.

The same mechanism fires the `onInitialized` hook, so mods can also react to "the main menu is ready".

## F2 — in-game knowledge base

The framework binds **F2** to open the LiFx knowledge base (`$LiFx::kb`, default `https://kb.lifxmod.com/`) in the game's built-in browser window. The window is movable, resizable relative to the play GUI, and supports back/forward navigation. Only one instance opens at a time.

Mods can open their own browser windows with the same facility:

```torquescript
LiFx::LiFxWeb("https://example.com/my-mod-help", "My Mod Help");
```

## Main-menu version overlay

On *Your Own* clients, the framework overlays its version (`$LiFx::Version`) on the main menu's multiplayer window. Players (and support) can tell at a glance whether the autoloader is installed and which version is running.

## File verification (CRC / SHA-256)

LiFx servers can verify the integrity of client files — for example to check that a required modpack is installed and untampered. The server sends a peer command naming a file; the client hashes it and replies:

- `peerCmdLiFxCRC` → replies with the engine CRC of the file.
- `peerCmdLiFxSHA256` → replies with a SHA-256 hash (computed by the bundled pure-TorqueScript implementation).

No configuration is needed on the client; server-side tooling decides what to check and how to react.

## Server-controlled chat defaults

A server can send `peerCmdDisableChat` to stop the client from auto-joining the default chat channels, letting server mods manage chat channel membership themselves.

## Mods folder bootstrap

On startup the framework creates `mods/` (and `mods/LiFx/`) in the client root if missing, so there is always a place to install mods manually.
