# AGENTS.md - DungeonHeroClicker

Guidelines for AI coding agents working in this Unity project.

## Project Overview

| Property | Value |
|----------|-------|
| Engine | Unity 6000.0.60f1 (Unity 6 LTS) |
| Language | C# 9.0 |
| Framework | .NET Framework 4.7.1 |
| Rendering | URP (Universal Render Pipeline) |
| Platform | Android (Portrait, 1080x2280) |

## Build & Test Commands

### Unity Editor Commands (via Unity Hub or command line)
```bash
# Open project in Unity Editor
/Applications/Unity/Hub/Editor/6000.0.60f1/Unity.app/Contents/MacOS/Unity -projectPath .

# Build Android APK (headless)
Unity -batchmode -projectPath . -executeMethod BuildScript.BuildAndroid -quit

# Run all tests (Edit Mode + Play Mode)
Unity -batchmode -projectPath . -runTests -testResults ./TestResults.xml -quit

# Run single test class
Unity -batchmode -projectPath . -runTests -testFilter "ClassName" -quit

# Run single test method
Unity -batchmode -projectPath . -runTests -testFilter "ClassName.MethodName" -quit
```

### IDE Integration
- **Rider**: Open `.sln` file, use Unity integration for running tests
- **VS Code**: Use C# Dev Kit + Unity extensions

## Code Style Guidelines

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Classes, Structs, Enums | PascalCase | `ClickManager`, `DamageType` |
| Interfaces | `I` prefix + PascalCase | `IDamageable`, `IClickable` |
| Private instance fields | `_` prefix + camelCase | `_clickDamage`, `_health` |
| Static fields | `s_` prefix + camelCase | `s_instance` |
| Thread static fields | `t_` prefix + camelCase | `t_timeSpan` |
| Constants | PascalCase | `MaxHealth`, `DefaultDamage` |
| Methods, Properties | PascalCase | `TakeDamage()`, `CurrentHealth` |
| Parameters, Locals | camelCase | `clickInfo`, `targetMonster` |

### Formatting Rules

- **Indentation**: 4 spaces (no tabs)
- **Braces**: Allman style (opening brace on new line)
- **One statement per line**
- **One declaration per line**

```csharp
// Correct - Allman style
public void TakeDamage(int amount)
{
    if (amount > 0)
    {
        _health -= amount;
    }
}

// Wrong - K&R style
public void TakeDamage(int amount) {
    if (amount > 0) {
        _health -= amount;
    }
}
```

### Type Usage

- Use language keywords: `int`, `string`, `bool` (not `Int32`, `String`, `Boolean`)
- Use `var` only when type is obvious from the right side
- Prefer `string.Empty` over `""`

```csharp
// Good
var manager = new ClickManager();  // Type is obvious
int damage = CalculateDamage();    // Type not obvious, be explicit

// Bad
var damage = CalculateDamage();    // Type unclear
System.Int32 count = 0;            // Use language keyword
```

### Error Handling

- Catch specific exceptions only, never bare `catch (Exception)`
- Use `try-finally` or `using` for resource cleanup
- Validate parameters at method entry

### Comments

- Use `//` for comments, on separate line
- Start with capital letter, end with period
- **Do NOT write XML doc comments** - method/parameter names should be self-documenting

```csharp
// Calculate total damage including buffs.
private int CalculateTotalDamage()
{
    // ...
}
```

## Unity-Specific Guidelines

### MonoBehaviour Structure

```csharp
public class ExampleScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speed = 5f;
    
    [Header("Dependencies")]
    [SerializeField] private SpriteRenderer _renderer;
    
    private Transform _cachedTransform;
    
    private void Awake()
    {
        _cachedTransform = transform;
    }
    
    private void Start()
    {
        // Initialization after all Awake calls.
    }
    
    private void Update()
    {
        // Per-frame logic.
    }
}
```

### Critical Rules

1. **Cache components** - Never call `GetComponent<T>()` in Update/FixedUpdate
2. **Use object pooling** - For frequently spawned objects (popups, effects)
3. **Never modify .meta files** - Unity manages these
4. **Match filename to class name** - `Monster.cs` must contain `class Monster`
5. **Use SerializeField** - Prefer over public fields for inspector values

### ScriptableObject Pattern

```csharp
[CreateAssetMenu(fileName = "NewData", menuName = "DungeonHeroClicker/DataName")]
public class ExampleData : ScriptableObject
{
    [Header("Info")]
    [SerializeField] private string _name;
    
    public string Name => _name;
}
```

## Design Principles

### SOLID Principles (Enforced)

| Principle | Key Rule |
|-----------|----------|
| **SRP** | One class = one reason to change |
| **OCP** | Extend via inheritance/composition, don't modify existing code |
| **LSP** | Subtypes must be substitutable for base types |
| **ISP** | Small, focused interfaces (1-3 methods) |
| **DIP** | Depend on abstractions, not concretions |

### Law of Demeter

Avoid chained calls ("train wrecks"):
```csharp
// Bad
_player.GetInventory().GetWeapon().GetDamage();

// Good
_player.GetWeaponDamage();
```

### Interface-First Design

Define interfaces before implementations. Key interfaces in this project:
- `IDamageable` - Can receive damage
- `IClickable` - Can be clicked
- `IPoolable` - Object pooling target
- `ICurrencyProvider` - Provides currency

## Project Structure

```
Assets/
├── 00.Scenes/           # Scene files
├── 01.Scripts/          # C# source code
│   ├── Core/            # Managers, utilities
│   ├── Ingame/          # Gameplay (Click, Monster, Hero, Stage)
│   ├── Interfaces/      # Interface definitions
│   ├── Outgame/         # Meta systems (Currency, Upgrade, Save)
│   └── UI/              # UI components
├── 02.Prefabs/          # Prefab assets
├── 08.ScriptableObjects/# Data assets
└── Plugins/             # External (DOTween, LeanPool)
```

## External Dependencies

| Package | Purpose |
|---------|---------|
| DOTween | Animation tweening (`DOMove`, `DOFade`, `DOScale`) |
| LeanPool | Object pooling system |
| TextMesh Pro | Text rendering |
| Input System | New Unity input handling |

## Git Workflow

- `main` - Stable releases
- `develop` - Development integration
- `feature/*` - Feature branches
- `test/*` - Experiments

## Quick Reference

- **BigNumber**: Custom struct for large number handling (mantissa x 10^exponent)
- **Event pattern**: Use `event Action` with subscribe in `OnEnable`, unsubscribe in `OnDisable`
- **Header attribute**: Use `[Header("Section")]` to organize inspector
- **Avoid GC allocations**: No `new` in Update loops, use object pooling
