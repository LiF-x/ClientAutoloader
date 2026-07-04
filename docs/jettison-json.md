# Jettison — JSON for TorqueScript

The autoloader bundles **Jettison** (`client/jettison.cs`), a pure-TorqueScript JSON parser and serializer. The framework itself uses its `JettisonArray` type for all hook arrays; mods can use the full library for config files, data exchange, and structured storage.

## JSON types

Jettison maps JSON to TorqueScript as follows:

| JSON type name | TorqueScript representation |
|---|---|
| `"string"` | UTF-8 string |
| `"number"` | numeric string |
| `"boolean"` | `1` (true) or `0` (false) |
| `"null"` | empty string |
| `"object"` | a `ScriptObject` — `JettisonObject` for `{}`, `JettisonArray` for `[]` |

## Parsing

### `jettisonParse(%text)`

Parses a JSON string. Returns **false on success** (sets `$JSON::Value` and `$JSON::Type`) and **true on failure** (sets `$JSON::Error` and `$JSON::Index`).

```torquescript
%text = "{\"name\": \"Example\", \"level\": 3, \"tags\": [\"a\", \"b\"]}";

if (jettisonParse(%text))
{
    error("JSON error (at " @ $JSON::Index @ "): " @ $JSON::Error);
    return;
}

%data = $JSON::Value;          // a JettisonObject
echo(%data.name);              // "Example"
echo(%data.level);             // 3
echo(%data.tags.length);       // 2
echo(%data.tags.value[0]);     // "a"

%data.delete();                // you own parsed objects — clean them up
```

### `jettisonReadFile(%filename)`

Reads and parses a whole file. Same return/global convention as `jettisonParse`.

## Serializing

### `jettisonStringify(%type, %value)`

Serializes a value to a JSON string. `%type` is one of the JSON type names above; for `"object"`, the value must implement `::toJSON()` (both Jettison types do).

```torquescript
%obj = JettisonObject();
%obj.set("name", "string", "Example");
%obj.set("level", "number", 3);

%array = JettisonArray();
%array.push("string", "a");
%array.push("string", "b");
%obj.set("tags", "object", %array);

echo(jettisonStringify("object", %obj));
// {"name":"Example","level":3,"tags":["a","b"]}

%obj.delete(); // also deletes nested Jettison values
```

### `jettisonWriteFile(%filename, %type, %value)`

Serializes and writes to a file. Returns an empty string on success, an error message on failure.

## `JettisonObject`

Created by `JettisonObject()` or by parsing a JSON `{}`.

| Member | Description |
|---|---|
| `.set(%key, %type, %value)` | Set a key. Also exposes the key as a field (`%obj.myKey`) when the name is safe to do so. |
| `.remove(%key)` | Remove a key; returns true if it existed. |
| `.keyCount` | Number of keys. |
| `.keyName[%i]` | Key name at index `%i`. |
| `.type[%key]` / `.value[%key]` | Type and value stored for a key. |
| `.getField(%name)` | Read a key through the field-access helper (works for any key). |
| `.toJSON()` | Serialize to a JSON string. |

Keys named after internal members (`class`, `keyCount`, `type…`, `value…`, `keyName…`, …) are still stored and serialized correctly but are not mirrored as direct fields — use `.value[%key]` or `.getField()` for those.

## `JettisonArray`

Created by `JettisonArray()` / `JettisonArray(%name)` or by parsing a JSON `[]`.

| Member | Description |
|---|---|
| `.push(%type, %value)` | Append an element. |
| `.length` | Number of elements (the framework's hook arrays are also readable via `.Length`). |
| `.type[%i]` / `.value[%i]` | Type and value at index `%i`. |
| `.toJSON()` | Serialize to a JSON string. |

## Memory management

Parsed and constructed Jettison objects are `ScriptObject`s — **you must `.delete()` them when done**. Deleting a `JettisonObject`/`JettisonArray` recursively deletes any nested Jettison values it owns.

## Known limitations

- Unicode escape sequences (`\uXXXX`) in strings are not implemented and produce a parse error.
- Number parsing is lenient: anything matching `[0-9.eE+-]+` is accepted verbatim rather than validated against the JSON grammar.
- Number serialization emits the value as-is without normalization.
